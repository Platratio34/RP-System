using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulbedLight : MonoBehaviour {

    public new Light light;
    public Renderer bulb;
    public bool active { get; private set; }
    public Color color { get; private set; }
    private Color lColor;
    private bool lActive;

    void Start() {
        lActive = light.gameObject.activeSelf;
        lColor = light.color;
    }

    public void SetActive(bool active) {
        this.active = active;
    }

    void Update() {
        if(active) {
            if(!lActive) light.gameObject.SetActive(true);
            if(color != lColor) bulb.material.SetColor("_Color", color);
            if(color != lColor) light.color = color;
            lColor = color;
        } else {
            if(lActive) {
                bulb.material.SetColor("_Color", Color.black);
                light.gameObject.SetActive(false);
            }
        }
        lActive = active;
    }

    public void setColor(Color color) {
        this.color = color;
    }
}
