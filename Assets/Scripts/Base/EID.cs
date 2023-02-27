using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Entity Interface Device, used for buttons, switches, dials, and the like
/// </summary>
public class EID : Interactable {

    /// <summary>
    /// Current state of the device
    /// </summary>
    public int state = 0;
    /// <summary>
    /// State used for animation
    /// </summary>
    private float cState = 0;
    /// <summary>
    /// How long the push button should stay pushed
    /// </summary>
    public float pushTime = 0.5f;
    /// <summary>
    /// How long since it was pushed
    /// </summary>
    private float pTime = 0f;
    /// <summary>
    /// They type of device
    /// </summary>
    public EidType type = EidType.TOGGLE;
    /// <summary>
    /// The maximum value of the dial
    /// </summary>
    public int maxVal = 10;
    /// <summary>
    /// Event called every time the state is changed
    /// </summary>
    public EidEvent onChangeEvent;
    public EidEvent onActivateEvent;
    /// <summary>
    /// Position of the device at state 0
    /// </summary>
    public Vector3 pos0;
    /// <summary>
    /// Rotation of the device at state 0
    /// </summary>
    public Vector3 rot0;
    /// <summary>
    /// Position of the device at max state
    /// </summary>
    public Vector3 posF;
    /// <summary>
    /// Rotation of the device at max state
    /// </summary>
    public Vector3 rotF;
    /// <summary>
    /// Stating position of the device, used to make pos0 and posF relative to starting
    /// </summary>
    private Vector3 sPos;
    /// <summary>
    /// Stating rotation of the device, used to make rots0 and rotF relative to starting
    /// </summary>
    private Vector3 sRot;
    /// <summary>
    /// The amount of time it takes to move one state
    /// </summary>
    public float moveTime = 0.25f;
    /// <summary>
    /// Animator for sub-object movement
    /// </summary>
    public ObjAnimator animator;
    /// <summary>
    /// The move target
    /// </summary>
    public float mTarget = 1f;

    private bool firstTick = true;

    public override void OnInteract(bool gm, int b) {
        if(type == EidType.PUSH) {
            state = 1;
            pTime = pushTime;
            onActivateEvent.Invoke((EID)this);
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
        if(firstTick) {
            firstTick = false;
            if(state != 0) {
                if(type == EidType.PUSH) {
                    pTime = pushTime;
                } else if(type == EidType.DIAL) {
                    if(state < 0) state = 0;
                    if(state > maxVal) state = maxVal;
                } else if(type == EidType.DIAL_CONT) {
                    state %= (maxVal+1);
                    if(state < 0) {
                        state += maxVal+1;
                    }
                }
                if(animator != null) animator.speed = 1;
                onChangeEvent.Invoke((EID)this);
            }
        }

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

    /// <summary>
    /// The type of Entity Interaction Device
    /// </summary>
    public enum EidType {
        /// <summary>
        /// Push button, resets after time
        /// </summary>
        PUSH,
        /// <summary>
        /// Toggle button, only changes on input
        /// </summary>
        TOGGLE,
        /// <summary>
        /// Dial from 0 to max
        /// </summary>
        DIAL,
        /// <summary>
        /// Dial, but wraps after max
        /// </summary>
        DIAL_CONT
    }

    /// <summary>
    /// Makes this EID match another EID
    /// </summary>
    /// <param name="eid">The EID to match</param>
    public void matchEID(EID eid) {
        if(eid.type == type) {
            state = eid.state;
        }
    }

    public void rotate(int r) {
        // Debug.Log("Rotating by " + r);
        state += r;
        if(state < 0) {
            if (type == EidType.DIAL_CONT) {
                state += maxVal;
            } else {
                state = 0;
            }
        }
        if(state > maxVal) {
            if (type == EidType.DIAL_CONT) {
                state -= maxVal;
            } else {
                state = maxVal;
            }
        }
    }
}

/// <summary>
/// EID on change event
/// </summary>
[System.Serializable]
public class EidEvent : UnityEvent<EID> {}
