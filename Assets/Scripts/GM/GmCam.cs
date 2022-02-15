using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GmCam : Entity {
    
    [Header("GM Camera")]
    public bool orbit = true;

    public float zoom = 0.1f;
    public GameObject cam;
    public GameObject inner;
    [Header("Orbit Camera")]
    public float maxZoom = 100f;
    public float minZoom = 0.005f;
    public AnimationCurve zoomCurve;
    public AnimationCurve moveCurve;
    // [Header("First Person Camera")]

    public override void OnStart() {
        
    }

    void Update() {
        if(!GameMaster.IsPointerOverUIObject()) {
            float z = -Input.GetAxis("Zoom");
            Vector3 m = Vector3.zero;
            m += transform.forward * Input.GetAxis("GM Cam Forward/Backward");
            m += transform.right * Input.GetAxis("GM Cam Right/Left");
            m.y += Input.GetAxis("GM Cam Up/Down");

            if(Input.GetButton("GM Cam Look")) {
                Vector3 r = Vector3.zero;
                r.x += Input.GetAxis("GM Cam Look Up/Down") / 8f;
                r.y += Input.GetAxis("GM Cam Rotate Left/Right") / 10f;
                r.y *= -1f;

                transform.Rotate(new Vector3(0,r.y,0), Space.Self);
                inner.transform.Rotate(new Vector3(r.x,0,0), Space.Self);
            }

            if(Input.GetButtonDown("GM Cam Orbit/Free Switch")) {
                orbit = !orbit;
            }

            if(orbit) {
                if(Input.GetButtonDown("GM Cam Look Down")) {
                    inner.transform.localEulerAngles = new Vector3(90,0,0);
                }
                z *= zoomCurve.Evaluate(zoom);
                cam.transform.localPosition = new Vector3(0, 0, -zoom * maxZoom);
            } else {
                cam.transform.localPosition = Vector3.zero;
            }
            zoom += z;
            zoom = Mathf.Clamp(zoom, minZoom, 1);
            m *= moveCurve.Evaluate(zoom);
            transform.position += m;
        }
    }

    protected override void OnEntitySave() {
        if(customData.ContainsKey("oZoom")) {
            customData["oZoom"] = zoom + "";
        } else {
            customData.Add("oZoom", zoom + "");
        }
    }
    protected override void OnEntityLoad() {
        if(customData.ContainsKey("oZoom")) {
            zoom = float.Parse(customData["oZoom"]);
        } else {
            print("GM Camera [" + entityId + "] missing key: \"oZoom\"");
        }
    }
}
