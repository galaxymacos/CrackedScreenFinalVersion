using UnityEngine;
using UnityEngine.UI;

namespace Skills
{
    public class CounterAttack : Skill
    {
        [SerializeField] private int damage = 10;
        public EnemyDetector EnemyDetector;
        private Player playerProperty;
        [SerializeField] private float stiffTimeWhenHit = 1f;


        [SerializeField] private Vector3 enemyKnockdownForce;

        public override void Start()
        {
            base.Start();
            playerProperty = GameManager.Instance.player.GetComponent<Player>();
        }

        public override void Play()
        {
            PlayerProperty.playerClass.enemyHitPlayerWhenDefend = false;
            PlayerProperty.playerClass.defendRecoilTimeRemain = 0;
            base.Play();
            GameManager.Instance.animator.SetTrigger("Counter Attack");
            AudioManager.instance.PlaySound(AudioGroup.Character,"Counter Attack");
            
        }

        /// <summary>
        /// The method will be called in the frame when the animation hits the enemy
        /// </summary>
        public void Slash()   
        {
            var enemies = EnemyDetector._enemiesInRange;
            foreach (var enemy in enemies)
            {

                if (!enemy || !enemy.GetComponent<Enemy>())
                {
                    continue;
                }
//                enemy.GetComponent<Rigidbody>().velocity = Vector3.zero;
                if (enemy.transform.position.x>GameManager.Instance.player.transform.position.x)
                {
                    enemy.GetComponent<Enemy>().GetKnockUp(enemyKnockdownForce);
                }
                else
                {
                    enemy.GetComponent<Enemy>().GetKnockUp(new Vector3(-enemyKnockdownForce.x,enemyKnockdownForce.y,enemyKnockdownForce.z));
                }
                enemy.GetComponent<Enemy>().TakeDamage(damage);
            }

            PlayerProperty.player.GetComponent<PlayerController>().canControl = true;
        }
        
        public void CreateAirborneSlashCollider(bool isFacingRight)
        {
            enemyKnockdownForce = new Vector3(-enemyKnockdownForce.x, enemyKnockdownForce.y, enemyKnockdownForce.z);
        }
    }
}
