using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    private CameraEffect _cameraEffect;

    private EnemyDetector _enemyDetector;
    private Rigidbody rb;

    [Range(0,10)]
    [SerializeField] private float responseDelay = 1f;

    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float speedIncreaseRate = 0.5f;
    private float currentSpeed;

    [SerializeField] private float toNextLevelDelay = 3f;

    private bool elevatorHasOperated;
    // Start is called before the first frame update
    void Start()
    {
        if (Camera.main != null) _cameraEffect = Camera.main.GetComponent<CameraEffect>();
        _enemyDetector = GetComponent<EnemyDetector>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyDetector.playerInRange() && !elevatorHasOperated)
        {
            responseDelay -= Time.deltaTime;
            if (responseDelay < 0)
            {
                print("Try to shake the camera");
                _cameraEffect.ShakeForSeconds(0.5f);
                elevatorHasOperated = true;
            }
        }

        if (elevatorHasOperated)
        {
            if (currentSpeed < maxSpeed)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, speedIncreaseRate);
            }
            rb.velocity = new Vector3(0,currentSpeed,0);
        }

        if (Math.Abs(currentSpeed - maxSpeed) < 0.5f)
        {
            toNextLevelDelay -= Time.deltaTime;
            if (toNextLevelDelay <= 0)
            {
                Camera.main.GetComponent<CameraFollow>().enabled = false;

                LevelChanger.Instance.FadeToNextLevel();
            }
        }
    }
}
