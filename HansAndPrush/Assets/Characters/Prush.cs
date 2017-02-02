using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Prush : MonoBehaviour {
	Revolver rev;
	Transform hans;
	public static Prush prush;
	public int maxHorizontalRange;
	public List<Transform> enemiesInRange;
	public Transform currentEnemy;
	public Transform target;
	public float secondsToDestination;
	public float xOffset;
	public float yOffset;
	public float timeToPlatform;
 	CentralNervousSystem hansCNS;
	HumanInput hansBrain;
	public PrushPlatform prushPlatform;

	public void Awake(){
		prush = this;
		hans = (Transform) GameObject.FindGameObjectWithTag("Player").transform;
		hansCNS = hans.GetComponent<CentralNervousSystem> ();
		hansBrain = hans.GetComponent<HumanInput> ();
		prushPlatform.gameObject.SetActive (false);
	}

	public void Start(){
		rev = Revolver.revolver;
		MaintainHomeostasis();
	}

	public void MaintainHomeostasis(){
		StartCoroutine ("FindAllEnemiesWithinHorizontalRange");
		StartCoroutine ("GetClosestEnemy");
		StartCoroutine ("Prioritize");
		StartCoroutine ("Pursue");
		StartCoroutine ("ProcessInput");
	}

	public IEnumerator FindAllEnemiesWithinHorizontalRange(){
		while (true) {
			List<Transform> allEnemies = GameData.enemies;
			List<Transform> tempEnemies = new List<Transform>();
			if (allEnemies != null) {
				foreach (Transform enemy in allEnemies) {
					float dist = enemy.position.x - hans.position.x;
					if (dist * dist < maxHorizontalRange * maxHorizontalRange) {
						tempEnemies.Add (enemy);
					}
					yield return null;
				}
				enemiesInRange = tempEnemies;
			} else {
				enemiesInRange = null;
			}
			yield return null;
		}
	}
	public IEnumerator GetClosestEnemy(){
		while (true) {
			if (enemiesInRange == null || enemiesInRange.Count == 0) {
				currentEnemy = null;
			} else {
				Transform temp = enemiesInRange [0];
				for (int i = 1; i < enemiesInRange.Count; i++) {
					if (enemiesInRange != null && (hans.position - temp.position).sqrMagnitude < (enemiesInRange [i].position - hans.position).sqrMagnitude) {
						temp = enemiesInRange [i];
					}
					yield return null;
				}
				currentEnemy = temp;
			}
			yield return null;
		}
	}
	public IEnumerator Prioritize(){
		while (true) {
			if (currentEnemy != null) {
				target = currentEnemy;
			} else {
				target = hans;
			}
			yield return null;
		}
	}
	public IEnumerator Pursue(){
		while (true) {
			if (target != null) {
				float timer = 0.0f;
				Vector2 prushPos = (Vector2) transform.position;
				while (timer < secondsToDestination) {
					Vector2 goalPos = new Vector2 (target.position.x + xOffset, target.position.y + yOffset);
					if (target == null) {
						break;
					} else {
						timer += Time.deltaTime;
						transform.position = Vector2.Lerp (prushPos, goalPos, timer / secondsToDestination);
						yield return null;
					}
				}
			}
			yield return null;
		}
	}
	public IEnumerator ProcessInput(){
		while (true) {
			if (currentEnemy != null && Input.GetAxisRaw ("Action1") > 0) {
				rev.Attack (currentEnemy);
			}
			yield return null;
		}
	}
		
	public void CancelPlatform(){
		StopCoroutine ("PlatformCycle");
		prushPlatform.gameObject.SetActive (false);
		MaintainHomeostasis ();
	}

	public void Platform(){
		StopAllCoroutines ();
		StartCoroutine ("PlatformCycle");
	}
	public IEnumerator PlatformCycle(){
		Vector2 destination = new Vector2 (hans.position.x, hans.position.y - 3);
		Vector2 currentPos = transform.position;
		float timer = 0.0f;
		while ((Vector2) transform.position != destination) {
			timer += Time.deltaTime;
			transform.position = Vector2.Lerp (currentPos, destination, timer / timeToPlatform);
			yield return null;
		}
		prushPlatform.gameObject.SetActive (true);

		while (!prushPlatform.hansHasJumpedOff) {
			yield return null;
		}
		prushPlatform.gameObject.SetActive (false);
		MaintainHomeostasis ();
		while (!hansCNS.grounded) { 
			yield return null;
		}
		hansBrain.platformPrimed = true;
	}

}