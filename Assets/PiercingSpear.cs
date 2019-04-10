using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingSpear : BossAbility
{
    private float pierceSpeed = 200f;
    private bool pierceRight;
    private bool isPiercing;
    private Rigidbody rb;

    internal bool tookDamageInFirstStage;

    [SerializeField] private EnemyDetector piercingSpearHitBox;
    
    private bool isTouchingWall;
    internal bool piercingPlayer;
    [SerializeField] private int hitWallDamage = 15;
    [SerializeField] private AudioSource piercingHitWall;

  

    private void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (transform.parent.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PiercingSpear"))
        {
            if (piercingSpearHitBox.playerInRange() && !tookDamageInFirstStage && PlayerProperty.playerClass.hp > 0 && PlayerProperty.playerClass.invincibleTimeRemains<=0)
            {
                tookDamageInFirstStage = true;
                LevelManager.Instance.piercingPlayer = true;
                AudioManager.instance.PlaySound(AudioGroup.FirstBoss,"PierceHitPlayer");
                PlayerProperty.playerClass.GetKnockOff(transform.parent.position);
                PlayerProperty.playerClass.TakeDamage(5);
                PlayerProperty.playerClass.invincibleTimeRemains = 5f;
//                PlayerProperty.playerClass.GetKnockOff(transform.position);
                
                PlayerProperty.player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            if (pierceRight)
            {
                if (LevelManager.Instance.piercingPlayer) {
                    if (piercingSpearHitBox.playerInRange() && PlayerProperty.playerClass.hp > 0)
                    {
                        PlayerProperty.playerClass.GetKnockOff(transform.parent.position);

                        PlayerProperty.player.transform.position = transform.position+new Vector3(3,0,0);

                    }    
                }
                
                transform.parent.GetComponent<FirstStageBoss>().Flip(true);
                rb.AddForce(new Vector3(pierceSpeed,0,0));
            }
            if(!pierceRight)
            {
                if (LevelManager.Instance.piercingPlayer) {
                    if (piercingSpearHitBox.playerInRange() && PlayerProperty.playerClass.hp > 0)
                    {
                        PlayerProperty.playerClass.GetKnockOff(transform.parent.position);

                        PlayerProperty.player.transform.position = transform.position+new Vector3(-3,0,0);
                    }
                }
                

                transform.parent.GetComponent<FirstStageBoss>().Flip(false);
                rb.AddForce(new Vector3(-pierceSpeed,0,0));

            }
        }
    }

    public override void Play()
    {
        if (isTouchingWall)
        {
            return;
        }
        transform.parent.GetComponent<Animator>().SetTrigger("PiercingSpear");
        AudioManager.instance.PlaySound(AudioGroup.FirstBoss,"Pierce");
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
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall") && !isTouchingWall)
        {
            tookDamageInFirstStage = false;
            isTouchingWall = true;
            
            if (piercingHitWall !=null &&piercingHitWall.clip != null)
            {
                piercingHitWall.Play();
            }
            AudioManager.instance.StopSound(AudioGroup.FirstBoss);
            AudioManager.instance.PlaySound(AudioGroup.FirstBoss,"PierceHitWall");
            transform.parent.GetComponent<Animator>().SetTrigger("PiercingSpearHitWall");
            print("Hit wall");
            if (LevelManager.Instance.piercingPlayer)
            {
                LevelManager.Instance.piercingPlayer = false;
                PlayerProperty.controller.canControl = true;
                PlayerProperty.playerClass.ResetInvincibleTime();
                if (pierceRight)
                {
                    PlayerProperty.playerClass.GetKnockOff(PlayerProperty.player.transform.position+new Vector3(-2,0,0),new Vector3(15,15,0));
                }
                else
                {
                    PlayerProperty.playerClass.GetKnockOff(PlayerProperty.player.transform.position-new Vector3(2,0,0),new Vector3(-15,15,0));
                }
                PlayerProperty.playerClass.TakeDamage(hitWallDamage);

            }
           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            isTouchingWall = false;
        }
    }
    
    
    


}
