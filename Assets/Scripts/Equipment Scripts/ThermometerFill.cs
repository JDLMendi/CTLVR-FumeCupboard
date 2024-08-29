using UnityEngine;
using UltimateXR.Avatar;
using UltimateXR.Haptics;
using UltimateXR.Manipulation;

public class ThermometerFill : MonoBehaviour
{
    public GameObject liquidObject;

    Material objectMaterial;
    float progressBorder;
    float temperature;

    private float minTemp = 0f;
    private float maxTemp = 100f;
    private bool inLiquid = false;

    void Start()
    {
        objectMaterial = this.GetComponent<Renderer>().material;
        objectMaterial.SetFloat("_ProgressBorder", 0.009f);
    }

    void Update()
    {
        UpdateThermometer();
        Debug.Log(inLiquid);
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
    public float GetTemperature() {
        return temperature;
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

    public bool InsideLiquid()
    {
        return inLiquid;
    }

    public void OnTriggerEnter(Collider other) {
        Debug.Log("ENTER TRIGGER");
        if (other.gameObject == liquidObject) {
            inLiquid = true;
            UxrAvatar.LocalAvatar.ControllerInput.SendGrabbableHapticFeedback(this.gameObject.GetComponent<UxrGrabbableObject>(), UxrHapticClipType.RumbleFreqNormal);
        }
    }

    public void OnTriggerExit(Collider other) {
        Debug.Log("EXIT TRIGGER");
        if (other.gameObject == liquidObject) {
            inLiquid = false;
            UxrAvatar.LocalAvatar.ControllerInput.SendGrabbableHapticFeedback(this.gameObject.GetComponent<UxrGrabbableObject>(), UxrHapticClipType.RumbleFreqNormal);
        }
    }
}