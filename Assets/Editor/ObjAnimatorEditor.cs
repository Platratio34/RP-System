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
        bool change = false;

        if(saveChange && !Application.IsPlaying(gO)) UnityEditor.Undo.RecordObject(gO, "descriptive name of this operation");

        EditorGUILayout.LabelField("Speed");

        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("-1")) {
            o.speed = -1f;
            change = true;
        }
        if(GUILayout.Button("Freeze")) {
            o.speed = 0f;
            change = true;
        }
        if(GUILayout.Button("+1")) {
            o.speed = 1f;
            change = true;
        }
        EditorGUILayout.EndHorizontal();

        float t = EditorGUILayout.Slider(o.speed, -1f, 1f);
        if(t != o.speed) {
            o.speed = t;
            change = true;
        }

        EditorGUILayout.LabelField("Target");
        t = EditorGUILayout.Slider(o.target, -1f, o.steps);
        if(t != o.target) {
            o.target = t;
            change = true;
        }


        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Time");
        t = EditorGUILayout.Slider(o.time, 0f, o.timeScale * o.steps);
        if(t != o.time) {
            o.time = t;
            change = true;
        }
        t = EditorGUILayout.FloatField("Timescale", o.timeScale);
        if(t != o.timeScale) {
            o.timeScale = t;
            change = true;
        }
        t = EditorGUILayout.IntField("Steps", o.steps);
        if(t != o.timeScale) {
            o.timeScale = t;
            change = true;
        }

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
                change = true;
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
                        change = true;
                    }
                    EditorGUILayout.EndHorizontal();

                    GameObject tGO = (GameObject)EditorGUILayout.ObjectField(a.g, typeof(GameObject), true);
                    if(tGO != a.g) {
                        a.g = tGO;
                        change = true;
                    }

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
                        change = true;
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
                                change = true;
                            }
                            EditorGUILayout.EndHorizontal();

                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("");
                            if(j > 0 && GUILayout.Button("Move Up")) {
                                ATransform at = a.p[j-1];
                                a.p[j-1] = a.p[j];
                                a.p[j] = at;
                                change = true;
                            }
                            if(j < a.p.Length - 1 && GUILayout.Button("Move Down")) {
                                ATransform at = a.p[j+1];
                                a.p[j+1] = a.p[j];
                                a.p[j] = at;
                                change = true;
                            }
                            EditorGUILayout.EndHorizontal();

                            EditorGUI.indentLevel++;

                            Vector3 tV3 = EditorGUILayout.Vector3Field("Pos", a.p[j].position);
                            if(tV3 != a.p[j].position) {
                                a.p[j].position = tV3;
                                change = true;
                            }
                            tV3 = EditorGUILayout.Vector3Field("Rot", a.p[j].rotation);
                            if(tV3 != a.p[j].rotation) {
                                a.p[j].rotation = tV3;
                                change = true;
                            }
                            tV3 = EditorGUILayout.Vector3Field("Scale", a.p[j].scale);
                            if(tV3 != a.p[j].scale) {
                                a.p[j].scale = tV3;
                                change = true;
                            }



                            EditorGUI.indentLevel--;

                            EditorGUILayout.EndVertical();

                            if(j < a.p.Length - 1) {
                                r2 = (Rect)EditorGUILayout.BeginVertical();
                                EditorGUI.DrawRect(r2, new Color(0.3f, 0.3f, 0.3f));

                                AnimationCurve tC = EditorGUILayout.CurveField(a.p[j].curve);
                                if(tC != a.p[j].curve) {
                                    a.p[j].curve = tC;
                                    change = true;
                                }

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

        if(saveChange && !Application.IsPlaying(gO) && change) {
            UnityEditor.EditorUtility.SetDirty(gO);
            UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(gO);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gO.scene);
        }
        o.UpdateObjs();
    }
}
