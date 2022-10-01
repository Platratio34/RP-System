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
        aP.goToPos = EditorGUILayout.Toggle("Go To Position", aP.goToPos);
        aP.targetPos =  EditorGUILayout.Vector3Field("- Target Position", aP.targetPos);
        aP.goToRot = EditorGUILayout.Toggle("Go To Rotation", aP.goToRot);
        aP.targetRot =  EditorGUILayout.Vector3Field("- Target Rotation", aP.targetRot);
        aP.waitRotForPos = EditorGUILayout.Toggle("- Wait for Pos before Rot", aP.waitRotForPos);
        EditorGUILayout.Space();
        aP.goToTransform = EditorGUILayout.Toggle("Go To Transform", aP.goToTransform);
        aP.targetTrans = (Transform)EditorGUILayout.ObjectField("- Target Transform", aP.targetTrans, typeof(Transform), true);
        aP.sameRot = EditorGUILayout.Toggle("- Same Rotation as Transform", aP.sameRot);
        aP.distToTransform = EditorGUILayout.FloatField("- Distance to Transform", aP.distToTransform);
        EditorGUILayout.Space();
        // aP.posHold = EditorGUILayout.Toggle("Position Hold", aP.posHold);
        EditorGUILayout.Space();
        aP.flatZ = EditorGUILayout.Toggle("Flat Z", aP.flatZ);
        EditorGUILayout.Space();
        aP.ship = (Ship)EditorGUILayout.ObjectField("Ship", aP.ship, typeof(Ship), true);
        EditorGUILayout.Space();
        if(aP.atTarget) GUILayout.Label("At Target");
    }
}
