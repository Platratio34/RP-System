using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(InteractableDoor))]
public class InteractableDoorEditor : InteractableEditor {

    protected InteractableDoor iD;

    protected new void OnEnable() {
        base.OnEnable();
        iD = (InteractableDoor)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("- Door -");

        iD.open = EditorGUILayout.Toggle("Open", iD.open);
        iD.locked = EditorGUILayout.Toggle("Locked", iD.locked);
        iD.lockable = EditorGUILayout.Toggle("Lockable", iD.lockable);
        iD.frozen = EditorGUILayout.Toggle("Frozen", iD.frozen);

        EditorGUILayout.Space();
        iD.invertAnimation = EditorGUILayout.Toggle("Invert Animation", iD.invertAnimation);
        iD.animator = (ObjAnimator)EditorGUILayout.ObjectField("Object Animator", (UnityEngine.Object)iD.animator, typeof(ObjAnimator), true);

        base.serializedObject.ApplyModifiedProperties();
    }
}
