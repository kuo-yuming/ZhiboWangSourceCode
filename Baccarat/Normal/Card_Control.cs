using UnityEngine;
using System.Collections;
using GameCore.Manager.Baccarat;
using GameEnum;
using System.Collections.Generic;

public class Card_Control : MonoBehaviour
{
    public static byte[] BankerCard = new byte[3];//莊家Card 0:第一張 1:第二張 2:第三張
    public static byte[] PlayerCard = new byte[3];//閒家Card 0:第一張 1:第二張 2:第三張

    public UISprite[] BankerCardSprite;
    public UISprite[] PlayerCardSprite;
    public GameObject[] SituationMsg_Object = new GameObject[2];
    public UILabel SituationMsg_Label1;
    public UILabel SituationMsg_Label2;

    public GameObject EndWindow;
    public UILabel[] EndWindow_Label;

    public static byte[] SaveBankerPoint = new byte[3];
    public static byte[] SaveplayerPoint = new byte[3];

    public static bool CardDataGetBool = false;
    public static bool CardShowOverBool = false;
    public static byte[] BankerCardOpenNumber = new byte[3];
    public static byte[] PlayerCardOpenNumber = new byte[3];

    float DelayTimer = 0;
    public static float DelayTimerMax = 2;
    float DelayTimer2 = 0;
    float DelayTimer3 = 0;
    bool EndCheckBool = true;

    public TweenPosition[] EndWnodwShow = new TweenPosition[2];
    public GameObject MoveCardGameObject;
    public GameObject OpenCardGameObject;
    public GameObject StartOpenGameObject;

    public static byte TotalBankerPoint = 0;
    public static byte TotalPlayerPoint = 0;
    public static Dictionary<byte, CPACK_Baccarat_NotifyAward> SaveFinallData = new Dictionary<byte, CPACK_Baccarat_NotifyAward>();
    public static bool CardPointOK = false;
    public static bool FinallDataOk = false;
    // Use this for initialization
    void Start()
    {
        CardDataGetBool = false;
        CardShowOverBool = false;
        CardEndShowVoid();
        SituationMsg_Object[0].SetActive(false);
        SituationMsg_Object[1].SetActive(false);
        EndWindow.SetActive(false);
        if (!VersionDef.BaccaratCardSize)
        {
            MoveCardGameObject.transform.localPosition = new Vector3(0, -2, 0);
            OpenCardGameObject.transform.localPosition = new Vector3(0, -2, 0);
            OpenCardGameObject.transform.localScale = new Vector3(1.02f, 1.05f, 1);
            StartOpenGameObject.transform.localPosition = new Vector3(0, 100, 0);
        }
        else
        {
            MoveCardGameObject.transform.localPosition = new Vector3(0, 0, 0);
            OpenCardGameObject.transform.localPosition = new Vector3(0, 0, 0);
            OpenCardGameObject.transform.localScale = new Vector3(2.04f, 2.1f, 1);
            StartOpenGameObject.transform.localPosition = new Vector3(0, 205, 0);
        }
        SaveFinallData.Clear();
        CardPointOK = false;
        FinallDataOk = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (CardPointOK && FinallDataOk)
        {
            FinallVoid();
            CardPointOK = false;
            FinallDataOk = false;
            SaveFinallData.Clear();
        }
        if (CardDataGetBool)
        {
            CardNumberVoid();
            CardDataGetBool = false;
        }

        if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.WaitStop)
        {
            SituationMsg_Object[0].SetActive(false);
            SituationMsg_Object[1].SetActive(false);
            EndWnodwShow[0].ResetToBeginning();
            EndWnodwShow[1].ResetToBeginning();
            EndWindow.SetActive(false);
            EndCheckBool = true;
            for (int i = 0; i < 3; i++)
            {
                BankerCardOpenNumber[i] = 0;
                PlayerCardOpenNumber[i] = 0;
            }
        }

