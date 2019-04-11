using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class DragonFist : BossAbility
{
    private Animator animator;

    private bool isDashingForward;
    private bool isDashingRight;

    [SerializeField] private float dashingSpeed = 30f;

    [SerializeField] private float dragonFistFlyKnockUpForce = 1000f;

    [SerializeField] private EnemyDetector dragonFistHitBoxLeft;
    [SerializeField] private EnemyDetector dragonFistHitBoxRight;
    [SerializeField] private EnemyDetector dragonFistDashHitBox;

    [SerializeField] private float followHomeRunChance = 0.5f;

    private bool hasBumpPlayer;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.Instance.isDashingForward)
        {
            if (isDashingRight)
            {
                transform.Translate(new Vector3(dashingSpeed*Time.deltaTime,0,0));
                if (dragonFistDashHitBox.playerInRange())
                {
                    if (!hasBumpPlayer)
                    {
                        bool playerIsKnockOff = PlayerProperty.playerClass.GetKnockOff(PlayerProperty.playerPosition-new Vector3(2,0,0), new Vector3(-20,0,0));
                        if (playerIsKnockOff)
                        {
                            PlayerProperty.playerClass.ResetInvincibleTime();
                            hasBumpPlayer = true;    
                        }
                        
                    }
                }
            }
            else
            {
                transform.Translate(new Vector3(-dashingSpeed*Time.deltaTime,0,0));
                if (dragonFistDashHitBox.playerInRange())
                {
                    if (!hasBumpPlayer)
                    {            
                        bool playerIsKnockOff = PlayerProperty.playerClass.GetKnockOff(PlayerProperty.playerPosition+new Vector3(2,0,0), new Vector3(-20,0,0));
                        if (playerIsKnockOff)
                        {
                            PlayerProperty.playerClass.ResetInvincibleTime();
                            hasBumpPlayer = true;    
                        }
                    }
                }
            }

            
        }
    }

    public override void Play()
    {
        animator.SetTrigger("DragonFist");
        LevelManager.Instance.isDashingForward = true;
        isDashingRight = PlayerProperty.playerPosition.x > transform.position.x;
        AudioManager.instance.PlaySound(AudioGroup.SecondBoss, "DragonFistDash");

    }

    public void DragonFistStrike()
    {
        LevelManager.Instance.isDashingForward = false;
        hasBumpPlayer = false;
        AudioManager.instance.PlaySound(AudioGroup.SecondBoss, "DragonFistStrike");
        if(isDashingRight && dragonFistHitBoxRight.playerInRange() || !isDashingRight && dragonFistHitBoxLeft.playerInRange())
        {
            if (PlayerProperty.playerClass.GetKnockOff(transform.position, new Vector3(0, dragonFistFlyKnockUpForce, 0))
            )
            {
                PlayerProperty.playerClass.TakeDamage(10);
                Camera.main.GetComponent<CameraEffect>().EnlargeCamera(Camera.main.orthographicSize/0.7f);
                GetComponent<SecondStageBoss>().hasEnlargedCameraDragonFist = true;
                PlayerProperty.playerClass.ResetInvincibleTime();    // Player not invincible after it is knocked up by dragon fist
                animator.SetBool("DragonFistHitPlayer",true);
            }
        }
    }


    public void decideIfFollowHomerun()
    {
        float RandomFloat = Random.Range(0, 100) / 100f;
        if (RandomFloat < followHomeRunChance)
        {
            
        }
        else
        {
            animator.enabled = false;
            animator.enabled = true;
        }
    }

    private void OnDrawGizmos()
    {
    }
}
