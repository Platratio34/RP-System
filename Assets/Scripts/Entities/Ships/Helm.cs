using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helm : Interactable {

    public bool active = true;

    public bool localPlayerControlled = false;
    public bool aIControlled = false;

    public bool velDampers = false;
    public bool rotDampers = false;

    public Ship ship;
    private Transform shipT;

    public bool autopilot;
    public Vector3 autoTarget;
    public bool autoFlat;

    public Vector3 pI;
    public Vector3 aII;

    public Vector3 tVel;
    public Vector3 tRot;

    public AutoPilot aP;

    void Start() {
        shipT = ship.transform;
    }

    void Update() {
        Vector3 relVel = ship.getRelativeVel();
        Vector3 eRot = shipT.rotation.eulerAngles;
        Vector3 thrust = Vector3.zero;
        Vector3 rot = Vector3.zero;
        if(velDampers && relVel.magnitude > 0.0001f) {
            thrust = new Vector3(relVel.x / ship.effectiveAccl.x, relVel.y / ship.effectiveAccl.y, relVel.z / ship.effectiveAccl.z) * -1f;
            tVel = Vector3.zero;
        }
        if(rotDampers && ship.rVel.magnitude > 0.0001f) {
            rot = new Vector3(ship.rVel.x / ship.effectiveRot.x, ship.rVel.y / ship.effectiveRot.y, ship.rVel.z / ship.effectiveRot.z) * -1f;
            tRot = Vector3.zero;
        }

        if(autopilot) {
            if(aP.atTarget) {
                autopilot = false;
            }
            if(aP == null) {
                autopilot = false;
                Debug.LogWarning("Atempted to use an AutoPilot, but none linked");
            } else {
                thrust = aP.cThrust;
                rot = aP.cRot;
            }
            // Debug.DrawLine(shipT.position, autoTarget, Color.yellow);
            // Debug.DrawLine(shipT.position, shipT.position+shipT.forward, Color.red);
            // Vector3 tAng = shipT.eulerAngles;
            // Vector3 dPos = autoTarget - shipT.position;
            // bool oT = true;
            // if(dPos.magnitude > 0.1) {
            //     Vector2 xz = new Vector2(dPos.x, dPos.z);
            //     tAng.y = Mathf.Atan2(xz.x, xz.y) * (360f/6.28f);
            //     tAng.x = Mathf.Atan2(dPos.y, xz.magnitude) * (360f/6.28f);
            //     // print("x=" + tAng.x + ", y=" + tAng.y);
            // }
            // if(autoFlat) {
            //     tAng.z = 0;
            // }
            // if(eRot.x > tAng.x + 0.1 || eRot.x < tAng.x - 0.1) {
            //     float tv = getDiff(tAng.x, eRot.x, 360) / 2f;
            //     tv = Mathf.Clamp(pID(tv), -15, 15);
            //     tRot.x = tv;
            //     rot.x = (tv - ship.rVel.x) / ship.effectiveRot.x;
            //     oT = false;
            // }
            // if(eRot.y > tAng.y + 0.1 || eRot.y < tAng.y - 0.1) {
            //     float tv = getDiff(tAng.y, eRot.y, 360) / 2f;
            //     tv = Mathf.Clamp(pID(tv), -15, 15);
            //     tRot.y = tv;
            //     rot.y = (tv - ship.rVel.y) / ship.effectiveRot.y;
            //     oT = false;
            // }
            // if(eRot.z > tAng.z + 0.1 || eRot.z < tAng.z - 0.1) {
            //     float tv = getDiff(tAng.z, eRot.z, 360) / 2f;
            //     tv = Mathf.Clamp(pID(tv), -15, 15);
            //     tRot.z = tv;
            //     // print("e=" + eRot.z + ", tv=" + tv);
            //     rot.z = (tv - ship.rVel.z) / ship.effectiveRot.z;
            //     // oT = false;
            // }
            // if(oT) {
            //     float ts = Mathf.Clamp(pID(dPos.magnitude), 0, 1f);
            //     tVel.x = ts;
            //     thrust.x = ts - relVel.x;
            // }
        } else if(localPlayerControlled) {
            thrust = addOver(thrust, pI);
            Debug.DrawLine(shipT.position, shipT.position + shipT.forward, Color.red);
        } else if(aIControlled) {
            thrust = addOver(thrust, aII);
            Debug.DrawLine(shipT.position, shipT.position + shipT.forward, Color.red);
        }

        if((localPlayerControlled || aIControlled) && active) {
            ship.thrust(thrust);
            ship.rotate(rot);
        }
    }

    private Vector3 addOver(Vector3 b, Vector3 o) {
        if(o.x > 0) {
            b.x = Mathf.Max(b.x, o.x);
        } else if(o.x < 0) {
            b.x = Mathf.Min(b.x, o.x);
        }
        if(o.y > 0) {
            b.y = Mathf.Max(b.y, o.y);
        } else if(o.y < 0) {
            b.y = Mathf.Min(b.y, o.y);
        }
        if(o.z > 0) {
            b.z = Mathf.Max(b.z, o.z);
        } else if(o.z < 0) {
            b.z = Mathf.Min(b.z, o.z);
        }
        return b;
    }

    private float getDiff(float a, float b, float max) {
        float d0 = a - b;
        float d1 = (a+max) - b;
        if(Mathf.Abs(d1) < Mathf.Abs(d0)) {
            return d1;
        }
        return d0;
    }

    private float pID(float v) {
        return 2f * Mathf.Log((v/8f)+1);
    }


}
