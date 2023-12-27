using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSpeed = 100f;
    [SerializeField] private float jumpHeight = 7f;
    private bool isGrounded;
    private Rigidbody rb;

    private float timeSinceJump = 0f;
    public float jumpCooldown = 1.5f; // Adjust the cooldown time as needed

    private float speed;
    private float boostTimer;

    private bool boosting;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = moveSpeed;
        boostTimer = 0;

    }

    void Update()
    {
        // Move forward
        if (Input.GetKey(KeyCode.W)){
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        // Move backward
        if (Input.GetKey(KeyCode.S)){
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }

        // Move left
        if (Input.GetKey(KeyCode.A)){
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }

        // Move right
        if (Input.GetKey(KeyCode.D)){
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }

        // Turn left
        if (Input.GetKey(KeyCode.Q)){
            transform.Rotate(Vector3.down * turnSpeed * Time.deltaTime);
        }

        // Turn right
        if (Input.GetKey(KeyCode.E)){
            transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space)){
            if (timeSinceJump > jumpCooldown){
                rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
                timeSinceJump = 0f; // Reset the cooldown timer
            }
        }

        timeSinceJump += Time.deltaTime;

        if (boosting){
            boostTimer += Time.deltaTime;
            if (boostTimer >= 3){
                speed = moveSpeed;
                boostTimer = 0;
                boosting = false;
            }
        }
    }
    void OnTriggerEnter(Collider other){
        if (other.tag == "SpeedBoost"){
            boosting = true;
            speed = moveSpeed * 3;
        }
    }

 // Function to set the player's position to a checkpoint
    public void SetCheckpointPosition(Vector3 checkpointPosition){
        rb.velocity = Vector3.zero; // Reset the player's velocity
        transform.position = checkpointPosition; // Set player position to checkpoint
    }

    // Function to check if the player has fallen below a certain y-position
    public void CheckYPosition(float yThreshold, Vector3 checkpointPosition){
        if (transform.position.y < yThreshold){
            SetCheckpointPosition(checkpointPosition);
        }
    }

}    
    
 