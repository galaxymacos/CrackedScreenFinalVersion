using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalBlackHole : Skill
{
    [SerializeField] private GameObject gravitationalBlackHole;
    [SerializeField] private Transform place;


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
            suckEnemyDurationLeft -= Time.deltaTime;
            if (suckEnemyDurationLeft <= 0)
            {
                enemyPicked.GetComponent<Enemy>().enabled = true;
                enemyPicked.GetComponent<Animator>().enabled = true;
                print("enable");

                enemyPicked.GetComponent<Enemy>().TakeDamage(damage);
                if (PlayerProperty.playerPosition.x < enemyPicked.transform.position.x)
                {
                    enemyPicked.GetComponent<Enemy>().KnockUp(enemyKnockUpForce);
                }
                else
                {
                    enemyPicked.GetComponent<Enemy>().KnockUp(new Vector3(-enemyKnockUpForce.x,enemyKnockUpForce.y,enemyKnockUpForce.z));
                }

                hasSuckEnemy = false;
            }
        }
    }

    private bool hasSuckEnemy;
    [SerializeField] private float suckEnemyDuration = 2f;
    private float suckEnemyDurationLeft;
    private GameObject enemyPicked;

    public override void Play()
    {
        if (_skillNotOnCooldown) // Check if the skill is on cooldown
        {
            GameManager.Instance.animator.SetTrigger("Black Hole");

            _skillNotOnCooldown = false;
//            print("playing skill");
            base.Play();
//            Instantiate(gravitationalBlackHole, place.position, Quaternion.identity);

            if (!hasSuckEnemy)
            {
                hasSuckEnemy = true;
                suckEnemyDurationLeft = suckEnemyDuration;
                var enemies = skillHitBox._enemiesInRange;
                if (enemies.Count > 0)
                {
                    enemyPicked = enemies[Random.Range(0, enemies.Count - 1)].gameObject;
                    enemyPicked.GetComponent<Animator>().enabled = false;
                    enemyPicked.GetComponent<Enemy>().enabled = false;
                    enemyPicked.transform.position = skillHitBox.transform.position;
                }
            }

            


        }
        else
        {
            print("Black hole attack is on cooldown");
        }
    }
}