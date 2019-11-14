using UnityEngine;
using System.Collections;
using MoneyTable;
using GameCore.Manager.BlackJack;
using GameCore;

public class Button_Control : MonoBehaviour {
    //按鈕列表
    public enum ButtonList
    {
        Coin1,//錢幣1
        Coin2,//錢幣2
        Coin3,//錢幣3
        Coin4,//錢幣4
        BetEnter,//押注確定
        BetCancel,//押注取消
        Deal,//要牌
        Double,//加倍押注
        DealEnd,//要牌結束
        Capitulate,//投降
        Back,//回到大廳
        BetInsure,//購買保險
        NoInsure,//不購買保險
        InformationOpen,//資訊
        InformationCancel,//關閉資訊
        InformationNext,//資訊下一頁
        InformationBack,//資訊上一頁
        Scoreboard,//分牌
        OnBet,//押注
        BJ21,//報到
        GameEnd,//結算關閉
        NoScoreboard,//不分牌
        NoBJ21,//不報到
        BJGameHelp,//HELP
        BJHelpNext,
        BJHelpBack,
        InformationBoxOpen,
        BJHelpClose,
        GameOut,
    }

    public ButtonList ButtonList_Control;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        if (!Cash_Control.OnBetClick)
        {
            //錢幣
            #region Coin
            if (ButtonList_Control == ButtonList.Coin1)
            {
                BJMainGame_Control.SelectCoin = 1000;
            }
            else if (ButtonList_Control == ButtonList.Coin2)
            {
                BJMainGame_Control.SelectCoin = 5000;
            }
            else if (ButtonList_Control == ButtonList.Coin3)
            {
                BJMainGame_Control.SelectCoin = 10000;
            }
            else if (ButtonList_Control == ButtonList.Coin4)
            {
                BJMainGame_Control.SelectCoin = 100000;
            }
            #endregion

            //押注確定,取消
            #region Enter_Cancel
            if (ButtonList_Control == ButtonList.BetEnter)
            {
                if (Cash_Control.TableCash[0] != 0)
                {
                    CPACK_BlackJack_ReqBet Data = new CPACK_BlackJack_ReqBet();
                    Data.m_iAddBet = (int)Cash_Control.TableCash[0];

                    GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.BlackJack, (uint)ENUM_BlackJack_PACKID_GC.C2G_Game_ReqBet, GameConnet.m_oGameClient.DoSerialize<CPACK_BlackJack_ReqBet>(Data));
                    Cash_Control.OnBetClick = true;
                }
            }
            else if (ButtonList_Control == ButtonList.BetCancel)
            {
                if (!BJMainGame_Control.BetCancel_Bool)
                    BJMainGame_Control.BetCancel_Bool = true;
            }
            #endregion

