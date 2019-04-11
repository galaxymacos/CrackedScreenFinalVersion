using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBossAttack : MonoBehaviour
{
    [SerializeField] private EnemyDetector[] strikeHitBoxs;
    // Start is called before the first frame update

    public void BasicAttackHitPlayer(int attackIndex)
    {
        switch (attackIndex) {
            case 0: 
                AudioManager.instance.PlaySound(AudioGroup.SecondBoss,"BasicAttackFirstStrike");
                break;
            case 1: 
                AudioManager.instance.PlaySound(AudioGroup.SecondBoss,"BasicAttackSecondStrike");
                break;
            case 2: 
                AudioManager.instance.PlaySound(AudioGroup.SecondBoss,"BasicAttackThirdStrike");
                break;
        }
        print(attackIndex);
        if (strikeHitBoxs[attackIndex].playerInRange())
        {
            if (attackIndex == 2)     // If it is the last strike of the basic attack, deal extra damage and knock up player
            {
                PlayerProperty.playerClass.GetKnockOff(transform.position);
                PlayerProperty.playerClass.TakeDamage(30);
            }
            else
            {
                var knockedOff = PlayerProperty.playerClass.GetKnockOff(transform.position);
                PlayerProperty.playerClass.TakeDamage(10);
                if (knockedOff) {
                    PlayerProperty.playerClass.ResetInvincibleTime();
                }
            }
        }
    }
}
