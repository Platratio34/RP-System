using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Equipment), true)]
public class EquipmentEditor : InteractableEditor {

    protected Equipment eq;
    // private bool objs = true;

    protected void OnEnable() {
        base.OnEnable();
        eq = (Equipment)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        bool change = false;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("- Equipment -");
        EditorGUILayout.LabelField("Net Power", Mathf.Round(eq.getNetPower())+" W");
        EditorGUILayout.LabelField("Required Power", Mathf.Round(eq.getPowerReq())+" W");
        EditorGUILayout.LabelField("Net Heat", Math.Round(eq.getNetHeat(),2)+" K/s");
        // EditorGUILayout.LabelField("Passive Heat Disipation", eq.getPasiveHeatDisp()+" K/s");
        eq.setPasiveHeatDisp(EditorGUILayout.FloatField("Pasive Heat Disp", eq.getPasiveHeatDisp()));
        EditorGUILayout.LabelField("Total Heat", Math.Round(eq.getTotalHeat(),2)+" K");
        EditorGUILayout.LabelField("Health", Math.Round(eq.getHealth(),1)+"/100");

        base.serializedObject.ApplyModifiedProperties();
    }
}
