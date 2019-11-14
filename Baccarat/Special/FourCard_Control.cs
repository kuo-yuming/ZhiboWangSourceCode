using UnityEngine;
using System.Collections;
using GameCore.Manager.Baccarat;
using GameEnum;

public class FourCard_Control : MonoBehaviour
{
    public static ulong FCJPMoney = 0;
    public UILabel FCJPMoney_Label;
    public static byte FourCard = 0;
    public static bool FourCardBet = false;
    public UILabel[] BetMoneyLabel = new UILabel[4];
    public static ulong[] BetMoney = new ulong[2];
    public static ulong[] AllFCBetMoney = new ulong[2];
    public ulong[] FCMoneyUpData = new ulong[2];
    public static bool FourCardBetShow = false;
    public GameObject FourCardObject;
    public static bool FourCardObjectOpen = false;
    public static bool FourCardStartShowBool = false;
    public UISprite FCStrateCardMove_Sprite;
    public TweenPosition FCStart_TweenPosition;
    public TweenScale FCStart_TweenScale;
    public TweenColor[] FCStart_TweenColor;

    public static bool FCBetOK_Bool = false;
    public UISprite FCCardOpen1_Sprite;
    public UISprite FCCardOpen2_Sprite;
    public UISprite FCCardOpenBlack_Sprite;
    public UISprite FCMoveCard_Sprite;
    public TweenPosition FCFirstMoveCard1;
    public TweenPosition FCFirstMoveCard2;
    public TweenPosition FCCardOpenBlack_Position;
    public TweenPosition FCCardOpenBlack_Position2;
    public TweenPosition FCMoveCard_Position;
    public TweenScale FCMoveCard_Scale;

    public UILabel FeeLabel;
    public UILabel FeeLabel2;
    public static byte FeeMoney = 0;
    private Vector3 MoveFC_Banker = new Vector3(190, 200, 0);
    private Vector3 MoveFC_Player = new Vector3(-190, 200, 0);
    private Vector3 MoveFC_Draw = new Vector3(0, 35, 0);

    public GameObject EndWindow;
    public UILabel[] EndWindow_Label;
    public UISprite RedBackground_Sprite;
    //////////////////金錢生成
    public GameObject[] FCCoinGameobject;
    public UITable[] FCBetTable;

    //Lock
    private Object FCBankerLock = new Object();
    private Object FCPlayerLock = new Object();

    //硬幣數量計算
    private byte[] FCTotal_Gold1M = new byte[2];
    private byte[] FCTotal_Gold100k = new byte[2];
    private byte[] FCTotal_Gold10k = new byte[2];
    private byte[] FCTotal_Gold5k = new byte[2];
    private byte[] FCTotal_Gold1k = new byte[2];
    private byte[] FCTotal_Gold500 = new byte[2];
    private byte[] FCTotal_Gold100 = new byte[2];
    private byte[] FCTable_Total_Gold = new byte[2];

    float Timer = 0;
    float Timer2 = 0;
    float Timer3 = 0;
    float Timer4 = 0;
    bool CardMoveTimeBool = false;
    bool WaitCashTime = false;
    bool FCOpenCardBool = false;
    bool FCMoveCardBool = false;

    bool OpenSound = false;
    bool MoveSound = false;

