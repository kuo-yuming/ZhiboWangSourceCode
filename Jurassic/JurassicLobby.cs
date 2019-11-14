using UnityEngine;
using System.Collections;
using GameCore;
using System;

public class JurassicLobby : MonoBehaviour {
	public UIButton m_KeepMachineBtn;
	public static event Action<int> change;
	IEnumerator Start () {
		while (!JurassicManager.isInit)
			yield return null;
		//JurassicManager.GetMachineDataU2G();
		m_KeepMachineBtn.isEnabled = GameConnet.m_uiKeepMID != 0 ? true : false;
		if (change != null)
			change (JurassicManager.page);
	}
	void Close_OnClick()
	{
		JurassicManager.page = 0;
		GameConnet.CloseGameConnet();
	}
	void Next_OnClick()
	{
		JurassicManager.page += JurassicManager.page == JurassicManager.sumPage ? -JurassicManager.page : 1;
		JurassicManager.GetMachineDataU2G();
		if (change != null)
			change (JurassicManager.page);
	}
	void Back_OnClick()
	{
		JurassicManager.page += JurassicManager.page == 0 ? JurassicManager.sumPage : -1;
		JurassicManager.GetMachineDataU2G();
		if (change != null)
			change (JurassicManager.page);
	}
	void AutoBuyIn_OnClick()
	{
		GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Jurassic, (uint)ENUM_COMMON_PACKID_GC.C2G_PMachine_AutoSelect,null);
	}
	void BackMachine_OnClick()
	{
		if (GameConnet.m_uiKeepMID != 0)
			JurassicManager.BuyInGame(GameConnet.m_uiKeepMID);
	}
}
