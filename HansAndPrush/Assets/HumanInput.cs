using UnityEngine;
using System.Collections;

public class HumanInput : Brain {
	public static HumanInput humanInput;
	CentralNervousSystem cns;

	public void Awake(){
		humanInput = this;
		cns = gameObject.GetComponent<CentralNervousSystem> ();
	}

	public void Start(){
		StartCoroutine (InputListen ());
	}

	public IEnumerator InputListen(){
		while(true){
			cns.SetxInput(Input.GetAxisRaw("Horizontal"));
			cns.SetyInput(Input.GetAxisRaw ("Vertical"));
			if (Input.GetAxisRaw ("Platform") > 0) {
				if (!cns.grounded && Prush.prush.jumpsThusFar < Prush.prush.maxPlatforms) {
					Prush.prush.Platform ();
					yield return new WaitForSeconds (.3f);
				}
			}
		yield return null;
		}
	}

}
