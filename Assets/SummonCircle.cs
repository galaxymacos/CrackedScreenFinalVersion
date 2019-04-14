using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonCircle : MonoBehaviour
{
    [SerializeField] private Transform summonPlace;

    [SerializeField] private GameObject enemyType;
    // Start is called before the first frame update
    internal GameObject enemy;

    public void Summon()
    {
        if (enemyType)
        {
            Instantiate(enemyType, summonPlace.position, Quaternion.identity);
        }
    }
}
