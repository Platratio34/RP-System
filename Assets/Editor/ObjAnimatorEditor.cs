using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(ObjAnimator))]
public class ObjAnimatorEditor : Editor {

    private ObjAnimator o;
    private GameObject gO;
    private bool saveChange = true;
    // private bool objs = true;

    void OnEnable() {
        o = (ObjAnimator)target;
        gO = o.gameObject;
    }

    public override void OnInspectorGUI() {

        if(saveChange && !Application.IsPlaying(gO)) UnityEditor.Undo.RecordObject(gO, "descriptive name of this operation");

        EditorGUILayout.LabelField("Speed");

        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("-1")) {
            o.speed = -1f;
        }
        if(GUILayout.Button("Freeze")) {
            o.speed = 0f;
        }
        if(GUILayout.Button("+1")) {
            o.speed = 1f;
        }
        EditorGUILayout.EndHorizontal();

        o.speed = EditorGUILayout.Slider(o.speed, -1f, 1f);

        EditorGUILayout.LabelField("Target");
        o.target = EditorGUILayout.Slider(o.target, -1f, o.steps);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Time");
        o.time = EditorGUILayout.Slider(o.time, 0f, o.timeScale * o.steps);
        o.timeScale = EditorGUILayout.FloatField("Timescale", o.timeScale);
        o.steps = EditorGUILayout.IntField("Steps", o.steps);

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        // objs = EditorGUILayout.BeginFoldoutHeaderGroup(objs, "Objects");
        EditorGUILayout.LabelField("Objects");
        // if(objs) {
            if(GUILayout.Button("Add")) {
                AnimatedObj[] oA = new AnimatedObj[1];
                if(o.objs != null) {
                    oA = new AnimatedObj[o.objs.Length + 1];
                    Array.Copy(o.objs, 0, oA, 1, o.objs.Length);
                }
                o.objs = oA;
                o.objs[0] = new AnimatedObj();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUI.indentLevel++;
            if(o.objs != null) {
                for(int i = 0; i < o.objs.Length; i++) {
                    AnimatedObj a = o.objs[i];
                    Rect r = (Rect)EditorGUILayout.BeginVertical();
                    EditorGUI.DrawRect(r, new Color(0.35f, 0.35f, 0.35f));

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(a.g!=null?a.g.name:"None");
                    if(GUILayout.Button("Remove")) {
                        AnimatedObj[] oA = new AnimatedObj[o.objs.Length - 1];
                        Array.Copy(o.objs, 0, oA, 0, i);
                        Array.Copy(o.objs, i + 1, oA, i, o.objs.Length - (i+1) );
                        o.objs = oA;
                    }
                    EditorGUILayout.EndHorizontal();

                    a.g = (GameObject)EditorGUILayout.ObjectField(a.g, typeof(GameObject), true);

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Positions");
                    if(GUILayout.Button("Add")) {
                        ATransform[] oA = new ATransform[1];
                        if(a.p != null) {
                            oA = new ATransform[a.p.Length + 1];
                            Array.Copy(a.p, 0, oA, 0, a.p.Length);
                        }
                        a.p = oA;
                        a.p[a.p.Length - 1] = a.p.Length==1?new ATransform():a.p[a.p.Length - 2].Clone();
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();

                    EditorGUI.indentLevel++;
                    if(a.p != null) {
                        for(int j = 0; j < a.p.Length; j++) {
                            Rect r2 = (Rect)EditorGUILayout.BeginVertical();
                            EditorGUI.DrawRect(r2, new Color(0.3f, 0.3f, 0.3f));

                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField(j + "");
                            if(GUILayout.Button("Remove")) {
                                ATransform[] oA = new ATransform[a.p.Length - 1];
                                Array.Copy(a.p, 0, oA, 0, i);
                                Array.Copy(a.p, i + 1, oA, i, a.p.Length - (i+1) );
                                a.p = oA;
                            }
                            EditorGUILayout.EndHorizontal();

                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("");
                            if(j > 0 && GUILayout.Button("Move Up")) {
                                ATransform at = a.p[j-1];
                                a.p[j-1] = a.p[j];
                                a.p[j] = at;
                            }
                            if(j < a.p.Length - 1 && GUILayout.Button("Move Down")) {
                                ATransform at = a.p[j+1];
                                a.p[j+1] = a.p[j];
                                a.p[j] = at;
                            }
                            EditorGUILayout.EndHorizontal();

                            EditorGUI.indentLevel++;

                            a.p[j].position = EditorGUILayout.Vector3Field("Pos", a.p[j].position);
                            a.p[j].rotation = EditorGUILayout.Vector3Field("Rot", a.p[j].rotation);
                            a.p[j].scale = EditorGUILayout.Vector3Field("Scale", a.p[j].scale);



                            EditorGUI.indentLevel--;

                            EditorGUILayout.EndVertical();

                            if(j < a.p.Length - 1) {
                                r2 = (Rect)EditorGUILayout.BeginVertical();
                                EditorGUI.DrawRect(r2, new Color(0.3f, 0.3f, 0.3f));

                                a.p[j].curve = EditorGUILayout.CurveField(a.p[j].curve);

                                EditorGUILayout.EndVertical();
                            }

                            EditorGUILayout.Space();
                        }
                    }
                    EditorGUI.indentLevel--;

                    EditorGUILayout.EndFoldoutHeaderGroup();

                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                }
            }

            EditorGUI.indentLevel--;
        // }
        // EditorGUILayout.EndFoldoutHeaderGroup();

        if(!Application.IsPlaying(gO)) {
            saveChange = EditorGUILayout.Toggle("Save Changes to prefab", saveChange);
        }

        if(saveChange && !Application.IsPlaying(gO)) {
            UnityEditor.EditorUtility.SetDirty(gO);
            UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(gO);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gO.scene);
        }
        o.UpdateObjs();
    }
}
