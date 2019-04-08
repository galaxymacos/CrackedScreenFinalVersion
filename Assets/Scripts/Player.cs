using System.Collections;
using System.Globalization;
using TMPro;
using UnityEditor.U2D;
using UnityEngine;

public enum Position {
    Left, Right, Up, Down
}

public class Player : MonoBehaviour {
    private static readonly int knockOff = Animator.StringToHash("Knock Off");
    private PlayerController _playerController;

    private PlayerMovement _playerMovement;

    [SerializeField] private float defendRecoilForce = 10f;
    [SerializeField] private float defendRecoilTime = 0.5f;
    private Position defendRecoilDirection;
    
    private bool recoilDirection;
    internal float defendRecoilTimeRemain = 0;

    public GameObject floatingText;
    public float hp = 200;

    [SerializeField] private float invincibieTime = 2f;
    internal float invincibleTimeRemains;

    public Vector3 knockUpForce = new Vector3(200, 200, 0);
    private float lastTimeKnockOff;
    private float lastTimeTakeDamage;
    public float maxHp = 200f;
    public float maxRage = 200f;
    public float rage;

    private Rigidbody rb;
    private GameObject DimensionLeapParticleEffect;
    
    


    private void Start()
    {
        GameManager.Instance.onPlayerDieCallback += PlayerInvincibleWhenRespawn;
        bloodPlace = transform.Find("BloodPlace");
        _skinnedMeshRenderers = sprites.GetComponentsInChildren<SkinnedMeshRenderer>();
//        GameManager.Instance.gameObjects.Add(gameObject);

        rb = GetComponent<Rigidbody>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerController = GetComponent<PlayerController>();
        hp = maxHp;

        GameManager.Instance.onPlayerDieCallback += RestorePlayerHealth;
        DimensionLeapParticleEffect = transform.Find("DimensionLeapParticleEffect").gameObject;
    }

    private void Update() {
        if (defendRecoilTimeRemain > 0) {
            PlayerProperty.animator.SetBool("DefendRecoiling", true);
        }
        else {
            PlayerProperty.animator.SetBool("DefendRecoiling", false);
        }
        if (defendRecoilTimeRemain > 0) {
            defendRecoilTimeRemain -= Time.deltaTime;
            if (defendRecoilDirection == Position.Left) {
                transform.Translate(new Vector3(-defendRecoilForce*Time.deltaTime,0,0));
            }
            else {
                transform.Translate(new Vector3(defendRecoilForce*Time.deltaTime,0,0));
            }
        }

        if (IsPlayerInvincible())
        {
            PlayerFlickerWhenTakeDamage();
            invincibleTimeRemains -= Time.deltaTime;

        }
        else
        {
            foreach (SkinnedMeshRenderer skinnedMeshRenderer in _skinnedMeshRenderers)
            {
                skinnedMeshRenderer.enabled = true;
            }
        }
    }

    public void ResetInvincibleTime() {
        lastTimeKnockOff = 0;
        lastTimeTakeDamage = 0;
        invincibleTimeRemains = 0;
    }

    private Transform bloodPlace;
    
    public bool TakeDamage(int damage) {
        if (IsPlayerInvincible())
        {
            return false;
        }
        else
        {
            invincibleTimeRemains = invincibieTime;
        }
        lastTimeTakeDamage = Time.time;
        if (GameManager.Instance.PlayerDying) return false;
        DimensionLeapParticleEffect.SetActive(false);
        PlayerProperty.controller.transferStoragePowerFull = false;

        if (Camera.main.GetComponent<CameraEffect>().isShaking)
        {
            Camera.main.GetComponent<CameraEffect>().StopShaking();
        }
        if (_playerMovement.playerCurrentState == PlayerMovement.PlayerState.Block) {
            print("block enemy attack");
            AudioManager.instance.PlaySound(AudioGroup.Character,"Defend");
            GameManager.Instance.player.GetComponent<PlayerCombat>().EnterCounterAttackMode();

        }
        else {
            AudioManager.instance.PlaySound(AudioGroup.Character,"PlayerHurt");
            ChangeHpTo(hp - damage);
            var playerTransform = transform;
            ShowFloatingDamage(damage, playerTransform);
            BloodParticleEffectDisplay(damage);
            FloatingDamageDisplay(damage);
            if (hp <= 0)
            {
//                _playerMovement.ApplyGravity();
                
                GameManager.Instance.PlayerAnimator.PlayerStartDying();
            }
        }

        return true;

    }
    
