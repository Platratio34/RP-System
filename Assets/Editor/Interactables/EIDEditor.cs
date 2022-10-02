using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(EID))]
public class EIDEditor : InteractableEditor {

    protected EID eid;

    protected new void OnEnable() {
        base.OnEnable();
        eid = (EID)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("- EID -");

        EditorGUILayout.PropertyField(serializedObject.FindProperty("type"), new GUIContent("Type"));
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("state"), new GUIContent("State"));

        if(eid.type == EID.EidType.PUSH)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("pushTime"), new GUIContent("Push Time"));
        if(eid.type == EID.EidType.DIAL || eid.type == EID.EidType.DIAL_CONT)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maxVal"), new GUIContent("Maximum Value"));
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("onChangeEvent"), new GUIContent("On Change Event"));

        base.serializedObject.ApplyModifiedProperties();
    }
}
