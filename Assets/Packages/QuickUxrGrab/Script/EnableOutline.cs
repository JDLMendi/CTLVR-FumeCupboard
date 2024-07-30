using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOutline : MonoBehaviour
{
    [SerializeField] private Outline outline;

    public void Initialise(GameObject parent)
    {
        outline = parent.GetComponent<Outline>();
    }

    private void OnEnable()
    {
        if (outline != null)
        {
            outline.enabled = true;
        }
    }

    private void OnDisable()
    {
        if (outline != null)
        {
            outline.enabled = false;
        }
    }
}
