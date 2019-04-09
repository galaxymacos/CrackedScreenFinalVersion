using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Skill
{
    [SerializeField] private float blockingDuration = 0.3f;
    private float blockingDurationLeft;
    private void Update() {
        PlayerProperty.animator.SetFloat("DefendTimeRemains", blockingDurationLeft);
        if (!_skillNotOnCooldown)
        {
            if (TimePlayed + cooldown <= Time.time)
            {
                _skillNotOnCooldown = true;
            }
        }
        
        if (blockingDurationLeft > 0) {
            blockingDurationLeft -= Time.deltaTime;
        
            if (blockingDurationLeft<=0)
            {
                    EndDefend();
            }
        }
       
    }

    public void EndDefend()
    {
        playerMovement.ChangePlayerState(PlayerMovement.PlayerState.Stand);
        _isPlaying = false;
    }

    public override void Play()
    {
        if (_skillNotOnCooldown)
        {
            GameManager.Instance.animator.SetTrigger("Defend");
            AudioManager.instance.PlaySound(AudioGroup.Character,"Defend");
//            playerController.canControl = false;
            base.Play();
            _skillNotOnCooldown = false; // Skill is on cooldown
            playerMovement.ChangePlayerState(PlayerMovement.PlayerState.Block);
            blockingDurationLeft = blockingDuration;

        }
        else
        {
            print("Blocking is on cooldown");
        }
    }
}