using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Reactor))]
public class ReactorEditor : EquipmentEditor {

    protected Reactor rt;

    protected new void OnEnable() {
        base.OnEnable();
        rt = (Reactor)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("- Reactor -");

        EditorGUILayout.PropertyField(serializedObject.FindProperty("controlLevel"), new GUIContent("Control Level"));
        // rt.controlLevel = EditorGUILayout.Slider("Control Level", rt.controlLevel, 0, 1);
        EditorGUILayout.LabelField("Level", Math.Round(rt.getActualLevel()*100, 2) + "%");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("controlSpeed"), new GUIContent("Control Time"));
        // rt.controlSpeed = EditorGUILayout.FloatField("Control Time", rt.controlSpeed);

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("powerGenTotal"), new GUIContent("Max Power Gen"));
        // rt.powerGenTotal = EditorGUILayout.FloatField("Max Power Gen", rt.powerGenTotal);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("powerCurve"), new GUIContent("Power Curve"));
        // rt.powerCurve = EditorGUILayout.CurveField("Power Curve", rt.powerCurve);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("heatGenTotal"), new GUIContent("Max Heat Gen"));
        // rt.heatGenTotal = EditorGUILayout.FloatField("Max Heat Gen", rt.heatGenTotal);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("heatCurve"), new GUIContent("Heat Curve"));
        // rt.heatCurve = EditorGUILayout.CurveField("Heat Curve", rt.heatCurve);

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("overheatTemp"), new GUIContent("Overheat Temperature"));
        // rt.overheatTemp = EditorGUILayout.FloatField("Overheat Tempature", rt.overheatTemp);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxOverheatTime"), new GUIContent("Max Overheat Time"));
        // rt.maxOverheatTime = EditorGUILayout.FloatField("Max Overheat Time", rt.maxOverheatTime);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("damageMod"), new GUIContent("Damage Mod"));
        // rt.damageMod = EditorGUILayout.FloatField("Damage Mod", rt.damageMod);

        if(rt.overheating) {
            EditorGUILayout.LabelField("Overheating!");
            EditorGUILayout.LabelField("Time", rt.overheatingTime+"s");
        }

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("modifyEmission"), new GUIContent("Modify Emission"));
        if(rt.modifyEmission) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("minEmission"), new GUIContent("- Min"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maxEmission"), new GUIContent("- Max"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("emissionRenderer"), new GUIContent("- Renderer"));
        }

        base.serializedObject.ApplyModifiedProperties();
    }
}
