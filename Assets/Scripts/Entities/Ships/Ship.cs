using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ship : Entity {

    public Satellite orbit;
    private Satellite cSat;
    public Vector3 vel;
    private Vector3 pPos;
    public Vector3 accl;

    public Vector3 rVel;
    public Vector3 rAccl;

    public Vector3 effectiveAccl;
    public Vector3 effectiveRot = Vector3.one;

    public ThrustArray thrustArray;

    void Update() {
        // if(orbit.parent!=null) {
        //     Vector3 dPos = transform.position - pPos;
        //     dPos /= Time.deltaTime;
        //     vel = dPos;
        //     pPos = transform.position;
        // } else {
        vel += accl * Time.deltaTime;
        Vector3 mVel = new Vector3(vel.x,vel.y,vel.z);
        if(cSat != null) {
            mVel -= cSat.getVel();
        }
        transform.position += mVel * Time.deltaTime;

        rVel += rAccl * Time.deltaTime;
        transform.Rotate(rVel * Time.deltaTime);
    }

    void OnTriggerStay(Collider other) {
        checkCollider(other);
    }
    void OnTriggerEnter(Collider other) {
        checkCollider(other);
    }
    void OnTriggerExit(Collider other) {
        Satellite nSat = other.GetComponent<Satellite>();
        if(nSat != null && cSat == nSat) {
            cSat = null;
            // orbit.parent = null;
            // transform.SetParent(null);
            transform.parent = null;
            // vel += nSat.getVel();
        }
    }

    private void checkCollider(Collider other) {
        Satellite nSat = other.GetComponent<Satellite>();
        if(nSat != null && nSat != cSat) {
            if(cSat == null) {
                cSat = nSat;
                // newOrbit();
                // parent object
                // transform.SetParent(other.transform);
                transform.parent = other.transform;
                // vel -= nSat.getVel();
            } else {
                if(cSat.sOI > nSat.sOI) {
                    cSat = nSat;
                    // newOrbit();
                    // parent object
                    // transform.SetParent(other.transform);
                    transform.parent = other.transform;
                    // vel -= nSat.getVel();
                } else {
                    if(Vector3.Distance(transform.position, cSat.transform.position) > cSat.sOI) {
                        cSat = nSat;
                        // newOrbit();
                        // parent object
                        // transform.SetParent(other.transform);
                        transform.parent = other.transform;
                        // vel -= nSat.getVel();
                    }
                }
            }
        }
    }

    private void newOrbit() {
        float dist = Vector3.Distance(transform.position, cSat.transform.position);
        orbit.parent = cSat;
        orbit.orbit = new Orbit(dist, 0, 0);
        orbit.calulatePeriod();
        // orbit.orbitalPos = orbit.getTimeForPos(transform.position);
        return;
    }

    public void thrust(Vector3 dir) {
        accl = Vector3.zero;
        accl += (Mathf.Clamp(dir.x,-1f,1f) * effectiveAccl.x) * transform.forward;
        accl += (Mathf.Clamp(dir.y,-1f,1f) * effectiveAccl.y) * transform.up;
        accl += (Mathf.Clamp(dir.z,-1f,1f) * effectiveAccl.z) * transform.right;
        thrustArray.setT(dir);
    }

    public Vector3 getRelativeVel() {
        Vector3 eV = new Vector3(vel.x,vel.y,vel.z);
        if(isInSOI()) {
            eV -= cSat.getVel();
        }
        Vector3 outV = eV.x * transform.forward;
        outV += eV.y * transform.up;
        outV += eV.z * transform.right;
        return outV;
    }

    public void rotate(Vector3 rot) {
        rAccl = new Vector3(Mathf.Clamp(rot.x,-1f,1f) * effectiveRot.x, Mathf.Clamp(rot.y,-1f,1f) * effectiveRot.y, Mathf.Clamp(rot.z,-1f,1f) * effectiveRot.z);
    }

    public Vector3 makeRel(Vector3 inV) {
        Vector3 outV = inV.x * transform.forward;
        outV += inV.y * transform.up;
        outV += inV.z * transform.right;
        return outV;
    }

    public bool isInSOI() {
        return cSat != null;
    }

    public Vector3 getSatVel() {
        return cSat.getVel();
    }

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
