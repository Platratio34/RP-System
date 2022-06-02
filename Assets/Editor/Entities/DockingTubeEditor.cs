using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(DockingTube))]
public class DockingTubeEditor : InteractableEditor {

    protected DockingTube dt;

    protected new void OnEnable() {
        base.OnEnable();
        dt = (DockingTube)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("- Docking Tube -");
        
        dt.manual = EditorGUILayout.Toggle("Manual Mode", dt.manual);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Auto");
        EditorGUI.indentLevel++;
        dt.open = EditorGUILayout.Toggle("Open", dt.open);
        dt.extend = EditorGUILayout.Toggle("Extend", dt.extend);
        EditorGUI.indentLevel--;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Manual");

        dt.fPos = EditorGUILayout.IntSlider("Floor Extension", dt.fPos, 0, 3);
        dt.floorAnimator = (ObjAnimator)EditorGUILayout.ObjectField("Floor Animator", (UnityEngine.Object)dt.floorAnimator, typeof(ObjAnimator), true);
        dt.exPos = EditorGUILayout.IntSlider("Ring Extension", dt.exPos, 0, 2);
        dt.ringAnimator = (ObjAnimator)EditorGUILayout.ObjectField("Ring Animator", (UnityEngine.Object)dt.ringAnimator, typeof(ObjAnimator), true);
        dt.door = EditorGUILayout.Toggle("Door", dt.door);
        dt.doorAnimator = (ObjAnimator)EditorGUILayout.ObjectField("Door Animator", (UnityEngine.Object)dt.doorAnimator, typeof(ObjAnimator), true);

        EditorGUILayout.Space();
        // iD.invertAnimation = EditorGUILayout.Toggle("Invert Animation", iD.invertAnimation);
        // iD.animator = (ObjAnimator)EditorGUILayout.ObjectField("Object Animator", (UnityEngine.Object)iD.animator, typeof(ObjAnimator), true);

        base.serializedObject.ApplyModifiedProperties();
    }
}
