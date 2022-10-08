using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class RotateInteractable : XRBaseInteractable {

    private float lRot;
    public float speed = 0.1f;
    private XRBaseInteractor interactor;

    public RotateEvent onChange;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(interactor != null) {
            float r = interactor.transform.localEulerAngles.z;
            float d = lRot - r;
            d *= speed;
            // Debug.Log(d);
            if(d >= 1) {
                onChange.Invoke(1);
                // Debug.Log("1");
                lRot = r;
            } else if(d <= -1) {
                onChange.Invoke(-1);
                // Debug.Log("-1");
                lRot = r;
            }
        }
    }

    protected override void OnSelectEntering(XRBaseInteractor interactor) {
        lRot = interactor.transform.localEulerAngles.z;
        this.interactor = interactor;
    }

    protected override void OnSelectExiting(XRBaseInteractor interactor) {
        this.interactor = null;
    }
}

[System.Serializable]
public class RotateEvent : UnityEvent<int> { };
