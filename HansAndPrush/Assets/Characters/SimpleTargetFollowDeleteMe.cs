using UnityEngine;
using System.Collections;

public class SimpleTargetFollowDeleteMe : Brain {
	public	Transform target;
	public	CentralNervousSystem cns;
	public AttackTransmitter at;

	public float xInputCurrent;
	public void Awake(){
		cns = GetComponent<CentralNervousSystem> ();
	}

	public void Start(){
		at = GetComponent<AttackTransmitter> ();
		//because we're an enemy, add to enemy list
		GameData.enemies.Add(transform);
		if (target != null) {
			StartCoroutine ("ChaseTarget");
			StartCoroutine ("ShootTarget");
		}
	}

	public IEnumerator ChaseTarget(){
		Vector2 targetPos = target.transform.position;
		Vector2 youPos = transform.position;
		float xInputNew =  0.0f;
		while (true) {
			targetPos = target.transform.position;
			youPos = transform.position;

			if (targetPos.x > youPos.x) {
				xInputNew = 1;
			} else if (targetPos.x < youPos.x) {
				xInputNew = -1;
			}
			if (xInputCurrent != xInputNew) {
				cns.SetxInput (xInputNew);
				xInputCurrent = xInputNew;
			}
			yield return null;
		}

	}
	public IEnumerator ShootTarget(){
		while(true){
			if (Mathf.Abs ((target.position - transform.position).sqrMagnitude) < 100) {
				at.Attack ();
			}
			yield return null;
		}
	}
}