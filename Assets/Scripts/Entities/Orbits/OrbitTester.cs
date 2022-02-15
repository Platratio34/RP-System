using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OrbitTester : Satellite {

    public Vector3 vel;
    private float sPos;

    void Start() {

    }

    void Update() {
        if(parent != null) {
            CalulateOrbit(vel);
            offset = getTimeForPos(transform.position);
            // float mu = parent.mass * 6.674f * Mathf.Pow(10, -11);
            // Vector3 r = parent.transform.position - transform.position;
            // Vector3 h = Vector3.Cross(r, vel);
            // Vector3 nH = Vector3.Cross(new Vector3(0,0,1),h);
            //
            // float vm2 = (vel.magnitude*vel.magnitude);
            // Vector3 eV = ( ( vm2 - (mu/r.magnitude) ) * r - Vector3.Dot(r,vel)*vel ) / mu;
            // float e = eV.magnitude;
            //
            // float E = (vm2/2f) - (mu/r.magnitude);
            // float a = 0f;
            // if(e < 1) {
            //     a = -mu / (2f*E);
            //     float p = a * (1 - (e*e) );
            // }
            //
            // float i = Mathf.Acos(h[1]/h.magnitude);
            //
            // // float Omega = Mathf.Acos(nH[0] / nH.magnitude);
            //
            // float argp = Mathf.Acos(Vector3.Dot(nH,eV) / (nH.magnitude*e) );
            //
            // if(eV[2] > 0) {
            //     argp = -argp;
            // }
            //
            // // float nu = Mathf.Acos(Vector3.Dot(eV,r) / (e*r.magnitude) );
            // //
            // // if(Vector3.Dot(r,vel) < 0) {
            // //     nu = -nu;
            // // }
            //
            // orbit.ect = e;
            // orbit.inc = i/Mathf.PI;
            // orbit.SetSemiMajorAxis(a);
            // // orbit.majorAxis = (nu-(90))/(360f);
            // // argp -= Mathf.PI;
            // argp /= Mathf.PI*2f;
            // // argp += 0.5f;
            // // print(nu);
            // orbit.majorAxis = argp;
            // orbit.Recalulate();
        }
    }

    public void Calulate(Vector3 vel, Vector3 pos, Orbit orbit, float M) {
        float mu = M * 6.674f * Mathf.Pow(10, -11);
        Vector3 r = pos;
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

        orbit.ect = e;
        orbit.inc = i/Mathf.PI;
        orbit.SetSemiMajorAxis(a);
        argp /= Mathf.PI*2f;
        orbit.majorAxis = argp;
        // sPos = getTimeForPos(transform.position);
        orbit.Recalulate();
    }

    protected override void DrawGizmos() {
        Gizmos.color = dispColor;
        Gizmos.DrawLine(transform.position, vel*10 + transform.position);
        Gizmos.DrawSphere(parent.transform.position + GetOrbitPoint(0, true), 1.1f);
        // Gizmos.DrawWireSphere(parent.transform.position, Vector3.Distance(parent.transform.position, transform.position));
    }
}
