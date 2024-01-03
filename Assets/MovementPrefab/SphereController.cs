using UnityEngine;

public class SphereController : MonoBehaviour
{
    // Inspector variables
    public float acceleration = 20.0f;
    public float maxSpeed = 10.0f;
    public float cameraSensitivity = 100.0f;
    public Transform cameraTransform;
    public float jumpForce = 5.0f;
    public float jumpCooldown = 2.0f;
    public LayerMask obstacleLayer;
    public float cameraDistanceScale = 1.0f;

    private EnemyMovement em; // Reference to the enemy's Movement script

    public float minYPosition = -16f; // The minimum y-position before resetting to the checkpoint
    // Movement variables
    private Rigidbody rb;
    private Vector3 cameraOffset;
    private float currentAngleHorizontal = 0.0f;
    private float currentAngleVertical = 0.0f;
    private float lastJumpTime = 0.0f;
    public float groundCheckDistance = 1.6f;
    public bool isGrounded = false;

    // Transformation variables
    private float flightSpeed = 20.0f;
    private float descentSpeed = 20.0f;
    private float jumpSpeedMultiplier = 0.04f;
    private float downSpeedMultiplier = 0.04f;
    private bool isTransformed = false;


    // Checkpoint variables
    public float yThreshold = -10.0f;
    Vector3 checkpointPosition = Vector3.zero;

    void Start()
    {
        em = GameObject.FindWithTag("Enemy").GetComponent<EnemyMovement>();
        //em = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyMovement>(); // Get the enemy's Movement script
        rb = GetComponent<Rigidbody>();
        if (cameraTransform != null)
        {
            cameraOffset = cameraTransform.position - transform.position + 5.0f * Vector3.up;
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        CheckIfGrounded();
        if (isTransformed)
        {
            HandleTransformedMovement();
        }
        RotateCamera();
        HandleJump();

        if (transform.position.y < minYPosition){
            // Get the checkpoint position or any specific reset position

            // Reset to the checkpoint position
            ResetToCheckpoint(checkpointPosition);
        }

        if (Input.GetKeyDown(KeyCode.R)){
            ResetToCheckpoint(checkpointPosition);
        }
        //Reset to checkpoint if player hits enemy
    }

    void FixedUpdate()
{
    MoveSphere();
    WallCheck();
    ApplyCustomDownwardAcceleration();
}

void ApplyCustomDownwardAcceleration()
{
    if (!isGrounded && !isTransformed) // Check if the sphere is in the air
    {
        float extraDownwardAcceleration = 20.0f; // Additional downward acceleration
        rb.AddForce(Vector3.down * extraDownwardAcceleration, ForceMode.Acceleration);
    }
}



    void MoveSphere()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        UpdateCameraVectors();

        Vector3 desiredDirection = (cameraTransform.forward * verticalInput + cameraTransform.right * horizontalInput).normalized;
        Vector3 velocityParallel = Vector3.Project(rb.velocity, desiredDirection);
        Vector3 velocityPerpendicular = rb.velocity - velocityParallel;

        float perpendicularReductionFactor = 0.05f;
        velocityPerpendicular *= (1 - perpendicularReductionFactor);

        rb.velocity = velocityParallel + velocityPerpendicular;

        if (horizontalInput != 0 || verticalInput != 0)
        {
            Vector3 force = desiredDirection * acceleration;
            if (rb.velocity.magnitude < maxSpeed)
            {
                rb.AddForce(force, ForceMode.Acceleration);
            }
        }

    }
    
    void HandleTransformedMovement()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            FlyUpwards();
        }
        else if (Input.GetKey(KeyCode.X))
        {
            Descend();
        }
    }

    void FlyUpwards()
    {
        rb.AddForce(Vector3.up * flightSpeed, ForceMode.Acceleration);
    }

    void Descend()
    {
        rb.AddForce(Vector3.down * descentSpeed, ForceMode.Acceleration);
    }


    void UpdateCameraVectors()
    {
        cameraTransform.forward = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        cameraTransform.right = new Vector3(cameraTransform.right.x, 0, cameraTransform.right.z).normalized;
    }

    void RotateCamera()
    {
        currentAngleHorizontal += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
        currentAngleVertical -= Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
        currentAngleVertical = Mathf.Clamp(currentAngleVertical, 270f, 360f);

        Quaternion rotation = Quaternion.Euler(currentAngleVertical, currentAngleHorizontal + 180f, 0);
        rotation = Quaternion.Normalize(rotation);

        cameraTransform.position = transform.position + rotation * (cameraOffset * cameraDistanceScale);
        cameraTransform.LookAt(transform.position);
    }

    void HandleJump()
    {
        if (!isTransformed && Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void CheckIfGrounded()
{
    RaycastHit hit;
    // Raycast downwards from the center of the object
    isGrounded = Physics.Raycast(transform.position, -Vector3.up, out hit, groundCheckDistance);
}


    void WallCheck()
    {
        RaycastHit hit;
        float wallCheckDistance = cameraOffset.magnitude * cameraDistanceScale;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, wallCheckDistance, obstacleLayer))
        {
            cameraTransform.position = hit.point - cameraTransform.forward * 0.2f;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector3 checkpointPosition = GetCheckpointPosition();
            ResetToCheckpoint(checkpointPosition);
        }
    }
    
 // Function to set the player's position to a checkpoint
    public void SetCheckpointPosition(Vector3 checkpointPos){
        checkpointPosition = checkpointPos; // Update the checkpoint position
    }

    public void ResetToCheckpoint(Vector3 checkpointPosition){
        if(checkpointPosition != Vector3.zero){
            transform.position = checkpointPosition;
        }
        else
        {
            Debug.LogWarning("No checkpoint set!");
        }
        if(em != null){
            em.ResetToSpawn();
        }
    }

    public Vector3 GetCheckpointPosition(){
        return checkpointPosition;
    }
    public void SetTransformationState(bool state)
{
    isTransformed = state;
}

}
