using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UltimateXR.Guides;

public class ObjectiveManager : MonoBehaviour
{

    public TextMeshProUGUI objectiveText;
    public Transform whiteboardGuide;
    public Transform fumeCabinetGuide;
    public Transform equipmentShelfGuide;


    void Start()
    {
       
    }

    void Update()
    {
        
    }

    public void UpdateText(string newText) {
        if (objectiveText != null) {
            objectiveText.text = newText;
        }
    }
}
