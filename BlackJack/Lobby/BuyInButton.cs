using UnityEngine;
using System.Collections;
using GameCore;
using GameCore.Machine;
using System.Collections.Generic;

public class BuyInButton : MonoBehaviour {
    public static List<uint> usTableID = new List<uint>(); 
	// Use this for initialization
	void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        AllScenceLoad.LoadScence = true;
        CPACK_TMachineAutoSelect Data = new CPACK_TMachineAutoSelect();
        Data.m_uiStartTID = BlackJackManager.m_MachineBuyInConfig.m_dicTableGroupSet[(ushort)usTableID[0]].m_uiStartTableID;
        Data.m_uiEndTID = BlackJackManager.m_MachineBuyInConfig.m_dicTableGroupSet[(ushort)usTableID[0]].m_uiStartTableID;
        Debug.Log("StartID: " + Data.m_uiStartTID + " //EndID: " + Data.m_uiEndTID);
        GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.BlackJack, (uint)ENUM_COMMON_PACKID_GC.C2G_TMachine_AutoSelect, GameConnet.m_oGameClient.DoSerialize<CPACK_TMachineAutoSelect>(Data));
    }
}
