using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingSpear : BossAbility
{
    private float pierceSpeed = 200f;
    private bool pierceRight;
    private bool isPiercing;
    [SerializeField] private HitWall hitwall;
    private Rigidbody rb;

    private bool tookDamageInFirstStage;

    [SerializeField] private EnemyDetector piercingSpearHitBox;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PiercingSpear"))
        {
            if (piercingSpearHitBox.playerInRange() && !tookDamageInFirstStage)
            {
                hitwall.piercingPlayer = true;
                tookDamageInFirstStage = true;
                PlayerProperty.playerClass.TakeDamage(10);
                PlayerProperty.playerClass.ResetInvincibleTime();
//                PlayerProperty.playerClass.GetKnockOff(transform.position);
                PlayerProperty.controller.canControl = false;
                
                PlayerProperty.player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            if (pierceRight)
            {
                if (piercingSpearHitBox.playerInRange())
                {
                    PlayerProperty.player.transform.position = transform.position+new Vector3(3,0,0);
                }
                GetComponent<FirstStageBoss>().Flip(true);
                rb.AddForce(new Vector3(pierceSpeed,0,0));
//                if (piercingSpearHitBox.playerInRange())
                {
//                    PlayerProperty.player.transform.position += new Vector3(25*Time.fixedDeltaTime,0,0);
                }
            }
            else
            {
                if (piercingSpearHitBox.playerInRange())
                {
                    PlayerProperty.player.transform.position = transform.position+new Vector3(-3,0,0);
                }

                GetComponent<FirstStageBoss>().Flip(false);
                rb.AddForce(new Vector3(-pierceSpeed,0,0));
//                if (piercingSpearHitBox.playerInRange())
                {
//                    PlayerProperty.player.transform.position += new Vector3(-25*Time.fixedDeltaTime,0,0);
                }
            }
        }
    }

    public override void Play()
    {
        GetComponent<Animator>().SetTrigger("PiercingSpear");
        isPiercing = true;
        if (PlayerProperty.playerPosition.x < transform.position.x)
        {
            pierceRight = false;
        }
        else
        {
            pierceRight = true;

        }
    }


}
