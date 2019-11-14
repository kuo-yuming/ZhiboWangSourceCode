using UnityEngine;
using System.Collections;
using GameCore;
using System;
using GameCore.Machine;
using GameCore.Manager.IceAge;
using GameCore.Manager.Common;
using System.Collections.Generic;

public class IceAgeManager : MonoBehaviour
{
    public static CPACK_IceAge_GameConfig m_GameConfig = null;
    private static Dictionary<int, bool> PageCheck = new Dictionary<int, bool>();   //確認有無要過資料
    public static CPACK_IceAge_MachineInfo m_MachineInfo = new CPACK_IceAge_MachineInfo();
    public static List<CPACK_PMachineAwardRecord> M_AwardRecord = new List<CPACK_PMachineAwardRecord>();
    public static CPACK_PMachineAwardRecordList M_AwardPacket = new CPACK_PMachineAwardRecordList();
    public static List<CPACK_PMachineAwardRecord> O_AwardRecord = new List<CPACK_PMachineAwardRecord>();
    public static CPACK_PMachineAwardRecordList O_AwardPacket = new CPACK_PMachineAwardRecordList();
    public static int MaxPage = 0; //最大頁數
    public static int NowPage = 0; //現在頁數
    public static bool LoadGameEnd = false;
    public static bool EnterMachine = false;
    public static CPACK_IceAge_BetResult m_BetResult = new CPACK_IceAge_BetResult();
    public static bool GetAward = false;
    public static CPACK_IceAge_DoubleResult m_RplyDoubleResult;
    public static bool GetDoubleRestle = false;
    public static CPACK_IceAge_BonusResult m_BonusResult;
    public static bool BounsGameDateGet = false;
    public static bool IsBonus = false;
    public static byte BonusNumber = 0;
    public static byte BonusGameTimes = 0;
    public static uint JPCnt = 200000;
    public static bool GetNewJP = false;
    bool M_AwardU2C = false;
    float O_AwardTimer = 0.0f;
    public static O_AwardGetData O_AwardStatus = O_AwardGetData.Idle;

    public enum O_AwardGetData
    {
        Idle,
        GetingData,
        GetDataEnd,
        CantGetData,

    }

    // Use this for initialization
    void Start()
    {
        MaxPage = 0; //最大頁數
        NowPage = 0; //現在頁數

    }

    // Update is called once per frame
    void Update()
    {
        HallWinnerTimer();
    }

