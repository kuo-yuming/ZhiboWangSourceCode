using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FontChange : MonoBehaviour {
	UISprite sprite;
	public bool isLabel;
	public List<string> Font = new List<string> ();
	// Use this for initialization
	void Start () {



		if (!isLabel) {
			sprite = gameObject.GetComponent<UISprite> ();
			if (VersionDef.m_enLanguage == GameCore.ENUM_LANGUAGE.TC)
				sprite.spriteName = Font [1];
			else
				sprite.spriteName = Font [0];
		} else {
			UILabel label = gameObject.GetComponent<UILabel> ();
			if (VersionDef.m_enLanguage == GameCore.ENUM_LANGUAGE.TC)
				label.text = Font [1];
			else
				label.text = Font [0];
		}
	}
}
