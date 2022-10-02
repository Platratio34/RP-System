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

        ps.maxStored = EditorGUILayout.FloatField("Capacity (J)", ps.maxStored);
        ps.currentStored = EditorGUILayout.FloatField("Current (J)", ps.currentStored);
        ps.maxIn = EditorGUILayout.FloatField("Max Input (W)", ps.maxIn);
        ps.maxOut = EditorGUILayout.FloatField("Max Output (W)", ps.maxOut);

        base.serializedObject.ApplyModifiedProperties();
    }
}
