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
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Move forward
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }

        // Move backward
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }

        // Move left
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }

        // Move right
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }

        // Turn left
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.down * turnSpeed * Time.deltaTime);
        }

        // Turn right
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
        }

        // Jump
    if (Input.GetKeyDown(KeyCode.Space))
    {
        if (timeSinceJump > jumpCooldown)
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            timeSinceJump = 0f; // Reset the cooldown timer
        }
    }

    timeSinceJump += Time.deltaTime;
    }    
    
}