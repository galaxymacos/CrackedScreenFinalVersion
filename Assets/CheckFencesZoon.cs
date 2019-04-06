using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFencesZoon : MonoBehaviour
{
    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {

            fenceSystemScript.GetOutOfAllFences = true;

        }
    }
}
