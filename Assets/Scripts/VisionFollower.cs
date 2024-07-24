using UnityEngine;

public class VisionFollower : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float distance = 3.0f;

    private bool isCentered = false;

    private void OnEnable() {
        MoveToInitialPosition();
        isCentered = false;
    }

    private void MoveToInitialPosition()
    {
        // Move to the target position instantly
        Vector3 targetPosition = FindTargetPosition();
        transform.position = targetPosition;
    }
    private void Update()
    {
        if (!isCentered)
        {
            // Find the position we need to be at
            Vector3 targetPosition = FindTargetPosition();
            Debug.Log("Target Position: " + targetPosition);

            // Move just a little bit at a time
            MoveTowards(targetPosition);
            Debug.Log("Current Position: " + transform.position);

            // Rotate to face the camera
            RotateTowardsCamera();
        }
    }

    private Vector3 FindTargetPosition()
    {
        // Let's get a position in front of the player's camera
        return cameraTransform.position + (cameraTransform.forward * distance);
    }

    private void MoveTowards(Vector3 targetPosition)
    {
        // Move towards target position
        float step = 1.5f * Time.deltaTime; // Adjust the step size for smoother movement
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }

    private void RotateTowardsCamera()
    {
        // Rotate to face the camera
        Vector3 direction = -((cameraTransform.position - transform.position).normalized);
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Adjust rotation speed as needed
    }

    private bool ReachedPosition(Vector3 targetPosition)
    {
        // Simple distance check, can be replaced if you wish
        return Vector3.Distance(targetPosition, transform.position) < 0.1f;
    }
}
