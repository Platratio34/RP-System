using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(JumpDrive))]
public class JumpDriveEditor : EquipmentEditor {

    protected JumpDrive jD;
    private Vector3 tPos;

    protected new void OnEnable() {
        base.OnEnable();
        jD = (JumpDrive)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("- Jump Drive -");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("opereratingIn"), new GUIContent("Operating Power (W)"));
        // jD.opereratingIn = EditorGUILayout.FloatField("Operating Power (W)", jD.opereratingIn);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxIn"), new GUIContent("Max Charging (W)"));
        // jD.maxIn = EditorGUILayout.FloatField("Max Charging (W)", jD.maxIn);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("overchargeRate"), new GUIContent("Passive Charging (W)"));
        // jD.overchargeRate = EditorGUILayout.FloatField("Passive Charging (W)", jD.overchargeRate);

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("currentStored"), new GUIContent("Current Stored (J)"));
        // jD.currentStored = EditorGUILayout.FloatField("Current Stored (J)", jD.currentStored);
        EditorGUILayout.LabelField("Current Required", jD.curReq + " W");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxStored"), new GUIContent("Max Power Stored (J)"));
        // jD.maxStored = EditorGUILayout.FloatField("Max Power Stored (J)", jD.maxStored);
        if(jD.state == 1) {
            String tTC = "";
            float sTC = jD.getTimeToCharged();
            if(sTC != float.PositiveInfinity && sTC < TimeSpan.MaxValue.TotalSeconds) {
                tTC = TimeSpan.FromSeconds(sTC).ToString(@"hh\:mm\:ss");
            } else {
                tTC = "Infinity";
            }
            EditorGUILayout.LabelField("Time to charged", tTC);
        }

        EditorGUILayout.Space();

        tPos = EditorGUILayout.Vector3Field("Target Position", tPos);
        EditorGUILayout.LabelField("Power for target", jD.calcEnergy(tPos) + " J");
        if(jD.state != 3) {
            if(GUILayout.Button("Prep for Jump")) {
                if(!jD.prepForJump(tPos)) EditorGUILayout.LabelField("Invalid Jump");
            }
        }
        if(jD.state == 2) {
            if(GUILayout.Button("Jump!")) {
                if(!jD.startJump()) {
                    EditorGUILayout.LabelField("Invalid Jump");
                }
            }
        }
        EditorGUILayout.LabelField("Jump state", jD.state+"");

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("ship"), new GUIContent("Ship"));
        // jD.ship = (GameObject)EditorGUILayout.ObjectField(jD.ship, typeof(GameObject), true);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("effecincy"), new GUIContent("Efficiency"));
        // jD.effecincy = EditorGUILayout.FloatField("Effecincy", jD.effecincy);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("totalMass"), new GUIContent("Total Mass (metric Tonne)"));
        // jD.totalMass = EditorGUILayout.FloatField("Total Mass (metric Tonne)", jD.totalMass);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("dischargeTime"), new GUIContent("Discharge Time"));
        // jD.dischargeTime = EditorGUILayout.FloatField("Discharge Time", jD.dischargeTime);

        base.serializedObject.ApplyModifiedProperties();
    }
}
