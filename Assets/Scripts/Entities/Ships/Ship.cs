using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Ship Entity, handles acceleration and movement
/// </summary>
public class Ship : Entity {

    /// <summary>
    /// The orbit handler for the ship
    /// </summary>
    public Satellite orbit;
    /// <summary>
    /// Parent body
    /// </summary>
    public Satellite cSat;

    /// <summary>
    /// Current velocity of the ship (m/s)
    /// </summary>
    public Vector3 vel;
    /// <summary>
    /// Current acceleration of the ship (m/s^2)
    /// </summary>
    public Vector3 accl;

    /// <summary>
    /// Current rotational velocity of the ship (deg/s)
    /// </summary>
    public Vector3 rVel;
    /// <summary>
    /// Current rotational acceleration of the ship (deg/s^2)
    /// </summary>
    public Vector3 rAccl;

    /// <summary>
    /// Mass of the space ship in kg
    /// </summary>
    public float mass = 1f;

    // public Vector3 effectiveAccl;
    /// <summary>
    /// Maximum acceleration of the ship
    /// </summary>
    public Vector3B effectiveAccl;
    /// <summary>
    /// Maximum rotational acceleration of the ship
    /// </summary>
    public Vector3 effectiveRot = Vector3.one;

    /// <summary>
    /// All of the ships engines
    /// </summary>
    public EngineArray engineArray;
    /// <summary>
    /// This will go away soon
    /// </summary>
    [Obsolete("No longer needed, will be removed soon")]
    public ThrustArray thrustArray;

    // Called from Unity every tick
    void Update() {
        engineArray.updateAccl(effectiveAccl, mass);

    }

    void FixedUpdate() {

        // if(orbit.parent!=null) {
        //     Vector3 dPos = transform.position - pPos;
        //     dPos /= Time.fixedDeltaTime;
        //     vel = dPos;
        //     pPos = transform.position;
        // } else {
        if(cSat != null) {
            Vector3 dir = cSat.transform.position - transform.position;
            float dist = dir.magnitude;
            float satMass = 6.673e-11f*(cSat.surfaceGravity*(cSat.radius*cSat.radius));
            // accl += dir.normalized * grav/(dist*dist);
            accl += dir.normalized * (6.673e-11f*((satMass)/(dist * dist)));
        }
        vel += accl * Time.fixedDeltaTime;
        accl = Vector3.zero;
        if(vel.magnitude > 10) {
            // float oSpd = vel.magnitude - 1000;
            vel -= vel * Mathf.Pow(vel.magnitude/10000f, 2) * Time.fixedDeltaTime;
        }
        if(vel.magnitude > 1e10) {
            Debug.LogError("TOO FAST!");
            vel = Vector3.zero;
        }
        Vector3 mVel = new Vector3(vel.x,vel.y,vel.z);
        // if(cSat != null) {
        //     mVel -= cSat.getVel();
        // }
        transform.position += mVel * Time.fixedDeltaTime;

        rVel += rAccl * Time.fixedDeltaTime;
        transform.Rotate(rVel * Time.fixedDeltaTime);
    }

    // Called when this object stays in a collider
    void OnTriggerStay(Collider other) {
        checkCollider(other);
    }
    // Called when this object enters a collider
    void OnTriggerEnter(Collider other) {
        checkCollider(other);
    }
    // Called when this object leaves a collider
    void OnTriggerExit(Collider other) {
        Satellite nSat = other.GetComponent<Satellite>();
        if(nSat != null && cSat == nSat) {
            // print("Adding: " + vel + " + " + cSat.getVel());
            // vel += cSat.getVel();
            cSat = null;
            // orbit.parent = null;
            // transform.SetParent(null);
            // transform.parent = null;
            // vel += nSat.getVel();
        }
    }

