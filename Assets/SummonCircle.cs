using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonCircle : MonoBehaviour
{
    [SerializeField] internal Transform summonPlace;

    [SerializeField] internal GameObject enemyType;
    // Start is called before the first frame update
    internal GameObject enemy;
    internal bool hasSummoned;

    public virtual void Summon()
    {
        hasSummoned = true;
        if (enemyType)
        {
            enemy = Instantiate(enemyType, summonPlace.position, Quaternion.identity);
        }
    }

    public virtual void Update()
    {
        if (!enemy && hasSummoned)
        {
            Destroy(gameObject);
        }
    }
}
