using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneCamera : MonoBehaviour {
	//listening to even flow
	Transform hans;
	Vector2 currentVel;
	public int cameraDistance;
	public float maxSpeed;
	public float targetTime;
	public bool cameraBob;
	public float cameraBobX;
	public float cameraBobY;
	public float cameraBobXTime;
	public float cameraBobYTime;
	public int pauseInput = 0;
	public int selectInput = 0;
	public bool paused = false;
	public bool pauseButtonDown = false;
	public bool selectButtonDown = false;
	public float timeScale = 0.0f;
	public Image pauseScreen;
	public Image selectScreen;
	//these variables are public for debugging purposes
	public Transform targetObject;

	Vector2 pos;
	Vector2 targetPos;
	float time;
	float theta;
	float distance;
	// Use this for initialization
	void Start () {
		hans = GameObject.FindGameObjectWithTag ("Player").transform;
		StartCoroutine ("PauseManage");
	}
	public IEnumerator PauseManage(){
		while (true) {
			pauseInput = (int) Input.GetAxisRaw ("Pause");
			selectInput = (int) Input.GetAxisRaw ("Select");
			if (pauseInput > 0 && pauseButtonDown == false) {
				TogglePause (pauseScreen);
				pauseButtonDown = true;
			} else if (selectInput > 0 && selectButtonDown == false) {
				TogglePause (selectScreen);
				selectButtonDown = true;
			}
			if (pauseInput == 0) {
				pauseButtonDown = false;
			}
			if (selectInput == 0) {
				selectButtonDown = false;
			}
			yield return null;
		}
	}

	public void TogglePause(Image screen){
		paused = !paused;
		if (paused) {
			timeScale = Time.timeScale;
			ShowPauseScreen (screen);
			Time.timeScale = 0.0f;
			Prush.prush.StopAllCoroutines ();
		} else {
			HidePauseScreen ();
			Time.timeScale = timeScale;
			Prush.prush.MaintainHomeostasis ();
		}
	}

	public void ShowPauseScreen(Image screen){
		screen.gameObject.SetActive (true);
	}
	public void HidePauseScreen(){
		pauseScreen.gameObject.SetActive (false);
		selectScreen.gameObject.SetActive (false);
	}

	public void LateUpdate(){
			try {
				pos = transform.position;
				targetPos = targetObject.position;

				if (cameraBob) {
					time = Time.timeSinceLevelLoad;
					theta = time / cameraBobYTime;
					distance = cameraBobY * Mathf.Sin (theta);
					targetPos.y += distance;

					theta = time / cameraBobXTime;
					distance = cameraBobX * Mathf.Sin (theta);
					targetPos.x += distance;
				}
				
				Vector2 intermediary = Vector2.SmoothDamp (pos, (Vector2)targetPos, ref currentVel, targetTime, maxSpeed, Time.deltaTime);
				transform.position = new Vector3 (intermediary.x, intermediary.y, cameraDistance);
			} catch (MissingReferenceException) {
				SetTargetObject (hans);
			}
	}
	public void SetTargetObject(Transform t){
		StopCoroutine ("MoveCamera");
		targetObject = t;
		StartCoroutine ("MoveCamera");
	}
}