    private void FloatingDamageDisplay(float damage)
    {
        var textInstantiated = Instantiate(GameManager.Instance.floatingDamage, transform.position + new Vector3(0, 1.5f),
            Quaternion.identity);
        textInstantiated.GetComponentInChildren<TextMeshPro>().text =
            Mathf.Clamp(damage, 0, Mathf.Infinity).ToString(CultureInfo.InvariantCulture);
        textInstantiated.transform.SetParent(null);
        textInstantiated.transform.localScale = new Vector3(Mathf.Abs(textInstantiated.transform.localScale.x),
            Mathf.Abs(textInstantiated.transform.localScale.y), Mathf.Abs(textInstantiated.transform.localScale.z));
    }
    private void BloodParticleEffectDisplay(float damage)
    {
        GameObject bloodTypeToSpawn;
        float bloodScaleMultiplier = 2f;
        if (damage >= 80)
        {
            bloodTypeToSpawn = GameManager.Instance.blood;
        }
        else if (damage > 49)
        {
            bloodTypeToSpawn = GameManager.Instance.smallBlood;    // TODO will be replaced by median blood
        }
        else
        {
            bloodTypeToSpawn = GameManager.Instance.smallBlood;
        }
        Vector3 InstantiateDir;
        InstantiateDir = (transform.position - PlayerProperty.playerPosition).normalized;
        InstantiateDir = new Vector3(InstantiateDir.x,InstantiateDir.y,0);
        GameObject blood = Instantiate(bloodTypeToSpawn, bloodPlace.position, Quaternion.FromToRotation(Vector3.right,InstantiateDir));
        blood.transform.localScale *= bloodScaleMultiplier;
        blood.transform.Rotate(new Vector3(0,0,180));
        blood.transform.SetParent(null);
    }

    private void ShowFloatingDamage(int damage, Transform playerTransform)
    {
        var floatingDamage = Instantiate(floatingText, playerTransform.position + new Vector3(0, 2, 0),
            Quaternion.identity);
        floatingDamage.GetComponent<TextMesh>().text = damage.ToString();
    }

    public bool isPlayerUsingAbility()
    {
        return PlayerProperty.animator.GetCurrentAnimatorStateInfo(0).IsName("Basic Attack") ||
               PlayerProperty.animator.GetCurrentAnimatorStateInfo(0).IsName("Air Slash") ||
               PlayerProperty.animator.GetCurrentAnimatorStateInfo(0).IsName("Dash Uppercut") ||
               PlayerProperty.animator.GetCurrentAnimatorStateInfo(0).IsName("Blackhole") ||
               PlayerProperty.animator.GetCurrentAnimatorStateInfo(0).IsName("Counterattack") ||
               PlayerProperty.animator.GetCurrentAnimatorStateInfo(0).IsName("Defend");
    }
    
    public bool isPlayerUsingGroundAbility()
    {
        return PlayerProperty.animator.GetCurrentAnimatorStateInfo(0).IsName("Basic Attack") ||
               PlayerProperty.animator.GetCurrentAnimatorStateInfo(0).IsName("Dash Uppercut") ||
               PlayerProperty.animator.GetCurrentAnimatorStateInfo(0).IsName("Blackhole") ||
               PlayerProperty.animator.GetCurrentAnimatorStateInfo(0).IsName("Counterattack") ||
               PlayerProperty.animator.GetCurrentAnimatorStateInfo(0).IsName("Defend");
    }

    private bool IsPlayerInvincible()
    {
        return invincibleTimeRemains > 0;
    }

