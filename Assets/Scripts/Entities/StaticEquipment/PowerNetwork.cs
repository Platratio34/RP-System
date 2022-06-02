using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerNetwork : MonoBehaviour {

    public Equipment[] equipment;
    public PowerNetwork[] subNets;
    public PowerStorage[] powerStorage;

    private float totalAvalib;
    private float totalReq;
    private float batUssage;
    private float overage;

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
        if(dif > 0) {
            for(int i = 0; i < equipment.Length; i++) {
                equipment[i].matchPowerIn();
            }
            for(int i = 0; i < powerStorage.Length; i++) {
                if(dif > 0) {
                    float input = Mathf.Clamp(dif, 0, powerStorage[i].maxIn);
                    input -= powerStorage[i].inputJ(input * Time.deltaTime);
                    batUssage -= input;
                    dif -= input;
                }
            }
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
            overage *= 1f;
            for(int i = 0; i < equipment.Length; i++) {
                if(avlb <= 0) {
                    break;
                }
                avlb -= equipment[i].inputPower(avlb);
            }
        }
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
}
