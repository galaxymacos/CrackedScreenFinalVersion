using UnityEngine;

namespace Skills
{
    public class AirborneSlash : Skill
    {
        [SerializeField] private int damage = 10;
        public EnemyDetector EnemyDetector;

        public Vector3 enemyKnockdownForce;
        private static readonly int airSlash = Animator.StringToHash("Air Slash");


        public override void Start()
        {
            base.Start();
//            playerController.onFacingChangeCallback += CreateAirborneSlashCollider;
        }

        private void Update()
        {
            if (!_skillNotOnCooldown)
            {
                if (TimePlayed + cooldown <= Time.time) _skillNotOnCooldown = true;
            }
        }

        public override void Play()
        {
            print("play air slash");
            if (_skillNotOnCooldown)
            {
                GameManager.Instance.animator.SetTrigger(airSlash);
                _skillNotOnCooldown = false;
                base.Play();

                AudioManager.instance.PlaySound(AudioGroup.Character, "Air Slash");
                AirSlashStrike();
            }
            else
            {
                print("Skill is on cooldown");
            }
        }

//        public void CreateAirborneSlashCollider(bool isFacingRight)
//        {
//            enemyKnockdownForce = new Vector3(-enemyKnockdownForce.x, enemyKnockdownForce.y, enemyKnockdownForce.z);
//        }

        public void AirSlashStrike()
        {
            var enemies = EnemyDetector._enemiesInRange;
            
            foreach (var enemy in enemies)
            {
                print(enemy.name);
                if (enemy == null)
                    continue;
                enemy.GetComponent<Rigidbody>().velocity = Vector3.zero;
                if (enemy.transform.position.x>PlayerProperty.playerPosition.x)
                {
                    enemy.GetComponent<Enemy>().GetKnockUp(enemyKnockdownForce);
                }
                else
                {

                    enemy.GetComponent<Enemy>().GetKnockUp(new Vector3(-enemyKnockdownForce.x,enemyKnockdownForce.y,enemyKnockdownForce.z));
                }
                print("airborne slash hits enemy");
                enemy.GetComponent<Enemy>().TakeDamage(damage);
            }
        }

        public bool PlayerisLeft()
        {
            Vector3 playerPos = GameManager.Instance.player.transform.position;
            Vector3 pos = transform.position;

            return playerPos.x < pos.x;
        }
    }
}