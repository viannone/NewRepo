using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorSwitch : MonoBehaviour {
	public List<Colorize> l;

	public void Awake(){
		l = new List<Colorize>();
	}

	public void AddToList(Colorize c){
		l.Add (c);
	}

	void OnTriggerEnter2D(){
		for (int i = 0; i < l.Count; i++) {
			l [i].ColorMe ();
		}
	}
}
