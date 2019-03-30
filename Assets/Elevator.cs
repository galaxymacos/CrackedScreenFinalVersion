using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    private CameraEffect _cameraEffect;

    private EnemyDetector _enemyDetector;

    private bool hasInteracted;
    // Start is called before the first frame update
    void Start()
    {
        if (Camera.main != null) _cameraEffect = Camera.main.GetComponent<CameraEffect>();
        _enemyDetector = GetComponent<EnemyDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyDetector.playerInRange() && !hasInteracted)
        {
            print("Try to shake the camera");
            _cameraEffect.ShakeForSeconds(0.5f);
            hasInteracted = true;

        }
    }
}
