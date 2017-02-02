using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Floater : MonoBehaviour {
	public float defaultTime = .5f;
	public Transform textPrefab;
	public Queue<Transform> displayQueue;
	public bool routineRunning;

	void Start(){
		displayQueue = new Queue<Transform> ();
	}

	public void NewMessage(string s, Color c){
		Transform t = Instantiate (textPrefab);
		t.SetParent (this.transform, false);
		Text te = t.GetComponent<Text> ();
		te.color = c;
		te.text = s;
		Add (t);
	}
	public void NewMessage (string s){
		Transform t = Instantiate (textPrefab);
		t.SetParent (this.transform, false);
		Text te = t.GetComponent<Text> ();
		te.color = Color.white;
		te.text = s;
		Add (t);
	}

	public void Add(Transform t){
		displayQueue.Enqueue (t);
		if (!routineRunning) {
			StartCoroutine ("Iterate");
		}
	}
	public IEnumerator Iterate(){
		routineRunning = true;
		while (displayQueue.Count > 0) {
			StartCoroutine (FloatAway (displayQueue.Dequeue()));
			yield return new WaitForSeconds (defaultTime / 4);
		}
		routineRunning = false;
	}
	public IEnumerator FloatAway(Transform t){
		float timer = 0.0f;
		Text te = t.GetComponent<Text> ();
		Color tempColor = te.color;
		while (timer < defaultTime) {
			timer += Time.deltaTime;
			float fract = timer / defaultTime;
			t.position = new Vector2(t.position.x, Mathf.Lerp (transform.position.y, transform.position.y + 5, fract));
			tempColor.a = Mathf.Lerp (1.0f, 0.0f, fract);
			te.color = tempColor;
			yield return null;
		}
		Destroy (t.gameObject);
	}
}
