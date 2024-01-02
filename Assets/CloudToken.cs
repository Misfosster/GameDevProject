using UnityEngine;

public class CloudToken : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player enters the cloud power up
        {
            // Get the PlayerTransformation component from the player GameObject
            PlayerTransformation transformation = other.GetComponent<PlayerTransformation>();
            if (transformation != null)
            {
                transformation.TriggerTransformation();
            }
        }
    }
}
