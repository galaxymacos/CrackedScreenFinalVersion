using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCatch : Skill
{
    [SerializeField] private EnemyDetector skillHitBox;
    [SerializeField] private int damage;
    [SerializeField] private Vector3 enemyKnockUpForce;
    


    private bool _tookDamage;

    private void Update()
    {
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

                print("enable");

                if (PlayerProperty.playerPosition.x < enemyPicked.transform.position.x)
                {
                    enemyPicked.GetComponent<Enemy>().KnockUp(enemyKnockUpForce);
                }
                else
                {
                    enemyPicked.GetComponent<Enemy>().KnockUp(new Vector3(-enemyKnockUpForce.x,enemyKnockUpForce.y,enemyKnockUpForce.z));
                }
                enemyPicked.GetComponent<Enemy>().TakeDamage(damage);
                var explodeEffect = Instantiate(explodeParticleEffect, explodeSpawnPlace.position, explodeSpawnPlace.rotation);
                explodeEffect.transform.parent = null;

                enemyPicked.GetComponent<Animator>().SetBool("isBeingSucked",false);
                hasSuckEnemy = false;

            }
        }
    }

    private bool hasSuckEnemy;
    [SerializeField] private float suckEnemyDuration = 2f;
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
            StartCoroutine(PlayerCanControl(headCatchAnimationClip.length));

            GameManager.Instance.animator.SetTrigger("Black Hole");    // play player catch head animation

            _skillNotOnCooldown = false;
            base.Play();

            if (!hasSuckEnemy)
            {
                
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