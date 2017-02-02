using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (CentralNervousSystem))]

public class RigidbodyInterface : MonoBehaviour {
	CentralNervousSystem cns;
	Rigidbody2D rigi;

	float xInput = 0.0f; //scaler --> -1 hard left, 1 hard right
	float yInput = 0.0f;

	public bool isJumpingCharacter = true;
	public bool jumpPrimed = true;
	public float jumpCoolDown = 1.0f;

	public float maxHorizontalSpeed = 10.0f;
	public float maxVerticalSpeed = 10.0f;

	public float currentXvelocity = 0.0f;
	public float currentYvelocity = 0.0f;

	public float XaccelerationTime = 0.0f;
	public float YaccelerationTime = 0.0f;

	private Coroutine xAccelCoroutine;
	private Coroutine yAccelCoroutine;

	public Vector2 velocity;
	void Awake(){
		rigi = GetComponent<Rigidbody2D> ();
		cns = GetComponent<CentralNervousSystem> ();
	}
	void Start(){

		if (isJumpingCharacter) {
			YaccelerationTime = 0.0f;
		} else {
			rigi.gravityScale = 0;
		}
	}
	void FixedUpdate(){
		if (isJumpingCharacter) {
			velocity = new Vector2 (currentXvelocity, rigi.velocity.y);
		} else {
			velocity = new Vector2 (currentXvelocity, currentYvelocity);
		}
		if (rigi.velocity != velocity) {
			rigi.velocity = velocity;//this looks stupid, but it's to prevent jitter from calling the rigidbody every frame
		}
	}

	public void SetxInput(float f){
			xInput = f;
			float destinationVelocity = xInput * maxHorizontalSpeed;
			if (XaccelerationTime != 0){
				float timeRatio = Mathf.Abs((currentXvelocity - destinationVelocity) / maxHorizontalSpeed);
				if(xAccelCoroutine != null){
					StopCoroutine (xAccelCoroutine); //stop to reset
				}
				xAccelCoroutine = StartCoroutine(AccelerateX (destinationVelocity, timeRatio * XaccelerationTime));
			}else{
				currentXvelocity = destinationVelocity;
			}
	}

	public IEnumerator AccelerateX(float destinationVelocity, float accelTime){
		float t = 0.0f;
		float oldVel = currentXvelocity;
		while (currentXvelocity != destinationVelocity) {
			t += Time.deltaTime;
			currentXvelocity = Mathf.Lerp (oldVel, destinationVelocity, t / accelTime);
			yield return new WaitForFixedUpdate ();
		}
	}

	public void SetyInput(float f){
		if (isJumpingCharacter) {
			if (cns.grounded && jumpPrimed && f > 0) {
				StartCoroutine ("PrimeJump");
				rigi.velocity = new Vector2 (rigi.velocity.x, rigi.velocity.y + maxVerticalSpeed);
				}
		} else {
			float destinationVelocity = yInput * maxVerticalSpeed;
			if (YaccelerationTime != 0){
				yInput = f;
				float timeRatio = (currentYvelocity - destinationVelocity) / maxVerticalSpeed;
				if(yAccelCoroutine != null){
					StopCoroutine (yAccelCoroutine); //stop to reset
				}
				yAccelCoroutine = StartCoroutine(AccelerateY (destinationVelocity, timeRatio * YaccelerationTime));
			}else{
				currentYvelocity = destinationVelocity;
			}
		}
	}

	public IEnumerator PrimeJump(){
		jumpPrimed = false;
		float t = 0.0f;
		while (t < jumpCoolDown) {
			t += Time.deltaTime;
			yield return null;
		}
		jumpPrimed = true;
	}

	public IEnumerator AccelerateY(float destinationVelocity, float accelTime){
		float t = 0.0f;
		float oldVel = currentYvelocity;
		while (currentYvelocity != destinationVelocity) {
			t += Time.deltaTime;
			currentYvelocity = Mathf.Lerp (oldVel, destinationVelocity, t / accelTime);
			yield return new WaitForFixedUpdate ();
		}
	}
		
}
