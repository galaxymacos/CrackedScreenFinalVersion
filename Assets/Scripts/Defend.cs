using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum DefendStage
{
    HoldShield,
    Recoil,
    None
}

public class Defend : Skill
{
    internal DefendStage currentDefendStage;
    [SerializeField] private float blockingDuration = 0.3f;
    private float defendTimeLeft;
    private void Update() {
        PlayerProperty.animator.SetFloat("DefendTimeRemains", defendTimeLeft);
        if (!_skillNotOnCooldown)
        {
            if (TimePlayed + cooldown <= Time.time)
            {
                _skillNotOnCooldown = true;
            }
        }
        
        if (defendTimeLeft > 0) {
            defendTimeLeft -= Time.deltaTime;
        
            if (defendTimeLeft<=0)
            {
                    EndDefend();
            }
        }
       
    }

    public void EndDefend()
    {
        print("End defend");
        PlayerProperty.controller.canControl = true;
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
            playerMovement.ChangePlayerState(PlayerMovement.PlayerState.Defend);
            defendTimeLeft = blockingDuration;

        }
        else
        {
            print("Blocking is on cooldown");
        }
    }

    public void ChangeDefendStage(DefendStage newDefendStage)
    {
        switch (newDefendStage)
        {
            case DefendStage.HoldShield:
                break;
            case DefendStage.Recoil:
                break;
            case DefendStage.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newDefendStage), newDefendStage, null);
        }
        currentDefendStage = newDefendStage;
    }

    public void IsAttack()
    {
        
    }
}