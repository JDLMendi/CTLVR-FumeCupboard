using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumControl : MonoBehaviour
{
    public ParticleSystemForceField vacuumer;
    public Transform dial;


    void Update()
    {
        float dialRotation = dial.localRotation.eulerAngles.z / -360; // Between 0 (fully shut) and 1 (fully open)
        // Debug.Log(dial.localRotation.eulerAngles.z);
        vacuumer.gravity = 0.12f * dialRotation;
    }
}
