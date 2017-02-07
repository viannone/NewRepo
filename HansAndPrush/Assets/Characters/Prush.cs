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
	public float targetTime;
	public float maxDistanceDelta;
	public int maxPlatforms;
	public int jumpsThusFar;
	private float currentVel;
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
			if (allEnemies.Count != 0) {
				for (int i = 0; i < allEnemies.Count; i++) {
					if (Mathf.Abs (allEnemies [i].position.x - hans.position.x) <= maxHorizontalRange) {
						
					}
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
					if (enemiesInRange != null && enemiesInRange [i] != null && (hans.position - temp.position).sqrMagnitude < (enemiesInRange [i].position - hans.position).sqrMagnitude) {
						temp = enemiesInRange [i];
					} else {
						enemiesInRange.Remove (enemiesInRange [i]);
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
			Transform oldTarget = target;
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
				transform.position = Vector2.MoveTowards ((Vector2)transform.position, (Vector2) target.position, maxDistanceDelta);
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
		StartCoroutine(GetUnderHans ());
		if (jumpsThusFar == 0) {
			StartCoroutine (PlatformResetCheck());
		}
	}


	public IEnumerator GetUnderHans(){
			Vector2 destination = new Vector2 (hans.position.x, hans.position.y - 3);
			Vector2 currentPos = transform.position;
			float timer = 0.0f;
			while ((Vector2)transform.position != destination) {
				timer += Time.deltaTime;
				transform.position = Vector2.Lerp (currentPos, destination, timer / timeToPlatform);
				yield return null;
			}
			prushPlatform.gameObject.SetActive (true);

			while (!prushPlatform.hansHasJumpedOff) {
				yield return null;
			}
			prushPlatform.gameObject.SetActive (false);
	}
	public IEnumerator PlatformResetCheck(){
		while (!hansCNS.grounded) { 
			yield return null;
		}
		MaintainHomeostasis ();
		jumpsThusFar = 0;
	}


}