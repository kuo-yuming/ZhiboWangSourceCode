using UnityEngine;
using System.Collections;
using System;
using GameCore;
using GameCore.Manager.Jurassic;
using System.Collections.Generic;

public class JurassicUIManager : MonoBehaviour {

	public GameObject startButton;
	public GameObject StopButton;
	public UIButton stopBut;
	public UIButton startBut;
	public UILabel BetLabel;
	public UILabel playerMoney;
	public UInt64 _RewardMoney;
	public Collider stopCollider;
	public UInt64 RewardMoney{
		get{
			return _RewardMoney;
		}
		set{
			_RewardMoney = value;
		}
	}
	public int BetAmount = 1;
	public bool Test = false;
	private static JurassicUIManager _instance;

	public static JurassicUIManager instance
	{
		get{

			return _instance;
		}
	}

	public UInt64 UserMoney{
		get{
			return _UserMoney;
		}
		set{
			_UserMoney = value;
			playerMoney.text = _UserMoney.ToString ();
		}
	}

	private UInt64 _UserMoney;

	public enum State
	{
		Idle,
		Spin,
		OnHold,
	}

	void Start()
	{
		if (_instance == null)
			_instance = this.gameObject.GetComponent<JurassicUIManager> ();
		UserMoney = RewardMoney = GameConnet.m_BuyInMoney;
		stopBut = StopButton.GetComponent<UIButton> ();
		startBut = startButton.GetComponent<UIButton> ();
	}

	void Update()
	{
		//Debug.LogWarning (RewardMoney+"---"+UserMoney);
		if (RewardMoney > UserMoney) {
			if (RewardMoney - UserMoney >= 10000)
				UserMoney += 10000;
			if (RewardMoney - UserMoney >= 1000)
				UserMoney += 1000;
			if (RewardMoney - UserMoney >= 100)
				UserMoney += 100;
			if (RewardMoney - UserMoney >= 10)
				UserMoney += 10;
		}
		if (RewardMoney < UserMoney && RewardMoney > 0) {
			if (UserMoney - RewardMoney >= 100)
				UserMoney -= 100;
			if (UserMoney - RewardMoney >= 10)
				UserMoney -= 10;
		}
	}

	/// <summary>
	/// StartButton or autospin
	/// </summary>
	public void StartSpin()
	{
		if (SlotManager.lastState != SlotManager.State.spining) {
			Debug.Log ("AutoAnimaion");
			return;
		}
		if (!AutoSpin.instance.AutoSet && AutoSpin.instance.AutoCount != 0) {
			AutoSpin.instance.AutoSet = true;
			AutoSpin.instance.AutoSpins ();
		} else {
		if(!Test){
				//Debug.LogWarning (SlotManager.RewardMoney+"---"+SlotManager.playerMoney);
			//SlotManager.instance.SetItem();
			SlotManager.instance.SetMoney ();
			UserMoney = RewardMoney;
				if (BetAmount * GameConnet.m_PMachineConfig.m_usBetMoney * GameConnet.m_PMachineConfig.m_byMaxLine > (int)UserMoney) {
					Message_Control.OpenMessage = true;
					Message_Control.MessageStatus = Message_Control.MessageStatu.Buyin_MoneyNotEnough;
					Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
					return;
				}

			if (GameConnet.m_oGameClient != null)
				GameConnet.m_oGameClient.Send (ENUM_GAME_FRAME.Jurassic, (uint)ENUM_JURASSIC_PACKID_GC.C2G_Bet_ReqBet,
					GameConnet.m_oGameClient.DoSerialize<byte> ((byte)BetAmount));
				
				//Debug.LogWarning (UserMoney - (UInt64)(BetAmount * GameConnet.m_PMachineConfig.m_usBetMoney * GameConnet.m_PMachineConfig.m_byMaxLine)+"--"+RewardMoney);
				RewardMoney = UserMoney - (UInt64)(BetAmount * GameConnet.m_PMachineConfig.m_usBetMoney * GameConnet.m_PMachineConfig.m_byMaxLine);
				//BugUse.AddMessage ("Send package");
		}
		if (JurassicUIManager.instance.Test) {
			List<int> aaa = new List<int>{ 1,1,1,1,9,1,1,9,1,1,1,1,1,1,9};
			List<int> bbb = new List<int>{ 0,0,0,0,0,0,0,0,0,0};
			SlotManager.slotdata.SetData (bbb,aaa,SlotManager.instance.testcombo);
				SlotManager.slotdata.JackPot = SlotManager.instance.JP;
			SlotManager.slotdata.SetExcited (SlotManager.instance.testexcited,SlotManager.instance.textexcit);
		}
			SlotManager.StartButton ();
		}
	}

	/// <summary>
	/// excited or freegame use
	/// </summary>
	public void StartComboSpin()
	{
		if(!JurassicUIManager.instance.Test){
			//SlotManager.instance.SetItem();
			SlotManager.instance.SetMoney ();
			UserMoney = RewardMoney;
			if (GameConnet.m_oGameClient != null)
				GameConnet.m_oGameClient.Send (ENUM_GAME_FRAME.Jurassic, (uint)ENUM_JURASSIC_PACKID_GC.C2G_Bet_ReqBet,
					GameConnet.m_oGameClient.DoSerialize<byte> ((byte)BetAmount));
			
			RewardMoney = UserMoney - (UInt64)(BetAmount * GameConnet.m_PMachineConfig.m_usBetMoney * GameConnet.m_PMachineConfig.m_byMaxLine);
		}
		if (JurassicUIManager.instance.Test) {
			List<int> aaa = new List<int>{ 1,1,1,1,9,1,1,9,1,1,1,1,1,1,9};
			List<int> bbb = new List<int>{ 0,0,3,0,0,0,0,0,0,0};
			SlotManager.slotdata.SetData (bbb,aaa,SlotManager.instance.testcombo);
			SlotManager.slotdata.JackPot = SlotManager.instance.JP;
			SlotManager.slotdata.SetExcited (SlotManager.instance.testexcited,SlotManager.instance.textexcit);
		}
		SlotManager.StartButton ();
	}

	public void StopSpin()
	{
		if (SlotManager.slotdata.slotSet == false || SlotManager.slotState == SlotManager.State.Idle)
			return;
		SlotManager.StopButton ();
	}

	public void BuyOut()
	{
		GameConnet.BuyOut_GameLobbySuccess = true;
	}
}
