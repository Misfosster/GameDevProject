using UnityEngine;

public class ShopHandler : MonoBehaviour
{
    [SerializeField] GameObject canvas;

    [SerializeField]
    private GameObject connectedObject;

    [SerializeField]
    private Material newMaterial;
    ScoreManager sm;
    private void Start()
    {
        sm = canvas.GetComponent<ScoreManager>();
        // Ensure that the connectedObject and newMaterial are not null before using them
        if (connectedObject == null)
        {
            Debug.LogError("Connected Object is not assigned in ShopHandler.");
        }

        if (newMaterial == null)
        {
            Debug.LogError("New Material is not assigned in ShopHandler.");
        }
    }

    private void Update()
{
    // Check for H key press and total is greater than or equal to 200
    if (Input.GetKeyDown(KeyCode.H) && (sm.getTotal() >= 200f))
    {   
        sm.subtractTotal(100f);
        ChangeMaterial();
    }
}

    private void ChangeMaterial()
    {
        // Ensure that the newMaterial is not null before changing the material
        if (newMaterial != null)
        {
            // Change the material of the connectedObject
            Renderer renderer = connectedObject.GetComponent<Renderer>();

            // Check if the connectedObject has a Renderer component
            if (renderer != null)
            {
                renderer.material = newMaterial;
            }
            else
            {
                Debug.LogError("Connected Object does not have a Renderer component.");
            }
        }
        else
        {
            Debug.LogError("New Material is not assigned. Please assign a material in the inspector.");
        }
    }
}
