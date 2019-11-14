using UnityEngine;
using System.Collections;
using GameCore.Machine;
using GameCore.Manager.BlackJack;
using System.Collections.Generic;
using GameCore;
using System;
using MoneyTable;
using WinLoseListClass;
using CardTeamListClass;
using HistorySaveData;

public class BlackJackManager : MonoBehaviour {

    public static string GameVol = "1.0.0.170224";
    public static CPACK_TMGameConfig m_MachineBuyInConfig = null;       //機台設定資料
    public static CPACK_BlackJack_GameConfig m_GameConfig = null;       //遊戲設定資料
    public static Dictionary<uint, CPACK_TMachineData> m_MachineDatas = null;         //機台資料
    public static Dictionary<uint, ushort> m_MachineTableArea = null;         //機台資料
    public static CPACK_TMachineMemberList MachineMemberList = new CPACK_TMachineMemberList();  //該機台 成員名單


    private UnityEngine.Object BetLock = new UnityEngine.Object();


    // Use this for initialization
    void Start()
    {
        Competition.DataChange_Bool = false;
    }

    // Update is called once per frame
    void Update()
    {
    }
    //---------------------------------------------------------------------

    public void Reseat()
    {
        if (m_MachineDatas != null)
            m_MachineDatas.Clear();         //機台資料
        if (m_MachineTableArea != null)
            m_MachineTableArea.Clear();         //機台資料

    }

    // 當收到汎用系統封包時觸發
    public void OnRcvCommonFrameData(uint uiPackID, byte[] byarData)
    {
        Debug.Log(string.Format("OnRcvCommonFrameData. PackID={0}", uiPackID));

        switch (uiPackID)
        {

            case (uint)ENUM_COMMON_PACKID_GC.G2C_TMachine_KickToLobby:
                Message_Control.OpenMessage = true;
                Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                Message_Control.MessageStatus = Message_Control.MessageStatu.PlayerOut;
                break;
            case (uint)ENUM_COMMON_PACKID_GC.C2G_TMachine_ReqBuyout:
                Message_Control.OpenMessage = true;
                Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                Message_Control.MessageStatus = Message_Control.MessageStatu.PlayerOut;
                break;
            default:
                Debug.Log(string.Format("{0}:Unknown packid={1}", DateTime.Now, uiPackID));
                break;
        }
        Debug.Log(string.Format("OnRcvCommonFrameData. PackID={0} end", uiPackID));
    }

