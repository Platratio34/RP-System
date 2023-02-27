using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic class for easy animation of objects
/// </summary>
[ExecuteInEditMode]
public class ObjAnimator : MonoBehaviour {

    /// <summary>
    /// Array of objects to animate
    /// </summary>
    public AnimatedObj[] objs;
    /// <summary>
    /// Animation speed (-1 - 1)
    /// </summary>
    [Range(-1,1)]
    public float speed = 0;
    /// <summary>
    /// Animation time
    /// </summary>
    public float time = 0f;
    /// <summary>
    /// Time per step
    /// </summary>
    public float timeScale = 1f;
    /// <summary>
    /// Number of steps in the animation
    /// </summary>
    public int steps = 1;

    /// <summary>
    /// Target step, use -1 to ignore
    /// </summary>
    public float target = -1f;

    /// <summary>
    /// In the animation loops
    /// </summary>
    public bool loop = false;

    private float lt = float.NegativeInfinity;

    void Start() {

    }

    void Update() {
        if(Application.IsPlaying(this)) {
            if(target != -1f) {
                // target %= steps;
                if(speed > 0) {
                    if(!loop) {
                        if(time > target * timeScale) {
                            speed *= -1f;
                            time += speed * Time.deltaTime;
                            time = Mathf.Clamp(time, timeScale * target, timeScale * steps);
                        } else {
                            time += speed * Time.deltaTime;
                            time = Mathf.Clamp(time, 0, timeScale * target);
                        }
                    } else {
                        float trg2 = target + steps/2f;
                        trg2 *= timeScale;
                        trg2 %= timeScale * steps;
                        float tm2 = time + (timeScale*steps)/2f;
                        if(!(time > trg2 && time > target)) {
                            speed *= -1f;
                            time += speed * Time.deltaTime;
                            time %= timeScale * steps;
                            time = Mathf.Clamp(time, timeScale * target, timeScale * steps);
                        } else {
                            time += speed * Time.deltaTime;
                            time %= timeScale * steps;
                            time = Mathf.Clamp(time, 0, timeScale * target);
                        }
                    }
                } else if(speed < 0) {
                    if(!loop) {
                        if(time < target * timeScale) {
                            speed *= -1f;
                            time += speed * Time.deltaTime;
                            time = Mathf.Clamp(time, 0, timeScale * target);
                        } else {
                            time += speed * Time.deltaTime;
                            time = Mathf.Clamp(time, timeScale * target, timeScale * steps);
                        }
                    } else {
                        float trg2 = target + steps/2f;
                        trg2 *= timeScale;
                        trg2 %= timeScale * steps;
                        float tm2 = time + (timeScale*steps)/2f;
                        if(!(time < trg2 && time < target)) {
                            speed *= -1f;
                            time += speed * Time.deltaTime;
                            time %= timeScale * steps;
                            time = Mathf.Clamp(time, 0, timeScale * target);
                        } else {
                            time += speed * Time.deltaTime;
                            time %= timeScale * steps;
                            time = Mathf.Clamp(time, timeScale * target, timeScale * target);
                        }
                    }
                }
            } else {
                time += speed * Time.deltaTime;
                if(!loop) time = Mathf.Clamp(time, 0, timeScale * steps);
                else time %= timeScale * steps;
            }
        }
        UpdateObjs();
    }

    /// <summary>
    /// Updates all objects to current time state
    /// </summary>
    public void UpdateObjs() {
        float t = time / timeScale;
        if(t == lt) return;
        lt = t;
        for(int i = 0; i < objs.Length; i++) {
            objs[i].Update(t);
        }
    }

    /// <summary>
    /// Retune the maximum time of the animation
    /// </summary>
    /// <returns>Max animation time</returns>
    public bool maxTime() {
        return time == timeScale * steps;
    }
}

/// <summary>
/// Object to be animated
/// </summary>
[System.Serializable]
public class AnimatedObj {
    /// <summary>
    /// The object to animate
    /// </summary>
    public GameObject g;
    /// <summary>
    /// Array of animation states
    /// </summary>
    public ATransform[] p;
    /// <summary>
    /// If the object is show in the editor
    /// </summary>
    public bool show;

    

    /// <summary>
    /// Updates the objects position, rotation, and scale to the time state
    /// </summary>
    /// <param name="t">Animation time</param>
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

/// <summary>
/// Animation step for object
/// </summary>
[System.Serializable]
public class ATransform {
    /// <summary>
    /// The position of the object at the step
    /// </summary>
    public Vector3 position;
    /// <summary>
    /// The rotation of the object at the step
    /// </summary>
    public Vector3 rotation;
    /// <summary>
    /// The scale of the object at the step
    /// </summary>
    public Vector3 scale;

    /// <summary>
    /// Animation curve into the step. From the step numerically before
    /// </summary>
    public AnimationCurve curve;

    /// <summary>
    /// Creates a new ATransform with position and rotation of 0, scale of 1, and basic linear curve
    /// </summary>
    public ATransform() {
        scale = Vector3.one;
        curve = AnimationCurve.Linear(0,0,1,1);
    }
    /// <summary>
    /// Creates a new ATransfrom with givin position, rotation, scale, and basic linear curve
    /// </summary>
    /// <param name="position">Position at step</param>
    /// <param name="rotation">Rotation at step</param>
    /// <param name="scale">Scale at step</param>
    public ATransform(Vector3 position, Vector3 rotation, Vector3 scale) {
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
        curve = AnimationCurve.Linear(0,0,1,1);
    }

    /// <summary>
    /// Creates an identical ATransform
    /// </summary>
    /// <returns>Duplicate ATransform</returns>
    public ATransform Clone() {
        ATransform a = new ATransform();

        a.position.Set(position.x, position.y, position.z);
        a.rotation.Set(rotation.x, rotation.y, rotation.z);
        a.scale.Set(scale.x, scale.y, scale.z);

        a.curve = new AnimationCurve(curve.keys);

        return a;
    }

    /// <summary>
    /// Evaluates the animation curve a time
    /// </summary>
    /// <param name="t">Time</param>
    /// <returns>Point in curve</returns>
    public float Evaluate(float t) {
        return curve.Evaluate(t);
    }
}
