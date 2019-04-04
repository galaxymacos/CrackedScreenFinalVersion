﻿using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FirstStageBoss : Enemy
{
    public delegate void OnBossDie();

    [SerializeField] private EnemyDetector autoAttackRange;


    public BossAbility[] BossAbilities;


// Start is called before the first frame update
    private bool canMove;
    private int currentKnockUpTimes;

    [SerializeField]
    private float ignoreKnockUpTime = 3f; // Enemy can't be knocked up for seconds after boss just stand up from lying 

    private float ignoreKnockUpTimeLeft;
    private bool isplayingAnimation;

    public float moveTimeInARow = 3f;

    private float moveTimeRemainsThisRound;

    public bool moveTowardsPlayer;
    public float movingTowardsPlayerPercentage = 0.7f;

    public OnBossDie OnBossDieCallback;
    public string[] specialAttackAnimationNames;

    public float specialAttackInterval = 10f;
    public float specialAttackTimeRemains;

    protected override void Start()
    {
        OnChangeEnemyStateCallback += AnimateEnemy;
        specialAttackTimeRemains = specialAttackInterval;
        base.Start();
    }


    public override void KnockUp(Vector3 force)
    {
        // Boss can't be knocked up for more than several times


        if (ignoreKnockUpTimeLeft > 0f) return;
        base.KnockUp(force);
    }

    public override void StandUp()
    {
        base.StandUp();
        ignoreKnockUpTimeLeft = ignoreKnockUpTime;
    }

    protected override void Die()
    {
        base.Die();
        OnBossDieCallback?.Invoke();
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
               animator.GetCurrentAnimatorStateInfo(0).IsName("ContinuousStrike");
    }

    public override void InteractWithPlayer()
    {
        if (ignoreKnockUpTimeLeft > 0) ignoreKnockUpTimeLeft -= Time.deltaTime;
        if ((StiffTimeRemain <= 0) & (_enemyCurrentState == EnemyState.Standing))
        {
            if (autoAttackRange.playerInRange())
                if (attackCooldownUp() && !animationPlaying())
                {
                    rb.velocity = new Vector3(0, rb.velocity.y, 0);
                    animator.SetTrigger("Attack");

                    nextAttackTime = Time.time + 1 / attackSpeed;
                }

            SpecialAttack();
        }


        if (!animationPlaying())
        {
            ReleaseEnemyMove();
            ChangeFacing(rb.velocity.x);
        }
        else
        {
            LockEnemyMove();
        }

        canMove = !isStiffed && !isplayingAnimation;

        animator.SetFloat("HorizontalVelocity", rb.velocity.x);
    }

    private void SpecialAttack()
    {
        specialAttackTimeRemains -= Time.deltaTime;
        if (specialAttackTimeRemains <= 0)
            if (!animationPlaying())
            {
                specialAttackTimeRemains = specialAttackInterval;
                var randomAbilityIndex = Random.Range(0, BossAbilities.Length);
                BossAbilities[randomAbilityIndex].Play();
            }
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

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (canMove && _enemyCurrentState == EnemyState.Standing) Move();
    }

// 


    public override void Move()
    {
        if (moveTimeRemainsThisRound > 0)
        {
            if (moveTowardsPlayer)
            {
//                rb.MovePosition(transform.position + PlayerDirectionInPlane()*moveSpeed*Time.fixedDeltaTime);
                rb.velocity = new Vector3(PlayerDirectionInPlane().x * moveSpeed, rb.velocity.y);
//                transform.Translate(PlayerDirectionInPlane()*moveSpeed*Time.deltaTime);
                moveTimeRemainsThisRound -= Time.fixedDeltaTime;
            }
            else
            {
//                rb.MovePosition(transform.position-PlayerDirectionInPlane()*moveSpeed*Time.fixedDeltaTime);
                rb.velocity = new Vector3(-PlayerDirectionInPlane().x * moveSpeed, rb.velocity.y);


                moveTimeRemainsThisRound -= Time.fixedDeltaTime;
            }
        }
        else
        {
            ChangeBossMovementDirectionInRandom();
        }
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall")) moveTowardsPlayer = !moveTowardsPlayer;
    }

    public void AnimateEnemy(EnemyState enemyState)
    {
        switch (_enemyCurrentState)
        {
            case EnemyState.Standing:
                animator.SetBool("Stand", true);
                break;
            case EnemyState.GotHitToAir:
                animator.SetTrigger("HitToAir");
                break;
            case EnemyState.LayOnGround:
                animator.SetTrigger("LayDown");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void AutoAttack()
    {
        if (autoAttackRange.playerInRange()) Attack();
    }
}