    // 當收到BlackJack系統封包時觸發
    public void OnRcvBlackJackData(uint uiPackID, byte[] byarData)
    {
        Debug.Log(string.Format("OnRcvBlackJackData. PackID={0}", uiPackID));

        switch (uiPackID)
        {
            case (uint)ENUM_COMMON_PACKID_GC.G2C_TMachine_NotifyGameConfig:
                RcvMachineData(byarData);
                break;
            case (uint)ENUM_BlackJack_PACKID_GC.G2C_NotifyGameConfig:
                RcvGameConfigData(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_TMachine_NotifyMachineList:
                RcvMachineList(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_TMachine_NotifyMachineData:
                RcvUpdateMachineList(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_TMachine_NotifyStartGame:
                RcvBuyInSuccess(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_TMachine_AutoSelectResult:
                RcvAutoBuyIn(byarData);
                break;

            //BJ21
            case (uint)ENUM_BlackJack_PACKID_GC.G2C_Game_SeatDataOne:
                SeatDataCheck(byarData);
                break;
            case (uint)ENUM_BlackJack_PACKID_GC.G2C_Game_SeatDataAll:
                NowSeatDataCheck(byarData);
                break;
            case (uint)ENUM_BlackJack_PACKID_GC.G2C_Game_NotifyState:
                NowTableState(byarData);
                break;
            case (uint)ENUM_BlackJack_PACKID_GC.G2C_Game_NotifyDeal:
                FirstCardListResult(byarData);
                break;
            case (uint)ENUM_BlackJack_PACKID_GC.G2C_Game_NotifyBet:
                GetBetResult(byarData);
                break;
            case (uint)ENUM_BlackJack_PACKID_GC.G2C_Game_NowTargetPlayer:
                NowPlayingPlayer(byarData);
                break;
            case (uint)ENUM_BlackJack_PACKID_GC.G2C_Game_playerDo:
                ByuInsureResult(byarData);
                break;
            case (uint)ENUM_BlackJack_PACKID_GC.G2C_Game_NotifyAward:
                FinallResult(byarData);
                break;
            case (uint)ENUM_BlackJack_PACKID_GC.G2C_Game_AllPlayerWin:
                FinallEnd(byarData);
                break;
            case (uint)ENUM_BlackJack_PACKID_GC.G2C_Game_PlayerOut:
                GamePlayerOut(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_TMachine_KickToLobby:
                Message_Control.OpenMessage = true;
                Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                Message_Control.MessageStatus = Message_Control.MessageStatu.PlayerOut;
                break;
            default:
                Debug.Log(string.Format("{0}:Unknown packid={1}", DateTime.Now, uiPackID));
                break;
        }

        Debug.Log(string.Format("OnRcvBlackJackData. PackID={0} end", uiPackID));
    }
    //---------------------------------------------------------------------



    // 收到遊戲資料資料
    public void RcvGameConfigData(byte[] byarData)
    {
        m_GameConfig = GameConnet.m_oGameClient.DoDeSerialize<CPACK_BlackJack_GameConfig>(byarData);
        Debug.Log("收到機台資料");
    }
    //---------------------------------------------------------------------
    // 收到檯桌式機台機制
    public void RcvMachineData(byte[] byarData)
    {
        BuyInButton.usTableID.Clear();
        m_MachineDatas = new Dictionary<uint, CPACK_TMachineData>();
        m_MachineTableArea = new Dictionary<uint, ushort>();
        m_MachineBuyInConfig = GameConnet.m_oGameClient.DoDeSerialize<CPACK_TMGameConfig>(byarData);
        BJMainGame_Control.NoBetRound = m_MachineBuyInConfig.m_byChkUnbetRound;
        Debug.Log("收到檯桌式機台機制 : " + "等級多少以上才能Buyin : " + MainConnet.m_PlayerData.m_strNickName);
        foreach (var item in m_MachineBuyInConfig.m_dicTableGroupSet)
        {
            Debug.Log("分區標籤名稱 : " + item.Value.m_strTagName + "  起始桌號 : " + item.Value.m_uiStartTableID + "  末尾桌號 : " + item.Value.m_uiEndTableID);
        }
        foreach (var item in m_MachineBuyInConfig.m_dicTableGroupSet)
        {
            BuyInButton.usTableID.Add(item.Value.m_byGroupID);
            for (uint i = item.Value.m_uiStartTableID; i <= item.Value.m_uiEndTableID; i++)
            {
                if (!m_MachineDatas.ContainsKey(i))
                {
                    CPACK_TMachineData Data = new CPACK_TMachineData();
                    Data.m_uiTID = i;
                    Data.m_usMemberCnt = 0;
                    m_MachineDatas.Add(i, Data);

                }
                if (!m_MachineTableArea.ContainsKey(i))
                    m_MachineTableArea.Add(i, item.Value.m_byGroupID);
            }
        }
        AutoButton_Control.TableDataGet = true;
        Debug.Log("收到檯桌式機台資料");
    }
    //---------------------------------------------------------------------
    // 收到機台資料
    public void RcvMachineList(byte[] byarData)
    {
        CPACK_TMachineDataList LocalData = GameConnet.m_oGameClient.DoDeSerialize<CPACK_TMachineDataList>(byarData);
        if (LocalData != null)
        {
            foreach (var item in LocalData.m_listMachineData)
            {
                if (m_MachineDatas.ContainsKey(item.m_uiTID))
                {
                    Debug.Log("收到更新機台人數   ID : " + item.m_uiTID + "  Cnt : " + item.m_usMemberCnt);
                    m_MachineDatas[item.m_uiTID].m_usMemberCnt = item.m_usMemberCnt;
                }
                else
                {
                    Debug.LogError("收到更新機台人數 : 未知的機台ID");
                    continue;
                }
            }
        }
    }
    //---------------------------------------------------------------------
    // 收到更新機台資料
    public void RcvUpdateMachineList(byte[] byarData)
    {
        CPACK_TMachineData MachineData = GameConnet.m_oGameClient.DoDeSerialize<CPACK_TMachineData>(byarData);
        m_MachineDatas[MachineData.m_uiTID] = MachineData;
        Debug.Log("收到更新機台 : " + MachineData.m_uiTID + " 資料");
    }
    //---------------------------------------------------------------------

    // 收到玩家成功BuyIn
    public void RcvBuyInSuccess(byte[] byarData)
    {

        GameConnet.m_TMachineBuyInGameData = GameConnet.m_oGameClient.DoDeSerialize<CPACK_TMachineEnter>(byarData);
        GameConnet.m_BuyInMoney = GameConnet.m_TMachineBuyInGameData.m_uiGameMoney;
        GameConnet.m_NowBuyInMachineID = GameConnet.m_TMachineBuyInGameData.m_uiTID;
        GameConnet.LogIn_GameSuccess = true;
        RaceWindowBox.RaceMoneyBoxBool = false;
        Debug.Log("成功BUYIN: " + GameConnet.m_TMachineBuyInGameData.m_uiGameMoney);
    }
    public void RcvBuyInFail(byte[] byarData)
    {
        Message_Control.OpenMessage = true;
        Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
        Message_Control.MessageStatus = Message_Control.MessageStatu.Buyin_LVNotEnough;
        AllScenceLoad.LoadScence = false;
    }
    //---------------------------------------------------------------------
    // 收到玩家自動配位
    public void RcvAutoBuyIn(byte[] byarData)
    {
        CPACK_TMachineAutoSelectResult PlayerAutoBuyIn = GameConnet.m_oGameClient.DoDeSerialize<CPACK_TMachineAutoSelectResult>(byarData);
        if (PlayerAutoBuyIn.m_iResultCode == (int)ENUM_COMMON_ERROR_CODE.Success)
        {
            if (MainConnet.m_PlayerData.m_ui64OwnMoney >= m_MachineBuyInConfig.m_uiMinBuyinMoney)
            {
                ulong BuyInMoney = MainConnet.m_PlayerData.m_ui64OwnMoney;
                if (BuyInMoney > m_MachineBuyInConfig.m_uiMaxBuyinMoney)
                    BuyInMoney = m_MachineBuyInConfig.m_uiMaxBuyinMoney;
                CPACK_TMachineBuyin m_BuyInMoney = new CPACK_TMachineBuyin();
                m_BuyInMoney.m_uiTID = PlayerAutoBuyIn.m_uiTID;
                m_BuyInMoney.m_uiBuyinMoney = (uint)BuyInMoney;
                GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.BlackJack, (uint)ENUM_COMMON_PACKID_GC.C2G_TMachine_ReqBuyin, GameConnet.m_oGameClient.DoSerialize<CPACK_TMachineBuyin>(m_BuyInMoney));
                Debug.Log("自動BUYIN");
            }
            else
            {
                AllScenceLoad.LoadScence = false;
                Message_Control.OpenMessage = true;
                Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                Message_Control.MessageStatus = Message_Control.MessageStatu.Buyin_MoneyNotEnough;
            }
        }
        else
        {
            AllScenceLoad.LoadScence = false;
            Message_Control.OpenMessage = true;
            Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
            Message_Control.MessageStatus = Message_Control.MessageStatu.Machine_NoneIdleMachine;
        }


    }
    //---------------------------------------------------------------------

    public void Get_WebKeyFail(string WebKey)
    {

    }

    //---------------------------------------------------------------------
    //BJ21
    //更新桌台狀態
    public void SeatDataCheck(byte[] byarData)
    {
        CSeatPlayerData Data = GameConnet.m_oGameClient.DoDeSerialize<CSeatPlayerData>(byarData);
        if (Data.m_uiPlayerDBID != MainConnet.m_PlayerData.m_uiDBID && Data.m_uiPlayerDBID != 0)
        {
            if (Cash_Control.SavePlayerSeat < Data.m_bySeatID)
            {
                Cash_Control.ThisTablePlayerIn[Data.m_bySeatID - 1] = 1;
                Cash_Control.PlayerDBID[Data.m_bySeatID - 1] = Data.m_uiPlayerDBID;
                Cash_Control.PlayerRealSeat[Data.m_bySeatID - 1] = Data.m_bySeatID;
                Cash_Control.PlayerName[Data.m_bySeatID - 1] = Data.m_strNickName;
                Cash_Control.TableCash[(Data.m_bySeatID - 1) * 2] = (int)Data.m_uiPlayerBet;
                Debug.Log("更新座位狀態: " + (Data.m_bySeatID - 1).ToString() + " \\原本座位ID: " + Data.m_bySeatID + " \\自己座位ID: " + Cash_Control.SavePlayerSeat);
            }
            else
            {
                Cash_Control.ThisTablePlayerIn[Data.m_bySeatID] = 1;
                Cash_Control.PlayerDBID[Data.m_bySeatID] = Data.m_uiPlayerDBID;
                Cash_Control.PlayerRealSeat[Data.m_bySeatID] = Data.m_bySeatID;
                Cash_Control.PlayerName[Data.m_bySeatID] = Data.m_strNickName;
                Cash_Control.TableCash[Data.m_bySeatID * 2] = (int)Data.m_uiPlayerBet;
                Debug.Log("更新座位狀態: " + Data.m_bySeatID + " \\自己座位ID: " + Cash_Control.SavePlayerSeat + " //PlaterDBID: " + Data.m_uiPlayerDBID);
            }
        }
    }

    //所有座位現在資料
    public void NowSeatDataCheck(byte[] byarData)
    {
        CAllSeatData Data = GameConnet.m_oGameClient.DoDeSerialize<CAllSeatData>(byarData);
        if (Data != null)
        {
            for (int i = 0; i < Data.m_liSeatDatas.Count; i++)
            {
                if (Data.m_liSeatDatas[i].m_uiPlayerDBID == MainConnet.m_PlayerData.m_uiDBID && Cash_Control.ThisTablePlayerIn[(byte)TableList.MyTable] != 1 && Data.m_liSeatDatas[i].m_uiPlayerDBID != 0)
                {
                    Cash_Control.ThisTablePlayerIn[(byte)TableList.MyTable] = 1;
                    Cash_Control.PlayerDBID[(byte)TableList.MyTable] = Data.m_liSeatDatas[i].m_uiPlayerDBID;
                    Cash_Control.SavePlayerSeat = Data.m_liSeatDatas[i].m_bySeatID;
                    Cash_Control.PlayerRealSeat[(byte)TableList.MyTable] = Data.m_liSeatDatas[i].m_bySeatID;
                    Cash_Control.PlayerName[(byte)TableList.MyTable] = Data.m_liSeatDatas[i].m_strNickName;
                }
            }

            for (int i = 0; i < Data.m_liSeatDatas.Count; i++)
            {
                if (Data.m_liSeatDatas[i].m_uiPlayerDBID != MainConnet.m_PlayerData.m_uiDBID && Data.m_liSeatDatas[i].m_uiPlayerDBID != 0)
                {
                    if (Cash_Control.SavePlayerSeat < Data.m_liSeatDatas[i].m_bySeatID)
                    {
                        Cash_Control.ThisTablePlayerIn[Data.m_liSeatDatas[i].m_bySeatID - 1] = 1;
                        Cash_Control.PlayerDBID[Data.m_liSeatDatas[i].m_bySeatID - 1] = Data.m_liSeatDatas[i].m_uiPlayerDBID;
                        Cash_Control.PlayerRealSeat[Data.m_liSeatDatas[i].m_bySeatID - 1] = Data.m_liSeatDatas[i].m_bySeatID;
                        Cash_Control.PlayerName[Data.m_liSeatDatas[i].m_bySeatID - 1] = Data.m_liSeatDatas[i].m_strNickName;
                        Cash_Control.TableCash[(Data.m_liSeatDatas[i].m_bySeatID - 1) * 2] = (int)Data.m_liSeatDatas[i].m_uiPlayerBet;
                    }
                    else
                    {
                        Cash_Control.ThisTablePlayerIn[Data.m_liSeatDatas[i].m_bySeatID] = 1;
                        Cash_Control.PlayerDBID[Data.m_liSeatDatas[i].m_bySeatID] = Data.m_liSeatDatas[i].m_uiPlayerDBID;
                        Cash_Control.PlayerRealSeat[Data.m_liSeatDatas[i].m_bySeatID] = Data.m_liSeatDatas[i].m_bySeatID;
                        Cash_Control.PlayerName[Data.m_liSeatDatas[i].m_bySeatID] = Data.m_liSeatDatas[i].m_strNickName;
                        Cash_Control.TableCash[Data.m_liSeatDatas[i].m_bySeatID * 2] = (int)Data.m_liSeatDatas[i].m_uiPlayerBet;
                    }
                }
            }
        }

        if (Data == null)
            Debug.Log("所有座位現在資料: " + "Data == NULL");
        else
            Debug.Log("所有座位現在資料");
    }

    //更新檯桌的狀態階段
    public void NowTableState(byte[] byarData)
    {
        CPACK_BlackJack_UpdateTbleState Data = GameConnet.m_oGameClient.DoDeSerialize<CPACK_BlackJack_UpdateTbleState>(byarData);

        //自己動作
        if (Cash_Control.PlayerRound[(byte)TableList.MyTable] == 1)
        {
            BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState = ENUM_BLACKJACK_TABLE_STATE.CheckPlayer;
            if (BJMainGame_Control.WaitAngle)
                BJMainGame_Control.WaitAngle = false;
        }

        if (Data.m_enumState != ENUM_BLACKJACK_TABLE_STATE.PlayerTime)
        {
            for (byte i = 0; i < 5; i++)
                Cash_Control.PlayerRound[i] = 0;
        }

        BJMainGame_Control.NowStateSave = Data.m_enumState;

        if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState != ENUM_BLACKJACK_TABLE_STATE.Idle)
        {
            if ((BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState != ENUM_BLACKJACK_TABLE_STATE.PlayerOver) && BJMainGame_Control.NowStateSave == ENUM_BLACKJACK_TABLE_STATE.GameOver)
            {
                ButtonSprite_Control.BetInsure_Bool = false;
                ButtonSprite_Control.BJ21_Bool = false;
                ButtonSprite_Control.Scoreboard_Bool = false;
                Cash_Control.PlayerRound[(byte)TableList.MyTable] = 0;
                BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState = ENUM_BLACKJACK_TABLE_STATE.PlayerOver;
                BJMainGame_Control.NowStateSave = ENUM_BLACKJACK_TABLE_STATE.GameSettlement;
            }
            else if ((BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.PlayerOver) && BJMainGame_Control.NowStateSave == ENUM_BLACKJACK_TABLE_STATE.GameOver)
            {
                ButtonSprite_Control.BetInsure_Bool = false;
                ButtonSprite_Control.BJ21_Bool = false;
                ButtonSprite_Control.Scoreboard_Bool = false;
                BJMainGame_Control.NowStateSave = ENUM_BLACKJACK_TABLE_STATE.GameSettlement;
            }
            else if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.InsuranceTime || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.InsuranceOver)
            {
                if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.InsuranceOver && Cash_Control.PlayerRound[(byte)TableList.MyTable] == 0)
                {
                    BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState = ENUM_BLACKJACK_TABLE_STATE.CheckPlayer;
                }
                if (Data.m_enumState == ENUM_BLACKJACK_TABLE_STATE.GameOver)
                {
                    ButtonSprite_Control.BetInsure_Bool = false;
                    ButtonSprite_Control.BJ21_Bool = false;
                    ButtonSprite_Control.Scoreboard_Bool = false;
                    BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState = ENUM_BLACKJACK_TABLE_STATE.PlayerOver;
                    BJMainGame_Control.NowStateSave = ENUM_BLACKJACK_TABLE_STATE.GameSettlement;
                }
                else if (Data.m_enumState == ENUM_BLACKJACK_TABLE_STATE.PlayerTime)
                {
                    ButtonSprite_Control.BetInsure_Bool = false;
                    ButtonSprite_Control.BJ21_Bool = false;
                    ButtonSprite_Control.Scoreboard_Bool = false;
                    BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState = ENUM_BLACKJACK_TABLE_STATE.CheckPlayer;
                }
            }

            if ((BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.NewRound || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.WaitBet)
                && Data.m_enumState == ENUM_BLACKJACK_TABLE_STATE.NewRound)
            {
                Cash_Control.TableCash[0] = 0;
                StateShow_Control.PleaseBetStateStart = true;
                BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState = ENUM_BLACKJACK_TABLE_STATE.WaitBet;
                BJMainGame_Control.NowStateSave = ENUM_BLACKJACK_TABLE_STATE.WaitBet;
            }
        }

        BJMainGame_Control.BetTimerMax = Data.m_uiWaitMSec / 1000;
        BJMainGame_Control.BetTimerMinus = 0;
        Debug.Log("更新檯桌狀態: " + Data.m_enumState + " //時間更新: " + BJMainGame_Control.BetTimerMax);
    }

    //玩家動作結果
    public void ByuInsureResult(byte[] byarData)
    {
        CPACK_BlackJack_RePlayerDo Data = GameConnet.m_oGameClient.DoDeSerialize<CPACK_BlackJack_RePlayerDo>(byarData);

        if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState != ENUM_BLACKJACK_TABLE_STATE.Idle)
        {
            if (Data.m_enumResult == ENUM_COMMON_ERROR_CODE.Success)
            {
                //買保險
                #region Insurance
                if (Data.m_enumPlayerDoResult == ENUM_BLACKJACK_PLAYERDO.INSURE)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (Data.m_uiBetPlayerDBID == Cash_Control.PlayerDBID[i])
                        {
                            Cash_Control.BuyInsure[i] = 1;

                            if (i == 0)
                            {
                                Cash_Control.TableCash[i * 2] = (int)((float)Cash_Control.TableCash[i * 2] * 1.5f);
                            }
                            else if (i == 1)
                            {
                                Cash_Control.TableCash[i * 2] = (int)((float)Cash_Control.TableCash[i * 2] * 1.5f);
                            }
                            else if (i == 2)
                            {
                                Cash_Control.TableCash[i * 2] = (int)((float)Cash_Control.TableCash[i * 2] * 1.5f);
                            }
                            else if (i == 3)
                            {
                                Cash_Control.TableCash[i * 2] = (int)((float)Cash_Control.TableCash[i * 2] * 1.5f);
                            }
                            else if (i == 4)
                            {
                                Cash_Control.TableCash[i * 2] = (int)((float)Cash_Control.TableCash[i * 2] * 1.5f);
                            }
                        }
                    }

                    if (Data.m_uiBetPlayerDBID == Cash_Control.PlayerDBID[(byte)TableList.MyTable])
                    {
                        BJMainGame_Control.NowStateSave = ENUM_BLACKJACK_TABLE_STATE.InsuranceOver;
                        BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState = ENUM_BLACKJACK_TABLE_STATE.InsuranceOver;
                        ButtonSprite_Control.BetInsure_Bool = false;
                        Cash_Control.OnBetClick = false;
                    }
                    Debug.Log("保險成功");
                }
                #endregion

                //報到
                #region BLACKJACK
                if (Data.m_enumPlayerDoResult == ENUM_BLACKJACK_PLAYERDO.BLACKJACK)
                {
                    if (Data.m_uiBetPlayerDBID == Cash_Control.PlayerDBID[(byte)TableList.MyTable])
                    {
                        BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState = ENUM_BLACKJACK_TABLE_STATE.PlayerOver;
                        BJMainGame_Control.TableWinLose[CardTeamList.Card1Team1] = WinLoseList.BlackJack;
                        BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.CardBanker] = 1;
                        Cash_Control.PlayerRound[(byte)TableList.MyTable] = 0;
                        BJMainGame_Control.MainWinLoseShow = true;
                        ButtonSprite_Control.BJ21_Bool = false;
                        Cash_Control.OnBetClick = false;
                        BJMainGame_Control.BlackJack_Bool[0] = 1;
                        Debug.Log("報到成功");
                    }
                    else
                    {
                        for (int i = 1; i < 5; i++)
                        {
                            if (Data.m_uiBetPlayerDBID == Cash_Control.PlayerDBID[i])
                            {
                                if (i == 1)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card2Team1] = WinLoseList.BlackJack;
                                    BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card2Team1] = 1;
                                    BJMainGame_Control.BlackJack_Bool[i] = 1;
                                }
                                else if (i == 2)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card3Team1] = WinLoseList.BlackJack;
                                    BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card3Team1] = 1;
                                    BJMainGame_Control.BlackJack_Bool[i] = 1;
                                }
                                else if (i == 3)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card4Team1] = WinLoseList.BlackJack;
                                    BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card4Team1] = 1;
                                    BJMainGame_Control.BlackJack_Bool[i] = 1;
                                }
                                else if (i == 4)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card5Team1] = WinLoseList.BlackJack;
                                    BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card5Team1] = 1;
                                    BJMainGame_Control.BlackJack_Bool[i] = 1;
                                }
                            }
                        }
                        Debug.Log("其他玩家報到成功: " + Data.m_uiBetPlayerDBID);
                    }
                }
                #endregion

                //停牌
                #region Pass
                if (Data.m_enumPlayerDoResult == ENUM_BLACKJACK_PLAYERDO.STAND)
                {
                    if (Data.m_uiBetPlayerDBID == Cash_Control.PlayerDBID[(byte)TableList.MyTable] && !BJMainGame_Control.CardSenceList_Bool)
                    {
                        Cash_Control.OnBetClick = false;
                        BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState = ENUM_BLACKJACK_TABLE_STATE.PlayerOver;
                        Cash_Control.PlayerRound[(byte)TableList.MyTable] = 0;
                    }
                    else
                    {
                        Cash_Control.OnBetClick = false;
                        BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState = ENUM_BLACKJACK_TABLE_STATE.CheckPlayer;
                        BJMainGame_Control.WaitAngle = true;
                        BJMainGame_Control.CardSenceList_Bool = false;
                    }
                    Debug.Log("停牌成功");
                }
                #endregion

                //分牌 
                #region SPLIT
                if (Data.m_enumPlayerDoResult == ENUM_BLACKJACK_PLAYERDO.SPLIT)
                {
                    for (byte i = 0; i < 5; i++)
                    {
                        if (Cash_Control.PlayerDBID[i] == Data.m_uiBetPlayerDBID && i == 0)
                        {
                            Cash_Control.PlayerRoundPlicture_Number[(byte)TableList.MyTable] = 2;
                            BJCard_Control.Seat1Team2.Add(BJCard_Control.Seat1Team1[1]);
                            BJCard_Control.Seat1Team1.RemoveAt(1);

                            BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card1Team1] = 1;
                            BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card1Team2] = 1;

                            byte CardNumber = Data.m_byMainCard;
                            byte CardNumber2 = Data.m_byOtherCard;
                            BJCard_Control.AddCard.Add(CardTeamList.Card1Team1, CardNumber);
                            BJCard_Control.AddCard.Add(CardTeamList.Card1Team2, CardNumber2);
                            Cash_Control.TableCash[1] = Cash_Control.TableCash[0];
                            BJMainGame_Control.CardSenceList_Bool = true;
                            ButtonSprite_Control.Scoreboard_Bool = false;
                            i = 5;
                        }
                        else
                        {
                            if (Cash_Control.PlayerDBID[i] == Data.m_uiBetPlayerDBID && i == 1)
                            {
                                Cash_Control.PlayerRoundPlicture_Number[(byte)TableList.PlayerTable1] = 2;
                                BJCard_Control.Seat2Team2.Add(BJCard_Control.Seat2Team1[1]);
                                BJCard_Control.Seat2Team1.RemoveAt(1);

                                BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card2Team1] = 1;
                                BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card2Team2] = 1;

                                byte CardNumber = Data.m_byMainCard;
                                byte CardNumber2 = Data.m_byOtherCard;
                                BJCard_Control.AddCard.Add(CardTeamList.Card2Team1, CardNumber);
                                BJCard_Control.AddCard.Add(CardTeamList.Card2Team2, CardNumber2);
                                Cash_Control.TableCash[3] = Cash_Control.TableCash[2];
                                BJMainGame_Control.CardSenceList_Bool = true;
                                i = 5;
                            }
                            else if (Cash_Control.PlayerDBID[i] == Data.m_uiBetPlayerDBID && i == 2)
                            {
                                Cash_Control.PlayerRoundPlicture_Number[(byte)TableList.PlayerTable2] = 2;
                                BJCard_Control.Seat3Team2.Add(BJCard_Control.Seat3Team1[1]);
                                BJCard_Control.Seat3Team1.RemoveAt(1);

                                BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card3Team1] = 1;
                                BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card3Team2] = 1;

                                byte CardNumber = Data.m_byMainCard;
                                byte CardNumber2 = Data.m_byOtherCard;
                                BJCard_Control.AddCard.Add(CardTeamList.Card3Team1, CardNumber);
                                BJCard_Control.AddCard.Add(CardTeamList.Card3Team2, CardNumber2);
                                Cash_Control.TableCash[5] = Cash_Control.TableCash[4];
                                BJMainGame_Control.CardSenceList_Bool = true;
                                i = 5;
                            }
                            else if (Cash_Control.PlayerDBID[i] == Data.m_uiBetPlayerDBID && i == 3)
                            {
                                Cash_Control.PlayerRoundPlicture_Number[(byte)TableList.PlayerTable3] = 2;
                                BJCard_Control.Seat4Team2.Add(BJCard_Control.Seat4Team1[1]);
                                BJCard_Control.Seat4Team1.RemoveAt(1);

                                BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card4Team1] = 1;
                                BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card4Team2] = 1;

                                byte CardNumber = Data.m_byMainCard;
                                byte CardNumber2 = Data.m_byOtherCard;
                                BJCard_Control.AddCard.Add(CardTeamList.Card4Team1, CardNumber);
                                BJCard_Control.AddCard.Add(CardTeamList.Card4Team2, CardNumber2);
                                Cash_Control.TableCash[7] = Cash_Control.TableCash[6];
                                BJMainGame_Control.CardSenceList_Bool = true;
                                i = 5;
                            }
                            else if (Cash_Control.PlayerDBID[i] == Data.m_uiBetPlayerDBID && i == 4)
                            {
                                Cash_Control.PlayerRoundPlicture_Number[(byte)TableList.PlayerTable4] = 2;
                                BJCard_Control.Seat5Team2.Add(BJCard_Control.Seat5Team1[1]);
                                BJCard_Control.Seat5Team1.RemoveAt(1);

                                BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card5Team1] = 1;
                                BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card5Team2] = 1;

                                byte CardNumber = Data.m_byMainCard;
                                byte CardNumber2 = Data.m_byOtherCard;
                                BJCard_Control.AddCard.Add(CardTeamList.Card5Team1, CardNumber);
                                BJCard_Control.AddCard.Add(CardTeamList.Card5Team2, CardNumber2);
                                Cash_Control.TableCash[9] = Cash_Control.TableCash[8];
                                BJMainGame_Control.CardSenceList_Bool = true;
                                i = 5;
                            }
                        }
                    }
                    Debug.Log("分牌成功");
                }
                #endregion

                //要牌
                #region Hit
                if (Data.m_enumPlayerDoResult == ENUM_BLACKJACK_PLAYERDO.HIT)
                {
                    for (byte i = 0; i < 5; i++)
                    {
                        if (Cash_Control.PlayerDBID[i] == Data.m_uiBetPlayerDBID && Data.m_byMainCard != 0)
                        {
                            DealerCard_Move.CardNumber = Data.m_byMainCard;
                            if (i == 0)
                            {
                                BJCard_Control.Seat1Team1.Add(Data.m_byMainCard);
                            }
                            else if (i == 1)
                            {
                                BJCard_Control.Seat2Team1.Add(Data.m_byMainCard);
                            }
                            else if (i == 2)
                            {
                                BJCard_Control.Seat3Team1.Add(Data.m_byMainCard);
                            }
                            else if (i == 3)
                            {
                                BJCard_Control.Seat4Team1.Add(Data.m_byMainCard);
                            }
                            else if (i == 4)
                            {
                                BJCard_Control.Seat5Team1.Add(Data.m_byMainCard);
                            }
                            BJCard_Control.SeatTeamPoint[i * 2]++;
                            BJCard_Control.SeatTeamAddCheck[i * 2] = 1;
                            BJCard_Control.NowFinallCardSeat[i * 2] = 1;
                        }
                        else if (Cash_Control.PlayerDBID[i] == Data.m_uiBetPlayerDBID && Data.m_byMainCard == 0)
                        {
                            DealerCard_Move.CardNumber = Data.m_byOtherCard;
                            if (i == 0)
                            {
                                BJCard_Control.Seat1Team2.Add(Data.m_byOtherCard);
                            }
                            else if (i == 1)
                            {
                                BJCard_Control.Seat2Team2.Add(Data.m_byOtherCard);
                            }
                            else if (i == 2)
                            {
                                BJCard_Control.Seat3Team2.Add(Data.m_byOtherCard);
                            }
                            else if (i == 3)
                            {
                                BJCard_Control.Seat4Team2.Add(Data.m_byOtherCard);
                            }
                            else if (i == 4)
                            {
                                BJCard_Control.Seat5Team2.Add(Data.m_byOtherCard);
                            }
                            BJCard_Control.SeatTeamPoint[i * 2 + 1]++;
                            BJCard_Control.SeatTeamAddCheck[i * 2 + 1] = 1;
                            BJCard_Control.NowFinallCardSeat[i * 2 + 1] = 1;
                        }
                    }
                    Debug.Log("要牌成功");
                }
                #endregion

                //投降
                #region SURRENDER
                if (Data.m_enumPlayerDoResult == ENUM_BLACKJACK_PLAYERDO.SURRENDER)
                {
                    if (Data.m_uiBetPlayerDBID == Cash_Control.PlayerDBID[(byte)TableList.MyTable])
                    {
                        BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState = ENUM_BLACKJACK_TABLE_STATE.PlayerOver;
                        BJMainGame_Control.TableWinLose[CardTeamList.Card1Team1] = WinLoseList.WinBanker;
                        BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.CardBanker] = 1;
                        Cash_Control.PlayerRound[(byte)TableList.MyTable] = 0;
                        BJMainGame_Control.MainWinLoseShow = true;
                        Cash_Control.OnBetClick = false;
                    }
                    else
                    {
                        for (int i = 1; i < 5; i++)
                        {
                            if (Data.m_uiBetPlayerDBID == Cash_Control.PlayerDBID[i])
                            {
                                if (i == 1)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card2Team1] = WinLoseList.WinBanker;
                                    BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card2Team1] = 1;
                                }
                                else if (i == 2)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card3Team1] = WinLoseList.WinBanker;
                                    BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card3Team1] = 1;
                                }
                                else if (i == 3)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card4Team1] = WinLoseList.WinBanker;
                                    BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card4Team1] = 1;
                                }
                                else if (i == 4)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card5Team1] = WinLoseList.WinBanker;
                                    BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card5Team1] = 1;
                                }
                            }
                        }
                    }
                    Debug.Log("投降成功");
                }
                #endregion

                //加倍
                #region DOUBLE
                if (Data.m_enumPlayerDoResult == ENUM_BLACKJACK_PLAYERDO.DOUBLE)
                {
                    for (byte i = 0; i < 5; i++)
                    {
                        if (Cash_Control.PlayerDBID[i] == Data.m_uiBetPlayerDBID && Data.m_byMainCard != 0)
                        {
                            DealerCard_Move.CardNumber = Data.m_byMainCard;
                            if (i == 0)
                            {
                                BJCard_Control.Seat1Team1.Add(Data.m_byMainCard);
                                Cash_Control.TableCash[i * 2] = Cash_Control.TableCash[i * 2] * 2;
                            }
                            else if (i == 1)
                            {
                                BJCard_Control.Seat2Team1.Add(Data.m_byMainCard);
                                Cash_Control.TableCash[i * 2] = Cash_Control.TableCash[i * 2] * 2;
                            }
                            else if (i == 2)
                            {
                                BJCard_Control.Seat3Team1.Add(Data.m_byMainCard);
                                Cash_Control.TableCash[i * 2] = Cash_Control.TableCash[i * 2] * 2;
                            }
                            else if (i == 3)
                            {
                                BJCard_Control.Seat4Team1.Add(Data.m_byMainCard);
                                Cash_Control.TableCash[i * 2] = Cash_Control.TableCash[i * 2] * 2;
                            }
                            else if (i == 4)
                            {
                                BJCard_Control.Seat5Team1.Add(Data.m_byMainCard);
                                Cash_Control.TableCash[i * 2] = Cash_Control.TableCash[i * 2] * 2;
                            }
                            BJCard_Control.SeatTeamPoint[i * 2]++;
                            BJCard_Control.SeatTeamAddCheck[i * 2] = 1;
                            BJCard_Control.NowFinallCardSeat[i * 2] = 1;
                        }
                    }
                    if (Data.m_uiBetPlayerDBID == Cash_Control.PlayerDBID[(byte)TableList.MyTable])
                    {
                        Cash_Control.OnBetClick = false;
                        BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState = ENUM_BLACKJACK_TABLE_STATE.PlayerOver;
                        Cash_Control.PlayerRound[(byte)TableList.MyTable] = 0;
                    }
                }
                #endregion

                if (Data.m_uiBetPlayerDBID == Cash_Control.PlayerDBID[(byte)TableList.MyTable])
                {
                    ButtonSprite_Control.BJ21_Bool = false;
                    ButtonSprite_Control.Scoreboard_Bool = false;
                    ButtonSprite_Control.BetInsure_Bool = false;
                }
            }
            else
            {
                if (Data.m_uiBetPlayerDBID == Cash_Control.PlayerDBID[(byte)TableList.MyTable])
                {
                    #region Error
                    //保險
                    if (Data.m_enumPlayerDoResult == ENUM_BLACKJACK_PLAYERDO.INSURE)
                    {
                        BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState = ENUM_BLACKJACK_TABLE_STATE.InsuranceOver;
                        ButtonSprite_Control.BetInsure_Bool = false;
                        Debug.Log("保險失敗: " + Data.m_enumResult);
                    }

                    //各種錯誤代碼
                    //金額不足
                    if (Data.m_enumResult == ENUM_COMMON_ERROR_CODE.BlackJack_MoneyNotEnough)
                    {
                        Message_Control.OpenMessage = true;
                        Message_Control.MessageStatus = Message_Control.MessageStatu.NoBetMoney;
                        Debug.Log("ERROR");
                    }
                    //超過押注最大上限
                    if (Data.m_enumResult == ENUM_COMMON_ERROR_CODE.BlackJack_OverBetLimit)
                    {
                        Message_Control.OpenMessage = true;
                        Message_Control.MessageStatus = Message_Control.MessageStatu.MaxBetOut;
                    }
                    if (Data.m_enumResult == ENUM_COMMON_ERROR_CODE.BlackJack_StopBet)
                    {
                        Message_Control.OpenMessage = true;
                        Message_Control.MessageStatus = Message_Control.MessageStatu.StopMode;
                        Cash_Control.TableCash[0] = 0;
                        BJMainGame_Control.EnterBetBool = false;
                    }
                    Cash_Control.OnBetClick = false;
                    #endregion
                }
            }
        }
        Debug.Log("玩家動作結果" + "  //結果代碼: " + Data.m_enumResult + "  //事件代碼: " + Data.m_enumPlayerDoResult + "  //桌子ID: " + Data.m_uiBetPlayerDBID + "  //桌子ID: " + Cash_Control.PlayerDBID[(byte)TableList.MyTable]);
    }

    //收到押注結果
    public void GetBetResult(byte[] byarData)
    {
        CPACK_BlackJack_NotifyBet Data = GameConnet.m_oGameClient.DoDeSerialize<CPACK_BlackJack_NotifyBet>(byarData);
        if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState != ENUM_BLACKJACK_TABLE_STATE.Idle)
        {
            if (Data.m_enumResult == ENUM_COMMON_ERROR_CODE.Success)
            {
                for (int i = 1; i < 5; i++)
                {
                    if (Data.m_uiBetPlayerDBID == Cash_Control.PlayerDBID[i])
                    {
                        Cash_Control.TableCash[i * 2] += Data.m_uiPlayerBetMoney;
                    }

                }
                if (Data.m_uiBetPlayerDBID == Cash_Control.PlayerDBID[(byte)TableList.MyTable])
                {
                    BJMainGame_Control.EnterBetBool = true;
                    Cash_Control.OnBetClick = false;
                }
            }
            else
            {
                //錯誤處理
                if (Data.m_uiBetPlayerDBID == Cash_Control.PlayerDBID[(byte)TableList.MyTable])
                {
                    //金額不足
                    if (Data.m_enumResult == ENUM_COMMON_ERROR_CODE.BlackJack_MoneyNotEnough)
                    {
                        Message_Control.OpenMessage = true;
                        Message_Control.MessageStatus = Message_Control.MessageStatu.NoBetMoney;
                    }
                    //超過押注最大上限
                    if (Data.m_enumResult == ENUM_COMMON_ERROR_CODE.BlackJack_OverBetLimit)
                    {
                        Message_Control.OpenMessage = true;
                        Message_Control.MessageStatus = Message_Control.MessageStatu.MaxBetOut;
                    }
                    Cash_Control.TableCash[0] = 0;
                    BJMainGame_Control.EnterBetBool = false;
                    Cash_Control.OnBetClick = false;
                }
            }


        }
        Debug.Log("收到押注結果: " + Data.m_enumResult + " //押注金額: " + Data.m_uiPlayerBetMoney);
    }

    //玩家最初牌面結果
    public void FirstCardListResult(byte[] byarData)
    {

        CPACK_BlackJack_NotifyDealData Data = GameConnet.m_oGameClient.DoDeSerialize<CPACK_BlackJack_NotifyDealData>(byarData);

        if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState != ENUM_BLACKJACK_TABLE_STATE.Idle)
        {
            for (byte i = 1; i < 6; i++)
            {
                if (Data.m_dicPlayerCard.ContainsKey(i))
                {
                    BJMainGame_Control.FirstCardListSave.Add(Data.m_dicPlayerCard[i].m_uiDBID, Data.m_dicPlayerCard[i].m_byarCards);
                }
            }

            byte[] ByteData = new byte[2];
            ByteData[0] = Data.m_byBankerFirstCard;
            ByteData[1] = 0;
            BJMainGame_Control.FirstCardListSave.Add(6, ByteData);
            BJMainGame_Control.FirstCardDataGet_Bool = true;
        }

        foreach (var item in Data.m_dicPlayerCard)
        {
            Debug.Log("DBID: " + item.Key + " //DBID: " + item.Value.m_uiDBID + " //Card1: " + item.Value.m_byarCards[0] + " //Card2: " + item.Value.m_byarCards[1]);
        }
        Debug.Log("DBID: " + 6 + " //Card1: " + Data.m_byBankerFirstCard + " //Card2: " + 0);
        Debug.Log("收到玩家最初牌面結果");
    }

    //現在可行動玩家
    public void NowPlayingPlayer(byte[] byarData)
    {
        CNowCanDoTarget Data = GameConnet.m_oGameClient.DoDeSerialize<CNowCanDoTarget>(byarData);

        if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState != ENUM_BLACKJACK_TABLE_STATE.Idle)
        {
            for (int i = 0; i < 5; i++)
            {
                if (Data.m_uiPlayerDBID == Cash_Control.PlayerDBID[i])
                    Cash_Control.PlayerRound[i] = 1;
                else
                    Cash_Control.PlayerRound[i] = 0;

                if (Data.m_uiPlayerDBID == Cash_Control.PlayerDBID[i])
                {
                    if (Cash_Control.PlayerRoundPlicture_Number[i] != 0)
                        Cash_Control.PlayerRoundPlicture_Number[i] -= 1;
                }
            }
        }
        Debug.Log("現在可行動玩家: " + Data.m_uiPlayerDBID);
    }

    //收到當局結果
    public void FinallResult(byte[] byarData)
    {
        CPACK_BlackJack_NotifyAward Data = GameConnet.m_oGameClient.DoDeSerialize<CPACK_BlackJack_NotifyAward>(byarData);

        Debug.Log("結果 // 莊家點數和:" + Data.m_oBetAward.m_BankerCards.m_bySam + " //總贏金: " + Data.m_ui64GameMoney);
        foreach (var item in Data.m_oBetAward.m_dicPlayerCardAward)
        {
            Debug.Log("結果 DBID :" + item.Value.m_uiDBID + " //AWARD: " + item.Value.m_byMainCardList.m_ENAwardStatus + " // 第一組點數和: " + item.Value.m_byMainCardList.m_ENAwardStatus + " // 第二組點數和: " + item.Value.m_byOtherCardList.m_ENAwardStatus);
        }

        if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState != ENUM_BLACKJACK_TABLE_STATE.Idle)
        {
            byte[] SaveLiat = new byte[Data.m_oBetAward.m_BankerCards.m_byMainCardList.Count];
            for (byte i = 0; i < Data.m_oBetAward.m_BankerCards.m_byMainCardList.Count; i++)
            {
                SaveLiat[i] = Data.m_oBetAward.m_BankerCards.m_byMainCardList[i];
                Debug.Log("CardID: " + SaveLiat[i]);
            }
            BJMainGame_Control.FinallCardListSave.Add(0, SaveLiat);

            foreach (var item in Data.m_oBetAward.m_dicPlayerCardAward)
            {
                FinallData FinallData2 = new FinallData();
                for (byte i = 0; i < 5; i++)
                {
                    if (item.Value.m_uiDBID == Cash_Control.PlayerDBID[i])
                    {
                        #region FirstCardListAward
                        if (i == 0)
                        {
                            if (BJMainGame_Control.TableWinLose[CardTeamList.Card1Team1] == WinLoseList.PointOut)
                            {
                                FinallData2.Card1Award = WinLoseList.PointOut;
                            }
                            else
                            {
                                if (item.Value.m_byMainCardList.m_ENCardStatus == ENUM_BLACKJACK_CardStatus.BlackJack && BJMainGame_Control.BlackJack_Bool[i] == 1)
                                {
                                    FinallData2.Card1Award = WinLoseList.BlackJack;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinBanker)
                                {
                                    FinallData2.Card1Award = WinLoseList.WinBanker;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinPlayer)
                                {
                                    FinallData2.Card1Award = WinLoseList.WinPlayer;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinDraw)
                                {
                                    FinallData2.Card1Award = WinLoseList.WinDraw;
                                }
                            }
                        }
                        else if (i == 1)
                        {
                            if (BJMainGame_Control.TableWinLose[CardTeamList.Card2Team1] == WinLoseList.PointOut)
                            {
                                FinallData2.Card1Award = WinLoseList.PointOut;
                            }
                            else
                            {
                                if (item.Value.m_byMainCardList.m_ENCardStatus == ENUM_BLACKJACK_CardStatus.BlackJack && BJMainGame_Control.BlackJack_Bool[i] == 1)
                                {
                                    FinallData2.Card1Award = WinLoseList.BlackJack;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinBanker)
                                {
                                    FinallData2.Card1Award = WinLoseList.WinBanker;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinPlayer)
                                {
                                    FinallData2.Card1Award = WinLoseList.WinPlayer;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinDraw)
                                {
                                    FinallData2.Card1Award = WinLoseList.WinDraw;
                                }
                            }
                        }
                        else if (i == 2)
                        {
                            if (BJMainGame_Control.TableWinLose[CardTeamList.Card3Team1] == WinLoseList.PointOut)
                            {
                                FinallData2.Card1Award = WinLoseList.PointOut;
                            }
                            else
                            {
                                if (item.Value.m_byMainCardList.m_ENCardStatus == ENUM_BLACKJACK_CardStatus.BlackJack && BJMainGame_Control.BlackJack_Bool[i] == 1)
                                {
                                    FinallData2.Card1Award = WinLoseList.BlackJack;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinBanker)
                                {
                                    FinallData2.Card1Award = WinLoseList.WinBanker;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinPlayer)
                                {
                                    FinallData2.Card1Award = WinLoseList.WinPlayer;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinDraw)
                                {
                                    FinallData2.Card1Award = WinLoseList.WinDraw;
                                }
                            }
                        }
                        else if (i == 3)
                        {
                            if (BJMainGame_Control.TableWinLose[CardTeamList.Card4Team1] == WinLoseList.PointOut)
                            {
                                FinallData2.Card1Award = WinLoseList.PointOut;
                            }
                            else
                            {
                                if (item.Value.m_byMainCardList.m_ENCardStatus == ENUM_BLACKJACK_CardStatus.BlackJack && BJMainGame_Control.BlackJack_Bool[i] == 1)
                                {
                                    FinallData2.Card1Award = WinLoseList.BlackJack;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinBanker)
                                {
                                    FinallData2.Card1Award = WinLoseList.WinBanker;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinPlayer)
                                {
                                    FinallData2.Card1Award = WinLoseList.WinPlayer;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinDraw)
                                {
                                    FinallData2.Card1Award = WinLoseList.WinDraw;
                                }
                            }
                        }
                        else if (i == 4)
                        {
                            if (BJMainGame_Control.TableWinLose[CardTeamList.Card5Team1] == WinLoseList.PointOut)
                            {
                                FinallData2.Card1Award = WinLoseList.PointOut;
                            }
                            else
                            {
                                if (item.Value.m_byMainCardList.m_ENCardStatus == ENUM_BLACKJACK_CardStatus.BlackJack && BJMainGame_Control.BlackJack_Bool[i] == 1)
                                {
                                    FinallData2.Card1Award = WinLoseList.BlackJack;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinBanker)
                                {
                                    FinallData2.Card1Award = WinLoseList.WinBanker;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinPlayer)
                                {
                                    FinallData2.Card1Award = WinLoseList.WinPlayer;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinDraw)
                                {
                                    FinallData2.Card1Award = WinLoseList.WinDraw;
                                }
                            }
                        }
                        #endregion

                        #region SenceCardListAward
                        if (item.Value.m_byOtherCardList.m_ENAwardStatus != ENUM_BLACKJACK_AWARD.Idle)
                        {
                            if (i == 0)
                            {
                                if (BJMainGame_Control.TableWinLose[CardTeamList.Card1Team2] == WinLoseList.PointOut)
                                {
                                    FinallData2.Card2Award = WinLoseList.PointOut;
                                }
                                else
                                {
                                    if (item.Value.m_byOtherCardList.m_ENCardStatus == ENUM_BLACKJACK_CardStatus.BlackJack && BJMainGame_Control.BlackJack_Bool[i] == 1)
                                    {
                                        FinallData2.Card2Award = WinLoseList.BlackJack;
                                    }
                                    else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinBanker)
                                    {
                                        FinallData2.Card2Award = WinLoseList.WinBanker;
                                    }
                                    else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinPlayer)
                                    {
                                        FinallData2.Card2Award = WinLoseList.WinPlayer;
                                    }
                                    else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinDraw)
                                    {
                                        FinallData2.Card2Award = WinLoseList.WinDraw;
                                    }
                                }
                            }
                            else if (i == 1)
                            {
                                if (BJMainGame_Control.TableWinLose[CardTeamList.Card2Team2] == WinLoseList.PointOut)
                                {
                                    FinallData2.Card2Award = WinLoseList.PointOut;
                                }
                                else
                                {
                                    if (item.Value.m_byOtherCardList.m_ENCardStatus == ENUM_BLACKJACK_CardStatus.BlackJack && BJMainGame_Control.BlackJack_Bool[i] == 1)
                                    {
                                        FinallData2.Card2Award = WinLoseList.BlackJack;
                                    }
                                    else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinBanker)
                                    {
                                        FinallData2.Card2Award = WinLoseList.WinBanker;
                                    }
                                    else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinPlayer)
                                    {
                                        FinallData2.Card2Award = WinLoseList.WinPlayer;
                                    }
                                    else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinDraw)
                                    {
                                        FinallData2.Card2Award = WinLoseList.WinDraw;
                                    }
                                }
                            }
                            else if (i == 2)
                            {
                                if (BJMainGame_Control.TableWinLose[CardTeamList.Card3Team2] == WinLoseList.PointOut)
                                {
                                    FinallData2.Card2Award = WinLoseList.PointOut;
                                }
                                else
                                {
                                    if (item.Value.m_byOtherCardList.m_ENCardStatus == ENUM_BLACKJACK_CardStatus.BlackJack && BJMainGame_Control.BlackJack_Bool[i] == 1)
                                    {
                                        FinallData2.Card2Award = WinLoseList.BlackJack;
                                    }
                                    else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinBanker)
                                    {
                                        FinallData2.Card2Award = WinLoseList.WinBanker;
                                    }
                                    else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinPlayer)
                                    {
                                        FinallData2.Card2Award = WinLoseList.WinPlayer;
                                    }
                                    else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinDraw)
                                    {
                                        FinallData2.Card2Award = WinLoseList.WinDraw;
                                    }
                                }
                            }
                            else if (i == 3)
                            {
                                if (BJMainGame_Control.TableWinLose[CardTeamList.Card4Team2] == WinLoseList.PointOut)
                                {
                                    FinallData2.Card2Award = WinLoseList.PointOut;
                                }
                                else
                                {
                                    if (item.Value.m_byOtherCardList.m_ENCardStatus == ENUM_BLACKJACK_CardStatus.BlackJack && BJMainGame_Control.BlackJack_Bool[i] == 1)
                                    {
                                        FinallData2.Card2Award = WinLoseList.BlackJack;
                                    }
                                    else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinBanker)
                                    {
                                        FinallData2.Card2Award = WinLoseList.WinBanker;
                                    }
                                    else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinPlayer)
                                    {
                                        FinallData2.Card2Award = WinLoseList.WinPlayer;
                                    }
                                    else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinDraw)
                                    {
                                        FinallData2.Card2Award = WinLoseList.WinDraw;
                                    }
                                }
                            }
                            else if (i == 4)
                            {
                                if (BJMainGame_Control.TableWinLose[CardTeamList.Card5Team2] == WinLoseList.PointOut)
                                {
                                    FinallData2.Card2Award = WinLoseList.PointOut;
                                }
                                else
                                {
                                    if (item.Value.m_byOtherCardList.m_ENCardStatus == ENUM_BLACKJACK_CardStatus.BlackJack && BJMainGame_Control.BlackJack_Bool[i] == 1)
                                    {
                                        FinallData2.Card2Award = WinLoseList.BlackJack;
                                    }
                                    else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinBanker)
                                    {
                                        FinallData2.Card2Award = WinLoseList.WinBanker;
                                    }
                                    else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinPlayer)
                                    {
                                        FinallData2.Card2Award = WinLoseList.WinPlayer;
                                    }
                                    else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinDraw)
                                    {
                                        FinallData2.Card2Award = WinLoseList.WinDraw;
                                    }
                                }
                            }
                        }
                        #endregion

                        FinallData2.PlayerDBID = item.Value.m_uiDBID;
                        BJEndWindow_Control.m_BetAward.Add(i, FinallData2);

                        #region WinLoseShow
                        if (i == 0)
                        {
                            #region MyFinallData
                            if (BJMainGame_Control.TableWinLose[CardTeamList.Card1Team1] == WinLoseList.NoCheck)
                            {
                                if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.BlackJack)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card1Team1] = WinLoseList.BlackJack;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinBanker)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card1Team1] = WinLoseList.WinBanker;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinPlayer)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card1Team1] = WinLoseList.WinPlayer;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinDraw)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card1Team1] = WinLoseList.WinDraw;
                                }
                                BJEndWindow_Control.WinLoseShowSave[(byte)CardTeamList.Card1Team1] = 1;
                            }

                            if (BJMainGame_Control.TableWinLose[CardTeamList.Card1Team2] == WinLoseList.NoCheck && item.Value.m_byOtherCardList.m_ENAwardStatus != ENUM_BLACKJACK_AWARD.Idle)
                            {
                                if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.BlackJack)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card1Team2] = WinLoseList.BlackJack;
                                }
                                else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinBanker)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card1Team2] = WinLoseList.WinBanker;
                                }
                                else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinPlayer)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card1Team2] = WinLoseList.WinPlayer;
                                }
                                else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinDraw)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card1Team2] = WinLoseList.WinDraw;
                                }
                                BJEndWindow_Control.WinLoseShowSave[(byte)CardTeamList.Card1Team2] = 1;
                            }
                            #endregion
                        }
                        else if (i == 1)
                        {
                            #region Player1FinallData
                            if (BJMainGame_Control.TableWinLose[CardTeamList.Card2Team1] == WinLoseList.NoCheck)
                            {
                                if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.BlackJack)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card2Team1] = WinLoseList.BlackJack;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinBanker)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card2Team1] = WinLoseList.WinBanker;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinPlayer)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card2Team1] = WinLoseList.WinPlayer;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinDraw)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card2Team1] = WinLoseList.WinDraw;
                                }
                                BJEndWindow_Control.WinLoseShowSave[(byte)CardTeamList.Card2Team1] = 1;
                            }
                            if (BJMainGame_Control.TableWinLose[CardTeamList.Card2Team2] == WinLoseList.NoCheck && item.Value.m_byOtherCardList.m_ENAwardStatus != ENUM_BLACKJACK_AWARD.Idle)
                            {
                                if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.BlackJack)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card2Team2] = WinLoseList.BlackJack;
                                }
                                else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinBanker)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card2Team2] = WinLoseList.WinBanker;
                                }
                                else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinPlayer)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card2Team2] = WinLoseList.WinPlayer;
                                }
                                else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinDraw)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card2Team2] = WinLoseList.WinDraw;
                                }
                                BJEndWindow_Control.WinLoseShowSave[(byte)CardTeamList.Card2Team2] = 1;
                            }
                            #endregion
                        }
                        else if (i == 2)
                        {
                            #region Player2FinallData
                            if (BJMainGame_Control.TableWinLose[CardTeamList.Card3Team1] == WinLoseList.NoCheck)
                            {
                                if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.BlackJack)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card3Team1] = WinLoseList.BlackJack;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinBanker)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card3Team1] = WinLoseList.WinBanker;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinPlayer)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card3Team1] = WinLoseList.WinPlayer;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinDraw)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card3Team1] = WinLoseList.WinDraw;
                                }
                                BJEndWindow_Control.WinLoseShowSave[(byte)CardTeamList.Card3Team1] = 1;
                            }
                            if (BJMainGame_Control.TableWinLose[CardTeamList.Card3Team2] == WinLoseList.NoCheck && item.Value.m_byOtherCardList.m_ENAwardStatus != ENUM_BLACKJACK_AWARD.Idle)
                            {
                                if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.BlackJack)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card3Team2] = WinLoseList.BlackJack;
                                }
                                else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinBanker)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card3Team2] = WinLoseList.WinBanker;
                                }
                                else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinPlayer)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card3Team2] = WinLoseList.WinPlayer;
                                }
                                else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinDraw)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card3Team2] = WinLoseList.WinDraw;
                                }
                                BJEndWindow_Control.WinLoseShowSave[(byte)CardTeamList.Card3Team2] = 1;
                            }
                            #endregion
                        }
                        else if (i == 3)
                        {
                            #region Player3FinallData
                            if (BJMainGame_Control.TableWinLose[CardTeamList.Card4Team1] == WinLoseList.NoCheck)
                            {
                                if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.BlackJack)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card4Team1] = WinLoseList.BlackJack;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinBanker)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card4Team1] = WinLoseList.WinBanker;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinPlayer)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card4Team1] = WinLoseList.WinPlayer;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinDraw)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card4Team1] = WinLoseList.WinDraw;
                                }
                                BJEndWindow_Control.WinLoseShowSave[(byte)CardTeamList.Card4Team1] = 1;
                            }
                            if (BJMainGame_Control.TableWinLose[CardTeamList.Card4Team2] == WinLoseList.NoCheck && item.Value.m_byOtherCardList.m_ENAwardStatus != ENUM_BLACKJACK_AWARD.Idle)
                            {
                                if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.BlackJack)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card4Team2] = WinLoseList.BlackJack;
                                }
                                else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinBanker)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card4Team2] = WinLoseList.WinBanker;
                                }
                                else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinPlayer)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card4Team2] = WinLoseList.WinPlayer;
                                }
                                else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinDraw)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card4Team2] = WinLoseList.WinDraw;
                                }
                                BJEndWindow_Control.WinLoseShowSave[(byte)CardTeamList.Card4Team2] = 1;
                            }
                            #endregion
                        }
                        else if (i == 4)
                        {
                            #region Player4FinallData
                            if (BJMainGame_Control.TableWinLose[CardTeamList.Card5Team1] == WinLoseList.NoCheck)
                            {
                                if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.BlackJack)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card5Team1] = WinLoseList.BlackJack;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinBanker)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card5Team1] = WinLoseList.WinBanker;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinPlayer)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card5Team1] = WinLoseList.WinPlayer;
                                }
                                else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinDraw)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card5Team1] = WinLoseList.WinDraw;
                                }
                                BJEndWindow_Control.WinLoseShowSave[(byte)CardTeamList.Card5Team1] = 1;
                            }
                            if (BJMainGame_Control.TableWinLose[CardTeamList.Card5Team2] == WinLoseList.NoCheck && item.Value.m_byOtherCardList.m_ENAwardStatus != ENUM_BLACKJACK_AWARD.Idle)
                            {
                                if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.BlackJack)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card5Team2] = WinLoseList.BlackJack;
                                }
                                else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinBanker)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card5Team2] = WinLoseList.WinBanker;
                                }
                                else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinPlayer)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card5Team2] = WinLoseList.WinPlayer;
                                }
                                else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinDraw)
                                {
                                    BJMainGame_Control.TableWinLose[CardTeamList.Card5Team2] = WinLoseList.WinDraw;
                                }
                                BJEndWindow_Control.WinLoseShowSave[(byte)CardTeamList.Card5Team2] = 1;
                            }
                            #endregion
                        }
                        #endregion
                    }
                    //歷史紀錄
                    #region Histroy
                    if (item.Value.m_uiDBID == Cash_Control.PlayerDBID[i] && i == 0)
                    {
                        HistoryData HistoryDataSave = new HistoryData();
                        HistoryDataSave.BankerPoint = Data.m_oBetAward.m_BankerCards.m_bySam;
                        HistoryDataSave.PlayerPoint = item.Value.m_byMainCardList.m_bySam;
                        if (item.Value.m_byMainCardList.m_ENCardStatus == ENUM_BLACKJACK_CardStatus.BlackJack && BJMainGame_Control.BlackJack_Bool[0] == 1)
                            HistoryDataSave.m_WinLoseList = WinLoseList.BlackJack;
                        else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinBanker)
                            HistoryDataSave.m_WinLoseList = WinLoseList.WinBanker;
                        else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinPlayer)
                            HistoryDataSave.m_WinLoseList = WinLoseList.WinPlayer;
                        else if (item.Value.m_byMainCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinDraw)
                            HistoryDataSave.m_WinLoseList = WinLoseList.WinDraw;

                        HistoryDataSave.SPLIT_Bool = false;

                        BJHistory_Control.History_DicSave.Add(0, HistoryDataSave);
                        if (item.Value.m_byOtherCardList.m_bySam != 0)
                        {
                            HistoryData HistoryDataSave2 = new HistoryData();
                            HistoryDataSave2.BankerPoint = Data.m_oBetAward.m_BankerCards.m_bySam;
                            HistoryDataSave2.PlayerPoint = item.Value.m_byOtherCardList.m_bySam;
                            if (item.Value.m_byOtherCardList.m_ENCardStatus == ENUM_BLACKJACK_CardStatus.BlackJack && BJMainGame_Control.BlackJack_Bool[0] == 1)
                                HistoryDataSave2.m_WinLoseList = WinLoseList.BlackJack;
                            else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinBanker)
                                HistoryDataSave2.m_WinLoseList = WinLoseList.WinBanker;
                            else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinPlayer)
                                HistoryDataSave2.m_WinLoseList = WinLoseList.WinPlayer;
                            else if (item.Value.m_byOtherCardList.m_ENAwardStatus == ENUM_BLACKJACK_AWARD.WinDraw)
                                HistoryDataSave2.m_WinLoseList = WinLoseList.WinDraw;
                            //if (item.Value.m_byOtherCardList.m_ENCardStatus == ENUM_BLACKJACK_CardStatus.BlackJack)
                            //    HistoryDataSave2.m_WinLoseList = WinLoseList.BlackJack;
                            //else
                            //    HistoryDataSave2.m_WinLoseList = WinLoseList.NoCheck;
                            //    HistoryDataSave2.m_WinLoseList = WinLoseList.NoCheck;
                            HistoryDataSave.SPLIT_Bool = true;

                            BJHistory_Control.History_DicSave.Add(1, HistoryDataSave2);
                        }
                    }
                    #endregion
                }
            }

            BJMainGame_Control.FinallMoney = Data.m_ui64GameMoney;

            BJMainGame_Control.BankerFinallData_Bool = true;
        }
        Debug.Log("收到當局最後結果");
    }

    //收到當局金額結算
    public void FinallEnd(byte[] byarData)
    {
        CPACK_BlackJack_PlayerRank Data = GameConnet.m_oGameClient.DoDeSerialize<CPACK_BlackJack_PlayerRank>(byarData);

        if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState != ENUM_BLACKJACK_TABLE_STATE.Idle)
        {
            foreach (var item in Data.m_dicPlayerWin)
            {
                BJMainGame_Control.FinallEndSave.Add(item.Key, item.Value);
                Debug.Log("玩家DBID: " + item.Key + "  //總贏金: " + item.Value.m_ui64GameMoney + "  //總押金: " + item.Value.m_ui64AllBetMoney);
            }
        }
        Debug.Log("收到當局金額結算");
    }

    //玩家中途離開
    public void GamePlayerOut(byte[] byarData)
    {
        CSeatPlayerData Data = GameConnet.m_oGameClient.DoDeSerialize<CSeatPlayerData>(byarData);
        for (int i = 0; i < 5; i++)
        {
            if (Data.m_uiPlayerDBID == Cash_Control.PlayerDBID[i])
            {
                Cash_Control.PlayerOutSave[i] = 1;
                Cash_Control.ThisTablePlayerIn[i] = 0;
                Cash_Control.TableCash[i * 2] = 0;
                Cash_Control.TableCash[(i * 2) + 1] = 0;
            }
        }
        Debug.Log("收到玩家中途離開");
    }
}
