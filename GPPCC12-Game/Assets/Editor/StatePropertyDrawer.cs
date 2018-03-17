using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Unit.State))]
public class StatePropertyDrawer : PropertyDrawer {
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		// Calculate rects
		var amountRect = new Rect(position.x, position.y, 30, position.height);
		var unitRect = new Rect(position.x + 35, position.y, 50, position.height);

		GUI.enabled = true;
		// Draw fields - passs GUIContent.none to each so they are drawn without labels
		EditorGUI.LabelField(amountRect, property.FindPropertyRelative("_id").intValue.ToString());
		EditorGUI.LabelField(unitRect, property.FindPropertyRelative("_name").stringValue);
		GUI.enabled = false;

		// Set indent back to what it was
		EditorGUI.indentLevel = indent;

		EditorGUI.EndProperty();
	}
}
