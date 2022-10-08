using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActiveteRay : MonoBehaviour {

    public InputActionProperty[] leftActivate;
    public InputActionProperty[] rightActivate;

    public LineRenderer leftRay;
    public LineRenderer rightRay;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

        bool left = false;
        for (int i = 0; i < leftActivate.Length; i++) {
            left = left || leftActivate[i].action.ReadValue<float>() > 0.1f;
        }
        leftRay.enabled = left;

        bool right = false;
        for (int i = 0; i < rightActivate.Length; i++) {
            right = right || rightActivate[i].action.ReadValue<float>() > 0.1f;
        }
        rightRay.enabled = right;

    }
}
