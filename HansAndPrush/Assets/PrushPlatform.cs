using UnityEngine;
using System.Collections;

public class PrushPlatform : MonoBehaviour {
	public bool hansHasJumpedOff;

	void OnEnable(){
		hansHasJumpedOff = false;
	}

	public void OnCollisionExit2D (Collision2D coll){
		if (coll.transform == HumanInput.humanInput.transform) {
			hansHasJumpedOff = true;
		}
	}
}
