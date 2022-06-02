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
        EditorGUILayout.LabelField("Net Power", eq.getNetPower()+" W");
        EditorGUILayout.LabelField("Required Power", eq.getPowerReq()+" W");
        EditorGUILayout.LabelField("Net Heat", eq.getNetHeat()+" K/s");
        // EditorGUILayout.LabelField("Passive Heat Disipation", eq.getPasiveHeatDisp()+" K/s");
        eq.setPasiveHeatDisp(EditorGUILayout.FloatField("Pasive Heat Disp", eq.getPasiveHeatDisp()));
        EditorGUILayout.LabelField("Total Heat", eq.getTotalHeat()+" K");
        EditorGUILayout.LabelField("Health", eq.getHealth()+"/100");

        base.serializedObject.ApplyModifiedProperties();
    }
}
