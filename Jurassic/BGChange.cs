using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BGChange : MonoBehaviour {


	public List<string> BGName = new List<string> ();
	UISprite BG;
	// Use this for initialization
	void Start () {
		SlotManager.OnChange += OnChange;
		BG = this.gameObject.GetComponent<UISprite> ();
	}

	void OnChange () {
		if (SlotManager.lastState == SlotManager.State.freeGame)
			BG.spriteName = BGName [1];
		else
			BG.spriteName = BGName [0];
	}
}
