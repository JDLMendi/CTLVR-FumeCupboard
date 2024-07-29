using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillObjects : MonoBehaviour
{
    void OnParticleCollision(GameObject other) {
        if (other.name == "Pipette") {
            other.transform.Find("PipetteLiquid").GetComponent<PipetteFill>().fillUp();
        }
    }
}
