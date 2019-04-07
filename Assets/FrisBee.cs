using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrisBee : MonoBehaviour
{
    internal GameObject master;

    private Rigidbody rb;

    [SerializeField] private int maxBounceBackTime = 3;
    private int currentBounceBackTime = -1;
    
    [SerializeField] private float flySpeed = 25f;
    [SerializeField] private float reverseSpeed = 20f;

    private Vector3 startDirection;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startDirection = (PlayerProperty.playerPosition - transform.position).normalized;
        rb.velocity = startDirection * flySpeed;
    }

    private void FixedUpdate()
    {
        Vector3 masterDirection = (master.transform.position - transform.position).normalized;
        rb.AddForce(masterDirection*reverseSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == master)
        {
            currentBounceBackTime++;
            if (currentBounceBackTime >= maxBounceBackTime)
            {
                Destroy(gameObject);
            }
        }

        if (other.gameObject == PlayerProperty.player)
        {
            PlayerProperty.playerClass.TakeDamage(5);
            PlayerProperty.playerClass.GetKnockOff(transform.position);
        }
    }
}
