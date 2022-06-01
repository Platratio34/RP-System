using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactor : Equipment {

    [Range(0,1)]
    public float controlLevel;

    public float powerGenTotal;
    public AnimationCurve powerCurve;
    public float heatGenTotal;
    public AnimationCurve heatCurve;

    public float overheatTemp = 100;
    public bool overheating;
    public float maxOverheatTime = 10f;
    public float overheatingTime;
    public float damageMod = 1f;

    void Start() {

    }

    protected override void onUpdate() {
        powerOut = powerGenTotal * powerCurve.Evaluate(controlLevel);
        heatGen = heatGenTotal * heatCurve.Evaluate(controlLevel);

        if(totalHeat > overheatTemp) {
            overheating = true;
        }
        if(totalHeat < overheatTemp*0.9f) {
            overheating = false;
        }
        if(totalHeat < overheatTemp && overheating) {
            overheatingTime -= Time.deltaTime * 0.5f;
        } else if(overheating) {
            overheatingTime += Time.deltaTime * ((totalHeat-overheatTemp)/overheatTemp);
        } else {
            overheatingTime -= Time.deltaTime;
        }
        overheatingTime = Mathf.Min(overheatingTime, 0);
        if(overheatingTime > maxOverheatTime) {
            damage(((totalHeat-overheatTemp)/overheatTemp) * damageMod * Time.deltaTime);
        }
    }
}
