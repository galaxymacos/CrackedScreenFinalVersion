using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private bool hasInteractedWithBarrier;
    [SerializeField] private bool isBossSpawner;
    private void Update()
    {
        if (playerDetector.playerInRange() && !hasSpawned)
        {
            Spawn();
            if (!isBossSpawner)
            {
                if (SceneManager.GetActiveScene().buildIndex == 5)
                {
                    AudioManager.instance.ChangeBgm("EnemySpawner");
                }
                else if(SceneManager.GetActiveScene().buildIndex == 6)
                {
                    AudioManager.instance.ChangeBgm("EnemySpawnerLevel2");
                }
            }
            
        }

        if (hasSpawned)
        {
            if (isAllEnemiesDead())
            {
                if (!hasInteractedWithBarrier)
                {
                    if (!isBossSpawner)
                    {
                        AudioManager.instance.ChangeBgm(AudioManager.instance.prevBgm);

                    }
                    hasInteractedWithBarrier = true;
                    if(barrier != null){
                        
                        barrier.GetComponent<EnemySpawnerComponent>().OnEnemyDie();
                }}
            }
        }
    }

    public void Spawn()
    {
        for (int i = 0; i < spawnPlace.Length; i++)
        {
            var enemyIns = Instantiate(enemyToSpawn[i], spawnPlace[i].position, Quaternion.identity);
            enemyAlreadySpawned.Add(enemyIns);
        }
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
