using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour{
    private NavMeshAgent navMeshAgent;
    public Transform Player;
    private float aggroRange = 23f; 
    
    SphereController pm; // Reference to the player's Movement script

    void Start(){
        navMeshAgent = GetComponent<NavMeshAgent>();
        pm = Player.GetComponent<SphereController>(); // Get the player's Movement script
    }

    void Update(){
        navMeshAgent.SetDestination(Player.position);
        if (Vector3.Distance(transform.position, Player.position) < aggroRange){
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(Player.position);
        }else{
            navMeshAgent.isStopped = true;
        }
    }

    // Call this method when the enemy collides with the player
    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 checkpointPosition = transform.position; // Get enemy position as a checkpoint
            pm.SetCheckpointPosition(checkpointPosition);
        }
    }
}
