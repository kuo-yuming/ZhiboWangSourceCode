using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Line : MonoBehaviour {
	
	public GameObject line;
	public UISpriteAnimation lightAnimation;
	public UISprite sprite;
	public List<GameObject> LineIcon = new List<GameObject> ();
	public List<Reel> Reels = new List<Reel> ();
	public List<Icon> icon = new List<Icon> ();
	public bool isCombo = false;
	public bool isShow = false;
	void Start()
	{
		foreach (var obj in LineIcon) {
			icon.Add (obj.GetComponent<Icon> ());
		}
			
		if(!isCombo){
			lightAnimation.Pause();
			line.SetActive (false);
			sprite.spriteName = "linelight0";
		}
		LineIcon.ForEach (x => x.SetActive(false));
	}

	public void ShowLevel(int level)
	{
		if (level == 0)
			return;
		for (int i = 0; i < level; i++) {
			LineIcon [i].SetActive (true);
			Reels [i].PlaySymbolAnimation ();
		}
		lightAnimation.namePrefix = "linelight";
		lightAnimation.Play ();
		line.SetActive (true);
		isShow = true;
	}
		
	public void LightStop()
	{
		if (!isShow)
			return;
		lightAnimation.Pause ();
		sprite.spriteName = "linelight0";
		LineIcon.ForEach (x => x.SetActive(false));
		Reels.ForEach (x => x.StopSymbolAnimation());
		line.SetActive (false);
		isShow = false;
	}


	public void ShowCombo()
	{
		
	}

	public void stopComobo()
	{
		foreach (var Icon in LineIcon) {
			Icon.SetActive (false);
		}
	}
}