    public TweenPosition[] EndWnodwShow = new TweenPosition[2];
    // Use this for initialization
    void Start()
    {
        FourCardObjectOpen = false;
        FourCard = 0;
        FourCardBet = false;
        FCMoneyDataInit();
        CardMoveTimeBool = false;
        FourCardBetShow = false;
        FourCardObject.SetActive(false);
        FCStrateCardMove_Sprite.enabled = false;
        FCMoveCard_Sprite.enabled = false;
        RedBackground_Sprite.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        BetMoneyLabel[0].text = BetMoney[0].ToString();
        BetMoneyLabel[1].text = BetMoney[1].ToString();
        BetMoneyLabel[2].text = AllFCBetMoney[0].ToString();
        BetMoneyLabel[3].text = AllFCBetMoney[1].ToString();

        FeeLabel.text = FeeMoney.ToString() + "%手續費";
        FeeLabel2.text = (1 - ((float)FeeMoney * 0.01)).ToString();

        FCBankerCoinGenerateVoid();
        FCPlayerCoinGenerateVoid();


        FCJPMoney_Label.text = FCJPMoney.ToString();

        if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.WaitStop)
        {
            FourCardHistory_Control.NowSizeFloat = 1;
            RedBackground_Sprite.enabled = false;
            EndWnodwShow[0].ResetToBeginning();
            EndWnodwShow[1].ResetToBeginning();
            FCMoneyDataInit();
        }

