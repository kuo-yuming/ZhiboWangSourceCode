using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientSimulator;
using GameCore;
using GameCore.Manager.Common;
using ProtoBuf;
using System;
using GameCore.Machine;
using GameCore.Manager.Baccarat;
using Race_Info_Data;
using GameEnum;

public class BaccaratManager : MonoBehaviour
{

    public static string GameVol = "1.0.0.160622";
    public static CPACK_TMGameConfig m_MachineBuyInConfig = null;       //機台設定資料
    public static CPACK_Baccarat_GameConfig m_GameConfig = null;       //遊戲設定資料
    public static Dictionary<uint, CPACK_TMachineData> m_MachineDatas = null;         //機台資料
    public static Dictionary<uint, ushort> m_MachineTableArea = null;         //機台資料
    public static CPACK_TMachineMemberList MachineMemberList = new CPACK_TMachineMemberList();  //該機台 成員名單


    public static  UnityEngine.Object BetLock = new UnityEngine.Object();


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

    // 當收到Baccarat系統封包時觸發
    public void OnRcvBaccaratData(uint uiPackID, byte[] byarData)
    {
        Debug.Log(string.Format("OnRcvBaccaratData. PackID={0}", uiPackID));

        switch (uiPackID)
        {
            case (uint)ENUM_COMMON_PACKID_GC.G2C_TMachine_NotifyGameConfig:
                RcvMachineData(byarData);
                break;
            case (uint)ENUM_BACCARAT_PACKID_GC.G2C_NotifyGameConfig:
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

            case (uint)ENUM_BACCARAT_PACKID_GC.G2C_Game_NotifyBet:
                RcvBetResult(byarData);
                break;
            case (uint)ENUM_BACCARAT_PACKID_GC.G2C_Game_NotifyState:
                RcvNowState(byarData);
                break;
            case (uint)ENUM_BACCARAT_PACKID_GC.G2C_Game_NotifyTableInfo:
                RcvHistoryDataInfo(byarData);
                break;
            case (uint)ENUM_BACCARAT_PACKID_GC.G2C_Game_NotifyDeal:
                RcvCardResult(byarData);
                break;
            case (uint)ENUM_BACCARAT_PACKID_GC.G2C_Game_NotifyAward:
                RcvFinallResult(byarData);
                break;
            case (uint)ENUM_BACCARAT_PACKID_GC.G2C_Game_NotifyBidInfo:
                RcvFourCardFinallBetResult(byarData);
                break;
            case (uint)ENUM_BACCARAT_PACKID_GC.G2C_Game_NotifyBid:
                RcvFourCardBetResult(byarData);
                break;
            case (uint)ENUM_BACCARAT_PACKID_GC.G2C_Game_NotifyShuffle:
                RcvShuffleCardResult(byarData);
                break;
            case (uint)ENUM_BACCARAT_PACKID_GC.G2C_Game_NofifyTotalBidMoney:
                RcvFCJPMoney(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_TMachine_RplyBuyinResult:
                RcvBuyInFail(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_TMachine_KickToLobby:
                Message_Control.OpenMessage = true;
                Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                Message_Control.MessageStatus = Message_Control.MessageStatu.PlayerOut;
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_Race_NotifyEventSchedule:
                RcvRaceData(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_Race_NotifySwitchState:
                RcvChangeRaceData(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_Race_RplySignEventResult:
                RcvRaceResult(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_Race_RplyCancelSignEventResult:
                RcvCancelResult(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_Race_RplyTableID:
                RaceTableResult(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_Race_NotifyRanking:
                RcvRankingData(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_Race_NotifyRaceEnd:
                RcvRankingEndData(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_Race_RplyPreviusRanking:
                RcvBeforeRanking(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_Race_NotifyCancelRace:
                RcvRaceNoPlay(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_Race_NotifyUpdatePlayerCnt:
                RcvChangePlayerData(byarData);
                break;
            default:
                Debug.Log(string.Format("{0}:Unknown packid={1}", DateTime.Now, uiPackID));
                break;
        }

        Debug.Log(string.Format("OnRcvBaccaratData. PackID={0} end", uiPackID));
    }
    //---------------------------------------------------------------------



    // 收到遊戲資料資料
    public void RcvGameConfigData(byte[] byarData)
    {
        m_GameConfig = GameConnet.m_oGameClient.DoDeSerialize<CPACK_Baccarat_GameConfig>(byarData);
        foreach (var item in m_GameConfig.m_dicBetLimit)
        {
            foreach (var item2 in item.Value.m_dicGroupBetLimit)
            {
                if (item2.Key == 1)
                {
                    if (item2.Value.m_uiMinBet < GameOutAndDataNumber.SmallMinBet || GameOutAndDataNumber.SmallMinBet == 0)
                    {
                        GameOutAndDataNumber.SmallMinBet = item2.Value.m_uiMinBet;
                    }
                    if (item2.Value.m_uiMinBet > GameOutAndDataNumber.SmallMaxBet)
                    {
                        GameOutAndDataNumber.SmallMaxBet = item2.Value.m_uiMaxBet;
                    }
                }
                else
                {
                    if (item2.Value.m_uiMinBet < GameOutAndDataNumber.BigMinBet || GameOutAndDataNumber.BigMinBet == 0)
                    {
                        GameOutAndDataNumber.BigMinBet = item2.Value.m_uiMinBet;
                    }
                    if (item2.Value.m_uiMinBet > GameOutAndDataNumber.BigMaxBet)
                    {
                        GameOutAndDataNumber.BigMaxBet = item2.Value.m_uiMaxBet;
                    }
                }
                //     Debug.Log("區號ID: " + item.Key + " //大小底台ID: " + item2.Key + " //最大押金: " + item2.Value.m_uiMaxBet + " //最小押金: " + item2.Value.m_uiMinBet + " //賠率: ");
            }
        }
        FourCard_Control.FeeMoney = m_GameConfig.m_byBidCommission;
        Debug.Log("收到各區塊賠率表資料");
    }
    //---------------------------------------------------------------------
    // 收到檯桌式機台機制
    public void RcvMachineData(byte[] byarData)
    {
        m_MachineDatas = new Dictionary<uint, CPACK_TMachineData>();
        m_MachineTableArea = new Dictionary<uint, ushort>();
        m_MachineBuyInConfig = GameConnet.m_oGameClient.DoDeSerialize<CPACK_TMGameConfig>(byarData);
        MainGame_Control.NoBetRound = m_MachineBuyInConfig.m_byChkUnbetRound;
        Debug.Log("收到檯桌式機台機制 : " + "等級多少以上才能Buyin : " + MainConnet.m_PlayerData.m_strNickName);
        foreach (var item in m_MachineBuyInConfig.m_dicTableGroupSet)
        {
            Debug.Log("分區標籤名稱 : " + item.Value.m_strTagName + "  起始桌號 : " + item.Value.m_uiStartTableID + "  末尾桌號 : " + item.Value.m_uiEndTableID);
        }
        foreach (var item in m_MachineBuyInConfig.m_dicTableGroupSet)
        {
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
                GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Baccarat, (uint)ENUM_COMMON_PACKID_GC.C2G_TMachine_ReqBuyin, GameConnet.m_oGameClient.DoSerialize<CPACK_TMachineBuyin>(m_BuyInMoney));
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
    //百家樂
    //接收桌台狀態
    public void RcvNowState(byte[] byarData)
    {
        CPACK_Baccarat_UpdateTbleState Data = GameConnet.m_oGameClient.DoDeSerialize<CPACK_Baccarat_UpdateTbleState>(byarData);
        if (GameConnet.m_TMachineBuyInGameData.m_uiTID != Data.m_uiTableID)
            return; //如果不是自己桌的狀態 不更新
        MainGame_Control.WaitTotalPlayerTimer = 0;
        if (MainConnet.m_Scence == MainConnet.SecnceType.Baccarat_Game)
        {
            if (MainGame_Control.FirstGameBool)
            {
                if (Data.m_enumState == ENUM_BACCARAT_TABLE_STATE.NewRound || Data.m_enumState == ENUM_BACCARAT_TABLE_STATE.ShuffleNewRound || Data.m_enumState == ENUM_BACCARAT_TABLE_STATE.WaitBet)
                {
                    MainGame_Control.StopModeState = GameEnum.ENUM_STOPMODE_STATE.WaitStop;
                }
                else if (Data.m_enumState == ENUM_BACCARAT_TABLE_STATE.Idle)
                {
                    MainGame_Control.StopModeState = GameEnum.ENUM_STOPMODE_STATE.Idle;
                }
                else
                {
                    MainGame_Control.StopModeState = GameEnum.ENUM_STOPMODE_STATE.WaitNextNewRound;
                }
                FourCardHistory_Control.FirstStautGet = true;
                MainGame_Control.FirstGameBool = false;
            }

            if (Data.m_enumState == ENUM_BACCARAT_TABLE_STATE.ShuffleNewRound)
            {
                History_Control.AngleInit = true;
            }

            MainGame_Control.NowGameState = (byte)Data.m_enumState;
            Time_Control.MaxTime = (byte)(Data.m_uiWaitMSec / 1000);

            Time_Control.MinusTime = 0;
            if (Data.m_enumState == ENUM_BACCARAT_TABLE_STATE.Idle)
            {
                if ((Data.m_uiWaitMSec / 1000) > 60)
                {
                    MainGame_Control.WaitTotalPlayerTimer = 60;
                }
                else
                {
                    MainGame_Control.WaitTotalPlayerTimer = (int)(Data.m_uiWaitMSec / 1000);
                }
            }
        }
        Debug.Log("收到機台更新  機台狀態: " + Data.m_enumState + " //時間: " + Data.m_uiWaitMSec);
    }

    //接收桌台結果資料
    public void RcvHistoryDataInfo(byte[] byarData)
    {
        CPACK_Baccarat_NotifyTableInfo Data = GameConnet.m_oGameClient.DoDeSerialize<CPACK_Baccarat_NotifyTableInfo>(byarData);
        if (GameConnet.m_TMachineBuyInGameData.m_uiTID != Data.m_uiTableID)
            return; //如果不是自己桌的狀態 不更新
        if (MainConnet.m_Scence == MainConnet.SecnceType.Baccarat_Game)
        {
            History_Control.HistoryNumber = 0;
            History_Control.HistoryData.Clear();
            History_Control.HistoryParkwayData.Clear();

            foreach (var item in Data.m_listRoundAward)
            {
                History_Control.HistoryData.Add(History_Control.HistoryNumber, item);
                History_Control.HistoryNumber++;
            }
            MainGame_Control.LeftOverCardPoint = Data.m_usRemainNormalCards;
            FourCardHistory_Control.FCLeftOverCardPoint = Data.m_usRemainBidCards;
            History_Control.HistoryDataGetBool = true;
            FourCardHistory_Control.FCHistoryDataGetBool = true;
        }
        Debug.Log("收到歷史紀錄");
    }

    //接收押注資料
    public void RcvBetResult(byte[] byarData)
    {
        if (MainConnet.m_Scence == MainConnet.SecnceType.Baccarat_Game)
        {
            lock (BetLock)
            {
                CPACK_Baccarat_NotifyBet Data = GameConnet.m_oGameClient.DoDeSerialize<CPACK_Baccarat_NotifyBet>(byarData);
                if (GameConnet.m_TMachineBuyInGameData.m_uiTID != Data.m_uiTableID)
                    return; //如果不是自己桌的狀態 不更新
                if (Data.m_enumResult == ENUM_COMMON_ERROR_CODE.Success)
                {
                    if (Data.m_uiBetPlayerDBID == MainConnet.m_PlayerData.m_uiDBID)
                    {
                        if (Data.m_byAreaID == (byte)ENUM_BACCARAT_AWARD_AREA.Banker)
                        {
                            if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.WaitStop)
                            {
                                BetTable_Control.MyBetMoneySeat[0] = Data.m_uiPlayerBetMoney;
                                MyBetCashTip_Control.MyBetCashData[0] = Data.m_uiPlayerBetMoney;
                                AutoMode_Control.SaveMoney[0] = Data.m_uiPlayerBetMoney;
                            }
                            else
                            {
                                BetTable_Control.SaveMoneyUpData[0] = Data.m_ui64AllBetMoney;
                                BetTable_Control.SaveCheck = true;
                            }
                            //         Debug.Log("自己押注(莊): " + BetTable_Control.MyBetMoneySeat[0]);
                        }
                        else if (Data.m_byAreaID == (byte)ENUM_BACCARAT_AWARD_AREA.Player)
                        {
                            if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.WaitStop)
                            {
                                BetTable_Control.MyBetMoneySeat[1] = Data.m_uiPlayerBetMoney;
                                MyBetCashTip_Control.MyBetCashData[1] = Data.m_uiPlayerBetMoney;
                                AutoMode_Control.SaveMoney[1] = Data.m_uiPlayerBetMoney;
                            }
                            else
                            {
                                BetTable_Control.SaveMoneyUpData[1] = Data.m_ui64AllBetMoney;
                                BetTable_Control.SaveCheck = true;
                            }                         
                            //       Debug.Log("自己押注(閒): " + BetTable_Control.MyBetMoneySeat[1]);
                        }
                        else if (Data.m_byAreaID == (byte)ENUM_BACCARAT_AWARD_AREA.Draw)
                        {
                            if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.WaitStop)
                            {
                                BetTable_Control.MyBetMoneySeat[2] = Data.m_uiPlayerBetMoney;
                                MyBetCashTip_Control.MyBetCashData[2] = Data.m_uiPlayerBetMoney;
                                AutoMode_Control.SaveMoney[2] = Data.m_uiPlayerBetMoney;
                            }
                            else
                            {
                                BetTable_Control.SaveMoneyUpData[2] = Data.m_ui64AllBetMoney;
                                BetTable_Control.SaveCheck = true;
                            }
                        
                            //        Debug.Log("自己押注(和): " + BetTable_Control.MyBetMoneySeat[2]);
                        }
                        else if (Data.m_byAreaID == (byte)ENUM_BACCARAT_AWARD_AREA.BankerPair)
                        {
                            if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.WaitStop)
                            {
                                BetTable_Control.MyBetMoneySeat[3] = Data.m_uiPlayerBetMoney;
                                MyBetCashTip_Control.MyBetCashData[3] = Data.m_uiPlayerBetMoney;
                                AutoMode_Control.SaveMoney[3] = Data.m_uiPlayerBetMoney;
                            }
                            else
                            {
                                BetTable_Control.SaveMoneyUpData[3] = Data.m_ui64AllBetMoney;
                                BetTable_Control.SaveCheck = true;
                            }
                         
                            //    Debug.Log("自己押注(莊對): " + BetTable_Control.MyBetMoneySeat[3]);
                        }
                        else if (Data.m_byAreaID == (byte)ENUM_BACCARAT_AWARD_AREA.PlayerPair)
                        {
                            if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.WaitStop)
                            {
                                BetTable_Control.MyBetMoneySeat[4] = Data.m_uiPlayerBetMoney;
                                MyBetCashTip_Control.MyBetCashData[4] = Data.m_uiPlayerBetMoney;
                                AutoMode_Control.SaveMoney[4] = Data.m_uiPlayerBetMoney;
                            }
                            else
                            {
                                BetTable_Control.SaveMoneyUpData[4] = Data.m_ui64AllBetMoney;
                                BetTable_Control.SaveCheck = true;
                            }
                          
                            //      Debug.Log("自己押注(閒對): " + BetTable_Control.MyBetMoneySeat[4]);
                        }
                        Money_Control.MyBetMoney += Data.m_iAddBetMoney;
                        Bet_Control.BetDataGetBool = false;
                    }

                    if (Data.m_byAreaID == (byte)ENUM_BACCARAT_AWARD_AREA.Banker)
                    {
                        if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.WaitStop)
                            BetTable_Control.TableAllBetMoneySeat[0] = Data.m_ui64AllBetMoney;
                        else
                        {
                            BetTable_Control.SaveMoneyUpData[0] = Data.m_ui64AllBetMoney;
                            BetTable_Control.SaveCheck = true;
                        }
                        //        Debug.Log("本桌押注(莊): " + BetTable_Control.TableAllBetMoneySeat[0]);
                    }
                    else if (Data.m_byAreaID == (byte)ENUM_BACCARAT_AWARD_AREA.Player)
                    {
                        if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.WaitStop)
                            BetTable_Control.TableAllBetMoneySeat[1] = Data.m_ui64AllBetMoney;
                        else
                        {
                            BetTable_Control.SaveMoneyUpData[1] = Data.m_ui64AllBetMoney;
                            BetTable_Control.SaveCheck = true;
                        }
                        //       Debug.Log("本桌押注(閒): " + BetTable_Control.TableAllBetMoneySeat[1]);
                    }
                    else if (Data.m_byAreaID == (byte)ENUM_BACCARAT_AWARD_AREA.Draw)
                    {
                        if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.WaitStop)
                            BetTable_Control.TableAllBetMoneySeat[2] = Data.m_ui64AllBetMoney;
                        else
                        {
                            BetTable_Control.SaveMoneyUpData[2] = Data.m_ui64AllBetMoney;
                            BetTable_Control.SaveCheck = true;
                        }
                        //        Debug.Log("本桌押注(和): " + BetTable_Control.TableAllBetMoneySeat[2]);
                    }
                    else if (Data.m_byAreaID == (byte)ENUM_BACCARAT_AWARD_AREA.BankerPair)
                    {
                        if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.WaitStop)
                            BetTable_Control.TableAllBetMoneySeat[3] = Data.m_ui64AllBetMoney;
                        else
                        {
                            BetTable_Control.SaveMoneyUpData[3] = Data.m_ui64AllBetMoney;
                            BetTable_Control.SaveCheck = true;
                        }
                        //      Debug.Log("本桌押注(莊對): " + BetTable_Control.TableAllBetMoneySeat[3]);
                    }
                    else if (Data.m_byAreaID == (byte)ENUM_BACCARAT_AWARD_AREA.PlayerPair)
                    {
                        if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.WaitStop)
                            BetTable_Control.TableAllBetMoneySeat[4] = Data.m_ui64AllBetMoney;
                        else
                        {
                            BetTable_Control.SaveMoneyUpData[4] = Data.m_ui64AllBetMoney;
                            BetTable_Control.SaveCheck = true;
                        }
                        //       Debug.Log("本桌押注(閒對): " + BetTable_Control.TableAllBetMoneySeat[4]);
                    }
                }
                else if (Data.m_enumResult == ENUM_COMMON_ERROR_CODE.Baccarat_MoneyNotEnough)
                {
                    Message_Control.OpenMessage = true;
                    Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                    Message_Control.MessageStatus = Message_Control.MessageStatu.NoBetMoney;
                    Bet_Control.BetDataGetBool = false;
                }
                else if (Data.m_enumResult == ENUM_COMMON_ERROR_CODE.Baccarat_StopBet)
                {
                    Message_Control.OpenMessage = true;
                    Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                    Message_Control.MessageStatus = Message_Control.MessageStatu.StopMode;
                    Bet_Control.BetDataGetBool = false;
                }
                else if (Data.m_enumResult == ENUM_COMMON_ERROR_CODE.Baccarat_StopBid)
                {
                    Message_Control.OpenMessage = true;
                    Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                    Message_Control.MessageStatus = Message_Control.MessageStatu.StopSnatchCardMode;
                    Bet_Control.BetDataGetBool = false;
                }
                else if (Data.m_enumResult == ENUM_COMMON_ERROR_CODE.Baccarat_OverBetLimit)
                {
                    Message_Control.OpenMessage = true;
                    Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                    Message_Control.MessageStatus = Message_Control.MessageStatu.MaxBetOut;
                    Bet_Control.BetDataGetBool = false;
                }
            }

            //   Debug.Log("收到押注資料");
        }
    }

    //接收CARD資料
    public void RcvCardResult(byte[] byarData)
    {
        CPACK_Baccarat_NotifyDealData Data = GameConnet.m_oGameClient.DoDeSerialize<CPACK_Baccarat_NotifyDealData>(byarData);
        if (GameConnet.m_TMachineBuyInGameData.m_uiTID != Data.m_uiTableID)
            return; //如果不是自己桌的狀態 不更新
        if (MainConnet.m_Scence == MainConnet.SecnceType.Baccarat_Game)
        {
            for (int i = 0; i < 3; i++)
            {
                Card_Control.BankerCard[i] = Data.m_byBankerCard[i];
                Card_Control.PlayerCard[i] = Data.m_byPlayerCard[i];
                if (Card_Control.BankerCard[i] != 0)
                {
                    CardOpen_Control.CardOpenSeat[i] = 1;
                }
                else
                {
                    CardOpen_Control.CardOpenSeat[i] = 0;
                }

                if (Card_Control.PlayerCard[i] != 0)
                {
                    CardOpen_Control.CardOpenSeat[i + 3] = 1;
                }
                else
                {
                    CardOpen_Control.CardOpenSeat[i + 3] = 0;
                }
            }
            FourCard_Control.FourCard = Data.m_byFourthCard;
            Card_Control.CardDataGetBool = true;

        }

        Debug.Log("收到Card資料: " + " //莊 : " + Card_Control.BankerCard[0] + "," + Card_Control.BankerCard[1] + "," + Card_Control.BankerCard[2] + " //閒 : " + Card_Control.PlayerCard[0] + "," + Card_Control.PlayerCard[1] + "," + Card_Control.PlayerCard[2] + " //搶牌: " + FourCard_Control.FourCard);
    }

    //接收結算資料
    public void RcvFinallResult(byte[] byarData)
    {
        CPACK_Baccarat_NotifyAward Data = GameConnet.m_oGameClient.DoDeSerialize<CPACK_Baccarat_NotifyAward>(byarData);
        if (GameConnet.m_TMachineBuyInGameData.m_uiTID != Data.m_uiTableID)
            return; //如果不是自己桌的狀態 不更新
        if (MainConnet.m_Scence == MainConnet.SecnceType.Baccarat_Game)
        {
            if (MainConnet.m_Scence == MainConnet.SecnceType.Baccarat_Game)
            {
                if (MainGame_Control.StopModeState != GameEnum.ENUM_STOPMODE_STATE.WaitNextNewRound || MainGame_Control.StopModeState != GameEnum.ENUM_STOPMODE_STATE.Idle)
                {
                    byte Number = 0;
                    Card_Control.SaveFinallData.Add(Number, Data);
                    Card_Control.FinallDataOk = true;
                }
                else
                {
                    History_Control.HistoryData[(byte)(History_Control.HistoryNumber - 1)].m_oLastAward.m_enumAward = Data.m_oBetAward.m_enumAward;
                    History_Control.HistoryData[(byte)(History_Control.HistoryNumber - 1)].m_oLastAward.m_bBankerOnePair = Data.m_oBetAward.m_bBankerOnePair;
                    History_Control.HistoryData[(byte)(History_Control.HistoryNumber - 1)].m_oLastAward.m_bPlayerOnePair = Data.m_oBetAward.m_bPlayerOnePair;
                    History_Control.HistoryData[(byte)(History_Control.HistoryNumber - 1)].m_oLastAward.m_byBankerPoint = Data.m_byBankerPoint;
                    History_Control.HistoryData[(byte)(History_Control.HistoryNumber - 1)].m_oLastAward.m_byPlayerPoint = Data.m_byPlayerPoint;
                    if (Data.m_byBankerPoint > Data.m_byPlayerPoint)
                    {
                        FourCardHistory_Control.SaveFcCardPoint = Data.m_byBankerPoint;
                    }
                    else if (Data.m_byBankerPoint < Data.m_byPlayerPoint)
                    {
                        FourCardHistory_Control.SaveFcCardPoint = Data.m_byPlayerPoint;
                    }
                    else if (Data.m_byBankerPoint == Data.m_byPlayerPoint)
                    {
                        FourCardHistory_Control.SaveFcCardPoint = Data.m_byBankerPoint;
                    }
                    FourCardHistory_Control.FCHistorySaveOkBool = true;
                }
            }
        }
        Debug.Log("收到最後結果");
    }

    //接收競標最後金額資料
    public void RcvFourCardFinallBetResult(byte[] byarData)
    {
        CPACK_Baccarat_NotifyBidInfo Data = GameConnet.m_oGameClient.DoDeSerialize<CPACK_Baccarat_NotifyBidInfo>(byarData);

        FourCard_Control.AllFCBetMoney[0] = Data.m_ui64BankerAllBidMoney;
        FourCard_Control.AllFCBetMoney[1] = Data.m_ui64PlayerAllBidMoney;
        FourCard_Control.FourCard = Data.m_byFourthCard;
        if (Data.m_byFourthCard != 0)
        {
            FourCardHistory_Control.FCCardPoint = (byte)(((Data.m_byFourthCard - 1) % 13) + 1);
        }
        else
        {
            FourCardHistory_Control.FCCardPoint = 0;
        }
        CardOpen_Control.FourCardWiner_Bool = true;
    }

    //接收競標押注資料
    public void RcvFourCardBetResult(byte[] byarData)
    {
        lock (BetLock)
        {
            CPACK_Baccarat_NotifyBet Data = GameConnet.m_oGameClient.DoDeSerialize<CPACK_Baccarat_NotifyBet>(byarData);
            if (GameConnet.m_TMachineBuyInGameData.m_uiTID != Data.m_uiTableID)
                return; //如果不是自己桌的狀態 不更新
            if (Data.m_enumResult == ENUM_COMMON_ERROR_CODE.Success)
            {
                if (Data.m_byAreaID == (byte)ENUM_BACCARAT_AWARD_AREA.Banker)
                {
                    if (Data.m_uiBetPlayerDBID == MainConnet.m_PlayerData.m_uiDBID)
                    {
                        Money_Control.MyBetMoney += Data.m_iAddBetMoney;
                        EndWindow_Control.FourCardBetMoney[0] = Data.m_uiPlayerBetMoney;
                        FourCard_Control.BetMoney[0] = Data.m_uiPlayerBetMoney;
                        FourCard_Control.FourCardBet = false;
                    }

                    FourCard_Control.AllFCBetMoney[0] = Data.m_ui64AllBetMoney;
                    Debug.Log("搶牌押注(莊): " + Data.m_uiPlayerBetMoney + "所有押金: " + Data.m_ui64AllBetMoney);
                }
                else if (Data.m_byAreaID == (byte)ENUM_BACCARAT_AWARD_AREA.Player)
                {
                    if (Data.m_uiBetPlayerDBID == MainConnet.m_PlayerData.m_uiDBID)
                    {
                        Money_Control.MyBetMoney += Data.m_iAddBetMoney;
                        EndWindow_Control.FourCardBetMoney[1] = Data.m_uiPlayerBetMoney;
                        FourCard_Control.BetMoney[1] = Data.m_uiPlayerBetMoney;
                        FourCard_Control.FourCardBet = false;
                    }

                    FourCard_Control.AllFCBetMoney[1] = Data.m_ui64AllBetMoney;
                    Debug.Log("搶牌押注(閒): " + Data.m_uiPlayerBetMoney + "所有押金: " + Data.m_ui64AllBetMoney);
                }

            }
            else if (Data.m_enumResult == ENUM_COMMON_ERROR_CODE.Baccarat_MoneyNotEnough)
            {
                Message_Control.OpenMessage = true;
                Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                Message_Control.MessageStatus = Message_Control.MessageStatu.NoBetMoney;
                FourCard_Control.FourCardBet = false;
            }
            else if (Data.m_enumResult == ENUM_COMMON_ERROR_CODE.Baccarat_StopBet)
            {
                Message_Control.OpenMessage = true;
                Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                Message_Control.MessageStatus = Message_Control.MessageStatu.StopMode;
                FourCard_Control.FourCardBet = false;
            }
            else if (Data.m_enumResult == ENUM_COMMON_ERROR_CODE.Baccarat_StopBid)
            {
                Message_Control.OpenMessage = true;
                Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                Message_Control.MessageStatus = Message_Control.MessageStatu.StopSnatchCardMode;
                FourCard_Control.FourCardBet = false;
            }
            else if (Data.m_enumResult == ENUM_COMMON_ERROR_CODE.Baccarat_OverBetLimit)
            {
                Message_Control.OpenMessage = true;
                Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                Message_Control.MessageStatus = Message_Control.MessageStatu.MaxBetOut;
                FourCard_Control.FourCardBet = false;
            }
        }
    }

    public void RcvShuffleCardResult(byte[] byarData)
    {
        if (MainConnet.m_Scence == MainConnet.SecnceType.Baccarat_Game)
        {
            CPACK_Baccarat_NotifyShuffle Data = GameConnet.m_oGameClient.DoDeSerialize<CPACK_Baccarat_NotifyShuffle>(byarData);
            if (GameConnet.m_TMachineBuyInGameData.m_uiTID != Data.m_uiTableID)
                return; //如果不是自己桌的狀態 不更新
            FourCardHistory_Control.FCHistoryDataInit = Data.m_bBid;
        }
    }

    //競標JP更新
    public void RcvFCJPMoney(byte[] byarData)
    {
        FourCard_Control.FCJPMoney = GameConnet.m_oGameClient.DoDeSerialize<UInt64>(byarData);
        Debug.Log(string.Format("競標金更新:" + FourCard_Control.FCJPMoney));
    }

    //接收競賽資料
    public void RcvRaceData(byte[] byarData)
    {
        //if (!Competition.DataChange_Bool && !Competition.LockObject_Bool)
        lock (Competition.CompetitionLockObject)
        {
            Competition.CompetitionData.Clear();

            CPACK_RACE_EventSchedule Data = GameConnet.m_oGameClient.DoDeSerialize<CPACK_RACE_EventSchedule>(byarData);

            foreach (var item in Data.m_dicEventSchedule)
            {
                Competition.CompetitionData.Add(item.Key, item.Value);
            }

            if (!RaceInfo.RaceInfoDataChange_bool)
            {
                RaceInfo.RaceInfoData.Clear();
                foreach (var item in Data.m_dicEventSchedule)
                {
                    RaceInfoDataSave SaveData = new RaceInfoDataSave();
                    SaveData.DataType = item.Value.m_enumEventType;
                    foreach (var item2 in item.Value.m_dicRaceSetting)
                    {
                        SaveData.RaceData.Add(item2.Key, item2.Value);
                    }
                    RaceInfo.RaceInfoData.Add(item.Key, SaveData);
                }
                if (MainConnet.m_Scence == MainConnet.SecnceType.Baccarat_Lobby)
                    Competition.DataChange_Bool = true;
            }

            RaceInfo.RaceInfoDataChange_bool = true;
        }
        Debug.Log("接收競賽資料");
    }

    //更新競賽資料
    public void RcvChangeRaceData(byte[] byarData)
    {
        CPACK_RACE_SwitchState Data = GameConnet.m_oGameClient.DoDeSerialize<CPACK_RACE_SwitchState>(byarData);
        Debug.Log("直接接收競賽資料更新開始");
        if (Competition.CompetitionData.ContainsKey(Data.m_usEventID))
        {
            //if (!Competition.DataChange_Bool)
            lock (Competition.CompetitionLockObject)
            {
                Debug.Log("資料更新開始");
                Competition.CompetitionData[Data.m_usEventID].m_oSwitchState.m_timeNext = Data.m_timeNext;
                Competition.CompetitionData[Data.m_usEventID].m_oSwitchState.m_enumState = Data.m_enumState;

                if (Data.m_enumState == ENUM_RACE_STATE.Sign)
                {
                    Competition.CompetitionData[Data.m_usEventID].m_bSigned = false;
              //      Competition.CompetitionData[Data.m_usEventID].m_uiNowPlayerCnt = 0;
                }
                else if (Data.m_enumState == ENUM_RACE_STATE.End)
                {
                    Competition.CompetitionData[Data.m_usEventID].m_uiNowPlayerCnt = 0;
                }
                //Competition.SaveCompetition.Add(Data.m_usEventID, Data);
                //if (MainConnet.m_Scence == MainConnet.SecnceType.Baccarat_Lobby)
                //    Competition.DataChange_Bool = true;
                // Competition.LockObject_Bool = true;
                if (MainConnet.m_Scence == MainConnet.SecnceType.Baccarat_Lobby)
                    Competition.DataChange_Bool = true;
                Debug.Log("直接接收競賽資料更新" + " //賽事ID: " + Data.m_usEventID);
            }
            //else if (!Competition.LockObject_Bool)
            //{
            //    Debug.Log("储存競賽資料更新開始");
            //Competition.SaveCompetition.Add(Data.m_usEventID, Data);
            //    if (MainConnet.m_Scence == MainConnet.SecnceType.Baccarat_Lobby)
            //        Competition.LockObject_Bool = true;
            //    Debug.Log("储存競賽資料更新" + " //賽事ID: " + Data.m_usEventID);
            //}
        }
        Debug.Log("接收競賽資料更新" + " //賽事ID: " + Data.m_usEventID);
    }

    //接收報名結果
    public void RcvRaceResult(byte[] byarData)
    {
        CPACK_RACE_SignEventResult Data = GameConnet.m_oGameClient.DoDeSerialize<CPACK_RACE_SignEventResult>(byarData);

        //if (!Competition.DataChange_Bool && !Competition.LockObject_Bool)
        lock (Competition.CompetitionLockObject)
        {
            if (Data.m_iResultCode == 0)
            {
                Competition.CompetitionData[RaceWindowBox.RaceID].m_bSigned = true;
                Competition.SinedOK_Bool = true;
            }
            RaceWindowBox.CodeID = Data.m_iResultCode;
            RaceWindowBox.RaceWindowState = 4;
            Competition.RaceButtonClick = false;
        }
        Debug.Log("接收報名結果");
    }

    //接收取消報名結果
    public void RcvCancelResult(byte[] byarData)
    {
        CPACK_RACE_SignEventResult Data = GameConnet.m_oGameClient.DoDeSerialize<CPACK_RACE_SignEventResult>(byarData);
        if (Data.m_iResultCode == 0)
        {
            //if (!Competition.DataChange_Bool && !Competition.LockObject_Bool)
            lock (Competition.CompetitionLockObject)
            {
                Competition.CompetitionData[RaceWindowBox.RaceID].m_bSigned = false;
                Competition.SinedOK_Bool = false;
                RaceWindowBox.CodeID = 1;
            }
        }
        else
        {
            RaceWindowBox.CodeID = Data.m_iResultCode;
        }
        RaceWindowBox.RaceWindowState = 4;
        Competition.RaceButtonClick = false;

        Debug.Log("接收取消報名結果");
    }

    //進入比賽後桌子分配結果
    public void RaceTableResult(byte[] byarData)
    {
        CPACK_RACE_TableID Data = GameConnet.m_oGameClient.DoDeSerialize<CPACK_RACE_TableID>(byarData);

        if (Data.m_iResultCode == 0)
        {
            RaceWindowBox.MoneyBoxMoney = Data.m_ui64RaceMoney;
            RaceWindowBox.MoneyBoxID = Data.m_uiTableID;
            Debug.Log("參賽金: " + Data.m_ui64RaceMoney + " //參賽人數: " + Data.m_uiTableID);
            if (RaceWindowBox.BoxEventType != ENUM_RACE_EVENT_TYPE.Buying)
            {
                AllScenceLoad.LoadScence = false;
                RaceWindowBox.RaceMoneyBoxBool = true;
                Debug.Log("領取金幣視窗");
            }
            else
            {
                ulong BuyInMoney = MainConnet.m_PlayerData.m_ui64OwnMoney;
                if (BuyInMoney > m_MachineBuyInConfig.m_uiMaxBuyinMoney)
                    BuyInMoney = m_MachineBuyInConfig.m_uiMaxBuyinMoney;
                CPACK_TMachineBuyin m_BuyInMoney = new CPACK_TMachineBuyin();
                m_BuyInMoney.m_uiTID = Data.m_uiTableID;
                m_BuyInMoney.m_uiBuyinMoney = (uint)BuyInMoney;
                GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Baccarat, (uint)ENUM_COMMON_PACKID_GC.C2G_TMachine_ReqBuyin, GameConnet.m_oGameClient.DoSerialize<CPACK_TMachineBuyin>(m_BuyInMoney));
                Debug.Log("自動BUYIN: " + m_BuyInMoney.m_uiTID);
            }
            RaceWindowBox.RaceWindowState = 0;
            Competition.RaceGame_Bool = true;
        }
        else
        {
            AllScenceLoad.LoadScence = false;
            RaceWindowBox.CodeID = Data.m_iResultCode;
            RaceWindowBox.RaceWindowState = 4;
        }
        Race_Control.TotalPeople = Data.m_uiJoinCnt;
        Competition.RaceButtonClick = false;
        Debug.Log("進入比賽後桌子分配結果 : " + Data.m_iResultCode);
    }

    //取得名次排名
    public void RcvRankingData(byte[] byarData)
    {
        Race_Control.RaceRankingList.Clear();
        CPACK_Race_Ranking Data = GameConnet.m_oGameClient.DoDeSerialize<CPACK_Race_Ranking>(byarData);

        List<CRaceRanking> ListData = new List<CRaceRanking>(Data.m_listRanking);

        ListData.Sort(delegate (CRaceRanking x, CRaceRanking y)
        {
            return x.m_uiRank.CompareTo(y.m_uiRank);
        });

        ushort Number = 1;

        for (ushort i = 0; i < ListData.Count; i++)
        {
            if (ListData[i].m_uiRank != 0)
            {
                Race_Control.RaceRankingList.Add(Number, ListData[i]);
                Number++;
            }
            if (ListData[i].m_strNickName == MainConnet.m_PlayerData.m_strNickName)
            {
                if (ListData[i].m_uiRank != 0)
                {
                    Race_Control.NowPlayerScore = ListData[i].m_i64Score;
                }
                else
                {
                    Race_Control.NowPlayerScore = 0;
                }
            }
        }

        Race_Control.NowPlayerRanking = Data.m_uiNowPlayerRank;

        Race_Control.RaceRankingCheck = true;
        Debug.Log("取得名次排名");
    }

    //比賽結束資料
    public void RcvRankingEndData(byte[] byarData)
    {
        Race_Control.RaceRankingList.Clear();
        CPACK_Race_Ranking Data = GameConnet.m_oGameClient.DoDeSerialize<CPACK_Race_Ranking>(byarData);
        if (Data.m_uiTID != GameConnet.m_TMachineBuyInGameData.m_uiTID)
            return; //如果不是自己桌的狀態 不處理

        List<CRaceRanking> ListData = new List<CRaceRanking>(Data.m_listRanking);

        ListData.Sort(delegate (CRaceRanking x, CRaceRanking y)
        {
            return x.m_uiRank.CompareTo(y.m_uiRank);
        });

        ushort Number = 1;

        for (ushort i = 0; i < ListData.Count; i++)
        {
            if (ListData[i].m_uiRank != 0)
            {
                Race_Control.RaceRankingList.Add(Number, ListData[i]);
                Number++;
            }

            if (ListData[i].m_uiDBID == MainConnet.m_PlayerData.m_uiDBID)
            {
                if (ListData[i].m_uiRank != 0)
                {
                    Race_Control.NowPlayerScore = ListData[i].m_i64Score;
                }
                else
                {
                    Race_Control.NowPlayerScore = 0;
                }
                Race_Control.GetMoney = ListData[i].m_uiAward_Money;
                Race_Control.GetDiamond = ListData[i].m_uiAward_Diamond;
                Debug.Log("獲得獎金: " + ListData[i].m_uiAward_Money + " //玩家ID: " + ListData[i].m_uiDBID + " //玩家名稱: " + ListData[i].m_strNickName);
            }
        }

        Race_Control.NowPlayerRanking = Data.m_uiNowPlayerRank;

        Race_Control.RaceRankingCheck = true;
        Race_Control.RaceEnd_Bool = true;
        Debug.Log("取得最後名次排名");
    }

    //取得之前競賽資料
    public void RcvBeforeRanking(byte[] byarData)
    {
        CPACK_Race_RplyPreviusRanking Data = GameConnet.m_oGameClient.DoDeSerialize<CPACK_Race_RplyPreviusRanking>(byarData);
        BeforeRank.BeforeRankData.Clear();

        if (Data.m_iResultCode == 0)
        {
            TimeSpan TotalLogIn = DateTime.Now.Subtract(Data.m_dateTime); //LogIn日期相減

            if (TotalLogIn.TotalMinutes > 0)
            {
                ushort Number = 0;
                foreach (var item in Data.m_oRanking.m_listRanking)
                {
                    if (item.m_uiRank != 0)
                    {
                        BeforeRank.BeforeRankData.Add(Number, item);
                        Number++;
                    }
                }
                //        Competition.CompetitionBoxOpen_Bool = false;
                Competition.BeforeRankingBoxOpen_Bool = true;
                BeforeRank.BeforeRankDataCheck = true;
            }
            else
            {
                RaceWindowBox.CodeID = 1000;
                RaceWindowBox.RaceWindowState = 4;
            }
        }
        else
        {
            RaceWindowBox.CodeID = Data.m_iResultCode;
            RaceWindowBox.RaceWindowState = 4;
        }

        if (MainConnet.m_Scence == MainConnet.SecnceType.Baccarat_Lobby)
            Competition.RaceButtonClick = false;
        Debug.Log("取得之前競賽資料  資料比數: " + Data.m_oRanking.m_listRanking.Count);
    }

    //比賽流局
    public void RcvRaceNoPlay(byte[] byarData)
    {
        ushort Data = GameConnet.m_oGameClient.DoDeSerialize<ushort>(byarData);
        lock (Competition.CompetitionLockObject)
        //if (!Competition.DataChange_Bool && !Competition.LockObject_Bool)
        {
            Competition.CompetitionData[Data].m_bSigned = false;
            if (MainConnet.m_Scence == MainConnet.SecnceType.Baccarat_Lobby)
                Competition.DataChange_Bool = true;
        }
        Debug.Log("比賽流局");
    }

    //更新報名人數
    public void RcvChangePlayerData(byte[] byarData)
    {
        CPACK_RACE_UpdatePlayerCnt Data = GameConnet.m_oGameClient.DoDeSerialize<CPACK_RACE_UpdatePlayerCnt>(byarData);

        //if (!Competition.DataChange_Bool && !Competition.LockObject_Bool)
        //{
        //    Competition.CompetitionData[Data.m_usEventID].m_uiNowPlayerCnt = Data.m_uiPlayerCnt;

        //    Competition.ListObject[Data.m_usEventID].GetComponent<CPACK_RACE_EventData>().m_uiNowPlayerCnt = Data.m_uiPlayerCnt;
        //    if (MainConnet.m_Scence == MainConnet.SecnceType.Baccarat_Lobby)
        //        Competition.DataChange_Bool = true;
        //}
        lock (Competition.CompetitionLockObject)
        {
            Competition.SaveRacePlayerData.Add(Data.m_usEventID,Data);
            //Competition.CompetitionData[Data.m_usEventID].m_uiNowPlayerCnt = Data.m_uiPlayerCnt;
            //if (MainConnet.m_Scence == MainConnet.SecnceType.Baccarat_Lobby)
            //    Competition.DataChange_Bool = true;
        }

        //  Competition.ListObject[Data.m_usEventID].GetComponent<RaceData>().People_Label.text = Data.m_uiPlayerCnt + "人 / " + Competition.CompetitionData[Data.m_usEventID].m_uiMaxPlayerCnt + "人";
        Debug.Log("更新報名人數");
    }
}
