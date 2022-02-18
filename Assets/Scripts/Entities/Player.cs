using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Text lookingAtText;

    public override void OnStart() {
        rb.freezeRotation = true;
    }

    void Update() {
        Vector3 xD = new Vector3(0.1f*transform.forward.x,0.1f*transform.forward.y,0.1f*transform.forward.z);
        Vector3 yD = new Vector3(0.1f*transform.right.x,0.1f*transform.right.y,0.1f*transform.right.z);
        bool onGround = Physics.Raycast(transform.position+xD, transform.up*-1, 1.1f);
        onGround = onGround || Physics.Raycast(transform.position-xD, transform.up*-1, 1.1f);
        onGround = onGround || Physics.Raycast(transform.position+yD, transform.up*-1, 1.1f);
        onGround = onGround || Physics.Raycast(transform.position-yD, transform.up*-1, 1.1f);
        if(controlled) {
            float h = Input.GetAxis("Left/Right");
            move.y=h;
            float v = Input.GetAxis("Forward/Backward");
            move.x=v;
            Cursor.lockState = CursorLockMode.Locked;
            MouseAiming();
            if(!onGround) {
                rb.drag = 0.5f;
                rb.AddForce(transform.forward*v*rb.mass*mp/4f, ForceMode.Force);
                rb.AddForce(transform.right*h*rb.mass*mp/4f, ForceMode.Force);
            } else {
                rb.drag = 5f;
                rb.AddForce(transform.forward*v*rb.mass*mp, ForceMode.Force);
                rb.AddForce(transform.right*h*rb.mass*mp, ForceMode.Force);
                if(Input.GetButton("Jump")) {
                    rb.AddForce(transform.up*jp*rb.mass, ForceMode.Force);
                }
            }
            rb.AddForce(transform.up*-5*rb.mass, ForceMode.Force);

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
                    lookingAtText.text = inter.localName.ToString();/*LocalStrings.GetLocalString("interactableNames",inter.dispName)*/;
                } else {
                    lookingAtText.text = inter.dispName;
                }
            } else {
                lookingAtText.text = "";
            }

            if(Input.GetButtonDown("Interact") && inter != null) {
                inter.OnInteract(false);
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
