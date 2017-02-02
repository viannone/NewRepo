using UnityEngine;
using System.Collections;

public class HumanInput : Brain {
	public static HumanInput humanInput;
	CentralNervousSystem cns;
	public bool platformPrimed = true;

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
				if (!cns.grounded && platformPrimed) {
					platformPrimed = false;
					Prush.prush.Platform ();
				} else if (cns.grounded && !platformPrimed) {
					Prush.prush.CancelPlatform ();
					platformPrimed = true;
				}
			}
		yield return null;
		}
	}

}
