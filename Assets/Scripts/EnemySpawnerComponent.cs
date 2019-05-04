using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The gameobject which has this script
/// </summary>
public abstract class EnemySpawnerComponent : MonoBehaviour
{
    public abstract void OnEnemyDie();
}
