using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousStrike_FirstStageBoss : BossAbility
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Play()
    {
        animator.SetTrigger("ContinuousStrike");
    }
    
    public EnemyDetector hitbox;

    public float teleportLimit = 4f;
    public void ContinuousStrike()
    {
        AudioManager.instance.StopSound(AudioGroup.FirstBoss);
        
        AudioManager.instance.PlaySound(AudioGroup.FirstBoss,"SerialAttack");

//        if (continuousStrikeHitBoxScript.playerInRange)
//        {
//            Attack();
//        }
        foreach (GameObject col in hitbox._enemiesInRange)
        {
            if (col == PlayerProperty.player)
            {
                GetComponent<Enemy>().Attack();
                break;
            }
        }
    }

    public void Teleport()
    {
        Vector3 playerPos = PlayerProperty.playerPosition;
        if (Random.Range(0, 2) < 1)
        {
            transform.position = playerPos + new Vector3(-teleportLimit, 0, 0);
            GetComponent<Enemy>().Flip(true);
            
        }
        else
        {
            transform.position = playerPos + new Vector3(teleportLimit, 0, 0);
            GetComponent<Enemy>().Flip(false);
        }
    }
}
