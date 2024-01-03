using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySphere : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    ScoreManager scoreManager;
    private bool canInteract = true;

    private void Start()
    {
        scoreManager = canvas.GetComponent<ScoreManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canInteract && other.CompareTag("Player"))
        {
            // Get the SphereController component from the player
            SphereController sphereController = other.GetComponent<SphereController>();
            if (sphereController != null)
            {
                sphereController.Energy += 50;
                scoreManager.addScore();
            }

            // Disable the object for 20 seconds
            StartCoroutine(DisableForDuration(20f));
        }
    }

    private IEnumerator DisableForDuration(float duration)
    {
        canInteract = false;

        // EnergySphere disappears
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }

        yield return new WaitForSeconds(duration);

        // Re-enable the object after the specified duration
        canInteract = true;

        // EnergySphere reappears
        if (renderer != null)
        {
            renderer.enabled = true;
        }
    }
}