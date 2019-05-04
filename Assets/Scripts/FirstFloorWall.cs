using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstFloorWall : EnemySpawnerComponent
{
    public override void OnEnemyDie()
    {
        Destroy(gameObject);
    }
}
