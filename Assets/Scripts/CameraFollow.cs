using UnityEngine;

[RequireComponent(typeof(CameraEffect))]
public class CameraFollow : MonoBehaviour
{
    public Vector3 offsetIn2D = new Vector3(0,0,-10);
    public Vector3 offsetIn3D = new Vector3(0,10,-10);
    public Vector3 rotationIn2D = new Vector3(0,0,0);
    public Vector3 rotationIn3D = new Vector3(30,0,0);
    private Vector3 positionOffset;
    private Vector3 rotationOffset;
    internal Vector3 center;
    [SerializeField] private float smoothValueForSwitchDimension = 0.1f;
    [SerializeField] private GameObject player;
    public float horizontalLimit = 5f;
    public float verticalLimit = 2f;
    private CameraEffect cameraEffect;
    

    private void Start()
    {
        center = player.transform.position;
        GameManager.Instance.OnSceneChangeCallback += RotateCamera;
        cameraEffect = GetComponent<CameraEffect>();
        positionOffset = offsetIn2D;
        rotationOffset = transform.rotation.eulerAngles;
    }

    private void Update()
    {
        if (GameManager.Instance.is3D)
        {
            positionOffset = Vector3.Lerp(positionOffset, offsetIn3D, smoothValueForSwitchDimension);
            if (positionOffset == offsetIn3D)
            {
                rotationOffset = Vector3.Lerp(rotationOffset,rotationIn3D,smoothValueForSwitchDimension);
            }
        }
        else
        {
            positionOffset = Vector3.Lerp(positionOffset, offsetIn2D, smoothValueForSwitchDimension);
            if (positionOffset == offsetIn2D)
            {
                rotationOffset = Vector3.Lerp(rotationOffset,rotationIn2D,smoothValueForSwitchDimension);
            }
        }
        
    }

    private void LateUpdate()
    {
//        if (!cameraEffect.isPlayingCameraEffect())
//        {
            if (player.transform.position.x > center.x + horizontalLimit)
                center = new Vector3(player.transform.position.x - horizontalLimit,center.y,center.z);
            else if (player.transform.position.x < center.x - horizontalLimit)
                center = new Vector3(player.transform.position.x+horizontalLimit,center.y,center.z);
            if (player.transform.position.y < center.y - verticalLimit)
                center = new Vector3(center.x,player.transform.position.y+verticalLimit,center.z);
            else if (player.transform.position.y > center.y + verticalLimit)
                center = new Vector3(center.x,player.transform.position.y-verticalLimit,center.z);
            
            transform.position = cameraEffect.Play(center + positionOffset);
            transform.rotation = Quaternion.Euler(rotationOffset);
//        }
    }

    private void RotateCamera(bool is3D)
    {
//        transform.Rotate(is3D?20:-20,0,0);
    }
}