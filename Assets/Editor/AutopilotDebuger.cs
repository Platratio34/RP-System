using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AutopilotDebuger : EditorWindow {

    private static AutoPilot held;
    private static bool hold;
    private static APDebugSaver saver;

    [MenuItem ("Window/Veiwers/Autopilot Debuger")]
    public static void  ShowWindow() {
        EditorWindow.GetWindow(typeof(AutopilotDebuger));
        LoadFromSaver();
    }

    void Update() {
        if (EditorApplication.isPlaying && !EditorApplication.isPaused) {
            Repaint();
        }
    }

    void OnGUI() {
        LoadFromSaver();
        if(Selection.activeGameObject == null) {
            GUILayout.Label("Select a Autopilot");
            SaveToSaver();
            return;
        }
        AutoPilot aP = Selection.activeGameObject.GetComponent<AutoPilot>();
        if(hold) {
            aP = held;
        }
        if(aP == null) {
            GUILayout.Label("Select a Autopilot");
            SaveToSaver();
            return;
        }
        GUILayout.BeginHorizontal();
        GUILayout.Label("Autopilot: " + aP.name + (held?" (Held)":""));
        if(GUILayout.Button("Hold")) {
            hold = true;
            held = aP;
        }
        if(GUILayout.Button("Release")) {
            hold = false;
            held = null;
            aP = Selection.activeGameObject.GetComponent<AutoPilot>();
            SaveToSaver();
            return;
        }
        GUILayout.EndHorizontal();

        if(aP.atTarget) GUILayout.Label("At Target!");
        if(aP.goToPos || aP.goToTransform) EditorGUILayout.FloatField("Distance To Target", aP.distToTarget);
        if(aP.goToPos || aP.goToTransform) EditorGUILayout.Vector3Field("Target Lateral Velocity"+(aP.dRot2.magnitude>1?(" "+aP.dRot2.magnitude+" off"):""), aP.tLatVel);
        if(aP.goToPos || aP.goToRot || (aP.goToTransform && aP.sameRot)) EditorGUILayout.Vector3Field("Target Rotational Velocity", aP.tRotVel);
        if(aP.goToPos || aP.goToRot || (aP.goToTransform && aP.sameRot)) EditorGUILayout.Vector3Field("Target Rotation", aP.tRot);
        if(aP.goToPos || aP.goToTransform) EditorGUILayout.Vector3Field("Diffrence in Position", aP.dPos);
        if(aP.goToRot || (aP.goToTransform && aP.sameRot)) EditorGUILayout.Vector3Field("Diffrence in Rotation", aP.dRot);
        EditorGUILayout.Vector3Field("Current Thrusting", aP.cThrust);
        SaveToSaver();
    }

    private static void LoadFromSaver() {
        GameObject sv = GameObject.Find("APDebugSaver");
        if(sv == null) {
            Debug.LogError("Missing APDebugSaver GameObject on load, please create one");
            return;
        }
        saver = sv.GetComponent<APDebugSaver>();
        if(saver == null) {
            Debug.LogError("Missing APDebugSaver Component on load, please create one");
            return;
        }
        hold = saver.hold;
        held = saver.held;
    }
    private static void SaveToSaver() {
        if(saver == null) {
            GameObject sv = GameObject.Find("APDebugSaver");
            if(sv == null) {
                Debug.LogError("Missing APDebugSaver GameObject on save, please create one");
                return;
            }
            saver = sv.GetComponent<APDebugSaver>();
            if(saver == null) {
                Debug.LogError("Missing APDebugSaver Component on save, please create one");
                return;
            }
        }
        saver.hold = hold;
        saver.held = held;
    }
}
