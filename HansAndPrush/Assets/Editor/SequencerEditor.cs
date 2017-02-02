using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor (typeof(Sequencer))]
public class SequencerEditor : Editor {
	public override void OnInspectorGUI() {
		
		Sequencer sequencer = (Sequencer)target;
		if(GUILayout.Button("Remove Sequences")){
			sequencer.RemoveSequences ();
		}
		if(GUILayout.Button("Add Sequences")){
			sequencer.AddSequences ();
		}
			
		if(GUILayout.Button("Map Sequences")){
			sequencer.MapSequences ();
		}
		DrawDefaultInspector ();
	}
}
