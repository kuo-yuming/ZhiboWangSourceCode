using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore.Manager.Common;
using GameCore;
using BankEnum;

public class PlayerListCheck : MonoBehaviour {

    public static Dictionary<uint, CPACK_GetPlayerSimpleInfoResult> GetAllPlayerList = new Dictionary<uint, CPACK_GetPlayerSimpleInfoResult>();
    public static bool GetPlayerListData = false;
    public static uint PlayerDataNumber = 0;
    // Use this for initialization
    void Start()
    {
        GetPlayerListData = false;
        PlayerDataNumber = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (GetPlayerListData)
        {
            //Debug.LogError(System.DateTime.Now.ToString()+ System.DateTime.Now.Millisecond);
            //要求玩家資料
            foreach (var item in SNS_Manager.Public_GroupsMemberData)
            {
                MainConnet.m_oMainClient.Send(ENUM_GAME_FRAME.Common, (uint)ENUM_COMMON_PACKID_GC.C2G_Actor_ReqPlayerSimpleInfo,
                                        MainConnet.m_oMainClient.DoSerialize<uint>(item.Value.m_uiDBID));
                PlayerDataNumber++;
            }
           // Debug.LogError(System.DateTime.Now.ToString() + System.DateTime.Now.Millisecond);
            //要求好友資料
            foreach (var item in SNS_Manager.m_dicFriends)
            {
                if (item.Value.m_uiDBID != SNS_Manager.m_FriendConfig.m_uiFriendGMDBID)
                {
                    MainConnet.m_oMainClient.Send(ENUM_GAME_FRAME.Common, (uint)ENUM_COMMON_PACKID_GC.C2G_Actor_ReqPlayerSimpleInfo,
                                            MainConnet.m_oMainClient.DoSerialize<uint>(item.Value.m_uiDBID));
                    PlayerDataNumber++;
                }
                
            }
           // Debug.LogError(System.DateTime.Now.ToString() + System.DateTime.Now.Millisecond);
            GetPlayerListData = false;
        }

        if (Bank_Control.BankPage != (byte)ENUM_BANK_PAGE.BusinessPage)
        {
            GetAllPlayerList.Clear();
            PlayerDataNumber = 0;
        }
	}
}
