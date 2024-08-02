using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowControl : MonoBehaviour
{
    public ParticleSystem emitter;
    public Transform dial;


    void Update()
    {
        float dialRotation = dial.localRotation.eulerAngles.z / 360; // Between 0 (fully shut) and 1 (fully open)
        // Debug.Log(dialRotation);
        var flowRate = emitter.emission;
        flowRate.rateOverTime = 10 * dialRotation;
    }
}
