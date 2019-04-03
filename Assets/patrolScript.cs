using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class patrolScript : MonoBehaviour
{
    [SerializeField] Light RobotLight;
    public List<Transform> wayPoints = new List<Transform>();

    private Transform targetPoint;

    private int targetPontIndex=0;
    private float minDistance = 0.1f;
    private int lastwaypointIndex;
    private float movementSpeed = 0.2f;
    private float rotationSpeed = 0.2f;
    private bool checkAttack = false;
    private float attackDuration = 2.0f;

    private void Start()
    {
        lastwaypointIndex = wayPoints.Count - 1;
        targetPoint = wayPoints[targetPontIndex];

    }

    private void Update()
    {
        if (!checkAttack)
        {
            Move();
        }
        else
        {
            Attack();
        }
        
    }

    void Attack()
    {
        attackDuration -= Time.deltaTime;
        if (attackDuration < 0)
        {
            RobotLight.color = Color.white;
            checkAttack = false;
            attackDuration = 2.0f;
        }
    }

    void Move()
    {
        float movementStep = movementSpeed + Time.deltaTime;
        float rotationStep = rotationSpeed + Time.deltaTime;

        Vector3 directionToTargets = targetPoint.position - transform.position;
        Quaternion rotationToTarget = Quaternion.LookRotation(directionToTargets);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, rotationStep);
        float distance = Vector3.Distance(transform.position, targetPoint.position);
        CheckDistanceTopoint(distance);
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, movementStep);
    }

    void CheckDistanceTopoint(float currentDistance)
    {
        if (currentDistance <= minDistance)
        {
            targetPontIndex++;
            UpdayTargetPoint();
        }
    }

    void UpdayTargetPoint()
    {
        if (targetPontIndex > lastwaypointIndex)
        {
            targetPontIndex = 0;
        }
        targetPoint = wayPoints[targetPontIndex];
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            RobotLight.color = Color.red;
            checkAttack = true;
        }
    }
}

