using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIPScreen : MonoBehaviour {

    public Camera cam;
    public bool vis;
    Renderer renderer;


    // Start is called before the first frame update
    void Start() {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update() {
        cam.enabled = renderer.isVisible;
        vis = renderer.isVisible;
        // if(renderer.isVisible) {
        //     cam.enabled = true;
        // }
    }

    // void OnBecameVisible() {
    //     cam.enabled = true;
    //     Debug.Log("Vis");
    //     vis = true;
    //     // GetComponent<Renderer>().enabled = true;
    // }
    // void OnBecameInvisible() {
    //     cam.enabled = false;
    //     Debug.Log("Invis");
    //     vis = false;
    //     // GetComponent<Renderer>().enabled = false;
    // }
}
