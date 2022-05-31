using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ObjAnimator : MonoBehaviour {

    public AnimatedObj[] objs;
    [Range(-1,1)]
    public float speed = 0;
    public float time = 0f;
    public float timeScale = 1f;
    public int steps = 1;

    public float target = -1f;

    void Start() {

    }

    void Update() {
        if(Application.IsPlaying(this)) {
            if(target != -1f) {
                if(speed > 0) {
                    if(time > target * timeScale) {
                        speed *= -1f;
                        time += speed * Time.deltaTime;
                        time = Mathf.Clamp(time, timeScale * target, timeScale * steps);
                    } else {
                        time += speed * Time.deltaTime;
                        time = Mathf.Clamp(time, 0, timeScale * target);
                    }
                } else if(speed < 0) {
                    if(time < target * timeScale) {
                        speed *= -1f;
                        time += speed * Time.deltaTime;
                        time = Mathf.Clamp(time, 0, timeScale * target);
                    } else {
                        time += speed * Time.deltaTime;
                        time = Mathf.Clamp(time, timeScale * target, timeScale * steps);
                    }
                }
            } else {
                time += speed * Time.deltaTime;
                time = Mathf.Clamp(time, 0, timeScale * steps);
            }
        }
        UpdateObjs();
    }

    public void UpdateObjs() {
        for(int i = 0; i < objs.Length; i++) {
            objs[i].Update(time/timeScale);
        }
    }

    public bool maxTime() {
        return time == timeScale * steps;
    }
}

[System.Serializable]
public class AnimatedObj {
    public GameObject g;
    public ATransform[] p;
    public bool show;

    public void Update(float t) {
        int s = (int)Mathf.Floor(t);
        int e = (int)Mathf.Ceil(t);
        s = (int)MathP.Clamp(s, 0, p.Length - 1f);
        e = (int)MathP.Clamp(e, 0, p.Length - 1f);
        float t2 = t-s;

        if(p[s] != null && p[s].curve == null) {
            p[s].curve = AnimationCurve.Linear(0,0,1,1);
        }
        g.transform.localPosition = MathP.VLerp(p[s].position, p[e].position, p[s].Evaluate(t2) );
        g.transform.localEulerAngles = MathP.VLerp(p[s].rotation, p[e].rotation, p[s].Evaluate(t2) );
        g.transform.localScale= MathP.VLerp(p[s].scale, p[e].scale, p[s].Evaluate(t2) );
    }

}

[System.Serializable]
public class ATransform {
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;

    public AnimationCurve curve;

    public ATransform() {
        scale = Vector3.one;
        curve = AnimationCurve.Linear(0,0,1,1);
    }
    public ATransform(Vector3 position, Vector3 rotation, Vector3 scale) {
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
    }

    public ATransform Clone() {
        ATransform a = new ATransform();

        a.position.Set(position.x, position.y, position.z);
        a.rotation.Set(rotation.x, rotation.y, rotation.z);
        a.scale.Set(scale.x, scale.y, scale.z);

        a.curve = new AnimationCurve(curve.keys);

        return a;
    }

    public float Evaluate(float t) {
        return curve.Evaluate(t);
    }
}
