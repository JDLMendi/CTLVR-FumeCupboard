using UnityEngine;

public class ShakeDetector : MonoBehaviour
{
    public float shakeThreshold = 1.0f; // Set a threshold for what is considered a shake
    public bool isShaken;
    public int shakeCount;
    private Vector3 lastPosition;
    private Vector3 lastVelocity;
    private Vector3 velocity;
    private Vector3 acceleration;

    void Start()
    {
        lastPosition = transform.position;
        isShaken = false;
    }

    void Update()
    {
        // Calculate velocity based on position change
        velocity = (transform.position - lastPosition) / Time.deltaTime;

        // Calculate acceleration based on velocity change
        acceleration = (velocity - lastVelocity) / Time.deltaTime;

        // Check if the acceleration exceeds the shake threshold
        if (acceleration.magnitude > shakeThreshold)
        {
            isShaken = true;
            shakeCount++;
        } else {
            isShaken = false;
        }

        // Update the last position and last velocity for the next frame
        lastPosition = transform.position;
        lastVelocity = velocity;
    }
}
