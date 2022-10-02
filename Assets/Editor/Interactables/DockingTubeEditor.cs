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
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("manual"), new GUIContent("Manual Mode"));
        // dt.manual = EditorGUILayout.Toggle("Manual Mode", dt.manual);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Auto");
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("open"), new GUIContent("Open"));
        // dt.open = EditorGUILayout.Toggle("Open", dt.open);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("extend"), new GUIContent("Extend"));
        // dt.extend = EditorGUILayout.Toggle("Extend", dt.extend);
        EditorGUI.indentLevel--;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Manual");

        EditorGUILayout.PropertyField(serializedObject.FindProperty("fPos"), new GUIContent("Floor Extension"));
        // dt.fPos = EditorGUILayout.IntSlider("Floor Extension", dt.fPos, 0, 3);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("floorAnimator"), new GUIContent("Floor Animator"));
        // dt.floorAnimator = (ObjAnimator)EditorGUILayout.ObjectField("Floor Animator", (UnityEngine.Object)dt.floorAnimator, typeof(ObjAnimator), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("exPos"), new GUIContent("Ring Extension"));
        // dt.exPos = EditorGUILayout.IntSlider("Ring Extension", dt.exPos, 0, 2);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ringAnimator"), new GUIContent("Ring Animator"));
        // dt.ringAnimator = (ObjAnimator)EditorGUILayout.ObjectField("Ring Animator", (UnityEngine.Object)dt.ringAnimator, typeof(ObjAnimator), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("door"), new GUIContent("Door"));
        // dt.door = EditorGUILayout.Toggle("Door", dt.door);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("doorAnimator"), new GUIContent("Door Animator"));
        // dt.doorAnimator = (ObjAnimator)EditorGUILayout.ObjectField("Door Animator", (UnityEngine.Object)dt.doorAnimator, typeof(ObjAnimator), true);

        EditorGUILayout.Space();
        // iD.invertAnimation = EditorGUILayout.Toggle("Invert Animation", iD.invertAnimation);
        // iD.animator = (ObjAnimator)EditorGUILayout.ObjectField("Object Animator", (UnityEngine.Object)iD.animator, typeof(ObjAnimator), true);

        base.serializedObject.ApplyModifiedProperties();
    }
}
