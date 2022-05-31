using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EID : Interactable {

    public int state = 0;
    private float cState = 0;
    public float pushTime = 0.5f;
    private float pTime = 0f;
    public EidType type = EidType.TOGGLE;
    public int maxVal = 10;
    public EidEvent onChangeEvent;
    public Vector3 pos0;
    public Vector3 rot0;
    public Vector3 posF;
    public Vector3 rotF;
    private Vector3 sPos;
    private Vector3 sRot;
    public float moveTime = 0.25f;
    public ObjAnimator animator;
    public float mTarget = 1f;

    public override void OnInteract(bool gm, int b) {
        if(type == EidType.PUSH) {
            state = 1;
            pTime = pushTime;
        } else if(type == EidType.TOGGLE) {
            if(state == 0) {
                state = 1;
            } else {
                state = 0;
            }
        } else if(type == EidType.DIAL) {
            if(b == 0) state++;
            if(b == 1) state--;
            if(state < 0) state = 0;
            if(state > maxVal) state = maxVal;
        } else if(type == EidType.DIAL_CONT) {
            if(b == 0) state++;
            if(b == 1) state--;
            state %= (maxVal+1);
            if(state < 0) {
                state += maxVal+1;
            }
        }
        // print(this.state);
        EID eid = this;
        // EID eid = new EID();
        onChangeEvent.Invoke((EID)eid);
        if(animator != null) animator.speed = 1;
    }

    protected override void OnEdit() {

    }

    // Start is called before the first frame update
    void Start() {
        sPos = transform.localPosition;
        sRot = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update() {
        if(type == EidType.PUSH) {
            if(state != 0) {
                pTime -= Time.deltaTime;
                if(pTime <= 0) {
                    state = 0;
                    pTime = 0;
                    EID eid = this;
                    onChangeEvent.Invoke((EID)eid);
                }
            }
        }
        if(animator == null) {
            float mA = Time.deltaTime/moveTime;
            if(cState > state) {
                cState -= Mathf.Min(mA, cState-state);
            }
            if(cState < state) {
                cState += Mathf.Min(mA, state-cState);
            }
            if(type == EidType.PUSH || type == EidType.TOGGLE) {
                cState = Mathf.Clamp(cState, 0, 1);
                transform.localPosition = sPos + Vector3.Lerp(pos0, posF, cState);
                transform.localEulerAngles = sRot + Vector3.Lerp(rot0, rotF, cState);
            } else {
                cState = Mathf.Clamp(cState, 0, maxVal);
                transform.localPosition = sPos + Vector3.Lerp(pos0, posF, (float)cState/(float)maxVal);
                transform.localEulerAngles = sRot + Vector3.Lerp(rot0, rotF, (float)cState/(float)maxVal);
            }
        } else {
            if(type == EidType.DIAL) {
                animator.target = state * (animator.steps / (float)maxVal);
            } else if(type == EidType.DIAL_CONT) {
                animator.target = state * (animator.steps / ((float)maxVal+1f));
            } else if(state == 0) {
                animator.target = 0;
            } else {
                animator.target = mTarget;
            }
        }
    }

    public enum EidType {
        PUSH,
        TOGGLE,
        DIAL,
        DIAL_CONT
    }

    public void matchEID(EID eid) {
        if(eid.type == type) {
            state = eid.state;
        }
    }
}

[System.Serializable]
public class EidEvent : UnityEvent<EID> {}
