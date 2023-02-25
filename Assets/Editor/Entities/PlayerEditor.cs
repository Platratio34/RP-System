using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerEditor : EntityEditor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("- Player -");
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("controlled"), new GUIContent("Local Controlled"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("jp"), new GUIContent("Jump Power"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("mp"), new GUIContent("Move Power"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("sp"), new GUIContent("Sprint Power"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("turnSpeed"), new GUIContent("Turn Speed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("firstPers"), new GUIContent("First Person"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cameraLock"), new GUIContent("Camera Lock"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("interactDist"), new GUIContent("Interaction Distance"));

        EditorGUILayout.LabelField(" -- Objects");
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rb"), new GUIContent("Ridgedbody"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cameraO"), new GUIContent("Camera Object"));

        EditorGUILayout.LabelField(" -- -- UI");

        EditorGUILayout.PropertyField(serializedObject.FindProperty("tooltip1"), new GUIContent("Tooltip 1"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("tooltip2"), new GUIContent("Tooltip 2"));

        base.serializedObject.ApplyModifiedProperties();
    }
}