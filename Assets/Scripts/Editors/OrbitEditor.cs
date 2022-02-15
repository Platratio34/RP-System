using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

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

// [CustomPropertyDrawer(typeof(Orbit))]
// public class OrbitProperyDrawer : PropertyDrawer {
//     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
//         // Using BeginProperty / EndProperty on the parent property means that
//         // prefab override logic works on the entire property.
//         EditorGUI.BeginProperty(position, label, property);

//         // Draw label
//         position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

//         // Don't make child fields be indented
//         var indent = EditorGUI.indentLevel;
//         EditorGUI.indentLevel = 0;

//         // Calculate rects
//         var altRect = new Rect(position.x, position.y, position.width, 18);
//         var incRect = new Rect(position.x, position.y + 20, position.width, 18);
//         var ectRect = new Rect(position.x, position.y + 40, position.width, 18);
//         var buttonRect = new Rect(position.x, position.y + 60, position.width, 18);

//         // Draw fields - pass GUIContent.none to each so they are drawn without labels
//         EditorGUI.PropertyField(altRect, property.FindPropertyRelative("alt"), new GUIContent("Altitude"));
//         EditorGUI.PropertyField(incRect, property.FindPropertyRelative("inc"), new GUIContent("Inclination"));
//         EditorGUI.PropertyField(ectRect, property.FindPropertyRelative("ect"), new GUIContent("Eccentricity"));
//         // GUI.Button()

//         // Set indent back to what it was
//         EditorGUI.indentLevel = indent;

        

//         EditorGUI.EndProperty();
//     }

//     public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
//         return 80.0f;
//     }
// }
