using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavLights : MonoBehaviour {
    public bool on = true;
    [Range(-1,1)]
    public int dir = 1;
    public bool allOn = false;
    public NavLight[] mLights;
    public float onTime;
    public float offTime;
    private float time;

    void Start() {
        
    }

    void Update() {
        if(on) {
            time += Time.deltaTime * dir;
            if(time < 0) {
                time += onTime + offTime;
            } else {
                time %= onTime + offTime;
            }
            for(int i = 0; i < mLights.Length; i++) {
                mLights[i].Update(time, onTime);
            }
        } else {
            time = 0f;
            for(int i = 0; i < mLights.Length; i++) {
                mLights[i].SetOn(allOn);
            }
        }
    }
}

[System.Serializable]
public class NavLight {
    public Light l;
    public Light[] ls;
    public float offset;

    public void Update(float time, float onTime) {
        if(l != null)
            l.enabled = time > offset && time <= (onTime + offset);
        for(int i= 0; i < ls.Length; i++) {
            ls[i].enabled = time > offset && time <= (onTime + offset);
        }
    }

    public void Off() {
        if(l != null)
            l.enabled = false;
        for(int i= 0; i < ls.Length; i++) {
            ls[i].enabled = false;
        }
    }
    public void SetOn(bool b) {
        if(l != null)
            l.enabled = b;
        for(int i= 0; i < ls.Length; i++) {
            ls[i].enabled = b;
        }
    }
}
