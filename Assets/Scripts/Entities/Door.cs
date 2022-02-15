using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Entity {

    [Header("Door")]
    public bool closed = true;
    public Vector3[] positions;
    public float maxTime;
    private float time;
    public GameObject doorObject;
    public ObjAnimator animator;

    public override void OnStart() {
        
    }

    void Update() {
        if(closed) {
            if(time > 0) {
                time -= Time.deltaTime;
            } else if(time < 0) {
                time  = 0;
            }
        } else {
            if(time < maxTime) {
                time += Time.deltaTime;
            } else if(time > maxTime) {
                time  = maxTime;
            }
        }
        if(animator != null) {
            animator.speed = closed ? -1 : 1;
        }
        if(doorObject != null) {
            doorObject.transform.localPosition = new Vector3(Mathf.Lerp(positions[0].x, positions[1].x, time/maxTime), Mathf.Lerp(positions[0].y, positions[1].y, time/maxTime), Mathf.Lerp(positions[0].z, positions[1].z, time/maxTime));
            transform.GetComponent<BoxCollider>().center = new Vector3(Mathf.Lerp(positions[0].x, positions[1].x, time/maxTime), Mathf.Lerp(positions[0].y, positions[1].y, time/maxTime), Mathf.Lerp(positions[0].z, positions[1].z, time/maxTime));
        }
    }

    protected override void OnEdit() {
        closed = eParams.GetParam("closed").valueB;
    }

    public override void OnInteract(bool gm) {
        closed = !closed;
        eParams.GetParam("closed").valueB = closed;
    }
}
