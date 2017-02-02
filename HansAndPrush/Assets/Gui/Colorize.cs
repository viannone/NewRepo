using UnityEngine;
using System.Collections;

public class Colorize : MonoBehaviour {

	public Color targetColor = Color.red;
	public float changeRate = 0.1f;
	public bool isColored = false;
	public string color;
	SpriteRenderer sr;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		GameObject.FindGameObjectWithTag (color).GetComponent<ColorSwitch>().AddToList(this);
	}

	public void ColorMe(){
		StartCoroutine ("Colorify");
	}


	public IEnumerator Colorify(){
		Color currColor = sr.color;
		Color oldColor = currColor;
		float t = 0.0f;
		while(currColor.r > targetColor.r){
			t += Time.deltaTime;
			currColor.r = Mathf.Lerp(oldColor.r, targetColor.r, changeRate * t);
			sr.color = currColor;
			yield return null;
		}
		t = 0.0f;
		oldColor = sr.color;
		while (currColor.g > targetColor.g) {
			t += Time.deltaTime;
			currColor.g = Mathf.Lerp (oldColor.g, targetColor.g, changeRate * t);
			sr.color = currColor;
			yield return null;
		}
		t = 0.0f;
		oldColor = sr.color;
		while (currColor.b > targetColor.b) {
			t += Time.deltaTime;
			currColor.b = Mathf.Lerp (oldColor.b, targetColor.b, changeRate * t);
			sr.color = currColor;
			yield return null;
		}
		isColored = true;
	}
}
