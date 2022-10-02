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

        eng.throttle = EditorGUILayout.Slider("Throttle", eng.throttle, 0, 1);
        eng.maxPower = EditorGUILayout.FloatField("Max Power", eng.maxPower);
        eng.maxThrust = EditorGUILayout.FloatField("Max Thrust", eng.maxThrust);
        eng.maxHeatGen = EditorGUILayout.FloatField("Max Heat Gen", eng.maxHeatGen);

        EditorGUILayout.LabelField("Current Thrust", eng.thrust + " N");

        eng.ship = (Ship)EditorGUILayout.ObjectField("Ship", (UnityEngine.Object)eng.ship, typeof(Ship), true);
        eng.dir = EditorGUILayout.Vector3Field("Direction", eng.dir);

        base.serializedObject.ApplyModifiedProperties();
    }
}
