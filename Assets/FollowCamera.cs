using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform targetCamera;
    public float offsetTop = 0.1f;
    private Transform parentTransform;


    // Start is called before the first frame update
    private void OnEnable()
    {
        // Get the parent transform
        parentTransform = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if (parentTransform != null && targetCamera != null)
        {
            // Set the position to be on top of the parent
            transform.position = parentTransform.position + Vector3.up * offsetTop; // Adjust the offset as needed

            // Make the object match the camera's rotation
            transform.rotation = targetCamera.rotation;
        }
    }
}
