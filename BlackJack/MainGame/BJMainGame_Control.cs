using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CardTeamListClass;
using WinLoseListClass;
using MoneyTable;
using GameCore.Manager.BlackJack;
using GameCore;

namespace WinLoseListClass
{
    public enum WinLoseList
    {
        WinDraw = 0,  //和贏
        WinBanker = 1,//莊贏
        WinPlayer = 2,//閒贏
        BlackJack = 3,//閒家選擇報到
        PointOut = 4, //爆牌
        NoCheck =5,   //尚未確認
    }
}

public class BJMainGame_Control : MonoBehaviour {

    public static Dictionary<CardTeamList, WinLoseList> TableWinLose = new Dictionary<CardTeamList, WinLoseList>();//輸贏狀況储存
    public static Dictionary<uint, CPACK_BlackJack_UpdateTbleState> TableState = new Dictionary<uint, CPACK_BlackJack_UpdateTbleState>(); //uint = 桌檯ID
    public static ENUM_BLACKJACK_TABLE_STATE NowStateSave = ENUM_BLACKJACK_TABLE_STATE.Idle;
    public static byte[] TableWinLoseCheck = new byte[11];//輸贏表演確認 0:不表演 1:表演開始
    public static bool OneTeamWinLoseShow = false;//主要玩家不分牌時輸贏表演
    public static bool MainWinLoseShow = false;//主要輸贏表演
    public static bool EnterBetBool = false;//確認押注BOOL
    public static int SelectCoin = 1000; //選擇硬幣
    public GameObject[] BetBarObject = new GameObject[2];//MainBar
    public static uint TableID = 1;//桌台編號
    public static bool AllCardShow_Bool = false;//所有卡片表演完成
    public static bool BetCancel_Bool = false;//取消押注
    public static bool MainInit_Bool = false;
    public static byte NowCardPoint = 0;
    public static byte NowCardPoint2 = 0;
    public static bool FirstGetCard_Bool = false;//第一次發牌結束
    public static bool CardSenceList_Bool = false;//玩家(自己)是否分牌
    public static Dictionary<uint, byte[]> FirstCardListSave = new Dictionary<uint, byte[]>();//第一次發牌資料储存
    public static Dictionary<uint, byte[]> FinallCardListSave = new Dictionary<uint, byte[]>();//最後發牌資料储存
    public static bool FirstCardDataGet_Bool = false;//確定第一次發牌所以資料
    public static bool FirstCardShow_Bool = false;//第一次發牌表演
    public static bool BankerFinallData_Bool = false;//莊家最後結果
    public static bool WaitAngle = false;//在一次
    public static Dictionary<uint, CPACK_BlackJack_PlayerBetWin> FinallEndSave = new Dictionary<uint, CPACK_BlackJack_PlayerBetWin>();//最後資料储存
    float FirstCardShowDelayTime = 0;
    public static byte[] BlackJack_Bool = new byte[5];

    bool CheckBlackJack = false;//是否要報到
    bool CheckScoreboard = false;//是否要分牌
    bool AllReady = true;

    //時間
    public static uint BetTimerMax = 0;//接收現在時間
    public static float BetTimerMinus = 0;//要減去時間
    public GameObject TimeObject;
    public UISprite Time_Sprite;
    public UISprite[] TimeTwo_Sprite = new UISprite[2];

    //金幣
    public UILabel MyMoneyLabel;
    public UILabel BetMoenyLabel;
    public static ulong MyMoney = 0;
    public static long BetMoney = 0;
    public static ulong FinallMoney = 0;
    public static bool MoneyEndShow_Bool = false;

    public UILabel Machine_Sprite;
    public static uint BuyInMachine = 0;

    //卡片點數
    byte Card1Point = 0;
    byte Card2Point = 0;

    //時間(方塊版)
    public UISprite[] TimeBox = new UISprite[5];

    bool CheckTime_Bool = false;

