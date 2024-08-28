using UnityEngine;
using UltimateXR.Avatar;
using UltimateXR.Haptics;
using UltimateXR.Manipulation;

public class SpoonPelletInteractionController : MonoBehaviour
{
    public GameObject spoonObject;
    public GameObject pelletsContainer;
    public GameObject petriDishObject;
    public GameObject liquidObject;
    public ThermometerFill thermometer;
    public float timerDuration = 120f;
    public float startTemperature = 20f;
    public float endTemperature = 40f;
    public float temperatureDecreaseRate = 2f; // Degrees per second

    private bool pelletsVisible = false;
    private bool timerRunning = false;
    private bool isInLiquid = false;
    private bool timerSpeedIncreased = false; // New variable to track if the timer speed should be increased
    private float currentTemperature;
    private float elapsedTime = 0f;

    private void Start()
    {
        if (pelletsContainer != null)
        {
            pelletsContainer.SetActive(false);
        }
        else
        {
            Debug.LogError("Pellets container is not assigned!");
        }
        currentTemperature = startTemperature;
        UpdateThermometer();
    }

    private void Update()
    {
        if (timerRunning)
        {
            UpdateTimer();
        }
        else if (!isInLiquid && currentTemperature > startTemperature)
        {
            DecreaseTemperature();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == petriDishObject)
        {
            if (pelletsContainer != null)
            {
                pelletsContainer.SetActive(true);
                pelletsVisible = true;
                Debug.Log("Pellets revealed");
            }
        }
        else if (other.gameObject == liquidObject)
        {
            isInLiquid = true;
            if (pelletsVisible)
            {
                DestroyPellets();
                if (!timerRunning)
                {
                    StartTimer();
                    Debug.Log("Pellets destroyed and timer started");
                }
                else
                {
                    timerSpeedIncreased = true; // Set flag if pellets are destroyed again
                    Debug.Log("Pellets destroyed again, timer will run twice as fast");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == liquidObject)
        {
            isInLiquid = false;
            // Timer continues even if the spoon exits the liquid, so no action is needed here.
        }
    }

    private void DestroyPellets()
    {
        if (pelletsContainer != null)
        {
            UxrAvatar.LocalAvatar.ControllerInput.SendGrabbableHapticFeedback(this.gameObject.GetComponent<UxrGrabbableObject>(), UxrHapticClipType.RumbleFreqNormal);
            pelletsContainer.SetActive(false);
            pelletsVisible = false;
        }
    }

    private void StartTimer()
    {
        timerRunning = true;
        elapsedTime = 0f;
        timerSpeedIncreased = false; // Reset the speed increase flag when starting a new timer
    }

    private void UpdateTimer()
    {
        float speedMultiplier = timerSpeedIncreased ? 2f : 1f; // Double the speed if the flag is set

        elapsedTime += Time.deltaTime * speedMultiplier;
        if (elapsedTime >= timerDuration)
        {
            timerRunning = false;
            currentTemperature = endTemperature;
            Debug.Log("Timer finished. Final temperature: " + currentTemperature);
        }
        else
        {
            float progress = elapsedTime / timerDuration;
            currentTemperature = Mathf.Lerp(startTemperature, endTemperature, progress);
            Debug.Log("Current temperature: " + currentTemperature);
        }
        UpdateThermometer();
    }

    private void DecreaseTemperature()
    {
        currentTemperature -= temperatureDecreaseRate * Time.deltaTime;
        currentTemperature = Mathf.Max(currentTemperature, startTemperature);
        Debug.Log("Temperature decreasing. Current temperature: " + currentTemperature);
        UpdateThermometer();
    }

    private void UpdateThermometer()
    {
        if (thermometer != null)
        {
            thermometer.SetTemperature(currentTemperature);
        }
    }
}
