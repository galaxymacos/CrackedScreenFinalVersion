using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireDamage : MonoBehaviour
{
    [SerializeField] GameObject deathUI;

    private void OnParticleCollision(GameObject col)
    {
        if (col.gameObject.tag == "Player")
        {

            fenceScript.death = true;
            deathUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }

   
}
