using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor (typeof(Seq))]
public class SeqEditor : Editor {
	public override void OnInspectorGUI() {
		Seq seq = (Seq)target;
		if (GUILayout.Button ("Find Frames")) {
			seq.GetFrames ();
		}
		if(GUILayout.Button("Find Transitions")){
			seq.FindTransitions ();
		}
		if(GUILayout.Button("Compile Transitions")){
			seq.CompileTransitions ();
		}
		DrawDefaultInspector ();
	}
}
