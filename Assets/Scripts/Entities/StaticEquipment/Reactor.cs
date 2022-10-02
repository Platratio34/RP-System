using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates power for the network, also produces heat
/// </summary>
public class Reactor : Equipment {

    [Range(0,1)]
    public float controlLevel;
    private float actualLevel = 0f;
    public float controlSpeed = 1f;

    public float powerGenTotal;
    public AnimationCurve powerCurve;
    public float heatGenTotal;
    public AnimationCurve heatCurve;

    public float overheatTemp = 100;
    public bool overheating;
    public float maxOverheatTime = 10f;
    public float overheatingTime;
    public float damageMod = 1f;

    public bool modifyEmission;
    public float minEmission = 0;
    public float maxEmission = 1;
    public Renderer emissionRenderer;
    private Color emissionColor;

    void Start() {
        if(modifyEmission) {
            emissionColor = emissionRenderer.material.GetColor("_EmissionColor");
        }
    }

    protected override void onUpdate() {
        if(actualLevel < controlLevel) {
            actualLevel = Mathf.Clamp(actualLevel + (Time.deltaTime/controlSpeed), 0, controlLevel);
        } else if(actualLevel > controlLevel) {
            actualLevel = Mathf.Clamp(actualLevel - (Time.deltaTime/controlSpeed), controlLevel, 1);
        }

        powerOut = powerGenTotal * powerCurve.Evaluate(actualLevel);
        heatGen = heatGenTotal * heatCurve.Evaluate(actualLevel);

        if(totalHeat > overheatTemp) {
            overheating = true;
        }
        if(totalHeat < overheatTemp*0.9f) {
            overheating = false;
        }
        if(totalHeat < overheatTemp && overheating) {
            overheatingTime -= Time.deltaTime * 0.5f;
        } else if(overheating) {
            overheatingTime += Time.deltaTime * (totalHeat / overheatTemp);
        } else {
            overheatingTime -= Time.deltaTime;
        }
        overheatingTime = Mathf.Max(overheatingTime, 0);
        if(overheatingTime > maxOverheatTime) {
            damage( (totalHeat / overheatTemp) * damageMod * Time.deltaTime);
        }

        if(modifyEmission) {
            emissionRenderer.material.SetColor("_EmissionColor", emissionColor * Mathf.Lerp(minEmission, maxEmission, actualLevel));
        }
    }

    public float getActualLevel() {
        return actualLevel;
    }
}
