using UnityEngine;
using System.Collections;

public class ShadowFollow : MonoBehaviour {
	public Transform target;
	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
		transform.position = target.transform.position;
	}
}
