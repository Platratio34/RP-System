using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using CatchCo;
using System;

[ExecuteInEditMode]
public class Satellite : Entity {

    public Satellite parent;
    /// <value>The altitude of the orbit.</value>
    // public float orbitalAlt;
    /// <value>The inclenation of the orbit. Not implemented</value>
    // public float orbitalInc;
    /// <value>The period of the orbit in seconds.</value>
    public float orbitalPer;
    // [Range(0.0f, 1.0f)]
    public float orbitalPos;
    // [Range(0.5f,1.5f)]
    /// <value>The ecentricity of the orbit. Percent of alt for minimum altitude, inverted for max</value>
    // public float orbitalEct = 1f;
    public float offset;
    public float sOI;
    public float mass = 1f;
    public Orbit orbit;
    public bool disp = true;
    public bool dispPoints = false;
    public Color dispColor;
    public bool focus;
    public bool lockDirToParent = false;
    public bool lookDirToPrograde = false;
    public SphereCollider sOICollider;
    public bool calcPer = false;
    private Vector3 cVel = Vector3.zero;
    private Vector3 lpos;

    void Start() {
        lpos = transform.position;
        CalulateOrbit();
    }

    // [ExposeMethodInEditor]
    public void CalulateOrbit() {
        if(parent != null) {
            if(orbit.period == 0) {
                orbit.period = orbitalPer;
            }
            orbit.Recalulate();
            if(calcPer) {
                calulatePeriod();
            }
        }
    }

    void OnValidate() {
        CalulateOrbit();
    }

    void Update() {
        if(!Application.IsPlaying(this)) {
            if(parent != null) {
                Vector3 p = orbit.GetPointA(offset);
                transform.position = parent.transform.position + p;
                if(lockDirToParent) {
                    transform.LookAt(parent.transform);
                }
                if(lookDirToPrograde) {
                    float t2 = orbitalPos + 0.05f;
                    t2 %= orbit.period;
                    transform.LookAt(orbit.GetPointA( (t2/orbit.period) + offset));
                }
            }
        }
        if(sOICollider != null) {
            sOICollider.radius = sOI;
        }
    }

    void FixedUpdate() {
        if(parent != null && !orbit.inited) {
            CalulateOrbit();
        }
        cVel = transform.position - lpos;
        cVel /= Time.deltaTime;
        orbitalPos += Time.fixedDeltaTime;
        orbitalPos %= orbit.period;
        if(parent != null) {
            Vector3 p = orbit.GetPointA( (orbitalPos/orbit.period) + offset);
            transform.position = parent.transform.position + p;
            if(lockDirToParent) {
                transform.LookAt(parent.transform);
            }
            if(lookDirToPrograde) {
                float t2 = orbitalPos + Time.fixedDeltaTime;
                t2 %= orbit.period;
                transform.LookAt(orbit.GetPointA( (t2/orbit.period) + offset));
            }
        }
    }

    void OnDrawGizmos() {
        if(parent != null && !orbit.inited) {
            CalulateOrbit();
        }
        if(disp && orbit != null && orbit.inited && parent != null) {
            Gizmos.color = dispColor;
            float ts = 0.01f;
            // Debug.Log(GetMaxOP());
            for(float i = 0; i < GetMaxOP(); i += ts) {
                Gizmos.DrawLine(GetOrbitPoint(i, true), GetOrbitPoint(i+ts, true));
            }
            Gizmos.color = Color.green;
            if(dispPoints) {
                for(int i = 0; i < 100; i++) {
                    Gizmos.DrawSphere(parent.transform.position + orbit.GetPointI(i), 0.025f);
                }
                Gizmos.color = Color.green;
                Gizmos.DrawLine(parent.transform.position, GetOrbitPoint(0, true, false));
                Gizmos.color = dispColor;
                Gizmos.DrawLine(parent.transform.position, GetOrbitPoint(orbit.period/2f, true, false));
            }
        } else if(disp && parent != null) {
            if(orbit == null) {
                Debug.Log("Orbit null, " + gameObject.name);
            } else if(!orbit.inited) {
                Debug.Log("Orbit not calculated, " + gameObject.name);
            }
        }
        if(disp) {
            if(sOI > 0) {
                Gizmos.color = dispColor;
                Gizmos.DrawWireSphere(transform.position, sOI);
            }
        }
        DrawGizmos();
    }
    protected virtual void DrawGizmos() {

    }

