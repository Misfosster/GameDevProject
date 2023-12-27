using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] GameObject player;
    Vector3 vectorPoint;

    Movement movementScript;

    private void Start(){
        movementScript = player.GetComponent<Movement>();
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            movementScript.SetCheckpointPosition(transform.position);
        }
    }
}
