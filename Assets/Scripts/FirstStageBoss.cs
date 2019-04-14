using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class FirstStageBoss : Enemy
{
    internal delegate void OnFirstStageBossDie();

    [SerializeField] private EnemyDetector autoAttackRange;


    public BossAbility[] BossAbilities;


// Start is called before the first frame update
    private int currentKnockUpTimes;

    [SerializeField]
    private float ignoreKnockUpTime = 3f; // Enemy can't be knocked up for seconds after boss just stand up from lying 

    private float ignoreKnockUpTimeLeft;
    private bool isplayingAnimation;

    public float moveTimeInARow = 3f;

    private float moveTimeRemainsThisRound;

    public bool moveTowardsPlayer;
    public float movingTowardsPlayerPercentage = 0.7f;

    internal OnFirstStageBossDie onFirstStageBossDieCallback;
    public string[] specialAttackAnimationNames;

    public float specialAttackInterval = 10f;
    public float specialAttackTimeRemains;
    
    

    protected override void Start()
    {
        OnChangeEnemyStateCallback += AnimateEnemy;
        OnChangeEnemyStateCallback += SpawnEnemyWhenStandUp;
        specialAttackTimeRemains = specialAttackInterval;
        base.Start();
    }


    public override void GetKnockUp(Vector3 force)
    {
        if (ignoreKnockUpTimeLeft > 0f) return;
        base.GetKnockUp(force);
        LevelManager.Instance.piercingPlayer = false;
    }

    public override void StandUp()
    {
        base.StandUp();
        ignoreKnockUpTimeLeft = ignoreKnockUpTime;
    }

    protected override void Die()
    {
        base.Die();
        AudioManager.instance.PlaySound(AudioGroup.FirstBoss,"Die");
        onFirstStageBossDieCallback?.Invoke();
        LevelManager.Instance.piercingPlayer = false;
    }

    public void LockEnemyMove()
    {
        isplayingAnimation = true;
    }

    public void ReleaseEnemyMove()
    {
        isplayingAnimation = false;
    }


    private bool animationPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") ||
               animator.GetCurrentAnimatorStateInfo(0).IsName("RollingAttack") ||
               animator.GetCurrentAnimatorStateInfo(0).IsName("ContinuousStrike") ||
               animator.GetCurrentAnimatorStateInfo(0).IsName("TornadoMaker")||
               animator.GetCurrentAnimatorStateInfo(0).IsName("PiercingSpear");
    }

    public override void InteractWithPlayer()
    {
        if (ignoreKnockUpTimeLeft > 0) ignoreKnockUpTimeLeft -= Time.deltaTime;
        if (_enemyCurrentState == EnemyState.Standing)
        {
            if (autoAttackRange.playerInRange())
                if (attackCooldownUp() && !animationPlaying())
                {
                    rb.velocity = new Vector3(0, rb.velocity.y, 0);
                    animator.SetTrigger("Attack");
                    AudioManager.instance.PlaySound(AudioGroup.FirstBoss,"BasicAttack");

                    nextAttackTime = Time.time + 1 / attackSpeed;
                }

            specialAttackTimeRemains -= Time.deltaTime;
            if (specialAttackTimeRemains <= 0 && !animationPlaying())
            {
                SpecialAttack();

            }
        }
        if (!animationPlaying() && !IsHitOnAirOrLayDown())
        {
            ReleaseEnemyMove();
            ChangeFacing(rb.velocity.x);
        }
        else
        {
            LockEnemyMove();
        }
        animator.SetFloat("HorizontalVelocity", rb.velocity.x);
    }
    private void SpecialAttack()
    {
        print("Play special attack");
        specialAttackTimeRemains = specialAttackInterval;
        var randomAbilityIndex = Random.Range(0, BossAbilities.Length); 
        BossAbilities[randomAbilityIndex].Play();
    }

    public bool CanMove()
    {
        return !AnimationPlaying() && _enemyCurrentState == EnemyState.Standing && !IsHitOnAirOrLayDown();
    }
    
    public bool IsHitOnAirOrLayDown()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("HitToAir") ||
               animator.GetCurrentAnimatorStateInfo(0).IsName("LayDown");
    }

    
    public override void TakeDamage(float damage)
    {
        if (ignoreKnockUpTimeLeft > 0)
            return;
        base.TakeDamage(damage);
        AudioManager.instance.PlaySound(AudioGroup.FirstBoss,"IsHit");
        LevelManager.Instance.piercingPlayer = false;
    }
    
    

    

    private void ChangeFacing(float horizontalSpeed)
    {
        if (horizontalSpeed > 0) Flip(true);

        if (horizontalSpeed < 0) Flip(false);
    }


    private bool attackCooldownUp()
    {
        return Time.time >= nextAttackTime;
    }

    private bool flickerTrigger;

    
    public override void Update()
    {
        base.Update();
        if (ignoreKnockUpTimeLeft>0)
        {
            PlayerFlickerWhenTakeDamage();

        }
        else
        {
            foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            {
                sr.enabled = true;
            }
        }
    }
    
    private void PlayerFlickerWhenTakeDamage()
    {
        if (flickerTrigger)
        {
            flickerTrigger = false;
            foreach (SpriteRenderer skinnedMeshRenderer in GetComponentsInChildren<SpriteRenderer>())
            {
                skinnedMeshRenderer.enabled = false;
            }
        }
        else
        {
            flickerTrigger = true;
            foreach (SpriteRenderer skinnedMeshRenderer in GetComponentsInChildren<SpriteRenderer>())
            {
                skinnedMeshRenderer.enabled = true;
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (CanMove() && _enemyCurrentState == EnemyState.Standing)
        {
            Move();
        }
    }

    [SerializeField] private EnemyDetector playerInRangeDetector;

    public override void Move()
    {
        /*
        if (moveTimeRemainsThisRound > 0)
        {
            if (moveTowardsPlayer)
            {
                rb.velocity = new Vector3(PlayerDirectionInPlane().x * moveSpeed, rb.velocity.y);
                moveTimeRemainsThisRound -= Time.fixedDeltaTime;
            }
            else
            {
                rb.velocity = new Vector3(-PlayerDirectionInPlane().x * moveSpeed, rb.velocity.y);


                moveTimeRemainsThisRound -= Time.fixedDeltaTime;
            }
        }
        else
        {
            ChangeBossMovementDirectionInRandom();
        }
        */
        if(!playerInRangeDetector.playerInRange())
        rb.velocity = new Vector3(PlayerDirectionInPlane().x * moveSpeed, rb.velocity.y, PlayerDirectionInPlane().z*moveSpeed);
    }

    /// <summary>
    ///  This method needs to be updated when new attack ability is added in animator
    /// </summary>
    /// <returns></returns>
    public override bool AnimationPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("RollingAttack") ||
               animator.GetCurrentAnimatorStateInfo(0).IsName("ContinuousStrike") ||
               animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
    }

    private Vector3 PlayerDirectionInPlane()
    {
        var playerDirection = (GameManager.Instance.player.transform.position - transform.position).normalized;
        return new Vector3(playerDirection.x, 0, playerDirection.z);
    }

    private void ChangeBossMovementDirectionInRandom()
    {
        var randomNumber = Random.Range(0, 100);
        if (randomNumber <= movingTowardsPlayerPercentage * 100) moveTowardsPlayer = true;

        moveTimeRemainsThisRound = moveTimeInARow;
    }

    public override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            moveTowardsPlayer = !moveTowardsPlayer;
            

        }
    }

    public void AnimateEnemy(EnemyState enemyState)
    {
        switch (_enemyCurrentState)
        {
            case EnemyState.Standing:
                animator.SetBool("Stand", true);
                break;
            case EnemyState.GotHitToAir:
                animator.SetBool("Stand",false);
                animator.SetTrigger("HitToAir");
                break;
            case EnemyState.LayOnGround:
                animator.SetBool("Stand",false);
                animator.SetTrigger("LayDown");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private bool[] hasSpawnedEnemy = new bool[3];
    
    public void SpawnEnemyWhenStandUp(EnemyState enemyState)
    {
        if (!canSummon)
        {
            return;
        }
        
        if (enemyState == EnemyState.Standing)
        {
            if (HP / maxHp > 0.9)
            {
                return;
            }
            else if (HP / maxHp > 0.6 && !hasSpawnedEnemy[0])
            {
                Instantiate(LevelManager.Instance.SummonCircleMeleeEnemy, transform.position + new Vector3(3, 3),
                    Quaternion.identity);
                hasSpawnedEnemy[0] = true;
                specialAttackInterval /= 0.7f;
                attackSpeed *= 1.5f;
                moveSpeed *= 1.5f;
            }
            else if(HP/maxHp>0.3 && hasSpawnedEnemy[1])
            {
                Instantiate(LevelManager.Instance.SummonCircleArcherEnemy, transform.position + new Vector3(-3, 3),
                    Quaternion.identity);
                hasSpawnedEnemy[1] = true;
                specialAttackInterval /= 0.5f;
                attackSpeed *= 1.3f;
                moveSpeed *= 1.5f;
            }
            else if(!hasSpawnedEnemy[2])
            {
                Instantiate(LevelManager.Instance.SummonCircleArcherEnemy, transform.position + new Vector3(3, 3),
                    Quaternion.identity);
                Instantiate(LevelManager.Instance.SummonCircleMeleeEnemy, transform.position + new Vector3(-3, 3),
                    Quaternion.identity);
                hasSpawnedEnemy[2] = true;

            }
        }
    }

    public void AutoAttack()
    {
        if (autoAttackRange.playerInRange()) Attack();
    }
}