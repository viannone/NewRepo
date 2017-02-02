using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
[AddComponentMenu("Prush/Sequence")]
[RequireComponent (typeof(Sequencer))]

public class Seq : MonoBehaviour {
	public string folder;
	public string CHARACTER_ALLCAPS;
	public bool speedSensitive = false;
	public int frameRate = 30;
	public int minFrameRate;
	public int maxFrameRate;
	public int currentFrame = 0;
	public Frame[] frames;
	public PivotType[] pivotTypes;//first index = to which sequence, second index is just an index number
	public Seq nextAnimation;
	SpriteRenderer sr;
	public Sequencer sequencer;

	public void Awake(){
		if (sequencer == null) {
			Debug.LogError ("Forgot To Compile Transitions in: " + folder + ", Dumbass");
		}
		sr = GetComponent<SpriteRenderer> ();
	}
	public void Play(){
		StartCoroutine (IterateFrames ());
	}

	public void Play(int frame){
		currentFrame = frame;
		StartCoroutine (IterateFrames ());
	}
	public void Stop(){
		StopAllCoroutines ();
		Reset ();
	}
	public void ChangeFrameRate(int rate){
		frameRate = rate;
	}
	public void Reset(){
		if (frames != null) {//for stupid compling purposes
			foreach (Frame f in frames) {
				f.terminal = false;
			}
		}
	}
	public void FindTransitions(){
		sequencer = GetComponent<Sequencer> ();
		Debug.Log ("Finding Transitions Baked Into Filenames");
		for (int frameIndex = 0; frameIndex < frames.Length; frameIndex++) {
			Frame f = frames [frameIndex];
			string spriteName = f.frame.name;
			if (spriteName.Split ('-').Length > 1) {
				string allTransitions = (spriteName.Split ('-')) [1];
				if (allTransitions != null) {
					string[] eachTransition = allTransitions.Split ('_'); //note difference between "-" and "_"!!
					foreach (string s in eachTransition) {
						Debug.Log (s);
						if (s.Contains ("TO")) {
							bool seqFound = false;
							foreach (Seq seq in sequencer.allSequences) {
								Debug.Log (Regex.Replace (Regex.Replace (s, @"TO", ""), @"\d", ""));
								if (seq.folder.Split ('/') [seq.folder.Split ('/').Length - 1].Equals (Regex.Replace(Regex.Replace(s, @"TO", ""), @"\d", ""))) {
									Pivot p = new Pivot ();
									p.toSeq = Regex.Replace(Regex.Replace(s, @"TO", ""), @"\d", "");
									p.atFrame = int.Parse (Regex.Replace (s, @"\D", ""));
									if (f.transitions != null) {
										Pivot[] newPivots = new Pivot[f.transitions.Length + 1];
										f.transitions.CopyTo (newPivots, 0);
										newPivots [newPivots.Length - 1] = p;
										f.transitions = newPivots;
									} else {
										f.transitions = new Pivot[1];
										f.transitions [0] = p;
									}
									seqFound = true;
									break;
								}
							}
							if (!seqFound) {
								Debug.LogError ("Sprite Transitions TO Unknown Sequence: " + spriteName);
							}
						} else if (s.Contains ("FROM")) {
							bool seqFound = false;
							foreach (Seq seq in sequencer.allSequences) {
								if (seq.folder.Split ('/') [seq.folder.Split ('/').Length - 1].Equals (Regex.Replace(Regex.Replace(s, @"FROM", ""), @"\d", ""))) {

									Pivot p = new Pivot ();
									p.toSeq = folder;
									p.atFrame = frameIndex;
									p.fromFrame = int.Parse(Regex.Replace(s, @"\D", ""));
									int fromFrame = int.Parse (Regex.Replace (s, @"\D", ""));
									if (seq.frames [fromFrame].transitions != null) {
										Pivot[] temp = new Pivot[seq.frames [fromFrame].transitions.Length + 1];
										seq.frames [fromFrame].transitions.CopyTo (temp, 0);
										temp [temp.Length - 1] = p;
										seq.frames [fromFrame].transitions = temp;
									} else {
										seq.frames [fromFrame].transitions = new Pivot[1];
										seq.frames [fromFrame].transitions [0] = p;
									}
									seqFound = true;
									break;
								}
							}
							if (!seqFound) {
								Debug.LogError ("Sprite Transitions FROM Unknown Sequence: " + spriteName);
							}
						} else {
							Debug.LogError ("Sprite name improperly formatted!! " + spriteName);
						}
					}
				}
			}
		}
	}

