using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Satellite))]
public class SatelliteEditor : EntityEditor {

    new void OnEnable() {
        base.OnEnable();
    }

    public override void OnInspectorGUI() { // EditorGUILayout.PropertyField(serializedObject.FindProperty("rVel"), new GUIContent("Rotation"));
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("- Satellite -");

        EditorGUILayout.PropertyField(serializedObject.FindProperty("parent"), new GUIContent("Parent"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("orbitalPer"), new GUIContent("Period"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("orbitalPos"), new GUIContent("Position"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("offset"), new GUIContent("Offset"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("sOI"), new GUIContent("SOI"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("sOICollider"), new GUIContent("SOI Collider"));

        // EditorGUILayout.PropertyField(serializedObject.FindProperty("mass"), new GUIContent("Mass"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("surfaceGravity"), new GUIContent("Surface Gravity"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("radius"), new GUIContent("Radius"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("orbit"), new GUIContent("Orbit"));
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("lockDirToParent"), new GUIContent("Parent Look"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("lookDirToPrograde"), new GUIContent("Prograde Look"));

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Display:");
        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("disp"), new GUIContent("Visible"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("dispPoints"), new GUIContent("Points"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("dispColor"), new GUIContent("Color"));
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("focus"), new GUIContent("Focus"));

        EditorGUI.indentLevel--;

        base.serializedObject.ApplyModifiedProperties();
    }
}