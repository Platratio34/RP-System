using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Althing that uses power, or works with heat
/// </summary>
public class Equipment : Interactable {

    /// <summary>
    /// The current power out of the equipment
    /// </summary>
    [SerializeField] protected float powerOut;
    /// <summary>
    /// The current power from the network
    /// </summary>
    [SerializeField] protected float powerIn;
    /// <summary>
    /// The power requested by the equipment
    /// </summary>
    [SerializeField] protected float powerInReq;
    /// <summary>
    /// The current amount of heat being generated by the equipment
    /// </summary>
    [SerializeField] protected float heatGen;
    /// <summary>
    /// Total current heat of the equipment
    /// </summary>
    [SerializeField] protected float totalHeat;
    /// <summary>
    /// Passive heat dispersal of the equipment
    /// </summary>
    [SerializeField] protected float pasiveHeatDisp;
    /// <summary>
    /// Current heat removed from the equipment
    /// </summary>
    [SerializeField] protected float heatOut;
    /// <summary>
    /// Health of the equipment (0-100)
    /// </summary>
    [SerializeField] protected float health = 100;

    void Start() {

    }

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
    /// <summary>
    /// Called after powerIn is set
    /// </summary>
    public virtual void onPowerIn() {}
    /// <summary>
    /// Called every tick after heat and health are updated
    /// </summary>
    protected virtual void onUpdate() {}

    /// <summary>
    /// Makes the equipment explode
    /// </summary>
    public virtual void explode() {
        print("BOOM!");
    }

    /// <summary>
    /// Damages the equipment
    /// </summary>
    /// <param name="amt">The amount of damage</param>
    public void damage(float amt) {
        health = Mathf.Clamp(health - amt, 0, 100);
    }

    /// <summary>
    /// Gets the net power of the equipment (power out - power in)
    /// </summary>
    /// <returns>The net power (W)</returns>
    public float getNetPower() {
        return powerOut - powerIn;
    }
    /// <summary>
    /// Gets the requested power of the equipment
    /// </summary>
    /// <returns>The requested power (W)</returns>
    public virtual float getPowerReq() {
        return powerInReq;
    }
    /// <summary>
    /// Gets the net heat of the equipment (gen - passive - out)
    /// </summary>
    /// <returns>The net heat (deg/s)</returns>
    public float getNetHeat() {
        return heatGen - pasiveHeatDisp - heatOut;
    }
    /// <summary>
    /// Gets the passive heat dispersal of the equipment
    /// </summary>
    /// <returns>The pastiche heat dispersal (deg/s)</returns>
    public float getPasiveHeatDisp() {
        return pasiveHeatDisp;
    }
    /// <summary>
    /// Sets the passive heat dispersal
    /// </summary>
    /// <param name="disp">The new heat dispersal (deg/s)</param>
    public void setPasiveHeatDisp(float disp) {
        pasiveHeatDisp = disp;
    }
    /// <summary>
    /// Gets the total heat of the equipment
    /// </summary>
    /// <returns>The total heat</returns>
    public float getTotalHeat() {
        return totalHeat;
    }
    /// <summary>
    /// Gets the health of the equipment
    /// </summary>
    /// <returns>The health (0-100)</returns>
    public float getHealth() {
        return health;
    }

    /// <summary>
    /// Gets the current power out of the equipment
    /// </summary>
    /// <returns>The power out (W)</returns>
    public float getPowerOut() {
        return powerOut;
    }

    /// <summary>
    /// Sets the power in to match the requested power
    /// </summary>
    public void matchPowerIn() {
        powerIn = powerInReq;
    }
    /// <summary>
    /// Takes in power, returning the amount taken
    /// </summary>
    /// <param name="power">Power in (W)</param>
    /// <returns>The power used (W)</returns>
    public float inputPower(float power) {
        power = Mathf.Min(power, powerInReq);
        powerIn = power;
        return power;
    }

    /// <summary>
    /// Sets the power in to the equipment
    /// </summary>
    /// <param name="power">The power in (W)</param>
    public void setPowerIn(float power) {
        powerIn = power;
    }
}
