using System.Collections;
using UnityEngine;

public class PlayerTransformation : MonoBehaviour
{
    public Mesh[] playerMeshes;
    public Material transformationMaterial;

    private MeshFilter meshFilter;
    private Renderer renderer;
    private Material originalMaterial;
    private Mesh originalMesh;
    private Vector3 originalScale;
    private bool isTransforming = false;
    private float transformationStartTime;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        meshFilter = GetComponent<MeshFilter>();
        renderer = GetComponent<Renderer>();
        originalMaterial = renderer.material;
        originalMesh = meshFilter.sharedMesh;
        originalScale = transform.localScale;
    }

    
    public void TriggerTransformation()
    {
    if (!isTransforming)
    {
        StartCoroutine(ScaleOverTime(0.1f, 1f));
        TransformPlayer();
        transformationStartTime = Time.time;
        isTransforming = true;

        // Set the transformation state in SphereController
        SphereController controller = GetComponent<SphereController>();
        if (controller != null)
        {
            controller.SetTransformationState(true);
        }
    }
    }
    void Update()
    {
        if (isTransforming && Time.time - transformationStartTime >= 20f)
        {
            RevertTransformation();
        }
    }

    private void TransformPlayer()
    {
        if (transformationMaterial != null && playerMeshes.Length > 0)
        {
            renderer.material = transformationMaterial;
            meshFilter.mesh = GetNextMesh();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
        else
        {
            Debug.LogWarning("Transformation material or player meshes are not assigned in the Inspector.");
        }
    }

    private Mesh GetNextMesh()
    {
        int currentIndex = System.Array.IndexOf(playerMeshes, meshFilter.sharedMesh);
        int nextIndex = (currentIndex + 1) % playerMeshes.Length;
        return playerMeshes[nextIndex];
    }


    private void RevertTransformation()
{
    StartCoroutine(ScaleOverTime(originalScale.x, 1f));
    renderer.material = originalMaterial;
    meshFilter.mesh = originalMesh;
    rb.useGravity = true;
    rb.constraints = RigidbodyConstraints.None;
    isTransforming = false;

    // Reset the transformation state in SphereController
    SphereController controller = GetComponent<SphereController>();
    if (controller != null)
    {
        controller.SetTransformationState(false);
    }
}


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
}