    public static byte NoBetRound = 0;
    // Use this for initialization
    void Start () {
        CheckTime_Bool = false;
        AllScenceLoad.LoadScence = false;
        TableState.Clear();
        SelectCoin = 1000;
        BetBarObject[0].SetActive(true);
        BetBarObject[1].SetActive(false);
        MainInit_Bool = false;
        BetTimerMax = 0;
        BetTimerMinus = 0;
        NowStateSave = ENUM_BLACKJACK_TABLE_STATE.Idle;
        TimeObject.SetActive(false);

        SeatWinLose();
        DataInit();

        MyMoney = GameConnet.m_BuyInMoney;
        BetMoney = 0;
        //測試
        CPACK_BlackJack_UpdateTbleState TestData = new CPACK_BlackJack_UpdateTbleState();
        TestData.m_enumState = ENUM_BLACKJACK_TABLE_STATE.Idle;
        TestData.m_uiWaitMSec = 0;
        TableState.Add(TableID, TestData);

        Machine_Sprite.text = GameConnet.m_TMachineBuyInGameData.m_uiTID.ToString("000");
    }

    // Update is called once per frame
    void Update()
    {
        if (AllReady)
        {
            GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.BlackJack, (uint)ENUM_COMMON_PACKID_GC.C2G_TMachine_GameReady, null);
            AllReady = false;
        }

        if (TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.NewRound || TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.ShuffleNewRound || TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.WaitBet)
        {
            BetBarObject[0].SetActive(true);
            BetBarObject[1].SetActive(false);
        }
        else
        {
            BetBarObject[0].SetActive(false);
            BetBarObject[1].SetActive(true);
        }

        if (MainInit_Bool)
        {
            DataInit();
            MainInit_Bool = false;
        }

        //押注金額和持有金幣
        if (MoneyEndShow_Bool || TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.GameOver)
        {
            BetMoney = 0;
            MyMoney = FinallMoney;
        }
        else
        {
            BetMoney = Cash_Control.TableCash[0] + Cash_Control.TableCash[1];
        }

        MyMoneyLabel.text = ((long)MyMoney - BetMoney).ToString();
        BetMoenyLabel.text = BetMoney.ToString();


        //時間(押注)
        #region NormalTime
        if (TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.NewRound || TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.ShuffleNewRound || TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.WaitBet
            || TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.InsuranceTime || TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.InsuranceOver 
            || TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.ShuffleNewRound || TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.PlayerTime || TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.CheckPlayer || TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.PlayerOver)
        {
            #region Time1
            if ((TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.PlayerTime || TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.CheckPlayer || TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.PlayerOver) && !Cash_Control.OnBetClick)
            {
                TimeObject.SetActive(false);
                if (Cash_Control.PlayerRound[(byte)TableList.MyTable] == 1)
                {
                    if (TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.PlayerTime)
                    {
                        TimeBox[0].enabled = true;
                        TimeBox[0].fillAmount = (1 - (BetTimerMinus / (float)BetTimerMax));
                    }
                    else
                    {
                        TimeBox[0].enabled = false;
                        TimeBox[0].fillAmount = 1;
                    }
                }
                else if (Cash_Control.PlayerRound[(byte)TableList.PlayerTable1] == 1)
                {
                    if (TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.PlayerTime)
                    {
                        TimeBox[1].enabled = true;
                        TimeBox[1].fillAmount = (1 - (BetTimerMinus / (float)BetTimerMax));
                    }
                    else
                    {
                        TimeBox[1].enabled = false;
                        TimeBox[1].fillAmount = 1;
                    }
                }
                else if (Cash_Control.PlayerRound[(byte)TableList.PlayerTable2] == 1)
                {
                    if (TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.PlayerTime)
                    {
                        TimeBox[2].enabled = true;
                        TimeBox[2].fillAmount = (1 - (BetTimerMinus / (float)BetTimerMax));
                    }
                    else
                    {
                        TimeBox[2].enabled = false;
                        TimeBox[2].fillAmount = 1;
                    }
                }
                else if (Cash_Control.PlayerRound[(byte)TableList.PlayerTable3] == 1)
                {
                    if (TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.PlayerTime)
                    {
                        TimeBox[3].enabled = true;
                        TimeBox[3].fillAmount = (1 - (BetTimerMinus / (float)BetTimerMax));
                    }
                    else
                    {
                        TimeBox[3].enabled = false;
                        TimeBox[3].fillAmount = 1;
                    }
                }
                else if (Cash_Control.PlayerRound[(byte)TableList.PlayerTable4] == 1)
                {
                    if (TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.PlayerTime)
                    {
                        TimeBox[4].enabled = true;
                        TimeBox[4].fillAmount = (1 - (BetTimerMinus / (float)BetTimerMax));
                    }
                    else
                    {
                        TimeBox[4].enabled = false;
                        TimeBox[4].fillAmount = 1;
                    }
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        TimeBox[i].enabled = false;
                        TimeBox[i].fillAmount = 1;
                    }
                }
            }
            else if((TableState[TableID].m_enumState != ENUM_BLACKJACK_TABLE_STATE.PlayerTime && TableState[TableID].m_enumState != ENUM_BLACKJACK_TABLE_STATE.CheckPlayer || TableState[TableID].m_enumState != ENUM_BLACKJACK_TABLE_STATE.PlayerOver) && !Cash_Control.OnBetClick)
            {
                TimeObject.SetActive(true);
                for (int i = 0; i < 5; i++)
                {
                    TimeBox[i].enabled = false;
                    TimeBox[i].fillAmount = 1;
                }
            }
            #endregion

