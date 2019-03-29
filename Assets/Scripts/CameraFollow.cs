using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offsetIn2D = new Vector3(0,0,-10);
    private Vector3 center;
    [SerializeField] private GameObject player;
    public float horizontalLimit = 5f;
    public float verticalLimit = 2f;

    private void Start()
    {
        center = player.transform.position;
        GameManager.Instance.OnSceneChangeCallback += RotateCamera;
    }

    private void LateUpdate()
    {
        if (player.transform.position.x > center.x + horizontalLimit)
            center = new Vector3(player.transform.position.x - horizontalLimit,center.y,center.z);
        else if (player.transform.position.x < center.x - horizontalLimit)
            center = new Vector3(player.transform.position.x+horizontalLimit,center.y,center.z);
        if (player.transform.position.y < center.y - verticalLimit)
            center = new Vector3(center.x,player.transform.position.y+verticalLimit,center.z);
        else if (player.transform.position.y > center.y + verticalLimit)
            center = new Vector3(center.x,player.transform.position.y-verticalLimit,center.z);
        transform.position = center + offsetIn2D;
    }

    private void RotateCamera(bool is3D)
    {
//        transform.Rotate(is3D?20:-20,0,0);
    }
}