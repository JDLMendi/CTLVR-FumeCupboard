using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumControl : MonoBehaviour
{
    public float attractionForce = 10f;  // Force of attraction
    public float maxAttractionForce; // Maximum force of attraction
    public float attractionDistance = 5f;  // Distance within which particles are attracted
    public float destroyDistance = 0.3f;  // Distance within which particles are destroyed
    public Transform target;  // Reference to the target object

    public Transform dial; // Dial rotation

    private void start() {
        maxAttractionForce = attractionForce;
    }

    private void LateUpdate()
    {
        if (target == null)
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
        float dialRotation = dial.localRotation.eulerAngles.z / 360;
        attractionForce = maxAttractionForce * dialRotation;

        ParticleSystem.MainModule mainModule = particleSystem.main;
        int maxParticles = mainModule.maxParticles;
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[maxParticles];
        int numParticlesAlive = particleSystem.GetParticles(particles);

        for (int i = 0; i < numParticlesAlive; i++)
        {
            Vector3 particleWorldPosition = particleSystem.transform.TransformPoint(particles[i].position);
            float distanceToTarget = Vector3.Distance(particleWorldPosition, target.position);

            if (distanceToTarget <= destroyDistance)
            {
                particles[i].remainingLifetime = 0;  // Destroy the particle
            }
            else if (distanceToTarget <= attractionDistance)
            {
                Vector3 direction = (target.position - particleWorldPosition).normalized;
                Vector3 force = direction * attractionForce * Time.deltaTime;
                particles[i].velocity += force;
            }
        }

        particleSystem.SetParticles(particles, numParticlesAlive);
    }
}
