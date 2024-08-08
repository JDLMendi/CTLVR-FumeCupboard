using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipetteFill : MonoBehaviour
{
    public float storedLiquid;
    Material objectMaterial;
    float progressBorder;
    GameObject pipetteParticles;
    ParticleSystem pipetteParticleSystem;

    void Start() {
        objectMaterial = this.GetComponent<Renderer>().material;
        pipetteParticles = transform.Find("PipetteParticles").gameObject;
        pipetteParticleSystem = pipetteParticles.GetComponent<ParticleSystem>();
    }

    void Update() {
        progressBorder = (0.2f * (storedLiquid / 100.0f)) - 0.1f;
        objectMaterial.SetFloat("_ProgressBorder", progressBorder);
    }

    public void fillUp() {
        storedLiquid = Mathf.Min(100.0f, storedLiquid + 1.0f);
    }

    public void pourOut() {
        storedLiquid = Mathf.Max(0.0f, storedLiquid - 0.25f);
        if (storedLiquid > 0.0f) {
            pipetteParticleSystem.Emit(1);
        }
    }
}
