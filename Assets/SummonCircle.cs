using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonCircle : MonoBehaviour
{
    [SerializeField] private Transform summonPlace;

    [SerializeField] private GameObject enemyType;
    // Start is called before the first frame update
    internal GameObject enemy;
    private bool hasSummoned;

    public void Summon()
    {
        hasSummoned = true;
        if (enemyType)
        {
            enemy = Instantiate(enemyType, summonPlace.position, Quaternion.identity);
        }
    }

    private void Update()
    {
        if (!enemy && hasSummoned)
        {
            Destroy(gameObject);
        }
    }
}
