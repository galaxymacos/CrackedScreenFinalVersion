using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fencesDeathCheck : MonoBehaviour
{
    [SerializeField] Transform restartPlace;
    [SerializeField] GameObject Charactor;
    // Update is called once per frame
    void Update()
    {
        if (fenceScript.death)
        {
            Charactor.transform.position = restartPlace.position;
            if (Charactor.transform.position == restartPlace.position)
            {
                fenceScript.death = false;
            }
        }
    }
}
