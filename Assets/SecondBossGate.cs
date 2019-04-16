using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBossGate : EnemySpawnerComponent
{
    [SerializeField] private AudioSource rockRubbing;

    public override void OnEnemyDie()
    {
        if (rockRubbing)
        {
            rockRubbing.Play();
        }

        GetComponent<AudioSource>().enabled = true;

        GetComponent<Animator>().enabled = true;
    }
}
