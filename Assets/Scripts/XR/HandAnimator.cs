using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimator : MonoBehaviour {

    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gripAnimationAction;
    public Animator handAnimator;


    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        float pinch = pinchAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Trigger", pinch);
        float grip = gripAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Grip", grip);
    }
}
