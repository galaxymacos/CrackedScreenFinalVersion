using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FallingTrap : MonoBehaviour
{
    private Rigidbody rb;

    private Player playerScript;

    [SerializeField] private GameObject snowLotus;
    private bool hasInteracted;

    [SerializeField] private float delayBeforeFalling = 0.5f;
    private float crackingTimeRemains;

    [SerializeField] private AudioSource soundOfIceCracking;
    [SerializeField] private AudioSource soundOfIceFalling;
    private AudioSource audioSource;
    private bool isFalling;
    [SerializeField] private float extraForce = 10f;

    private void Start()
    {
        

        rb = GetComponent<Rigidbody>();
        playerScript = GameManager.Instance.player.GetComponent<Player>();
    }

    private void Update()
    {
        if (crackingTimeRemains > 0)
        {
            crackingTimeRemains -= Time.deltaTime;
            if (crackingTimeRemains <= 0)
            {
                rb.useGravity = true;
                isFalling = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isFalling)
        {
            rb.AddForce(new Vector3(0,-extraForce,0));
        }
    }

    public void Play()
    {
        if (soundOfIceFalling)
        {
            soundOfIceFalling.Play();
        }
        crackingTimeRemains = delayBeforeFalling;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.player)
        {
            var thisTransform = transform;
            Physics.Raycast(thisTransform.position, thisTransform.forward, out var hit);
            playerScript.GetKnockOff(hit.point);
            playerScript.TakeDamage(10);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && !hasInteracted)
        {
            if (soundOfIceCracking)
            {
                soundOfIceCracking.Play();
            }
            Instantiate(snowLotus, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
            hasInteracted = true;
        }
    }
}