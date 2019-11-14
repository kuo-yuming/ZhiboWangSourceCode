using UnityEngine;
using System.Collections;

public class BetButton : MonoBehaviour {
	public  UILabel betView;
	public  UILabel betViewChild;
	public UISprite more;
	public UISprite less;
	public UISprite max;
	public TweenPosition Chose;
	public UISprite Betbutton;
	bool isClick = false;
	private static BetButton _instance;
	public static BetButton instance{get {return _instance;}}

	int _BetAmount = 1;
	public int BetAmount{
		get {
			return _BetAmount;
		}
		set{
			_BetAmount = value;
			betViewChild.text = (_BetAmount * GameConnet.m_PMachineConfig.m_usBetMoney * GameConnet.m_PMachineConfig.m_byMaxLine).ToString ();
			JurassicUIManager.instance.BetAmount = value;
			//betView.text = (JurassicUIManager.instance.BetAmount * GameConnet.m_PMachineConfig.m_usBetMoney * GameConnet.m_PMachineConfig.m_byMaxLine).ToString ();
		}
	}
		
	void Start () {
		if (_instance == null)
			_instance = this.gameObject.GetComponent<BetButton> ();
		betView.text = betViewChild.text = (JurassicUIManager.instance.BetAmount * GameConnet.m_PMachineConfig.m_usBetMoney * GameConnet.m_PMachineConfig.m_byMaxLine).ToString ();
	}

	void Update()
	{
		if (BetAmount == GameConnet.m_PMBetMax) {
			more.spriteName = "btn_more_1";
			max.spriteName = "btn_max_1";
		} else {
			more.spriteName = "btn_more_0";
			max.spriteName = "btn_max_0";
		}

		if (BetAmount == 1) {
			less.spriteName = "btn_less_1";
		} else {
			less.spriteName = "btn_less_0";
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
		AutoSpin.instance.CheckClick ();
		Jurassic_GameUIBag.instance.CheckClick ();
		JurassicInfos.instance.CheckClick ();
		if (SlotManager.lastState != SlotManager.State.spining)
			return;
		if (!isClick) {
			BetAmount = JurassicUIManager.instance.BetAmount;
			instance.Chose.PlayForward ();
			Betbutton.spriteName = "btn_moneyback_1";
			betView.text = string.Empty;
		}
		else {
			JurassicUIManager.instance.BetAmount = BetAmount;
			instance.Chose.PlayReverse ();
		}
		isClick = !isClick;
	}

	public void CheckButton()
	{
		if(!isClick){
			Betbutton.spriteName = "btn_moneyback_2";
			betView.text = (JurassicUIManager.instance.BetAmount * GameConnet.m_PMachineConfig.m_usBetMoney * GameConnet.m_PMachineConfig.m_byMaxLine).ToString ();
		}
	}

	public void BetAmountAdd()
	{
		if(BetAmount < GameConnet.m_PMBetMax)
			BetAmount++;
	}

	public void ReduceBetAmount()
	{
		if(BetAmount - 1 > 0)
			instance.BetAmount--;
	}

	public void MaxAmount()
	{
		if(BetAmount < GameConnet.m_PMBetMax)
			BetAmount = GameConnet.m_PMBetMax;
	}
}
