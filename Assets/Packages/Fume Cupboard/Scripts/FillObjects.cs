using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillObjects : MonoBehaviour
{
    GameObject pipetteLiquid;

    void Start() {
        pipetteLiquid = GameObject.Find("PipetteLiquid");
    }

    void OnParticleCollision(GameObject other) {
        if (other.name == "Pipette") {
            pipetteLiquid.GetComponent<PipetteFill>().fillUp();
        }
    }
}
