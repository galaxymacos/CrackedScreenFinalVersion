using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using Random = UnityEngine.Random;

public class SecondStageBoss : Enemy
{
    public bool moveTowardsPlayer;
    private float moveTimeRemainsThisRound;
    internal bool hasEnlargedCameraDragonFist;

    public BossAbility[] BossAbilities;

    [SerializeField] private float ignoreKnockUpTime = 3f;    // Enemy can't be knocked up for seconds after boss just stand up from lying 
    private float ignoreKnockUpTimeLeft;
    public delegate void OnBossDie();

    public OnBossDie OnBossDieCallback;
    [SerializeField] private EnemyDetector autoAttackRange;

    protected override void Start()
    {
        OnChangeEnemyStateCallback += AnimateEnemy;
        OnChangeEnemyStateCallback += SpawnEnemyWhenStandUp;
        specialAttackTimeRemains = specialAttackInterval;
        base.Start();
    }

    public override void GetKnockUp(Vector3 force)
    {
        // Boss can't be knocked up for more than several times

        if (ignoreKnockUpTimeLeft > 0f)
        {
            return;
        }
        base.GetKnockUp(force);
        LevelManager.Instance.isDashingForward = false;
    }

    public override void StandUp()
    {
        base.StandUp();
        ignoreKnockUpTimeLeft = ignoreKnockUpTime;
    }

    protected override void Die()
    {
        base.Die();
        AudioManager.instance.PlaySound(AudioGroup.SecondBoss,"Death");
        OnBossDieCallback?.Invoke();
//        BroadcastMessage("OnSecondBossDie");

    }

    public override void Update()
    {
        base.Update();
        animator.SetBool("AnimationPlaying", AnimationPlaying());
        
        if (ignoreKnockUpTimeLeft>0)
        {
            FlickerWhenTakeDamage();

        }
        else
        {
            foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            {
                sr.enabled = true;
            }
        }
        RageMode();

    }
    
    private bool flickerTrigger;

    public override void TakeDamage(int damage)
    {
        if (ignoreKnockUpTimeLeft > 0)
        {
            return;
        }
        base.TakeDamage(damage);
        LevelManager.Instance.isDashingForward = false;
    }

    private void FlickerWhenTakeDamage()
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


// Start is called before the first frame update

