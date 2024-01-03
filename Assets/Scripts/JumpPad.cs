using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpBoostMultiplier = 2.0f; // Multiplier to increase the player's jump force
    private SphereController playerController; // Reference to the player's SphereController

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player enters the jump pad
        {
            playerController = other.GetComponent<SphereController>();
            if (playerController != null)
            {
                playerController.jumpForce *= jumpBoostMultiplier; // Increase the player's jump force
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && playerController != null) // Check if the player leaves the jump pad
        {
            playerController.jumpForce /= jumpBoostMultiplier; // Reset the player's jump force by dividing it by the multiplier
            playerController = null;
        }
    }
}
