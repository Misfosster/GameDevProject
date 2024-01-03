using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class SphereController : MonoBehaviour
{
    // Inspector variables for custom settings
    public float acceleration = 20.0f;
    public float maxSpeed = 10.0f;
    public float cameraSensitivity = 100.0f;
    public Transform cameraTransform;
    public float jumpForce = 5.0f;
    public float jumpCooldown = 2.0f;
    public LayerMask obstacleLayer;
    public float cameraDistanceScale = 1.0f;
    public float minYPosition = -16f; // Minimum y-position before reset
    public float groundCheckDistance = 1.6f;
    public bool isGrounded = false;

    //Resourcerbar
    public Image EnergyBar;
    public float Energy, MaxEnergy;
    public float SpeedBoostCost;
    public float ChargeRate;
    private Coroutine recharge;
    public bool boosting = false;
    // Private variables for internal logic
    private Rigidbody rb;
    private Vector3 cameraOffset;
    private float currentAngleHorizontal = 0.0f;
    private float currentAngleVertical = 0.0f;
    private EnemyMovement em; // Reference to the enemy's movement script
    private bool isTransformed = false;

    // Checkpoint related variables
    public float yThreshold = -10.0f;
    private Vector3 checkpointPosition = Vector3.zero;

    // Transformation specific variables
    private float flightSpeed = 20.0f;
    private float descentSpeed = 20.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        em = GameObject.FindWithTag("Enemy").GetComponent<EnemyMovement>();

        if (cameraTransform != null)
        {
            cameraOffset = cameraTransform.position - transform.position + 5.0f * Vector3.up;
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        CheckIfGrounded();
        HandleJump();
        RotateCamera();
        HandleBoosting();
    
        if (isTransformed)
        {
            HandleTransformedMovement();
        }

        if (transform.position.y < minYPosition)
        {
            ResetToCheckpoint(checkpointPosition);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetToCheckpoint(checkpointPosition);
        }
        
        
        
        
    }

    void FixedUpdate()
    {
        MoveSphere();
        WallCheck();
        ApplyCustomDownwardAcceleration();
    }

    private void MoveSphere()
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
            
            if(boosting) force *= 2f; // Double the force if boosting

            if (rb.velocity.magnitude < maxSpeed)
            {
                rb.AddForce(force, ForceMode.Acceleration);
            }
        }
    }

    private void HandleTransformedMovement()
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

    private void HandleBoosting(){
    if(Input.GetKeyDown("left shift")){
            boosting = true;
        } else if(Input.GetKeyUp("left shift")) {
            boosting = false;
        }
        if(!boosting){
            maxSpeed = 10f;
        }
        if(boosting && Energy > 0){
            
            maxSpeed = 20f;
            
            Energy -= SpeedBoostCost * Time.deltaTime; ;
            if(Energy < 0) Energy = 0; // Prevents energy from going below 0 
            EnergyBar.fillAmount = Energy / MaxEnergy;

            if(recharge != null) StopCoroutine(recharge); // Stop recharging energy when action is performed
            recharge = StartCoroutine(RechargeEnergy()); // Start recharging energy

        }
    }
    
    private IEnumerator RechargeEnergy () {
        yield return new WaitForSeconds (1f);
        while (Energy < MaxEnergy) {
            Energy += ChargeRate / 10f;
            if(Energy > MaxEnergy) Energy = MaxEnergy; // Prevents energy from going over max
            EnergyBar.fillAmount = Energy / MaxEnergy;
            yield return new WaitForSeconds (0.1f);
        
        }
        recharge = null;
    }

    private void FlyUpwards()
    {
        rb.AddForce(Vector3.up * flightSpeed, ForceMode.Acceleration);
    }

    private void Descend()
    {
        rb.AddForce(Vector3.down * descentSpeed, ForceMode.Acceleration);
    }

    private void RotateCamera()
    {
        currentAngleHorizontal += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
        currentAngleVertical -= Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
        currentAngleVertical = Mathf.Clamp(currentAngleVertical, 270f, 360f);

        Quaternion rotation = Quaternion.Euler(currentAngleVertical, currentAngleHorizontal + 180f, 0);
        rotation = Quaternion.Normalize(rotation);

        cameraTransform.position = transform.position + rotation * (cameraOffset * cameraDistanceScale);
        cameraTransform.LookAt(transform.position);
    }

    private void UpdateCameraVectors()
{
    // Ensure the camera's forward and right vectors are horizontal (y component is zero)
    cameraTransform.forward = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
    cameraTransform.right = new Vector3(cameraTransform.right.x, 0, cameraTransform.right.z).normalized;
}


    private void HandleJump()
    {
        if (!isTransformed && Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void CheckIfGrounded()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, out hit, groundCheckDistance);
    }

    private void WallCheck()
    {
        RaycastHit hit;
        float wallCheckDistance = cameraOffset.magnitude * cameraDistanceScale;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, wallCheckDistance, obstacleLayer))
        {
            cameraTransform.position = hit.point - cameraTransform.forward * 0.2f;
        }
    }

    private void ApplyCustomDownwardAcceleration()
    {
        if (!isGrounded && !isTransformed)
        {
            float extraDownwardAcceleration = 20.0f;
            rb.AddForce(Vector3.down * extraDownwardAcceleration, ForceMode.Acceleration);
        }
    }

    public void SetCheckpointPosition(Vector3 checkpointPos)
    {
        checkpointPosition = checkpointPos;
    }

    public void ResetToCheckpoint(Vector3 checkpointPosition)
    {
        if (checkpointPosition != Vector3.zero)
        {
            transform.position = checkpointPosition;
        }
        else
        {
            Debug.LogWarning("No checkpoint set!");
        }

        if (em != null)
        {
            em.ResetToSpawn();
        }
    }

    public Vector3 GetCheckpointPosition()
    {
        return checkpointPosition;
    }

    public void SetTransformationState(bool state)
    {
        isTransformed = state;
    }

    // Additional methods can be implemented here...
}