    [SerializeField] private GameObject sprites;
    private bool flickerTrigger;
    private SkinnedMeshRenderer[] _skinnedMeshRenderers;

    private void PlayerFlickerWhenTakeDamage()
    {
        if (flickerTrigger)
        {
            flickerTrigger = false;
            foreach (SkinnedMeshRenderer skinnedMeshRenderer in sprites.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                skinnedMeshRenderer.enabled = false;
            }
        }
        else
        {
            flickerTrigger = true;
            foreach (SkinnedMeshRenderer skinnedMeshRenderer in sprites.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                skinnedMeshRenderer.enabled = true;
            }
        }
    }

    private void ChangeHpTo(float newHp) {
        hp = newHp;
        GameUi.Instance.hpBar.fillAmount = hp / maxHp;
    }

    public void ChangeRageTo(float newRageLevel) {
            rage = Mathf.Clamp(newRageLevel,0,maxRage);
            GameUi.Instance.mpBar.fillAmount = rage / maxRage;
    }

    // Player debuff


    public bool GetKnockOff(Vector3 attackPosition) {

        if (lastTimeKnockOff + invincibieTime > Time.time)
        {
            return false;
        }

        if (_playerMovement.playerCurrentState == PlayerMovement.PlayerState.Block) {
            print("block enemy attack");
            AudioManager.instance.PlaySound(AudioGroup.Character,"PlayerHurt");
            GameManager.Instance.player.GetComponent<PlayerCombat>().EnterCounterAttackMode();
            defendRecoilTimeRemain = defendRecoilTime;
            if (attackPosition.x > transform.position.x) {
                defendRecoilDirection = Position.Left;
            }
            else {
                defendRecoilDirection = Position.Right;
            }
            return false;
            
        }
        DimensionLeapParticleEffect.SetActive(false);
        lastTimeKnockOff = Time.time;
        if (hp <= 0) return false;
        rb.velocity = Vector3.zero;
        PlayerProperty.controller.canControl = false;
        if (attackPosition.x < transform.position.x)
            rb.velocity = new Vector3(-knockUpForce.x, knockUpForce.y, knockUpForce.z);
        else // If enemy attacks from the right
            rb.velocity = knockUpForce;
        _playerMovement.ChangePlayerState(PlayerMovement.PlayerState.KnockUp);
        GameManager.Instance.animator.SetTrigger(knockOff);
        return true;
    }

    public void GetKnockOff(Vector3 attackPosition, Vector3 force) {
        if (lastTimeKnockOff + invincibieTime > Time.time) return;

        if (_playerMovement.playerCurrentState == PlayerMovement.PlayerState.Block) {
            print("block enemy attack");
            AudioManager.instance.PlaySound(AudioGroup.Character,"PlayerHurt");
            GameManager.Instance.player.GetComponent<PlayerCombat>().EnterCounterAttackMode();


            defendRecoilTimeRemain = defendRecoilTime;
            if (attackPosition.x > transform.position.x) {
                defendRecoilDirection = Position.Left;
            }
            else {
                defendRecoilDirection = Position.Right;
            }
            return;
            
        }

        lastTimeKnockOff = Time.time;
        if (hp <= 0) return;
        rb.velocity = Vector3.zero;
        PlayerProperty.controller.canControl = false;
        if (attackPosition.x < transform.position.x)
            rb.velocity = new Vector3(-force.x, force.y, force.z);
        else // If enemy attacks from the right
            rb.velocity = force;
        _playerMovement.ChangePlayerState(PlayerMovement.PlayerState.KnockUp);
        GameManager.Instance.animator.SetTrigger(knockOff);
    }



    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Deadly") && !GameManager.Instance.PlayerDying) {
            print("Player dies to death zone");
            ChangeHpTo(0);
            GameManager.Instance.PlayerAnimator.PlayerStartDying();
        }
    }

    public void RestorePlayerHealth() {
        ChangeHpTo(maxHp);
    }

    public void PlayerInvincibleWhenRespawn()
    {
        invincibleTimeRemains = 3f;
    }
}