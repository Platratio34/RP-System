using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Entity), true)]
public class EntityEditor : Editor {

    private Entity o;
    private GameObject gO;
    // private bool saveChange = true;
    // private bool objs = true;

    protected void OnEnable() {
        o = (Entity)target;
        gO = o.gameObject;
    }

    public override void OnInspectorGUI() {
        // base.OnInspectorGUI();
        // EditorGUILayout.PropertyField(serializedObject.FindProperty("ship"), new GUIContent("Ship"));

        EditorGUILayout.LabelField("- Entity -");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("entityId"), new GUIContent("Entity ID"));
        if(o.isNameKey) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("localName"), new GUIContent("Localized Name"));
        } else {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("dispName"), new GUIContent("Display Name"));
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("entityType"), new GUIContent("Entity Type"));

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("staticEntity"), new GUIContent("Static"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("editableEntity"), new GUIContent("Editable"));

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("parentEntity"), new GUIContent("Parent"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("children"), new GUIContent("Children"));

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("inventory"), new GUIContent("Inventory"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("interactableController"), new GUIContent("Intractable Controller"));

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("customDataArray"), new GUIContent("Custom Data"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("eParams"), new GUIContent("Editable Parameters"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("selBoxes"), new GUIContent("Selection Boxes"));

        // EditorGUILayout.PropertyField(serializedObject.FindProperty("selBoxes"), new GUIContent("Gravity Source"));
        EditorGUILayout.ObjectField("Gravity", o.gravitySource, typeof(GravityArea), true);

        base.serializedObject.ApplyModifiedProperties();
    }
}
