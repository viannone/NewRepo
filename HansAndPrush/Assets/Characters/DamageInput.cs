using UnityEngine;
using System.Collections;
[RequireComponent (typeof (CentralNervousSystem))]
public class DamageInput : MonoBehaviour {

	public CentralNervousSystem cns;
	AttackColor colorWeakness;
	float colorDamageMultiplier = 1.0f;
	float amplifier = 1.0f;
	// Use this for initialization
	public void SetUp(CentralNervousSystem central){
		cns = central;
		colorWeakness = central.colorWeakness;
		colorDamageMultiplier = central.colorDamageMultiplier;
	}

	public void TakeDamage(int amount, Color c){
		cns.ChangeHealth ((int) (-amount * amplifier), c);
	}

	public void TakeDamage(int amount){
		TakeDamage (amount, Color.white);
	}

	public void TakeHit(Attack incomingAttack){
		//first just take the damage
		if (incomingAttack.attackColor == colorWeakness) {
			TakeDamage ((int) (incomingAttack.value * colorDamageMultiplier));
		} else {
			TakeDamage ((int) incomingAttack.value);
		}
		if (incomingAttack.effects != null) {
			foreach (Effects e in incomingAttack.effects) {//if there's an EFFECT that comes with the damage
				switch (e.effect) {
				case Effect.DOT:
					StartCoroutine (DOT (e.effectValue, e.effectTime));
					break;
				case Effect.SLOW:
					StartCoroutine (SLOW (e.effectValue, e.effectTime));
					break;
				case Effect.AMPLIFY:
					StartCoroutine (AMPLIFY (e.effectValue, e.effectTime));
					break;
				case Effect.ARMOR:
					incomingAttack.sender.damageInput.StartCoroutine (ARMOR (e.effectValue, e.effectTime));
					break;
				case Effect.CLEANSE:
					incomingAttack.sender.damageInput.Cleanse ();
					incomingAttack.sender.PostMessage ("Cleansed!", Color.white);
					break;
				case Effect.HEAL:
					incomingAttack.sender.ChangeHealth ((int)e.effectValue, Color.green);
					break;
				case Effect.VAMPIRIC:
					StartCoroutine (VAMPIRIC (incomingAttack.sender, e.effectValue, e.effectTime));
					break;
				}
			}
		}
	}
	public IEnumerator VAMPIRIC(CentralNervousSystem sender, float effectValue, float effectTime){
		float count = 0;
		cns.PostMessage ("Vampiric Effect!", Color.magenta);
		while (count <= (int)effectTime) {
			yield return new WaitForSeconds (1.0f);
			cns.ChangeHealth ((int) -effectValue, Color.magenta);
			sender.ChangeHealth ((int) effectValue, Color.magenta);
			count++;
		}
	}

	public void Cleanse(){// only called on the SENDER of the attack
		StopAllCoroutines ();
	}

	public IEnumerator DOT(float effectValue, float effectTime){
		int count = 0;
		cns.PostMessage ("BURNED!", Color.red);
		while (count <= (int) effectTime) {
			yield return new WaitForSeconds(1.0f);
			cns.ChangeHealth ((int) -effectValue, Color.red);
			count++;
		}
	}
	public IEnumerator ARMOR(float effectValue, float effectTime){//this one affects the SENDER of the attack
		float amount = (effectValue * amplifier) - amplifier;
		cns.PostMessage ("Damage Amplified By: %" + (int) (amount * 100), Color.black);
		amplifier += amount;
		yield return new WaitForSeconds(effectTime);
		amplifier -= amount;
		cns.PostMessage ("Damage Now: %" + (int) (amplifier * 100) , Color.black);
	}
	public IEnumerator SLOW(float effectValue, float effectTime){
		float oldSpeed = cns.GetMaxSpeed ();
		float amount = effectValue * oldSpeed;
		cns.SetMaxSpeed (oldSpeed - amount);
		cns.PostMessage ("SLOWED", Color.blue);
		yield return new WaitForSeconds (effectTime);
		cns.SetMaxSpeed (cns.GetMaxSpeed() + amount);
		cns.PostMessage ("Speed Restored", Color.blue);
	}
	public IEnumerator AMPLIFY(float effectValue, float effectTime){
		float amount = (effectValue * amplifier) - amplifier;
		cns.PostMessage ("Damage Amplified By: %" + (int) (amount * 100), Color.yellow);
		amplifier += amount;
		yield return new WaitForSeconds(effectTime);
		amplifier -= amount;
		cns.PostMessage ("Damage Now: %" + (int) (amplifier * 100) , Color.yellow);
	}

}
