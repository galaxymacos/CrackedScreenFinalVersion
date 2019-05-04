using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFistHitBoxes : MonoBehaviour
{
    private GameObject boss;
    // Start is called before the first frame update
    void Start()
    {
        boss = transform.parent.gameObject;
        transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        if (boss)
        {
            transform.position = boss.transform.position;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