    public override bool AnimationPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") ||
               animator.GetCurrentAnimatorStateInfo(0).IsName("RollingStrike") ||
               animator.GetCurrentAnimatorStateInfo(0).IsName("DashUppercut") ||
               animator.GetCurrentAnimatorStateInfo(0).IsName("HomeRun") ||
               animator.GetCurrentAnimatorStateInfo(0).IsName("DragonFist") ||
               animator.GetCurrentAnimatorStateInfo(0).IsName("BaseBallAttack");

    }

    public bool IsHitOnAirOrLayDown()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("HitToAir")|| 
        animator.GetCurrentAnimatorStateInfo(0).IsName("LayOnGround")||
        animator.GetCurrentAnimatorStateInfo(0).IsName("GetUpFromGround");
    }

    public float specialAttackInterval = 10f;
    public float specialAttackTimeRemains;
    
    public override void InteractWithPlayer()
    {
        if (ignoreKnockUpTimeLeft > 0)
        {
            ignoreKnockUpTimeLeft -= Time.deltaTime;
        }
        if (_enemyCurrentState == EnemyState.Standing && !AnimationPlaying())
        {
            FaceBasedOnPlayerPosition();
            
            if (autoAttackRange.playerInRange())
            {
                if (attackCooldownUp())
                {
                    rb.velocity = new Vector3(0,rb.velocity.y,0);
                    animator.SetTrigger("Attack");

                    nextAttackTime = Time.time + 1 / attackSpeed;
                }
            }

            specialAttackTimeRemains -= Time.deltaTime;
            print(specialAttackTimeRemains);
            if (specialAttackTimeRemains <= 0)
            {
                SpecialAttack();
            }
            
        }

        animator.SetFloat("HorizontalVelocity",rb.velocity.x);
    }

    public bool CanMove()
    {

        return  !AnimationPlaying() && _enemyCurrentState == EnemyState.Standing && !IsHitOnAirOrLayDown();
    }

    private void SpecialAttack()
    {
        specialAttackTimeRemains = specialAttackInterval;
        int randomAbilityIndex = Random.Range(0, BossAbilities.Length);
        BossAbilities[randomAbilityIndex].Play();
            
    }

    private bool attackCooldownUp()
    {
        return Time.time >= nextAttackTime;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (CanMove())
        {

            Move();
            FaceBasedOnMoveDirection();
        }
        else
        {
            if (!IsHitOnAirOrLayDown())
            {
                // TODO why do I add vector.zero here?
//                FaceBasedOnPlayerPosition();    
            }
        }

 
    }

    

    /// <summary>
    /// Was called in FixedUpdate()
    /// </summary>
    public override void Move()
    {
            
            if (!autoAttackRange.playerInRange())
            {
                rb.velocity = new Vector3(PlayerDirectionInPlane().x * moveSpeed,rb.velocity.y,PlayerDirectionInPlane().z*moveSpeed);
            }
    }

    /// <summary>
    /// This method is to calculate the distance between player and enemy (ignore height difference)
    /// </summary>
    /// <returns>The distance of player and enemy </returns>
    private Vector3 PlayerDirectionInPlane()
    {
        Vector3 playerDirection = (GameManager.Instance.player.transform.position - transform.position).normalized;
       return new Vector3(playerDirection.x,0,playerDirection.z);
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
                
                animator.SetBool("Stand",true);
                specialAttackTimeRemains = specialAttackInterval;    // Can't attack instantly when boss get up from the ground
                break;
            case EnemyState.GotHitToAir:
                animator.SetTrigger("HitToAir");
                animator.SetBool("Stand",false);
                break;
            case EnemyState.LayOnGround:
                animator.SetTrigger("LayOnGround");
                animator.SetBool("Stand",false);

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private bool[] hasSpawnedEnemy = new bool[4];
    private GameObject wife;

    public void SpawnEnemyWhenStandUp(EnemyState enemyState)
    {
        if (enemyState == EnemyState.Standing)
        {
            if (HP / maxHp > 0.8)
            {
                if (!hasSpawnedEnemy[0])
                {
                    Instantiate(LevelManager.Instance.SummonCircleMeleeEnemy, transform.position + new Vector3(3, 3),
                        Quaternion.identity);
                    hasSpawnedEnemy[0] = true;
                }
                
            }
            else if (HP / maxHp > 0.6 )
            {
                if (!hasSpawnedEnemy[1])
                {
                    Instantiate(LevelManager.Instance.SummonCircleArcherEnemy, transform.position + new Vector3(-3, 3),
                        Quaternion.identity);
                    hasSpawnedEnemy[1] = true;
                    specialAttackInterval /= 0.7f;
                    attackSpeed *= 1.1f;
                    moveSpeed *= 1.1f;
                }
                
            }
            else if(HP/maxHp>0.4)
            {
                if (!hasSpawnedEnemy[2])
                {
                    Instantiate(LevelManager.Instance.SummonCircleArcherEnemy, transform.position + new Vector3(3, 3),
                        Quaternion.identity);
                    Instantiate(LevelManager.Instance.SummonCircleMeleeEnemy, transform.position + new Vector3(-3, 3),
                        Quaternion.identity);
                    hasSpawnedEnemy[2] = true;    
                    specialAttackInterval /= 0.5f;
                    attackSpeed *= 1.2f;
                    moveSpeed *= 1.2f;
                }
                
            }
            else
            {
                if (!hasSpawnedEnemy[3])
                {
                    GetComponent<DialogueTrigger>().enabled = true;
                    StartCoroutine("InstantiateWife", 1);
                    hasSpawnedEnemy[3] = true;
                    
                }
                
            }
        }
    }

    private bool isRage;
    public void RageMode()
    {
        if (HP < maxHp && isRage)
        {
            HP += 30 * Time.deltaTime;
            if (HP >= maxHp)
            {
                HP = maxHp;
                isRage = false;
            }
        }
        
    }

    public void InstantiateWife()
    {
        wife = Instantiate(LevelManager.Instance.SummonCircleFirstStageBoss, transform.position + new Vector3(3, 3),
            Quaternion.identity);
        isRage = true;
    }
}