    void HallWinnerTimer()
    {
        if (O_AwardStatus == O_AwardGetData.GetDataEnd)
        {
            O_AwardTimer += Time.deltaTime;

            if (O_AwardTimer >= ((int)(CCommonDef._DEF_LOBBY_AWARD_REQLOCK_MINUTE) * 60))
            {
                O_AwardStatus = O_AwardGetData.Idle;
                O_AwardTimer = 0.0f;
            }

        }

    }
    public void Reseat()
    {
        MaxPage = 0; //最大頁數
        NowPage = 0; //現在頁數
        PageCheck.Clear();
        m_MachineInfo = new CPACK_IceAge_MachineInfo();
        LoadGameEnd = false;

    }
    public void OnRcvIceAgeData(uint uiPackID, byte[] byarData)
    {
        Debug.Log(string.Format("OnRcvIceAgeFrameData. PackID={0}", uiPackID));

        switch (uiPackID)
        {
            case (uint)ENUM_COMMON_PACKID_GC.G2C_PMachine_NotifyGameConfig:
                RcvPMGameConfig(byarData);
                break;
            case (uint)ENUM_ICEAGE_PACKID_GC.G2C_NotifyGameConfig:
                RcvGameConfig(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_PMachine_RplyBuyinResult:
                AllScenceLoad.LoadScence = false;
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_PMachine_NotifyKeepMID:
                RcvKeepMachine(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_PMachine_NotifyMachineList:
                RcvPMachinesData(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_PMachine_NotifyMachineData:
                RcvOnePMachineData(byarData);
                break;
            case (uint)ENUM_ICEAGE_PACKID_GC.G2C_Machine_NotifyMachineInfo:
                RcvMachineInfo(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_PMachine_AutoSelectResult:
                RcvRadomMachine(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_PMachine_NotifyStartGame:
                RcvBuyInOk(byarData);
                break;
            case (uint)ENUM_ICEAGE_PACKID_GC.G2C_Bet_RplyBetResult:
                RcvBetAns(byarData);    //收到押注拉獎結果
                break;
            case (uint)ENUM_ICEAGE_PACKID_GC.G2C_Bet_RplyDoubleResult:
                RcvDoubleAns(byarData); //收到比倍結果
                break;
            case (uint)ENUM_ICEAGE_PACKID_GC.G2C_Bet_RplyShootResult:
                RcvBounsGame1Ans(byarData); //收到射擊拉獎的結果
                break;
            case (uint)ENUM_ICEAGE_PACKID_GC.G2C_Bet_RplyKnockEggResult:
                RcvBounsGame2Ans(byarData); //收到敲蛋拉獎的結果
                break;
            case (uint)ENUM_ICEAGE_PACKID_GC.G2C_Game_UpdateJPVal:
                RcvJPNumber(byarData);  //更新水庫JP
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_PMachine_RplyPersonalAwardRec:
                RcvM_AwardRecord(byarData); //收到個人大獎
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_PMachine_RplyLobbyAwardRec:
                RcvO_AwardRecord(byarData); //收到本廳大獎
                break;
            default:
                Debug.Log(string.Format("{0}:Unknown packid={1}", DateTime.Now, uiPackID));
                break;
        }
        Debug.Log(string.Format("OnRcvIceAgeFrameData. PackID={0} end", uiPackID));
    }

    // 收到機台設定資料
    void RcvPMGameConfig(byte[] byarData)
    {
        GameConnet.m_PMachineConfig = GameConnet.m_oGameClient.DoDeSerialize<CPACK_PMGameConfig>(byarData);
        Debug.Log(string.Format("{0}:收到機台設定.最大機台數 = {1}, 每頁機台數={2}, BuyIn最大金額={3}, BuyIn最小金額={4}, BetMoney={5}", DateTime.Now
                          , GameConnet.m_PMachineConfig.m_uiMaxMachineCnt, GameConnet.m_PMachineConfig.m_uiPageMachineCnt, GameConnet.m_PMachineConfig.m_uiMaxBuyinMoney, GameConnet.m_PMachineConfig.m_uiMinBuyinMoney, GameConnet.m_PMachineConfig.m_usBetMoney));
        //Reseat();
        MachinePageCheck();
        GetMachineDataU2G();
        GetM_AwardRecord();
        LoadGameEnd = true;
    }
    //---------------------------
    // 設定PageCheck
    public void MachinePageCheck()
    {
        PageCheck.Clear();
        if ((GameConnet.m_PMachineConfig.m_uiMaxMachineCnt % GameConnet.m_PMachineConfig.m_uiPageMachineCnt) != 0)
        {
            for (int i = 0; i <= (GameConnet.m_PMachineConfig.m_uiMaxMachineCnt / GameConnet.m_PMachineConfig.m_uiPageMachineCnt); i++)
            {
                PageCheck.Add(i, false);
            }
        }
        else
        {
            for (int i = 0; i < (GameConnet.m_PMachineConfig.m_uiMaxMachineCnt / GameConnet.m_PMachineConfig.m_uiPageMachineCnt); i++)
            {
                PageCheck.Add(i, false);
            }

        }
        MaxPage = PageCheck.Count - 1;
    }
    //---------------------------------------------------------------------
    // 玩家要求機台資料
    public static void GetMachineDataU2G()
    {

        CPACK_GetPMachineList StartMachine_U2G = new CPACK_GetPMachineList();
        uint OnePageMax = GameConnet.m_PMachineConfig.m_uiPageMachineCnt;
        Debug.Log("當頁最大機台數 : " + OnePageMax);

        if (!PageCheck[NowPage])
        {
            Debug.Log("要求第" + NowPage + "頁機台資料");
            StartMachine_U2G.m_uiStartMID = (uint)((NowPage * OnePageMax) + 1);
            StartMachine_U2G.m_uiEndMID = (uint)((NowPage * OnePageMax) + OnePageMax);
            PageCheck[NowPage] = true;
            GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.IceAge, (uint)ENUM_COMMON_PACKID_GC.C2G_PMachine_GetMachineList,
                       GameConnet.m_oGameClient.DoSerialize<CPACK_GetPMachineList>(StartMachine_U2G));
        }
        else
        {
            Debug.Log("已有機台資料");
            return;
        }
    }
    //---------------------------------------------------------------------
    // 玩家翻頁 0:不處理  1:下一頁  2:上一頁
    public static void ChangeMachinePage(byte Type = 0)
    {
        if (Type == 1)
        {
            if (NowPage == MaxPage)
                NowPage = 0;
            else
                NowPage++;

        }
        else if (Type == 2)
        {
            if (NowPage == 0)
                NowPage = MaxPage;
            else
                NowPage--;
        }
        else
            return;
        GetMachineDataU2G();
    }
    //---------------------------------------------------------------------
    // 收到遊戲設定資料
    void RcvGameConfig(byte[] byarData)
    {
        m_GameConfig = GameConnet.m_oGameClient.DoDeSerialize<CPACK_IceAge_GameConfig>(byarData);
    }
    //---------------------------------------------------------------------
    // 玩家保留機台
    public void RcvKeepMachine(byte[] byarData)
    {
        GameConnet.m_uiKeepMID = GameConnet.m_oGameClient.DoDeSerialize<uint>(byarData);
        Debug.Log(string.Format("{0}:收到玩家保留機台, Keep MachineID={1}", DateTime.Now, GameConnet.m_uiKeepMID));

    }
    //---------------------------------------------------------------------
    // 收到一組機台資料-----加入機台資料
    public void RcvPMachinesData(byte[] byarData)
    {
        Debug.Log(DateTime.Now + "  Get MachineData...........");
        CPACK_PMachineDataList m_LocalMachineDatas = GameConnet.m_oGameClient.DoDeSerialize<CPACK_PMachineDataList>(byarData);
        foreach (CPACK_PMachineData Datas in m_LocalMachineDatas.m_listMachineData)
        {
            if (!GameConnet.m_PMachinesData.ContainsKey(Datas.m_uiMID))
            {
                GameConnet.m_PMachinesData.Add(Datas.m_uiMID, Datas);
                Debug.Log("Mach : " + GameConnet.m_PMachinesData[Datas.m_uiMID].m_uiMID + "  DBID : " + GameConnet.m_PMachinesData[Datas.m_uiMID].m_uiDBID);
            }
            else
            {
                GameConnet.m_PMachinesData[Datas.m_uiMID] = Datas;
            }
            if (GameConnet.m_PMachinesData[Datas.m_uiMID].m_uiDBID == MainConnet.m_PlayerData.m_uiDBID)
            {
                GameConnet.m_uiKeepMID = GameConnet.m_PMachinesData[Datas.m_uiMID].m_uiMID;
            }
        }
    }
    //---------------------------------------------------------------------
    // 收到一台機台更新資料
    public void RcvOnePMachineData(byte[] byarData)
    {
        CPACK_PMachineData OneMachineData = GameConnet.m_oGameClient.DoDeSerialize<CPACK_PMachineData>(byarData);
        //uint OnePageMax = m_MachineConfig.m_uiPageMachineCnt;
        if (!GameConnet.m_PMachinesData.ContainsKey(OneMachineData.m_uiMID))
        {
            GameConnet.m_PMachinesData.Add(OneMachineData.m_uiMID, OneMachineData);
            Debug.Log("Mach : " + GameConnet.m_PMachinesData[OneMachineData.m_uiMID].m_uiMID + "  DBID : " + GameConnet.m_PMachinesData[OneMachineData.m_uiMID].m_uiDBID
                    + "機台狀態 : " + GameConnet.m_PMachinesData[OneMachineData.m_uiMID].m_enumState);
        }
        else
        {
            GameConnet.m_PMachinesData[OneMachineData.m_uiMID] = OneMachineData;
            Debug.Log("Mach : " + GameConnet.m_PMachinesData[OneMachineData.m_uiMID].m_uiMID + "  DBID : " + GameConnet.m_PMachinesData[OneMachineData.m_uiMID].m_uiDBID);
        }

        if (GameConnet.m_PMachinesData[OneMachineData.m_uiMID].m_uiDBID == MainConnet.m_PlayerData.m_uiDBID && MainConnet.m_PlayerData.m_byVIPType != 0)
        {
            GameConnet.m_uiKeepMID = GameConnet.m_PMachinesData[OneMachineData.m_uiMID].m_uiMID;
        }
        if (GameConnet.m_PMachinesData[OneMachineData.m_uiMID].m_uiMID == GameConnet.m_uiKeepMID && GameConnet.m_PMachinesData[OneMachineData.m_uiMID].m_enumState == ENUM_PMACHINE_STATE.Idle)
        {
            GameConnet.m_uiKeepMID = 0;

        }

    }
    //---------------------------------------------------------------------
    // 收到一台機台詳細資料-----加入機台資料
    public void RcvMachineInfo(byte[] byarData)
    {
        m_MachineInfo = GameConnet.m_oGameClient.DoDeSerialize<CPACK_IceAge_MachineInfo>(byarData);
        Debug.Log("GetMachineInfo");
    }
    //---------------------------------------------------------------------
    // 收到個人大獎紀錄-----加入機台資料
    public void GetM_AwardRecord()
    {
        if (!M_AwardU2C)
        {
            GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.IceAge, (uint)ENUM_COMMON_PACKID_GC.C2G_PMachine_ReqPersonalAwardRec,
                                      null);
        }
    }
    //---------------------------------------------------------------------
    // 收到個人大獎紀錄-----加入機台資料
    public void RcvM_AwardRecord(byte[] byarData)
    {
        if (!M_AwardU2C)
        {
            M_AwardRecord.Clear();
        }
        M_AwardU2C = true;
        Debug.Log(DateTime.Now + " ; Get M_AwardRecord...........");
        M_AwardPacket = GameConnet.m_oGameClient.DoDeSerialize<CPACK_PMachineAwardRecordList>(byarData);
        foreach (CPACK_PMachineAwardRecord item in M_AwardPacket.m_listAwardRec)
        {
            M_AwardRecord.Add(item);
        }
        Debug.Log("M_AwardRecord..........." + M_AwardPacket.m_listAwardRec.Count);
        if (M_AwardPacket.m_bEnd)
        {
            Debug.Log(DateTime.Now + " : Get M_AwardRecord...........END");
        }

    }
    //---------------------------------------------------------------------
    // 收到本廳大獎紀錄-----加入機台資料
    public void RcvO_AwardRecord(byte[] byarData)
    {
        if (O_AwardStatus == O_AwardGetData.Idle)
        {
            O_AwardRecord.Clear();
            O_AwardStatus = O_AwardGetData.GetingData;
        }
        Debug.Log(DateTime.Now + " : Get O_AwardRecord...........");
        O_AwardPacket = GameConnet.m_oGameClient.DoDeSerialize<CPACK_PMachineAwardRecordList>(byarData);
        //O_AwardRecord.Clear ();
        foreach (CPACK_PMachineAwardRecord item in O_AwardPacket.m_listAwardRec)
        {

            O_AwardRecord.Add(item);
        }
        Debug.Log("O_AwardRecord..........." + O_AwardPacket.m_listAwardRec.Count);
        if (O_AwardPacket.m_bEnd)
        {
            Debug.Log(DateTime.Now + " : Get O_AwardRecord...........END");
            O_AwardStatus = O_AwardGetData.GetDataEnd;
        }
    }
    //---------------------------------------------------------------------
    // 收到自動配位的機台ID
    public void RcvRadomMachine(byte[] byarData)
    {
        Debug.Log(DateTime.Now + "Get RadomMachineID...................G2U");
        CPACK_PMachineAutoSelectResult RadomPack = GameConnet.m_oGameClient.DoDeSerialize<CPACK_PMachineAutoSelectResult>(byarData);
        if (RadomPack.m_iResultCode == 0)
        {
            BuyInGame(RadomPack.m_uiMID);
            Debug.Log("Get RadomMachineID...................ID : " + RadomPack.m_uiMID);
        }
        else if (RadomPack.m_iResultCode == 40)
        {
            if (GameConnet.m_uiKeepMID != 0)
            {
                Message_Control.OpenMessage = true;
                Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                Message_Control.MessageStatus = Message_Control.MessageStatu.Machine_NoneIdleMachineBacKToKeep;
            }
            else
            {
                Debug.Log(string.Format("{0}:無可用機台", DateTime.Now));
                Message_Control.OpenMessage = true;
                Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                Message_Control.MessageStatus = Message_Control.MessageStatu.Machine_NoneIdleMachine;
            }
        }
        Debug.Log("Get RadomMachineID...................END");

    }
    //---------------------------------------------------------------------
    // 收到BuyIn結果
    public void RcvBuyInOk(byte[] byarData)
    {
        Debug.Log(DateTime.Now + "BuyInGameOK");
        GameConnet.m_PMachineBuyInGameData = GameConnet.m_oGameClient.DoDeSerialize<CPACK_PMachineEnter>(byarData);
        GameConnet.m_BuyInMoney = GameConnet.m_PMachineBuyInGameData.m_uiGameMoney;
        GameConnet.m_NowBuyInMachineID = GameConnet.m_PMachineBuyInGameData.m_uiMID;
        GameConnet.LogIn_GameSuccess = true;
        EnterMachine = true;
    }
    //---------------------------------------------------------------------
    public static void BuyInGame(uint machineID)
    {
        if (MainConnet.m_PlayerData.m_ui64OwnMoney >= GameConnet.m_PMachineConfig.m_uiMinBuyinMoney)
        {
            AllScenceLoad.LoadScence = true;
            ulong BuyInMoney = MainConnet.m_PlayerData.m_ui64OwnMoney;
            if (BuyInMoney > GameConnet.m_PMachineConfig.m_uiMaxBuyinMoney)
                BuyInMoney = GameConnet.m_PMachineConfig.m_uiMaxBuyinMoney;
            CPACK_PMachineBuyin m_BuyInMoney = new CPACK_PMachineBuyin();
            m_BuyInMoney.m_uiMID = machineID;
            m_BuyInMoney.m_uiBuyinMoney = (uint)BuyInMoney;
            GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.IceAge, (uint)ENUM_COMMON_PACKID_GC.C2G_PMachine_ReqBuyin, GameConnet.m_oGameClient.DoSerialize<CPACK_PMachineBuyin>(m_BuyInMoney));
        }
        else
        {
            Message_Control.OpenMessage = true;
            Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
            Message_Control.MessageStatus = Message_Control.MessageStatu.Buyin_MoneyNotEnough;
        }
    }
    //---------------------------------------------------------------------

    // 收到壓注結果
    public void RcvBetAns(byte[] byarData)
    {
        m_BetResult = MainConnet.m_oMainClient.DoDeSerialize<CPACK_IceAge_BetResult>(byarData);
        IsBonus = false;
        BonusNumber = 0;
        foreach (var item in m_BetResult.m_dicLineAward)
        {
            if (item.Value == 27 || item.Value == 36)
            {
                IsBonus = true;   //進入Bonus
                BonusNumber = item.Value;   //設定編號 以利判斷
                BonusGameTimes = m_BetResult.m_byBonusRoundCnt; //BonusGame 遊戲次數
            }
            else if (item.Value == 37)  //JP
            {
                IsBonus = true;   //進入Bonus
                BonusNumber = item.Value;   //設定編號 以利判斷
            }
        }

        GetAward = true;
    }
    //---------------------------------------------------------------------

    // 收到比倍結果
    public void RcvDoubleAns(byte[] byarData)
    {
        m_RplyDoubleResult = MainConnet.m_oMainClient.DoDeSerialize<CPACK_IceAge_DoubleResult>(byarData);
        GetDoubleRestle = true;
    }
    //---------------------------------------------------------------------

    //收到射擊拉獎的結果
    public void RcvBounsGame1Ans(byte[] byarData)
    {
        m_BonusResult = MainConnet.m_oMainClient.DoDeSerialize<CPACK_IceAge_BonusResult>(byarData);
        BounsGameDateGet = true;
    }
    //---------------------------------------------------------------------

    //收到敲蛋拉獎的結果
    void RcvBounsGame2Ans(byte[] byarData)
    {
        m_BonusResult = MainConnet.m_oMainClient.DoDeSerialize<CPACK_IceAge_BonusResult>(byarData);
        BounsGameDateGet = true;
    }
    //---------------------------------------------------------------------

    //更新水庫JP
    public void RcvJPNumber(byte[] byarData)
    {
        JPCnt = MainConnet.m_oMainClient.DoDeSerialize<UInt32>(byarData);
        GetNewJP = true;
    }
    //---------------------------------------------------------------------
}
