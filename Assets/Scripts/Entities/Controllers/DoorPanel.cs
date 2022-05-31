using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPanel : MonoBehaviour {

    public InteractableDoor door;

    void Awake() {

    }

    void Update() {

    }

    public void onOpen(EID eid) {
        door.onOpen(eid);
    }

    public void onLock(EID eid) {
        door.onLock(eid);
    }
}
