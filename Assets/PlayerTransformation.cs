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
    private bool isFirstTransformation = true;
    private bool hasInstantScaled = false;
    private bool isAutoReverting = false; // Track if auto-reverting is in progress
    private float lastTKeyPressTime; // Record the time when "T" was last pressed
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

    void Update()
    {
        if (IsOnWater() && Input.GetKeyDown(KeyCode.T))
        {
            lastTKeyPressTime = Time.time; // Record the time when "T" is pressed

            if (isFirstTransformation && !hasInstantScaled)
            {
                transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                hasInstantScaled = true;
            }
            else
            {
                StartCoroutine(ScaleOverTime(0.1f, 1f));
            }

            TransformPlayer();
        }
        else if (isAutoReverting)
        {
            // Auto-revert 20 seconds after "T" was pressed
            float timePassed = Time.time - lastTKeyPressTime;
            if (timePassed >= 20f)
            {
                StartCoroutine(ScaleOverTime(originalScale.x, 1f));
                RevertTransformation();
                isAutoReverting = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(ScaleOverTime(originalScale.x, 1f));
            RevertTransformation();

            // Start the auto-reverting process
            isAutoReverting = true;
        }
    }

    bool IsOnWater()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                return true;
            }
        }
        return false;
    }

    void TransformPlayer()
    {
        if (transformationMaterial != null && playerMeshes.Length > 0)
        {
            renderer.material = transformationMaterial;
            meshFilter.mesh = GetNextMesh();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            }
        }
        else
        {
            Debug.LogWarning("Transformation material or player meshes are not assigned in the Inspector.");
        }
    }

    Mesh GetNextMesh()
    {
        int currentIndex = System.Array.IndexOf(playerMeshes, meshFilter.sharedMesh);
        int nextIndex = (currentIndex + 1) % playerMeshes.Length;
        return playerMeshes[nextIndex];
    }

    void RevertTransformation()
    {
        renderer.material = originalMaterial;
        meshFilter.mesh = originalMesh;
        rb.useGravity = true;
        rb.constraints &= ~RigidbodyConstraints.FreezeRotationX;
        rb.constraints &= ~RigidbodyConstraints.FreezeRotationY;
        rb.constraints &= ~RigidbodyConstraints.FreezeRotationZ;
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
