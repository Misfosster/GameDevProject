using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public float interactionRange = 2f; // Adjust this value based on your desired range

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TryRemoveWall();
        }
    }

    private void TryRemoveWall()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRange);

        foreach (var hitCollider in hitColliders)
        {
            // Check if the collided object has a Wall script attached
            Wall wall = hitCollider.GetComponent<Wall>();

            if (wall != null)
            {
                // Call the RemoveWall method in the Wall script
                wall.RemoveWall();
            }
        }
    }
}
