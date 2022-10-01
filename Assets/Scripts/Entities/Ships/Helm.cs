using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller for ships, supports Player, AI, and AP control
/// </summary>
public class Helm : Interactable {

    /// <summary>
    /// The active state of the helm
    /// </summary>
    public bool active = true;
    
    /// <summary>
    /// If the helm is controlled by a local player
    /// </summary>
    public bool localPlayerControlled = false;
    /// <summary>
    /// If the helm is controlled by an AI
    /// </summary>
    public bool aIControlled = false;

    /// <summary>
    /// Velocity dampers. (Tries to maintain 0 velocity)
    /// </summary>
    public bool velDampers = false;
    /// <summary>
    /// Rotational velocity dampers. (Tries to maintain 0 rotational velocity)
    /// </summary>
    public bool rotDampers = false;
    /// <summary>
    /// Holds the position the ship was at when activated
    /// </summary>
    public bool posHold = false;
    /// <summary>
    /// If it was holding position last tick
    /// </summary>
    private bool lposHold = false;
    /// <summary>
    /// The position to hold
    /// </summary>
    private Vector3 holdPos;

    /// <summary>
    /// This ship this helm controls
    /// </summary>
    public Ship ship;
    /// <summary>
    /// The Transform of the ship
    /// </summary>
    private Transform shipT;

    /// <summary>
    /// If the autopilot is active
    /// </summary>
    public bool autopilot;
    /// <summary>
    /// If the AP stopped it self
    /// </summary>
    public bool apStopped;

    /// <summary>
    /// The local player's input
    /// </summary>
    public Vector3 pI;
    /// <summary>
    /// The AI's input
    /// </summary>
    public Vector3 aII;

    /// <summary>
    /// The AP associated with this helm
    /// </summary>
    public AutoPilot aP;

    // Called when the helm is created
    void Start() {
        shipT = ship.transform;
    }

    // Called every tick
    void Update() {
        if(shipT == null) {
            shipT = ship.transform;
        }

        Vector3 relVel = ship.getRelativeVel();
        Vector3 eRot = shipT.rotation.eulerAngles;
        Vector3 thrust = Vector3.zero;
        Vector3 rot = Vector3.zero;
        if(velDampers && relVel.magnitude > 0.0001f) {
            // thrust = new Vector3(relVel.x / ship.effectiveAccl.x, relVel.y / ship.effectiveAccl.y, relVel.z / ship.effectiveAccl.z) * -1f;
            thrust = ship.effectiveAccl.divide(relVel) * -1f;
        }
        if(rotDampers && ship.rVel.magnitude > 0.0001f) {
            rot = new Vector3(ship.rVel.x / ship.effectiveRot.x, ship.rVel.y / ship.effectiveRot.y, ship.rVel.z / ship.effectiveRot.z) * -1f;
        }

        if(autopilot) {
            if(aP.atTarget) {
                autopilot = false;
                apStopped = true;
            } else {
                apStopped = false;
                if(aP == null) {
                    autopilot = false;
                    Debug.LogWarning("Attempted to use an AutoPilot, but none linked");
                } else {
                    thrust = aP.cThrust;
                    rot = aP.cRot;
                }
            }
        } else if(localPlayerControlled) { // Add the player's input if they are in control
            thrust = addOver(thrust, pI);
            Debug.DrawLine(shipT.position, shipT.position + shipT.forward, Color.red);
        } else if(aIControlled) { // Add the AI's input if it is in control
            thrust = addOver(thrust, aII);
            Debug.DrawLine(shipT.position, shipT.position + shipT.forward, Color.red);
        } if(posHold) { // This is comment out for now to try to figure out what was creating the oscillation
            // if(!lposHold) {
            //     holdPos = shipT.localPosition;
            // }
            // Vector3 tVel = shipT.localPosition - holdPos * 0.5f;
            // if(tVel.magnitude > 0.01f) {
            //     // thrust = (1/tVel.x) * shipT.forward;
            //     // thrust += (1/tVel.y) * shipT.up;
            //     // thrust += (1/tVel.z) * shipT.right;
            //     thrust = tVel.x * shipT.forward;
            //     thrust += tVel.y * shipT.up;
            //     thrust += tVel.z * shipT.right;
            // } else {
            //     thrust = Vector3.zero;
            // }
        }
        lposHold = posHold;

        if((localPlayerControlled || aIControlled) && active) { // Apply the control form the helm to the ship
            ship.thrust(thrust);
            ship.rotate(rot);
        }
    }

    /// <summary>
    /// Returns a vector of the highest magnitude for each component. I think?
    /// </summary>
    /// <param name="b">Vector 1</param>
    /// <param name="o">Vector 2</param>
    /// <returns>Combination of the 2 vectors</returns>
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

    /// <summary>
    /// Not sure what this does
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private float getDiff(float a, float b, float max) {
        float d0 = a - b;
        float d1 = (a+max) - b;
        if(Mathf.Abs(d1) < Mathf.Abs(d0)) {
            return d1;
        }
        return d0;
    }
    /// <summary>
    /// No longer needed
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    private float pID(float v) {
        return 2f * Mathf.Log((v/8f)+1);
    }


}
