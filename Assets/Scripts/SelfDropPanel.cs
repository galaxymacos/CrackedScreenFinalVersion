using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDropPanel : MonoBehaviour
{
    [Tooltip("Panel drops after touching player for ? seconds")]
    [SerializeField,Range(0,10)] private float dropDelay = 2f;
    private float dropTimeRemains;
    private bool hasInteracted;
    private Rigidbody rb;
    public bool isFalling;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (dropTimeRemains > 0)
        {
            dropTimeRemains -= Time.deltaTime;
            if (dropTimeRemains <= 0)
            {
                rb.useGravity = true;
                rb.isKinematic = false;
                gameObject.layer = LayerMask.NameToLayer("Abandoned");
                foreach (Transform child in transform)
                {
                    child.gameObject.layer = LayerMask.NameToLayer("Abandoned");
                }

                isFalling = true;
            }
        }

        
    }

    private void FixedUpdate()
    {
        if (isFalling)
        {
            ApplyGravity();
        }
    }

    public void ApplyGravity()
    {
        rb.velocity-=new Vector3(0,20*Time.fixedDeltaTime,0);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == PlayerProperty.player && !hasInteracted)
        {
            print("player in range");
            dropTimeRemains = dropDelay;
            hasInteracted = true;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Deadly"))
        {
            Destroy(gameObject,2f);
        }
    }
}
