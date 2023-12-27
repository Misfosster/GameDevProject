using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] GameObject player;
    Vector3 vectorPoint;

    SphereController ms;

    private void Start(){
        ms = player.GetComponent<SphereController>();
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            ms.SetCheckpointPosition(transform.position);
        }
    }
}
