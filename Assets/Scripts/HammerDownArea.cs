using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerDownArea : EnemySpawnerComponent
{

    public override void OnEnemyDie()
    {
        Destroy(gameObject);
    }
}
