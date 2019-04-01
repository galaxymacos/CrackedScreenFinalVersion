using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public delegate void OnChangeEnemyState(EnemyState enemyState);

    // State
    public enum EnemyState
    {
        Standing,
        GotHitToAir,
        LayOnGround
    }

    public EnemyState _enemyCurrentState;


    // Private 
    public int atk = 5;
    public float attackRange = 2f;
    public float attackSpeed = 1f; // 1 hit in 1 sec

    [SerializeField] private bool canAttack = true;
    [SerializeField] private bool canKnockUp;
    [SerializeField] private bool canStiff;


    [Header("Enemy ability")] [SerializeField]
    private bool canStun;

    // Ability
    public int defense = 1;

    private float delayBeforeDestroy;

    // Floating damage text
    [SerializeField] private GameObject floatingText;
    public float HP;

    public bool isFacingRight;

    // buff and 
    protected bool isStiffed;

    public float laySec = 2f; // How many seconds the enemy will stay on ground if it got hit to the air

    public float laySecLeft;

    // enemy type
    public float maxHp = 200f;
    public float moveSpeed = 5f;
    public float nextAttackTime;

    public OnChangeEnemyState OnChangeEnemyStateCallback;

    private Player playerScript;


    internal Rigidbody rb;
    internal SpriteRenderer spriteRenderer;


    internal float StiffTimeRemain;
    [SerializeField] private float stunDuration = 0.5f;

    [SerializeField] private AudioSource dieSound;

    public void ChangeEnemyState(EnemyState enemyState)
    {
        if (_enemyCurrentState != enemyState)
        {
            _enemyCurrentState = enemyState;
            OnChangeEnemyStateCallback?.Invoke(enemyState);
        }
    }


    // Start is called before the first frame update
    protected virtual void Start()
    {
        HP = maxHp;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();


        GameManager.Instance.gameObjects.Add(gameObject);

        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        ChangeEnemyState(EnemyState.Standing);

        nextAttackTime = 0;
    }

    public float HitPauseTime = 0.2f;    // The game stop for this second when the enemy is attacked to increase hit feel
    private float HitPauseTimeRemain;    // The game stop for this second when the enemy is attacked to increase hit feel
    private bool istimeSlowing;

    public virtual void TakeDamage(float damage)
    {
        if (HP < 0)
        {
            print("The enemy is already dead");
            return;
        }
        HP -= Mathf.Clamp(damage - defense, 0, Mathf.Infinity);
        HitPauseTimeRemain = HitPauseTime;
        if (HP <= 0)
        {
            Die();
            // TODO add die pause time
        }
        else
        {
            HitPauseTimeRemain = HitPauseTime;
            istimeSlowing = true;
        }
        if (floatingText != null)
        {
            ShowFloatingText();
        }
        else
        {
            print("Floating text is missing for "+gameObject.name);
        }
    }

    internal bool isDead;
    internal Animator animator;
    /// <summary>
    ///     This method will be implemented by children only
    /// </summary>
    protected virtual void Die()
    {
        dieSound.Play();
        isDead = true;
        animator.SetBool("isDead",true);
        Destroy(gameObject, 3f);
    }

    public void Stiff(float time)
    {
        if (!canStiff)
            return;
        isStiffed = true;
        StiffTimeRemain = time;
    }

    public virtual void KnockUp(Vector3 force)
    {
        if (!canKnockUp) return;
        if (HP < 0) return;
//        print("Try to knock up the enemy");
        if (_enemyCurrentState == EnemyState.GotHitToAir) GetComponent<Animator>().SetTrigger("HitToAir");
        FaceBasedOnPlayerPosition();
        rb.AddForce(force);
        ChangeEnemyState(EnemyState.GotHitToAir);
    }

    public void FaceBasedOnPlayerPosition()
    {
        if (AnimationPlaying()) // Can't change facing when boss is using its ability
            return;
        if (PlayerIsAtRight())
            Flip(true);
        else
            Flip(false);
    }

    public void FaceBasedOnMoveDirection()
    {
        if (rb.velocity.x > 0)
            Flip(true);
        else
            Flip(false);
    }

    public bool PlayerIsAtRight()
    {
        return PlayerProperty.playerPosition.x - transform.position.x > 0;
    }

    /// <summary>
    ///     This method will be called when enemy takes damage
    /// </summary>
    private void ShowFloatingText()
    {
        var text = Instantiate(floatingText, transform.position, Quaternion.identity);
        text.GetComponent<TextMesh>().text = Convert.ToInt32(HP).ToString();
    }

    public virtual void Update()
    {
        if (istimeSlowing)
        {
            if (HitPauseTimeRemain > 0)
            {
                Time.timeScale = 0.1f;
                HitPauseTimeRemain -= Time.deltaTime / Time.timeScale;
            }
            else
            {
                istimeSlowing = false;
                Time.timeScale = 1f;
            }
        }

        if (isDead)
        {
            return;
        }

        if (canStiff) TryUnstiffing();


        if (canKnockUp)
            if (_enemyCurrentState == EnemyState.LayOnGround)
                TryStandUp();


        if (GameManager.Instance.PlayerDying) return;

        InteractWithPlayer();
    }

    private void TryUnstiffing()
    {
        if (StiffTimeRemain > 0)
        {
            isStiffed = true;
            StiffTimeRemain -= Time.deltaTime;
        }
        else
        {
            isStiffed = false;
        }
    }

    private void TryStandUp()
    {
        laySecLeft -= Time.deltaTime;
        if (laySecLeft <= 0) StandUp();
    }

    public virtual void StandUp()
    {
        ChangeEnemyState(EnemyState.Standing);
    }


    public void Flip(bool facingRight) // Dealing with face changing in children behavior
    {
        if (facingRight)
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
    }

    /// <summary>
    ///     This method is called in update method
    /// </summary>
    public abstract void InteractWithPlayer();

    public virtual void ForceMove(Vector3 force)
    {
        rb.MovePosition(transform.position + force);
    }

    /// <summary>
    ///     This method is not required, generally, this method will be implemented in InteractWithPlayer method
    /// </summary>
    public abstract void Move();

    /// <summary>
    ///     This method return whether the player is current playing an ability animation
    /// </summary>
    /// <returns></returns>
    public abstract bool AnimationPlaying();

    public void Attack()
    {
        PlayerProperty.playerClass.TakeDamage(atk);
        if (canStun && PlayerProperty.movementClass.playerCurrentState != PlayerMovement.PlayerState.Block)
        {
            PlayerProperty.playerClass.GetStunned(stunDuration);
            Debug.Log("Stun player");
        }

        if (canKnockUp && PlayerProperty.movementClass.playerCurrentState != PlayerMovement.PlayerState.Block)
            PlayerProperty.playerClass.GetKnockOff(transform.position);
    }

    public virtual void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Deadly")) Die();
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            if (_enemyCurrentState == EnemyState.GotHitToAir)
            {
                laySecLeft = laySec;
                ChangeEnemyState(EnemyState.LayOnGround);
            }
    }
}