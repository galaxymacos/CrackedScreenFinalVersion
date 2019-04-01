using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class fencesDeathCheck : MonoBehaviour
{
    // Update is called once per frame

    void Update()
    {
        if (fenceScript.death)
        {
            SceneManager.LoadScene("SchoolScene");
        }
    }
}
