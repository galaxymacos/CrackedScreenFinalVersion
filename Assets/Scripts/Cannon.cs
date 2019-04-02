using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject cannonBall;
    [SerializeField] private float spawnCannonBallInterval;
    [SerializeField] private GameObject exampleCannonBall;
    private Vector3 exampleCannonBallPosition;
    private Quaternion exampleCannonBallQuaternion;
    private float nextCannonBallTimeRemains;
    private AudioSource _audioSource;

    [SerializeField] private bool canAttack;

    [SerializeField] private EnemyDetector enemyDetector;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        exampleCannonBallPosition = exampleCannonBall.transform.position;
        exampleCannonBallQuaternion = exampleCannonBall.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyDetector.playerInRange())
        {
            canAttack = true;
            transform.LookAt(PlayerProperty.player.transform);
        }
        else
        {
            canAttack = false;
        }
        
        if (canAttack)
        {
            nextCannonBallTimeRemains -= Time.deltaTime;
            if (nextCannonBallTimeRemains <= 0)
            {
                nextCannonBallTimeRemains = spawnCannonBallInterval;
                Instantiate(cannonBall, exampleCannonBallPosition, exampleCannonBallQuaternion);
            }
        }
        
    }
}
