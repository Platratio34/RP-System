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
        currentStored += jules;
        if(currentStored > maxStored) {
            float dif = currentStored - maxStored;
            currentStored = maxStored;
            return dif;
        }
        return 0;
    }

    public float testOutputJ(float jules) {
        return Mathf.Min(jules, maxStored);
    }
    public float outputJ(float jules) {
        jules = testOutputJ(jules);
        currentStored -= jules;
        return jules;
    }
}
