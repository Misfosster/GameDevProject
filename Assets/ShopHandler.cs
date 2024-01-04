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
    if (Input.GetKeyDown(KeyCode.H) && (sm.getTotal() >= 200f))
    {   
        sm.subtractTotal(200f);
        ChangeMaterial();
    }
}

    private void ChangeMaterial()
    {
        if (newMaterial != null)
        {
            // Material change
            Renderer renderer = connectedObject.GetComponent<Renderer>();

           
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
