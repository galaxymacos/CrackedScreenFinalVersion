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

    public float shakeIntensity = 0.4f;
    public float shakeDuration = 0.3f;
    private float originalX;
    [SerializeField] private AudioSource Dropping;
    [SerializeField] private AudioSource Shaking;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalX = transform.position.x;
    }

    private void Update()
    {
        if (dropTimeRemains > 0)
        {
            dropTimeRemains -= Time.deltaTime;
            if (dropTimeRemains <= 0)
            {
                Dropping.Play();
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

        if (hasInteracted)
        {
            if (shakeDuration > 0f)
            {
                shakeDuration -= Time.deltaTime;
                float randomX = Random.Range(-shakeIntensity, shakeIntensity);
                transform.position = new Vector3(originalX + randomX, transform.position.y, transform.position.z);
            }
            
        }

        //if (true)
        //{
        //    Instantiate(this);
        //}
        
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
            Shaking.Play();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Deadly"))
        {
            Destroy(gameObject,2f);
        }
    }
}
