using UnityEngine;
using System.Collections;
using GameCore.Manager.Baccarat;
using GameEnum;
using GameCore;
using GameCore.Manager.Common;

public class MainGame_Control : MonoBehaviour {
    public static bool EndShow = false;//結束表演
    public static bool SnatchCardGame = false;//是否開啟搶排遊戲
    public static ENUM_STOPMODE_STATE StopModeState = ENUM_STOPMODE_STATE.WaitStop;
    public static byte[] WinArea = new byte[5];
    public static bool FinallDataGetBool = false;
    public static bool BaccactFCBetBool = false;

    public UILabel[] HistroyDataLabel = new UILabel[6];
    public static byte NowGameState = 0;
    public static bool FirstGameBool = true;

    public static ENUM_BACCARAT_AWARD SaveWin = ENUM_BACCARAT_AWARD.WinBanker;
    public static ENUM_BACCARAT_AWARD LastWin = ENUM_BACCARAT_AWARD.WinBanker;
    public static byte BankerWinPoint = 0;
    public static byte PlayerWinPoint = 0;
    public static byte DrawWinPoint = 0;
    public static byte BankerPairWinPoint = 0;
    public static byte PlayerPairWinPoint = 0;
    public static ushort LeftOverCardPoint = 0;
    public static bool SaveBankerPair = false;
    public static bool SavePlayerPair = false;
    public static bool GetNormalWinDataBool = false;
    public static bool GetLastWinDataBool = false;
    
    public GameObject ShuffleCardObject;
    public static bool ShuffleCardBool = false;

    public static bool AutoAndInfoClickBool = false;
    public GameObject WaitRoundObject;
    public GameObject EndObject;
    public UILabel MachineNumber_Label;
    public UISprite[] Background_Sprite;
    public GameObject NormalClearButton;
    public UILabel FCCard_Label;
    public GameObject EndTime_Object;
    public UILabel EndTime_Label;
    public GameObject BetButton_Object;
    public GameObject Idle_Object;
    float Timer = 0;
    float DelayTimer = 0;
    public static byte NoBetRound = 0;
    public static bool FourCardPlay_Bool = false; //競標模式開關
    public GameObject[] InfoButton;
    public UILabel PeopleNumber;
    bool InitData_Bool = false;
    bool StartDataGet = false;

    float WaitPlayerTimer = 0;
    public static int WaitTotalPlayerTimer = 0;
    public UILabel WaitPlayerTimer_Lable;
    // Use this for initialization
    void Start () {
        AllScenceLoad.LoadScence = false;
        FourCardPlay_Bool = VersionDef.FourCardPlay;
        if (MainConnet.m_PlayerData.m_byVIPType == (byte)BaseAttr.ENUM_VIP_TYPE.Rookie)
        {
            MainGame_Control.BaccactFCBetBool = false;
        }
        else
        {
            MainGame_Control.BaccactFCBetBool = true;
        }
        FinallDataGetBool = false;
        
        MachineNumber_Label.text = GameConnet.m_TMachineBuyInGameData.m_uiTID.ToString();

        if (StopModeState == ENUM_STOPMODE_STATE.FourCardMoneyShow){
            NormalClearButton.SetActive(false);
        }
        else 
        {
            NormalClearButton.SetActive(true);
        }

        if (!Competition.RaceGame_Bool)
        {
            InfoButton[0].SetActive(true);
            InfoButton[1].SetActive(false);
            if (BaccaratManager.m_MachineTableArea[GameConnet.m_TMachineBuyInGameData.m_uiTID] == 2)
            {
                Background_Sprite[0].spriteName = "bg_bgldesk_a2";
                Background_Sprite[1].spriteName = "bg_bgldealerback_2";
                Background_Sprite[2].spriteName = "bg_bgldealertop_2";
            }
            else
            {
                Background_Sprite[0].spriteName = "bg_bgldesk_a1";
                Background_Sprite[1].spriteName = "bg_bgldealerback_1";
                Background_Sprite[2].spriteName = "bg_bgldealertop_1";
            }
        }
        else
        {
            InfoButton[0].SetActive(false);
            InfoButton[1].SetActive(true);
            Background_Sprite[0].spriteName = "bg_bgldesk_a2";
            Background_Sprite[1].spriteName = "bg_bgldealerback_2";
            Background_Sprite[2].spriteName = "bg_bgldealertop_2";
        }

        for (int i = 0; i < 5; i++)
        {
            WinArea[i] = 0;
        }
        EndObject.SetActive(false);
        WaitRoundObject.SetActive(false);
        ShuffleCardObject.SetActive(false);
        FirstGameBool = true;
        GetLastWinDataBool = false;
        GetNormalWinDataBool = false;
        AutoAndInfoClickBool = false;
        StartDataGet = false;
    }

    void Update()
    {
        if (!StartDataGet)
        {
            GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Baccarat, (uint)ENUM_COMMON_PACKID_GC.C2G_TMachine_GameReady, null);
            StartDataGet = true;
        }
        LabelVoid();
        FCCard_Label.text = FourCardHistory_Control.FCLeftOverCardPoint.ToString();
        PeopleNumber.text = BaccaratManager.m_MachineDatas[GameConnet.m_TMachineBuyInGameData.m_uiTID].m_usMemberCnt.ToString();