            //要牌,加倍押注,要牌結束,投降,分牌
            #region Bet
            if (ButtonList_Control == ButtonList.Deal)
            {
                GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.BlackJack, (uint)ENUM_BlackJack_PACKID_GC.C2G_Game_HIT, null);
                Cash_Control.OnBetClick = true;
            }
            if (ButtonList_Control == ButtonList.Double)
            {
                GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.BlackJack, (uint)ENUM_BlackJack_PACKID_GC.C2G_Game_DOUBLE, null);
                Cash_Control.OnBetClick = true;
            }
            if (ButtonList_Control == ButtonList.DealEnd)
            {
                GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.BlackJack, (uint)ENUM_BlackJack_PACKID_GC.C2G_Game_STAND, null);
                Cash_Control.OnBetClick = true;
            }
            if (ButtonList_Control == ButtonList.Capitulate)
            {
                GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.BlackJack, (uint)ENUM_BlackJack_PACKID_GC.C2G_Game_SURRENDER, null);
                Cash_Control.OnBetClick = true;
                Debug.Log("投降按鈕");
            }
            //分牌
            if (ButtonList_Control == ButtonList.Scoreboard)
            {
                if (((long)BJMainGame_Control.MyMoney - (BJMainGame_Control.BetMoney * 2)) >=0)
                {
                    GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.BlackJack, (uint)ENUM_BlackJack_PACKID_GC.C2G_Game_SPLIT, null);
                    Cash_Control.OnBetClick = true;
                }
                else
                {
                    Message_Control.OpenMessage = true;
                    Message_Control.MessageStatus = Message_Control.MessageStatu.NoBetMoney;
                }
                Debug.Log("分牌按鈕");
            }
            if (ButtonList_Control == ButtonList.NoScoreboard)
            {
                ButtonSprite_Control.Scoreboard_Bool = false;
            }
            //報到
            if (ButtonList_Control == ButtonList.BJ21)
            {
                GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.BlackJack, (uint)ENUM_BlackJack_PACKID_GC.C2G_Game_BLACKJACK, null);
                Cash_Control.OnBetClick = true;
                Debug.Log("報到按鈕");
            }
            if (ButtonList_Control == ButtonList.NoBJ21)
            {
                ButtonSprite_Control.BJ21_Bool = false;
            }
            //保險
            if (ButtonList_Control == ButtonList.BetInsure)
            {
                GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.BlackJack, (uint)ENUM_BlackJack_PACKID_GC.C2G_Game_INSURE, null);
                Cash_Control.OnBetClick = true;
                Debug.Log("保險按鈕");
            }
            if (ButtonList_Control == ButtonList.NoInsure)
            {
                BJMainGame_Control.NowStateSave = ENUM_BLACKJACK_TABLE_STATE.InsuranceOver;
                BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState = ENUM_BLACKJACK_TABLE_STATE.InsuranceOver;
                ButtonSprite_Control.BetInsure_Bool = false;
            }
            #endregion

            //UI相關
            #region UI
            if (ButtonList_Control == ButtonList.Back)
            {
                GameConnet.BuyOut_GameLobbySuccess = true;
            }
            if (ButtonList_Control == ButtonList.InformationOpen)
            {
                if (!BJHistory_Control.HistoryOpen_Bool)
                {
                    BJHistory_Control.HistoryOpen_Bool = true;
                    BJHistory_Control.PagePlanning_Bool = true;
                }
                else
                    BJHistory_Control.HistoryOpen_Bool = false;

                BJHelp.BJHelpOpen_Bool = false;
                BJHelp.BarOpen_Bool = false;
            }
            if (ButtonList_Control == ButtonList.InformationCancel)
            {
                BJHistory_Control.HistoryOpen_Bool = false;
                BJHelp.BJHelpOpen_Bool = false;
            }
            if (ButtonList_Control == ButtonList.InformationNext)
            {
                if (!BJHistory_Control.PagePlanning_Bool)
                {
                    if ((BJHistory_Control.HistoryPage + 1) > ((uint)BJHistory_Control.History_Dic.Count / 8) + 1)
                    {
                        BJHistory_Control.HistoryPage = 1;
                    }
                    else
                    {
                        BJHistory_Control.HistoryPage++;
                    }
                    BJHistory_Control.PagePlanning_Bool = true;
                }
            }
            if (ButtonList_Control == ButtonList.InformationBack)
            {
                if (!BJHistory_Control.PagePlanning_Bool)
                {
                    if ((BJHistory_Control.HistoryPage - 1) < 1)
                    {
                        BJHistory_Control.HistoryPage = 1;
                    }
                    else
                    {
                        BJHistory_Control.HistoryPage--;
                    }
                    BJHistory_Control.PagePlanning_Bool = true;
                }
            }
            #endregion

            //押注
            #region Bet
            if (ButtonList_Control == ButtonList.OnBet)
            {
                if ((BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.NewRound
                    || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.WaitBet
                    || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.ShuffleNewRound) && !BJMainGame_Control.EnterBetBool)
                {
                    if (((long)BJMainGame_Control.MyMoney - (BJMainGame_Control.BetMoney + BJMainGame_Control.SelectCoin) >= 0))
                    {
                        Cash_Control.TableCash[0] += BJMainGame_Control.SelectCoin;
                        //  Cash_Control.OnBetClick = true;
                    }
                    else if ((long)BJMainGame_Control.MyMoney - (BJMainGame_Control.BetMoney + BJMainGame_Control.SelectCoin) < 0)
                    {
                        Message_Control.OpenMessage = true;
                        Message_Control.MessageStatus = Message_Control.MessageStatu.NoBetMoney;
                    }
                }
            }
            #endregion

            //關閉ENDWINDOW
            if (ButtonList_Control == ButtonList.GameEnd)
            {
                BJEndWindow_Control.EndWindow_Bool = false;
                StateShow_Control.Backgrond_Bool = true;
                BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState = ENUM_BLACKJACK_TABLE_STATE.GameOver;
            }

            //Help
            if (ButtonList_Control == ButtonList.BJGameHelp)
            {
                if (BJHelp.BJHelpOpen_Bool) 
                    BJHelp.BJHelpOpen_Bool = false;
                else
                    BJHelp.BJHelpOpen_Bool = true;

                BJHistory_Control.HistoryOpen_Bool = false;
                BJHelp.BarOpen_Bool = false;
            }

            //Help
            if (ButtonList_Control == ButtonList.BJHelpNext)
            {
                if (BJHelp.BJHelpPageNumber < 8)
                    BJHelp.BJHelpPageNumber++;
                else            
                    BJHelp.BJHelpPageNumber = 1;
            }

            //Help
            if (ButtonList_Control == ButtonList.BJHelpBack)
            {
                if (BJHelp.BJHelpPageNumber > 1)
                    BJHelp.BJHelpPageNumber--;
                else
                    BJHelp.BJHelpPageNumber = 8;
            }
            //Help
            if (ButtonList_Control == ButtonList.BJHelpClose)
            {
                BJHelp.BJHelpOpen_Bool = false;
            }

            if (ButtonList_Control == ButtonList.InformationBoxOpen)
            {
                if (BJHelp.BarOpen_Bool)
                    BJHelp.BarOpen_Bool = false;
                else
                    BJHelp.BarOpen_Bool = true;

                BJHelp.BJHelpOpen_Bool = false;
                BJHistory_Control.HistoryOpen_Bool = false;
            }

            if (ButtonList_Control == ButtonList.GameOut)
            {
                GameConnet.CloseGameConnet();
            }
        }
    }
}
