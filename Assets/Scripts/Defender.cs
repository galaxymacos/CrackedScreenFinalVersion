using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.is3D)
        {
            GetComponent<MeshCollider>().enabled = true;
        }
        else
        {
            GetComponent<MeshCollider>().enabled = false;
        }
    }
}
