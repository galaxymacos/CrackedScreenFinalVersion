using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingStrike : BossAbility
{
    private Animator animator;

    [SerializeField] private EnemyDetector RollingStrikeHitBoxPlayerDetector;

    [SerializeField] private float jumpForce = 1000f;
    private bool startJumping;
    private float gravity = 4000f;
    [SerializeField] private int damage = 20;

    private Rigidbody _rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        if (startJumping)
        {
            _rigidbody.AddForce(0,-gravity*Time.fixedDeltaTime,0);
            transform.position = Vector3.Lerp(transform.position,PlayerProperty.playerPosition,0.02f);
        }

    }

    public override void Play()
    {
        AudioManager.instance.PlaySound(AudioGroup.SecondBoss,"RollingAttackJump");
        JumpOnPlayer();
        animator.SetTrigger("RollingStrike");
    }

    public void HitPlayerIfItIsInHitBox()
    {
        startJumping = false;
        if (RollingStrikeHitBoxPlayerDetector.playerInRange())
        {
            PlayerProperty.playerClass.GetKnockOff(transform.position);
            PlayerProperty.playerClass.TakeDamage(damage);
        }
    }

//    public void RollingStrikeInitialized()
//    {
//        
//    }
    public void JumpOnPlayer()
    {
        _rigidbody.AddForce(0,jumpForce,0);
        startJumping = true;
    }
}
