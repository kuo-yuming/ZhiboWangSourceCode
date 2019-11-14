using UnityEngine;
using System.Collections;
using GameCore;
using System;
using GameCore.Machine;
using GameCore;
using GameCore.Manager.Common;
using GameCore.Manager.SicBo;
using System.Collections.Generic;

public class SicBoManager : MonoBehaviour
{
    public static bool LoadGameEnd = false;
    public static bool EnterMachine = false;
    public static CPACK_TMGameConfig m_MachineBuyInConfig = null;       //機台設定資料
    public static CPACK_SicBo_GameConfig m_GameConfig = null;       //遊戲設定資料
    public static Dictionary<uint, CPACK_TMachineData> m_MachineDatas = null;   //機台資料
    public static Dictionary<uint, byte> m_MachineTableArea = null;   //每個桌檯的分區編號 <桌檯ID, 分區編號>
    public static CPACK_TMachineMemberList MachineMemberList = new CPACK_TMachineMemberList();  //該機台 成員名單
    public static CPACK_SicBo_MachineInfo MachineInfo = new CPACK_SicBo_MachineInfo();  //該機台 詳細資料
    public static CPACK_SicBo_NotifyTableInfo NotifyTableInfo = new CPACK_SicBo_NotifyTableInfo();//該檯桌 戰績資訊
    public static CPACK_TMachineEnter PlayerBuyInData = new CPACK_TMachineEnter();  //BuyIn 詳細資料
    public static CPACK_SicBo_NotifyBet TableBetReslut = new CPACK_SicBo_NotifyBet();   //更新各區金額    
    public static CPACK_SicBo_NotifyAward NoitfyAwardData = new CPACK_SicBo_NotifyAward();  //得獎結果
    public static CPACK_SicBo_HundredAwardRec RecentHundredData = new CPACK_SicBo_HundredAwardRec();//近百局拉獎記錄
    public static bool SetTableType; //進入遊戲先執行桌檯背景與籌碼設定
    public static ushort[] Table_usOdds = new ushort[24];
    public static string NobetRound = "";
    public static byte NowGroup = 0;    //設定投注為大底或小底
    public static Dictionary<ushort, List<uint>> m_MachineClass = new Dictionary<ushort, List<uint>>();   //<桌檯分區ID, 各分區桌檯列表>
    public static List<uint> MachineList = new List<uint>();
    public static bool BackLobbySetting = false;
    public static uint AutoBuyInMoney = 0;
    public static bool CheckMachineClass = false;
    public static bool BuyInFrist = false;      //是否為BuyIn的首輪
    private bool SicBoLobbyTrusteeship = false; //SicBoLobby託管
    private bool RecentHundredTrusteeship = false; //百局名單託管

    // Update is called once per frame
    void Update()
    {
        if (BackLobbySetting && SicBoLobby.Inst != null)
        {
            BackLobbySetting = false;
            DoBackLobbySetting();
        }

        if (SicBoLobby.Inst != null && SicBoLobbyTrusteeship)
        {
            SicBoLobbyTrusteeship = false;
            SicBoLobby.Inst.LobbyStart = true;

        }

        if (SicBoGameMain.Inst != null && SicBoGameMain.Inst.RecentHundred != null && RecentHundredTrusteeship)
        {
            RecentHundredTrusteeship = false;
            SicBoGameMain.Inst.RecentHundred.ServerHundredUpdate = true;
        }
    }

    public void Reseat()
    {
        BuyInFrist = false;
        NowGroup = 0;
    }

    public void DoBackLobbySetting()
    {   //回到機台畫面的設定
        SicBoLobby.Inst.ClickAnteLow(); //按下小底台區
        MachineInfo = new CPACK_SicBo_MachineInfo();  //清空機台詳細資料
    }

