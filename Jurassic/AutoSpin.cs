using UnityEngine;
using System.Collections;

public class AutoSpin : MonoBehaviour {

	public  UILabel CountView;
	public  UILabel CountViewChild;
	public UISprite more;
	public UISprite less;
	public UISprite max;
	public GameObject AutoButton;
	public GameObject AutoButton1;
	public TweenPosition Chose;
	public  bool AutoSet = false;
	bool isClick = false;
	bool isStop = false;
	float waittime = 2f;
	private static AutoSpin _instance;
	public static AutoSpin instance{get {return _instance;}}

	string unlimited {
		get{
			if(VersionDef.m_enLanguage == GameCore.ENUM_LANGUAGE.TC)
				return "無限";
			else
				return "无限";
		}
	}

	public  int AutoCount
	{
		get{
			return _AutoCount;
		}
		set{
			_AutoCount = value;
			if (_AutoCount == 0)
				AutoSet = false;
		}
	}

	int _AutoCount = 0;
	int _bufAutoCount;
	int bufAutoCount {
		get {
			return _bufAutoCount;
		}
		set{
			_bufAutoCount = value;
			if (_bufAutoCount < 0)
				_bufAutoCount = 1000;
			if (_bufAutoCount > 1000)
				_bufAutoCount = 0;
		}
	}

	void Start()
	{
		if (_instance == null)
			_instance = this.gameObject.GetComponent<AutoSpin> ();
		CountView.text = "0";
		CountViewChild.text = string.Empty;

		instance.CountView.enabled = AutoCount != 0;
	}

	void Update()
	{
		if (bufAutoCount == 1000) {
			max.spriteName = "btn_infinity_1";
		} else {
			max.spriteName = "btn_infinity_0";
		}
	}

	public void CheckClick()
	{
		if (isClick == true) {
			isClick = false;
			instance.Chose.PlayReverse ();
		}
	}

	public void OnButtonClick()
	{
		BetButton.instance.CheckClick ();
		Jurassic_GameUIBag.instance.CheckClick ();
		JurassicInfos.instance.CheckClick ();
		if (!isClick) {
			bufAutoCount = AutoCount;
			CountViewChild.text = bufAutoCount == 1000 ? unlimited : bufAutoCount.ToString();
			instance.Chose.PlayForward ();
			instance.AutoButton.SetActive (true);
			instance.AutoButton1.SetActive (false);
			CountView.text = string.Empty;
		}
		else {
			instance.Chose.PlayReverse ();

		}
		isClick = !isClick;
	}

	public void CheckButton()
	{
		if (!isClick) {
			AutoCount = bufAutoCount;
			instance.AutoButton.SetActive (AutoCount == 0);
			instance.AutoButton1.SetActive (AutoCount != 0);
			instance.CountView.enabled = AutoCount != 0;
			CountView.text = bufAutoCount == 1000 ? unlimited : bufAutoCount.ToString ();
		}
	}

	public void AddAutoCount()
	{
		bufAutoCount++;
		CountViewChild.text = bufAutoCount == 1000 ? unlimited : bufAutoCount.ToString();
	}

	public void ReduceAutoCount()
	{
		bufAutoCount--;
		CountViewChild.text = bufAutoCount == 1000 ? unlimited : bufAutoCount.ToString();
	}

	public void MaxCount()
	{
		if (bufAutoCount < 1000) {
			bufAutoCount = 1000;
			CountViewChild.text = bufAutoCount == 1000 ? unlimited : bufAutoCount.ToString ();
		}
	}

	/// <summary>
	/// Autos the spins.
	/// </summary>
	public void AutoSpins()
	{
		instance.StartCoroutine (_AutoSpins());
	}

	public void StopComboSpin()
	{
		isStop = true;
	}

	IEnumerator _ComboSpins()
	{
		//Debug.LogWarning ("AutoCombo");
		while (SlotManager.slotState != SlotManager.State.Idle)
			yield return null;
		if (!isStop) {
			JurassicUIManager.instance.StartComboSpin ();
			while (SlotManager.slotState != SlotManager.State.Idle)
				yield return null;
			yield return new WaitForSeconds (waittime);
//			if (!isStop)
//				ComboSpin ();
		}
	}

	IEnumerator _AutoSpins()
	{
		//Debug.LogWarning ("Auto"+SlotManager.lastState);
		while (SlotManager.slotState != SlotManager.State.Idle)
			yield return null;
		if (AutoCount > 0  ||  SlotManager.lastState != SlotManager.State.spining) {
			if (AutoCount == 1000 && SlotManager.lastState == SlotManager.State.spining) {
				JurassicUIManager.instance.StartSpin ();
			}else if(SlotManager.lastState != SlotManager.State.spining){
				JurassicUIManager.instance.StartComboSpin ();
			}else {
				AutoCount--;
				if(!isClick)
					CountView.text = AutoCount.ToString ();
				JurassicUIManager.instance.StartSpin ();
			}
		}


		while (SlotManager.slotState != SlotManager.State.Idle) {
			//Debug.LogWarning ("wait");
			yield return null;
		}
		if (SlotManager.lastState != SlotManager.State.excited) {
			if(SlotManager.RewardMoney > 0)
			yield return new WaitForSeconds (waittime);
		}
		if (AutoCount > 0 || SlotManager.lastState != SlotManager.State.spining)
			AutoSpins ();


		if(!isClick){
			instance.AutoButton.SetActive (AutoCount == 0);
			instance.AutoButton1.SetActive (AutoCount != 0);
			instance.CountView.enabled = AutoCount != 0;
		}


	}
}
//无限