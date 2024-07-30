using UnityEngine;

public class EnableTooltip : EnableOutline
{
    [SerializeField] private GameObject tooltip;

    private void Awake()
    {
        // Find the 'Tooltip' GameObject under the same parent
        tooltip = transform.parent.Find("Tooltip").gameObject;

        if (tooltip == null)
        {
            Debug.LogError("Tooltip GameObject not found under the same parent.");
        }
    }

    private void OnEnable()
    {
        if (outline != null)
        {
            outline.enabled = true;
            tooltip.SetActive(true);
        }
    }

    private void OnDisable()
    {
        if (outline != null)
        {
            outline.enabled = false;
            tooltip.SetActive(false);
        }
    }
}
