using UnityEngine;

public class TransformationController : MonoBehaviour
{
    public GameObject cloudPrefab; // Drag your cloud prefab into this field in the Unity Inspector
    public KeyCode transformButton = KeyCode.T; // Change this to the desired key

    private GameObject currentObject;
    private Rigidbody rb;

    void Start()
    {
        // Instantiate the sphere as the initial object
        currentObject = Instantiate(gameObject, transform.position, transform.rotation);
        rb = currentObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(transformButton))
        {
            TransformToCloud();
        }
    }

    void TransformToCloud()
    {
        // Get the current sphere's Rigidbody
        Rigidbody currentRb = rb;

        // Disable the sphere's Rigidbody (optional)
        if (currentRb != null)
        {
            currentRb.velocity = Vector3.zero;
            currentRb.angularVelocity = Vector3.zero;
            currentRb.isKinematic = true;
        }

        // Destroy the visible sphere but keep the underlying object
        Destroy(currentObject);

        // Instantiate the cloud at the same position and rotation as the sphere
        currentObject = Instantiate(cloudPrefab, transform.position, transform.rotation);
        rb = currentObject.GetComponent<Rigidbody>();

        // Enable the cloud's Rigidbody (optional)
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }
}