    public void OnRcvSicBoData(uint uiPackID, byte[] byarData)
    {
        Debug.Log(string.Format("OnRcvSicBoFrameData. PackID={0}", uiPackID));

        switch (uiPackID)
        {
            case (uint)ENUM_COMMON_PACKID_GC.G2C_TMachine_NotifyGameConfig:
                RcvMachineData(byarData);   // GS->UC 登入時通知遊戲相關設定.  CPACK_TMGameConfig
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_TMachine_NotifyMachineList:
                RcvMachineList(byarData);   // GS->UC 通知各檯桌資料的列表. CPACK_TMachineDataList
                break;
            case (uint)ENUM_SICBO_PACKID_GC.G2C_NotifyGameConfig:
                RcvGameConfigData(byarData);// GS->UC 通知骰寶特有的遊戲相關設定.    CPACK_SicBo_GameConfig
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_TMachine_NotifyMachineData:
                RcvUpdateMachineList(byarData); //通知更新某檯桌的資料    CPACK_TMachineData
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_TMachine_RplyMachineMemberList:    //回覆某檯桌的成員列表    CPACK_TMachineMemberList    (依資料量,有可能收到多包)
            case (uint)ENUM_SICBO_PACKID_GC.G2C_Machine_NotifyMachineInfo:          //通知某幾台的機台資訊.   CPACK_SicBo_MachineInfo
                RcvMachineInfo(uiPackID, byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_TMachine_NotifyStartGame:
                RcvBuyInSuccess(byarData);  //通知 buyin成功,可進入遊戲.  CPACK_TMachineEnter
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_TMachine_RplyBuyinResult:
                RcvBuyInFail(byarData); //回覆 buyin結果. int   (失敗時才回傳, 參閱ENUM_COMMON_ERROR_CODE) 
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_TMachine_AutoSelectResult:
                RcvAutoBuyIn(byarData); //自動配位結果     CPACK_TMachineAutoSelectResult
                break;
            case (uint)ENUM_SICBO_PACKID_GC.G2C_Game_NotifyTableInfo:
                NotifyTableData(byarData); //通知此機台的遊戲資訊 CPACK_SicBo_NotifyTableInfo
                break;
            case (uint)ENUM_SICBO_PACKID_GC.G2C_Game_HundredAwardRec:
                HundredAwardData(byarData); //通知近百局開獎記務     CPACK_SicBo_HundredAwardRec
                break;
            case (uint)ENUM_SICBO_PACKID_GC.G2C_Game_NotifyState:
                NotifyStateData(byarData);  //通知更新某機台的狀態階段.   CPACK_SicBo_UpdateTbleState  
                break;
            case (uint)ENUM_SICBO_PACKID_GC.G2C_Game_NotifyBet:
                TableCashChange(byarData);  //通知 更新某區域的押金變動     CPACK_SicBo_NotifyBet
                break;
            case (uint)ENUM_SICBO_PACKID_GC.G2C_Game_NotifyAward:
                SicBoDataGet(byarData); //通知 開獎結果       CPACK_SicBo_NotifyAward
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_TMachine_KickToLobby:
                PlayerOut(byarData);
                break;

            default:
                Debug.Log(string.Format("{0}:Unknown packid={1}", DateTime.Now, uiPackID));
                break;
        }
        Debug.Log(string.Format("OnRcvSicBoFrameData. PackID={0} end", uiPackID));
    }
    //---------------------------------------------------------------------
    // 收到檯桌式機台機制
    public void RcvMachineData(byte[] byarData)
    {
        m_MachineDatas = new Dictionary<uint, CPACK_TMachineData>();
        m_MachineTableArea = new Dictionary<uint, byte>();
        m_MachineBuyInConfig = GameConnet.m_oGameClient.DoDeSerialize<CPACK_TMGameConfig>(byarData);
        MainGame_Control.NoBetRound = m_MachineBuyInConfig.m_byChkUnbetRound;
        Debug.Log("收到檯桌式機台機制 : " + "等級多少以上才能Buyin : " + m_MachineBuyInConfig.m_usBuyinLv);

        NobetRound = m_MachineBuyInConfig.m_byChkUnbetRound.ToString();
        foreach (var item in m_MachineBuyInConfig.m_dicTableGroupSet)
        {   //分區數量做設定
            Debug.Log("桌群ID: " + item.Value.m_byGroupID + " 分區標籤名稱 : " + item.Value.m_strTagName + "  起始桌號 : " + item.Value.m_uiStartTableID + "  末尾桌號 : " + item.Value.m_uiEndTableID);
            if (NowGroup == 0) //如果目前分區是初始值 設定目前分區
                NowGroup = item.Value.m_byGroupID;
            for (uint i = item.Value.m_uiStartTableID; i <= item.Value.m_uiEndTableID; i++)
            {   //根據各分區起末號 設定各機台的ID跟人數            
                if (!m_MachineDatas.ContainsKey(i))
                {   //m_MachineDatas 如果沒有找到 加入桌檯資料
                    CPACK_TMachineData Data = new CPACK_TMachineData();
                    Data.m_uiTID = i;
                    Data.m_usMemberCnt = 0;
                    m_MachineDatas.Add(i, Data);
                }
                if (!m_MachineTableArea.ContainsKey(i)) //根據桌檯ID給予分區編號
                    m_MachineTableArea.Add(i, item.Value.m_byGroupID);
            }
        }
        CheckMachineClass = true;
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
                    Debug.Log("收到更新機台人數 : 未知的機台ID");
                    continue;
                }
            }
        }

        if (SicBoLobby.Inst == null)    //SicBoLobby.Inst = null 開啟託管
            SicBoLobbyTrusteeship = true;
        else  //收到機台資料  SicBoLobby.Inst.LobbyStart = true
            SicBoLobby.Inst.LobbyStart = true;
    }
    //---------------------------------------------------------------------
    // 收到遊戲資料資料
    public void RcvGameConfigData(byte[] byarData)
    {
        for (int i = 0; i < 24; i++)
            Table_usOdds[i] = 0;
        m_GameConfig = GameConnet.m_oGameClient.DoDeSerialize<CPACK_SicBo_GameConfig>(byarData);
        foreach (var item in m_GameConfig.m_dicAreaOdds)
        {
            Table_usOdds[item.Key] = (ushort)(item.Value.m_usOdds + 1);
            Debug.Log("區塊: " + item.Key + "賠率: " + Table_usOdds[item.Key]);
        }
        Debug.Log("收到各區塊賠率表資料");
    }
    //---------------------------------------------------------------------
    // 收到更新機台資料
    public void RcvUpdateMachineList(byte[] byarData)
    {
        CPACK_TMachineData MachineData = GameConnet.m_oGameClient.DoDeSerialize<CPACK_TMachineData>(byarData);
        m_MachineDatas[MachineData.m_uiTID] = MachineData;
        if (MachineData.m_uiTID == GameConnet.m_TMachineBuyInGameData.m_uiTID)
            SicBoGameMain.Inst.ButtonControl.UpdatePlayersCnt = true;   //如果收到更新人數 且 是自己的機台 則更新顯示
        Debug.Log("收到更新機台 : " + MachineData.m_uiTID + " 資料");
    }
    //---------------------------------------------------------------------
    // 收到更新機台資料
    public void RcvMachineInfo(uint PackID, byte[] byarData)
    {
        if (PackID == (uint)ENUM_COMMON_PACKID_GC.G2C_TMachine_RplyMachineMemberList)
        {   // GS->UC 回覆某檯桌的成員列表    CPACK_TMachineMemberList    (依資料量,有可能收到多包)
            CPACK_TMachineMemberList Data = GameConnet.m_oGameClient.DoDeSerialize<CPACK_TMachineMemberList>(byarData);
            if (MachineMemberList.m_bEnd)
            {
                MachineMemberList = Data;
                if (Data.m_bEnd)
                {
                    MachineMemberList.m_uiTID = Data.m_uiTID;
                    SicBoLobby.Inst.NameList.ChangeNameList = true;
                    MachineMemberList.m_bEnd = true;
                }
            }
            else
            {
                foreach (var item in Data.m_listMember)
                {
                    MachineMemberList.m_listMember.Add(item);
                }
                if (Data.m_bEnd)
                {
                    MachineMemberList.m_uiTID = Data.m_uiTID;
                    SicBoLobby.Inst.NameList.ChangeNameList = true;
                    MachineMemberList.m_bEnd = true;
                }
            }
            Debug.Log("收到更新機台 : " + MachineMemberList.m_uiTID + " 成員資料");

        }
        if (PackID == (uint)ENUM_SICBO_PACKID_GC.G2C_Machine_NotifyMachineInfo)
        {   // GS->UC 通知某機台的機台資訊
            MachineInfo = GameConnet.m_oGameClient.DoDeSerialize<CPACK_SicBo_MachineInfo>(byarData);
            SicBoLobby.Inst.TableInfo.ChangeInfo = true;
            Debug.Log("收到更新機台 : " + MachineInfo.m_uiTID + " 資料");
        }
    }
    //---------------------------------------------------------------------
    // 收到玩家成功BuyIn
    public void RcvBuyInSuccess(byte[] byarData)
    {
        SetTableType = true;    //進入遊戲先執行桌檯背景與籌碼設定
        GameConnet.m_TMachineBuyInGameData = GameConnet.m_oGameClient.DoDeSerialize<CPACK_TMachineEnter>(byarData);
        GameConnet.m_BuyInMoney = GameConnet.m_TMachineBuyInGameData.m_uiGameMoney;
        GameConnet.m_NowBuyInMachineID = GameConnet.m_TMachineBuyInGameData.m_uiTID;
        NowGroup = m_MachineTableArea[GameConnet.m_TMachineBuyInGameData.m_uiTID];  //設定 大底台或小底台
        BuyInFrist = true;  //是否為BuyIn的首輪
        GameConnet.LogIn_GameSuccess = true;
        Debug.Log("成功BUYIN: " + GameConnet.m_TMachineBuyInGameData.m_uiGameMoney);
    }
    //---------------------------------------------------------------------
    public void RcvBuyInFail(byte[] byarData)
    {
        AllScenceLoad.LoadScence = false;
        Message_Control.OpenMessage = true;
        Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
        Message_Control.MessageStatus = Message_Control.MessageStatu.Buyin_LVNotEnough;
    }
    // 收到玩家自動配位
    public void RcvAutoBuyIn(byte[] byarData)
    {
        CPACK_TMachineAutoSelectResult PlayerAutoBuyIn = GameConnet.m_oGameClient.DoDeSerialize<CPACK_TMachineAutoSelectResult>(byarData);
        if (PlayerAutoBuyIn.m_iResultCode == (int)ENUM_COMMON_ERROR_CODE.Success)
        {
            CPACK_TMachineBuyin m_BuyInMoney = new CPACK_TMachineBuyin();
            m_BuyInMoney.m_uiTID = PlayerAutoBuyIn.m_uiTID;
            m_BuyInMoney.m_uiBuyinMoney = AutoBuyInMoney;
            Debug.Log("要BUYIN的機台 : " + m_BuyInMoney.m_uiTID + " 金錢 : " + m_BuyInMoney.m_uiBuyinMoney);
            GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.SicBo, (uint)ENUM_COMMON_PACKID_GC.C2G_TMachine_ReqBuyin, GameConnet.m_oGameClient.DoSerialize<CPACK_TMachineBuyin>(m_BuyInMoney));
        }
    }
    //---------------------------------------------------------------------
    //檯桌的戰績
    public void NotifyTableData(byte[] byarData)
    {
        CPACK_SicBo_NotifyTableInfo Data = GameConnet.m_oGameClient.DoDeSerialize<CPACK_SicBo_NotifyTableInfo>(byarData);
        //設定未開圍骰與四枚局數
        SicBoGameMain.Inst.ButtonControl.LotteryRecord[0] = Data.m_usNotSameNum;
        SicBoGameMain.Inst.ButtonControl.LotteryRecord[1] = Data.m_usNoQuadrupleRound;
        //更新顯示
        SicBoGameMain.Inst.ButtonControl.UpdateNowAnyTriple = true;
        SicBoGameMain.Inst.ButtonControl.UpdateNowAnyQuadruple = true;
    }

    //檯桌的近百局拉獎記錄
    public void HundredAwardData(byte[] byarData)
    {
        RecentHundredData = GameConnet.m_oGameClient.DoDeSerialize<CPACK_SicBo_HundredAwardRec>(byarData);
        if (RecentHundredData != null)
        {
            if (SicBoGameMain.Inst == null || SicBoGameMain.Inst.RecentHundred == null)
                RecentHundredTrusteeship = true;    //開啟託管
            else
                SicBoGameMain.Inst.RecentHundred.ServerHundredUpdate = true;
        }
    }

    //更新此檯桌的狀態
    public void NotifyStateData(byte[] byarData)
    {
        CPACK_SicBo_UpdateTbleState TableStateData = GameConnet.m_oGameClient.DoDeSerialize<CPACK_SicBo_UpdateTbleState>(byarData);
        switch (TableStateData.m_enumState)
        {
            case ENUM_SICBO_TABLE_STATE.NewRound:
                SicBoGameMain.Inst.NowStatus = SicBoGameMain.SicBoGameStatus.NewRound;
                break;
            case ENUM_SICBO_TABLE_STATE.WaitBet:
                SicBoGameMain.Inst.NowStatus = SicBoGameMain.SicBoGameStatus.WaitBet;
                break;
            case ENUM_SICBO_TABLE_STATE.StopBet:
                SicBoGameMain.Inst.NowStatus = SicBoGameMain.SicBoGameStatus.StopBet;
                break;
        }

        SicBoGameMain.Inst.StatusMSec = TableStateData.m_uiWaitMSec;    //狀態秒數
        SicBoGameMain.Inst.GetStatusUpdate = true;  //GameMain更新狀態
        Debug.Log("現在桌子狀態: " + TableStateData.m_enumState + "//現在桌子時間: " + TableStateData.m_uiWaitMSec);
    }

    //更新台桌各區金額
    public void TableCashChange(byte[] byarData)
    {
        TableBetReslut = new CPACK_SicBo_NotifyBet();
        TableBetReslut = GameConnet.m_oGameClient.DoDeSerialize<CPACK_SicBo_NotifyBet>(byarData);
        if (TableBetReslut.m_enumResult != ENUM_COMMON_ERROR_CODE.Success)
        {   //如果押注失敗
            Message_Control.OpenMessage = true;
            if (TableBetReslut.m_enumResult == ENUM_COMMON_ERROR_CODE.SicBo_MoneyNotEnough)
            {   //金額不足
                Message_Control.MessageStatus = Message_Control.MessageStatu.NoBetMoney;
                SicBoGameMain.Inst.ButtonControl.NowCanAuto = false;    //解除自動模式
            }
            else if (TableBetReslut.m_enumResult == ENUM_COMMON_ERROR_CODE.SicBo_OverBetLimit)
            {   //超過押注上限
                Message_Control.MessageStatus = Message_Control.MessageStatu.MaxBetOut;
            }
        }
        else
        {
            lock (SicBoGameMain.Inst.BetAreaControl.BetResultLock)
            {
                SicBoGameMain.Inst.BetAreaControl.BetData_Hold.Add(TableBetReslut);
            }
        }
        Debug.Log("取得押注金額資料");
        Debug.Log("結果: " + TableBetReslut.m_enumResult + "//投注區塊: " + TableBetReslut.m_byAreaID + "//次要區塊: " + TableBetReslut.m_byOffset + "//自己的押金: " + TableBetReslut.m_uiPlayerBetMoney + "//總押金: " + TableBetReslut.m_uiAllBetMoney);
    }

    //取得得獎資料
    public void SicBoDataGet(byte[] byarData)
    {
        Debug.Log("取得得獎資料");
        NoitfyAwardData = GameConnet.m_oGameClient.DoDeSerialize<CPACK_SicBo_NotifyAward>(byarData);        
        Debug.Log("第一顆骰子: " + NoitfyAwardData.m_byarDiceNumber[0] + "第二顆骰子: " + NoitfyAwardData.m_byarDiceNumber[1] + "第三顆骰子: " + NoitfyAwardData.m_byarDiceNumber[2] + "第四顆骰子: " + NoitfyAwardData.m_byarDiceNumber[3]);
        Debug.Log("第一顆骰子位子: " + NoitfyAwardData.m_byarDiceID[0] + "第二顆骰子位子: " + NoitfyAwardData.m_byarDiceID[1] + "第三顆骰子位子: " + NoitfyAwardData.m_byarDiceID[2] + "第四顆骰子位子: " + NoitfyAwardData.m_byarDiceID[3]);
        Debug.Log("第一顆骰子停止時間: " + NoitfyAwardData.m_uiarActionSec[0] + "第二顆骰子停止時間: " + NoitfyAwardData.m_uiarActionSec[1] + "第三顆骰子停止時間: " + NoitfyAwardData.m_uiarActionSec[2] + "第四顆骰子停止時間: " + NoitfyAwardData.m_uiarActionSec[3]);
        Debug.Log("更新後金額: " + NoitfyAwardData.m_ui64GameMoney);

        for (int i = 0; i < 3; i++)
        {   //輪盤 1~3 的參數
            SicBoGameMain.Inst.RouletteTurnControl.RouletteDiceNumber[i] = NoitfyAwardData.m_byarDiceNumber[i]; //給予骰子點數
            SicBoGameMain.Inst.RouletteTurnControl.RouletteStopTime[i] = NoitfyAwardData.m_uiarActionSec[i] / 1000; //給予停止時間
            SicBoGameMain.Inst.RouletteTurnControl.RouletteDiceID[i] = NoitfyAwardData.m_byarDiceID[i]; //給予骰子ID
        }
        //輪盤4的參數
        SicBoGameMain.Inst.RouletteTurnControl.Roulette4DiceNumber = NoitfyAwardData.m_byarDiceNumber[3];   //給予骰子點數
        SicBoGameMain.Inst.RouletteTurnControl.Roulette4StopTime = NoitfyAwardData.m_uiarActionSec[3] / 1000;   //給予停止時間
        SicBoGameMain.Inst.RouletteTurnControl.Roulette4DiceID = NoitfyAwardData.m_byarDiceID[3];   //給予骰子ID        //黃金豹預告時間
        SicBoGameMain.Inst.RouletteTurnControl.GoldLeopardNoticeTime = SicBoGameMain.Inst.RouletteTurnControl.Roulette4StopTime - 2.0f;

        bool Triple = false;
        bool Quadruple = false;
        foreach (var item in NoitfyAwardData.m_listAwardAreaID)
        {
            Debug.Log("得獎桌子編號:" + item.m_byAwardAreaID);
            Debug.Log("得獎區塊的編號:" + item.m_byAwardNumber);
            Triple = (item.m_byAwardAreaID == (byte)ENUM_SicBo_AWARD_AREA.AnyTriple);   //如果有開圍骰 = true
            Quadruple = (item.m_byAwardAreaID == (byte)ENUM_SicBo_AWARD_AREA.AnyQuadruple); //如果有開四枚 = true
        }
        //如果沒有開圍骰或四枚  更新圍骰或四枚局數
        SicBoGameMain.Inst.Update3and4[0] = !Triple;
        SicBoGameMain.Inst.Update3and4[1] = !Quadruple;
    }

    //把不押注該死的玩家踢回大廳
    public void PlayerOut(byte[] byarData)
    {
        Message_Control.OpenMessage = true;
        Message_Control.MessageStatus = Message_Control.MessageStatu.PlayerOut;
    }
}