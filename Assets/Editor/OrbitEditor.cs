using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// [CustomEditor(typeof(Orbit))]
// [CanEditMultipleObjects]
// public class OrbitEditor : Editor {
//     private SerializedProperty  alt;
//     private SerializedProperty  inc;
//     private SerializedProperty  ect;
    
//     void OnEnable() {
//         alt = serializedObject.FindProperty("alt");
//         inc = serializedObject.FindProperty("inc");
//         ect = serializedObject.FindProperty("ect");
//     }

//     public override void OnInspectorGUI() {
//         serializedObject.UpdateIfRequiredOrScript();
        
//         EditorGUILayout.PropertyField(alt, new GUIContent("Altitude"));
//         EditorGUI.indentLevel++;
//         EditorGUILayout.PropertyField(inc, new GUIContent("Inclenation"));
//         EditorGUI.indentLevel--;
//         EditorGUILayout.PropertyField(ect, new GUIContent("Ecentricity"));

//         serializedObject.ApplyModifiedProperties();
//     }
// }
