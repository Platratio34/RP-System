using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : Equipment {

    [Range(0,1)]
    public float throttle;
    public float maxPower = 1;
    public float maxThrust;
    public float thrust;

    void Start() {

    }


    protected override void onUpdate() {
        powerInReq = maxPower * throttle;
        float pP = powerIn/maxPower;
        pP = Mathf.Clamp(pP, 0, 1);
        thrust = maxThrust * pP;
    }
}
