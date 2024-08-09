using UnityEngine;

public class SpoonPelletInteractionController : MonoBehaviour
{
    public GameObject spoonObject;
    public GameObject pelletsContainer;
    public Collider petriDishCollider;
    public Collider beakerCollider;

    private bool pelletsVisible = false;

    private void Start()
    {
        // Ensure the spoon is visible and the pellets are hidden at start
        if (spoonObject != null)
            spoonObject.SetActive(true);
        if (pelletsContainer != null)
            pelletsContainer.SetActive(false);

        // Ensure the spoon has a collider and it's set to trigger
        Collider spoonCollider = spoonObject.GetComponent<Collider>();
        if (spoonCollider == null)
        {
            spoonCollider = spoonObject.AddComponent<BoxCollider>();
        }
        spoonCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == petriDishCollider && !pelletsVisible)
        {
            ShowPellets();
        }
        else if (other == beakerCollider && pelletsVisible)
        {
            DestroyPellets();
        }
    }

    private void ShowPellets()
    {
        if (pelletsContainer != null)
        {
            pelletsContainer.SetActive(true);
            pelletsVisible = true;
            Debug.Log("Pellets revealed!");
        }
        else
        {
            Debug.LogError("Pellets container is null!");
        }
    }

    private void DestroyPellets()
    {
        if (pelletsContainer != null)
        {
            Destroy(pelletsContainer);
            pelletsVisible = false;
            Debug.Log("Pellets destroyed!");
        }
        else
        {
            Debug.LogError("Pellets container is null!");
        }
    }
}