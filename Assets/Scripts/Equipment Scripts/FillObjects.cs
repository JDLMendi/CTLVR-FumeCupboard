using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillObjects : MonoBehaviour
{
    void OnParticleCollision(GameObject other) {
        Debug.Log($"Collided with something {other.name}");
        switch (other.name) {
            case "Pipette":
                other.transform.Find("PipetteLiquid").GetComponent<PipetteFill>().fillUp();
                break;
            case "Liquid":
                Debug.Log("Collided with a container");
                other.GetComponent<LiquidControl>().Fill();
                break;
            default:
                break;
        }
    }
}
