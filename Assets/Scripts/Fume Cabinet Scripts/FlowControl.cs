using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowControl : MonoBehaviour
{
    public ParticleSystem emitter;
    public Transform dial;
    public AudioSource flowAudio;

    void Start() {
        flowAudio = GetComponent<AudioSource>();
        flowAudio.volume = 0.0f;
    }

    void Update()
    {
        float dialRotation = dial.localRotation.eulerAngles.z / 360; // Between 0 (fully shut) and 1 (fully open)
        // Debug.Log(dialRotation);
        var flowRate = emitter.emission;
        flowRate.rateOverTime = 10 * dialRotation;
        flowAudio.volume = Mathf.Min(dialRotation,0.8f);
    }
}
