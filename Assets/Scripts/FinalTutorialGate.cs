using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTutorialGate:EnemySpawnerComponent
{
    

    public override void OnEnemyDie()
    {
        GetComponent<Animator>().SetTrigger("OpenGate");
    }
}
