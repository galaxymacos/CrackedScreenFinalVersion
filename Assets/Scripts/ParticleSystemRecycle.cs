using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemRecycle : MonoBehaviour
{
    public void Recycle()
    {
        Destroy(gameObject);
    }
}
