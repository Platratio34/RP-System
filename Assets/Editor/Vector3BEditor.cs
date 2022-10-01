using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Vector3B))]
public class Vector3BEditor : Editor {

    Vector3B v;

    public void OnEnable() {
        // v = (Vector3B)target;
    }
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        
        EditorGUILayout.Vector3Field("Positive", v.positive3());
        EditorGUILayout.Vector3Field("Negative", v.negative3());
    }
}