using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ALController : MonoBehaviour {

    public InteractableDoor interiorDoor;
    public InteractableDoor exteriorDoor;

    [Range(-1,1)]
    public int target = 0;
    public int cState = 0;
    public int state = 0;
    public bool locked;
    public   float time;
    [Range(0,10)]
    public float decompTime = 1f;
    [Range(0,10)]
    public float compTime = 1f;

    void Start() {
        interiorDoor.setOpenO(false);
        exteriorDoor.setOpenO(false);
        interiorDoor.setLock(true);
        exteriorDoor.setLock(true);
    }

    void Update() {
        if(interiorDoor.isOpen()) {
            state = -1;
        } else if(exteriorDoor.isOpen()) {
            state = 1;
        } else {
            state = 0;
        }
        if(target == 1) {
            if(cState == 1) {
                interiorDoor.setOpenO(false);
                exteriorDoor.setOpenO(true);
                cState = 1;
            }
            if(state == 0) {
                cState = 0;
                time += Time.deltaTime;
                if(time >= decompTime) {
                    cState = 1;
                }
            }
            if(state == -1) {
                interiorDoor.setOpenO(false);
                exteriorDoor.setOpenO(false);
                time = 0;
                cState = -1;
            }
        } else if(target == 0) {
            interiorDoor.setOpenO(false);
            exteriorDoor.setOpenO(false);
            if(state == 0) cState = 0;
        } else if(target == -1) {
            if(cState == -1) {
                interiorDoor.setOpenO(true);
                exteriorDoor.setOpenO(false);
                cState = -1;
            }
            if(state == 0) {
                cState = 0;
                time += Time.deltaTime;
                if(time >= compTime) {
                    cState = -1;
                }
            }
            if(state == 1) {
                interiorDoor.setOpenO(false);
                exteriorDoor.setOpenO(false);
                time = 0;
                cState = 1;
            }
        }

    }

    public void onCycleExt(EID eid) {
        if(eid.state == 1 && !locked) {
            target = 1;
        }
    }

    public void onCycleInt(EID eid) {
        if(eid.state == 1 && !locked) {
            target = -1;
        }
    }

    public void onCycleCenter(EID eid) {
        if(eid.state == 1 && !locked) {
            target = 0;
        }
    }

    public void onLockCycle(EID eid) {
        locked = eid.state == 1;
    }

}
