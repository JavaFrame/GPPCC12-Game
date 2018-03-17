using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EditStateAttribute))]
public class StateDropdown : PropertyDrawer
{
	private int selected = 0;
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);
		string[] options = new string[]
		{
			"test1",
			"test2"
		};
		var labelRect = new Rect(position.x, position.y, position.width/3, position.height);
		var dropdownRect = new Rect(position.x + position.width/3, position.y, position.width/3, position.height);
		selected = EditorGUI.Popup(dropdownRect, selected, options);
		EditorGUI.LabelField(labelRect, label);
		EditorGUI.EndProperty();

	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return base.GetPropertyHeight(property, label);
	}
}