    /// <summary>
    /// Checks if the collider action is required to modify orbit information
    /// </summary>
    /// <param name="other">The collided that was entered or still in</param>
    private void checkCollider(Collider other) {
        Satellite nSat = other.GetComponent<Satellite>();
        if(nSat != null && nSat != cSat) {
            Debug.Log("Entering SOI");
            if(cSat == null) {
                cSat = nSat;
                // print("Subtracting: " + vel + " - " + cSat.getVel());
                // vel -= cSat.getVel();
                // newOrbit();
                // parent object
                // transform.SetParent(other.transform);
                // transform.parent = other.transform;
                // vel -= nSat.getVel();
            } else {
                if(cSat.sOI > nSat.sOI) {
                    // print("Adding: " + vel + " + " + cSat.getVel());
                    // vel += cSat.getVel();
                    cSat = nSat;
                    // print("Subtracting: " + vel + " - " + cSat.getVel());
                    // vel -= cSat.getVel();
                    // newOrbit();
                    // parent object
                    // transform.SetParent(other.transform);
                    // transform.parent = other.transform;
                    // vel -= nSat.getVel();
                } else {
                    if(Vector3.Distance(transform.position, cSat.transform.position) > cSat.sOI) {
                        // print("Adding: " + vel + " + " + cSat.getVel());
                        // vel += cSat.getVel();
                        cSat = nSat;
                        // print("Subtracting: " + vel + " - " + cSat.getVel());
                        // vel -= cSat.getVel();
                        // newOrbit();
                        // parent object
                        // transform.SetParent(other.transform);
                        // transform.parent = other.transform;
                        // vel -= nSat.getVel();
                    }
                }
            }
        }
        Entity ent = other.GetComponent<Entity>();
        if(ent != null) {
            if(!ent.transform.IsChildOf(transform)) {
                ent.transform.SetParent(transform);
            }
        }
    }

    /// <summary>
    /// Resets the orbit
    /// </summary>
    private void newOrbit() {
        float dist = Vector3.Distance(transform.position, cSat.transform.position);
        orbit.parent = cSat;
        orbit.orbit = new Orbit(dist, 0, 0);
        orbit.calulatePeriod();
        // orbit.orbitalPos = orbit.getTimeForPos(transform.position);
        return;
    }

    /// <summary>
    /// Tells the ship to thrust in a givin direction by percent of maximum acceleration.
    /// </summary>
    /// <param name="dir">The direction to thrust in</param>
    public void thrust(Vector3 dir) {
        // print(dir);
        dir.x = Mathf.Clamp(dir.x, -1f, 1f);
        dir.y = Mathf.Clamp(dir.y, -1f, 1f);
        dir.z = Mathf.Clamp(dir.z, -1f, 1f);
        // thrustArray.setT(dir);
        engineArray.setT(dir);
    }

    /// <summary>
    /// Applies a force (in Newtons) to the ship
    /// </summary>
    /// <param name="force">The force in newtons relative to the direction of the ship</param>
    public void applyForce(Vector3 force) {
        accl += (force.x / mass) * transform.forward;
        accl += (force.y / mass) * transform.up;
        accl += (force.z / mass) * transform.right;
    }

    /// <summary>
    /// Returns the velocity of the ship relative it's direction
    /// </summary>
    /// <returns>Directional velocity of the ship</returns>
    public Vector3 getRelativeVel() {
        Vector3 eV = new Vector3(vel.x,vel.y,vel.z);
        // if(isInSOI()) {
        //     eV -= cSat.getVel();
        // }
        Vector3 outV = eV.x * transform.forward;
        outV += eV.y * transform.up;
        outV += eV.z * transform.right;
        return outV;
    }

    /// <summary>
    /// Applies an rotational acceleration to the ship by percent of maximum acceleration
    /// </summary>
    /// <param name="rot">The acceleration to apply</param>
    public void rotate(Vector3 rot) {
        // print(rot);
        rAccl = new Vector3(Mathf.Clamp(rot.x,-1f,1f) * effectiveRot.x, Mathf.Clamp(rot.y,-1f,1f) * effectiveRot.y, Mathf.Clamp(rot.z,-1f,1f) * effectiveRot.z);
    }

    /// <summary>
    /// Makes a global vector relative to the ship
    /// </summary>
    /// <param name="inV"></param>
    /// <returns></returns>
    public Vector3 makeRel(Vector3 inV) {
        Vector3 outV = inV.x * transform.forward;
        outV += inV.y * transform.up;
        outV += inV.z * transform.right;
        return outV;
    }

    /// <summary>
    /// Checks if the ship is in any SOI of a celestial body
    /// </summary>
    /// <returns></returns>
    public bool isInSOI() {
        return cSat != null;
    }

    /// <summary>
    /// Gets the speed of the parent body
    /// </summary>
    /// <returns></returns>
    public Vector3 getSatVel() {
        return cSat.getVel();
    }

