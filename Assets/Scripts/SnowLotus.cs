using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowLotus : MonoBehaviour
{
    private float scaleMultiplier = 1f;

    [SerializeField] private float scaleIncreaseSpeed = 2f;

    [SerializeField] private float existDuration = 0.5f;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
//        AudioManager.instance.PlaySfx("SnowLotus");    // TODO find snow lotus sound
        Destroy(gameObject,existDuration);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale *= scaleMultiplier + scaleIncreaseSpeed * Time.deltaTime;
        transform.Translate(0,Time.deltaTime*1f,0);
    }
}
