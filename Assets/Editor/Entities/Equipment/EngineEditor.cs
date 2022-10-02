using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Engine))]
public class EngineEditor : EquipmentEditor {

    protected Engine eng;

    protected new void OnEnable() {
        base.OnEnable();
        eng = (Engine)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("- Engine -");
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("throttle"), new GUIContent("Throttle"));
        // eng.throttle = EditorGUILayout.Slider("Throttle", eng.throttle, 0, 1);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxPower"), new GUIContent("Max Power"));
        // eng.maxPower = EditorGUILayout.FloatField("Max Power", eng.maxPower);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxThrust"), new GUIContent("Max Thrust"));
        // eng.maxThrust = EditorGUILayout.FloatField("Max Thrust", eng.maxThrust);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxHeatGen"), new GUIContent("SMax Heat Genources"));
        // eng.maxHeatGen = EditorGUILayout.FloatField("Max Heat Gen", eng.maxHeatGen);

        EditorGUILayout.LabelField("Current Thrust", eng.thrust + " N");

        EditorGUILayout.PropertyField(serializedObject.FindProperty("ship"), new GUIContent("Ship"));
        // eng.ship = (Ship)EditorGUILayout.ObjectField("Ship", (UnityEngine.Object)eng.ship, typeof(Ship), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("dir"), new GUIContent("Direction"));
        // eng.dir = EditorGUILayout.Vector3Field("Direction", eng.dir);

        base.serializedObject.ApplyModifiedProperties();
    }
}
