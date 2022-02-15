using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(EditableParam))]
public class EParamEditor : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

        EditorGUI.BeginProperty(position, label, property);
        // EditabelParams eP = (EditabelParams)property.serializedObject.FindProperty(property.name);objectReferenceValue
        // EditabelParams eP = (EditabelParams)property.objectReferenceValue;
        // EditabelParams eP = new EditabelParams();
        // Debug.Log(property.FindPropertyRelative("parameters").arrayElementType);

        float lh = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        // EditableParam p = ;
        Rect rect = new Rect(position.x, position.y, position.size.x, EditorGUIUtility.singleLineHeight);
        property.FindPropertyRelative("name").stringValue = EditorGUI.TextField(rect, "Name", property.FindPropertyRelative("name").stringValue);

        rect.y += lh;
        ParamType type = (ParamType)Enum.GetValues( typeof(ParamType) ).GetValue( property.FindPropertyRelative("type").enumValueIndex);
        type = (ParamType)EditorGUI.EnumPopup(rect, "Type", type);
        property.FindPropertyRelative("type").enumValueIndex = (int)type;

        rect.y += lh;
        if(type == ParamType.STRING) {
            property.FindPropertyRelative("valueS").stringValue = EditorGUI.TextField(rect, "Value", property.FindPropertyRelative("valueS").stringValue);
        } else if(type == ParamType.FLOAT) {
            property.FindPropertyRelative("valueF").floatValue = EditorGUI.FloatField(rect, "Value", property.FindPropertyRelative("valueF").floatValue);
        } else if(type == ParamType.INT) {
            property.FindPropertyRelative("valueI").intValue = EditorGUI.IntField(rect, "Value", property.FindPropertyRelative("valueI").intValue);
        } else if(type == ParamType.BOOL) {
            property.FindPropertyRelative("valueB").boolValue = EditorGUI.Toggle(rect, "Value", property.FindPropertyRelative("valueB").boolValue);
        }
        if(type == ParamType.FLOAT || type == ParamType.INT) {
            rect.y += lh;
            property.FindPropertyRelative("range").vector2Value = EditorGUI.Vector2Field(rect, "Range", property.FindPropertyRelative("range").vector2Value);
            rect.y += lh;
            property.FindPropertyRelative("slid").boolValue = EditorGUI.Toggle(rect, "Slid", property.FindPropertyRelative("slid").boolValue);
        }

        rect.y += lh;
        rect = new Rect(rect.x, rect.y + 1, rect.size.x, 1);
        // rect.size.y = 6;
        EditorGUI.DrawRect(rect, new Color ( 0.16f,0.16f,0.16f, 1 ) );

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        int lineCount = 2;
        ParamType type = (ParamType)Enum.GetValues( typeof(ParamType) ).GetValue( property.FindPropertyRelative("type").enumValueIndex);
        if(type != ParamType.NULL) {
            lineCount++;
        }
        if(type == ParamType.FLOAT || type == ParamType.INT) {
            lineCount += 2;
        }
        return EditorGUIUtility.singleLineHeight * lineCount + EditorGUIUtility.standardVerticalSpacing * (lineCount-1) + 10;
    }

}