        if (StopModeState == ENUM_STOPMODE_STATE.WaitNextNewRound)
        {
            WaitRoundObject.SetActive(true);
            EndObject.SetActive(false);
            ShuffleCardObject.SetActive(false);
        }
        else
        {
            WaitRoundObject.SetActive(false);
        }

        if (StopModeState == ENUM_STOPMODE_STATE.WaitStop)
        {
            if (!History_Control.HistoryInit && !FourCardHistory_Control.FCHistoryDataInit && InitData_Bool)
            {
                History_Control.HistoryOpenBool = false;
                FourCardHistory_Control.FCHistoryClickBool = false;
                InitData_Bool = false;
            }
        }

        if (StopModeState == ENUM_STOPMODE_STATE.Idle)
        {
            Idle_Object.SetActive(true);
            WaitPlayerTimer_Lable.enabled = true;
            ShuffleCardObject.SetActive(false);
            WaitRoundObject.SetActive(false);
            EndObject.SetActive(false);
            if ((WaitTotalPlayerTimer - (int)WaitPlayerTimer) > 0)
            {
                WaitPlayerTimer += Time.deltaTime;
                WaitPlayerTimer_Lable.text = (WaitTotalPlayerTimer - (int)WaitPlayerTimer).ToString();
            }
            else
            {
                WaitPlayerTimer_Lable.text = "0";
            }

            if (NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.NewRound || NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.WaitBet)
            {
                StopModeState = ENUM_STOPMODE_STATE.WaitStop;
            }
        }
        else
        {
            Idle_Object.SetActive(false);
            WaitPlayerTimer_Lable.enabled = false;
            WaitPlayerTimer = 0;
          //  WaitTotalPlayerTimer = 0;
        }

        if (NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.ShuffleNewRound || StopModeState == ENUM_STOPMODE_STATE.ShuffleTimeShow)
        {
            ShuffleCardObject.SetActive(true);
            WaitRoundObject.SetActive(false);
            EndObject.SetActive(false);
        }
        else
        {
            ShuffleCardObject.SetActive(false);
        }

        if (StopModeState == ENUM_STOPMODE_STATE.WaitNextNewRound)
        {
            if (NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.NewRound || NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.ShuffleNewRound)
            {
                if (NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.NewRound)
                {
                    StopModeState = ENUM_STOPMODE_STATE.WaitStop;
                    GameSound.BetStart_Bool = true;
                }
                else if (NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.ShuffleNewRound)
                {
                    //洗牌後新局開始
                    StopModeState = ENUM_STOPMODE_STATE.ShuffleTimeShow;
                    DataInitVoid();
                }
            }
        }
        else
        {
            WaitRoundObject.SetActive(false);
        }

        if (StopModeState == ENUM_STOPMODE_STATE.FourCardShow || StopModeState == ENUM_STOPMODE_STATE.FourCardShow || StopModeState == ENUM_STOPMODE_STATE.FourCardMoneyShow || StopModeState == ENUM_STOPMODE_STATE.FourCardEnd || StopModeState == ENUM_STOPMODE_STATE.WaitFourCardTime)
        {
            BetButton_Object.SetActive(false);
        }
        else
        {
            BetButton_Object.SetActive(true);
        }

