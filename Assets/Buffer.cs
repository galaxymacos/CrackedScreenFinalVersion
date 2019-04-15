using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer : MonoBehaviour
{
    private float lastTouchTime;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == PlayerProperty.player)
        {
            
            PlayerProperty.player.transform.SetParent(transform.parent.Find("PlatformNode"));
            print("set player parent to platform node");
            lastTouchTime = Time.time;
            PlayerProperty.player.GetComponent<Rigidbody>().velocity = new Vector3(PlayerProperty.player.GetComponent<Rigidbody>().velocity.x,0,PlayerProperty.player.GetComponent<Rigidbody>().velocity.z);
        }
    }
    
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject == PlayerProperty.player && Time.time-lastTouchTime>0.2f)
        {
            print("set player parent to null");
            PlayerProperty.player.transform.SetParent(null);
        }
    }
}
