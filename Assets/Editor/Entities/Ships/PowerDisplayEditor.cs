using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(PowerDisplay))]
public class PowerDisplayEditor : InteractableEditor {

    protected PowerDisplay pDisp;

    protected new void OnEnable() {
        base.OnEnable();
        pDisp = (PowerDisplay)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("- Power Display -");

        pDisp.network = (PowerNetwork)EditorGUILayout.ObjectField("Network", (UnityEngine.Object)pDisp.network, typeof(PowerNetwork), true);

        pDisp.gCard = (GraphicsCard)EditorGUILayout.ObjectField("Graphics Card", (UnityEngine.Object)pDisp.gCard, typeof(GraphicsCard), true);

        pDisp.networkName = EditorGUILayout.TextField("Network Name", pDisp.networkName);

        base.serializedObject.ApplyModifiedProperties();
    }
}
