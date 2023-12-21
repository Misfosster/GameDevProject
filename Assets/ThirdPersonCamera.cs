using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;             // Reference to the player's transform
    public LayerMask obstacleLayer;      // Layer mask for obstacles

    public float smoothSpeed = 10f;       // Speed at which the camera moves
    [SerializeField] private float maxDistance = 10f;
    private void LateUpdate()
    {
        if (player == null)
        {
            Debug.LogError("Player not set for the camera!");
            return;
        }

        // Calculate the desired position for the camera
        Vector3 desiredPosition = player.position - transform.forward * maxDistance;  

        // Use Raycast to check for obstacles between camera and player
        RaycastHit hit;
        if (Physics.Raycast(player.position, -transform.forward, out hit, maxDistance, obstacleLayer))
        {
            // If an obstacle is hit, adjust the desired position to avoid it
            desiredPosition = hit.point;
        }

        // Smoothly move the camera to the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Make the camera look at the player
        transform.LookAt(player);
    }
}