using UnityEngine;
using System.Collections;

public class PrushPlatform : MonoBehaviour {
	public bool hansOnPrush = false;

	public void OnEnable(){
		hansOnPrush = false;
	}
	public void OnCollisionEnter2D(Collision2D coll){
		if (coll.transform == HumanInput.humanInput.transform) {
			hansOnPrush = true;
		}
	}

	public void OnCollisionExit2D (Collision2D coll){
		if (coll.transform == HumanInput.humanInput.transform) {
			hansOnPrush = false;
		}
	}
}
