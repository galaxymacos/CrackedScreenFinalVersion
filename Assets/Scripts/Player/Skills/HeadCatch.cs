using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCatch : Skill
{
    [SerializeField] private EnemyDetector skillHitBox;
    [SerializeField] private int damage;
    [SerializeField] private Vector3 enemyKnockUpForce;
    private bool canRelease;
    private bool hasReleased;

    private bool _tookDamage;

    private void Update()
    {
        if (PlayerProperty.animator.GetCurrentAnimatorStateInfo(0).IsName("Blackhole") && !canRelease)
        {
            hasReleased = false;
            PlayerProperty.controller.canControl = false;
        }
        else
        {
            if (!hasReleased)
            {
                PlayerProperty.controller.canControl = true;
                hasReleased = true;
            }
        }
        if (!_skillNotOnCooldown)
        {
            if (TimePlayed + cooldown <= Time.time)
            {
                _skillNotOnCooldown = true;
            }
        }
        
        if (hasSuckEnemy && suckEnemyDurationLeft > 0)
        {
            enemyPicked.transform.position = skillHitBox.transform.position;

            suckEnemyDurationLeft -= Time.deltaTime;
            if (suckEnemyDurationLeft <= 0)
            {
                if (enemyPicked == null || !enemyPickedIsHitToAir)
                {
                    return;
                }
                enemyPicked.GetComponent<Enemy>().enabled = true;

                if (PlayerProperty.playerPosition.x < enemyPicked.transform.position.x)
                {
                    enemyPicked.GetComponent<Enemy>().GetKnockUp(enemyKnockUpForce);
                }
                else
                {
                    enemyPicked.GetComponent<Enemy>().GetKnockUp(new Vector3(-enemyKnockUpForce.x,enemyKnockUpForce.y,enemyKnockUpForce.z));
                }
                AudioManager.instance.PlaySound(AudioGroup.Character,"HeadCatchExplode");
                enemyPicked.GetComponent<Enemy>().TakeDamage(damage);
                var explodeEffect = Instantiate(explodeParticleEffect, explodeSpawnPlace.position, explodeSpawnPlace.rotation);
                explodeEffect.transform.parent = null;

                enemyPicked.GetComponent<Animator>().SetBool("isBeingSucked",false);
                hasSuckEnemy = false;
            }
        }
    }

    private bool hasSuckEnemy;
    private float suckEnemyDuration = 1.1f;
    [SerializeField] private AnimationClip headCatchAnimationClip;
    [SerializeField] private GameObject explodeParticleEffect;
    [SerializeField] private Transform explodeSpawnPlace;
    private float suckEnemyDurationLeft;
    private GameObject enemyPicked;
    private bool enemyPickedIsHitToAir;

    public override void Play()
    {
        if (_skillNotOnCooldown) // Check if the skill is on cooldown
        {
            playerController.canControl = false;
            StartCoroutine(PlayerCanControl(PlayerProperty.animator.GetCurrentAnimatorStateInfo(0).length));
            AudioManager.instance.PlaySound(AudioGroup.Character,"HeadCatchPerform");
            GameManager.Instance.animator.SetTrigger("Black Hole");    // play player catch head animation

            _skillNotOnCooldown = false;
            base.Play();

            if (!hasSuckEnemy)
            {
                if (Time.time - GameManager.Instance.lastHitEnemyTime < 0.5f)
                {
                    enemyPicked = GameManager.Instance.lastHitEnemy;
                    if (enemyPicked != null)
                    {
                        if (enemyPicked.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("HitToAir"))
                        {
                            suckEnemyDurationLeft = suckEnemyDuration;
                            hasSuckEnemy = true;
                            enemyPickedIsHitToAir = true;
                            enemyPicked.GetComponent<Animator>().SetBool("isBeingSucked",true);
                            enemyPicked.GetComponent<Enemy>().enabled = false;
                            return;
                        }  
                    }

                }
                var enemies = skillHitBox._enemiesInRange;
                if (enemies.Count > 0)
                {
                    enemyPicked = enemies[Random.Range(0, enemies.Count - 1)].gameObject;
                    if (enemyPicked.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("HitToAir"))
                    {
                        suckEnemyDurationLeft = suckEnemyDuration;
                        hasSuckEnemy = true;
                        enemyPickedIsHitToAir = true;
                        enemyPicked.GetComponent<Animator>().SetBool("isBeingSucked",true);
                        print("is being sucked");
                        
                        enemyPicked.GetComponent<Enemy>().enabled = false;
                    }  
                }
                else {
                    hasSuckEnemy = false;
                }
                   
                    
                
            }
        }
        else
        {
            print("Black hole attack is on cooldown");
        }
    }
}