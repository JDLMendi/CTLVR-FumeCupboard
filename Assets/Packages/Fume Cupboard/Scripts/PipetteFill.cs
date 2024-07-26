using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipetteFill : MonoBehaviour
{
    Material objectMaterial;
    float progressBorder;
    float storedLiquid;
    ParticleSystem pipetteParticles;

    GameObject pourPivot;
    Vector3 pourPos;

    void Start() {
        objectMaterial = GetComponent<Renderer>().material;
        pipetteParticles = (ParticleSystem)FindObjectOfType(typeof(ParticleSystem));
        pourPivot = GameObject.Find("PipettePivot");
    }

    void Update() {
        progressBorder = (0.2f * (storedLiquid / 100.0f)) - 0.1f;
        objectMaterial.SetFloat("_ProgressBorder", progressBorder);
        pourPos = pourPivot.transform.position;
    }

    public void fillUp() {
        storedLiquid = Mathf.Min(100.0f, storedLiquid + 1.0f);
    }

    public void pourOut() {
        storedLiquid = Mathf.Max(0.0f, storedLiquid - 0.25f);

        var emitParams = new ParticleSystem.EmitParams();
        emitParams.position = this.transform.position;
        emitParams.velocity = new Vector3(0.0f, -1.0f, 0.0f);
        if (storedLiquid > 0.0f) {
            pipetteParticles.Emit(1);
        }
    }
}
