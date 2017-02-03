using UnityEngine;
using System.Collections;

public class DeathScript : MonoBehaviour {

	public void Die(){
		GameData.enemies.Remove (this.transform);
		Destroy (gameObject);
	}
}
