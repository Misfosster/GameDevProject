using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Transform Player;
    private float aggroRange = 15f; 
    
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
       navMeshAgent.SetDestination(Player.position); 
      if (Vector3.Distance(transform.position, Player.position) < aggroRange)

    {

        navMeshAgent.isStopped = false; 

        navMeshAgent.SetDestination(Player.position);

    }

    else

    {

        navMeshAgent.isStopped = true; 

    }
    }
}
