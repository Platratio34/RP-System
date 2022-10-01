using UnityEditor;
using UnityEngine;
// using UnityEditor.UIElements;
// using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(Vector3B))]
public class Vector3BDrawer: PropertyDrawer {

    private Vector3B v = new Vector3B();

    // public override VisualElement CreatePropertyGUI(SerializedProperty property) {
    //     Vector3B v = (Vector3B)property.serializedObject.targetObject;
    //     var container = new VisualElement();

    //     // Create property fields.
    //     var amountField = new BaseField<Vector3B>("Positive", v.positive3());
    //     var unitField = new BaseField<Vector3B>("Negative", v.negative3());

    //     // Add fields to the container.
    //     container.Add(amountField);
    //     container.Add(unitField);

    //     return container;
    //     // EditorGUILayout.Vector3Field("Positive", v.positive3());
    //     // EditorGUILayout.Vector3Field("Negative", v.negative3());
    // }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        // Debug.Log(property.stringValue);
        // Vector3B v = (Vector3B)property.objectReferenceValue;
        Vector3B v = Vector3B.unProperty(property);

        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = indent+1;

        // Calculate rects
        var amountRect = new Rect(position.x, position.y, 30, position.height);
        var unitRect = new Rect(position.x + 35, position.y, 50, position.height);
        var nameRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);

        // Draw fields - pass GUIContent.none to each so they are drawn without labels
        v.applyPositive3(EditorGUILayout.Vector3Field("Positive", v.positive3()));
        v.applyNegative3(EditorGUILayout.Vector3Field("Negative", v.negative3()));
        v.property(property);
        // EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("amount"), GUIContent.none);
        // EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("unit"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;
        EditorGUILayout.Space();

        EditorGUI.EndProperty();
    }
}