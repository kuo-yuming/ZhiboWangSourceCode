using UnityEngine;
using System.Collections;
using GameCore.Manager.Common;
using GameCore;
using GameCore.Machine;


public class AutoBuyIn : MonoBehaviour {

    public enum ENUM_LOBBY_BUTTON
    {
        SmallTable = 0,
        BigTable = 1,
        GameOutButton = 10,
        ExplainButton = 11,
        CompetitionButton = 12,
        ExplainNextButton = 13,
        ExplainBackButton = 14,
        ExplainCloseButton = 15,
        CompetitionClose = 16,
        BeforeRankingClose = 17,
        RaceInfoClose = 18,
    }

    public ENUM_LOBBY_BUTTON ButtonID;
    public byte TableID;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (AutoButton_Control.TableGroupID.Count != 0)
        {
            for (byte i = 0; i < AutoButton_Control.TableGroupID.Count; i++)
            {
                if (i == (byte)ButtonID)
                {
                    TableID = byte.Parse(AutoButton_Control.TableGroupID[i].ToString());
                }
            }
        }

        if (ButtonID == ENUM_LOBBY_BUTTON.BigTable)
        {
            Bet_Control.BetID = AutoButton_Control.ClickGroupID;
        }
	}

    void OnClick()
    {
        if (ButtonID == ENUM_LOBBY_BUTTON.SmallTable)
        {
            if (!Competition.SinedOK_Bool)
            {
                if (MainConnet.m_PlayerData.m_usLv >= BaccaratManager.m_MachineBuyInConfig.m_usBuyinLv)
                {
                    AllScenceLoad.LoadScence = true;
                    AutoButton_Control.ClickGroupID = TableID;
                    CPACK_TMachineAutoSelect Data = new CPACK_TMachineAutoSelect();
                    Data.m_uiStartTID = BaccaratManager.m_MachineBuyInConfig.m_dicTableGroupSet[AutoButton_Control.ClickGroupID].m_uiStartTableID;
                    Data.m_uiEndTID = BaccaratManager.m_MachineBuyInConfig.m_dicTableGroupSet[AutoButton_Control.ClickGroupID].m_uiEndTableID;
                    Debug.Log("小底台" + "StartID: " + Data.m_uiStartTID + " //EndID: " + Data.m_uiEndTID);
                    GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Baccarat, (uint)ENUM_COMMON_PACKID_GC.C2G_TMachine_AutoSelect, GameConnet.m_oGameClient.DoSerialize<CPACK_TMachineAutoSelect>(Data));
                }
                else
                {
                    Message_Control.OpenMessage = true;
                    Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                    Message_Control.MessageStatus = Message_Control.MessageStatu.RookiePlayerCanNotEnter;
                }
            }
            else
            {
                Message_Control.OpenMessage = true;
                Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                Message_Control.MessageStatus = Message_Control.MessageStatu.NowRace;
            }
        }
        else if (ButtonID == ENUM_LOBBY_BUTTON.BigTable)
        {
            if (!Competition.SinedOK_Bool)
            {
                if (MainConnet.m_PlayerData.m_usLv >= BaccaratManager.m_MachineBuyInConfig.m_usBuyinLv)
                {
                    AllScenceLoad.LoadScence = true;
                    AutoButton_Control.ClickGroupID = TableID;
                    CPACK_TMachineAutoSelect Data = new CPACK_TMachineAutoSelect();
                    Data.m_uiStartTID = BaccaratManager.m_MachineBuyInConfig.m_dicTableGroupSet[AutoButton_Control.ClickGroupID].m_uiStartTableID;
                    Data.m_uiEndTID = BaccaratManager.m_MachineBuyInConfig.m_dicTableGroupSet[AutoButton_Control.ClickGroupID].m_uiEndTableID;
                    Debug.Log("大底台: " + Data.m_uiStartTID + " //EndID: " + Data.m_uiEndTID);
                    GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Baccarat, (uint)ENUM_COMMON_PACKID_GC.C2G_TMachine_AutoSelect, GameConnet.m_oGameClient.DoSerialize<CPACK_TMachineAutoSelect>(Data));
                }
                else
                {
                    Message_Control.OpenMessage = true;
                    Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                    Message_Control.MessageStatus = Message_Control.MessageStatu.RookiePlayerCanNotEnter;
                }
            }
            else
            {
                Message_Control.OpenMessage = true;
                Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                Message_Control.MessageStatus = Message_Control.MessageStatu.NowRace;
            }

        }
        else if (ButtonID == ENUM_LOBBY_BUTTON.GameOutButton)
        {
            GameConnet.CloseGameConnet();
            Competition.CompetitionData.Clear();
            Competition.SequenceData.Clear();
            Competition.ListObject.Clear();
        }
        else if (ButtonID == ENUM_LOBBY_BUTTON.ExplainButton)
        {
            if (!Explain.ExplainBoxOpen_Bool)
            {
                Explain.ExplainBoxOpen_Bool = true;
            }
        }
        else if (ButtonID == ENUM_LOBBY_BUTTON.CompetitionButton)
        {
            if (!Competition.CompetitionBoxOpen_Bool)
            {
                Competition.CompetitionBoxOpen_Bool = true;
            }
        }
        else if (ButtonID == ENUM_LOBBY_BUTTON.ExplainNextButton)
        {
            if (Explain.Page_Number > 5)
            {
                Explain.Page_Number = 1;
            }
            else
            {
                Explain.Page_Number++;
            }
        }
        else if (ButtonID == ENUM_LOBBY_BUTTON.ExplainBackButton)
        {
            if (Explain.Page_Number < 2)
            {
                Explain.Page_Number = 6;
            }
            else
            {
                Explain.Page_Number--;
            }
        }
        else if (ButtonID == ENUM_LOBBY_BUTTON.ExplainCloseButton)
        {
            Explain.ExplainBoxOpen_Bool = false;
        }
        else if (ButtonID == ENUM_LOBBY_BUTTON.CompetitionClose)
        {
            Competition.CompetitionBoxOpen_Bool = false;
        }
        else if (ButtonID == ENUM_LOBBY_BUTTON.BeforeRankingClose)
        {
            Competition.BeforeRankingBoxOpen_Bool = false;
        }
        else if (ButtonID == ENUM_LOBBY_BUTTON.RaceInfoClose)
        {
            RaceInfo.RaceInfoObject_bool = false;
        }
    }
}
