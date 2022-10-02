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
        jD.opereratingIn = EditorGUILayout.FloatField("Operating Power (W)", jD.opereratingIn);
        jD.maxIn = EditorGUILayout.FloatField("Max Charging (W)", jD.maxIn);
        jD.overchargeRate = EditorGUILayout.FloatField("Passive Charging (W)", jD.overchargeRate);

        EditorGUILayout.Space();

        jD.currentStored = EditorGUILayout.FloatField("Current Stored (J)", jD.currentStored);
        EditorGUILayout.LabelField("Current Required", jD.curReq + " W");
        jD.maxStored = EditorGUILayout.FloatField("Max Power Stored (J)", jD.maxStored);
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

        jD.ship = (GameObject)EditorGUILayout.ObjectField(jD.ship, typeof(GameObject), true);

        jD.effecincy = EditorGUILayout.FloatField("Effecincy", jD.effecincy);
        jD.totalMass = EditorGUILayout.FloatField("Total Mass (metric Tonne)", jD.totalMass);
        jD.dischargeTime = EditorGUILayout.FloatField("Discharge Time", jD.dischargeTime);

        base.serializedObject.ApplyModifiedProperties();
    }
}
