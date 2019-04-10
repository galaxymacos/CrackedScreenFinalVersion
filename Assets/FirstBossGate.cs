using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossGate : EnemySpawnerComponent
{
    [SerializeField] private AudioSource rockRubbing;
    
    public override void OnEnemyDie()
    {
        if (rockRubbing)
        {
            rockRubbing.Play();
        }

        GetComponent<Animator>().enabled = true;
    }
    
    
}
