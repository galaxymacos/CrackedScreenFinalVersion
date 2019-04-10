using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmChangeTrigger : MonoBehaviour
{
    [SerializeField] private string bgmName;
    private EnemyDetector playerDetector;
    // Start is called before the first frame update
    void Start()
    {
        playerDetector = GetComponent<EnemyDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDetector.playerInRange())
        {
            AudioManager.instance.currentBgm = bgmName;
        }
    }
}
