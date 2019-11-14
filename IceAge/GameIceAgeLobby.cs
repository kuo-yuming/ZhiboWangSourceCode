using UnityEngine;
using System.Collections;
using GameCore;

public class GameIceAgeLobby : MonoBehaviour {
    public UIButton m_KeepMachineBtn;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (IceAgeManager.LoadGameEnd)
        {
            if (GameConnet.m_uiKeepMID != 0)
            {
                m_KeepMachineBtn.isEnabled = true;
            }
            else
            {
                m_KeepMachineBtn.isEnabled = false;
            }
        }
	}
    void Close_OnClick()
    {
        GameConnet.CloseGameConnet();
    }
    void Next_OnClick()
    {
        IceAgeManager.ChangeMachinePage(1);
    }
    void Back_OnClick()
    {
        IceAgeManager.ChangeMachinePage(2);
    }
    void AutoBuyIn_OnClick()
    {
        GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.IceAge, (uint)ENUM_COMMON_PACKID_GC.C2G_PMachine_AutoSelect,
                                       null);
    }
    void BackMachine_OnClick()
    {
        if (GameConnet.m_uiKeepMID != 0)
        {
            IceAgeManager.BuyInGame(GameConnet.m_uiKeepMID);
        }
    }
}
