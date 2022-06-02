using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Entity), true)]
public class EntityEditor : Editor {

    private Entity o;
    private GameObject gO;
    private bool saveChange = true;
    // private bool objs = true;

    void OnEnable() {
        o = (Entity)target;
        gO = o.gameObject;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
    }
}
