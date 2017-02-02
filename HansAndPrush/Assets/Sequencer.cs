using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[AddComponentMenu("Prush/Sequencer")]
public class Sequencer : MonoBehaviour {
	public string CHARACTER_ALLCAPS;
	public CentralNervousSystem cns;
	public RigidbodyInterface ri;
	public SpriteRenderer r;
	public int initialSequence;
	public int run;
	public int walk;

	public Seq[] allSequences;
	public string[] folderToIndexMap;
	public Seq currentSequence;

	public void Awake(){
		cns = gameObject.GetComponent<CentralNervousSystem> ();
		r = gameObject.GetComponent<SpriteRenderer> ();
		ri = gameObject.GetComponent<RigidbodyInterface> ();
	}

	public void Start(){
		if (initialSequence != null) {
			allSequences [initialSequence].Play ();
		} else {
			allSequences [0].Play ();
		}
	}
	public void AddSequences(){
		if (CHARACTER_ALLCAPS.Equals("")) {
			Debug.LogError ("No Character Specified, Asshole");
		}
		string path = "Assets/Resources/Sequences/" + CHARACTER_ALLCAPS + "/";
		string[] directories = Directory.GetDirectories (path);
		Debug.Log (path);
		foreach (string s in directories) {
			string localPath = s;
			localPath = localPath.Replace ("Assets/Resources/Sequences/" + CHARACTER_ALLCAPS + "/", "");//to get a path local to Resources
			Debug.Log (localPath);
			Seq seq = gameObject.AddComponent<Seq> ();
			seq.CHARACTER_ALLCAPS = CHARACTER_ALLCAPS;
			seq.folder = localPath;
		}
		MapSequences ();
	}
	public void RemoveSequences(){
		Seq[] seqs = GetComponents<Seq> ();
		foreach (Seq seq in seqs) {
			DestroyImmediate (seq, false);
		}
	}
	public void MapSequences(){
		allSequences = GetComponents<Seq> ();
		folderToIndexMap = new string[allSequences.Length];
		for(int i = 0; i< allSequences.Length; i++){
			folderToIndexMap[i] = allSequences [i].folder;
		}
		for (int i = 0; i < allSequences.Length; i++) {
			Debug.Log ("Sequence " + i + " is " + allSequences [i].folder);
		}
	}

	public void SetCurrentSequence(Seq s){
		StopCoroutine ("ManageAnimations");
		Debug.Log ("Now Playing: " + s.folder);
		currentSequence = s;
		StartCoroutine ("ManageAnimations");
	}

	public void Cue(int sequence){
		currentSequence.nextAnimation = allSequences[sequence];
		//find transition frames to s
		if (currentSequence.pivotTypes [sequence].pivots.Length == 0) {
			Debug.LogWarning ("No Transitions To " + folderToIndexMap [sequence]);
		} else {
			foreach (Pivot p in currentSequence.pivotTypes[sequence].pivots) {
				currentSequence.MakeFrameTerminal (p);
			}
		}
	}

	public IEnumerator ManageAnimations(){
		int maxFrameRate = currentSequence.maxFrameRate;
		int minFrameRate = currentSequence.minFrameRate;
		float oldX = ri.currentXvelocity;
		while (true) {
			float xVel = ri.currentXvelocity;

			if (xVel < 0) {
				r.flipX = true;
			} else {
				r.flipX = false;
			}

			if ((oldX < 0 && xVel > 0)) {
				Debug.Log (walk);
				Cue (walk);
			} else if ((oldX > 0 && xVel < 0)) {
				Cue (run);
			}
				oldX = xVel;
			if (currentSequence.speedSensitive) {
				int frameRate = Mathf.RoundToInt ((Mathf.Abs (ri.currentXvelocity / ri.maxHorizontalSpeed) * maxFrameRate));
				if (frameRate < minFrameRate) {
					frameRate = minFrameRate;
				}
				currentSequence.ChangeFrameRate (frameRate);
			}
			yield return null;
		}
	}

}