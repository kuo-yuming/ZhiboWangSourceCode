using UnityEngine;
using System.Collections;
using GameEnum;
using GameCore;

public class GameSound : MonoBehaviour {
    public AudioSource[] PlayerSound = new AudioSource[10];
    public AudioSource[] BankerSound = new AudioSource[10];
    public AudioSource[] MianGameSound = new AudioSource[11];
    public AudioSource[] ResultSound = new AudioSource[3];
    public AudioSource[] BackgroundSound = new AudioSource[2];

    public static bool BetStart_Bool = false;
    public static bool BetStop_Bool = false;
    public static bool BidStart_Bool = false;
    public static bool RoundResult_Bool = false;
    public static bool CashBack_Bool = false;

    public static bool DrawCard_Bool = false;
    public static bool OpenCard_Bool = false;
    public static bool OpenEndCard_Bool = false;
    public static bool CardMove_Bool = false;
    public static bool StartBid_Bool = false;
    public static bool TenSence_Bool = false;
    public static bool TapStart_Bool = false;

    //  public static bool 

    public static byte PlayerPoint = 0;
    public static byte BankerPoint = 0;
    public static byte ResultNumber = 0;

    bool PlayerPoint_Bool = true;
    bool BankerPoint_Bool = true;
    bool Testbool = false;
    bool Background_Bool = false;
    float StartDelayTime = 0.0f;
    float DelayTime = 0.0f;
    // Use this for initialization
    void Start()
    {
        BetStart_Bool = false;
        BetStop_Bool = false;
        BidStart_Bool = false;
        RoundResult_Bool = false;
        CashBack_Bool = false;
        DrawCard_Bool = false;
        OpenCard_Bool = false;
        OpenEndCard_Bool = false;
        CardMove_Bool = false;
        StartBid_Bool = false;
        TenSence_Bool = false;
        TapStart_Bool = false;
        StartDelayTime = 0.0f;
        DelayTime = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {

        if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.WaitStop)
        {
            PlayerPoint_Bool = true;
            BankerPoint_Bool = true;
            RoundResult_Bool = false;
            Testbool = false;
        }

        if (DelayTime < 1.5f)
        {
            DelayTime += Time.deltaTime;
        }
        else
        {
            //背景
            if (MainGame_Control.StopModeState != ENUM_STOPMODE_STATE.FourCardEnd && MainGame_Control.StopModeState != ENUM_STOPMODE_STATE.FourCardMoneyShow && MainGame_Control.StopModeState != ENUM_STOPMODE_STATE.FourCardShow && MainGame_Control.StopModeState != ENUM_STOPMODE_STATE.WaitFourCardTime && !Background_Bool)
            {
                BackgroundSound[0].Play();
                BackgroundSound[1].Stop();
                Background_Bool = true;

            }
            else if ((MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.FourCardEnd || MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.FourCardMoneyShow || MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.FourCardShow || MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.WaitFourCardTime) && Background_Bool)
            {
                BackgroundSound[1].Play();
                BackgroundSound[0].Stop();
                Background_Bool = false;

            }
        }

        //押注開始
        if (BetStart_Bool)
        {
            MianGameSound[0].Play();
            BetStart_Bool = false;
        }

        //押注停止
        if (BetStop_Bool)
        {
            MianGameSound[1].Play();
            BetStop_Bool = false;
        }

        //競標開始
        if (BidStart_Bool)
        {
            MianGameSound[2].Play();
            BidStart_Bool = false;
        }

        //結果發表
        if (RoundResult_Bool)
        {
            if (PlayerPoint_Bool && BankerPoint_Bool)
            {
                PlayerSound[PlayerPoint].Play();
                PlayerPoint_Bool = false;
            }

            if (!PlayerPoint_Bool && BankerPoint_Bool && !PlayerSound[PlayerPoint].isPlaying)
            {
                if (!Testbool)
                {
                    BankerSound[BankerPoint].Play();
                    Testbool = true;
                }
                else if (Testbool && !BankerSound[BankerPoint].isPlaying)
                {
                    ResultSound[ResultNumber].Play();
                    BankerPoint_Bool = false;
                    RoundResult_Bool = false;
                }
            }
        }

        //回收金幣
        if (CashBack_Bool)
        {
            MianGameSound[3].Play();
            CashBack_Bool = false;
        }

        //出牌
        if (DrawCard_Bool)
        {
            MianGameSound[4].Play();
            DrawCard_Bool = false;
        }

        //翻牌
        if (OpenCard_Bool)
        {
            MianGameSound[5].Play();
            OpenCard_Bool = false;
        }

        //翻牌完成
        if (OpenEndCard_Bool)
        {
            MianGameSound[6].Play();
            OpenEndCard_Bool = false;
        }

        //牌移動
        if (CardMove_Bool)
        {
            MianGameSound[7].Play();
            CardMove_Bool = false;
        }

        //競標開始
        if (StartBid_Bool)
        {
            MianGameSound[8].Play();
            StartBid_Bool = false;
        }

        //倒數10秒
        if (StartDelayTime < 1)
        {
            StartDelayTime += Time.deltaTime;
        }
        else
        {
            if (TenSence_Bool)
            {
                if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.WaitStop && !MianGameSound[9].isPlaying)
                {
                    MianGameSound[9].Play();
                }
                else if (MainGame_Control.StopModeState != ENUM_STOPMODE_STATE.WaitStop)
                {
                    TenSence_Bool = false;
                }
            }

            if (MainGame_Control.StopModeState != ENUM_STOPMODE_STATE.WaitStop || !TenSence_Bool)
            {
                MianGameSound[9].Stop();
            }
        }

        //競標開始鎚子敲擊
        if (TapStart_Bool)
        {
            MianGameSound[10].Play();
            TapStart_Bool = false;
        }
    }

    public void Result_Void()
    {
        RoundResult_Bool = true;
    }

    public void EndOpenCard_Void()
    {
        OpenEndCard_Bool = true;
    }
}
