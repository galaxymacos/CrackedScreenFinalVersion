using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Input;

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
    internal bool enemyHitPlayerWhenDefend;

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
    
    
    public void RespawnPlayer()
    {
        string encryptedCoordinate = PlayerPrefs.GetString("PlayerLastCoordinate");
        string[] encryptedConponent = encryptedCoordinate.Split(',');
        float x = float.Parse(encryptedConponent[0]);
        float y = float.Parse(encryptedConponent[1]);
        float z = float.Parse(encryptedConponent[2]);
        Vector3 playerLastCoordinate = new Vector3(x, y, z);
        GameManager.Instance.player.transform.position = playerLastCoordinate;
//        PlayerProperty.player.GetComponent<Rigidbody>().velocity = Vector3.zero;
//        PlayerProperty.movementClass.ChangePlayerState(PlayerMovement.PlayerState.Stand);
    }

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

        if (PlayerPrefs.HasKey("PlayerLastCoordinate") && PlayerPrefs.GetString("HasInteracted") == "False")
        {
            RespawnPlayer();
            print("respawn player");
            PlayerPrefs.SetString("HasInteracted", "True");
        }
    }

    [SerializeField] private GameObject threeDimensionParticleSystem;
    private void Update() {
        if (!PlayerProperty.animator.GetCurrentAnimatorStateInfo(0).IsName("Defend"))
        {
            enemyHitPlayerWhenDefend = false;
        }
        
        if (GameManager.Instance.is3D)
        {
            var hasHitCenterGround = Physics.Raycast(PlayerProperty.playerPosition, Vector3.down, out var raycastHit);
            if (hasHitCenterGround)
            {
                threeDimensionParticleSystem.transform.position = raycastHit.point;
            }
            else
            {
                threeDimensionParticleSystem.transform.position = PlayerProperty.playerPosition + new Vector3(0, -2, 0);
            }
        }

PlayerProperty.animator.SetBool("EnemyHitPlayerWhenDefend",enemyHitPlayerWhenDefend);
        if (enemyHitPlayerWhenDefend)
        {
            if (defendRecoilTimeRemain > 0) {
                PlayerProperty.animator.SetBool("DefendRecoiling", true);
            }
            else {
                PlayerProperty.animator.SetBool("DefendRecoiling", false);
                PlayerProperty.controller.canControl = true;
            }
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

//    private void ShowFloatingDamage(int damage, Transform playerTransform)
//    {
//        var floatingDamage = Instantiate(floatingText, playerTransform.position + new Vector3(0, 2, 0),
//            Quaternion.identity);
//        floatingDamage.GetComponent<TextMesh>().text = damage.ToString();
//    }

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
    public bool TakeDamage(int damage) {
        
        if (IsPlayerInvincible() || PlayerProperty.hasSuckedEnemy)
        {
            return false;
        }
        else
        {
            invincibleTimeRemains = invincibieTime;
            PlayerProperty.hasSuckedEnemy = false;
        }
        lastTimeTakeDamage = Time.time;
        if (GameManager.Instance.PlayerDying) return false;
        PlayerProperty.controller.HandleTransformRelease(new InputAction.CallbackContext());
        PlayerProperty.controller.transferStoragePowerFull = false;

        if (Camera.main.GetComponent<CameraEffect>().isShaking)
        {
            Camera.main.GetComponent<CameraEffect>().StopShaking();
        }
        if (PlayerProperty.animator.GetCurrentAnimatorStateInfo(0).IsName("Defend")) {
            AudioManager.instance.PlaySound(AudioGroup.Character,"DefendSucceed");
            GameManager.Instance.player.GetComponent<PlayerCombat>().EnterCounterAttackMode();
            return false;
        }
        else {
            AudioManager.instance.PlaySound(AudioGroup.Character,"PlayerHurt");
            ChangeHpTo(hp - damage);
            BloodParticleEffectDisplay(damage);
            GameManager.Instance.SpawnText(damage,transform.position+new Vector3(0,1,0));
            if (hp <= 0)
            {
                GameManager.Instance.PlayerAnimator.PlayerStartDying();
            }
        }

        return true;

    }

    public bool GetKnockOff(Vector3 attackPosition) {
        if (IsPlayerInvincible() || PlayerProperty.animator.GetCurrentAnimatorStateInfo(0).IsName("Dash Uppercut") || PlayerProperty.hasSuckedEnemy)
        {
            return false;
        }
        else
        {
            PlayerProperty.hasSuckedEnemy = false;
        }
        

        if (PlayerProperty.animator.GetCurrentAnimatorStateInfo(0).IsName("Defend")) {
            
            print("block enemy attack");
            AudioManager.instance.PlaySound(AudioGroup.Character,"DefendSucceed");
            GameManager.Instance.player.GetComponent<PlayerCombat>().EnterCounterAttackMode();
            defendRecoilTimeRemain = defendRecoilTime;
            enemyHitPlayerWhenDefend = true;
            if (attackPosition.x > transform.position.x) {
                defendRecoilDirection = Position.Left;
            }
            else {
                defendRecoilDirection = Position.Right;
            }
            return false;
            
        }
        PlayerProperty.controller.HandleTransformRelease(new InputAction.CallbackContext());

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

    public bool GetKnockOff(Vector3 attackPosition, Vector3 force) {
        if (IsPlayerInvincible() || PlayerProperty.hasSuckedEnemy)
        {
            return false;
        }

        if (_playerMovement.playerCurrentState == PlayerMovement.PlayerState.Defend) {
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

        lastTimeKnockOff = Time.time;
        if (hp <= 0) return false;
        PlayerProperty.controller.HandleTransformRelease(new InputAction.CallbackContext());

        rb.velocity = Vector3.zero;
        PlayerProperty.controller.canControl = false;
        if (attackPosition.x < transform.position.x)
            rb.velocity = new Vector3(-force.x, force.y, force.z);
        else // If enemy attacks from the right
            rb.velocity = force;
        _playerMovement.ChangePlayerState(PlayerMovement.PlayerState.KnockUp);
        GameManager.Instance.animator.SetTrigger(knockOff);
        return true;
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