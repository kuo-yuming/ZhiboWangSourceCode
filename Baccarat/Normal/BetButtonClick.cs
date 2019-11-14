using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore.Manager.Baccarat;
using GameCore;
using GameEnum;
public class BetButtonClick : MonoBehaviour {

    public ENUM_PUBLIC_BUTTON EnumButton;
    public static uint ClickNumber = 0;

    void OnClick()
    {
        ///////押注按鈕設定
        if (!Bet_Control.BetClickBool && !Bet_Control.BetDataGetBool && MainGame_Control.StopModeState != ENUM_STOPMODE_STATE.ShuffleTimeShow && MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.WaitStop && !AutoMode_Control.StartAutoBetBool &&
            (MainGame_Control.NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.NewRound || MainGame_Control.NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.ShuffleNewRound || MainGame_Control.NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.WaitBet))
        {
            if (EnumButton == ENUM_PUBLIC_BUTTON.Banker || EnumButton == ENUM_PUBLIC_BUTTON.Player || EnumButton == ENUM_PUBLIC_BUTTON.Draw || EnumButton == ENUM_PUBLIC_BUTTON.BankerPair || EnumButton == ENUM_PUBLIC_BUTTON.PlayerPair || EnumButton == ENUM_PUBLIC_BUTTON.BetClear)
            {
                if (EnumButton == ENUM_PUBLIC_BUTTON.Banker)
                {
                    Bet_Control.BetSeat = (byte)ENUM_BACCARAT_AWARD_AREA.Banker;
                    Debug.Log("BetBanker");
                }
                else if (EnumButton == ENUM_PUBLIC_BUTTON.Player)
                {
                    Bet_Control.BetSeat = (byte)ENUM_BACCARAT_AWARD_AREA.Player;
                    Debug.Log("BetPlayer");
                }
                else if (EnumButton == ENUM_PUBLIC_BUTTON.Draw)
                {
                    Bet_Control.BetSeat = (byte)ENUM_BACCARAT_AWARD_AREA.Draw;
                    Debug.Log("BetDraw");
                }
                else if (EnumButton == ENUM_PUBLIC_BUTTON.BankerPair)
                {
                    Bet_Control.BetSeat = (byte)ENUM_BACCARAT_AWARD_AREA.BankerPair;
                    Debug.Log("BetBankerPair");
                }
                else if (EnumButton == ENUM_PUBLIC_BUTTON.PlayerPair)
                {
                    Bet_Control.BetSeat = (byte)ENUM_BACCARAT_AWARD_AREA.PlayerPair;
                    Debug.Log("BetPlayerPair");
                }
                else if (EnumButton == ENUM_PUBLIC_BUTTON.BetClear)
                {
                    Bet_Control.BetSeat = (byte)ENUM_PUBLIC_BUTTON.BetClear;
                    Debug.Log("BetClear");
                }
                Bet_Control.BetClickMoney = (int)CashButtonClick.SelectCash;
                Bet_Control.BetClickBool = true;
                Debug.Log("押注金額: " + Bet_Control.BetClickMoney);
            }
        }


        ////離開遊戲按鈕
        if (EnumButton == ENUM_PUBLIC_BUTTON.GameOut)
        {
            //Message_Control.OpenMessage = true;
            //Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
            //Message_Control.MessageStatus = Message_Control.MessageStatu.BuyOut_Ckeck;
            GameConnet.BuyOut_GameLobbySuccess = true;
        }


        ////AUTO按鈕設定
        if (EnumButton == ENUM_PUBLIC_BUTTON.AutoPlus)
        {
            AutoMode_Control.AutoModeNumber++;
            if (AutoMode_Control.AutoModeNumber > 1000)
            {
                AutoMode_Control.AutoModeNumber = 0;
            }
        }

        if (EnumButton == ENUM_PUBLIC_BUTTON.AutoMinus)
        {
            AutoMode_Control.AutoModeNumber--;
            if (AutoMode_Control.AutoModeNumber < 0)
            {
                AutoMode_Control.AutoModeNumber = 1000;
            }
        }

        if (!MainGame_Control.AutoAndInfoClickBool)
        {
            if (EnumButton == ENUM_PUBLIC_BUTTON.AutoModeButton)
            {
                if (AutoMode_Control.AutoModeOpen)
                {
                    AutoMode_Control.AutoModeOpen = false;
                }
                else
                {
                    AutoMode_Control.AutoModeOpen = true;
                    if (Info_Control.InfoButtonClickBool)
                    {
                        Info_Control.InfoButtonClickBool = false;
                    }
                }
                MainGame_Control.AutoAndInfoClickBool = true;
            }
            else if (EnumButton == ENUM_PUBLIC_BUTTON.InfoButton)
            {
                if (Info_Control.InfoButtonClickBool)
                {
                    Info_Control.InfoButtonClickBool = false;
                }
                else
                {
                    Info_Control.InfoButtonClickBool = true;
                    if (AutoMode_Control.AutoModeOpen)
                    {
                        AutoMode_Control.AutoModeOpen = false;
                    }
                }
                MainGame_Control.AutoAndInfoClickBool = true;
            }
        }

        /////////歷史紀錄按鈕
        if (EnumButton == ENUM_PUBLIC_BUTTON.HistroyButton)
        {
            if (History_Control.HistoryOpenBool)
            {
                History_Control.HistoryOpenBool = false;
                FourCardHistory_Control.FCHistoryClickBool = false;
            }
            else if (!History_Control.HistoryOpenBool && Info_Control.InfoButtonClickBool)
            {
                Info_Control.InfoButtonClickBool = false;
                History_Control.HistoryOpenBool = true;
            }
            Help_Control.HelpOpen_Bool = false;
            Race_Control.RaceRankingOpen = false;
        }

        //////////金額總結視窗按鈕
        if (EnumButton == ENUM_PUBLIC_BUTTON.EndButton)
        {
            EndWindow_Control.EndWindowOpenBool = false;
        }

        /////////競標莊閒按鈕
        //競標莊
        if (EnumButton == ENUM_PUBLIC_BUTTON.FourCardBankerButton)
        {
            if (MainGame_Control.BaccactFCBetBool)
            {
                if (!FourCard_Control.FourCardBet && MainGame_Control.NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.StartBid && FourCard_Control.FCBetOK_Bool)
                {
                    Bet_Control.BetClickMoney = (int)CashButtonClick.SelectCash;
                    if (((Money_Control.MyMoney - (ulong)Money_Control.MyBetMoney) + (ulong)Bet_Control.BetClickMoney) >= 0)
                    {
                        CPACK_Baccarat_ReqBet Data = new CPACK_Baccarat_ReqBet();
                        Data.m_byAreaID = (byte)ENUM_BACCARAT_AWARD_AREA.Banker;
                        Data.m_iAddBet = Bet_Control.BetClickMoney;
                        GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Baccarat, (uint)ENUM_BACCARAT_PACKID_GC.C2G_Game_ReqBid, GameConnet.m_oGameClient.DoSerialize<CPACK_Baccarat_ReqBet>(Data));
                        Debug.Log("競標押注成功: 莊" + " 押注區域: " + Data.m_byAreaID + " 押注金額: " + Data.m_iAddBet);
                        FourCard_Control.FourCardBet = true;
                    }
                    else
                    {
                        Message_Control.OpenMessage = true;
                        Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                        Message_Control.MessageStatus = Message_Control.MessageStatu.NoBetMoney;
                    }
                }
            }
            else
            {
                Message_Control.OpenMessage = true;
                Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                Message_Control.MessageStatus = Message_Control.MessageStatu.FCNoBet;
            }
        }

