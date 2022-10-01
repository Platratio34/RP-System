using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AutoPilot : MonoBehaviour {

    /// <summary>
    /// If the AP should go to a givin location
    /// </summary>
    public bool goToPos = true;
    /// <summary>
    /// The target location
    /// </summary>
    public Vector3 targetPos;
    /// <summary>
    /// If the AP should rotate to a givin rotation (not working)
    /// </summary>
    public bool goToRot = false;
    /// <summary>
    /// The rotation to go to
    /// </summary>
    public Vector3 targetRot;
    /// <summary>
    /// If it should wait to get the target position before rotating
    /// </summary>
    public bool waitRotForPos = true;

    /// <summary>
    /// If the AP should go to a target transform. Overrides goToPos
    /// </summary>
    public bool goToTransform;
    /// <summary>
    /// The target transform
    /// </summary>
    public Transform targetTrans;
    /// <summary>
    /// If the AP should match rotation with the target transform
    /// </summary>
    public bool sameRot;
    /// <summary>
    /// The distance to the target transform the AP should stop at
    /// </summary>
    public float distToTransform;
    /// <summary>
    /// For debugging
    /// </summary>
    private Vector3 transTargPoint;
    /// <summary>
    /// For debugging
    /// </summary>
    private Vector3 transLPos;

    /// <summary>
    /// If the AP should maintain 0 in the z of rotation.
    /// </summary>
    public bool flatZ = true;

    // debug vars;
    public Vector3 tLatVel;
    public Vector3 tRotVel;
    public Vector3 tRot;
    public float distToTarget;
    public Vector3 dPos;
    public Vector3 dRot;
    public Vector3 dRot2;
    // end debug vars

    /// <summary>
    /// The ship the AP controls
    /// </summary>
    public Ship ship;
    /// <summary>
    /// Transform of the controlled ship
    /// </summary>
    private Transform shipT;

    /// <summary>
    /// Current requested thrust of the AP
    /// </summary>
    public Vector3 cThrust;
    /// <summary>
    /// Current requested rotational acceleration of the AP
    /// </summary>
    public Vector3 cRot;

    /// <summary>
    /// If the AP is at it's target
    /// </summary>
    public bool atTarget;

    /// <summary>
    /// Rotational gain factor
    /// </summary>
    private float rG = 10f;

    // Called when the AP is created
    void Start() {
        shipT = ship.transform;
    }

    // Called every tick
    void Update() {
        if(shipT == null) {
            shipT = ship.transform;
        }
        if(targetTrans == null) {
            goToTransform = false;
        } else {
            // If the AP as a target transform, calculate the stopping position (from distToTransform)
            transTargPoint = targetTrans.position - shipT.position;
            transTargPoint = transTargPoint.normalized * (transTargPoint.magnitude - distToTransform);
            transTargPoint += shipT.position;
        }
        tRot = shipT.eulerAngles;
        dPos = targetPos - shipT.position;
        dRot = targetRot - shipT.eulerAngles;
        // Vector3 transVel;
        Vector3 relVel = ship.getRelativeVel();

        // If the AP is going to a transform, set the difference in position accordingly
        if(goToTransform) {
            dPos = transTargPoint - shipT.position;
            // If the AP is supposed to match rotation, set the difference in rotation accordingly (not working yet)
            if(sameRot) {
                dRot = targetTrans.eulerAngles - shipT.eulerAngles;
            }
            // If the ship is not in the same SOI as the target transform or in the target's SOI, subtract the target's speed to the ship's speed
            if(shipT.parent != targetTrans && shipT.parent != targetTrans.parent) {
            //     // Satellite s = shipT.parent.GetComponent<Satellite>();
            //     float v = ;
            //     // v -= ship.makeRel(s.getVel()).x;
            //      += v;
                relVel.x += ship.makeRel((transLPos - targetTrans.position) / Time.deltaTime).x;
            }
        }

        // Something to do with orbits, I think this is no longer needed
        // if(ship.isInSOI()) {
        //     relVel += ship.makeRel(ship.getSatVel());
        // }

        Vector3 eRot = shipT.rotation.eulerAngles; // euler rotation of the ship right now
        dRot2 = Vector3.zero;
        // bool oT = true;
        distToTarget = dPos.magnitude;

        // If the ship is not yet at the target (and has one), set the target rotation
        if(distToTarget > 0.1 && (goToPos || goToTransform) ) {
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
            tRot.z = 0; // Keeps the ship flat
        }

        // Set the requested rotational acceleration
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

        // If the ship is pointing pretty close to the target, set the target speed relative to the distance
        if(dRot2.magnitude < 1) {
            float ts = Mathf.Clamp(pID(dPos.magnitude), 0, 2f);
            tLatVel.x = ts;
        } else { // If the ship is pointing somewhat close, set the target speed, but reduce it based on how far off in direction the ship is
            float ts = Mathf.Clamp(pID(dPos.magnitude), 0, 2f);
            tLatVel.x = ts * (dRot2.magnitude/Mathf.Pow(dRot2.magnitude, 1.1f));
        }
        if(dRot2.magnitude > 10 || atTarget) {
            tLatVel.x = 0; // Don't thrust at all if the ship is not pointing close to the target or ar at the target
        }

        // Re-ranges the diffrence in velocity to match percentage thrusting
        cThrust = ship.effectiveAccl.divide(new Vector3((tLatVel.x - relVel.x), (tLatVel.y - relVel.y), (tLatVel.z - relVel.z)));

        // Don't thrust if it is only a small amount
        if(Mathf.Abs(cThrust.x) < 0.00001) {
            cThrust.x = 0;
        }
        if(Mathf.Abs(cThrust.y) < 0.00001) {
            cThrust.y = 0;
        }
        if(Mathf.Abs(cThrust.z) < 0.00001) {
            cThrust.z = 0;
        }

        // Update the last position of the target transform
        if(targetTrans != null) {
            transLPos = targetTrans.position;
        }
    }

    // Draws pretty colors in the editor
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

    /// <summary>
    /// Returns the difference in position, warping at max
    /// </summary>
    /// <param name="a">Value 1</param>
    /// <param name="b">Value 2</param>
    /// <param name="max">Wrap point</param>
    /// <returns>Difference warped at max</returns>
    private float getDiff(float a, float b, float max) {
        float d0 = a - b;
        float d1 = (a+max) - b;
        if(Mathf.Abs(d1) < Mathf.Abs(d0)) {
            return d1;
        }
        return d0;
    }

    /// <summary>
    /// Sort of a PID controller
    /// </summary>
    /// <param name="v">Process variable</param>
    /// <returns>Change</returns>
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
