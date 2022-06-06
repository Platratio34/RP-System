using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDrive : Equipment {

    public float maxStored;
    public float curReq;
    public float currentStored;
    public float maxIn;
    public float opereratingIn;

    public bool charge = false;
    public bool isCharged = false;

    public float overchargeRate = 1000;

    public float effecincy = 0.001f;
    public float totalMass = 1f;
    public float dischargeTime = 0.0001f;
    private float C = 300000;

    private Vector3 dest;

    public int state = 0;

    private float timeToJump = 1;

    public GameObject ship;

    void Start() {

    }

    protected override void onUpdate() {
        if(charge) {
            isCharged = currentStored >= curReq;
            if(!isCharged) {
                currentStored += (powerIn - opereratingIn) * Time.deltaTime;
                currentStored = Mathf.Clamp(currentStored, 0, maxStored);
                powerInReq = Mathf.Min(maxIn, (curReq-currentStored)/Time.deltaTime) + opereratingIn;
                isCharged = currentStored >= curReq;
            }
            if(isCharged) {
                charge = false;
                state = 2;
            }
        } else {
            powerInReq = opereratingIn;
            if(currentStored < maxStored) {
                powerInReq += Mathf.Clamp((maxStored-currentStored)/Time.deltaTime,0,overchargeRate);
                currentStored += (powerIn - opereratingIn) * Time.deltaTime;
            }
        }
        if(state == 3) {
            if(timeToJump <= 0) {
                jump();
            } else {
                timeToJump -= Time.deltaTime;
            }
        }
    }
    // E_req = (d*k)/(0.1*m)+(m*c)
    public float calcEnergy(float dist) {
        float eReq = (dist*effecincy)/(0.1f*totalMass) + (totalMass*C);
        float eT = eReq * (1 + C*C*dischargeTime*dischargeTime*dischargeTime);
        return eT;
    }
    public float calcEnergy(Vector3 target) {
        return calcEnergy(Vector3.Distance(ship.transform.position, target));
    }

    public bool prepForJump(Vector3 target) {
        float dist = Vector3.Distance(ship.transform.position, target);
        float eReq = calcEnergy(dist);
        if(eReq > maxStored) {
            state = -1;
            return false;
        }
        dest = target;
        curReq = eReq;
        charge = true;
        state = 1;
        if(currentStored >= curReq) {
            charge = false;
            isCharged = false;
            state = 2;
        }
        return true;
    }

    public bool startJump() {
        if(!isCharged) return false;
        if(state != 2) return false;
        state = 3;
        currentStored -= curReq;
        return true;
    }

    private bool jump() {
        if(state != 3) {
            return false;
        }
        if(Physics.OverlapSphere(dest, 100f).Length > 0) {
            state = -1;
            return false;
        }
        ship.transform.position = dest;
        state = 4;
        timeToJump = 1;
        return true;
    }

    public float getTimeToCharged() {
        float missing = curReq - currentStored;
        if(missing < 0) return 0;
        float incoming = powerIn - opereratingIn;
        return missing / incoming;
    }

}
