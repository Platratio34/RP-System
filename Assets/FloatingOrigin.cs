using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingOrigin : MonoBehaviour
{

    public float maxDist = 1000;
    public Transform trans;
    private static Transform WorldOriginTransform;

    // Start is called before the first frame update
    void Start() {
        WorldOriginTransform = transform;
    }

    // Update is called once per frame
    void LateUpdate() {
        Vector3 pos = trans.position;
        if(pos.magnitude > maxDist) {
            transform.position = transform.position - pos;
        }
    }

    public static Vector3 WorldToFloating(Vector3 world) {
        if (WorldOriginTransform == null) return world;
        return WorldOriginTransform.position + world;
    }
}
