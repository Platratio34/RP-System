using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Engine for ship
/// </summary>
public class Engine : Equipment {

    /// <summary>
    /// The current throttle of the engine
    /// </summary>
    [Range(0,1)]
    public float throttle;
    /// <summary>
    /// The maximum power draw of the engine
    /// </summary>
    public float maxPower = 1;
    /// <summary>
    /// The thrust produced at max power
    /// </summary>
    public float maxThrust;
    /// <summary>
    /// The heat produced at max power
    /// </summary>
    public float maxHeatGen = 0;
    /// <summary>
    /// The current thrust of the engine
    /// </summary>
    public float thrust;

    /// <summary>
    /// The ship the engine is attached to
    /// </summary>
    public Ship ship;
    /// <summary>
    /// The direction of the thrust relative to the ship (vector)
    /// </summary>
    public Vector3 dir;

    /// <summary>
    /// If the engine has already thrusted this tick
    /// </summary>
    private bool thrusted = false;

    void Start() {
        dir = dir.normalized;
    }


    /// <summary>
    /// Recalculates and returns the requested power
    /// </summary>
    /// <returns>The requested power</returns>
    public override float getPowerReq() {
        powerInReq = maxPower * throttle;
        return powerInReq;
    }

    /// <summary>
    /// Sets the new thrust level of the engine
    /// </summary>
    /// <param name="throttle"></param>
    public void setThrust(float throttle) {
        throttle = Mathf.Clamp(throttle, 0, 1);
        this.throttle = throttle;

        powerInReq = maxPower * throttle; // recalculates the requested power
        float pP = powerIn / maxPower; // calculates the current percent of full thrust possible
        pP = Mathf.Clamp(pP, 0, throttle); // limits thrust to throttle
        thrust = maxThrust * pP; // Sets thrust
        heatGen = maxHeatGen * pP;
        
        if(thrusted) {
            return;
        }
        if(ship != null) {
            ship.applyForce(dir * thrust); // applies thrust to ship
            thrusted = true;
        }
    }

    // Thrusts
    protected override void onUpdate() {
        throttle = Mathf.Clamp(throttle, 0, 1);

        powerInReq = maxPower * throttle; // recalculates the requested power
        float pP = powerIn / maxPower; // calculates the current percent of full thrust possible
        pP = Mathf.Clamp(pP, 0, throttle); // limits thrust to throttle
        thrust = maxThrust * pP; // sets thrust
        heatGen = maxHeatGen * pP;

        if(thrusted) { // If setThrust() was called since last tick, skip applying thrust to the ship
            thrusted = false;
            return;
        }
        if(ship != null) {
            ship.applyForce(dir * thrust); // applies thrust to ship
        }
    }
}
