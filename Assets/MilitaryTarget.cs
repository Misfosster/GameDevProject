using UnityEngine;

public class MilitaryTarget : MonoBehaviour
{
    public int correspondingWallID; // Set this in the Unity editor to match the associated wall's ID
    public float interactionRange = 2f; // Adjust this value based on your desired range

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && IsPlayerInRange())
        {
            RemoveCorrespondingWall();
        }
    }

    private bool IsPlayerInRange()
    {
        float distance = Vector3.Distance(transform.position, PlayerController.Instance.transform.position);
        return distance <= interactionRange;
    }

    private void RemoveCorrespondingWall()
    {
        // Find the corresponding wall based on the assigned ID
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall"); // Assuming your walls have the "Wall" tag
        foreach (var wall in walls)
        {
            Wall wallScript = wall.GetComponent<Wall>();
            if (wallScript != null && wallScript.wallID == correspondingWallID)
            {
                wallScript.RemoveWall();
                return; // Exit the loop once the corresponding wall is found and removed
            }
        }
    }
}
