using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitcher : MonoBehaviour {

    public bool on;
    public bool opMode;
    public bool emergency;
    public GameObject emerg;
    public GameObject op;
    public GameObject main;
    private float t;
    
    void Update() {
        t += Time.deltaTime;
        t %= 1;
        emerg.SetActive(emergency && t < 0.15f);
        main.SetActive(on && !opMode);
        op.SetActive(on && opMode);
    }
}
