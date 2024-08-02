using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItems : MonoBehaviour
{
    GameObject tpPoint;

    void Start() {
        tpPoint = GameObject.Find("TeleportPoint");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>() != null)
        {
            other.transform.position = tpPoint.transform.position;
        }
    }
}
