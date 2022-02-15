using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {

    public bool controlled;
    public Rigidbody rb;
    public Vector2 move;
    public float jp = 6f;
    public float mp = 2f;
    public float turnSpeed = 1f;
    private float crx = 45f;
    public GameObject cameraO;
    public bool firstPers = true;
    public bool cameraLock = true;
    public float interactDist = 3f;
    private Vector3 interP;
    private Vector3 interP2;

    public override void OnStart() {
        rb.freezeRotation = true;
    }

    void Update() {
        bool onGround = Physics.Raycast(transform.position, transform.up*-1, 1.1f);
        float h = Input.GetAxis("Left/Right");
        move.y=h;
        float v = Input.GetAxis("Forward/Backward");
        move.x=v;
        if(controlled) {
            Cursor.lockState = CursorLockMode.Locked;
            MouseAiming();
            if(!onGround) {
                rb.drag = 0.5f;
            } else {
                rb.drag = 5f;
                rb.AddForce(transform.forward*v*rb.mass*mp, ForceMode.Force);
                rb.AddForce(transform.right*h*rb.mass*mp, ForceMode.Force);
                if(Input.GetButton("Jump")) {
                    rb.AddForce(transform.up*jp*rb.mass, ForceMode.Force);
                }
            }
            rb.AddForce(transform.up*-5*rb.mass, ForceMode.Force);

            if(Input.GetButtonDown("Interact")) {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, interactDist)) {
                    interP = hit.point;
                    interP2 = transform.position;
                    Interactable inter = Interactable.GetInteractable(hit.collider.gameObject);
                    // print("atemtpting interaction " + interP);
                    if(inter != null) {
                        // print("interacting with " + inter.id);
                        inter.OnInteract(false);
                    }
                }
            }
        } else {
            Cursor.lockState = CursorLockMode.None;
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
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + y, transform.eulerAngles.z);
        cameraO.transform.eulerAngles = new Vector3(crx, cameraO.transform.eulerAngles.y, cameraO.transform.eulerAngles.z);
    }

    protected override void OnEntitySave() {

    }
    protected override void OnEntityLoad() {

    }
}
