using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{

    [SerializeField] private int damage = 10;
    [SerializeField] private float targetLocalScale = 1.5f;
    private float originalLocalScale;
    private bool hasFullyGrown;

    [SerializeField] private float existedTime = 3f;

    [SerializeField] private AudioSource tornadoGrowing;
    [SerializeField] private AudioSource tornadoHasFullyGrown;

    private float existedTimeRemains;
    // Start is called before the first frame update
    void Start()
    {
        if (tornadoGrowing)
        {
            tornadoGrowing.Play();
        }
        existedTimeRemains = existedTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x < targetLocalScale)
        {
            transform.localScale += new Vector3(Time.deltaTime,Time.deltaTime,Time.deltaTime);
        }
        else
        {
            if (!hasFullyGrown)
            {
                FinishGrowing();
            }
        }

        if (hasFullyGrown)
        {
            existedTimeRemains -= Time.deltaTime;
            if (existedTimeRemains <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void FinishGrowing()
    {
        hasFullyGrown = true;
        GetComponent<SpriteRenderer>().color = Color.white;
        if (tornadoGrowing.isPlaying)
        {
            tornadoGrowing.Stop();
        }

        if (tornadoHasFullyGrown)
        {
            tornadoHasFullyGrown.Play();
        }
        // TODO add tornado turns to full state sound
    }

 

    private void OnTriggerStay(Collider other)
    {
        if (hasFullyGrown && other.gameObject == PlayerProperty.player)
        {
            PlayerProperty.playerClass.GetKnockOff(transform.position);
            PlayerProperty.playerClass.TakeDamage(damage);
        }
    }
}
