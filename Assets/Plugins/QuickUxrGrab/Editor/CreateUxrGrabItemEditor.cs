using UnityEngine;
using UnityEditor;
using UltimateXR.Manipulation;

[CustomEditor(typeof(CreateUxrGrabItem))]
public class CreateUxrGrabItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector GUI elements
        DrawDefaultInspector();

        // Get a reference to the CreateUxrGrabItem instance
        CreateUxrGrabItem createUxrGrabItem = (CreateUxrGrabItem)target;

        // Add a button to the inspector GUI
        if (GUILayout.Button("Apply Script and Create Child"))
        {
            // Call the ApplyScript method when the button is clicked
            ApplyScript(createUxrGrabItem);
        }
    }

    private void ApplyScript(CreateUxrGrabItem createUxrGrabItem)
    {
        Debug.Log("Creating GameObject and Attaching Scripts");

        GameObject gameObject = createUxrGrabItem.gameObject;

        // Try to get the Outline component, add it if it doesn't exist, and disable it
        Outline outline = gameObject.GetComponent<Outline>();
        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>();
            outline.enabled = false;
        }

        // Add UxrGrabbableObject component if it doesn't exist
        if (gameObject.GetComponent<UxrGrabbableObject>() == null)
        {
            gameObject.AddComponent<UxrGrabbableObject>();
            Debug.Log("Remember to reference the 'Enable Tooltip' GameObject.");
        }

        // Add UxrGrabbableObject component if it doesn't exist
        if (gameObject.GetComponent<Rigidbody>() == null)
        {
            gameObject.AddComponent<Rigidbody>();
        }


        // Check if the "Enable Tooltip" child already exists
        Transform existingChild = gameObject.transform.Find("Enable Tooltip");

        // If it doesn't exist, create the required child GameObjects
        if (existingChild == null)
        {
            // Create "Enable Outline" child and attach EnableOutline script
            GameObject child = new GameObject("Enable Outline");
            child.transform.parent = gameObject.transform;
            child.SetActive(false);
            EnableOutline enableOutline = child.AddComponent<EnableOutline>();
            enableOutline.Initialise(gameObject);

            // Create "Enable Tooltip" child and attach EnableTooltip script
            child = new GameObject("Enable Tooltip");
            child.transform.parent = gameObject.transform;
            child.SetActive(false);
            EnableTooltip enableTooltip = child.AddComponent<EnableTooltip>();
            enableTooltip.Initialise(gameObject);

            // Instantiate the Tooltip prefab as a child of the main GameObject
            if (createUxrGrabItem.tooltipPrefab != null)
            {
                GameObject tooltipInstance = Instantiate(createUxrGrabItem.tooltipPrefab, gameObject.transform);
                tooltipInstance.name = "Tooltip";
                tooltipInstance.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Tooltip prefab not assigned. Please assign it in the Inspector.");
            }
        }
    }
}
