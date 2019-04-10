using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private EnemyDetector playerDetector;
    [SerializeField] private Transform[] spawnPlace;
    [SerializeField] private GameObject[] enemyToSpawn;
    private bool hasSpawned;

    private void Start()
    {
        if (spawnPlace.Length != enemyToSpawn.Length)
        {
            Debug.LogError("Enemy spawner should have the same number of place and enemy to work");
        }
        playerDetector = GetComponent<EnemyDetector>();
    }

    private void Update()
    {
        if (playerDetector.playerInRange())
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        for (int i = 0; i < spawnPlace.Length; i++)
        {
            Instantiate(enemyToSpawn[i], spawnPlace[i].position, Quaternion.identity);
        }
        print("Spawn enemy");
        
        enabled = false;
    }
}