        if (StopModeState == ENUM_STOPMODE_STATE.EndShow)
        {
            if (Timer < 5)
            {
                Timer += Time.deltaTime;
                EndTime_Label.text = (5 - (int)Timer).ToString();
                if (!EndWindow_Control.EndWindowOpenBool)
                {
                    EndObject.SetActive(true);
                    WaitRoundObject.SetActive(false);
                    ShuffleCardObject.SetActive(false);
                    EndTime_Object.SetActive(false);
                }
                else
                {
                    EndTime_Object.SetActive(true);
                    if (Competition.RaceGame_Bool)
                    {
                        EndTime_Object.transform.localPosition = new Vector3(563,154,1);
                    }
                }
            }
            else
            {
                EndWindow_Control.EndWindowOpenBool = false;
                if (Race_Control.RaceEnd_Bool)
                {
                    EndObject.SetActive(false);
                    EndTime_Object.SetActive(false);
                    EndWindow_Control.EndWindowOpenBool = false;
                }
                    EndObject.SetActive(true);
                WaitRoundObject.SetActive(false);
                ShuffleCardObject.SetActive(false);
                EndTime_Object.SetActive(false);

                if (NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.NewRound)
                {
                   
                    //新局開始
                    StopModeState = ENUM_STOPMODE_STATE.WaitStop;
                    GameSound.BetStart_Bool = true;
                    EndWindow_Control.EndWindowOpenBool = false;
                    if (AutoMode_Control.AutoModeNumber > 0)
                    {
                        AutoMode_Control.StartAutoBetBool = true;
                    }
                    else
                    {
                        AutoMode_Control.AutoClearBetBool = true;
                    }
                    DataInitVoid();
                }
                else if (NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.ShuffleNewRound)
                {
                    //洗牌後新局開始
                    StopModeState = ENUM_STOPMODE_STATE.ShuffleTimeShow;
                    DataInitVoid();
                }
                else
                {
                 
                    //等待下局開始
                }
            }
        }
        else if (StopModeState == ENUM_STOPMODE_STATE.WaitStop)
        {
            EndObject.SetActive(false);
            WaitRoundObject.SetActive(false);
            ShuffleCardObject.SetActive(false);
            EndTime_Object.SetActive(false);
            //停止押注
            if (NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.StopBet)
            {
                StopModeState = ENUM_STOPMODE_STATE.CardShow;
                GameSound.BetStop_Bool = true;
            }
        }
        else if (StopModeState == ENUM_STOPMODE_STATE.ShuffleTimeShow)
        {
            if (DelayTimer < 3)
            {
                DelayTimer += Time.deltaTime;
                ShuffleCardObject.SetActive(true);
                EndObject.SetActive(false);
                WaitRoundObject.SetActive(false);
                EndTime_Object.SetActive(false);
            }
            else
            {
                InitData_Bool = true;
                History_Control.HistoryOpenBool = true;
                FourCardHistory_Control.FCHistoryClickBool = true;
                History_Control.HistoryInit = true;
                FourCardHistory_Control.FCHistoryDataInit = true;
                EndWindow_Control.EndWindowOpenBool = false;
                StopModeState = ENUM_STOPMODE_STATE.WaitStop;
                NowGameState = (byte)ENUM_BACCARAT_TABLE_STATE.NewRound;
                GameSound.BetStart_Bool = true;
                LeftOverCardPoint = 8 * 52;
                BankerWinPoint = 0;
                PlayerWinPoint = 0;
                DrawWinPoint = 0;
                BankerPairWinPoint = 0;
                PlayerPairWinPoint = 0;
                DelayTimer = 0;
                GameSound.BetStart_Bool = true;
                EndWindow_Control.EndWindowOpenBool = false;
                if (AutoMode_Control.AutoModeNumber > 0)
                {
                    AutoMode_Control.StartAutoBetBool = true;
                }
                else
                {
                    AutoMode_Control.AutoClearBetBool = true;
                }
                DataInitVoid();
            }
        }

        if (StopModeState == ENUM_STOPMODE_STATE.WaitFourCardTime && NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.StopBid)
        {
            StopModeState = ENUM_STOPMODE_STATE.FourCardShow;
            GameSound.BetStop_Bool = true;
        }

        if (GetLastWinDataBool && GetNormalWinDataBool)
        {
            //CAllBetAward Data = new CAllBetAward();
            //Data.m_oNormalAward.m_enumAward = SaveWin;
            //Data.m_oNormalAward.m_bBankerOnePair = SaveBankerPair;
            //Data.m_oNormalAward.m_bPlayerOnePair = SavePlayerPair;
            //Data.m_oLastAward.m_enumAward = LastWin;
            //Data.m_oLastAward.m_bBankerOnePair = SaveBankerPair;
            //Data.m_oLastAward.m_bPlayerOnePair = SavePlayerPair;
            //History_Control.HistoryData.Add(History_Control.HistoryNumber, Data);
            //History_Control.HistoryNumber++;

            EndWindow_Control.EndPlanningBool = true;
            MainGame_Control.FinallDataGetBool = true;
            GetLastWinDataBool = false;
            GetNormalWinDataBool = false;
        }
    }

    void LabelVoid()
    {
        if (VersionDef.InternationalLanguageSystem)
        {
            HistroyDataLabel[4].text = Font_Control.Instance.m_dicMsgStr[2008031] + BankerWinPoint;
            HistroyDataLabel[0].text = Font_Control.Instance.m_dicMsgStr[2008032] + PlayerWinPoint;
            HistroyDataLabel[2].text = Font_Control.Instance.m_dicMsgStr[2008033] + DrawWinPoint;
            HistroyDataLabel[3].text = Font_Control.Instance.m_dicMsgStr[2008034] + BankerPairWinPoint;
            HistroyDataLabel[1].text = Font_Control.Instance.m_dicMsgStr[2008035] + PlayerPairWinPoint;
            HistroyDataLabel[5].text = Font_Control.Instance.m_dicMsgStr[2008036] + LeftOverCardPoint;
        }
        else
        {
            HistroyDataLabel[4].text = "莊贏：" + BankerWinPoint;
            HistroyDataLabel[0].text = "閒贏：" + PlayerWinPoint;
            HistroyDataLabel[2].text = "平和：" + DrawWinPoint;
            HistroyDataLabel[3].text = "莊對：" + BankerPairWinPoint;
            HistroyDataLabel[1].text = "閒對：" + PlayerPairWinPoint;
            HistroyDataLabel[5].text = "剩餘張數：" + LeftOverCardPoint;
        }
      
    }

    void DataInitVoid()
    {
        for (int i = 0; i < 5; i++)
        {
            WinArea[i] = 0;
        }
        Card_Control.CardShowOverBool = false;
        FourCard_Control.FourCard = 0;
        FinallDataGetBool = false;
        Timer = 0;
    }
}
