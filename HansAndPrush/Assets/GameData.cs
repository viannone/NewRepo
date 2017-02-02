using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class GameData : MonoBehaviour {
	public static GameData gameData;
	public static List<Transform> enemies;

	public ColorData[] colorData;
	public Sprite[] colorImages;
	public int revolverSize;
	public int currentLevel;

	public void Awake(){
		if (gameData == null) {
			gameData = this;
		} else if (gameData != this) {
			Debug.Log (gameData);
			Destroy (gameObject);
		}
		DontDestroyOnLoad (this);
		enemies = new List<Transform> ();
	}

	public void Save(){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerData.Prush");
		bf.Serialize (file, PlayerData.Pack ());
		file.Close ();
	}
	public void Load(){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerData.Prush");
		PlayerData data = (PlayerData)bf.Deserialize (file);
		file.Close ();
		data.Unpack ();
	}

}

[Serializable]
class PlayerData{
	public ColorData[] colorData;
	public int currentLevel;

	public static PlayerData Pack(){
		GameData data = GameData.gameData;
		PlayerData playerData = new PlayerData ();
		playerData.colorData = data.colorData;
		playerData.currentLevel = data.currentLevel;
		return playerData;
	}

	public void Unpack(){
		GameData data = GameData.gameData;
		data.colorData = colorData;
		data.currentLevel = currentLevel; 
	}

}

[System.Serializable]
public class ColorData{
	public string name;
	public bool unlocked;
	public Attack attack;
}
