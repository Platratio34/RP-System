using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPanel : MonoBehaviour {

    public EID[] openers;
    public EID[] locks;

    public InteractableDoor door;

    private bool fT = true;

    void Awake() {

    }

    void Update() {
        if(fT) {
            for(int i = 0; i < openers.Length; i++) {
                openers[i].onChangeEvent.AddListener(onOpen);
            }
            for(int i = 0; i < locks.Length; i++) {
                locks[i].onChangeEvent.AddListener(onLock);
            }
            fT = true;
        }
    }

    public void onOpen(EID eid) {
        door.setOpen(eid.state==1);
        for(int i = 0; i < openers.Length; i++) {
            openers[i].state = eid.state;
        }
    }

    public void onLock(EID eid) {
        door.setLock(eid.state==1);
        for(int i = 0; i < locks.Length; i++) {
            locks[i].state = eid.state;
        }
    }
}
