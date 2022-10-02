using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Equipment), true)]
public class EquipmentEditor : InteractableEditor {

    protected Equipment eq;
    // private bool objs = true;

    protected new void OnEnable() {
        base.OnEnable();
        eq = (Equipment)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("- Equipment -");
        EditorGUILayout.LabelField("Net Power", Math.Round(eq.getNetPower(), 2)+" W");
        EditorGUILayout.LabelField("Required Power", Math.Round(eq.getPowerReq(), 2)+" W");
        EditorGUILayout.LabelField("Net Heat", Math.Round(eq.getNetHeat(), 2)+" C/s");
        // EditorGUILayout.LabelField("Passive Heat Disipation", eq.getPasiveHeatDisp()+" K/s");
        // eq.setPasiveHeatDisp(EditorGUILayout.FloatField("Pasive Heat Disp", eq.getPasiveHeatDisp()));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("pasiveHeatDisp"), new GUIContent("Passive Heat Disp"));
        EditorGUILayout.LabelField("Total Heat", Math.Round(eq.getTotalHeat(),2)+" C");
        EditorGUILayout.LabelField("Health", Math.Round(eq.getHealth(),1)+"/100");

        base.serializedObject.ApplyModifiedProperties();
    }
}