	public void CompileTransitions(){
		Debug.Log ("Beginning... This may take a while");
		sequencer = GetComponent<Sequencer> ();
		pivotTypes = new PivotType[sequencer.folderToIndexMap.Length];
		int[] countPerType = new int[pivotTypes.Length];
		foreach (Frame f in frames) {
			if (f.transitions != null) {
				foreach (Pivot p in f.transitions) {
					//FIRST map the folder names to the corresponding sequence in the sequencer
					bool foundSequenceInList = false;
					for (int i = 0; i < sequencer.folderToIndexMap.Length; i++) {
						if (sequencer.folderToIndexMap [i].Equals (p.toSeq)) {
							p.toSeqIndex = i;
							countPerType [i]++;
							foundSequenceInList = true;
							break;
						}
					}
					if (!foundSequenceInList) {
						Debug.LogError ("Sequence: <color=red>" + p.toSeq + "</color> not found in Sequencer");
					}
				}
			}
		}
		//initialize the individual pivot arrays
		for (int i = 0; i < pivotTypes.Length; i++) {
			pivotTypes [i] = new PivotType(countPerType [i]);
		}
		//stick the pivots in the appropriate slots
		for (int f = 0; f < frames.Length; f++) {
			if (frames [f].transitions != null) {
				foreach (Pivot p in frames[f].transitions) {
					p.fromFrame = f;
					int index = 0;
					while (pivotTypes [p.toSeqIndex].pivots [index] != null) {
						index++;
					}
					pivotTypes [p.toSeqIndex].pivots [index] = p;
				}
			}
		}
		//Printout all the pivots and check to make sure everything worked
		Debug.Log("From: <color=green>" + folder +"</color>");
		for(int type = 0; type < pivotTypes.Length; type++){
			Debug.Log ("     To: <color=red>" + sequencer.folderToIndexMap [type] + "</color>");
			if (pivotTypes [type].pivots.Length == 0) {
				Debug.LogWarning ("No Transitions To Above Sequence!");
			}else{
				foreach (Pivot p in pivotTypes[type].pivots) {
					Debug.Log ("<color=orange>        From: " + p.fromFrame + " To: " + p.atFrame + "</color>");
				}
			}
		}
	}
	public void MakeFrameTerminal(Pivot p){
		frames [p.fromFrame].terminal = true;
		frames [p.fromFrame].toFrame = p.atFrame;
	}

	public void GetFrames(){
		if (folder != null) {
			Debug.Log ("Getting Frames...");
			Object[] sprites = Resources.LoadAll ("Sequences/" + CHARACTER_ALLCAPS + "/" + folder, typeof(Sprite));
			Debug.Log (sprites.Length + " sprites at " + folder);
			frames = new Frame[sprites.Length];
			for (int i = 0; i < frames.Length; i++) {
				frames [i] = new Frame ();
				frames [i].frame = (Sprite) sprites [i];
			}
		} else {
			Debug.LogError ("Invalid Path Specified");
		}
	}
	public IEnumerator IterateFrames(){
		sequencer.SetCurrentSequence (this);
		float timer = 0.0f;
		sr.sprite = frames[currentFrame].frame;
		int numFrames = frames.Length;
		while (true) {
			sr.sprite = frames [currentFrame].frame;
			float frameLength = (float) 1 / frameRate;
			timer += Time.deltaTime;
			if (timer >= frameLength) {
			/*	if (timer > frameLength * 2) {
					Debug.LogWarning ("Frame Repeated:" + currentFrame);
				}*/ 
				timer = timer % frameLength;//put the leftover time towards the next frame;
				currentFrame++;
				if (currentFrame >= numFrames) {
						currentFrame -= numFrames;
					}
				}
				sr.sprite = frames [currentFrame].frame;
			if (frames [currentFrame].terminal) {
				Reset ();
				nextAnimation.Play (frames [currentFrame].toFrame);
				Debug.Log("Coroutine Ended");
				yield break;
			}
			yield return null;
			}
		}

	}
	[System.Serializable]
	public class PivotType{
	public Pivot[] pivots;
	public PivotType(int i){
		pivots = new Pivot[i];
	}
}
	[System.Serializable]
	public class Frame{
		public bool terminal = false;
		public Sprite frame;
		public Pivot[] transitions;
		public int toFrame;
	}
	[System.Serializable]
	public class Pivot{
	public int fromFrame;
	public string toSeq;
	public int toSeqIndex;
	public int atFrame;
	}
	