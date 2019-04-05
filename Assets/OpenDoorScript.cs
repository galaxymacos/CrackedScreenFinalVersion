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

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {

            if (Input.GetKey(KeyCode.Space))
            {
                Debug.Log("Open");
                door.Play("open");
            }
        
        }
    }

}
