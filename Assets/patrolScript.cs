using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class patrolScript : MonoBehaviour
{
    public Transform[] directPoints; 

    private int index = 0;

    public float patroTime = 3f;

    private float timer = 0;



    private NavMeshAgent navMeshAgent;

 

 

void Awake()

    {

        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.destination = directPoints[index].position;

    }



    void Update()

    {

        if (navMeshAgent.remainingDistance < 0.5f)

        {

            timer += Time.deltaTime;

            if (timer == patroTime)

            {

                index++;

                index %= 4;

                timer = 0;

                navMeshAgent.destination = directPoints[index].position;

            }

        }

    }



}

