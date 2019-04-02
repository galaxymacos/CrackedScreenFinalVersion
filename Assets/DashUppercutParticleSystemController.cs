using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashUppercutParticleSystemController : MonoBehaviour
{
    [SerializeField] private GameObject dashUppercutParticleEffectRight;

    private float timeToDisappear = 0.55f;
    private Vector3 rotationLeft = new Vector3(20.2f, -9.5f,-114.7f);
    private Vector3 rotationRight = new Vector3(-196.029f,22.301f,-41.123f);
    private float timeRemainToDisappear;

    // Update is called once per frame
    void Update()
    {
        if (timeRemainToDisappear > 0)
        {
            timeRemainToDisappear -= Time.deltaTime;
            if (timeRemainToDisappear <= 0)
            {
                dashUppercutParticleEffectRight.SetActive(false);
            }
        }
    }

    public void EnableDashUppercutParticleEffect()
    {
        if (PlayerProperty.controller.isFacingRight)
        {
            dashUppercutParticleEffectRight.transform.localRotation = Quaternion.Euler(rotationRight);
        }
        else
        {
            dashUppercutParticleEffectRight.transform.localRotation = Quaternion.Euler(rotationLeft);
        }
        dashUppercutParticleEffectRight.SetActive(true);

        timeRemainToDisappear = timeToDisappear;
    }
}
