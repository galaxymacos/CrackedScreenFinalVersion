using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraEffect : MonoBehaviour
{
    private Coroutine shakeCoroutine;
    public bool isShaking => shakeTimeRemain > 0;
    public float shakeIntensity = 0.4f;
    public float shakeIntensityIncreaseSpeed = 0.1f;
    public float shakeTimeRemain;
    public Vector3 originalPos;
    [SerializeField] private float sizeSmoothValue = 0.5f;
    private Camera _camera;
    private CameraFollow cameraFollow;
    private float originalSizeSmoothValue;
    private Camera spriteCamera;

    private void Start()
    {
        originalSizeSmoothValue = sizeSmoothValue;
        cameraFollow = GetComponent<CameraFollow>();
        originalPos = transform.localPosition;
        _camera = GetComponent<Camera>();
        destinationCameraSize = _camera.orthographicSize;
        spriteCamera = transform.Find("SpriteCamera").gameObject.GetComponent<Camera>();
    }

    /// <summary>
    /// This method is mainly used by the CameraFollow script to stop tracking enemy if camera effect is playing
    /// </summary>
    /// <returns></returns>
    public bool isPlayingCameraEffect()
    {
        return isShaking;
    }

    /// <summary>
    /// This method will be called in LateUpdate() method mainly to change the position of the camera
    /// </summary>
    /// <param name="cameraOriginalPos"></param>
    /// <returns></returns>
    public Vector3 Play(Vector3 cameraOriginalPos)
    {
        if (shakeTimeRemain > 0f)
        {
            shakeTimeRemain -= Time.deltaTime;
        }
        
                
        if (Camera.main != null && Math.Abs(Camera.main.orthographicSize - destinationCameraSize) > Mathf.Epsilon)
        {
            ChangeSizeToTargetSize();
        }
        else
        {
//            sizeSmoothValue = originalSizeSmoothValue;
        }

        if (isShaking)
        {
            shakeIntensity += shakeIntensityIncreaseSpeed * Time.deltaTime;
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;

            return new Vector3(cameraOriginalPos.x + x, cameraOriginalPos.y + y, cameraOriginalPos.z);
        }
//        
        return cameraOriginalPos;
        
        
    }

    private void ChangeSizeToTargetSize()
    {
        Camera.main.orthographicSize =
            Mathf.Lerp(Camera.main.orthographicSize, destinationCameraSize, sizeSmoothValue);
        spriteCamera.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, destinationCameraSize, sizeSmoothValue);
    }

    public void StartShaking(float shakeStartIntensity)
    {
        shakeTimeRemain = 10000f;
        
        shakeIntensity = shakeStartIntensity;

    }
    
    public void StopShaking()
    {
        shakeTimeRemain = 0f;
    }


    /// <summary>
    ///  Shake for constant time
    /// </summary>
    /// <param name="shakeTimeInSecond"></param>

    public void ShakeForSeconds(float shakeTimeInSecond)
    {
        StartShaking(shakeIntensity);
        shakeTimeRemain = shakeTimeInSecond;
    }


    private float destinationCameraSize;

    public void EnlargeCamera(float size)
    {
        destinationCameraSize = size;
    }
    
    public void EnlargeCamera(float size, float smoothValue)
    {
        destinationCameraSize = size;
        sizeSmoothValue = smoothValue;
    }

}