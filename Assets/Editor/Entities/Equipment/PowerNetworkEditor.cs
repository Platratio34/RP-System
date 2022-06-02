using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(PowerNetwork), true)]
public class PowerNetworkEditor : Editor {

    protected PowerNetwork net;

    protected void OnEnable() {
        net = (PowerNetwork)target;
    }

    public override void OnInspectorGUI() {

        EditorGUILayout.LabelField("Network Stats:");

        EditorGUILayout.LabelField("Total Available", Mathf.Round(net.getTotalAvalib())+" W");
        EditorGUILayout.LabelField("Total Required", Mathf.Round(net.getTotalReq())+" W");
        EditorGUILayout.LabelField("Net Power", Mathf.Round(net.getTotalAvalib()-net.getTotalReq())+" W");

        EditorGUILayout.LabelField("Bat Draw", Mathf.Round(net.getBatUssage())+" W");
        EditorGUILayout.LabelField("Overage", Mathf.Round(net.getOverage())+" W");

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("equipment"), new GUIContent("Equipment"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("powerStorage"), new GUIContent("Power Storage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("subNets"), new GUIContent("Sub Nets"));

        base.serializedObject.ApplyModifiedProperties();
    }
}
