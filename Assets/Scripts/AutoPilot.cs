using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AutoPilot : MonoBehaviour {

    public bool goToPos = true;
    public Vector3 targetPos;
    public bool goToRot = false;
    public Vector3 targetRot;
    public bool waitRotForPos = true;

    public bool goToTransform;
    public Transform targetTrans;
    public bool sameRot;
    public float distToTransform;
    private Vector3 transTargPoint;
    private Vector3 transLPos;

    public bool flatZ = true;

    // debug vars;
    public Vector3 tLatVel;
    public Vector3 tRotVel;
    public Vector3 tRot;
    public float distToTarget;
    public Vector3 dPos;
    public Vector3 dRot;
    public Vector3 dRot2;

    public Ship ship;
    private Transform shipT;

    public Vector3 cThrust;
    public Vector3 cRot;

    public bool atTarget;

    private float rG = 10f;

    void Start() {
        shipT = ship.transform;
    }

    void Update() {
        tRot = shipT.eulerAngles;
        dPos = targetPos - shipT.position;
        dRot = targetRot - shipT.eulerAngles;

        transTargPoint = targetTrans.position - shipT.position;
        transTargPoint = transTargPoint.normalized * (transTargPoint.magnitude - distToTransform);
        transTargPoint += shipT.position;
        // Vector3 transVel;
        Vector3 relVel = ship.getRelativeVel();
        if(goToTransform) {
            dPos = transTargPoint - shipT.position;
            if(sameRot) {
                dRot = targetTrans.eulerAngles - shipT.eulerAngles;
            }
            if(shipT.parent != targetTrans) {
                Satellite s = shipT.parent.GetComponent<Satellite>();
                float v = ship.makeRel((transLPos - targetTrans.position) / Time.deltaTime).x;
                v -= ship.makeRel(s.getVel()).x;
                relVel.x += v;
            }
        }
        Vector3 eRot = shipT.rotation.eulerAngles;
        dRot2 = Vector3.zero;
        // bool oT = true;
        distToTarget = dPos.magnitude;
        if(dPos.magnitude > 0.1 ) {
            atTarget = false;
            Vector2 xz = new Vector2(dPos.x, dPos.z);
            tRot.y = Mathf.Atan2(xz.x, xz.y) * (360f/6.28f);
            tRot.x = Mathf.Atan2(dPos.y, xz.magnitude) * (-360f/6.28f);
            // print("x=" + tRot.x + ", y=" + tRot.y);
        } else {
            atTarget = true;
            tLatVel = Vector3.zero;
            cThrust.x = 0;
            eRot = shipT.rotation.eulerAngles;
        }
        if(flatZ) {
            tRot.z = 0;
        }
        if(eRot.x > tRot.x + 0.1 || eRot.x < tRot.x - 0.1) {
            float tv = getDiff(tRot.x, eRot.x, 360) / 2f;
            dRot2.x = tv*2f;
            tv = Mathf.Clamp(pID(tv)*rG, -15, 15);
            tRotVel.x = tv;
            cRot.x = (tv - ship.rVel.x) / ship.effectiveRot.x;
            // oT = false;
        }
        if(eRot.y > tRot.y + 0.1 || eRot.y < tRot.y - 0.1) {
            float tv = getDiff(tRot.y, eRot.y, 360) / 2f;
            dRot2.y = tv*2f;
            // print(tv + ", " + pID(tv));
            tv = Mathf.Clamp(pID(tv)*rG, -15, 15);
            tRotVel.y = tv;
            cRot.y = (tv - ship.rVel.y) / ship.effectiveRot.y;
            // oT = false;
        }
        if(eRot.z > tRot.z + 0.1 || eRot.z < tRot.z - 0.1) {
            float tv = getDiff(tRot.z, eRot.z, 360) / 2f;
            dRot2.z = tv*2f;
            tv = Mathf.Clamp(pID(tv)*rG, -15, 15);
            tRotVel.z = tv;
            // print("e=" + eRot.z + ", tv=" + tv);
            cRot.z = (tv - ship.rVel.z) / ship.effectiveRot.z;
            // oT = false;
        }
        if(dRot2.magnitude < 1) {
            float ts = Mathf.Clamp(pID(dPos.magnitude), 0, 2f);
            tLatVel.x = ts;
        } else {
            float ts = Mathf.Clamp(pID(dPos.magnitude), 0, 2f);
            tLatVel.x = ts * (dRot2.magnitude/Mathf.Pow(dRot2.magnitude, 1.1f));
        }
        if(dRot2.magnitude > 10 || atTarget) {
            tLatVel.x = 0;
        }
        cThrust = new Vector3((tLatVel.x - relVel.x) / ship.effectiveAccl.x, (tLatVel.y - relVel.y) / ship.effectiveAccl.y, (tLatVel.z - relVel.z) / ship.effectiveAccl.z);
        // print(cRot);
        if(targetTrans != null) {
            transLPos = targetTrans.position;
        }
    }

    void OnDrawGizmos() {
        Vector3 trDir = cThrust.x * shipT.forward;
        trDir += cThrust.y * shipT.up;
        trDir += cThrust.z * shipT.right;
        Gizmos.color = ((goToPos&&!goToTransform)?Color.yellow:Color.blue);
        Gizmos.DrawLine(shipT.position, targetPos);
        Gizmos.DrawSphere(targetPos, 0.1f);
        if(targetTrans != null) {
            Gizmos.color = goToTransform?Color.yellow:Color.blue;
            // Gizmos.color = (goToTransform?Color.yellow:Color.blue);
            Gizmos.DrawLine(shipT.position, targetTrans.position);
            Gizmos.DrawSphere(transTargPoint, 0.1f);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawLine(shipT.position, shipT.position+shipT.forward);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(shipT.position, shipT.position + trDir);
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
        bool n = v<0;
        if(n) v *= -1f;
        v /= 8;
        v += 1;
        if(v == 0) {
            return 0;
        }
        return (n?-2f:2f) * Mathf.Log(v);
    }
}
