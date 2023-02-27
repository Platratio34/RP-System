using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulbedLight : MonoBehaviour {

    public new Light light;
    public Renderer bulb;
    public bool active;
    public Color color { get; private set; }
    // private Color lColor;
    private bool lActive;
    private bool colorChange = false;

    void Start() {
        lActive = light.gameObject.activeSelf;
        // lColor = light.color;
    }

    public void SetActive(bool active) {
        this.active = active;
    }

    void Update() {
        if(active) {
            if(!lActive) light.gameObject.SetActive(true);
            if(colorChange) {
                bulb.material.SetColor("_Color", color);
                light.color = color;
                colorChange = false;
            }
            // lColor = color;
        } else {
            if(lActive) {
                bulb.material.SetColor("_Color", Color.black);
                light.gameObject.SetActive(false);
            }
        }
        lActive = active;
    }

    public void setColor(Color color) {
        colorChange = this.color != color;
        this.color = color;
    }
}
