using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject canvas;
    [SerializeField] int id;
    Vector3 vectorPoint;

    SphereController ms;
    ScoreManager sm;

    private void Start(){
        ms = player.GetComponent<SphereController>();
        sm = canvas.GetComponent<ScoreManager>();

    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            if(id!=sm.getCheckpointId())
            {
                sm.addTotal();
            }
            sm.scoreReset();
            sm.setCheckpointId(id);
            ms.SetCheckpointPosition(transform.position);


        }
    }
}