    public Vector3 GetOrbitPoint(float t, bool foc) {
        return GetOrbitPoint(t, foc, true);
    }
    public Vector3 GetOrbitPoint(float t, bool foc, bool off) {
        return GetOrbitPoint(t, foc, off , true);
    }
    public Vector3 GetOrbitPoint(float t, bool foc, bool off, bool f) {
        t %= orbit.period;
        if(parent != null && !(orbit == null || !orbit.inited || orbit.period <= 0)) {
            if(focus && foc && f) {
                if(off) {
                    return orbit.GetPointA( (t / orbit.period) + offset) + parent.GetOrbitPoint(t, foc, true, false);
                } else {
                    return orbit.GetPointA(t / orbit.period) + parent.GetOrbitPoint(t, foc, true, false);
                }
            }
            if(focus && foc) {
                return transform.position;
            }
            if(off) {
                return orbit.GetPointA( (t / orbit.period) + offset) + parent.GetOrbitPoint(t, foc, true, false);
            } else {
                return orbit.GetPointA(t / orbit.period) + parent.GetOrbitPoint(t, foc, true, false);
            }
        }
        return transform.position;
    }

    public float getTimeForPos(Vector3 pos) {
        if(parent != null) {
            Vector3 dPos = parent.transform.position - pos;
            float a = Vector3.Angle(new Vector3(0,0,1), dPos);
            a /= 360f;
            return a - 0.25f;
        } else {
            return 0f;
        }
    }

    public float GetMaxOP() {
        return GetMaxOP(true);
    }
    public float GetMaxOP(bool f) {
        if(parent != null) {
            if(focus) {
                if(f) {
                    float p = parent.GetMaxOP(false);
                    return (orbit.period < p) ? p : orbit.period;
                } else {
                    return 0    ;
                }
            } else {
                float p = parent.GetMaxOP(false);
                return (orbit.period < p) ? p : orbit.period;
            }
        } else {
            return 0;
        }
    }

    public void calulatePeriod() {
        if(parent != null) {
            orbit.period = orbit.calcOrbitalPeriod(parent.mass);
        } else {
            orbit.period = 1f;
        }
    }

    public void CalulateOrbit(Vector3 vel) {
        if(parent == null) {
            Debug.LogError("Satellite must have parent to calulate orbit");
            return;
        }
        float mu = parent.mass * 6.674f * Mathf.Pow(10, -11);
        Vector3 r = parent.transform.position - transform.position;
        Vector3 h = Vector3.Cross(r, vel);
        Vector3 nH = Vector3.Cross(new Vector3(0,0,1),h);

        float vm2 = (vel.magnitude*vel.magnitude);
        Vector3 eV = ( ( vm2 - (mu/r.magnitude) ) * r - Vector3.Dot(r,vel)*vel ) / mu;
        float e = eV.magnitude;

        float E = (vm2/2f) - (mu/r.magnitude);
        float a = 0f;
        if(e < 1) {
            a = -mu / (2f*E);
            float p = a * (1 - (e*e) );
        }

        float i = Mathf.Acos(h[1]/h.magnitude);

        // float Omega = Mathf.Acos(nH[0] / nH.magnitude);

        float argp = Mathf.Acos(Vector3.Dot(nH,eV) / (nH.magnitude*e) );

        if(eV[2] > 0) {
            argp = -argp;
        }

        // float nu = Mathf.Acos(Vector3.Dot(eV,r) / (e*r.magnitude) );
        //
        // if(Vector3.Dot(r,vel) < 0) {
        //     nu = -nu;
        // }

        orbitalPos = 0;
        orbit.ect = e;
        orbit.inc = i/Mathf.PI;
        orbit.SetSemiMajorAxis(a);
        argp /= Mathf.PI*2f;
        orbit.majorAxis = argp;
        orbit.Recalulate();
    }

    public Vector3 getVel() {
        return new Vector3(cVel.x, cVel.y, cVel.z);
    }
}

