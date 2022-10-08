using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandLight : MonoBehaviour {

    public InputActionProperty lightToggleAction;
    public Light handLight;
    private bool on = false;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(lightToggleAction.action.ReadValue<bool>()) {
            on = !on;
        }
        handLight.enabled = on;
    }
}
