using UnityEngine;
public class PlayerTransformation : MonoBehaviour

{
    public Mesh[] playerMeshes;         // Assign the different meshes in the Inspector
    public Material transformationMaterial; // Assign the material you want to transform into in the Inspector

    private MeshFilter meshFilter;       // Reference to the MeshFilter component
    private Renderer renderer;           // Reference to the Renderer component
    private Material originalMaterial;    // To store the original material
    private Mesh originalMesh;           // To store the original mesh

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        renderer = GetComponent<Renderer>();

        // Store the original material and mesh of the player object
        originalMaterial = renderer.material;
        originalMesh = meshFilter.sharedMesh;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TransformPlayer();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            RevertTransformation();
        }
    }

    void TransformPlayer()
    {
        if (transformationMaterial != null && playerMeshes.Length > 0)
        {
            // Change the material of the player object
            renderer.material = transformationMaterial;

            // Change the mesh of the player object
            meshFilter.mesh = GetNextMesh();
        }
        else
        {
            Debug.LogWarning("Transformation material or player meshes are not assigned in the Inspector.");
        }
    }

    Mesh GetNextMesh()
    {
        // Find the current mesh index
        int currentIndex = System.Array.IndexOf(playerMeshes, meshFilter.sharedMesh);

        // Get the next mesh in the array
        int nextIndex = (currentIndex + 1) % playerMeshes.Length;

        // Return the next mesh
        return playerMeshes[nextIndex];
    }

    // To revert the transformation and restore the original material and mesh
    void RevertTransformation()
    {
        renderer.material = originalMaterial;
        meshFilter.mesh = originalMesh;
    }
}
