using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private EnemyDetector playerDetector;
    [SerializeField] private Transform[] spawnPlace;
    [SerializeField] private GameObject[] enemyToSpawn;
    [SerializeField] private GameObject barrier;
    private List<GameObject> enemyAlreadySpawned;
    private bool hasSpawned = false;

    private void Start()
    {
        enemyAlreadySpawned = new List<GameObject>();
        if (spawnPlace.Length != enemyToSpawn.Length)
        {
            Debug.LogError("Enemy spawner should have the same number of place and enemy to work");
        }
        playerDetector = GetComponent<EnemyDetector>();
    }

    private void Update()
    {
        if (playerDetector.playerInRange() && !hasSpawned)
        {
            Spawn();
        }

        if (hasSpawned)
        {
            if (isAllEnemiesDead())
            {
                if (barrier != null)
                {
                    barrier.GetComponent<EnemySpawnerComponent>().OnEnemyDie();
                }
            }
        }
    }

    public void Spawn()
    {
        for (int i = 0; i < spawnPlace.Length; i++)
        {
            print("Spawn enemy");
            var enemyIns = Instantiate(enemyToSpawn[i], spawnPlace[i].position, Quaternion.identity);
            enemyAlreadySpawned.Add(enemyIns);
        }
        print("Spawn enemy");
        hasSpawned = true;

    }

    public bool isAllEnemiesDead()
    {
        for (int i = 0; i < enemyAlreadySpawned.Count; i++)
        {
            if (enemyAlreadySpawned[i] != null)
            {
                return false;
            }
        }

        return true;
    }
}
