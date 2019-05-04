using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airfreighter : MonoBehaviour
{
    private EnemyDetector playerDetector;
    // Start is called before the first frame update
    void Start()
    {
        playerDetector = GetComponent<EnemyDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDetector && playerDetector.playerInRange())
        {
            GetComponent<FloatingPanel>().enabled = true;
        }
    }
}
