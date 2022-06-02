using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Reactor))]
public class ReactorEditor : EquipmentEditor {

    protected Reactor rt; 
    // private bool objs = true;

    protected void OnEnable() {
        base.OnEnable();
        rt = (Reactor)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
    }
}
