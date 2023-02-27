using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LightSwitcher))]
public class LightSwitcherEditor : EquipmentEditor {

    protected LightSwitcher ls;
    // private bool objs = true;

    protected new void OnEnable() {
        base.OnEnable();
        ls = (LightSwitcher)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("- Light Switcher -");

        EditorGUILayout.Space();
        if(!ls.on) {
            EditorGUILayout.LabelField("Lights Off");
        } else if(ls.backup) {
            EditorGUILayout.LabelField("On Backup");
        } else if(!ls.onPow) {
            EditorGUILayout.LabelField("No Power");
        } else {
            EditorGUILayout.LabelField("Lights On");
        }
        EditorGUILayout.Space();

        // ls.on = EditorGUILayout.Toggle("Lights on", ls.on);
        // ls.emergency = EditorGUILayout.Toggle("Emergency Mode", ls.emergency);
        // ls.opMode = EditorGUILayout.Toggle("Op Mode", ls.opMode);
        // ls.backup = EditorGUILayout.Toggle("Backup Lighting", ls.backup);

        // EditorGUILayout.LabelField(" -- Settings");

        EditorGUILayout.PropertyField(serializedObject.FindProperty("on"), new GUIContent("Lights on"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("emergency"), new GUIContent("Emergency Mode"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("opMode"), new GUIContent("Op Mode"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("backup"), new GUIContent("Backup Lighting"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("lights"), new GUIContent("Lights"));

        // EditorGUILayout.PropertyField(serializedObject.FindProperty("cullingBox"), new GUIContent("Culling Box"));

        base.serializedObject.ApplyModifiedProperties();

        // DrawDefaultInspector();
    }
}