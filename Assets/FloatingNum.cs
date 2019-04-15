using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingNum : MonoBehaviour
{
    [SerializeField] private float floatingSpeed = 2f;

    [SerializeField] private float existTime = 1f;

    // Update is called once per frame
    void Update()
    {
        if (existTime > 0f)
        {
            transform.Translate(new Vector3(0,floatingSpeed*Time.deltaTime));
            existTime -= Time.deltaTime;
            if (existTime < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