    /// <summary>
    /// Array of engines for cleanliness sake.
    /// </summary>
    [Serializable]
    public class EngineArray {
        /// <summary>
        /// All engines that thrust the ship forward
        /// </summary>
        public Engine[] forward;
        /// <summary>
        /// All engines that thrust the ship backwards
        /// </summary>
        public Engine[] backwards;
        /// <summary>
        /// All engines that thrust the ship right
        /// </summary>
        public Engine[] right;
        /// <summary>
        /// All engines that thrust the ship left
        /// </summary>
        public Engine[] left;
        /// <summary>
        /// All engines that thrust the ship up
        /// </summary>
        public Engine[] up;
        /// <summary>
        /// All engines that thrust the ship down
        /// </summary>
        public Engine[] down;

        /// <summary>
        /// Calculates the effective acceleration based on the active engines
        /// </summary>
        /// <param name="accl">The current effective acceleration (this is modified)</param>
        /// <param name="mass">The mass of the ship</param>
        public void updateAccl(Vector3B accl, float mass) {
            if(forward.Length > 0) {
                accl.xp = 0;
                for(int i = 0; i < forward.Length; i++) {
                    accl.xp += forward[i].maxThrust / mass;
                }
            }
            if(backwards.Length > 0) {
                accl.xn = 0;
                for(int i = 0; i < backwards.Length; i++) {
                    accl.xn += backwards[i].maxThrust / mass;
                }
            }
            
            if(up.Length > 0) {
                accl.yp = 0;
                for(int i = 0; i < up.Length; i++) {
                    accl.yp += up[i].maxThrust / mass;
                }
            }
            if(down.Length > 0) {
                accl.yn = 0;
                for(int i = 0; i < down.Length; i++) {
                    accl.yn += down[i].maxThrust / mass;
                }
            }
            
            if(right.Length > 0) {
                accl.zp = 0;
                for(int i = 0; i < right.Length; i++) {
                    accl.zp += right[i].maxThrust / mass;
                }
            }
            if(left.Length > 0) {
                accl.zn = 0;
                for(int i = 0; i < left.Length; i++) {
                    accl.zn += left[i].maxThrust / mass;
                }
            }
        }

        /// <summary>
        /// Sets the throttle of all engines
        /// </summary>
        /// <param name="v3">The current thrust state</param>
        public void setT(Vector3 v3) {
            if(v3.x > 0) {
                for(int i = 0; i < backwards.Length; i++) {
                    backwards[i].setThrust(0);
                }
                for(int i = 0; i < forward.Length; i++) {
                    forward[i].setThrust(v3.x);
                }
            } else {
                for(int i = 0; i < forward.Length; i++) {
                    forward[i].setThrust(0);
                }
                for(int i = 0; i < backwards.Length; i++) {
                    backwards[i].setThrust(v3.x * -1f);
                }
            }
            
            if(v3.y > 0) {
                for(int i = 0; i < down.Length; i++) {
                    down[i].setThrust(0);
                }
                for(int i = 0; i < up.Length; i++) {
                    up[i].setThrust(v3.y);
                }
            } else {
                for(int i = 0; i < up.Length; i++) {
                    up[i].setThrust(0);
                }
                for(int i = 0; i < down.Length; i++) {
                    down[i].setThrust(v3.y * -1f);
                }
            }
            
            if(v3.z > 0) {
                for(int i = 0; i < left.Length; i++) {
                    left[i].setThrust(0);
                }
                for(int i = 0; i < right.Length; i++) {
                    right[i].setThrust(v3.z);
                }
            } else {
                for(int i = 0; i < right.Length; i++) {
                    right[i].setThrust(0);
                }
                for(int i = 0; i < left.Length; i++) {
                    left[i].setThrust(v3.z * -1f);
                }
            }
        } 
    }

    /// <summary>
    /// This will be removed soon
    /// </summary>
    [Obsolete("No longer needed, will be removed soon")]
    [Serializable]
    public class ThrustArray {
        public Thruster[] forward;
        public Thruster[] backwards;
        public Thruster[] right;
        public Thruster[] left;
        public Thruster[] up;
        public Thruster[] down;


        public Thruster[] pitchP;
        public Thruster[] pitchM;
        public Thruster[] yawP;
        public Thruster[] yawM;
        public Thruster[] rollP;
        public Thruster[] rollM;