        if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.EndShow)
        {
            for (int i = 0; i < 3; i++)
            {
                BankerCardSprite[i].enabled = false;
                PlayerCardSprite[i].enabled = false;
            }
        }

        if (MainGame_Control.StopModeState != ENUM_STOPMODE_STATE.WaitNextNewRound)
        {
            if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.CardShow)
            {
                CardShowVoid();
                CardOpenVoid();
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                BankerCardSprite[i].enabled = false;
                PlayerCardSprite[i].enabled = false;
            }
        }

        if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.EndShow && EndCheckBool)
        {
            if (((SaveBankerPoint[0] + SaveBankerPoint[1] + SaveBankerPoint[2]) % 10) > ((SaveplayerPoint[0] + SaveplayerPoint[1] + SaveplayerPoint[2]) % 10))
            {            
                MainGame_Control.BankerWinPoint++;
            }
            else if (((SaveBankerPoint[0] + SaveBankerPoint[1] + SaveBankerPoint[2]) % 10) < ((SaveplayerPoint[0] + SaveplayerPoint[1] + SaveplayerPoint[2]) % 10))
            {
                MainGame_Control.PlayerWinPoint++;
            }
            else if (((SaveBankerPoint[0] + SaveBankerPoint[1] + SaveBankerPoint[2]) % 10) == ((SaveplayerPoint[0] + SaveplayerPoint[1] + SaveplayerPoint[2]) % 10))
            {
                MainGame_Control.DrawWinPoint++;
            }

            if (MainGame_Control.SaveBankerPair)
            {
                MainGame_Control.BankerPairWinPoint++;
            }

            if (MainGame_Control.SavePlayerPair)
            {
                MainGame_Control.PlayerPairWinPoint++;
            }
            History_Control.HistoryStart_Bool = true;
            EndCheckBool = false;
        }
    }

    //卡片顯示編號
    void CardNumberVoid()
    {
        for (int i = 0; i < 3; i++)
        {
            if (PlayerCard[i] < 10)
            {
                PlayerCardSprite[i].spriteName = "0" + PlayerCard[i].ToString();
            }
            else
            {
                PlayerCardSprite[i].spriteName = PlayerCard[i].ToString();
            }

            if (BankerCard[i] < 10)
            {
                BankerCardSprite[i].spriteName = "0" + BankerCard[i].ToString();
            }
            else
            {
                BankerCardSprite[i].spriteName = BankerCard[i].ToString();
            }
        }

        //莊家
        if ((((BankerCard[0] - 1) % 13) + 1) > 9)
        {
            SaveBankerPoint[0] = 0;
        }
        else
        {
            SaveBankerPoint[0] = (byte)(((BankerCard[0] - 1) % 13) + 1);
        }

        if ((((BankerCard[1] - 1) % 13) + 1) > 9)
        {
            SaveBankerPoint[1] = 0;
        }
        else
        {
            SaveBankerPoint[1] = (byte)(((BankerCard[1] - 1) % 13) + 1);
        }

        if ((((BankerCard[2] - 1) % 13) + 1) > 9)
        {
            SaveBankerPoint[2] = 0;
        }
        else
        {
            SaveBankerPoint[2] = (byte)(((BankerCard[2] - 1) % 13) + 1);
        }

        //閒家
        if ((((PlayerCard[0] - 1) % 13) + 1) > 9)
        {
            SaveplayerPoint[0] = 0;
        }
        else
        {
            SaveplayerPoint[0] = (byte)(((PlayerCard[0] - 1) % 13) + 1);
        }

        if ((((PlayerCard[1] - 1) % 13) + 1) > 9)
        {
            SaveplayerPoint[1] = 0;
        }
        else
        {
            SaveplayerPoint[1] = (byte)(((PlayerCard[1] - 1) % 13) + 1);
        }

        if ((((PlayerCard[2] - 1) % 13) + 1) > 9)
        {
            SaveplayerPoint[2] = 0;
        }
        else
        {
            SaveplayerPoint[2] = (byte)(((PlayerCard[2] - 1) % 13) + 1);
        }

        TotalBankerPoint = (byte)((SaveBankerPoint[0] + SaveBankerPoint[1] + SaveBankerPoint[2]) % 10);
        TotalPlayerPoint = (byte)((SaveplayerPoint[0] + SaveplayerPoint[1] + SaveplayerPoint[2]) % 10);

        if (TotalBankerPoint > TotalPlayerPoint)
        {
            MainGame_Control.SaveWin = ENUM_BACCARAT_AWARD.WinBanker;
        }
        else if (TotalBankerPoint < TotalPlayerPoint)
        {
            MainGame_Control.SaveWin = ENUM_BACCARAT_AWARD.WinPlayer;
        }
        else if (TotalBankerPoint == TotalPlayerPoint)
        {
            MainGame_Control.SaveWin = ENUM_BACCARAT_AWARD.WinDraw;
        }

        Debug.Log("莊家牌點數: " + SaveBankerPoint[0] + "," + SaveBankerPoint[1] + "," + SaveBankerPoint[2]);
        Debug.Log("閒家牌點數: " + SaveplayerPoint[0] + "," + SaveplayerPoint[1] + "," + SaveplayerPoint[2]);
        Debug.Log("莊家和: " + TotalBankerPoint + "," + " //閒家和: " + TotalPlayerPoint + " //結果: " + MainGame_Control.SaveWin);
        CardPointOK = true;
        MainGame_Control.GetNormalWinDataBool = true;
    }

    //卡片表演
    void CardShowVoid()
    {
        if (DelayTimer < DelayTimerMax && !CardShowOverBool && !CardOpen_Control.CardAnimationShow_Bool)
        {
            DelayTimer += Time.deltaTime;
            if (DelayTimer > 0.5f)
            {
                SituationMsgVoid();
            }
        }
        else if (DelayTimer != 0)
        {
            DelayTimer = 0;
            SituationMsg_Object[0].SetActive(false);
            SituationMsg_Object[1].SetActive(false);
            CardOpen_Control.CardAnimationShow_Bool = true;
            MainGame_Control.LeftOverCardPoint--;
        }

        if (CardShowOverBool)
        {
            if (DelayTimer2 < 0.5f)
            {
                DelayTimer2 += Time.deltaTime;
            }
            else
            {
                if (FourCard_Control.FourCard != 0)
                {
                    if (MainGame_Control.NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.StartBid)
                    {
                        CardShowOverBool = false;
                        DelayTimer2 = 0;
                    }
                    else if (MainGame_Control.NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.StopBid)
                    {
                        CardShowOverBool = false;
                        DelayTimer2 = 0;
                    }
                    if (!FourBidStart.FourBidStart_Bool)
                    {
                        FourBidStart.FourBidStart_Bool = true;
                    }
                }
                else
                {
                    if (DelayTimer3 < 5)
                    {
                        DelayTimer3 += Time.deltaTime;
                        EndWindow.SetActive(true);
                        EndWnodwShow[0].PlayForward();
                        EndWnodwShow[1].PlayForward();
                        EndWindow_Label[0].text = GameSound.BankerPoint.ToString();
                        EndWindow_Label[1].text = GameSound.PlayerPoint.ToString();

                        if (MainGame_Control.LastWin == ENUM_BACCARAT_AWARD.WinBanker && MainGame_Control.WinArea[2] != 1)
                        {
                            if (VersionDef.InternationalLanguageSystem)
                            {
                                EndWindow_Label[2].text = Font_Control.Instance.m_dicMsgStr[2008049];
                            }
                            else
                            {
                                EndWindow_Label[2].text = "莊贏";
                            }
                           
                        }
                        else if (MainGame_Control.LastWin == ENUM_BACCARAT_AWARD.WinPlayer && MainGame_Control.WinArea[2] != 1)
                        {
                            if (VersionDef.InternationalLanguageSystem)
                            {
                                EndWindow_Label[2].text = Font_Control.Instance.m_dicMsgStr[2008050];
                            }
                            else
                            {
                                EndWindow_Label[2].text = "閒贏";
                            }
                        }
                        else if (MainGame_Control.LastWin == ENUM_BACCARAT_AWARD.WinDraw)
                        {
                            if (VersionDef.InternationalLanguageSystem)
                            {
                                EndWindow_Label[2].text = Font_Control.Instance.m_dicMsgStr[2008051];
                            }
                            else
                            {
                                EndWindow_Label[2].text = "平和";
                            }
                        }
                    }
                    else
                    {
                        EndWindow.SetActive(false);
                        MainGame_Control.StopModeState = ENUM_STOPMODE_STATE.MoneyShow;
                        CardShowOverBool = false;
                        DelayTimer2 = 0;
                        DelayTimer3 = 0;
                    }
                }
            }
        }
    }

    void CardOpenVoid()
    {
        if (BankerCardOpenNumber[0] == 1)
        {
            BankerCardSprite[0].enabled = true;
        }

        if (BankerCardOpenNumber[1] == 1)
        {
            BankerCardSprite[1].enabled = true;
        }

        if (BankerCardOpenNumber[2] == 1)
        {
            BankerCardSprite[2].enabled = true;
        }

        if (PlayerCardOpenNumber[0] == 1)
        {
            PlayerCardSprite[0].enabled = true;
        }

        if (PlayerCardOpenNumber[1] == 1)
        {
            PlayerCardSprite[1].enabled = true;
        }

        if (PlayerCardOpenNumber[2] == 1)
        {
            PlayerCardSprite[2].enabled = true;
        }
    }

    //卡片結束表演
    void CardEndShowVoid()
    {
        for (int i = 0; i < 3; i++)
        {
            BankerCardSprite[i].enabled = false;
            PlayerCardSprite[i].enabled = false;
            BankerCardOpenNumber[i] = 0;
            PlayerCardOpenNumber[i] = 0;
        }
    }

    void SituationMsgVoid()
    {
        if (CardOpen_Control.CardOpenSeat[3] != 0)
        {

        }
        else if (CardOpen_Control.CardOpenSeat[0] != 0)
        {

        }
        else if (CardOpen_Control.CardOpenSeat[4] != 0)
        {

        }
        else if (CardOpen_Control.CardOpenSeat[1] != 0)
        {

        }
        else if (CardOpen_Control.CardOpenSeat[5] != 0)
        {
            SituationMsg_Object[0].SetActive(true);
        }
        else if (CardOpen_Control.CardOpenSeat[2] != 0)
        {
            if (VersionDef.InternationalLanguageSystem)
            {
                SituationMsg_Label1.text = Font_Control.Instance.m_dicMsgStr[2008054] + "[72b8ff]" + ((SaveBankerPoint[0] + SaveBankerPoint[1]) % 10).ToString() + "[-]" + "[72b8ff]" + Font_Control.Instance.m_dicMsgStr[2008055] + "[-]";
            }
            else
            {
                SituationMsg_Label1.text = "莊家點數 " + "[72b8ff]" + ((SaveBankerPoint[0] + SaveBankerPoint[1]) % 10).ToString() + "[-]" + "[72b8ff]" + "點" + "[-]";
            }

            if (PlayerCard[2] == 0)
            {
                if (VersionDef.InternationalLanguageSystem)
                {
                    SituationMsg_Label2.text = Font_Control.Instance.m_dicMsgStr[2008056];
                }
                else
                {
                    SituationMsg_Label2.text = "莊家開出5點以下";
                }
            }
            else
            {
                if (((SaveBankerPoint[0] + SaveBankerPoint[1]) % 10) == 0)
                {
                    if (VersionDef.InternationalLanguageSystem)
                    {
                        SituationMsg_Label2.text = Font_Control.Instance.m_dicMsgStr[2008057];
                    }
                    else
                    {
                        SituationMsg_Label2.text = "總和2點以下";
                    }
                }
                else if (((SaveBankerPoint[0] + SaveBankerPoint[1]) % 10) == 1)
                {
                    if (VersionDef.InternationalLanguageSystem)
                    {
                        SituationMsg_Label2.text = Font_Control.Instance.m_dicMsgStr[2008057];
                    }
                    else
                    {
                        SituationMsg_Label2.text = "總和2點以下";
                    }
                }
                else if (((SaveBankerPoint[0] + SaveBankerPoint[1]) % 10) == 2)
                {
                    if (VersionDef.InternationalLanguageSystem)
                    {
                        SituationMsg_Label2.text = Font_Control.Instance.m_dicMsgStr[2008057];
                    }
                    else
                    {
                        SituationMsg_Label2.text = "總和2點以下";
                    }
                }
                else if (((SaveBankerPoint[0] + SaveBankerPoint[1]) % 10) == 3 && SaveplayerPoint[2] != 8)
                {
                    if (VersionDef.InternationalLanguageSystem)
                    {
                        SituationMsg_Label2.text = Font_Control.Instance.m_dicMsgStr[2008058];
                    }
                    else
                    {
                        SituationMsg_Label2.text = "閒家沒補牌或補牌非「8」點";
                    }
                }
                else if (((SaveBankerPoint[0] + SaveBankerPoint[1]) % 10) == 4 && SaveplayerPoint[2] != 0 && SaveplayerPoint[2] != 1 && SaveplayerPoint[2] != 8 && SaveplayerPoint[2] != 9)
                {
                    if (VersionDef.InternationalLanguageSystem)
                    {
                        SituationMsg_Label2.text = Font_Control.Instance.m_dicMsgStr[2008059];
                    }
                    else
                    {
                        SituationMsg_Label2.text = "閒家沒補牌或補牌非「0、1、8、9」點";
                    } 
                }
                else if (((SaveBankerPoint[0] + SaveBankerPoint[1]) % 10) == 5 && SaveplayerPoint[2] != 0 && SaveplayerPoint[2] != 1 && SaveplayerPoint[2] != 2 && SaveplayerPoint[2] != 3 && SaveplayerPoint[2] != 8 && SaveplayerPoint[2] != 9)
                {
                    if (VersionDef.InternationalLanguageSystem)
                    {
                        SituationMsg_Label2.text = Font_Control.Instance.m_dicMsgStr[2008060];
                    }
                    else
                    {
                        SituationMsg_Label2.text = "閒家沒補牌或補牌為「4~7」點";
                    }
                }
                else if (((SaveBankerPoint[0] + SaveBankerPoint[1]) % 10) == 6 && SaveplayerPoint[2] != 0 && SaveplayerPoint[2] != 1 && SaveplayerPoint[2] != 2 && SaveplayerPoint[2] != 3 && SaveplayerPoint[2] != 4 && SaveplayerPoint[2] != 5 && SaveplayerPoint[2] != 8 && SaveplayerPoint[2] != 9)
                {
                    if (VersionDef.InternationalLanguageSystem)
                    {
                        SituationMsg_Label2.text = Font_Control.Instance.m_dicMsgStr[2008061];
                    }
                    else
                    {
                        SituationMsg_Label2.text = "閒家補牌為「6、7」點";
                    }
                }
            }
            SituationMsg_Object[1].SetActive(true);
        }
    }

    void FinallVoid()
    {

        MainGame_Control.LastWin = SaveFinallData[0].m_oBetAward.m_enumAward;
        MainGame_Control.SaveBankerPair = SaveFinallData[0].m_oBetAward.m_bBankerOnePair;
        MainGame_Control.SavePlayerPair = SaveFinallData[0].m_oBetAward.m_bPlayerOnePair;
        GameSound.PlayerPoint = SaveFinallData[0].m_byPlayerPoint;
        GameSound.BankerPoint = SaveFinallData[0].m_byBankerPoint;
        if (SaveFinallData[0].m_oBetAward.m_enumAward == ENUM_BACCARAT_AWARD.WinBanker)
        {
            GameSound.ResultNumber = 0;
            FourCardHistory_Control.SaveFcCardPoint = SaveFinallData[0].m_byBankerPoint;
            MainGame_Control.WinArea[0] = 1;
        }
        else if (SaveFinallData[0].m_oBetAward.m_enumAward == ENUM_BACCARAT_AWARD.WinPlayer)
        {
            FourCardHistory_Control.SaveFcCardPoint = SaveFinallData[0].m_byPlayerPoint;
            MainGame_Control.WinArea[1] = 1;
            GameSound.ResultNumber = 1;
        }
        else if (SaveFinallData[0].m_oBetAward.m_enumAward == ENUM_BACCARAT_AWARD.WinDraw)
        {
            FourCardHistory_Control.SaveFcCardPoint = SaveFinallData[0].m_byBankerPoint;
            MainGame_Control.WinArea[0] = 1;
            MainGame_Control.WinArea[1] = 1;
            MainGame_Control.WinArea[2] = 1;
            GameSound.ResultNumber = 2;
        }

        if (SaveFinallData[0].m_oBetAward.m_bBankerOnePair)
        {
            MainGame_Control.WinArea[3] = 1;
        }

        if (SaveFinallData[0].m_oBetAward.m_bPlayerOnePair)
        {
            MainGame_Control.WinArea[4] = 1;
        }

        EndWindow_Control.TotalWinMoney = SaveFinallData[0].m_ui64Score;

        MainGame_Control.GetLastWinDataBool = true;
        MainGame_Control.GetNormalWinDataBool = true;
        CAllBetAward Data1 = new CAllBetAward();
        Data1.m_oNormalAward.m_enumAward = MainGame_Control.SaveWin;
        Data1.m_oNormalAward.m_bBankerOnePair = SaveFinallData[0].m_oBetAward.m_bBankerOnePair;
        Data1.m_oNormalAward.m_bPlayerOnePair = SaveFinallData[0].m_oBetAward.m_bPlayerOnePair;
        Data1.m_oNormalAward.m_byBankerPoint = TotalBankerPoint;
        Data1.m_oNormalAward.m_byPlayerPoint = TotalPlayerPoint;
        Data1.m_oLastAward.m_enumAward = SaveFinallData[0].m_oBetAward.m_enumAward;
        Data1.m_oLastAward.m_bBankerOnePair = SaveFinallData[0].m_oBetAward.m_bBankerOnePair;
        Data1.m_oLastAward.m_bPlayerOnePair = SaveFinallData[0].m_oBetAward.m_bPlayerOnePair;
        Data1.m_oLastAward.m_byBankerPoint = SaveFinallData[0].m_byBankerPoint;
        Data1.m_oLastAward.m_byPlayerPoint = SaveFinallData[0].m_byPlayerPoint;
        History_Control.HistoryData.Add(History_Control.HistoryNumber, Data1);
        History_Control.HistoryNumber++;
        if (Data1.m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinBanker)
        {
            History_Control.NormalTotalPoint = Data1.m_oNormalAward.m_byBankerPoint;
        }
        else if (Data1.m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinPlayer)
        {
            History_Control.NormalTotalPoint = Data1.m_oNormalAward.m_byPlayerPoint;
        }
        else if (Data1.m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinDraw)
        {
            History_Control.NormalTotalPoint = Data1.m_oNormalAward.m_byBankerPoint;
        }
        Debug.Log("储存資料: " + Data1.m_oNormalAward.m_enumAward + " //對子: " + Data1.m_oNormalAward.m_bBankerOnePair + "//" + Data1.m_oNormalAward.m_bPlayerOnePair + " //點數: " + Data1.m_oNormalAward.m_byBankerPoint + "//" + Data1.m_oNormalAward.m_byPlayerPoint);
        Money_Control.SaveMyMoney = SaveFinallData[0].m_ui64GameMoney;
        Debug.Log("收到最後結果  莊閒和結果: " + SaveFinallData[0].m_oBetAward.m_enumAward + " //莊對: " + SaveFinallData[0].m_oBetAward.m_bBankerOnePair + " //閒對: " + SaveFinallData[0].m_oBetAward.m_bPlayerOnePair + " //結算金額: " + Money_Control.SaveMyMoney + " //莊點數: " + SaveFinallData[0].m_byBankerPoint + " //閒點數: " + SaveFinallData[0].m_byPlayerPoint);
        Debug.Log("總贏金: " + EndWindow_Control.TotalWinMoney);
    }
}
