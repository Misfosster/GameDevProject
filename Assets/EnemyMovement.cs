using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Transform Player;

    [SerializeField]
    private float detectionRange = 10f; // Set the detection range in the Inspector
    [SerializeField]
    private int rayCount = 36; // Number of rays to cast in a circular pattern

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Check if the player is within the detection range and in sight
        if (IsPlayerInDetectionRange())
        {
            // Set the destination to the player
            navMeshAgent.SetDestination(Player.position);
        }
        else
        {
            // Optionally, you may want to clear the destination if the player is out of range or not in sight
            navMeshAgent.ResetPath();
        }
    }

    bool IsPlayerInDetectionRange()
    {
        // Calculate the angle between each ray
        float angleIncrement = 360f / rayCount;

        // Check in a 360-degree range around the enemy
        for (int i = 0; i < rayCount; i++)
        {
            // Calculate the direction of the ray based on the current angle
            float angle = i * angleIncrement;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;

            RaycastHit hit;

            // Check if there's a clear line of sight to the player
            if (Physics.Raycast(transform.position, direction, out hit, detectionRange))
            {
                // Ensure the hit object is the player
                if (hit.collider.CompareTag("Player"))
                {
                    // Player is in line of sight
                    return true;
                }
            }
        }

        // No clear line of sight to the player
        return false;
    }
}