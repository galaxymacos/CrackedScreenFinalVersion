using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannotSeeInGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = false;
        }
    }
}
