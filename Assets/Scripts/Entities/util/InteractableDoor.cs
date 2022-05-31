using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : Interactable {

    public bool open;
    public bool locked;
    public bool lockable;
    public bool frozen;
    public bool invertAnimation = false;
    public ObjAnimator animator;

    void Start() {

    }

    void Update() {
        if(!invertAnimation) {
            animator.speed = open ? 1 : -1;
        } else {
            animator.speed = !open ? 1 : -1;
        }
        if(frozen) {
            animator.speed = 0;
        }
        if(eParams.HasParam("open")) {
            eParams.GetParam("open").valueB = open;
        }
        if(eParams.HasParam("locked")) {
            eParams.GetParam("locked").valueB = locked;
        }
        if(eParams.HasParam("lockable")) {
            eParams.GetParam("lockable").valueB = lockable;
        }
        if(eParams.HasParam("frozen")) {
            eParams.GetParam("frozen").valueB = frozen;
        }
    }

    public override void OnInteract(bool gm, int b) {
        if(!locked || gm) {
            open = !open;
        }
        if(eParams.HasParam("open")) {
            eParams.GetParam("open").valueB = open;
        }
    }

    protected override void OnEdit() {
        if(eParams.HasParam("open")) {
            open = eParams.GetParam("open").valueB;
        }
        if(eParams.HasParam("locked")) {
            locked = eParams.GetParam("locked").valueB;
        }
        if(eParams.HasParam("lockable")) {
            lockable = eParams.GetParam("lockable").valueB;
        }
        if(eParams.HasParam("frozen")) {
            frozen = eParams.GetParam("frozen").valueB;
        }
    }

    public override JsonObj Save() {
        JsonObj obj = new JsonObj();
        obj.AddBool("open", open);
        obj.AddBool("locked", locked);
        obj.AddBool("lockable", lockable);
        obj.AddBool("frozen", frozen);
        return obj;
    }

    public override void Load(JsonObj obj) {
        if(obj.ContainsKey("open")) open = obj.GetBool("open");
        if(obj.ContainsKey("locked")) locked = obj.GetBool("locked");
        if(obj.ContainsKey("lockable")) lockable = obj.GetBool("lockable");
        if(obj.ContainsKey("frozen")) frozen = obj.GetBool("frozen");
    }

    public void setOpen(bool open) {
        if(!locked) this.open = open;
    }
    public void setLock(bool locked) {
        if(lockable) this.locked = locked;
    }

    public void onOpen(EID eid) {
        setOpen(eid.state==1);
    }
    public void onLock(EID eid) {
        setLock(eid.state==1);
    }
}
