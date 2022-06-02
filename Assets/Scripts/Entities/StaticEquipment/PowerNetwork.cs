using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerNetwork : MonoBehaviour {

    public Equipment[] equipment;
    public PowerNetwork[] subNets;
    public PowerStorage[] powerStorage;

    private float totalAvalib;
    private float totalReq;
    private float powerUsed;
    private float batUssage;
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
        }
        float dif = totalAvalib - totalReq;
        if(dif >= 0) {
            for(int i = 0; i < equipment.Length; i++) {
                equipment[i].matchPowerIn();
            }
            for(int i = 0; i < powerStorage.Length; i++) {
                if(dif > 0) {
                    float input = Mathf.Clamp(dif, 0, powerStorage[i].maxIn);
                    float p = powerStorage[i].inputJ(input * Time.deltaTime);
                    p /= Time.deltaTime;
                    input -= p;
                    batUssage -= p;
                    dif -= p;
                }
            }
            powerUsed = totalReq;
            overage = dif;
        } else {
            float avlb = 0;
            float req = dif * Time.deltaTime * -1f;
            for(int i = powerStorage.Length-1; i >= 0; i--) {
                if(req <= 0) {
                    break;
                }
                float p = powerStorage[i].outputJ(req);
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
                if(avlb <= 0) {
                    break;
                }
                float p = equipment[i].inputPower(avlb);
                avlb -= p;
                powerUsed += p;
            }
        }

        totalStored = 0;
        for(int i = 0; i < powerStorage.Length; i++) {
            totalStored += powerStorage[i].currentStored;
        }
        estTimeToOut = totalStored / Mathf.Max(batUssage, 0);
        if(estTimeToOut == float.NaN) estTimeToOut = float.PositiveInfinity;

        maxStored = 0;
        for(int i = 0; i < powerStorage.Length; i++) {
            maxStored += powerStorage[i].maxStored;
        }
        estTimeToCharged = (maxStored-totalStored) / Mathf.Max(batUssage*-1f, 0);
        if(estTimeToCharged == float.NaN) estTimeToCharged = float.PositiveInfinity;
        if(batUssage > 0) estTimeToCharged = float.PositiveInfinity;
    }

    public float getTotalAvalib() {
        return totalAvalib;
    }
    public float getTotalReq() {
        return totalReq;
    }

    public float getBatUssage() {
        return batUssage;
    }
    public float getOverage() {
        return overage;
    }

    public float getPowerUsed() {
        return powerUsed;
    }

    public float getTotalStored() {
        return totalStored;
    }
    public float getTimeToOut() {
        return estTimeToOut;
    }

    public float getMaxStorage() {
        return maxStored;
    }
    public float getTimeToCharged() {
        return estTimeToCharged;
    }
}
