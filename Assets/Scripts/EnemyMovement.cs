using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour{
    private NavMeshAgent navMeshAgent;
    private UnityEngine.Vector3 spawnPosition;
    public Transform Player;
    private float aggroRange = 23f; 
    private SphereController pm; // Reference to the player's Movement script

    public static List<EnemyMovement> allEnemies = new List<EnemyMovement>();


    void Start(){
        navMeshAgent = GetComponent<NavMeshAgent>();
        pm = Player.GetComponent<SphereController>(); // Get the player's Movement script
        spawnPosition = transform.position;

        allEnemies.Add(this);
    }

    void Update(){
        navMeshAgent.SetDestination(Player.position);
        if (UnityEngine.Vector3.Distance(transform.position, Player.position) < aggroRange){
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(Player.position);
        }else{
            navMeshAgent.isStopped = true;
        }
    }

    public void ResetToSpawn(){
        foreach (EnemyMovement enemy in allEnemies){
            enemy.transform.position = enemy.spawnPosition;
        }
    }

    // Call this method when the enemy collides with the player
    private void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Player")){
            // Get the player's SphereController script
            pm = other.gameObject.GetComponent<SphereController>(); // Get the player's Movement script

            if(pm != null){
                // Get the player's checkpoint position
                UnityEngine.Vector3 checkpointPosition = pm.GetCheckpointPosition(); 
            
                // Reset the player to the retrieved checkpoint position
                pm.ResetToCheckpoint(checkpointPosition); 
                ResetToSpawn();
            } else {
                Debug.Log("SphereController not found");
            }
        }    
    }
}
