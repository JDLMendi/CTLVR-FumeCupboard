// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class ChangeColour : MonoBehaviour
// {
//     private Renderer objectRenderer;

//     void Start()
//     {
//         objectRenderer = GetComponent<Renderer>();
//     }

//     void OnParticleCollision(GameObject other)
//     {
//         // Get the ParticleSystem component from the colliding object
//         ParticleSystem particleSystem = other.GetComponent<ParticleSystem>();

//         if (particleSystem != null)
//         {
//             // Get the main module of the ParticleSystem
//             ParticleSystem.MainModule mainModule = particleSystem.main;
//             // Change the color of the object to the start color of the particle system
//             objectRenderer.material.color = mainModule.startColor.color;
//         }
//     }
// }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColour : MonoBehaviour
{
    private Renderer objectRenderer;
    private Color currentColor;
    private int collisionCount;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        objectRenderer.material.color = Color.white;
        currentColor = objectRenderer.material.color;
        collisionCount = 0;
    }

    void OnParticleCollision(GameObject other)
    {
        // Get the ParticleSystem component from the colliding object
        ParticleSystem particleSystem = other.GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            // Get the main module of the ParticleSystem
            ParticleSystem.MainModule mainModule = particleSystem.main;
            // Get the start color of the particle system
            Color particleColor = mainModule.startColor.color;

            // Blend the current color with the new particle color
            BlendColors(particleColor);
        }
    }

    void BlendColors(Color newColor)
    {
        // Increment the collision count
        collisionCount++;

        // Calculate the new blended color
        currentColor = Color.Lerp(currentColor, newColor, 1f / collisionCount);

        // Apply the new blended color to the object
        objectRenderer.material.color = currentColor;
    }
}
