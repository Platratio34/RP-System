using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Equipment : Entity {

    [SerializeField] protected float powerOut;
    [SerializeField] protected float powerIn;
    [SerializeField] protected float powerInReq;
    [SerializeField] protected float heatGen;
    [SerializeField] protected float totalHeat;
    [SerializeField] protected float pasiveHeatDisp;
    [SerializeField] protected float heatOut;
    [SerializeField] protected float health = 100;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update() {
        onUpdate();
        totalHeat += heatGen * Time.deltaTime;
        totalHeat -= pasiveHeatDisp * Time.deltaTime;
        totalHeat -= heatOut * Time.deltaTime;
        totalHeat = Mathf.Max(totalHeat, 0);
        if(health == 0) {
            explode();
        }
    }
    protected virtual void onUpdate() {}

    public void explode() {
        print("BOOM!");
    }

    public void damage(float amt) {
        health = Mathf.Clamp(health - amt, 0, 100);
    }
}
