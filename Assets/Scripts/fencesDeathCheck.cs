using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class fencesDeathCheck : MonoBehaviour
{
    [SerializeField] GameObject deathUI;

    void Update()
    {
        if (fenceScript.death)
        {
            deathUI.SetActive(true);
            Time.timeScale = 0f;

        }
    }
}
