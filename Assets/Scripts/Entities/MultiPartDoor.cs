using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Obsolete("Not used any more, Use normal door and ObjAnimator", false)]
public class MultiPartDoor : Entity {

    [Header("Door")]
    private bool closed = true;
    public float maxTime;
    private float time;
    public DoorPart[] doorParts;
    public Vector3[] colliderPos;
    public Vector3[] colliderScale;

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
        for(int i = 0; i < doorParts.Length; i++) {
            doorParts[i].UpdatePos(time/maxTime);
        }
        transform.GetComponent<BoxCollider>().center = MathP.VLerp(colliderPos[0], colliderPos[1], time/maxTime);
        transform.GetComponent<BoxCollider>().size = MathP.VLerp(colliderScale[0], colliderScale[1], time/maxTime);
    }

    protected override void OnEdit() {
        closed = eParams.GetParam("Closed").valueB;
    }

    public override void OnInteract(bool gm, int b) {
        closed = !closed;
        eParams.GetParam("Closed").valueB = closed;
    }

    [System.Serializable]
    public class DoorPart {
        public GameObject gameObject;
        public Vector3[] positions;

        public void UpdatePos(float t) {
            gameObject.transform.localPosition = MathP.VLerp(positions[0], positions[1], t);
        }
    }


}
