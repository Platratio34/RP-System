using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity {

    public bool controlled;
    public Rigidbody rb;
    public Vector2 move;
    public float jp = 75f;
    public float mp = 20f;
    public float sp = 30f;
    public float turnSpeed = 60f;
    private float crx = 45f;
    public GameObject cameraO;
    public bool firstPers = true;
    public bool cameraLock = true;
    public float interactDist = 3f;
    private Vector3 interP;
    private Vector3 interP2;
    public Text tooltip1;
    public Text tooltip2;

    public override void OnStart() {
        rb.freezeRotation = true;
    }

    void Update() {
        

        if(controlled) {
            // float h = Input.GetAxis("Left/Right");
            // move.y = h;
            // float v = Input.GetAxis("Forward/Backward");
            // move.x = v;
            // Cursor.lockState = CursorLockMode.Locked;
            // MouseAiming();
            // if(!onGround) {
            //     rb.drag = 0.5f;
            //     rb.AddForce(transform.forward*v*mp, ForceMode.Acceleration);
            //     rb.AddForce(transform.right*h*mp*0.5f, ForceMode.Acceleration);
            // } else {
            //     rb.drag = 5f;
            //     rb.AddForce(transform.forward*v*mp, ForceMode.Acceleration);
            //     rb.AddForce(transform.right*h*mp, ForceMode.Acceleration);
            //     if(Input.GetButton("Jump")) {
            //         rb.AddForce(transform.up*jp, ForceMode.Acceleration);
            //     }
            // }
            // rb.AddForce(transform.up*-9.81f, ForceMode.Acceleration);

            Interactable inter = null;

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, interactDist)) {
                interP = hit.point;
                interP2 = transform.position;
                inter = Interactable.GetInteractable(hit.collider.gameObject);
                // print("atemtpting interaction " + interP);
                if(inter != null) {
                    // print("interacting with " + inter.id);
                }
            } else {
                inter = null;
            }

            if(inter != null) {
                if(inter.isNameKey) {
                    tooltip1.text = inter.localName.ToString();/*LocalStrings.GetLocalString("interactableNames",inter.dispName)*/;
                } else {
                    tooltip1.text = inter.dispName;
                }
                if(inter is EID) {
                    EID eid = (EID)inter;
                    if(eid.type == EID.EidType.PUSH || eid.type == EID.EidType.TOGGLE) {
                        tooltip2.text = (eid.state == 1 ? "True" : "False");
                    } else {
                        tooltip2.text = eid.state+"";
                    }
                }
            } else {
                tooltip1.text = "";
                tooltip2.text = "";
            }

            if(Input.GetButtonDown("Interact") && inter != null) {
                inter.OnInteract(false, Input.GetButton("Interact Modifier")?1:0);
            }
        } else {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void FixedUpdate() {
        Vector3 xD = new Vector3(0.1f*transform.forward.x,0.1f*transform.forward.y,0.1f*transform.forward.z);
        Vector3 yD = new Vector3(0.1f*transform.right.x,0.1f*transform.right.y,0.1f*transform.right.z);
        Vector3 xD2 = new Vector3(0.05f*transform.forward.x,0.05f*transform.forward.y,0.05f*transform.forward.z);
        Vector3 yD2 = new Vector3(0.05f*transform.right.x,0.05f*transform.right.y,0.05f*transform.right.z);
        bool onGround = Physics.Raycast(transform.position+xD, transform.up*-1, 1.1f);
        onGround = onGround || Physics.Raycast(transform.position-xD, transform.up*-1, 1.1f);
        onGround = onGround || Physics.Raycast(transform.position+yD, transform.up*-1, 1.1f);
        onGround = onGround || Physics.Raycast(transform.position-yD, transform.up*-1, 1.1f);
        
        onGround = onGround || Physics.Raycast(transform.position+xD2, transform.up*-1, 1.1f);
        onGround = onGround || Physics.Raycast(transform.position+yD2, transform.up*-1, 1.1f);
        onGround = onGround || Physics.Raycast(transform.position-xD2, transform.up*-1, 1.1f);
        onGround = onGround || Physics.Raycast(transform.position-yD2, transform.up*-1, 1.1f);

        if(controlled) {
            float h = Input.GetAxis("Left/Right");
            move.y = h;
            float v = Input.GetAxis("Forward/Backward");
            move.x = v;
            Cursor.lockState = CursorLockMode.Locked;
            MouseAiming();
            if(!onGround) {
                rb.drag = 0.5f;
                if(Input.GetButton("Sprint")) { 
                    rb.AddForce(transform.forward*v*sp/4f, ForceMode.Acceleration);
                } else {
                    rb.AddForce(transform.forward*v*mp/4f, ForceMode.Acceleration);
                }
                rb.AddForce(transform.right*h*mp*0.5f/4f, ForceMode.Acceleration);
            } else {
                rb.drag = 5f;
                if(Input.GetButton("Sprint")) { 
                    rb.AddForce(transform.forward*v*sp, ForceMode.Acceleration);
                } else {
                    rb.AddForce(transform.forward*v*mp, ForceMode.Acceleration);
                }
                rb.AddForce(transform.right*h*mp*0.5f, ForceMode.Acceleration);
                if(Input.GetButton("Jump")) {
                    rb.AddForce(transform.up*jp, ForceMode.Acceleration);
                }
            }
            rb.AddForce(transform.up*-9.81f, ForceMode.Acceleration);
        }
    }

    void OnDrawGizmos() {
        if(interP != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(interP, 0.1f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(interP2, 0.1f);
            Gizmos.DrawLine(interP2, interP);
        }
    }

    void MouseAiming () {
        Cursor.lockState = CursorLockMode.Locked;
        // get the mouse inputs
        float y = Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime;
        if(!firstPers) {
            crx += Input.GetAxis("Mouse Y") * turnSpeed * Time.deltaTime;
            crx = Mathf.Clamp(crx, 0, 90);
        } else {
            crx -= Input.GetAxis("Mouse Y") * turnSpeed * Time.deltaTime;
            crx = Mathf.Clamp(crx, -89, 89);
        }

        // rotate the camera
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + y, 0);
        cameraO.transform.localEulerAngles = new Vector3(crx, cameraO.transform.localEulerAngles.y, cameraO.transform.localEulerAngles.z);
    }

    protected override void OnEntitySave() {

    }
    protected override void OnEntityLoad() {

    }
}
