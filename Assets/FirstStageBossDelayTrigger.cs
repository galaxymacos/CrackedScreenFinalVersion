using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstStageBossDelayTrigger : MonoBehaviour
{
    [SerializeField] private float delay = 0.3f;

    private bool hasInteracted;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (delay > 0 && !hasInteracted)
        {
            delay -= Time.deltaTime;
            if (delay < 0)
            {
                hasInteracted = true;
                GetComponent<DialogueTrigger>().enabled = true;
                AudioManager.instance.ChangeBgm("BaJianShenQu");
            }
        }
    }
}
