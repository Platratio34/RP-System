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
        EditorGUILayout.LabelField("Total Power Stored", Mathf.Round(net.getTotalStored()/360f)+" / " + Mathf.Round(net.getMaxStorage()/360f) + " Wh");
        float sTO = net.getTimeToOut();
        String tTO = "";
        if(sTO != float.PositiveInfinity && sTO < TimeSpan.MaxValue.TotalSeconds) {
            tTO = TimeSpan.FromSeconds(sTO).ToString(@"hh\:mm\:ss");
        } else {
            tTO = "Infinity";
        }
        EditorGUILayout.LabelField("Time till out", tTO);
        float sTC = net.getTimeToCharged();
        String tTC = "";
        if(sTC != float.PositiveInfinity && sTC < TimeSpan.MaxValue.TotalSeconds) {
            tTC = TimeSpan.FromSeconds(sTC).ToString(@"hh\:mm\:ss");
        } else {
            tTC = "Infinity";
        }
        EditorGUILayout.LabelField("Time till Charged", tTC);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("equipment"), new GUIContent("Equipment"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("powerStorage"), new GUIContent("Power Storage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("subNets"), new GUIContent("Sub Nets"));

        base.serializedObject.ApplyModifiedProperties();
    }
}
