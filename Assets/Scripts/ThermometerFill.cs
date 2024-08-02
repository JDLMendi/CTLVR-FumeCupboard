using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermometerFill : MonoBehaviour
{
    Material objectMaterial;
    float progressBorder;
    float temperature;

    void Start()
    {
        objectMaterial = this.GetComponent<Renderer>().material;
        objectMaterial.SetFloat("_ProgressBorder", 0.009f);
    }

    void Update()
    {
        // Progess border range: -0.005 to 0.010
        progressBorder = (0.015f * (temperature / 100.0f)) - 0.005f;
        objectMaterial.SetFloat("_ProgressBorder", progressBorder);
    }

    public void increaseTemp()
    {
        temperature = Mathf.Min(100.0f, temperature + 0.25f);
    }

    public void decreaseTemp()
    {
        temperature = Mathf.Max(0.0f, temperature - 0.25f);
    }
}
