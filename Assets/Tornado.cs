using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    [SerializeField] private float growUpTime = 2f;
    private float growUpTimeRemains;
    [SerializeField] private float growUpSpeed = 0.5f;

    [SerializeField] private int damage = 10;
    // Start is called before the first frame update
    void Start()
    {
        growUpTimeRemains = growUpTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (growUpTimeRemains > 0)
        {
            growUpTimeRemains -= Time.deltaTime;
            transform.parent.localScale += new Vector3(growUpSpeed / Time.deltaTime, growUpSpeed / Time.deltaTime, growUpSpeed / Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerProperty.player)
        {
            if (growUpTimeRemains <= 0)
            {
                PlayerProperty.playerClass.TakeDamage(damage);
                PlayerProperty.playerClass.GetKnockOff(transform.position);
            }
        }
    }
}
