using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance; // Singleton instance

    private void Awake()
    {
        // Ensure there is only one instance of the PlayerController
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Add other player-related functionality as needed, such as movement, health, etc.
}
