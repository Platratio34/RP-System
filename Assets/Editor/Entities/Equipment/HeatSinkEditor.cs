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

        hs.maxHeatIn = EditorGUILayout.FloatField("Max heat in", hs.maxHeatIn);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("sources"), new GUIContent("Sources"));

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Heat in", hs.getHeatIn()+" C/s");

        base.serializedObject.ApplyModifiedProperties();
    }
}
