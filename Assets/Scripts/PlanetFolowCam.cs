using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlanetFolowCam : MonoBehaviour {

    public GameObject obj;
    public float dist;

    void Start(){

    }


    void Update() {
        if(obj != null) {
            transform.position = obj.transform.position + new Vector3(0,dist,0);
        }
    }
}