[System.Serializable]
public class Orbit {

    private float[] aPos;
    public float alt;
    [Range(-1f,1f)]
    public float inc;
    [Range(0.0f,0.5f)]
    public float ect;
    [Range(-0.5f,0.5f)]
    public float majorAxis;
    [System.NonSerialized]
    public bool inited = false;
    public bool debug = false;
    public float period;

    public Orbit(float altitude, float inclination, float ecentricity) {
        alt = altitude;
        inc = inclination;
        ect = ecentricity;
        inited = false;
        Recalulate();
    }

    public void Recalulate() {
        inited = false;
        if(alt > 0) {
            if(debug) {
                Debug.Log("Starting Orbit calculation");
            }
            Log log = new Log("Orbit Recalculation");
            log.saveLocation = "OR_1";
            log.format = log.format + "ff";

            aPos = new float[100];
            aPos[0] = 0f;
            aPos[aPos.Length/2] = 0.5f;

            float tA = Mathf.PI * GetSemiMajorAxis(false) * GetSemiMajorAxis(true);
            // float tA = CalcAreaOfSector(0f, 0.5f) + CalcAreaOfSector(0.5f, 1f);
            float dA = tA / (aPos.Length + (4f * ect) );
            float p = 0.0f;
            float lp = 0.0f;

            if(debug) {
                log.LogMsg("Alt: " + alt + ", Inc: " + inc + ", Ect: " + ect + ", dA=" + dA + ", tA=" + tA);
            }
            for(int i = 1; i < aPos.Length/2f; i++) {
                Log log2 = new Log("Orbit Recalculation, " + i);
                log2.saveLocation = "OR/OR_1-" + i;
                log2.format = log.format;
                if(debug) {
                    log2.LogMsg("Alt: " + alt + ", Inc: " + inc + ", Ect: " + ect + ", dA=" + dA);
                    p = CalcNextPoint(lp, dA, log2);
                } else {
                    p = CalcNextPoint(lp, dA);
                }
                float A = CalcAreaOfSector(lp, p);
                lp = p;
                aPos[i] = p;
                aPos[aPos.Length - i] = 1 - p;
                if(debug) {
                    log.LogMsg(i + ": (" + p + "), " + (aPos.Length - i) + ": (" + (1 - p) + "), A=" + A);
                    log2.LogMsg("(" + p + "), " + (aPos.Length - i) + ": (" + (1 - p) + "), A=" + A);
                    log2.Save();
                }
            }
            if(period == 0) {
                Debug.LogWarning("You should set the period of the orbit before calulating the orbit");
                period = 1;
            }
            inited = true;
            if(debug) {
                log.Save();
                Debug.Log("Orbital Calculation complete");
            }
        } else {
            if(debug) {
                Debug.LogError("Didn't calulate orbit of altitde 0");
            }
        }
    }

    public float calcOrbitalPeriod(float M) {
        if(M == 0) {
            Debug.LogError("Mass of parent body was 0");
            return 1;
        }
        float a = GetSemiMajorAxis();
        float G = 6.674f * Mathf.Pow(10, -11);
        // Debug.Log("VarLog: {M="+M+"; a="+a+"; G="+G+"}");
        return (float)(2f * Mathf.PI * Mathf.Sqrt((a*a*a)/(G*M)));
        /*
        T=2pi sqrt(a^3 / GM)
        */
    }

    private float CalcAreaOfSector(float p, float p2) {
        float r = OrbitalDist(p);
        return 0.5f * (r * r) * ( Mathf.Abs(p2 - p) * 2 * Mathf.PI);
    }

    /*
        r = OrbitalDist(lp);
        A = 0.5f * (r * r) * ( Mathf.Abs(p2 - p) * 8 * Mathf.PI);
        A / (0.5f * r * r * 8 * Mathf.PI) = p2 - p
        d = A / (0.5f * r * r * 8 * Mathf.PI);
    */
    private float CalcNextPoint(float lp, float rA) {
        float r = OrbitalDist(lp);
        float rd = rA / (0.5f * r * r * 2 * Mathf.PI);
        float p = lp + rd;

        return p;
    }
    private float CalcNextPoint(float lp, float rA, Log log) {
        float r = OrbitalDist(lp);
        float rd = rA / (0.5f * r * r * 2 * Mathf.PI);
        float p = lp + rd;
        log.LogMsg("Dist=" + rd);
        log.LogMsg("Final point; p=" + p + ", d=" + rd + ", A=" + CalcAreaOfSector(lp, p));
        log.Save();

        return p;
    }

