using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayToDialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueTrigger bossGreetingDialogue;

    [SerializeField] private float delay;

    private bool hasInteracted;
    private EnemyDetector playerDetector;
    // Start is called before the first frame update
    void Start()
    {
        playerDetector = GetComponent<EnemyDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasInteracted&&playerDetector.playerInRange())
        {
            hasInteracted = true;
        }

        if (hasInteracted&&delay>0)
        {
            delay -= Time.deltaTime;
            if (delay <= 0)
            {
                bossGreetingDialogue.enabled = true;
                bossGreetingDialogue.playerInRange = true;
            }
        }
    }
}