            #region Time2
            //if (TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.PlayerTime && Cash_Control.OnBetClick)
            //{
            //    TimeObject.SetActive(false);
            //}
            //else
            //{
            //    TimeObject.SetActive(true);
            //}
            #endregion

            Time_Sprite.spriteName = "numberC_time_" + ((BetTimerMax - (uint)BetTimerMinus) % 10).ToString();
            TimeTwo_Sprite[0].spriteName = "numberC_time_" + ((BetTimerMax - (uint)BetTimerMinus) % 10).ToString();
            TimeTwo_Sprite[1].spriteName = "numberC_time_" + (((BetTimerMax - (uint)BetTimerMinus) / 10) % 10).ToString();

            if ((BetTimerMax - (uint)BetTimerMinus) > 0)
            {
                BetTimerMinus += Time.deltaTime;
                if ((BetTimerMax - (uint)BetTimerMinus) > 9)
                {
                    Time_Sprite.enabled = false;
                    TimeTwo_Sprite[0].enabled = true;
                    TimeTwo_Sprite[1].enabled = true;
                }
                else
                {
                    Time_Sprite.enabled = true;
                    TimeTwo_Sprite[0].enabled = false;
                    TimeTwo_Sprite[1].enabled = false;
                }
            }
            else
            {
                BetTimerMinus = BetTimerMax;
            }
            CheckTime_Bool = false;
        }
        else
        {
            if (!CheckTime_Bool)
            {
                BetTimerMinus = 0;
                TimeObject.SetActive(false);
                for (int i = 0; i < 5; i++)
                {
                    TimeBox[i].enabled = false;
                    TimeBox[i].fillAmount = 1;
                }
                CheckTime_Bool = true;
            }
        }
        #endregion

        //桌台狀態
        StateControl();

        //第一次發牌表演
        if (FirstCardDataGet_Bool)
        {
            FirstCardShowControl();
        }

        //最後一次發牌
        if (BankerFinallData_Bool)
        {
            FinallCardShowControl();
        }
    }

    //生成座位輸贏
    void SeatWinLose()
    {
        TableWinLose.Clear();     
        TableWinLose.Add(CardTeamList.Card1Team1, WinLoseList.NoCheck);
        TableWinLose.Add(CardTeamList.Card1Team2, WinLoseList.NoCheck);
        TableWinLose.Add(CardTeamList.Card2Team1, WinLoseList.NoCheck);
        TableWinLose.Add(CardTeamList.Card2Team2, WinLoseList.NoCheck);
        TableWinLose.Add(CardTeamList.Card3Team1, WinLoseList.NoCheck);
        TableWinLose.Add(CardTeamList.Card3Team2, WinLoseList.NoCheck);
        TableWinLose.Add(CardTeamList.Card4Team1, WinLoseList.NoCheck);
        TableWinLose.Add(CardTeamList.Card4Team2, WinLoseList.NoCheck);
        TableWinLose.Add(CardTeamList.Card5Team1, WinLoseList.NoCheck);
        TableWinLose.Add(CardTeamList.Card5Team2, WinLoseList.NoCheck);
    }

    //所有桌台狀態管理
    void StateControl()
    {
        if (TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.Idle && (NowStateSave == ENUM_BLACKJACK_TABLE_STATE.NewRound
            || NowStateSave == ENUM_BLACKJACK_TABLE_STATE.ShuffleNewRound || NowStateSave == ENUM_BLACKJACK_TABLE_STATE.WaitBet))
        {
            //剛進遊戲是否新局開始判斷
            TableState[TableID].m_enumState = NowStateSave;
            if (TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.WaitBet)
                StateShow_Control.PleaseBetStateStart = true;
        }
        else if ((TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.NewRound || TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.ShuffleNewRound
            || TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.WaitBet) && NowStateSave == ENUM_BLACKJACK_TABLE_STATE.GameStart)
        {
            //遊戲開始階段
            Cash_Control.CashMoveStart = true;
            if (!EnterBetBool)
            {
                Cash_Control.TableCash[0] = 0;
            }
            TableState[TableID].m_enumState = NowStateSave;
        }
        else if ((TableState[TableID].m_enumState != ENUM_BLACKJACK_TABLE_STATE.NewRound && TableState[TableID].m_enumState != ENUM_BLACKJACK_TABLE_STATE.ShuffleNewRound
            && TableState[TableID].m_enumState != ENUM_BLACKJACK_TABLE_STATE.WaitBet && TableState[TableID].m_enumState != ENUM_BLACKJACK_TABLE_STATE.Idle)
            && NowStateSave == ENUM_BLACKJACK_TABLE_STATE.InsuranceTime && TableState[TableID].m_enumState != ENUM_BLACKJACK_TABLE_STATE.InsuranceTime && FirstGetCard_Bool)
        {
            //購買保險階段
            TableState[TableID].m_enumState = NowStateSave;
            if (Cash_Control.TableCash[0] != 0)
                ButtonSprite_Control.BetInsure_Bool = true;
        }
        else if (TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.NewRound && TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.ShuffleNewRound 
            && NowStateSave == ENUM_BLACKJACK_TABLE_STATE.InsuranceOver && FirstGetCard_Bool)
        {
            //保險購買結束
            ButtonSprite_Control.BetInsure_Bool = false;
        }
        else if (TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.CheckPlayer && TableState[TableID].m_enumState != ENUM_BLACKJACK_TABLE_STATE.PlayerTime 
            && NowStateSave == ENUM_BLACKJACK_TABLE_STATE.PlayerTime && FirstGetCard_Bool && !WaitAngle)
        {
            //玩家回合
            DealerCard_Move.DealerShowCancel_Bool = true;
            ButtonSprite_Control.BetInsure_Bool = false;
            TableState[TableID].m_enumState = NowStateSave;
        }
        else if (NowStateSave == ENUM_BLACKJACK_TABLE_STATE.GameSettlement && AllCardShow_Bool)
        {
            //結算
            #region End
            if (TableState[TableID].m_enumState != ENUM_BLACKJACK_TABLE_STATE.PlayerOver)
            {
                DealerCard_Move.DealerShowCancel_Bool = true;
                TableState[TableID].m_enumState = ENUM_BLACKJACK_TABLE_STATE.PlayerOver;
            }

            Cash_Control.CashMoveStart = false;
            TableState[TableID].m_enumState = NowStateSave;

            for (byte i = 0; i < 10; i++)
            {
                if (BJEndWindow_Control.WinLoseShowSave[i] == 1)
                {
                    if (i == 0)
                    {
                        if (BJEndWindow_Control.WinLoseShowSave[1] != 0)
                        {
                            TableWinLoseCheck[i] = 1;
                        }
                        else
                        {
                            TableWinLoseCheck[(byte)CardTeamList.CardBanker] = 1;
                            MainWinLoseShow = true;
                        }
                    }
                    else
                    {
                        TableWinLoseCheck[i] = 1;
                    }
                }
                else
                {
                    TableWinLoseCheck[i] = 0;
                }
            }   

            foreach (var item in FinallEndSave)
            {
                for (byte i = 0; i < 5; i++)
                {
                    if (BJEndWindow_Control.m_BetAward.ContainsKey(i))
                    {
                        if (BJEndWindow_Control.m_BetAward[i].PlayerDBID == item.Key)
                        {
                            BJEndWindow_Control.m_BetAward[i].PlayerName = item.Value.m_stPlayerName;
                            BJEndWindow_Control.m_BetAward[i].WinLoseMoney = ((long)item.Value.m_ui64GameMoney - (long)item.Value.m_ui64AllBetMoney);
                            if (item.Key == MainConnet.m_PlayerData.m_uiDBID)
                            {
                                if (BJHistory_Control.History_DicSave.Count == 1)
                                {
                                    BJHistory_Control.History_DicSave[0].WinLoseMoney = ((long)item.Value.m_ui64GameMoney - (long)item.Value.m_ui64AllBetMoney);
                                }
                                else if (BJHistory_Control.History_DicSave.Count == 2)
                                {
                                    if (BJHistory_Control.History_DicSave[0].m_WinLoseList == WinLoseList.BlackJack)
                                        BJHistory_Control.History_DicSave[0].WinLoseMoney = (long)(Cash_Control.TableCash[(byte)CardTeamList.Card1Team1] * 1.5f);
                                    else if (BJHistory_Control.History_DicSave[0].m_WinLoseList == WinLoseList.WinDraw)
                                        BJHistory_Control.History_DicSave[0].WinLoseMoney = 0;
                                    else if (BJHistory_Control.History_DicSave[0].m_WinLoseList == WinLoseList.WinBanker)
                                        BJHistory_Control.History_DicSave[0].WinLoseMoney = -Cash_Control.TableCash[(byte)CardTeamList.Card1Team1];
                                    else if (BJHistory_Control.History_DicSave[0].m_WinLoseList == WinLoseList.WinPlayer)
                                        BJHistory_Control.History_DicSave[0].WinLoseMoney = Cash_Control.TableCash[(byte)CardTeamList.Card1Team1];
                                    else if (BJHistory_Control.History_DicSave[0].m_WinLoseList == WinLoseList.PointOut)
                                        BJHistory_Control.History_DicSave[0].WinLoseMoney = -Cash_Control.TableCash[(byte)CardTeamList.Card1Team1];


                                    if (BJHistory_Control.History_DicSave[1].m_WinLoseList == WinLoseList.BlackJack)
                                        BJHistory_Control.History_DicSave[1].WinLoseMoney = (long)(Cash_Control.TableCash[(byte)CardTeamList.Card1Team2] * 1.5f);
                                    else if (BJHistory_Control.History_DicSave[1].m_WinLoseList == WinLoseList.WinDraw)
                                        BJHistory_Control.History_DicSave[1].WinLoseMoney = 0;
                                    else if (BJHistory_Control.History_DicSave[1].m_WinLoseList == WinLoseList.WinBanker)
                                        BJHistory_Control.History_DicSave[1].WinLoseMoney = -Cash_Control.TableCash[(byte)CardTeamList.Card1Team2];
                                    else if (BJHistory_Control.History_DicSave[1].m_WinLoseList == WinLoseList.WinPlayer)
                                        BJHistory_Control.History_DicSave[1].WinLoseMoney = Cash_Control.TableCash[(byte)CardTeamList.Card1Team2];
                                    else if (BJHistory_Control.History_DicSave[0].m_WinLoseList == WinLoseList.PointOut)
                                        BJHistory_Control.History_DicSave[1].WinLoseMoney = -Cash_Control.TableCash[(byte)CardTeamList.Card1Team2];
                                }
                            }
                        }
                    }
                }              
            }
            BJHistory_Control.HistorySaveOK_Bool = true;
            BJEndWindow_Control.EndWindow_Bool = true;
            AllCardShow_Bool = false;
            Debug.Log("結算");
            #endregion
        }


        //其他
        if (TableState[TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.PlayerTime)
        {
            if (BJCard_Control.Seat1Team1.Count == 2 && BJCard_Control.Seat1Team2.Count == 0)
            {
                //詢問是否要報到
                if ((NowCardPoint == 21 || NowCardPoint2 == 21) && !CheckBlackJack)
                {
                    ButtonSprite_Control.BJ21_Bool = true;
                    CheckBlackJack = true;
                }

                //是否要分牌
                if ((Card1Point == Card2Point) && !CheckScoreboard && Cash_Control.PlayerRound[(byte)TableList.MyTable] == 1)
                {
                    ButtonSprite_Control.Scoreboard_Bool = true;
                    CheckScoreboard = true;
                }
            }
        }
    }

    //第一次發牌表演管理
    void FirstCardShowControl()
    {
        if (!FirstCardShow_Bool)
        {
            if (FirstCardShowDelayTime < 0.5f)
            {
                FirstCardShowDelayTime += Time.deltaTime;
            }
            else
            {
                //第一張牌
                foreach (var item in FirstCardListSave)
                {
                    if (item.Value[0] != 0)
                    {
                        for (byte i = 0; i < 5; i++)
                        {
                            if (Cash_Control.PlayerDBID[i] == item.Key)
                            {
                                DealerCard_Move.CardNumber = item.Value[0];
                                if (i == 0)
                                {
                                    byte Point = (byte)(((item.Value[0] - 1) % 13) + 1);
                                    if (Point > 10)
                                        Card1Point = 10;
                                    else
                                        Card1Point = Point;
                                    BJCard_Control.Seat1Team1.Add(item.Value[0]);
                                }
                                else if (i == 1)
                                {
                                    BJCard_Control.Seat2Team1.Add(item.Value[0]);
                                }
                                else if (i == 2)
                                {
                                    BJCard_Control.Seat3Team1.Add(item.Value[0]);
                                }
                                else if (i == 3)
                                {
                                    BJCard_Control.Seat4Team1.Add(item.Value[0]);
                                }
                                else if (i == 4)
                                {
                                    BJCard_Control.Seat5Team1.Add(item.Value[0]);
                                }
                                BJCard_Control.SeatTeamPoint[i * 2]++;
                                BJCard_Control.SeatTeamAddCheck[i * 2] = 1;
                                BJCard_Control.NowFinallCardSeat[i * 2] = 1;
                                item.Value[0] = 0;
                                FirstCardShow_Bool = true;
                                FirstCardShowDelayTime = 0;
                                return;
                            }
                        }
                    }
                }

                if (FirstCardListSave[6][0] != 0)
                {
                    DealerCard_Move.CardNumber = FirstCardListSave[6][0];
                    BJCard_Control.SeatBanker.Add(FirstCardListSave[6][0]);
                    BJCard_Control.SeatTeamPoint[(byte)CardTeamList.CardBanker]++;
                    BJCard_Control.SeatTeamAddCheck[(byte)CardTeamList.CardBanker] = 1;
                    BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.CardBanker] = 1;
                    FirstCardListSave[6][0] = 0;
                    FirstCardShow_Bool = true;
                    FirstCardShowDelayTime = 0;
                    return;
                }
                //第二張牌
                foreach (var item in FirstCardListSave)
                {
                    if (item.Value[1] != 0)
                    {
                        for (byte i = 0; i < 5; i++)
                        {
                            if (Cash_Control.PlayerDBID[i] == item.Key)
                            {
                                DealerCard_Move.CardNumber = item.Value[1];
                                if (i == 0)
                                {
                                    byte Point = (byte)(((item.Value[1] - 1) % 13) + 1);
                                    if (Point > 10)
                                        Card2Point = 10;
                                    else
                                        Card2Point = Point;
                                    BJCard_Control.Seat1Team1.Add(item.Value[1]);
                                }
                                else if (i == 1)
                                {
                                    BJCard_Control.Seat2Team1.Add(item.Value[1]);
                                }
                                else if (i == 2)
                                {
                                    BJCard_Control.Seat3Team1.Add(item.Value[1]);
                                }
                                else if (i == 3)
                                {
                                    BJCard_Control.Seat4Team1.Add(item.Value[1]);
                                }
                                else if (i == 4)
                                {
                                    BJCard_Control.Seat5Team1.Add(item.Value[1]);
                                }
                                BJCard_Control.SeatTeamPoint[i * 2]++;
                                BJCard_Control.SeatTeamAddCheck[i * 2] = 1;
                                BJCard_Control.NowFinallCardSeat[i * 2] = 1;
                                item.Value[1] = 0;
                                FirstCardShow_Bool = true;
                                FirstCardShowDelayTime = 0;
                                return;
                            }
                        }
                    }
                }

                if (FirstCardListSave[6][1] == 0)
                {
                    DealerCard_Move.CardNumber = FirstCardListSave[6][1];
                    BJCard_Control.SeatBanker.Add(FirstCardListSave[6][1]);
                    BJCard_Control.SeatTeamPoint[(byte)CardTeamList.CardBanker]++;
                    BJCard_Control.SeatTeamAddCheck[(byte)CardTeamList.CardBanker] = 1;
                    BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.CardBanker] = 1;
                    FirstCardListSave[6][0] = 0;
                    FirstCardShow_Bool = true;
                    FirstCardShowDelayTime = 0;
                    FirstCardDataGet_Bool = false;
                    Cash_Control.CashMoveStart = false;
                    return;
                }
            }
        }
    }

    //最後表演管理
    void FinallCardShowControl()
    {
        if (!FirstCardShow_Bool)
        {
            if (BJCard_Control.SeatBanker[1] == 0)
            {
                BJCard_Control.SeatBanker[1] = FinallCardListSave[0][1];
                BJCard_Control.PointCheckStart[(byte)CardTeamList.CardBanker] = 1;
                FirstCardShow_Bool = true;
                return;
            }
            else if (BJCard_Control.SeatBanker.Count != FinallCardListSave[0].Length)
            {
                if (FirstCardShowDelayTime < 0.5f)
                {
                    FirstCardShowDelayTime += Time.deltaTime;
                }else
                {
                    DealerCard_Move.CardNumber = FinallCardListSave[0][(byte)(BJCard_Control.SeatBanker.Count)];
                    BJCard_Control.SeatBanker.Add(FinallCardListSave[0][(byte)(BJCard_Control.SeatBanker.Count)]);
                    BJCard_Control.SeatTeamPoint[(byte)CardTeamList.CardBanker]++;
                    BJCard_Control.SeatTeamAddCheck[(byte)CardTeamList.CardBanker] = 1;
                    BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.CardBanker] = 1;
                    FirstCardShow_Bool = true;
                    FirstCardShowDelayTime = 0;
                    return;
                }
            }
            else
            {
                if (FirstCardShowDelayTime < 2)
                {
                    FirstCardShowDelayTime += Time.deltaTime;
                }
                else
                {
                    BankerFinallData_Bool = false;
                    AllCardShow_Bool = true;
                  
                    FirstCardShowDelayTime = 0;
                }
            }
        }
    }

    void DataInit()
    {
        for (int i = 0; i < 11; i++)
        {
            TableWinLoseCheck[i] = 0;
            if (i < 5)
            {
                TimeBox[i].enabled = false;
                TimeBox[i].fillAmount = 1;
                BlackJack_Bool[i] = 0;
            }
        }
        NowCardPoint = 0;
        NowCardPoint2 = 0;
        TableWinLose[CardTeamList.Card1Team1] = WinLoseList.NoCheck;
        TableWinLose[CardTeamList.Card1Team2] = WinLoseList.NoCheck;
        TableWinLose[CardTeamList.Card2Team1] = WinLoseList.NoCheck;
        TableWinLose[CardTeamList.Card2Team2] = WinLoseList.NoCheck;
        TableWinLose[CardTeamList.Card3Team1] = WinLoseList.NoCheck;
        TableWinLose[CardTeamList.Card3Team2] = WinLoseList.NoCheck;
        TableWinLose[CardTeamList.Card4Team1] = WinLoseList.NoCheck;
        TableWinLose[CardTeamList.Card4Team2] = WinLoseList.NoCheck;
        TableWinLose[CardTeamList.Card5Team1] = WinLoseList.NoCheck;
        TableWinLose[CardTeamList.Card5Team2] = WinLoseList.NoCheck;
        TableWinLose[CardTeamList.CardBanker] = WinLoseList.NoCheck;
        
        MoneyEndShow_Bool = false;
        ButtonSprite_Control.Scoreboard_Bool = false;
        ButtonSprite_Control.BJ21_Bool = false;
        ButtonSprite_Control.BetInsure_Bool = false;
        CheckBlackJack = false;
        CheckScoreboard = false;
        OneTeamWinLoseShow = false;
        MainWinLoseShow = false;
        EnterBetBool = false;
        AllCardShow_Bool = false;
        BetCancel_Bool = false;
        FirstGetCard_Bool = false;
        CardSenceList_Bool = false;
        FirstCardListSave.Clear();
        FinallCardListSave.Clear();
        FirstCardDataGet_Bool = false;
        FirstCardShow_Bool = false;
        FirstCardShowDelayTime = 0;
        BankerFinallData_Bool = false;
        DealerCard_Move.BankerSenceCard = false;
        FinallEndSave.Clear();
        WaitAngle = false;
    }
}