    private Vector3 CalcPoint(float p) {
        Vector3 point = new Vector3();
        float d = OrbitalDist(p);
        point.x = d * Mathf.Cos( 2 * Mathf.PI * (p + majorAxis) );
        point.z = d * Mathf.Sin( 2 * Mathf.PI * (p + majorAxis) );

        // float mA = majorAxis * 2f * Mathf.PI;
        // float iA = inc * 0.5f * Mathf.PI;
        // Matrix rM = new Matrix(new float[,] {
        //     {Mathf.Cos(mA), 0, Mathf.Sin(mA)},
        //     {0, 1, 0},
        //     {-Mathf.Sin(mA), 0, Mathf.Cos(mA)}
        // });
        // Matrix rI = new Matrix(new float[,] {
        //     {Mathf.Cos(iA), Mathf.Sin(iA), 0},
        //     {-Mathf.Sin(iA), Mathf.Cos(iA), 0},
        //     {0, 0, 1}
        // });
        // Matrix rT = rM;

        // point.x = (point.x * rT.arr[0,0]) + (point.y * rT.arr[1,0]) + (point.z * rT.arr[2,0]);
        // point.y = (point.x * rT.arr[0,1]) + (point.y * rT.arr[1,1]) + (point.z * rT.arr[2,1]);
        // point.z = (point.x * rT.arr[0,2]) + (point.y * rT.arr[1,2]) + (point.z * rT.arr[2,2]);

        return point;
    }

    private float OrbitalDist(float p) {
        float b = 1f + (ect * Mathf.Cos(p * 2f * Mathf.PI) );
        return alt / b;
    }

    public float GetSemiMajorAxis(bool b = false) {
        if(!b) {
            return (alt) / (1f - (ect * ect) );
        } else {
            return (alt) / Mathf.Sqrt(1f - (ect * ect) );
        }
    }

    // (alt) / (1f - (ect * ect) )
    public void SetSemiMajorAxis(float a) {
        alt = a * (1f - (ect * ect) );
    }

    public Vector3 GetMajorAxis() {
        Vector3 point = new Vector3();
        point.x = alt * Mathf.Cos( 2 * Mathf.PI * (majorAxis) );
        point.z = alt * Mathf.Sin( 2 * Mathf.PI * (majorAxis) );
        return point;
    }

    public Vector3 GetPointA(float a) {
        if(inited && aPos.Length != 0) {
            // Debug.Log(a);
            if(a < 0) {
                a += 1;
            }
            a *= aPos.Length;
            a %= aPos.Length;
            if(a == Single.PositiveInfinity  || a == Single.NaN) {
                Debug.LogError("Infinity or NaN value for point not alowed! a=" + a);
                return Vector3.zero;
            }
            int i = (int)a;
            float t = a - i;
            if(i < 0) {
                Debug.LogError("Negative value for point not alowed! i=" + i + ", a=" + a);
                return Vector3.zero;
            } else if(i > aPos.Length) {
                Debug.LogError("Value for point too big! i=" + i + ", max=" + (aPos.Length - 1) );
                return Vector3.zero;
            }
            if(i+1 < aPos.Length) {
                return CalcPoint(Mathf.Lerp(aPos[i], aPos[i+1], t));
            } else if(i < aPos.Length) {
                return CalcPoint(Mathf.Lerp(aPos[i], 1, t));
            } else {
                return CalcPoint(aPos[0]);
            }
        } else {
            if(inited) {
                Debug.LogError("aPos not made, " + aPos.Length);
                Recalulate();
            } else {
                Debug.Log("Not inited");
                Recalulate();
            }
            return Vector3.zero;
        }
    }

    public Vector3 GetPointI(int i) {
        // i = (int)MathP.Clamp(i, 0f, aPos.Length - 1);
        return CalcPoint( aPos[i] );
    }
}
