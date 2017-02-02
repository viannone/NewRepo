using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SelectScreen : MonoBehaviour {
	ColorButton[] allColorButtons;
	int revolverSize;
	int nextSpot = 0;
	public bool isActive = true;
	void Awake(){
		allColorButtons = transform.GetComponentsInChildren<ColorButton> ();
	}
	public void OnEnable(){
		UpdateButtons ();
	}

	public void ButtonClicked(int i){
		if (allColorButtons[i].isUnlocked && !allColorButtons[i].isInRevolver) {
			Revolver.revolver.SetAttack (GameData.gameData.colorData [i].attack);
			UpdateButtons ();
		}
	}
	public void UpdateButtons(){
		ColorData[] colorData = GameData.gameData.colorData;


		for (int i = 0; i < allColorButtons.Length; i++) {
			allColorButtons [i].isInRevolver = false;
			allColorButtons [i].isUnlocked = colorData [i].unlocked;
			allColorButtons[i].button.interactable = true;
			allColorButtons[i].button.enabled = true;
			allColorButtons[i].button.image.color = new Color (1.0f, 1.0f, 1.0f);
		}

		foreach (Attack a in Revolver.revolver.attacks) {
			allColorButtons [(int) a.attackColor].isInRevolver = true;
		}


		foreach (ColorButton cb in allColorButtons) {
			if (!cb.isUnlocked) {
				cb.button.interactable = false;
				cb.button.enabled = false;
				cb.button.image.color = new Color (0.0f, 0.0f, 0.0f);
			}

			if (cb.isInRevolver) {
				cb.button.interactable = false;
				cb.button.enabled = false;
				cb.button.image.color = new Color (0.5f, 0.5f, 0.5f);
			}
		}
	}
}
