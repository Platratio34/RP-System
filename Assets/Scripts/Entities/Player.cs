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
    public float mpz = 10f;
    public float spz = 15f;
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
        if(gravitySource != null) {
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;
            Vector3 up = transform.up;
            Vector3 down = up * -1f;

            Vector3 xD = new Vector3(0.75f*forward.x,0.75f*forward.y,0.75f*forward.z);
            Vector3 yD = new Vector3(0.75f*right.x,0.75f*right.y,0.75f*right.z);
            // Vector3 xD2 = new Vector3(0.05f*forward.x,0.05f*forward.y,0.05f*forward.z);
            // Vector3 yD2 = new Vector3(0.05f*right.x,0.05f*right.y,0.05f*right.z);
            bool onGround = Physics.Raycast(transform.position, down, 1.1f);
            onGround = onGround || Physics.Raycast(transform.position+xD, down, 1.1f);
            onGround = onGround || Physics.Raycast(transform.position-xD, down, 1.1f);
            onGround = onGround || Physics.Raycast(transform.position+yD, down, 1.1f);
            onGround = onGround || Physics.Raycast(transform.position-yD, down, 1.1f);
            
            // onGround = onGround || Physics.Raycast(transform.position+xD2, down, 1.1f);
            // onGround = onGround || Physics.Raycast(transform.position+yD2, down, 1.1f);
            // onGround = onGround || Physics.Raycast(transform.position-xD2, down, 1.1f);
            // onGround = onGround || Physics.Raycast(transform.position-yD2, down, 1.1f);

            if(controlled) {
                float h = Input.GetAxis("Left/Right");
                move.y = h;
                float v = Input.GetAxis("Forward/Backward");
                move.x = v;
                Cursor.lockState = CursorLockMode.Locked;
                MouseAiming();
                Vector3 force = Vector3.zero;
                if(!onGround) {
                    rb.drag = 0.5f;
                    if(Input.GetButton("Sprint")) {
                        force += forward * v * sp / 4f;
                        // rb.AddForce(forward*v*sp/4f, ForceMode.Acceleration);
                    } else {
                        force += forward*v*mp/4f;
                        // rb.AddForce(forward*v*mp/4f, ForceMode.Acceleration);
                    }
                    force += right*h*mp*0.5f/4f;
                    // rb.AddForce(right*h*mp*0.5f/4f, ForceMode.Acceleration);
                } else {
                    rb.drag = 5f;
                    if(Input.GetButton("Sprint")) { 
                        force += forward*v*sp;
                        // rb.AddForce(forward*v*sp, ForceMode.Acceleration);
                    } else {
                        force += forward*v*mp;
                        // rb.AddForce(forward*v*mp, ForceMode.Acceleration);
                    }
                    force += right*h*mp*0.5f;
                    // rb.AddForce(right*h*mp*0.5f, ForceMode.Acceleration);
                    if(Input.GetButton("Jump")) {
                        force += up*jp;
                        // rb.AddForce(up*jp, ForceMode.Acceleration);
                    }
                }
                rb.AddForce(force, ForceMode.Acceleration);
            }
        } else {
            if(controlled) {
                Vector3 forward = transform.forward;
                Vector3 right = transform.right;
                Vector3 up = transform.up;
                Vector3 down = up * -1f;

                float h = Input.GetAxis("Left/Right");
                move.y = h;
                float v = Input.GetAxis("Forward/Backward");
                move.x = v;
                Cursor.lockState = CursorLockMode.Locked;
                MouseAiming();
                rb.drag = 0.5f;
                Vector3 force = Vector3.zero;
                if(Input.GetButton("Sprint")) {
                    force += forward*v*spz;
                    force += right*h*spz;
                    // rb.AddForce(forward*v*spz, ForceMode.Acceleration);
                    // rb.AddForce(right*h*spz, ForceMode.Acceleration);
                } else {
                    force += forward*v*mpz;
                    force += right*h*mpz;
                    // rb.AddForce(forward*v*mpz, ForceMode.Acceleration);
                    // rb.AddForce(right*h*mpz, ForceMode.Acceleration);
                }
                if(Input.GetButton("Jump")) {
                    force += up*mpz;
                    // rb.AddForce(up*mpz, ForceMode.Acceleration);
                }
                if(Input.GetButton("Thrust Down")) {
                    force += down*mpz;
                    // rb.AddForce(down*mpz, ForceMode.Acceleration);
                }
                rb.AddForce(force, ForceMode.Acceleration);
            }
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
        float y = Input.GetAxis("Mouse X") * turnSpeed;
        if(!firstPers) {
            crx += Input.GetAxis("Mouse Y") * turnSpeed;
            crx = Mathf.Clamp(crx, 0, 90);
        } else {
            crx -= Input.GetAxis("Mouse Y") * turnSpeed;
            crx = Mathf.Clamp(crx, -89, 89);
        }

        // rotate the camera
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + y, 0);
        Vector3 cAngle = cameraO.transform.localEulerAngles;
        cameraO.transform.localEulerAngles = new Vector3(crx, cAngle.y, cAngle.z);
    }

    protected override void OnEntitySave() {

    }
    protected override void OnEntityLoad() {

    }
}
