using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ComboAnimation : MonoBehaviour {
	public List<Texture2D> Combos = new List<Texture2D>();
	public List<UISprite> ComboInfo = new List<UISprite>();
	public List<UISprite> Total = new List<UISprite>();
	public GameObject ComboNumberObj;
	public GameObject ComboEndObj;
	public UITexture Comboview;
	public GameObject Title;
	public AudioSource AnimAudio;
	bool reverse = false;
	public float speed = 0.12f;

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
				ComboInfo [1].enabled = false;
				ComboInfo [3].enabled = false;
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
					Total [i].enabled = false;
				}
			}

		}
	}
	int _totalNumber = 0;


	void Start()
	{
		Title.SetActive (false);
		Comboview.enabled = false;
		ComboNumberObj.SetActive (false);
		ComboEndObj.SetActive (false);
}


	public void SetInfo(int combo,int total)
	{
		comboNumber = combo;
		totalNumber = total;
	}



	public void Play()
	{
		StartCoroutine(PlayCombo());
	}

	public void Stop()
	{
		StartCoroutine(EndCombo());
	}
	public void StopView(int combo,int total)
	{
		StartCoroutine(_StopView(combo,total));
		Stop ();
	}

	IEnumerator _StopView(int combo,int total)
	{
		ComboEndObj.SetActive (true);
		SetInfo (combo,total);
		yield return new WaitForSeconds (2.0f);
		ComboEndObj.SetActive (false);
	}


	IEnumerator PlayCombo()
	{
		if (reverse)
			Combos.Reverse ();
		reverse = false;
		Comboview.enabled = true;
		foreach (Texture2D obj in Combos) {
			Comboview.mainTexture = obj;
			yield return new WaitForSeconds (speed);
		}
		Title.SetActive (true);
		ComboNumberObj.SetActive (true);
		SetInfo (0, 0);
		if(!AutoSpin.instance.AutoSet)
			AutoSpin.instance.AutoSpins ();
		SlotManager.slotState = SlotManager.State.Idle;
	}

	IEnumerator EndCombo()
	{
		reverse = true;
		Combos.Reverse ();
		Title.SetActive (false);
		ComboNumberObj.SetActive (false);
		foreach (Texture2D obj in Combos) {
			Comboview.mainTexture = obj;
			yield return new WaitForSeconds (speed);
		}
		Comboview.enabled = false;
		JurassicUIManager.instance.RewardMoney = SlotManager.playerMoney;
		SlotManager.slotState = SlotManager.State.Idle;
	}

}