        //競標閒
        if (EnumButton == ENUM_PUBLIC_BUTTON.FourCardPlayerButton)
        {
            if (MainGame_Control.BaccactFCBetBool)
            {
                if (!FourCard_Control.FourCardBet && MainGame_Control.NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.StartBid && FourCard_Control.FCBetOK_Bool)
                {
                    Bet_Control.BetClickMoney = (int)CashButtonClick.SelectCash;
                    if (((Money_Control.MyMoney - (ulong)Money_Control.MyBetMoney) + (ulong)Bet_Control.BetClickMoney) >= 0)
                    {
                        CPACK_Baccarat_ReqBet Data = new CPACK_Baccarat_ReqBet();
                        Data.m_byAreaID = (byte)ENUM_BACCARAT_AWARD_AREA.Player;
                        Data.m_iAddBet = Bet_Control.BetClickMoney;
                        GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Baccarat, (uint)ENUM_BACCARAT_PACKID_GC.C2G_Game_ReqBid, GameConnet.m_oGameClient.DoSerialize<CPACK_Baccarat_ReqBet>(Data));
                        Debug.Log("競標押注成功: 閒" + " 押注區域: " + Data.m_byAreaID + " 押注金額: " + Data.m_iAddBet);
                        FourCard_Control.FourCardBet = true;
                    }
                    else
                    {
                        Message_Control.OpenMessage = true;
                        Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                        Message_Control.MessageStatus = Message_Control.MessageStatu.NoBetMoney;
                    }
                }
            }
            else
            {
                Message_Control.OpenMessage = true;
                Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                Message_Control.MessageStatus = Message_Control.MessageStatu.FCNoBet;
            }
        }

        //競標清除
        if (EnumButton == ENUM_PUBLIC_BUTTON.FourCardBetClear)
        {
            if (!FourCard_Control.FourCardBet && MainGame_Control.NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.StartBid)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (FourCard_Control.BetMoney[i] != 0)
                    {
                        CPACK_Baccarat_ReqBet Data = new CPACK_Baccarat_ReqBet();
                        if (i == 0)
                        {
                            Data.m_byAreaID = (byte)ENUM_BACCARAT_AWARD_AREA.Banker;
                            Debug.Log("競標清除成功: 莊" + " 押注區域: " + Data.m_byAreaID + " 押注金額: " + Data.m_iAddBet);
                        }
                        else if (i == 1)
                        {
                            Data.m_byAreaID = (byte)ENUM_BACCARAT_AWARD_AREA.Player;
                            Debug.Log("競標清除成功: 閒" + " 押注區域: " + Data.m_byAreaID + " 押注金額: " + Data.m_iAddBet);
                        }

                        Data.m_iAddBet = -(int)FourCard_Control.BetMoney[i];
                        GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Baccarat, (uint)ENUM_BACCARAT_PACKID_GC.C2G_Game_ReqBid, GameConnet.m_oGameClient.DoSerialize<CPACK_Baccarat_ReqBet>(Data));

                        FourCard_Control.FourCardBet = true;
                    }
                }
            }
        }

        if (EnumButton == ENUM_PUBLIC_BUTTON.FourCardButtonClick)
        {
            if (FourCardHistory_Control.FCHistoryClickBool)
            {
                FourCardHistory_Control.FCHistoryClickBool = false;
            }
            else
            {
                FourCardHistory_Control.FCHistoryClickBool = true;
            }
        }

        //遊戲說明
        if (EnumButton == ENUM_PUBLIC_BUTTON.Help && Info_Control.InfoButtonClickBool)
        {
            History_Control.HistoryOpenBool = false;
            Info_Control.InfoButtonClickBool = false;
            Race_Control.RaceRankingOpen = false;
            Help_Control.HelpOpen_Bool = true;
        }

        if (EnumButton == ENUM_PUBLIC_BUTTON.HelpOut)
        {
            Help_Control.HelpOpen_Bool = false;
        }

        if (EnumButton == ENUM_PUBLIC_BUTTON.HelpNext)
        {
            if (MainGame_Control.FourCardPlay_Bool)
            {
                if (Help_Control.HelpePage < 11)
                {
                    Help_Control.HelpePage++;
                }
                else
                {
                    Help_Control.HelpePage = 1;
                }
            }
            else
            {
                if (Help_Control.HelpePage < 9)
                {
                    Help_Control.HelpePage++;
                }
                else
                {
                    Help_Control.HelpePage = 1;
                }
            }
            // Help_Control.HelpePage++;
        }

        if (EnumButton == ENUM_PUBLIC_BUTTON.HelpBack)
        {
            if (MainGame_Control.FourCardPlay_Bool)
            {
                if (Help_Control.HelpePage > 1)
                {
                    Help_Control.HelpePage--;
                }
                else
                {
                    Help_Control.HelpePage = 11;
                }
            }
            else
            {
                if (Help_Control.HelpePage > 1)
                {
                    Help_Control.HelpePage--;
                }
                else
                {
                    Help_Control.HelpePage = 9;
                }
            }
        }

        if (EnumButton == ENUM_PUBLIC_BUTTON.HistoryButton2)
        {
            if (History_Control.HistoryOpenBool)
            {
                History_Control.HistoryOpenBool = false;

            }
            else if (!History_Control.HistoryOpenBool)
            {
                History_Control.HistoryOpenBool = true;
            }
            Help_Control.HelpOpen_Bool = false;
            FourCardHistory_Control.FCHistoryClickBool = false;
            Info_Control.InfoButtonClickBool = false;
            Race_Control.RaceRankingOpen = false;
        }

        if (EnumButton == ENUM_PUBLIC_BUTTON.RaceButton)
        {
            if (!Race_Control.RaceEnd_Bool)
            {
                if (Race_Control.RaceRankingOpen)
                {
                    Race_Control.RaceRankingOpen = false;
                }
                else if (!Race_Control.RaceRankingOpen)
                {
                    Race_Control.RaceRankingOpen = true;
                }
                History_Control.HistoryOpenBool = false;
                Help_Control.HelpOpen_Bool = false;
                FourCardHistory_Control.FCHistoryClickBool = false;
                Info_Control.InfoButtonClickBool = false;
            }
            else
            {
                if (MainGame_Control.StopModeState == GameEnum.ENUM_STOPMODE_STATE.EndShow)
                {
                    GameConnet.BuyOut_GameLobbySuccess = true;
                }
                else
                {
                    if (Race_Control.RaceRankingOpen)
                    {
                        Race_Control.RaceRankingOpen = false;
                    }
                    else if (!Race_Control.RaceRankingOpen)
                    {
                        Race_Control.RaceRankingOpen = true;
                    }
                    History_Control.HistoryOpenBool = false;
                    Help_Control.HelpOpen_Bool = false;
                    FourCardHistory_Control.FCHistoryClickBool = false;
                    Info_Control.InfoButtonClickBool = false;
                }
            }
        }
    }
}
