using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Helm))]
public class HelmEditor : InteractableEditor {

    protected Helm helm;

    protected new void OnEnable() {
        base.OnEnable();
        helm = (Helm)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("- Helm -");

        EditorGUILayout.PropertyField(serializedObject.FindProperty("ship"), new GUIContent("Ship"));
        // helm.ship = (Ship)EditorGUILayout.ObjectField("Ship", (UnityEngine.Object)helm.ship, typeof(Ship), true);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("active"), new GUIContent("Active"));
        // helm.active = EditorGUILayout.Toggle("Active", helm.active);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("localPlayerControlled"), new GUIContent("Local Player Controlled"));
        // helm.localPlayerControlled = EditorGUILayout.Toggle("Local Player Controlled", helm.localPlayerControlled);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("aIControlled"), new GUIContent("AI Controlled"));
        // helm.aIControlled = EditorGUILayout.Toggle("AI Controlled", helm.aIControlled);
        if(helm.aIControlled) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("aII"), new GUIContent("AI Input"));
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("velDampers"), new GUIContent("Lateral Dampers"));
        // helm.velDampers = EditorGUILayout.Toggle("Lateral Dampers", helm.velDampers);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rotDampers"), new GUIContent("Rotational Dampers"));
        // helm.rotDampers = EditorGUILayout.Toggle("Rotational Dampers", helm.rotDampers);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("posHold"), new GUIContent("Hold Position"));
        // helm.posHold = EditorGUILayout.Toggle("HoldPosition", helm.posHold);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("aP"), new GUIContent("Auto Pilot"));
        // helm.aP = (AutoPilot)EditorGUILayout.ObjectField("Auto Pilot", (UnityEngine.Object)helm.aP, typeof(AutoPilot), true);
        EditorGUILayout.BeginHorizontal();
        if(helm.autopilot) {
            GUILayout.Label("Autopilot Status: active");
            if(GUILayout.Button("Deactivate")) {
                helm.autopilot = false;
            }
        } else {
            if(helm.apStopped) {
                GUILayout.Label("Autopilot Status: stopped");
            } else {
                GUILayout.Label("Autopilot Status: inactive");
            }
            if(GUILayout.Button("Activate")) {
                helm.autopilot = true;
            }
        }
        EditorGUILayout.EndHorizontal();
        // helm.autopilot = EditorGUILayout.Toggle("Activate AP", helm.autopilot);

        // rt.controlLevel = EditorGUILayout.Slider("Control Level", rt.controlLevel, 0, 1);
        // EditorGUILayout.LabelField("Level", Math.Round(rt.getActualLevel()*100, 2) + "%");
        // rt.controlSpeed = EditorGUILayout.FloatField("Control Time", rt.controlSpeed);

        // EditorGUILayout.Space();

        // rt.powerGenTotal = EditorGUILayout.FloatField("Max Power Gen", rt.powerGenTotal);
        // rt.powerCurve = EditorGUILayout.CurveField("Power Curve", rt.powerCurve);

        // rt.heatGenTotal = EditorGUILayout.FloatField("Max Heat Gen", rt.heatGenTotal);
        // rt.heatCurve = EditorGUILayout.CurveField("Heat Curve", rt.heatCurve);

        // EditorGUILayout.Space();

        // rt.overheatTemp = EditorGUILayout.FloatField("Overheat Tempature", rt.overheatTemp);
        // rt.maxOverheatTime = EditorGUILayout.FloatField("Max Overheat Time", rt.maxOverheatTime);
        // rt.damageMod = EditorGUILayout.FloatField("Damage Mod", rt.damageMod);

        // if(rt.overheating) {
        //     EditorGUILayout.LabelField("Overheating!");
        //     EditorGUILayout.LabelField("Time", rt.overheatingTime+"s");
        // }

        EditorGUILayout.Vector3Field("Thrust", helm.thrust);

        base.serializedObject.ApplyModifiedProperties();
    }
}
