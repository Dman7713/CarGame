using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target the camera will follow (car's center)
    public float smoothing = 5.0f; // Smooth transition speed

    private Vector3 offset; // Offset from the target (car)

    void Start()
    {
        // Initialize the offset based on the initial target position
        if (target != null)
        {
            offset = transform.position - target.position;
        }
    }

    void Update()
    {
        if (target != null)
        {
            // Smoothly move the camera to follow the car's position
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothing * Time.deltaTime);
        }
    }
}
