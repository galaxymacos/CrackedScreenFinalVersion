using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBossGate : EnemySpawnerComponent
{
    [SerializeField] private AudioSource rockRubbing;

    [SerializeField] internal bool firstBossDie;
    [SerializeField ]internal bool secondBossDie;
    public override void OnEnemyDie()
    {
        secondBossDie = true;
    }

    private void Update()
    {
        if (firstBossDie && secondBossDie)
        {
            if (rockRubbing)
            {
                rockRubbing.Play();
            }

            GetComponent<AudioSource>().enabled = true;

            GetComponent<Animator>().enabled = true;
        }
    }
}
