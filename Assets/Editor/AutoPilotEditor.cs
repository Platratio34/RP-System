using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AutoPilot))]
[CanEditMultipleObjects]
public class AutoPilotEditor : Editor {

    private AutoPilot aP;

    void OnEnable() {
        aP = (AutoPilot)target;
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("goToPos"), new GUIContent("Go To Position"));
        EditorGUI.indentLevel++;
        // aP.goToPos = EditorGUILayout.Toggle("Go To Position", aP.goToPos);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("targetPos"), new GUIContent("Target Position"));
        // aP.targetPos =  EditorGUILayout.Vector3Field("- Target Position", aP.targetPos);
        EditorGUI.indentLevel--;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("goToRot"), new GUIContent("Go To Rotation"));
        // aP.goToRot = EditorGUILayout.Toggle("Go To Rotation", aP.goToRot);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("targetRot"), new GUIContent("Target Rotation"));
        // aP.targetRot =  EditorGUILayout.Vector3Field("- Target Rotation", aP.targetRot);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("waitRotForPos"), new GUIContent("Wait for Pos before Rot"));
        // aP.waitRotForPos = EditorGUILayout.Toggle("- Wait for Pos before Rot", aP.waitRotForPos);
        EditorGUI.indentLevel--;
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("goToTransform"), new GUIContent("Go To Transform"));
        EditorGUI.indentLevel++;
        // aP.goToTransform = EditorGUILayout.Toggle("Go To Transform", aP.goToTransform);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("targetTrans"), new GUIContent("Target Transform"));
        // aP.targetTrans = (Transform)EditorGUILayout.ObjectField("- Target Transform", aP.targetTrans, typeof(Transform), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("sameRot"), new GUIContent("Same Rotation as Transform"));
        // aP.sameRot = EditorGUILayout.Toggle("- Same Rotation as Transform", aP.sameRot);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("distToTransform"), new GUIContent("Distance to Transform"));
        EditorGUI.indentLevel--;
        // aP.distToTransform = EditorGUILayout.FloatField("- Distance to Transform", aP.distToTransform);
        EditorGUILayout.Space();
        // EditorGUILayout.PropertyField(serializedObject.FindProperty("posHold"), new GUIContent("Position Hold"));
        // aP.posHold = EditorGUILayout.Toggle("Position Hold", aP.posHold);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("flatZ"), new GUIContent("Flat Z"));
        // aP.flatZ = EditorGUILayout.Toggle("Flat Z", aP.flatZ);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ship"), new GUIContent("Ship"));
        // aP.ship = (Ship)EditorGUILayout.ObjectField("Ship", aP.ship, typeof(Ship), true);
        EditorGUILayout.Space();
        if(aP.atTarget) GUILayout.Label("At Target");

        serializedObject.ApplyModifiedProperties();
    }
}
