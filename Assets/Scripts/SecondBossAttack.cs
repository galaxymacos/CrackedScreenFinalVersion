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
            case 1: 
                AudioManager.instance.PlaySound(AudioGroup.SecondBoss,"BasicAttackFirstStrike");
                break;
            case 2: 
                AudioManager.instance.PlaySound(AudioGroup.SecondBoss,"BasicAttackSecondStrike");
                break;
            case 3: 
                AudioManager.instance.PlaySound(AudioGroup.SecondBoss,"BasicAttackThirdStrike");
                break;
        }
        if (strikeHitBoxs[attackIndex].playerInRange())
        {
            if (attackIndex == 5)     // If it is the last strike of the basic attack, deal extra damage and knock up player
            {
                PlayerProperty.playerClass.GetKnockOff(transform.position);
                PlayerProperty.playerClass.TakeDamage(30);
            }
            else
            {
                PlayerProperty.playerClass.GetKnockOff(transform.position);
                PlayerProperty.playerClass.TakeDamage(10);
                PlayerProperty.playerClass.ResetInvincibleTime();
            }
        }
    }
}
