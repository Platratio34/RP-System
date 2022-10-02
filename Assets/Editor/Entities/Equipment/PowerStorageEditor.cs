using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(PowerStorage))]
public class PowerStorageEditor : EquipmentEditor {

    protected PowerStorage ps;

    protected new void OnEnable() {
        base.OnEnable();
        ps = (PowerStorage)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("- Power Storage -");

        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxStored"), new GUIContent("Capacity (J)"));
        // ps.maxStored = EditorGUILayout.FloatField("Capacity (J)", ps.maxStored);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("currentStored"), new GUIContent("Current (J)"));
        // ps.currentStored = EditorGUILayout.FloatField("Current (J)", ps.currentStored);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxIn"), new GUIContent("Max Input (W)"));
        // ps.maxIn = EditorGUILayout.FloatField("Max Input (W)", ps.maxIn);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxOut"), new GUIContent("Max Output (W)"));
        // ps.maxOut = EditorGUILayout.FloatField("Max Output (W)", ps.maxOut);

        base.serializedObject.ApplyModifiedProperties();
    }
}
