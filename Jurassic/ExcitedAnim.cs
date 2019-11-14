using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ExcitedAnim : MonoBehaviour {

	public List<GameObject> fontobj = new List<GameObject>();
	public List<UISprite> ComboInfo = new List<UISprite>();
	public List<UISprite> Total = new List<UISprite>();
	public List<Font> fonts = new List<Font>();
	public GameObject fireObj;
	public UISprite fireSprite;
	public UISprite stoneSprite;
	public UISpriteAnimation fireAnim;
	public TweenScale stoneScale;
	public TweenAlpha stoneAlpha;
	public GameObject ComboNumberObj;
	public GameObject ComboEndObj;
	public AudioSource AminSource;
	bool play = false;
	public struct Font
	{
		public TweenPosition pos;
		public TweenAlpha alpha;
		public TweenScale scale;

	}

	public int comboNumber{
		get{return _comboNumber; }
		set{
			_comboNumber = value;
			string number = value.ToString ();
			List<char> buff = number.ToList(); 
			buff.Reverse ();
			ComboInfo [0].spriteName = ComboInfo [2].spriteName = "nb" + buff[0];
			if (buff.Count > 1) {
				ComboInfo [1].enabled = true;
				ComboInfo [3].enabled = true;
				ComboInfo [1].spriteName = ComboInfo [3].spriteName = "nb" + buff[1];
			}
			else {
				//ComboInfo [1].enabled = false;
				//ComboInfo [3].enabled = false;
				ComboInfo [1].spriteName = ComboInfo [3].spriteName = "nb0";
			}
		}
	}
	int _comboNumber = 0;

	public int totalNumber{
		get{return _totalNumber; }
		set{
			_totalNumber = value;
			string number = value.ToString ();
			List<char> buff = number.ToList();  
			buff.Reverse ();
			for (int i = 0; i < Total.Count; i++) {
				if (buff.Count > i) {
					Total [i].enabled = true;
					Total [i].spriteName = "nb" + buff[i];
				}
				else {
					//Total [i].enabled = false;
					Total [i].spriteName = "nb0";
				}
			}

		}
	}
	int _totalNumber = 0;

	void Start () {
		Font font = new Font();
		foreach(var obj in fontobj){
			font.pos = obj.GetComponent<TweenPosition> ();
			font.alpha = obj.GetComponent<TweenAlpha> ();
			font.scale = obj.GetComponent<TweenScale> ();
			fonts.Add (font);
		}
		stoneSprite.enabled = false;
	}

	public void PlayFire()
	{
		fireObj.SetActive (true);
		fireSprite.spriteName = "bg_op_7_11";
		StartCoroutine (_playFire());
	}
	IEnumerator _playFire()
	{
		for (int i = 11; i <= 25; i++) {
			fireSprite.spriteName = "bg_op_7_" + i.ToString ();
			yield return new WaitForSeconds(0.1f);
		}
		fireObj.SetActive (false);
	}

	public void ReversePlayFire()
	{
		fireObj.SetActive (true);
		fireSprite.spriteName = "bg_op_7_25";
		StartCoroutine (_playFire());
	}

	IEnumerator _ReverseplayFire()
	{
		for (int i = 25; i >= 11; i--) {
			fireSprite.spriteName = "bg_op_7_" + i.ToString ();
			yield return new WaitForSeconds(0.1f);
		}
		fireObj.SetActive (false);
	}
		

	public void playTitle()
	{
		if (play) {
			Particles.instance.PlayStone ();
			StartCoroutine (_playTitle ());
		}
	}

	IEnumerator _playTitle()
	{
		int i = 0;
		foreach (var obj in fonts) {
			obj.pos.Play();
			obj.alpha.Play();
			obj.scale.Play();
			if (i == 1)
				PlayFire ();
			if (i == 3)
				Particles.instance.PlayFire ();
			i++;
			yield return new WaitForSeconds (0.25f);
		}

		ComboNumberObj.SetActive (true);
		SetInfo (0, 0);
		if(!AutoSpin.instance.AutoSet)
			AutoSpin.instance.AutoSpins ();
		SlotManager.slotState = SlotManager.State.Idle;
	}
	// Update is called once per frame
	public bool ss;
	public bool bb;
	void Update () {
		if (ss) {
			playExcited();
			ss = false;
		}


		if (bb) {
			StartCoroutine(ReverseExcited());
			bb = false;
		}
	}

	IEnumerator ReverseExcited()
	{
		play = false;
		Particles.instance.StopParticle();
		ReversePlayFire ();
		ComboNumberObj.SetActive (false);
		//ReverseTitle
		for (int i = 3; i >= 0 ; i--) {
			fonts[i].pos.PlayReverse();
			fonts[i].alpha.PlayReverse();
			fonts[i].scale.PlayReverse();
			yield return new WaitForSeconds (0.25f);
		}
		stoneScale.PlayReverse ();
		stoneAlpha.PlayReverse ();
		fireObj.SetActive (false);
		yield return new WaitForSeconds (0.3f);
		stoneSprite.enabled = false;
		JurassicUIManager.instance.RewardMoney = SlotManager.playerMoney;
		SlotManager.slotState = SlotManager.State.Idle;
	}

	public void playExcited()
	{
		play = true;
		stoneSprite.enabled = true;
		AminSource.Play ();
		stoneScale.Play ();
		stoneAlpha.Play ();
	}

	public void Stop()
	{
		StartCoroutine (ReverseExcited());
	}

	public void SetInfo(int combo,int total)
	{
		comboNumber = combo;
		totalNumber = total;
	}

	public void StopView(int combo,int total)
	{
		StartCoroutine(_StopView(combo,total));
	}

	IEnumerator _StopView(int combo,int total)
	{
		AudioSource.PlayClipAtPoint (SlotManager.instance.audio[10],transform.position);
		ComboEndObj.SetActive (true);
		SetInfo (combo,total);
		yield return new WaitForSeconds (2.0f);
		ComboEndObj.SetActive (false);
		Stop ();

	}
}
