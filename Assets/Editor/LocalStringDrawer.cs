using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(LocalString))]
public class LocalStringDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUILayout.PropertyField(property.FindPropertyRelative("key"), new GUIContent("Key"));
        EditorGUILayout.PropertyField(property.FindPropertyRelative("catagory"), new GUIContent("Category"));
        EditorGUILayout.PropertyField(property.FindPropertyRelative("dropIns"), new GUIContent("Drop In Strings"));
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return -2;
    }
 }
