using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitcher : Equipment
{

    public bool on;
    public bool onPow { get; private set; }
    public bool opMode;
    public bool emergency;
    public bool backup;
    public GameObject[] emerg;
    public GameObject[] op;
    public GameObject[] main;
    private float t;

    private float powerPerLight = 10f;

    public CustomLight[] lights;

    // void Update() {
    //     t += Time.deltaTime;
    //     t %= 2;
    //     // bool emOn = emergency && t < 0.15f;
    //     // foreach(GameObject obj in emerg) {
    //     //     obj.SetActive(emOn);
    //     // }
    //     // foreach(GameObject obj in main) {
    //     //     obj.SetActive(on && !opMode);
    //     // }
    //     // foreach(GameObject obj in op) {
    //     //     obj.SetActive(on && opMode);
    //     // }
    //     foreach(CustomLight l in lights) {
    //         l.processes(onPow, opMode, emergency, backup, t);
    //     }
    // }

    public override float getPowerReq() {
        powerInReq = on ? powerPerLight * lights.Length : 0f;
        return powerInReq;
    }

    protected override void onUpdate() {
        if(!on) {
            onPow = false;
            backup = false;
        } else if(powerIn >= powerInReq) {
            onPow = true;
            backup = false;
        } else {
            onPow = false;
            backup = true;
        }
        
        t += Time.deltaTime;
        t %= 2;

        foreach(CustomLight l in lights) {
            l.processes(onPow, opMode, emergency, backup, t);
        }
    }

    public void setOn(EID eid) {
        on = eid.state == 1.0f;
    }
    public void setOp(EID eid) {
        opMode = eid.state == 1.0f;
    }
    public void setEmerg(EID eid) {
        emergency = eid.state == 1.0f;
    }

    [System.Serializable]
    public class CustomLight {

        public Light light;
        public Color color = Color.white;
        public bool isEmerg;
        public Color eColor = Color.red;
        public bool isBackup;
        public Color bColor = Color.white;
        public bool isOp;
        public Color oColor = Color.blue;

        [Range(0f,2f)]
        public float blinkLength = 0.15f;
        [Range(0.00f,2f)]
        public float blinkOffset = 0.0f;

        public void processes(bool on, bool op, bool em, bool backup, float t) {
            if(on || (isBackup && backup)) {
                light.gameObject.SetActive(true);
            } else {
                light.gameObject.SetActive(false);
                return;
            }
            if(em && isEmerg && checkBlink(t) ) {
                light.color = eColor;
            } else if(op && isOp) {
                light.color = oColor;
            } else if(backup && isBackup) {
                light.color = bColor;
            } else {
                light.color = color;
            }
        }

        private bool checkBlink(float t) {
            if(blinkLength == 2.0f) return true;
            
            if(t > blinkLength + blinkOffset) return false;

            if(t >= blinkOffset) return true;

            if(blinkLength + blinkOffset > 2.0f) {
                return t <= blinkLength + blinkOffset - 2;
            }

            return false;
        }
    }
}
