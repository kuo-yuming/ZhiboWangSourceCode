using UnityEngine;
using System.Collections;
using GameCore.Manager.Baccarat;
using GameCore;

public class AutoMode_Control : MonoBehaviour {
    public static bool AutoModeOpen = false;
    public static byte AutoModeStaut = 0;
    public UISprite AutoModeButtonSpr;

    public UISprite[] AutoButtonSprite = new UISprite[2];
    public UILabel ButtonLabel;
    //AUTO加和減按鈕
    public BoxCollider[] AutoP_M_ButtonBox;
    public UIButton[] AutoP_M_ButtonBut;

    public UILabel AutoModeNumberLab;
    public static short AutoModeNumber = 0;

    public static bool StartAutoBetBool = false;
    public static bool AutoClearBetBool = false;
    public static uint[] SaveMoney = new uint[5];
    public TweenPosition AutoModeTewwnPosition;
    uint TotalBetMoney = 0;
	// Use this for initialization
    void Start()
    {
        AutoModeStaut = 0;
        StartAutoBetBool = false;
        AutoClearBetBool = false;
        for (int i = 0; i < 5; i++)
        {
            SaveMoney[i] = 0;
        }
        AutoModeNumber = 0;
        TotalBetMoney = 0;
        AutoModeOpen = false;
        ButtonLabel.enabled = false;
        AutoButtonSprite[0].depth = 2;
        AutoButtonSprite[1].depth = 2;
    }
	
	// Update is called once per frame
	void Update () {
        AutoButtonVoid();

        if (!AutoModeOpen)
        {
            AutoButtonSprite[0].depth = 2;
            AutoButtonSprite[1].depth = 2;
        }

        if (AutoModeOpen && AutoModeStaut == 0)
        {
            AutoModeTewwnPosition.PlayForward();
        }
        else if (!AutoModeOpen && AutoModeStaut == 1)
        {
            AutoModeTewwnPosition.PlayReverse();
        }

        if (StartAutoBetBool)
        {
            AutoBetVoid();
            StartAutoBetBool = false;
        }

        if (AutoClearBetBool)
        {
            for (int i = 0; i < 5; i++)
            {
                SaveMoney[i] = 0;
            }
            AutoClearBetBool = false;
        }

        if (AutoModeNumber != 1000)
        {
            AutoModeNumberLab.text = AutoModeNumber.ToString();
            ButtonLabel.text = AutoModeNumber.ToString();
        }
        else
        {
            if (VersionDef.InternationalLanguageSystem)
            {
                AutoModeNumberLab.text = Font_Control.Instance.m_dicMsgStr[2008090];
                ButtonLabel.text = Font_Control.Instance.m_dicMsgStr[2008090];
            }
            else
            {
                AutoModeNumberLab.text = "無限";
                ButtonLabel.text = "無限";
            }
                
        }
	}

    void AutoButtonVoid()
    {
        if (AutoModeOpen)
        {
            AutoModeButtonSpr.spriteName = "btn_bglautomatic";
            ButtonLabel.enabled = false;
            AutoP_M_ButtonBox[0].enabled = true;
            AutoP_M_ButtonBox[1].enabled = true;
            AutoP_M_ButtonBut[0].enabled = true;
            AutoP_M_ButtonBut[1].enabled = true;
        }
        else
        {
            if (AutoModeNumber != 0)
            {
                AutoModeButtonSpr.spriteName = "btn_bglautomatic_3";
                ButtonLabel.enabled = true;
            }
            else
            {
                AutoModeButtonSpr.spriteName = "btn_bglautomatic";
                ButtonLabel.enabled = false;
            }
            AutoP_M_ButtonBox[0].enabled = false;
            AutoP_M_ButtonBox[1].enabled = false;
            AutoP_M_ButtonBut[0].enabled = false;
            AutoP_M_ButtonBut[1].enabled = false;
        }
    }

    void AutoBetVoid()
    {
        TotalBetMoney = SaveMoney[0] + SaveMoney[1] + SaveMoney[2] + SaveMoney[3] + SaveMoney[4];

        if ((Money_Control.MyMoney - (ulong)TotalBetMoney) >= 0)
        {
            for (int i = 0; i < 5; i++)
            {
                if (SaveMoney[i] > 0)
                {
                    CPACK_Baccarat_ReqBet Data = new CPACK_Baccarat_ReqBet();
                    if (i == 0)
                    {
                        Data.m_byAreaID = (byte)ENUM_BACCARAT_AWARD_AREA.Banker;
                    }
                    else if (i == 1)
                    {
                        Data.m_byAreaID = (byte)ENUM_BACCARAT_AWARD_AREA.Player;
                    }
                    else if (i == 2)
                    {
                        Data.m_byAreaID = (byte)ENUM_BACCARAT_AWARD_AREA.Draw;
                    }
                    else if (i == 3)
                    {
                        Data.m_byAreaID = (byte)ENUM_BACCARAT_AWARD_AREA.BankerPair;
                    }
                    else if (i == 4)
                    {
                        Data.m_byAreaID = (byte)ENUM_BACCARAT_AWARD_AREA.PlayerPair;
                    }

                    Data.m_iAddBet = (int)SaveMoney[i];
                    GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Baccarat, (uint)ENUM_BACCARAT_PACKID_GC.C2G_Game_ReqBet, GameConnet.m_oGameClient.DoSerialize<CPACK_Baccarat_ReqBet>(Data));
                    Debug.Log("押注成功: " + " 押注區域: " + Data.m_byAreaID + " 押注金額: " + Data.m_iAddBet);
                }
            }
            if (AutoModeNumber != 1000 && AutoModeNumber != 0)
            {
                AutoModeNumber--;
            }
        }
        else
        {
            Message_Control.OpenMessage = true;
            Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
            Message_Control.MessageStatus = Message_Control.MessageStatu.NoBetMoney;
            for (int i = 0; i < 5; i++)
            {
                SaveMoney[i] = BetTable_Control.MyBetMoneySeat[i];
            }
            AutoClearBetBool = true;
        }
    }

    public void AutoStautChangeVoid()
    {
        if (AutoModeOpen)
        {
            AutoModeStaut = 1;
            AutoButtonSprite[0].depth = 4;
            AutoButtonSprite[1].depth = 4;
        }
        else if (!AutoModeOpen)
        {
            AutoModeStaut = 0;
        }
        MainGame_Control.AutoAndInfoClickBool = false;
    }
}
