using UnityEngine;

public class SphereController : MonoBehaviour
{
    public float acceleration = 20.0f;
    public float maxSpeed = 10.0f;
    public float cameraSensitivity = 100.0f;
    public Transform cameraTransform;
    public float jumpForce = 5.0f;
    public float jumpCooldown = 2.0f; // Cooldown time in seconds
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;
    public float cameraDistanceScale = 1.0f;

    private Rigidbody rb;
    private Vector3 cameraOffset;
    private float currentAngleHorizontal = 0.0f;
    private float currentAngleVertical = 0.0f;
    private bool isTransformed = false;
    private bool unlocker = false;
    private float lastJumpTime = 0.0f;

    // Speed multipliers for jumping and going down
    private float jumpSpeedMultiplier = 0.04f; // Default speed multiplier
    private float downSpeedMultiplier = 0.04f; // Speed multiplier for going down

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
        if (!isTransformed)
        {
            RotateCamera();
            HandleJump();
        }

        if (Input.GetKey(KeyCode.T))
        {
            unlocker = true;
        }
        if (Input.GetKey(KeyCode.R))
        {
            unlocker = false;
        }

        if (unlocker && Input.GetKey(KeyCode.Space))
        {
            // Add an upward force with the adjusted multiplier
            rb.AddForce(Vector3.up * jumpForce * jumpSpeedMultiplier, ForceMode.Impulse);
        }

        if (unlocker && Input.GetKey(KeyCode.X))
        {
            // Add a downward force with the adjusted multiplier
            rb.AddForce(Vector3.down * jumpForce * downSpeedMultiplier, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        MoveSphere();
        WallCheck();
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

            Quaternion rotation = Quaternion.Euler(currentAngleVertical, currentAngleHorizontal + 180f, 0);
            rotation = Quaternion.Normalize(rotation);

            cameraTransform.position = transform.position + rotation * (cameraOffset * cameraDistanceScale);
            cameraTransform.LookAt(transform.position);
        }
    }

    void HandleJump()
    {
        if (!isTransformed && Input.GetKeyDown(KeyCode.Space) && (unlocker || Time.time - lastJumpTime >= jumpCooldown))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            lastJumpTime = Time.time;
        }
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
}
