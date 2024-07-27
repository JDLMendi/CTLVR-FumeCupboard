using UnityEngine;

public class EnableTooltip : EnableOutline
{
    [SerializeField] private GameObject tooltip;

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
