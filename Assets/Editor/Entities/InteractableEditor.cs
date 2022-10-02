using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Interactable), true)]
public class InteractableEditor : Editor {

    protected Interactable it;
    protected GameObject gO;

    protected void OnEnable() {
        it = (Interactable)target;
        gO = it.gameObject;
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.LabelField("- Intractable -");

        EditorGUILayout.PropertyField(serializedObject.FindProperty("id"), new GUIContent("Intractable ID"));
        // String tS = EditorGUILayout.TextField("Interactable ID", it.id);
        // if(it.id != tS) {
        //     it.id = tS;
        // }

        if(it.isNameKey) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("localName"), new GUIContent("Localized Name"));
        } else {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("dispName"), new GUIContent("Display Name"));
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("isNameKey"), new GUIContent("Localize Name"));
        // bool tB = EditorGUILayout.Toggle("Localize Name", it.isNameKey);
        // if(tB != it.isNameKey) {
        //     it.isNameKey = tB;
        // }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("eParams"), new GUIContent("Editable Parameters"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("selBoxes"), new GUIContent("Selection Boxes"));

        base.serializedObject.ApplyModifiedProperties();
    }
}
