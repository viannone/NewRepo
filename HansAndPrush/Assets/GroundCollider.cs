﻿using UnityEngine;
using System.Collections;

public class GroundCollider : MonoBehaviour {
	public CentralNervousSystem cns;

	public void OnTriggerStay2D(Collider2D coll){
		cns.grounded = true;
	}
	public void OnTriggerExit2D(Collider2D coll){
		cns.grounded = false;
	}
		
}
