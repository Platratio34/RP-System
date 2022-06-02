using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Engine))]
public class EngineEditor : EquipmentEditor {

    protected Engine eng;

    protected void OnEnable() {
        base.OnEnable();
        eng = (Engine)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("- Engine -");

        eng.throttle = EditorGUILayout.Slider("Throttle", eng.throttle, 0, 1);
        eng.maxPower = EditorGUILayout.FloatField("Max Power", eng.maxPower);
        eng.maxThrust = EditorGUILayout.FloatField("Max Thrust", eng.maxThrust);

        EditorGUILayout.LabelField("Thrust", eng.thrust + " N");

        base.serializedObject.ApplyModifiedProperties();
    }
}
