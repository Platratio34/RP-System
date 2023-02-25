using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Network of equipment using power, does no handle heat
/// </summary>
public class PowerNetwork : MonoBehaviour {

    /// <summary>
    /// Array of power using equipment. Ordered by priority
    /// </summary>
    public Equipment[] equipment;
    /// <summary>
    /// Array of sub nets. Ordered by priority
    /// </summary>
    public PowerNetwork[] subNets;
    /// <summary>
    /// Array of power storage. Charged in order, discharged in reverse order
    /// </summary>
    public PowerStorage[] powerStorage;

    // Start display variables see getters for description
    private float totalAvalib;
    private float totalReq;
    private float powerUsed;
    private float batUssage;
    private float[] batUssageRunAvg = new float[10];
    private int batUssageRunAvgInd = 0;
    private float overage;
    private float totalStored;
    private float estTimeToOut;
    private float maxStored;
    private float estTimeToCharged;

    void Start() {

    }

    void Update() {
        totalAvalib = 0;
        totalReq = 0;
        batUssage = 0;
        overage = 0;
        for(int i = 0; i < equipment.Length; i++) {
            totalAvalib += equipment[i].getPowerOut();
            totalReq += equipment[i].getPowerReq();
            equipment[i].setPowerIn(0);
        }
        float dif = totalAvalib - totalReq;
        if(dif >= 0) { // If there is enough power
            for(int i = 0; i < equipment.Length; i++) {
                equipment[i].matchPowerIn(); // Giv all equipment all requested power
            }
            for(int i = 0; i < powerStorage.Length; i++) {
                if(dif > 0) { // Store power into batteries if there is excess
                    float input = Mathf.Clamp(dif, 0, powerStorage[i].maxIn);
                    float p = powerStorage[i].inputJ(input * Time.deltaTime);
                    p /= Time.deltaTime;
                    input -= p;
                    batUssage -= p;
                    dif -= p;
                } else {
                    break; // don't keep filling batteries if out of power
                }
            }
            powerUsed = totalReq;
            overage = dif;
        } else {
            float avlb = 0;
            float req = dif * Time.deltaTime * -1f; // convert wats to jules for batteries
            for(int i = powerStorage.Length-1; i >= 0; i--) {
                if(req <= 0) { // If enough power was found from the batteries
                    break;
                }
                float p = powerStorage[i].outputJ(req); // take as much power as possible from battery
                req -= p;
                avlb += p;
            }
            avlb /= Time.deltaTime;
            batUssage = avlb;
            overage = dif+avlb;
            avlb += totalAvalib;
            overage *= 1f;
            powerUsed = 0;
            for(int i = 0; i < equipment.Length; i++) {
                if(avlb <= 0) { // If there was not enough power, stop the loop
                    break;
                }
                float p = equipment[i].inputPower(avlb); // Give as much power as possible
                avlb -= p;
                powerUsed += p;
            }
        }

        // keep track of power stored for information purposes
        batUssageRunAvg[batUssageRunAvgInd] = batUssage;
        batUssageRunAvgInd++;
        if(batUssageRunAvgInd >= 10) {
            batUssageRunAvgInd -= 10;
        }

        float batUssageAvg = 0;
        for (int i = 0; i < 10; i++) {
            batUssageAvg += batUssageRunAvg[i];
        }
        batUssageAvg /= 10;

        totalStored = 0;
        for(int i = 0; i < powerStorage.Length; i++) {
            totalStored += powerStorage[i].currentStored;
        }
        estTimeToOut = totalStored / Mathf.Max(batUssageAvg, 0);
        if(estTimeToOut == float.NaN) estTimeToOut = float.PositiveInfinity;

        maxStored = 0;
        for(int i = 0; i < powerStorage.Length; i++) {
            maxStored += powerStorage[i].maxStored;
        }
        estTimeToCharged = (maxStored-totalStored) / Mathf.Max(batUssageAvg*-1f, 0);
        if(estTimeToCharged == float.NaN) estTimeToCharged = float.PositiveInfinity;
        if(batUssageAvg > 0) estTimeToCharged = float.PositiveInfinity;
        
        for(int i = 0; i < equipment.Length; i++) {
            equipment[i].onPowerIn(); // let equipment know it's power in was updated
        }
    }

    /// <summary>
    /// Gets the total power availble on the network, excluding batteries
    /// </summary>
    /// <returns>Total power generated (W)</returns>
    public float getTotalAvalib() {
        return totalAvalib;
    }
    /// <summary>
    /// Gets the total power requested on the network
    /// </summary>
    /// <returns>Total requested power (W)</returns>
    public float getTotalReq() {
        return totalReq;
    }

    /// <summary>
    /// Gets the amount of power being pulled from batteries
    /// </summary>
    /// <returns>Power out from batteries (W)</returns>
    public float getBatUssage() {
        return batUssage;
    }
    /// <summary>
    /// Gets the amount of power over availble that is requested
    /// </summary>
    /// <returns>Power requested over availble (W)</returns>
    public float getOverage() {
        return overage;
    }

    /// <summary>
    /// Gets the total power used by equipment
    /// </summary>
    /// <returns>Total power used (W)</returns>
    public float getPowerUsed() {
        return powerUsed;
    }

    /// <summary>
    /// Gets total power stored in batteries
    /// </summary>
    /// <returns>Total power stored (J)</returns>
    public float getTotalStored() {
        return totalStored;
    }
    /// <summary>
    /// Gets the estimated time until batteries are out of power
    /// </summary>
    /// <returns>Time until batteries run out (s)</returns>
    public float getTimeToOut() {
        return estTimeToOut;
    }

    /// <summary>
    /// Gets the maximum amount of power the batteries can store
    /// </summary>
    /// <returns>Total possible power storage (J)</returns>
    public float getMaxStorage() {
        return maxStored;
    }
    /// <summary>
    /// Gets the estimated time until all batteries are charged
    /// </summary>
    /// <returns>Time until batteries are charged (s)</returns>
    public float getTimeToCharged() {
        return estTimeToCharged;
    }
}
