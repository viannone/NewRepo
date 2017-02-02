using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class HitScan : AttackPrefab {
	public RaycastHit2D[] enemiesHit;
	public int maxLength = 100;
	public Vector2 pos1;
	public Vector2 pos2;
	public Vector2 rotation;
	public Transform particleEffectChild;
	public void Start(){
		int layerCheck = gameObject.layer;
		if (layerCheck == 12) {
			layerCheck = 9;
		} else if (layerCheck == 10) {
			layerCheck = 11;
		} else {
			Debug.LogError ("IDIOT! Attack not assigned to proper layer");
		}
		pos1 = origin.position;
		pos2 = target.position;
		transform.position = (pos1 + pos2) / 2;
		particleEffectChild.localScale = new Vector3 (maxLength, 1, 1);
		particleEffectChild.rotation = Quaternion.LookRotation (pos2, Vector3.forward);
		enemiesHit = Physics2D.RaycastAll (pos1, rotation, maxLength, layerCheck);
		foreach(RaycastHit2D enemy in enemiesHit){
			enemy.transform.GetComponent<DamageInput> ().TakeHit (attack);
		}
		StartCoroutine(CountDownToSelfDestruct(2));
	}
}
