using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparatoryFunnel : MonoBehaviour
{
    public LayeredLiquidController liquidController;
    public GameObject stopcock;
    private Transform _stopcockTransform;
    private float _stopcockRotation;

    private void Start() {
        _stopcockTransform = stopcock.GetComponent<Transform>();
    }

    private void Update() {
        // Print the local x rotation of the Stopcock
        float xLocalRotation = _stopcockTransform.localRotation.eulerAngles.x;
        Debug.Log("Stopcock local x rotation: " + xLocalRotation);
    }

}
