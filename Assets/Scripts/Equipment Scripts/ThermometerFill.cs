using UnityEngine;

public class ThermometerFill : MonoBehaviour
{
    Material objectMaterial;
    float progressBorder;
    float temperature;
    public float minTemp = 0f;
    public float maxTemp = 100f;

    void Start()
    {
        objectMaterial = this.GetComponent<Renderer>().material;
        objectMaterial.SetFloat("_ProgressBorder", 0.009f);
    }

    void Update()
    {
        UpdateThermometer();
    }

    void UpdateThermometer()
    {
        // Progess border range: -0.005 to 0.010
        progressBorder = (0.015f * ((temperature - minTemp) / (maxTemp - minTemp))) - 0.005f;
        objectMaterial.SetFloat("_ProgressBorder", progressBorder);
    }

    public void SetTemperature(float newTemp)
    {
        temperature = Mathf.Clamp(newTemp, minTemp, maxTemp);
    }

    // These methods can be kept for manual control if needed
    public void increaseTemp()
    {
        temperature = Mathf.Min(maxTemp, temperature + 0.25f);
    }

    public void decreaseTemp()
    {
        temperature = Mathf.Max(minTemp, temperature - 0.25f);
    }
}