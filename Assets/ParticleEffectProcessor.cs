using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectProcessor : MonoBehaviour
{
    private GameObject threeDimensionParticleEffect;
    // Start is called before the first frame update
    void Start()
    {
        threeDimensionParticleEffect = transform.Find("ThreeDimensionParticleEffect").gameObject;
        GameManager.Instance.OnSceneChangeCallback += ToggleParticleEffect;
    }

    public void ToggleParticleEffect(bool is3D)
    {
        if (is3D)
        {
            threeDimensionParticleEffect.SetActive(true);
        }
        else
        {
            threeDimensionParticleEffect.SetActive(false);
        }
    }
}
