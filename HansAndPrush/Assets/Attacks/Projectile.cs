using UnityEngine;
using System.Collections;

public class Projectile : AttackPrefab {
	Rigidbody2D rigi;
	public int maxSpeed = 20;
	public void Start(){
		transform.position = origin.position;
		rigi = GetComponent<Rigidbody2D> ();
		if (target != null) {
			AddVelocity ();
		} else {
			SelfDestruct ();
		}
		StartCoroutine (CountDownToSelfDestruct(1.5f));
	}
	public void AddVelocity(){
		Vector2 dir = target.position - transform.position;
		rigi.velocity = dir.normalized * maxSpeed;
	}

	public void OnTriggerEnter2D(Collider2D other){
		DamageInput di = other.gameObject.GetComponent<DamageInput> ();
		if (di != null) {
			di.TakeHit (attack);
			SelfDestruct ();
		}
	}

}
