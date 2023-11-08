using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Ship))]
public class ShipEditor : EntityEditor {

    new void OnEnable() {
        base.OnEnable();
    }
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("- Ship -");

        // EditorGUILayout.PropertyField(serializedObject.FindProperty("orbit"), new GUIContent("Orbit"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("engineArray"), new GUIContent("Engines"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("mass"), new GUIContent("Mass"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("effectiveAccl"), new GUIContent("Effective Acceleration"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("effectiveRot"), new GUIContent("Effective Rotation"));

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Velocity");
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField("Speed: "+(serializedObject.FindProperty("vel").vector3Value.magnitude)+" m/s");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("vel"), new GUIContent("Lateral"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("accl"), new GUIContent("Acceleration"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rVel"), new GUIContent("Rotation"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rAccl"), new GUIContent("Acceleration"));

        EditorGUI.indentLevel--;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("cSat"), new GUIContent("Current Satellite"));

        base.serializedObject.ApplyModifiedProperties();
    }
}