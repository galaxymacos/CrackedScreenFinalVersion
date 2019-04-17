using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdFloorGate : EnemySpawnerComponent
{
    public override void OnEnemyDie()
    {
        Destroy(gameObject);
    }
}
