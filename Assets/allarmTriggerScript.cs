using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class allarmTriggerScript : MonoBehaviour
{
    private AudioSource souce;
    [SerializeField] GameObject timerUI;
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            souce.Play();
            timerUI.SetActive(true);
        }
    }

    private void Start()
    {
        souce = GetComponent<AudioSource>();
    }
}
