using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [ExecuteInEditMode]
public class LiquidControl : MonoBehaviour
{
    public Liquid liquid;
    public Color colour;
    [Range(0.42f, 0.52f)]
    public float _fillAmount;
    private Material _liquidMat;
    public bool _isEmpty;
    public bool _isFull;

    private void Start() {
        Renderer renderer = liquid.gameObject.GetComponent<Renderer>();
        _liquidMat = new Material(renderer.material);

        renderer.material = _liquidMat;
        _liquidMat.SetColor("_BottomColor", colour);
        _liquidMat.SetColor("_TopColor", colour);
    }
    private void Update() {
        liquid.fillAmount = _fillAmount;
        _liquidMat.SetColor("_BottomColor", colour);
        _liquidMat.SetColor("_TopColor", colour);

        if (_fillAmount <= 0.42f) {
            _isFull = true;
        } else if (_fillAmount >= 0.52f) {
            _isEmpty = true;
        } else {
            _isFull = _isEmpty = false;
        }
    }

    private void UpdateColour() {
        // If the particle that mixes with this object is not the same colour then mix the colours
    }


    public void Fill() {
        if (_isFull) return;
        _fillAmount -= 0.0002f;
    }

    public void Pour() {
        if (_isEmpty) return;
        _fillAmount += 0.0001f;
    }

    void OnParticleCollision(GameObject other)
    {
        // Get the ParticleSystem component from the colliding object
        ParticleSystem particleSystem = other.GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            Debug.Log("Trigger entered by: " + other.gameObject.name); // Log the object that triggered it
            if (other.CompareTag("Liquid")) {
                Fill();
            } else {
                Debug.Log("Other object detected with tag: " + other.tag);
            }
        }
    }
}

