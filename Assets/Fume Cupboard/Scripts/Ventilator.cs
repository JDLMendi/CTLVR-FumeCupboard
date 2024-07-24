using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ventilator : MonoBehaviour
{
    public float destroyDistance = 0.3f;  // Distance within which particles are destroyed
    public GameObject target;  // Reference to the target game object

    private Collider targetCollider;

    private void Start()
    {
        if (target != null)
        {
            targetCollider = target.GetComponent<Collider>();
            if (targetCollider == null)
            {
                Debug.LogError("Target does not have a Collider component.");
            }
        }
    }

    private void LateUpdate()
    {
        if (target == null || targetCollider == null)
            return;

        GameObject[] gasObjects = GameObject.FindGameObjectsWithTag("Gas");
        foreach (GameObject gasObject in gasObjects)
        {
            ParticleSystem particleSystem = gasObject.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                AttractAndDestroyParticles(particleSystem);
            }
        }
    }

    private void AttractAndDestroyParticles(ParticleSystem particleSystem)
    {
        ParticleSystem.MainModule mainModule = particleSystem.main;
        int maxParticles = mainModule.maxParticles;
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[maxParticles];
        int numParticlesAlive = particleSystem.GetParticles(particles);

        for (int i = 0; i < numParticlesAlive; i++)
        {
            // Convert particle position to world space
            Vector3 particleWorldPosition = particleSystem.transform.TransformPoint(particles[i].position);
            // Find the closest point on the target collider
            Vector3 closestPoint = targetCollider.ClosestPoint(particleWorldPosition);
            float distanceToClosestPoint = Vector3.Distance(particleWorldPosition, closestPoint);

            if (distanceToClosestPoint <= destroyDistance)
            {
                particles[i].remainingLifetime = 0;  // Destroy the particle
            }
        }

        // Apply the modified particles back to the particle system
        particleSystem.SetParticles(particles, numParticlesAlive);
    }
}
