using UnityEngine;

public class Wall : MonoBehaviour
{
    public int wallID = 1; // Unique identifier for the wall

    // Called when the wall should be removed
    public void RemoveWall()
    {
        // Optionally, you can perform any other actions or effects here
        Destroy(gameObject);
    }
}
