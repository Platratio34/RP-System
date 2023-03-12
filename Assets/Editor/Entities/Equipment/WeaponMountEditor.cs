using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(WeaponMount))]
public class WeaponMountEditor : EquipmentEditor {

    protected WeaponMount obj;

    protected new void OnEnable() {
        base.OnEnable();
        obj = (WeaponMount)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("- Weapon Mount -");
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("size"), new GUIContent("Mount Size"));
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("type"), new GUIContent("Weapon Type"));

        string[] wepSizes = new string[6];
        for (int i = 1; i <= 5; i++) {
            wepSizes[i] = WeaponMount.isWeaponOfSize(obj.type, i) ? i+"": "-";
        }
        EditorGUILayout.LabelField(string.Format("Weapon Size |{0}|{1}|{2}|{3}|{4}|",wepSizes[1],wepSizes[2],wepSizes[3],wepSizes[4],wepSizes[5]));
        if(!WeaponMount.isWeaponOfSize(obj.type, obj.size)) {
            EditorGUILayout.LabelField("~! Weapon size does not match mount size !~");
        } else {
            if(GUILayout.Button("Set Weapon")) {
                obj.setWeapon();
            }
        }
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("powerStore"), new GUIContent("Power Storage (J)"));
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxIn"), new GUIContent("Maximum In (W)"));
        EditorGUILayout.LabelField("Power Stored", string.Format("{0}J", obj.storedPower));

        if(GUILayout.Button("SHOOT")) {
            obj.shoot();
        }

        base.serializedObject.ApplyModifiedProperties();
    }
}
