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
    // public Bounds cullingBox;

    private bool lOn;
    private bool lOp;
    private bool lBackup = true;

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

    void Start() {
        lOn = !on;
        lOp = !opMode;
        lBackup = !backup;
    }

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

        if(!on && !lOn) {
            return;
        }
        
        if(emergency) {
            t += Time.deltaTime;
            t %= 2;
        } else {
            if(opMode == lOp && backup == lBackup && lOn == on) return;
        }

        // Debug.Log(string.Format("{0} {1} {2}", on, lOn, onPow));

        lOn = on;
        lOp = opMode;
        lBackup = backup;

        // bool any = false;
        // foreach (Camera cam in Camera.allCameras) {
        //     if(cullingBox.Contains(cam.transform.position)) {
        //         any = true;
        //     }
        // }
        // if(!any) {
        //     foreach(CustomLight l in lights) {
        //         l.off();
        //     }
        // }

        foreach(CustomLight l in lights) {
            l.processes(onPow, opMode, emergency, backup, t);
        }
    }

    // void OnDrawGizmosSelected() {
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawWireCube(cullingBox.center + transform.position, cullingBox.extents * 2f);
    // }

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

        public BulbedLight light;
        public Color color = Color.white;
        public bool isEmerg;
        public Color eColor = Color.red;
        public bool isBackup;
        public Color bColor = Color.white;
        public bool isOp;
        public Color oColor = Color.blue;

        [Range(0f, 2f)]
        public float blinkLength = 0.15f;
        [Range(0.00f, 2f)]
        public float blinkOffset = 0.0f;

        private int lC = -100;

        public void processes(bool on, bool op, bool em, bool backup, float t) {
            if(on || (isBackup && backup)) {
                light.active = true;
            } else {
                light.active = false;
                return;
            }



            if(em && isEmerg && checkBlink(t) ) {
                if(lC != 9) light.setColor(eColor);
                lC = 9;
            } else if(backup && isBackup) {
                if(lC != -1) light.setColor(bColor);
                lC = -1;
            } else if(op && isOp) {
                if(lC != 1) light.setColor(oColor);
                lC = 1;
            } else {
                if(lC != 0) light.setColor(color);
                lC = 0;
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