        public void set(Vector3 t, Vector3 r) {
            setT(t);
            setR(r);
        }
        public void setT(Vector3 t) {
            if(t.x > 0) {
                for(int i = 0; i < forward.Length; i++) {
                    forward[i].fire(t.x);
                }
                for(int i = 0; i < backwards.Length; i++) {
                    backwards[i].fire(0);
                }
            } else if(t.x < 0) {
                for(int i = 0; i < backwards.Length; i++) {
                    backwards[i].fire(t.x * -1f);
                }
                for(int i = 0; i < forward.Length; i++) {
                    forward[i].fire(0);
                }
            } else {
                for(int i = 0; i < backwards.Length; i++) {
                    backwards[i].fire(0);
                }
                for(int i = 0; i < forward.Length; i++) {
                    forward[i].fire(0);
                }
            }
            if(t.z > 0) {
                for(int i = 0; i < right.Length; i++) {
                    right[i].fire(t.z);
                }
                for(int i = 0; i < left.Length; i++) {
                    left[i].fire(0);
                }
            } else if(t.z < 0) {
                for(int i = 0; i < left.Length; i++) {
                    left[i].fire(t.z * -1f);
                }
                for(int i = 0; i < right.Length; i++) {
                    right[i].fire(0);
                }
            } else {
                for(int i = 0; i < right.Length; i++) {
                    right[i].fire(0);
                }
                for(int i = 0; i < left.Length; i++) {
                    left[i].fire(0);
                }
            }
            if(t.y > 0) {
                for(int i = 0; i < up.Length; i++) {
                    up[i].fire(t.y);
                }
                for(int i = 0; i < down.Length; i++) {
                    down[i].fire(0);
                }
            } else if(t.y < 0) {
                for(int i = 0; i < down.Length; i++) {
                    down[i].fire(t.y * -1f);
                }
                for(int i = 0; i < up.Length; i++) {
                    up[i].fire(0);
                }
            } else {
                for(int i = 0; i < up.Length; i++) {
                    up[i].fire(0);
                }
                for(int i = 0; i < down.Length; i++) {
                    down[i].fire(0);
                }
            }
        }
        public void setR(Vector3 t) {
            if(t.x > 0) {
                for(int i = 0; i < pitchP.Length; i++) {
                    pitchP[i].fire(t.x);
                }
                for(int i = 0; i < pitchM.Length; i++) {
                    pitchM[i].fire(0);
                }
            } else if(t.x < 0) {
                for(int i = 0; i < pitchM.Length; i++) {
                    pitchM[i].fire(t.x * -1f);
                }
                for(int i = 0; i < pitchP.Length; i++) {
                    pitchP[i].fire(0);
                }
            } else {
                for(int i = 0; i < pitchM.Length; i++) {
                    pitchM[i].fire(0);
                }
                for(int i = 0; i < pitchP.Length; i++) {
                    pitchP[i].fire(0);
                }
            }
            if(t.y > 0) {
                for(int i = 0; i < yawP.Length; i++) {
                    yawP[i].fire(t.y);
                }
                for(int i = 0; i < yawM.Length; i++) {
                    yawM[i].fire(0);
                }
            } else if(t.y < 0) {
                for(int i = 0; i < yawM.Length; i++) {
                    yawM[i].fire(t.y * -1f);
                }
                for(int i = 0; i < yawP.Length; i++) {
                    yawP[i].fire(0);
                }
            } else {
                for(int i = 0; i < yawP.Length; i++) {
                    yawP[i].fire(0);
                }
                for(int i = 0; i < yawM.Length; i++) {
                    yawM[i].fire(0);
                }
            }
            if(t.z > 0) {
                for(int i = 0; i < rollP.Length; i++) {
                    rollP[i].fire(t.z);
                }
                for(int i = 0; i < rollM.Length; i++) {
                    rollM[i].fire(0);
                }
            } else if(t.z < 0) {
                for(int i = 0; i < rollM.Length; i++) {
                    rollM[i].fire(t.z * -1f);
                }
                for(int i = 0; i < rollP.Length; i++) {
                    rollP[i].fire(0);
                }
            } else {
                for(int i = 0; i < rollP.Length; i++) {
                    rollP[i].fire(0);
                }
                for(int i = 0; i < rollM.Length; i++) {
                    rollM[i].fire(0);
                }
            }
        }
    }

}