        if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.ShuffleTimeShow)
        {
            RedBackground_Sprite.enabled = false;
        }

        if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.MoneyShow || MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.WaitStop)
        {
            FourCardObject.SetActive(false);
            FourCardBet = false;
        }

        if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.EndShow)
        {
            RedBackground_Sprite.enabled = false;
            FCMoveCard_Sprite.enabled = false;
        }

        if (FourCardObjectOpen)
        {
            FourCardObject.SetActive(true);
            Transform[] Objs = FCBetTable[0].GetComponentsInChildren<Transform>();
            int Len = Objs.Length;
            for (int i = 0; i < Len; i++)
            {
                if (Objs[i].name == "FCBankerCoin")
                {
                    Destroy(Objs[i].gameObject);
                }
            }
            Transform[] Objs2 = FCBetTable[1].GetComponentsInChildren<Transform>();
            int Len2 = Objs2.Length;
            for (int i = 0; i < Len2; i++)
            {
                if (Objs2[i].name == "FCPlayerCoin")
                {
                    Destroy(Objs2[i].gameObject);
                }
            }
            BetMoney[0] = 0;
            BetMoney[1] = 0;
            AllFCBetMoney[0] = 0;
            AllFCBetMoney[1] = 0;
            FourCardObjectOpen = false;
        }

        if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.WaitFourCardTime || MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.FourCardShow)
        {
            if (FourCardStartShowBool)
            {
                FCStart_TweenPosition.ResetToBeginning();
                FCStart_TweenScale.ResetToBeginning();
                FCStrateCardMove_Sprite.enabled = true;
                FCStart_TweenPosition.PlayForward();
                FCStart_TweenScale.PlayForward();
                GameSound.StartBid_Bool = true;
                for (int i = 0; i < 9; i++)
                {
                    FCStart_TweenColor[i].ResetToBeginning();
                    FCStart_TweenColor[i].PlayForward();
                }
                FourCardStartShowBool = false;
            }
        }

        if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.FourCardShow)
        {
            if (!FourCardBetShow)
            {
                FourCardBetTimeVoid();
            }
        }
        else if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.FourCardEnd)
        {
            FourCardEndShowVoid();
        }
    }

    void FourCardBetTimeVoid()
    {
        if (FourCard_Control.FourCard < 10)
        {
            FCCardOpen2_Sprite.spriteName = "0" + FourCard_Control.FourCard;
            FCMoveCard_Sprite.spriteName = "0" + FourCard_Control.FourCard;
        }
        else
        {
            FCCardOpen2_Sprite.spriteName = FourCard_Control.FourCard.ToString();
            FCMoveCard_Sprite.spriteName = FourCard_Control.FourCard.ToString();
        }

        ////////開牌表演一
        if (Timer2 < 0.5f && !CardMoveTimeBool && !WaitCashTime && !FCOpenCardBool)
        {
            Timer2 += Time.deltaTime;
        }
        else if (!CardMoveTimeBool && !WaitCashTime && !FCOpenCardBool)
        {
            if (!OpenSound)
            {
                GameSound.OpenCard_Bool = true;
                OpenSound = true;
            }
            FourCardHistory_Control.FCLeftOverCardPoint--;
            FCFirstMoveCard2.ResetToBeginning();
            FCCardOpenBlack_Position.ResetToBeginning();
            FCCardOpenBlack_Position2.ResetToBeginning();
            FCMoveCard_Position.ResetToBeginning();
            FCMoveCard_Scale.ResetToBeginning();
            FCFirstMoveCard1.PlayForward();
            FCFirstMoveCard2.PlayForward();
            FCCardOpenBlack_Position.PlayForward();
            FCCardOpenBlack_Position2.PlayForward();
            FCCardOpen2_Sprite.enabled = true;
            FCCardOpenBlack_Sprite.enabled = true;
            FCOpenCardBool = true;
            Timer2 = 0;
        }

        //////////開牌表演二
        if (Timer3 < 2 && CardMoveTimeBool && !WaitCashTime && !FCMoveCardBool)
        {
            Timer3 += Time.deltaTime;

            if (AllFCBetMoney[0] > AllFCBetMoney[1])
            {
                FCMoveCard_Position.to = MoveFC_Banker;
            }
            else if (AllFCBetMoney[0] < AllFCBetMoney[1])
            {
                FCMoveCard_Position.to = MoveFC_Player;
            }
            else if (AllFCBetMoney[0] == AllFCBetMoney[1])
            {
                FCMoveCard_Position.to = MoveFC_Draw;
            }
            if (Timer3 > 1.7f)
            {
                if (!MoveSound)
                {
                    GameSound.CardMove_Bool = true;
                    MoveSound = true;
                }
            }
        }
        else if (CardMoveTimeBool && !WaitCashTime && !FCMoveCardBool)
        {
            FCMoveCard_Position.PlayForward();
            FCMoveCard_Scale.PlayForward();
            FCMoveCardBool = true;
            Timer3 = 0;
        }

        if (Timer4 < 5 && WaitCashTime)
        {
            FCBetOK_Bool = false;
            Timer4 += Time.deltaTime;
            if (Timer4 > 1)
            {
                EndWindow.SetActive(true);
                EndWnodwShow[0].PlayForward();
                EndWnodwShow[1].PlayForward();

                EndWindow_Label[0].text = GameSound.BankerPoint.ToString();
                EndWindow_Label[1].text = GameSound.PlayerPoint.ToString();

                if (MainGame_Control.LastWin == ENUM_BACCARAT_AWARD.WinBanker &&  MainGame_Control.WinArea[2] != 1)
                {

                    EndWindow_Label[2].text = "莊贏";
                }
                else if (MainGame_Control.LastWin == ENUM_BACCARAT_AWARD.WinPlayer && MainGame_Control.WinArea[2] != 1)
                {

                    EndWindow_Label[2].text = "閒贏";
                }
                else if (MainGame_Control.LastWin == ENUM_BACCARAT_AWARD.WinDraw)
                {

                    EndWindow_Label[2].text = "平和";
                }
            }
        }
        else if (WaitCashTime)
        {
            Timer4 = 0;
            MainGame_Control.StopModeState = ENUM_STOPMODE_STATE.FourCardMoneyShow;
            EndWindow.SetActive(false);
        }

    }

    void FourCardEndShowVoid()
    {
        if (Timer < 3)
        {
            Timer += Time.deltaTime;
            if (Timer > 1)
            {
                FourCardObject.SetActive(false);
            }
        }
        else
        {
            Timer = 0;
            MainGame_Control.StopModeState = ENUM_STOPMODE_STATE.MoneyShow;
        }
    }

    void FCMoneyDataInit()
    {
       
        for (int i = 0; i < 2; i++)
        {
            BetMoney[i] = 0;
            FCMoneyUpData[i] = 0;
            AllFCBetMoney[i] = 0;
            FCTotal_Gold1M[i] = 0;
            FCTotal_Gold100k[i] = 0;
            FCTotal_Gold10k[i] = 0;
            FCTotal_Gold5k[i] = 0;
            FCTotal_Gold1k[i] = 0;
            FCTotal_Gold500[i] = 0;
            FCTotal_Gold100[i] = 0;
            FCTable_Total_Gold[i] = 0;
        }
        AllFCBetMoney[0] = 0;
        AllFCBetMoney[1] = 0;
        Timer = 0;
        Timer2 = 0;
        Timer3 = 0;
        Timer4 = 0;
        CardMoveTimeBool = false;
        WaitCashTime = false;
        FCBetOK_Bool = false;
        FourCardStartShowBool = false;
        FCMoveCard_Position.ResetToBeginning();
        FCMoveCard_Scale.ResetToBeginning();
        FCFirstMoveCard1.ResetToBeginning();
        FCFirstMoveCard2.ResetToBeginning();
        FCCardOpenBlack_Position.ResetToBeginning();
        FCCardOpenBlack_Position2.ResetToBeginning();
        FCCardOpen1_Sprite.enabled = false;
        FCCardOpen2_Sprite.enabled = false;
        FCCardOpenBlack_Sprite.enabled = false;
        FCMoveCard_Sprite.enabled = false;
        FCOpenCardBool = false;
        CardMoveTimeBool = false;
        FourCardBetShow = false;
    }

    ////////////////////////////////
    void FCBankerCoinGenerateVoid()
    {
        if (AllFCBetMoney[0] != FCMoneyUpData[0])
        {
            lock (FCBankerLock)
            {
                FCMoneyUpData[0] = AllFCBetMoney[0];
                Transform[] Objs = FCBetTable[0].GetComponentsInChildren<Transform>();
                int Len = Objs.Length;
                for (int i = 0; i < Len; i++)
                {
                    if (Objs[i].name == "FCBankerCoin")
                    {
                        Destroy(Objs[i].gameObject);
                    }
                }
                FCBankerCoinVoid();
            }
        }
    }
    void FCPlayerCoinGenerateVoid()
    {
        if (AllFCBetMoney[1] != FCMoneyUpData[1])
        {
            lock (FCPlayerLock)
            {
                FCMoneyUpData[1] = AllFCBetMoney[1];
                Transform[] Objs = FCBetTable[1].GetComponentsInChildren<Transform>();
                int Len = Objs.Length;
                for (int i = 0; i < Len; i++)
                {
                    if (Objs[i].name == "FCPlayerCoin")
                    {
                        Destroy(Objs[i].gameObject);
                    }
                }
                FCPlayerCoinVoid();
            }
        }
    }

    void FCBankerCoinVoid()
    {
        FCTotal_Gold1M[0] = (byte)((FCMoneyUpData[0] / 1000000) % 10);
        FCTotal_Gold100k[0] = (byte)((FCMoneyUpData[0] / 100000) % 10);
        FCTotal_Gold10k[0] = (byte)((FCMoneyUpData[0] / 10000) % 10);
        FCTotal_Gold5k[0] = (byte)((FCMoneyUpData[0] / 5000) % 2);
        FCTotal_Gold1k[0] = (byte)((FCMoneyUpData[0] / 1000) % 5);
        FCTotal_Gold500[0] = (byte)((FCMoneyUpData[0] / 500) % 2);
        FCTotal_Gold100[0] = (byte)((FCMoneyUpData[0] / 100) % 5);

        FCTable_Total_Gold[0] = (byte)(FCTotal_Gold1M[0] + FCTotal_Gold100k[0] + FCTotal_Gold10k[0] + FCTotal_Gold5k[0] + FCTotal_Gold1k[0] + FCTotal_Gold500[0] + FCTotal_Gold100[0]);

        for (byte i = 0; i < FCTable_Total_Gold[0]; i++)
        {
            byte XPosition = (byte)((i / 10) % 4);
            byte YPosition = (byte)(i % 10);
            byte YAddPosition = (byte)(i / 40);
            if (i < FCTotal_Gold1M[0])
            {
                GameObject Data = Instantiate(FCCoinGameobject[6]);
                Data.transform.parent = FCBetTable[0].transform;
                Data.name = "FCBankerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 82, (YPosition * 4) + (YAddPosition * -73), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - FCTotal_Gold1M[0]) < FCTotal_Gold100k[0])
            {
                GameObject Data = Instantiate(FCCoinGameobject[5]);
                Data.transform.parent = FCBetTable[0].transform;
                Data.name = "FCBankerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 82, (YPosition * 4) + (YAddPosition * -73), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (FCTotal_Gold1M[0] + FCTotal_Gold100k[0])) < FCTotal_Gold10k[0])
            {
                GameObject Data = Instantiate(FCCoinGameobject[4]);
                Data.transform.parent = FCBetTable[0].transform;
                Data.name = "FCBankerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 82, (YPosition * 4) + (YAddPosition * -73), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (FCTotal_Gold1M[0] + FCTotal_Gold100k[0] + FCTotal_Gold10k[0])) < FCTotal_Gold5k[0])
            {
                GameObject Data = Instantiate(FCCoinGameobject[3]);
                Data.transform.parent = FCBetTable[0].transform;
                Data.name = "FCBankerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 82, (YPosition * 4) + (YAddPosition * -73), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (FCTotal_Gold1M[0] + FCTotal_Gold100k[0] + FCTotal_Gold10k[0] + FCTotal_Gold5k[0])) < FCTotal_Gold1k[0])
            {
                GameObject Data = Instantiate(FCCoinGameobject[2]);
                Data.transform.parent = FCBetTable[0].transform;
                Data.name = "FCBankerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 82, (YPosition * 4) + (YAddPosition * -73), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (FCTotal_Gold1M[0] + FCTotal_Gold100k[0] + FCTotal_Gold10k[0] + FCTotal_Gold5k[0] + FCTotal_Gold1k[0])) < FCTotal_Gold500[0])
            {
                GameObject Data = Instantiate(FCCoinGameobject[1]);
                Data.transform.parent = FCBetTable[0].transform;
                Data.name = "FCBankerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 82, (YPosition * 4) + (YAddPosition * -73), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (FCTotal_Gold1M[0] + FCTotal_Gold100k[0] + FCTotal_Gold10k[0] + FCTotal_Gold5k[0] + FCTotal_Gold1k[0] + FCTotal_Gold500[0])) < FCTotal_Gold100[0])
            {
                GameObject Data = Instantiate(FCCoinGameobject[0]);
                Data.transform.parent = FCBetTable[0].transform;
                Data.name = "FCBankerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 82, (YPosition * 4) + (YAddPosition * -73), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
        }

    }
    void FCPlayerCoinVoid()
    {
        FCTotal_Gold1M[1] = (byte)((FCMoneyUpData[1] / 1000000) % 10);
        FCTotal_Gold100k[1] = (byte)((FCMoneyUpData[1] / 100000) % 10);
        FCTotal_Gold10k[1] = (byte)((FCMoneyUpData[1] / 10000) % 10);
        FCTotal_Gold5k[1] = (byte)((FCMoneyUpData[1] / 5000) % 2);
        FCTotal_Gold1k[1] = (byte)((FCMoneyUpData[1] / 1000) % 5);
        FCTotal_Gold500[1] = (byte)((FCMoneyUpData[1] / 500) % 2);
        FCTotal_Gold100[1] = (byte)((FCMoneyUpData[1] / 100) % 5);

        FCTable_Total_Gold[1] = (byte)(FCTotal_Gold1M[1] + FCTotal_Gold100k[1] + FCTotal_Gold10k[1] + FCTotal_Gold5k[1] + FCTotal_Gold1k[1] + FCTotal_Gold500[1] + FCTotal_Gold100[1]);

        for (byte i = 0; i < FCTable_Total_Gold[1]; i++)
        {
            byte XPosition = (byte)((i / 10) % 4);
            byte YPosition = (byte)(i % 10);
            byte YAddPosition = (byte)(i / 40);

            if (i < FCTotal_Gold1M[1])
            {
                GameObject Data = Instantiate(FCCoinGameobject[6]);
                Data.transform.parent = FCBetTable[1].transform;
                Data.name = "FCPlayerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 82, (YPosition * 4) + (YAddPosition * -73), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - FCTotal_Gold1M[1]) < FCTotal_Gold100k[1])
            {
                GameObject Data = Instantiate(FCCoinGameobject[5]);
                Data.transform.parent = FCBetTable[1].transform;
                Data.name = "FCPlayerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 82, (YPosition * 4) + (YAddPosition * -73), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (FCTotal_Gold1M[1] + FCTotal_Gold100k[1])) < FCTotal_Gold10k[1])
            {
                GameObject Data = Instantiate(FCCoinGameobject[4]);
                Data.transform.parent = FCBetTable[1].transform;
                Data.name = "FCPlayerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 82, (YPosition * 4) + (YAddPosition * -73), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (FCTotal_Gold1M[1] + FCTotal_Gold100k[1] + FCTotal_Gold10k[1])) < FCTotal_Gold5k[1])
            {
                GameObject Data = Instantiate(FCCoinGameobject[3]);
                Data.transform.parent = FCBetTable[1].transform;
                Data.name = "FCPlayerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 82, (YPosition * 4) + (YAddPosition * -73), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (FCTotal_Gold1M[1] + FCTotal_Gold100k[1] + FCTotal_Gold10k[1] + FCTotal_Gold5k[1])) < FCTotal_Gold1k[1])
            {
                GameObject Data = Instantiate(FCCoinGameobject[2]);
                Data.transform.parent = FCBetTable[1].transform;
                Data.name = "FCPlayerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 82, (YPosition * 4) + (YAddPosition * -73), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (FCTotal_Gold1M[1] + FCTotal_Gold100k[1] + FCTotal_Gold10k[1] + FCTotal_Gold5k[1] + FCTotal_Gold1k[1])) < FCTotal_Gold500[1])
            {
                GameObject Data = Instantiate(FCCoinGameobject[1]);
                Data.transform.parent = FCBetTable[1].transform;
                Data.name = "FCPlayerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 82, (YPosition * 4) + (YAddPosition * -73), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (FCTotal_Gold1M[1] + FCTotal_Gold100k[1] + FCTotal_Gold10k[1] + FCTotal_Gold5k[1] + FCTotal_Gold1k[1] + FCTotal_Gold500[1])) < FCTotal_Gold100[1])
            {
                GameObject Data = Instantiate(FCCoinGameobject[0]);
                Data.transform.parent = FCBetTable[1].transform;
                Data.name = "FCPlayerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 82, (YPosition * 4) + (YAddPosition * -73), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
        }

    }

    public void BetOkVoid()
    {
        FCStrateCardMove_Sprite.enabled = false;
        FCCardOpen1_Sprite.enabled = true;
        FCFirstMoveCard1.ResetToBeginning();
        FCBetOK_Bool = true;
    }

    public void OpenEndVoid()
    {
        RedBackground_Sprite.enabled = true;
        FCFirstMoveCard1.ResetToBeginning();
        FCFirstMoveCard2.ResetToBeginning();
        FCCardOpenBlack_Position.ResetToBeginning();
        FCCardOpenBlack_Position2.ResetToBeginning();
        FCCardOpen1_Sprite.enabled = false;
        FCCardOpen2_Sprite.enabled = false;
        FCCardOpenBlack_Sprite.enabled = false;
        FCMoveCard_Sprite.enabled = true;
        FCOpenCardBool = false;
        CardMoveTimeBool = true;
        OpenSound = false;
    }


    public void MoveEndVoid()
    {
        WaitCashTime = true;
        FCMoveCardBool = false;
        MoveSound = false;
    }
}
