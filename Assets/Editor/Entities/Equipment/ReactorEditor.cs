using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Reactor))]
public class ReactorEditor : EquipmentEditor {

    protected Reactor rt;

    protected void OnEnable() {
        base.OnEnable();
        rt = (Reactor)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("- Reactor -");

        rt.controlLevel = EditorGUILayout.Slider("Control Level", rt.controlLevel, 0, 1);
        EditorGUILayout.LabelField("Level", rt.getActualLevel()*100 + "%");
        rt.controlSpeed = EditorGUILayout.FloatField("Control Time", rt.controlSpeed);

        EditorGUILayout.Space();

        rt.powerGenTotal = EditorGUILayout.FloatField("Max Power Gen", rt.powerGenTotal);
        rt.powerCurve = EditorGUILayout.CurveField("Power Curve", rt.powerCurve);

        rt.heatGenTotal = EditorGUILayout.FloatField("Max Heat Gen", rt.heatGenTotal);
        rt.heatCurve = EditorGUILayout.CurveField("Heat Curve", rt.heatCurve);

        EditorGUILayout.Space();

        rt.overheatTemp = EditorGUILayout.FloatField("Overheat Tempature", rt.overheatTemp);
        rt.maxOverheatTime = EditorGUILayout.FloatField("Max Overheat Time", rt.maxOverheatTime);
        rt.damageMod = EditorGUILayout.FloatField("Damage Mod", rt.damageMod);

        if(rt.overheating) {
            EditorGUILayout.LabelField("Overheating!");
            EditorGUILayout.LabelField("Time", rt.overheatingTime+"s");
        }

        base.serializedObject.ApplyModifiedProperties();
    }
}
