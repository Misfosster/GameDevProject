using UnityEngine;

public class WreckingBallMovement : MonoBehaviour
{
    public float swingForce = 10f; // Adjust the force to control the swinging
    public float maxSwingAngle = 30f; // Maximum angle for swinging (in degrees)

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Read input for swinging
        float horizontalInput = Input.GetAxis("Horizontal");

        // Apply force to swing the wrecking ball
        Vector3 swingForceDirection = Quaternion.Euler(0, 0, -maxSwingAngle * horizontalInput) * transform.right;
        rb.AddForce(swingForce * swingForceDirection, ForceMode.Force);
    }
}
