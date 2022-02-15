using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//RigingController
[CustomEditor(typeof(RigingController))]
// [CanEditMultipleObjects]
public class RigingControllerEditor : Editor {

    private RigingController rC;
    private bool showPoses = true;
    private GameObject gO;
    private bool saveChange = true;
    
    void OnEnable() {
        rC = (RigingController)target;
        gO = rC.gameObject;
    }

    public override void OnInspectorGUI() {

        if(saveChange && !Application.IsPlaying(gO)) UnityEditor.Undo.RecordObject(gO, "descriptive name of this operation");

        rC.speed = EditorGUILayout.FloatField("Speed", rC.speed);
        rC.newPoseName = EditorGUILayout.TextField("SaveName", rC.newPoseName);
        if(GUILayout.Button("Save")) {
            rC.SavePose(rC.newPoseName);
        }
        if(GUILayout.Button("Load From File")) {
            if(SaveLoadData.Exists("Poses/" + rC.newPoseName + ".json")) {
                rC.LoadFromFile(rC.newPoseName);
            } else {
                Debug.Log("No pose file exists by that name");
            }
        }

        showPoses = EditorGUILayout.Foldout(showPoses, "Poses");
        if(showPoses) {
            EditorGUI.indentLevel++;
            for(int i = 0; i < rC.poses.Length; i++) {
                if(rC.poses[i] != null) {
                    if(rC.poses[i].name != null) {
                        EditorGUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField(rC.poses[i].name);

                        if(GUILayout.Button("Load") ) {
                            rC.RecalPose(i);
                        }
                        if(GUILayout.Button("Update")) {
                            rC.UpdatePose(i);
                        }
                        if(GUILayout.Button("Remove") ) {
                            rC.RemovePose(i);
                        }
                        if(GUILayout.Button("Save") ) {
                            rC.SavePoseToFile(i, rC.poses[i].name);
                        }

                        EditorGUILayout.EndHorizontal();
                    } else {
                        Debug.LogError("Couldn't show pose, not named, " + i + "; Removing it; \"" + rC.poses[i].name + "\"");
                        rC.RemovePose(i);
                        i--;
                    }
                } else {
                    Debug.LogError("Couldn't show pose, not created, " + i);
                }
            }
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();
        rC.parentBone = (GameObject)EditorGUILayout.ObjectField(rC.parentBone, typeof(GameObject), true);
        if(GUILayout.Button("Find Bones")) {
            rC.PopulateBones(rC.parentBone);
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Current Pose: " + rC.GetCurrentPoseName());
        EditorGUILayout.LabelField("Next Pose: " + rC.GetNextPoseName());

        EditorGUILayout.Space();

        if(!Application.IsPlaying(gO)) {
            saveChange = EditorGUILayout.Toggle("Save Changes to prefab", saveChange);
        }

        if(saveChange && !Application.IsPlaying(gO)) {
            UnityEditor.EditorUtility.SetDirty(gO);
            UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(gO);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gO.scene);
        }

    }

    public class NamePopup : PopupWindowContent {
        
        public string name2;

        public override Vector2 GetWindowSize() {
            return new Vector2(200, 15);
        }

        public override void OnGUI(Rect rect) {
            // GUILayout.Label("Popup Options Example"/*, EditorStyles.boldLabel*/);
            name2 = EditorGUILayout.TextField(name2);
        }

        public override void OnOpen() {
            Debug.Log("Popup opened: " + this);
        }

        public override void OnClose() {
            Debug.Log("Popup closed: " + this);
        }
    }
}