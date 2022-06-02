using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Interactable), true)]
public class InteractableEditor : Editor {

    protected Interactable it;
    protected GameObject gO;
    private bool saveChange = true;
    // private bool objs = true;

    protected void OnEnable() {
        it = (Interactable)target;
        gO = it.gameObject;
    }

    public override void OnInspectorGUI() {

    }
}
