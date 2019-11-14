using UnityEngine;
using System.Collections;
using GameCore.Manager.Baccarat;
using GameCore;
using GameEnum;
public class Bet_Control : MonoBehaviour {
    public uint[] BetMaxMoney = new uint[5];
    public uint[] BetMinMoney = new uint[5];
    public static int BetClickMoney = 0;
    public static bool BetClickBool = false;
    public static bool BetDataGetBool = false;
    public static byte BetSeat = 0;
    public static byte BetID = 0;

    public UISprite[] BetButtonBut;
	// Use this for initialization
	void Start () {
        BetClickBool = false;
        BetDataGetBool = false;
        BetSeat = 0;
        BetClickMoney = 0;
        foreach (var item in BaccaratManager.m_GameConfig.m_dicBetLimit)
        {
            foreach (var item2 in item.Value.m_dicGroupBetLimit)
            {
                if (item.Key == (byte)ENUM_BACCARAT_AWARD_AREA.Banker && item2.Key == BaccaratManager.m_MachineTableArea[GameConnet.m_TMachineBuyInGameData.m_uiTID])
                {
                    BetMaxMoney[0] = item2.Value.m_uiMaxBet;
                    BetMinMoney[0] = item2.Value.m_uiMinBet;
                }
                else if (item.Key == (byte)ENUM_BACCARAT_AWARD_AREA.Player && item2.Key == BaccaratManager.m_MachineTableArea[GameConnet.m_TMachineBuyInGameData.m_uiTID])
                {
                    BetMaxMoney[1] = item2.Value.m_uiMaxBet;
                    BetMinMoney[1] = item2.Value.m_uiMinBet;
                }
                else if (item.Key == (byte)ENUM_BACCARAT_AWARD_AREA.Draw && item2.Key == BaccaratManager.m_MachineTableArea[GameConnet.m_TMachineBuyInGameData.m_uiTID])
                {
                    BetMaxMoney[2] = item2.Value.m_uiMaxBet;
                    BetMinMoney[2] = item2.Value.m_uiMinBet;
                }
                else if (item.Key == (byte)ENUM_BACCARAT_AWARD_AREA.BankerPair && item2.Key == BaccaratManager.m_MachineTableArea[GameConnet.m_TMachineBuyInGameData.m_uiTID])
                {
                    BetMaxMoney[3] = item2.Value.m_uiMaxBet;
                    BetMinMoney[3] = item2.Value.m_uiMinBet;
                }
                else if (item.Key == (byte)ENUM_BACCARAT_AWARD_AREA.PlayerPair && item2.Key == BaccaratManager.m_MachineTableArea[GameConnet.m_TMachineBuyInGameData.m_uiTID])
                {
                    BetMaxMoney[4] = item2.Value.m_uiMaxBet;
                    BetMinMoney[4] = item2.Value.m_uiMinBet;
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (BetClickBool && !BetDataGetBool)
        {
            BetClickVoid();
            BetClickBool = false;
        }

        BetButtonChangeVoid();
	}

    //押注金額資料傳送
    void BetClickVoid()
    {
        if (((long)Money_Control.MyMoney - (long)Money_Control.MyBetMoney - (long)BetClickMoney) >= 0 && BetSeat != (byte)ENUM_PUBLIC_BUTTON.BetClear)
        {
            if ((BetTable_Control.MyBetMoneySeat[0] + BetClickMoney) < 0)
            {
                Message_Control.OpenMessage = true;
                Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                Message_Control.MessageStatus = Message_Control.MessageStatu.NoBetMoney;
            }
            else if (BetSeat == (byte)ENUM_BACCARAT_AWARD_AREA.Banker)
            {
                if ((BetTable_Control.MyBetMoneySeat[0] + BetClickMoney) >= BetMinMoney[0])
                {
                    if ((BetTable_Control.MyBetMoneySeat[0] + BetClickMoney) <= BetMaxMoney[0])
                    {
                        BetDataGetBool = true;
                        CPACK_Baccarat_ReqBet Data = new CPACK_Baccarat_ReqBet();
                        Data.m_byAreaID = (byte)ENUM_BACCARAT_AWARD_AREA.Banker;
                        Data.m_iAddBet = BetClickMoney;
                        GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Baccarat, (uint)ENUM_BACCARAT_PACKID_GC.C2G_Game_ReqBet, GameConnet.m_oGameClient.DoSerialize<CPACK_Baccarat_ReqBet>(Data));
                        Debug.Log("押注成功: 莊" + " 押注區域: " + Data.m_byAreaID + " 押注金額: " + Data.m_iAddBet);
                    }
                    else
                    {
                        Message_Control.OpenMessage = true;
                        Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                        Message_Control.MessageStatus = Message_Control.MessageStatu.MaxBetOut;
                    }
                }
                else
                {
                    Message_Control.OpenMessage = true;
                    Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes; 
                    Message_Control.MessageStatus = Message_Control.MessageStatu.NoBetMinMoney;
                }
            }
            else if (BetSeat == (byte)ENUM_BACCARAT_AWARD_AREA.Player)
            {
                if ((BetTable_Control.MyBetMoneySeat[1] + BetClickMoney) < 0)
                {
                    Message_Control.OpenMessage = true;
                    Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                    Message_Control.MessageStatus = Message_Control.MessageStatu.NoBetMoney;
                }
                else if ((BetTable_Control.MyBetMoneySeat[1] + BetClickMoney) >= BetMinMoney[1])
                {
                    if ((BetTable_Control.MyBetMoneySeat[1] + BetClickMoney) <= BetMaxMoney[1])
                    {
                        BetDataGetBool = true;
                        CPACK_Baccarat_ReqBet Data = new CPACK_Baccarat_ReqBet();
                        Data.m_byAreaID = (byte)ENUM_BACCARAT_AWARD_AREA.Player;
                        Data.m_iAddBet = BetClickMoney;
                        GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Baccarat, (uint)ENUM_BACCARAT_PACKID_GC.C2G_Game_ReqBet, GameConnet.m_oGameClient.DoSerialize<CPACK_Baccarat_ReqBet>(Data));
                        Debug.Log("押注成功: 閒" + " 押注區域: " + Data.m_byAreaID + " 押注金額: " + Data.m_iAddBet);
                    }
                    else
                    {
                        Message_Control.OpenMessage = true;
                        Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                        Message_Control.MessageStatus = Message_Control.MessageStatu.MaxBetOut;
                    }
                }
                else
                {
                    Message_Control.OpenMessage = true;
                    Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                    Message_Control.MessageStatus = Message_Control.MessageStatu.NoBetMinMoney;
                }
            }
            else if (BetSeat == (byte)ENUM_BACCARAT_AWARD_AREA.Draw)
            {
                if ((BetTable_Control.MyBetMoneySeat[2] + BetClickMoney) < 0)
                {
                    Message_Control.OpenMessage = true;
                    Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                    Message_Control.MessageStatus = Message_Control.MessageStatu.NoBetMoney;
                }
                else if ((BetTable_Control.MyBetMoneySeat[2] + BetClickMoney) >= BetMinMoney[2])
                {
                    if ((BetTable_Control.MyBetMoneySeat[2] + BetClickMoney) <= BetMaxMoney[2])
                    {
                        BetDataGetBool = true;
                        CPACK_Baccarat_ReqBet Data = new CPACK_Baccarat_ReqBet();
                        Data.m_byAreaID = (byte)ENUM_BACCARAT_AWARD_AREA.Draw;
                        Data.m_iAddBet = BetClickMoney;
                        GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Baccarat, (uint)ENUM_BACCARAT_PACKID_GC.C2G_Game_ReqBet, GameConnet.m_oGameClient.DoSerialize<CPACK_Baccarat_ReqBet>(Data));
                        Debug.Log("押注成功: 和" + " 押注區域: " + Data.m_byAreaID + " 押注金額: " + Data.m_iAddBet);
                    }
                    else
                    {
                        Message_Control.OpenMessage = true;
                        Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                        Message_Control.MessageStatus = Message_Control.MessageStatu.MaxBetOut;
                    }
                }
                else
                {
                    Message_Control.OpenMessage = true;
                    Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                    Message_Control.MessageStatus = Message_Control.MessageStatu.NoBetMinMoney;
                }
            }
            else if (BetSeat == (byte)ENUM_BACCARAT_AWARD_AREA.BankerPair)
            {
                if ((BetTable_Control.MyBetMoneySeat[3] + BetClickMoney) < 0)
                {
                    Message_Control.OpenMessage = true;
                    Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                    Message_Control.MessageStatus = Message_Control.MessageStatu.NoBetMoney;
                }
                else if ((BetTable_Control.MyBetMoneySeat[3] + BetClickMoney) >= BetMinMoney[3])
                {
                    if ((BetTable_Control.MyBetMoneySeat[3] + BetClickMoney) <= BetMaxMoney[3])
                    {
                        BetDataGetBool = true;
                        CPACK_Baccarat_ReqBet Data = new CPACK_Baccarat_ReqBet();
                        Data.m_byAreaID = (byte)ENUM_BACCARAT_AWARD_AREA.BankerPair;
                        Data.m_iAddBet = BetClickMoney;
                        GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Baccarat, (uint)ENUM_BACCARAT_PACKID_GC.C2G_Game_ReqBet, GameConnet.m_oGameClient.DoSerialize<CPACK_Baccarat_ReqBet>(Data));
                        Debug.Log("押注成功: 莊對" + " 押注區域: " + Data.m_byAreaID + " 押注金額: " + Data.m_iAddBet);
                    }
                    else
                    {
                        Message_Control.OpenMessage = true;
                        Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                        Message_Control.MessageStatus = Message_Control.MessageStatu.MaxBetOut;
                    }
                }
                else
                {
                    Message_Control.OpenMessage = true;
                    Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                    Message_Control.MessageStatus = Message_Control.MessageStatu.NoBetMinMoney;
                }
            }
            else if (BetSeat == (byte)ENUM_BACCARAT_AWARD_AREA.PlayerPair)
            {
                if ((BetTable_Control.MyBetMoneySeat[4] + BetClickMoney) < 0)
                {
                    Message_Control.OpenMessage = true;
                    Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                    Message_Control.MessageStatus = Message_Control.MessageStatu.NoBetMoney;
                }
                else if ((BetTable_Control.MyBetMoneySeat[4] + BetClickMoney) >= BetMinMoney[4])
                {
                    if ((BetTable_Control.MyBetMoneySeat[4] + BetClickMoney) <= BetMaxMoney[4])
                    {
                        BetDataGetBool = true;
                        CPACK_Baccarat_ReqBet Data = new CPACK_Baccarat_ReqBet();
                        Data.m_byAreaID = (byte)ENUM_BACCARAT_AWARD_AREA.PlayerPair;
                        Data.m_iAddBet = BetClickMoney;
                        GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Baccarat, (uint)ENUM_BACCARAT_PACKID_GC.C2G_Game_ReqBet, GameConnet.m_oGameClient.DoSerialize<CPACK_Baccarat_ReqBet>(Data));
                        Debug.Log("押注成功: 閒對" + " 押注區域: " + Data.m_byAreaID + " 押注金額: " + Data.m_iAddBet);
                    }
                    else
                    {
                        Message_Control.OpenMessage = true;
                        Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                        Message_Control.MessageStatus = Message_Control.MessageStatu.MaxBetOut;
                    }
                }
                else
                {
                    Message_Control.OpenMessage = true;
                    Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                    Message_Control.MessageStatus = Message_Control.MessageStatu.NoBetMinMoney;
                }
            }
        }
        else if (BetSeat != (byte)ENUM_PUBLIC_BUTTON.BetClear)
        {
            //if (((BetTable_Control.MyBetMoneySeat[0] + BetClickMoney) < 0) || (BetTable_Control.MyBetMoneySeat[1] + BetClickMoney) < 0)
            //{
                Message_Control.OpenMessage = true;
                Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                Message_Control.MessageStatus = Message_Control.MessageStatu.NoBetMoney;
           // }
        }

        //取消
        if (BetSeat == (byte)ENUM_PUBLIC_BUTTON.BetClear)
        {

            if (BetTable_Control.MyBetMoneySeat[0] > 0)
            {
                BetDataGetBool = true;
                CPACK_Baccarat_ReqBet Data = new CPACK_Baccarat_ReqBet();
                Data.m_byAreaID = (byte)ENUM_BACCARAT_AWARD_AREA.Banker;
                Data.m_iAddBet = -(int)BetTable_Control.MyBetMoneySeat[0];
                GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Baccarat, (uint)ENUM_BACCARAT_PACKID_GC.C2G_Game_ReqBet, GameConnet.m_oGameClient.DoSerialize<CPACK_Baccarat_ReqBet>(Data));
                Debug.Log("莊押注取消: " + -(int)BetTable_Control.MyBetMoneySeat[0]);
            }

            if (BetTable_Control.MyBetMoneySeat[1] > 0)
            {
                BetDataGetBool = true;
                CPACK_Baccarat_ReqBet Data = new CPACK_Baccarat_ReqBet();
                Data.m_byAreaID = (byte)ENUM_BACCARAT_AWARD_AREA.Player;
                Data.m_iAddBet = -(int)BetTable_Control.MyBetMoneySeat[1];
                GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Baccarat, (uint)ENUM_BACCARAT_PACKID_GC.C2G_Game_ReqBet, GameConnet.m_oGameClient.DoSerialize<CPACK_Baccarat_ReqBet>(Data));
            }

            if (BetTable_Control.MyBetMoneySeat[2] > 0)
            {
                BetDataGetBool = true;
                CPACK_Baccarat_ReqBet Data = new CPACK_Baccarat_ReqBet();
                Data.m_byAreaID = (byte)ENUM_BACCARAT_AWARD_AREA.Draw;
                Data.m_iAddBet = -(int)BetTable_Control.MyBetMoneySeat[2];
                GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Baccarat, (uint)ENUM_BACCARAT_PACKID_GC.C2G_Game_ReqBet, GameConnet.m_oGameClient.DoSerialize<CPACK_Baccarat_ReqBet>(Data));
            }

            if (BetTable_Control.MyBetMoneySeat[3] > 0)
            {
                BetDataGetBool = true;
                CPACK_Baccarat_ReqBet Data = new CPACK_Baccarat_ReqBet();
                Data.m_byAreaID = (byte)ENUM_BACCARAT_AWARD_AREA.BankerPair;
                Data.m_iAddBet = -(int)BetTable_Control.MyBetMoneySeat[3];
                GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Baccarat, (uint)ENUM_BACCARAT_PACKID_GC.C2G_Game_ReqBet, GameConnet.m_oGameClient.DoSerialize<CPACK_Baccarat_ReqBet>(Data));
            }

            if (BetTable_Control.MyBetMoneySeat[4] > 0)
            {
                BetDataGetBool = true;
                CPACK_Baccarat_ReqBet Data = new CPACK_Baccarat_ReqBet();
                Data.m_byAreaID = (byte)ENUM_BACCARAT_AWARD_AREA.PlayerPair;
                Data.m_iAddBet = -(int)BetTable_Control.MyBetMoneySeat[4];
                GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Baccarat, (uint)ENUM_BACCARAT_PACKID_GC.C2G_Game_ReqBet, GameConnet.m_oGameClient.DoSerialize<CPACK_Baccarat_ReqBet>(Data));
            }
        }
    }

    //各押注區按鈕變化
    void BetButtonChangeVoid()
    {
        if (MainGame_Control.StopModeState != ENUM_STOPMODE_STATE.ShuffleTimeShow && (MainGame_Control.NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.NewRound || MainGame_Control.NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.ShuffleNewRound || MainGame_Control.NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.WaitBet))
        {
            BetButtonBut[0].enabled = true;
            BetButtonBut[1].enabled = true;
            BetButtonBut[2].enabled = true;
            BetButtonBut[3].enabled = true;
            BetButtonBut[4].enabled = true;
        }
        else
        {
            BetButtonBut[0].enabled = false;
            BetButtonBut[1].enabled = false;
            BetButtonBut[2].enabled = false;
            BetButtonBut[3].enabled = false;
            BetButtonBut[4].enabled = false;
        }
    }
}
