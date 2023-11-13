using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    public float smoothSpeed = 0.125f;
    public bool rotateWithPlayer = true;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = player.transform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        if (rotateWithPlayer)
        {
            transform.rotation = player.transform.rotation;
        }
    }
}
