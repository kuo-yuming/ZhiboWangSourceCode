using UnityEngine;
using System.Collections;
using GameCore;
using GameCore.Manager.Common;
public class BankPlayerData : MonoBehaviour {
    
    public uint PlayerDBID;
    public string PlayerName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
     
	}

    void OnClick()
    {
        MainConnet.m_oMainClient.Send(ENUM_GAME_FRAME.Common, (uint)ENUM_COMMON_PACKID_GC.C2G_Transaction_ReqInviteTransaction,
                              MainConnet.m_oMainClient.DoSerialize<uint>(PlayerDBID));
        Business_Control.BusinessWaitTime = true;
        CashBusiness.PlayerDBID = PlayerDBID;
        CashBusiness.BusinessPlayerName = PlayerName;
        Debug.Log("交易對象DBID: " + PlayerDBID);
    }
}
