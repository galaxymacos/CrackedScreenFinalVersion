using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannotSeeInGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.enabled = false;
        }
    }
}
