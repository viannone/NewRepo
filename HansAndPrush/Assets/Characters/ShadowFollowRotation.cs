using UnityEngine;
using System.Collections;

public class ShadowFollowRotation : MonoBehaviour {
	public Transform target;
	// Use this for initialization

	// Update is called once per frame
	void Update () {
		transform.rotation = target.rotation;
	}
}
