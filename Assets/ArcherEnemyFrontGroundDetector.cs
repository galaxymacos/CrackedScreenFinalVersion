using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEnemyFrontGroundDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            transform.parent.GetComponent<ArcherEnemy>().floorExistsInFront = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            transform.parent.GetComponent<ArcherEnemy>().floorExistsInFront = false;
        }
    }
}
