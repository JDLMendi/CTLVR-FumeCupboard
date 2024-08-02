using UnityEngine;
using UnityEditor;
using UltimateXR.Manipulation;

[CustomEditor(typeof(CreateUxrGrab))]
public class CreateUxrGrabEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CreateUxrGrab createUxrGrab = (CreateUxrGrab)target;

        if (GUILayout.Button("Apply Script and Create Child"))
        {
            ApplyScript(createUxrGrab.gameObject);
        }
    }

    private void ApplyScript(GameObject gameObject)
    {
        Debug.Log("Creating GameObject and Attaching Scripts");
        // Apply the Outline script to the GameObject and disable it
        Outline outline = gameObject.GetComponent<Outline>();
        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>();
            outline.enabled = false;
        }


        if (gameObject.GetComponent<UxrGrabbableObject>() == null)
        {
            gameObject.AddComponent<UxrGrabbableObject>();
            Debug.Log("Remember to reference to the 'Enable Outline' GameObject.");
        }

        // Check if the "Enable Outline" child already exists
        Transform existingChild = gameObject.transform.Find("Enable Outline");

        if (existingChild == null)
        {
            // Create a new child GameObject and add the EnableOutline script
            GameObject child = new GameObject("Enable Outline");
            child.transform.parent = gameObject.transform;
            child.SetActive(false);
            EnableOutline enableOutline = child.AddComponent<EnableOutline>();
            enableOutline.Initialise(gameObject);
        }
    }

}
