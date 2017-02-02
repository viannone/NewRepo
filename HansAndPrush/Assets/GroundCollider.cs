using UnityEngine;
using System.Collections;

public class GroundCollider : MonoBehaviour {
	public CentralNervousSystem cns;

	public void OnTriggerEnter2D(Collider2D coll){
		cns.grounded = true;
	}
	public void OnTriggerExit2D(Collider2D coll){
		cns.grounded = false;
	}
		
}
