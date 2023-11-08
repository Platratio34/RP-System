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

        string tooFast = "";
        if(aP.cThrust.x < -1) {
            tooFast = " [Too Fast!]";
        }

        if(aP.atTarget) GUILayout.Label("At Dest!");
        if(aP.goToPos || aP.goToTransform) EditorGUILayout.FloatField("Distance To Dest", aP.distToTarget);
        if(aP.goToTransform) EditorGUILayout.Vector3Field("Dest Velocity", aP.tVel);
        if(aP.goToTransform) GUILayout.Label("Dest Speed: " + (aP.tVel.magnitude).ToString("0.0") + " m/s");
        // if(aP.goToTransform) EditorGUILayout.FloatField("Target Relative Speed", aP.tRSpeed);
        if(aP.goToTransform) GUILayout.Label("Dest Relative Velocity: " + (aP.tRSpeed).ToString("+0.0;-0.0") + " m/s");
        if(aP.goToTransform) GUILayout.Label("dSpeed: " + (aP.tRSpeed - aP.tLatVel.x).ToString("+0.0;-0.0") + " m/s");
        if(aP.goToTransform) GUILayout.Label("Target Speed: " + (aP.ts).ToString("0.0") + " m/s");
        if(aP.goToPos || aP.goToTransform) EditorGUILayout.Vector3Field("Target Lateral Velocity"+(aP.dRot2.magnitude>1?(", "+(Mathf.Floor(aP.dRot2.magnitude*10f)/10f)+"d away"):""), aP.tLatVel);
        if(aP.goToPos || aP.goToRot || aP.goToTransform) EditorGUILayout.Vector3Field("Target Rotational Velocity", aP.tRotVel);
        if(aP.goToPos || aP.goToRot || aP.goToTransform) EditorGUILayout.Vector3Field("Target Rotation", aP.tRot);
        if(aP.goToPos || aP.goToTransform) EditorGUILayout.Vector3Field("Diffrence in Position", aP.dPos);
        if(aP.goToRot || aP.goToTransform) EditorGUILayout.Vector3Field("Diffrence in Rotation", aP.dRot);
        EditorGUILayout.Vector3Field("Current Thrusting"+tooFast, aP.cThrust);
        EditorGUILayout.Vector3Field("Current Rotating", aP.cRot);
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
