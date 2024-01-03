using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject canvas;
    Vector3 vectorPoint;

    SphereController ms;
    ScoreManager sm;

    private void Start(){
        ms = player.GetComponent<SphereController>();
        sm = canvas.GetComponent<ScoreManager>();

    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            sm.addTotal();
            sm.scoreReset();

            ms.SetCheckpointPosition(transform.position);


        }
    }
}