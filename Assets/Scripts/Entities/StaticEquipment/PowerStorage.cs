using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores power for later usage
/// </summary>
public class PowerStorage : Equipment {

    /// <summary>
    /// Maximum possible power storage (J)
    /// </summary>
    public float maxStored;
    /// <summary>
    /// Current power stored (J)
    /// </summary>
    public float currentStored;
    /// <summary>
    /// Maximum charge rate (W)
    /// </summary>
    public float maxIn;
    /// <summary>
    /// Maximum discharge rate (W)
    /// </summary>
    public float maxOut;

    void Start() {

    }

    protected override void onUpdate() {

    }

    /// <summary>
    /// Input power, and return amount stored
    /// </summary>
    /// <param name="jules">Input power (J)</param>
    /// <returns>Amount of power stored (J)</returns>
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

    /// <summary>
    /// Checks if a givin amount of power can be output
    /// </summary>
    /// <param name="jules">Requested power (J)</param>
    /// <returns>Actual power possible (J)</returns>
    public float testOutputJ(float jules) {
        return Mathf.Clamp(jules, 0, Mathf.Min(maxOut*Time.deltaTime, currentStored));
    }
    /// <summary>
    /// Outputs power from storage
    /// </summary>
    /// <param name="jules">Power requested (J)</param>
    /// <returns>Power output (J)</returns>
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
