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
    }

    // Public method to trigger transformation
    public void TriggerTransformation()
    {
        if (!isTransforming)
        {
            StartCoroutine(ScaleOverTime(0.1f, 1f));
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

    // Reverts the player to original state
    private void RevertTransformation()
    {
        StartCoroutine(ScaleOverTime(originalScale.x, 1f));
        renderer.material = originalMaterial;
        meshFilter.mesh = originalMesh;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        isTransforming = false;

        // Update SphereController state
        UpdateSphereControllerState(false);
    }

    // Coroutine to scale the player over time
    private IEnumerator ScaleOverTime(float targetScale, float duration)
    {
        Vector3 originalScale = transform.localScale;
        Vector3 target = new Vector3(targetScale, targetScale, targetScale);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(originalScale, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = target;
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
