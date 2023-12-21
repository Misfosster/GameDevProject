using UnityEngine;

public class SphereController : MonoBehaviour
{
    public float acceleration = 20.0f;
    public float maxSpeed = 10.0f;
    public float cameraSensitivity = 100.0f;
    public Transform cameraTransform;
    public float jumpForce = 5.0f; // Jump force
    public LayerMask groundLayer; // Layer used to identify the ground
    public LayerMask obstacleLayer; // Layer used to identify obstacles
    public float cameraDistanceScale = 1.0f; // Adjust this value to change the camera distance


    private Rigidbody rb;
    private Vector3 cameraOffset;
    private float currentAngleHorizontal = 0.0f;
    private float currentAngleVertical = 0.0f;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (cameraTransform != null)
        {
            cameraOffset = cameraTransform.position - transform.position + 5.0f * Vector3.up;
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        RotateCamera();
        CheckGround();
        HandleJump();
    }

    void FixedUpdate()
    {
        if(isGrounded){
        MoveSphere();
        }
        WallCheck();
    }

    void MoveSphere()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Update camera vectors
        UpdateCameraVectors();

        Vector3 desiredDirection = (cameraTransform.forward * verticalInput + cameraTransform.right * horizontalInput).normalized;

        // Decompose the current velocity into parallel and perpendicular components
        Vector3 velocityParallel = Vector3.Project(rb.velocity, desiredDirection);
        Vector3 velocityPerpendicular = rb.velocity - velocityParallel;

        // Gradually reduce the perpendicular component
        float perpendicularReductionFactor = 0.05f; // Adjust this value as needed
        velocityPerpendicular *= (1 - perpendicularReductionFactor);

        // Set the new velocity
        rb.velocity = velocityParallel + velocityPerpendicular;

        // Apply force in the desired direction
        if (horizontalInput != 0 || verticalInput != 0)
        {
            Vector3 force = desiredDirection * acceleration;
            if (rb.velocity.magnitude < maxSpeed)
            {
                rb.AddForce(force, ForceMode.Acceleration);
            }
        }
    }

    void UpdateCameraVectors()
    {
        cameraTransform.forward = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        cameraTransform.right = new Vector3(cameraTransform.right.x, 0, cameraTransform.right.z).normalized;
    }

   void RotateCamera()
{
    if (cameraTransform != null)
    {
        currentAngleHorizontal += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
        currentAngleVertical -= Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
        currentAngleVertical = Mathf.Clamp(currentAngleVertical, 270f, 360f);

        // Invert the horizontal angle to position the camera on the opposite side
        Quaternion rotation = Quaternion.Euler(currentAngleVertical, currentAngleHorizontal + 180f, 0);
        rotation = Quaternion.Normalize(rotation);

        cameraTransform.position = transform.position + rotation * (cameraOffset * cameraDistanceScale);
        cameraTransform.LookAt(transform.position);
    }
}

    void CheckGround()
    {
        // Check if the sphere is on the ground
        RaycastHit hit;
        float distance = 1.7f; // Distance to check for the ground
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, out hit, distance, groundLayer);
    }

    void HandleJump()
    {
        // Jump if on the ground and Spacebar is pressed
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void WallCheck()
{
    // Check for obstacles in front of the camera
    RaycastHit hit;
    float wallCheckDistance = cameraOffset.magnitude * cameraDistanceScale; // Adjust this value as needed

    if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, wallCheckDistance, obstacleLayer))
    {
        // If an obstacle is detected, adjust the camera's position to avoid it
        cameraTransform.position = hit.point - cameraTransform.forward * 0.2f; // Adjust the offset from the obstacle
    }
}

}
