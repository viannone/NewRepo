using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomPropertyDrawer (typeof(Frame))]
public class FrameDrawer : PropertyDrawer {
	SerializedProperty frame;
	SerializedProperty transitions;
	SerializedProperty terminal;
	public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label){
		EditorGUI.BeginProperty (pos, label, prop);
		int indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;
		frame = prop.FindPropertyRelative ("frame");
		transitions = prop.FindPropertyRelative ("transitions"); 
		terminal = prop.FindPropertyRelative ("terminal");
		Rect terminalRect = new Rect (pos.x, pos.y, 20, 20);
		Rect frameRect = new Rect (terminalRect.x + terminalRect.width, pos.y, (pos.width * .3f), 20);
		Rect labelRect = new Rect (frameRect.x + frameRect.width + 10, pos.y, 50, transitions.arraySize * 20);
		Rect transitionsRect = new Rect (labelRect.x + labelRect.width + 30, pos.y, pos.max.x - labelRect.x - labelRect.width, transitions.arraySize * 20);
		EditorGUI.Toggle (terminalRect, terminal.boolValue);
		EditorGUI.PropertyField (frameRect, frame, GUIContent.none, true);
		EditorGUI.LabelField (labelRect, transitions.arraySize + " pivots");
		EditorGUI.PropertyField (transitionsRect, transitions, GUIContent.none, true);
		EditorGUI.indentLevel = indent;
		EditorGUI.EndProperty ();

	}
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		return EditorGUI.GetPropertyHeight (property);
	}
}

[CustomPropertyDrawer (typeof(Pivot))]
public class PivotDrawer : PropertyDrawer {
	public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label){
		EditorGUI.BeginProperty (pos, label, prop);
		int indentLevel = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;
		SerializedProperty toSeq = prop.FindPropertyRelative ("toSeq");
		SerializedProperty atFrame = prop.FindPropertyRelative ("atFrame");

		Rect message1 = new Rect (pos.x, pos.y, pos.width/8, 20);
		Rect message2 = new Rect (message1.x + message1.width, pos.y, pos.xMax/4, 20);
		Rect message3 = new Rect (message2.x + message2.width, pos.y, 70, 20);
		Rect message4 = new Rect (message3.x + message3.width, pos.y, 20, 20);
		EditorGUI.LabelField (message1, "To Sequence: ");
		EditorGUI.PropertyField (message2, toSeq, GUIContent.none, true);
		EditorGUI.LabelField (message3, "At Frame: ");
		EditorGUI.PropertyField (message4, atFrame, GUIContent.none, true);
		EditorGUI.indentLevel = indentLevel;
		EditorGUI.EndProperty ();
	}
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		return EditorGUI.GetPropertyHeight (property);
	}
}
