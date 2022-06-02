using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerStorage : Equipment {

    public float maxStored;
    public float currentStored;
    public float maxIn;
    public float maxOut;

    void Start() {

    }

    protected override void onUpdate() {

    }

    public float inputJ(float jules) {
        jules = Mathf.Clamp(jules, 0, maxIn*Time.deltaTime);
        jules = Mathf.Min(jules, maxStored - currentStored);
        currentStored += jules;
        // if(currentStored > maxStored) {
        //     float dif = currentStored - maxStored;
        //     currentStored = maxStored;
        //     return dif;
        // }
        return jules;
    }

    public float testOutputJ(float jules) {
        return Mathf.Clamp(jules, 0, Mathf.Min(maxOut*Time.deltaTime, currentStored));
    }
    public float outputJ(float jules) {
        jules = testOutputJ(jules);
        currentStored -= jules;
        if(currentStored < 0) {
            jules += currentStored;
            currentStored = 0;
        }
        return jules;
    }
}
