using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Thruster : MonoBehaviour {

    public float fA = 0;
    public float mA = 0.01f;
    public GameObject exaust;

    void Start() {

    }

    void Update() {
        exaust.SetActive(fA>mA);
    }

    public void fire(float a) {
        fA = Mathf.Clamp(a, 0, 1);
    }
}
