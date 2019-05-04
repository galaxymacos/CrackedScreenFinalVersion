using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VerticalDirection
{
    Up,
    Down
}
public class UpDownPanel : MonoBehaviour
{

    
    [SerializeField] private float speed = 5f;

    private bool movingUp;

    private VerticalDirection currentDirection;
    [SerializeField] private float timeForSwitchingDirection = 2f;
    private float switchTimeRemain;

    private bool playerInRange;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        switchTimeRemain = timeForSwitchingDirection;
        
    }

    // Update is called once per frame
    void Update()
    {
        switchTimeRemain -= Time.deltaTime;
        if (switchTimeRemain <= 0)
        {
            switchTimeRemain = timeForSwitchingDirection;
            if (currentDirection == VerticalDirection.Up)
            {
                currentDirection = VerticalDirection.Down;
            }
            else
            {
                currentDirection = VerticalDirection.Up;
            }
        }
    }

    private void FixedUpdate()
    {
        if (currentDirection == VerticalDirection.Up)
        {
            rb.velocity = new Vector3(0,speed,0);
        }
        else
        {
            rb.velocity = new Vector3(0,-speed,0);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
    }
    
    private void OnCollisionExit(Collision other)
    {
    }
}
