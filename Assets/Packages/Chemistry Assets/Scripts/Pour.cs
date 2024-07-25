using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pour : MonoBehaviour
{
    public ParticleSystem liquid;
    [Range(0f, 180f)]
    public float thresholdAngle = 90;

    private ParticleSystem.EmissionModule liquidEmission;
    private ParticleSystem.MainModule liquidMain;
    public GameObject colorSourceObject; // Reference to the other GameObject
    private Material sourceMaterial;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the particle system components
        liquid = GetComponent<ParticleSystem>();
        liquidEmission = liquid.emission;
        liquidMain = liquid.main;

        // Get the source material from the colorSourceObject
        if (colorSourceObject != null)
        {
            Renderer renderer = colorSourceObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                sourceMaterial = renderer.material;
                if (sourceMaterial.HasProperty("_Colour")) // Ensure the property exists
                {
                    Color topColor = sourceMaterial.GetColor("_Colour");
                    // Set the particle color to the material's top color
                    liquidMain.startColor = topColor;
                }
                else
                {
                    Debug.LogWarning("The material does not have a property called '_Colour'.");
                }
            }
        }

        // Ensure the particle system has a valid material assigned with a particle shader
        Renderer particleRenderer = liquid.GetComponent<Renderer>();
        if (particleRenderer != null)
        {
            if (particleRenderer.material == null)
            {
                // Create a new material with a particle shader
                particleRenderer.material = new Material(Shader.Find("Particles/Standard Unlit"));
            }
            else
            {
                // Ensure the existing material uses a particle shader
                particleRenderer.material.shader = Shader.Find("Particles/Standard Unlit");
            }

            // Set the color of the particle system material to match the top color
            if (sourceMaterial.HasProperty("_Colour"))
            {
                particleRenderer.material.color = sourceMaterial.GetColor("_Colour");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (liquid != null)
        {
            Vector3 rotation = transform.forward;

            if (Vector3.Angle(Vector3.down, transform.forward) <= thresholdAngle)
            {
                liquidEmission.enabled = true;
            }
            else
            {
                liquidEmission.enabled = false;
            }
        }
    }
}
