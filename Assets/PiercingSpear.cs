using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingSpear : BossAbility
{
    private float pierceSpeed = 200f;
    private bool pierceRight;
    private bool isPiercing;
    private Rigidbody rb;

    [SerializeField] private EnemyDetector piercingSpearHitBox;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate()
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PiercingSpear"))
        {
            if (piercingSpearHitBox.playerInRange())
            {
                PlayerProperty.playerClass.TakeDamage(10);
                PlayerProperty.playerClass.GetKnockOff(transform.position);
            }
            if (pierceRight)
            {
                GetComponent<FirstStageBoss>().Flip(true);
                rb.AddForce(new Vector3(pierceSpeed,0,0));
            }
            else
            {
                GetComponent<FirstStageBoss>().Flip(false);
                rb.AddForce(new Vector3(-pierceSpeed,0,0));
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
