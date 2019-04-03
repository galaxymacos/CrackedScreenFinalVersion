using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorScript : MonoBehaviour
{
    public Animation door;

    private void Start()
    {
        door = GetComponent<Animation>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
          door.Play("open");
        }
    }
}
