using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private PlayerController playerController;
    private static readonly int Die = Animator.StringToHash("DIE");

    private void Start()
    {
        playerController = GameManager.Instance.player.GetComponent<PlayerController>();
    }

    public void LockPlayerControl()
    {
        playerController.canControl = false;
    }

    public void ReleasePlayerControl()
    {
        print("release player control");
        playerController.canControl = true;
    }

    public void PlayerStartDying()
    {
        playerController.canControl = false;
        if (!GameManager.Instance.PlayerDying)
        {
            GameManager.Instance.PlayerDying = true;
            PlayerProperty.animator.SetTrigger(Die);
        }
//        AudioManager.instance.ForceStopAllSfx();
        AudioManager.instance.PlaySound(AudioGroup.Character,"Die");
    }

    public void PlayerDie()
    {
        GameManager.Instance.DecreaseLifeNum();
        if (!GameManager.Instance.isGameOver)
        {
            GameManager.Instance.onPlayerDieCallback?.Invoke();
            playerController.canControl = true;
            GameManager.Instance.PlayerDying = false;
        }

    }
    
    
    

}