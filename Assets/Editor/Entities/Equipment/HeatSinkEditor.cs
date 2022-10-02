using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(HeatSink))]
public class HeatSinkEditor : EquipmentEditor {

    protected HeatSink hs;

    protected new void OnEnable() {
        base.OnEnable();
        hs = (HeatSink)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("- Heat Sinnk -");
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxHeatIn"), new GUIContent("Max heat in"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("sources"), new GUIContent("Sources"));

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Heat in", hs.getHeatIn()+" C/s");

        base.serializedObject.ApplyModifiedProperties();
    }
}
