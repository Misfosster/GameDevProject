using System.Collections;
using UnityEngine;

public class PlayerTransformation : MonoBehaviour
{
    // Public variables for customization
    public Mesh[] playerMeshes;
    public Material transformationMaterial;

    // Private variables for internal logic
    private MeshFilter meshFilter;
    private Renderer renderer;
    private Material originalMaterial;
    private Mesh originalMesh;
    private Vector3 originalScale;
    private bool isTransforming = false;
    private float transformationStartTime;
    private Rigidbody rb;

    // Initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        meshFilter = GetComponent<MeshFilter>();
        renderer = GetComponent<Renderer>();

        // Store original properties
        originalMaterial = renderer.material;
        originalMesh = meshFilter.sharedMesh;
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if transformation duration has elapsed
        if (isTransforming && Time.time - transformationStartTime >= 20f)
        {
            RevertTransformation();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            RevertTransformation();
        }
    }

    // Public method to trigger transformation
    public void TriggerTransformation()
    {
        if (!isTransforming)
        {
            ScaleTo(0.1f); // Instant scale to 1
            TransformPlayer();
            transformationStartTime = Time.time;
            isTransforming = true;

            // Update SphereController state
            UpdateSphereControllerState(true);
        }
    }

    // Transforms the player
    private void TransformPlayer()
    {
        if (transformationMaterial != null && playerMeshes.Length > 0)
        {
            // Apply transformation properties
            renderer.material = transformationMaterial;
            meshFilter.mesh = GetNextMesh();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            Debug.LogWarning("Transformation material or player meshes are not assigned in the Inspector.");
        }
    }

    // Reverts the player to the original state
    private void RevertTransformation()
    {
        ScaleTo(originalScale.x); // Instant scale to the original size
        renderer.material = originalMaterial;
        meshFilter.mesh = originalMesh;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        isTransforming = false;

        // Update SphereController state
        UpdateSphereControllerState(false);
    }

    // Instantly scales the player to the specified size
    private void ScaleTo(float targetScale)
    {
        transform.localScale = new Vector3(targetScale, targetScale, targetScale);
    }

    // Gets the next mesh in the array
    private Mesh GetNextMesh()
    {
        int currentIndex = System.Array.IndexOf(playerMeshes, meshFilter.sharedMesh);
        int nextIndex = (currentIndex + 1) % playerMeshes.Length;
        return playerMeshes[nextIndex];
    }

    // Updates the state in SphereController
    private void UpdateSphereControllerState(bool isTransformed)
    {
        SphereController controller = GetComponent<SphereController>();
        if (controller != null)
        {
            controller.SetTransformationState(isTransformed);
        }
    }
}

 