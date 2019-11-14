using UnityEngine;
using System.Collections;

public class IceAgeButtonControl : MonoBehaviour
{
    public static IceAgeButtonControl Inst;
    public ButtonStatus NowButton;
    //機台設定
    public UILabel MachineID;
    public UISprite ButtonAreakground;  //按鈕背景
    public UIButton StartButton;        //開始按鈕
    public UIButton StopButton;         //停止按鈕
    public UIButton MoneyDoubleButton;  //比倍按鈕
    public UIButton GetScoreButton;     //得分按鈕
    public UIButton LeaveDoubleUpButton;//離開比倍
    //押注
    public UISprite BetBackground;      //按鈕背景
    public UILabel BetValue;            //顯示文字
    public TweenPosition BetPosition;   //設定押注滑動區塊
    public byte NumberOfBets;           //押注數量
    public UILabel BetSetValue;         //設定區塊文字
    public UIButton BetSetUp;           //設定區塊押注提升
    public UISprite BetUpSprite;        //設定區塊押注提升(背景)
    public UIButton BetSetDown;         //設定區塊押注降低
    public UISprite BetDownSprite;      //設定區塊押注降低(背景)
    public UIButton BetSetMax;          //設定區塊押注滿
    //自動
    public UISprite AutoBackground;     //按鈕背景
    public UILabel AutoValueLabel;      //顯示文字(字版)
    public TweenPosition AutoPosition;  //設定自動滑動區塊
    public ushort AutoTimes;            //自動次數
    public static bool AutoSeting = false;  //設定次數中
    private bool AutoUp;                //設定次數Up
    private bool AutoDown;              //設定次數Down
    public UIButton AutoUpButton;       //設定次數Up 按鈕
    public UISprite AutoUpSprite;       //設定次數Up 背景
    public UIButton AutoDownButton;     //設定次數Down 按鈕
    public UISprite AutoDownSprite;     //設定次數Down 背景
    public UIButton AutoInfinityButton; //設定次數無限 按鈕
    private float NowSpeed = 0.0f;      //設定次數  NowSpeed < Speed => AutoTimes++
    private float Speed = 10.0f;        //設定次數  NowSpeed > Speed => AutoTimes++
    private float MaxSpeed = 30.0f;     //設定次數  NowSpeed > MaxSpeed => AutoTimes += Speed
    public UILabel AutoSetValueLabel;   //設定區塊顯示文字(字版)
    //資訊
    public TweenPosition InfoPosition;
    public GameObject m_InfoObj;
    public GameObject m_MyAwardObj;
    public GameObject m_AllAwardObj;
    //背包
    public TweenPosition PackPosition;

    public enum ButtonStatus
    {
        Idle = 0,
        Bet = 11,
        Auto = 21,
        Pack = 31,
        Info = 41,
    }

    void Awake()
    {
        Inst = this;
    }

    // Use this for initialization
    void Start()
    {
        //初始按鈕狀態
        NowButton = ButtonStatus.Idle;
        StartButton.gameObject.SetActive(true);
        StartButton.isEnabled = true;
        MoneyDoubleButton.gameObject.SetActive(true);
        MoneyDoubleButton.isEnabled = false;
        StopButton.gameObject.SetActive(false);
        GetScoreButton.gameObject.SetActive(false);
        LeaveDoubleUpButton.gameObject.SetActive(false);
        //初始押金顯示
        BetBackground.spriteName = "btn_moneyback_2";
        BetValue.depth = BetBackground.depth + 1;
        NumberOfBets = 1;
        BetValue.text = (NumberOfBets * GameConnet.m_PMachineConfig.m_usBetMoney * GameConnet.m_PMachineConfig.m_byMaxLine).ToString();
        BetSetValue.text = (NumberOfBets * GameConnet.m_PMachineConfig.m_usBetMoney * GameConnet.m_PMachineConfig.m_byMaxLine).ToString();
        BetSetUp.isEnabled = false;
        BetSetDown.isEnabled = false;
        BetSetMax.isEnabled = false;
        BetUpSprite.depth = ButtonAreakground.depth - 1;
        BetDownSprite.depth = ButtonAreakground.depth - 1;
        //初始自動顯示
        AutoTimes = 0;
        AutoBackground.spriteName = "btn_automatic_1";
        AutoValueLabel.text = AutoTimes + "";
        AutoSetValueLabel.text = AutoTimes + "";
        AutoUpButton.isEnabled = false;
        AutoDownButton.isEnabled = false;
        AutoInfinityButton.isEnabled = false;
        AutoUpSprite.depth = ButtonAreakground.depth - 1;
        AutoDownSprite.depth = ButtonAreakground.depth - 1;
        //初始資訊按鈕
        InfoPosition.transform.GetChild(1).GetComponent<BoxCollider>().enabled = false;
        InfoPosition.transform.GetChild(2).GetComponent<BoxCollider>().enabled = false;
        InfoPosition.transform.GetChild(3).GetComponent<BoxCollider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IceAgeManager.EnterMachine)
        {
            SetMachineID();
            IceAgeManager.EnterMachine = false;
        }

        ShowAutoTimes();    //更新遊戲次數顯示
        if (AutoSeting)
        {
            NowSpeed += Speed * Time.deltaTime;
            if (AutoUp)
            {
                if (NowSpeed > MaxSpeed)
                    AutoTimes += (ushort)Speed;
                else if (NowSpeed > Speed)
                    AutoTimes++;
            }
            else if (AutoDown)
            {
                if (NowSpeed > MaxSpeed)
                    AutoTimes -= (ushort)Speed;
                else if (NowSpeed > Speed)
                    AutoTimes--;
            }
        }
        else
        {
            AutoUp = false;
            AutoDown = false;
            NowSpeed = 0;
        }
        //如果遊戲狀態不是Idle 關閉押注按鈕
        if (IceAgeGameMain.Inst.GameStatus != IceAgeGameMain.Game_Status.Idle)
            CloseBet();
        else //Idle狀態時 判斷是否開啟押注
            OpenBet();
    }

    public void SetMachineID()
    {
        MachineID.text = GameConnet.m_PMachineBuyInGameData.m_uiMID.ToString("000");
    }

    public void StatusChange(ButtonStatus NowBS)
    {
        switch (NowBS)
        {
            case ButtonStatus.Idle:
                if (NowButton != ButtonStatus.Idle) StatusChange(NowButton);
                NowButton = ButtonStatus.Idle;
                break;
            case ButtonStatus.Bet:
                ClickBet();
                break;
            case ButtonStatus.Auto:
                ClickAuto();
                break;
            case ButtonStatus.Pack:
                ClickPack();
                break;
            case ButtonStatus.Info:
                ClickInfo();
                break;
        }
    }

    public void SetMovingButton()
    {   //SlotMoving
        StopButton.gameObject.SetActive(true);
        StopButton.isEnabled = false;  //顯示停止 不可按
        MoneyDoubleButton.gameObject.SetActive(true);
        MoneyDoubleButton.isEnabled = false;   //顯示比倍 不可按
        //關閉投注調整與比倍 得分 離開比倍
        StartButton.gameObject.SetActive(false);
        GetScoreButton.gameObject.SetActive(false);
        LeaveDoubleUpButton.gameObject.SetActive(false);
        CloseBet();
    }

    public void SetStopButton()
    {   //SlotMoving
        if (AutoTimes == 0 && IceAgeGameMain.Inst.GameStatus != IceAgeGameMain.Game_Status.MiniGameSuccess)
            IceAgeGameMain.Inst.GameStatus = IceAgeGameMain.Game_Status.Idle;
        IceAgeGameMain.Inst.ReStartTimer = 0.0f;
        StartButton.gameObject.SetActive(true);
        StartButton.isEnabled = true;   //顯示開始 可以按        
        MoneyDoubleButton.gameObject.SetActive(true);
        MoneyDoubleButton.isEnabled = false;   //顯示比倍 不可按
        //關閉比倍 得分 離開比倍
        StopButton.gameObject.SetActive(false);
        GetScoreButton.gameObject.SetActive(false);
        LeaveDoubleUpButton.gameObject.SetActive(false);
    }

    public void SetMoneyDoubleButtonShow()
    {   //顯示比倍 得分 不可按
        MoneyDoubleButton.gameObject.SetActive(true);
        MoneyDoubleButton.isEnabled = false;
        GetScoreButton.gameObject.SetActive(true);
        GetScoreButton.isEnabled = false;
        //關閉開始 停止
        StartButton.gameObject.SetActive(false);
        StopButton.gameObject.SetActive(false);
        CloseBet();
    }

    public void SetMoneyDoubleButton()
    {   //金錢數字播放完 可以按比倍 不能按得分
        MoneyDoubleButton.gameObject.SetActive(true);
        MoneyDoubleButton.isEnabled = true;
        GetScoreButton.gameObject.SetActive(true);
        GetScoreButton.isEnabled = false;
        //關閉開始 停止
        StartButton.gameObject.SetActive(false);
        StopButton.gameObject.SetActive(false);
        CloseBet();
    }

    public void SetGetScoreButton()
    {   //動畫過數秒後 比倍 得分 都可以按
        MoneyDoubleButton.gameObject.SetActive(true);
        MoneyDoubleButton.isEnabled = true;
        GetScoreButton.gameObject.SetActive(true);
        GetScoreButton.isEnabled = true;
        //關閉開始 停止
        StartButton.gameObject.SetActive(false);
        StopButton.gameObject.SetActive(false);
        CloseBet();
    }

    public void SetDoubleUpGameButtonShow()
    {   //進入比倍時  顯示離開比倍 不可按
        LeaveDoubleUpButton.gameObject.SetActive(true);
        LeaveDoubleUpButton.isEnabled = false;
        //關閉 開始 停止 比倍 得分
        MoneyDoubleButton.gameObject.SetActive(false);
        GetScoreButton.gameObject.SetActive(false);
        StartButton.gameObject.SetActive(false);
        StopButton.gameObject.SetActive(false);
        CloseBet();
    }

    public void SetDoubleUpGameButton()
    {   //比倍開始 才可以按 離開比倍
        LeaveDoubleUpButton.gameObject.SetActive(true);
        LeaveDoubleUpButton.isEnabled = true;
        //關閉 開始 停止 比倍 得分
        MoneyDoubleButton.gameObject.SetActive(false);
        GetScoreButton.gameObject.SetActive(false);
        StartButton.gameObject.SetActive(false);
        StopButton.gameObject.SetActive(false);
        CloseBet();
    }

    public void SetFreeGameButton()
    {   //FreeGame中 按鈕 顯示開始 停止 都不可按
        StopButton.gameObject.SetActive(true);
        StopButton.isEnabled = false;
        MoneyDoubleButton.gameObject.SetActive(true);
        MoneyDoubleButton.isEnabled = false;
        //關閉比倍 離開 離開比倍 關閉投注與自動設置 投注金額顯示0
        LeaveDoubleUpButton.gameObject.SetActive(false);
        StartButton.gameObject.SetActive(false);
        GetScoreButton.gameObject.SetActive(false);
        BetValue.text = "0";
        BetSetValue.text = "0";
        CloseBet();
        AutoUpButton.isEnabled = false;
        AutoDownButton.isEnabled = false;
    }

    public void UnhideStopButton()
    {   //開放手動停止
        StopButton.gameObject.SetActive(true);
        StopButton.isEnabled = true;
    }

    void ClickBack()
    {
        if (NowButton != ButtonStatus.Idle) StatusChange(NowButton);
        NowButton = ButtonStatus.Idle;
        GameConnet.BuyOut_GameLobbySuccess = true;
    }

    void ClickBet()
    {
        if (NowButton != ButtonStatus.Bet)
        {
            //如果其他按鈕有出現 進行互斥 並更新目前狀態
            if (NowButton != ButtonStatus.Idle) StatusChange(NowButton);
            NowButton = ButtonStatus.Bet;
            BetPosition.PlayForward();  //彈出設定欄位
            //圖片變押金(大)
            BetBackground.spriteName = "btn_moneyback_1";
            BetValue.depth = BetBackground.depth - 1;
        }
        else
        {
            NowButton = ButtonStatus.Idle;
            BetPosition.PlayReverse();  //縮回設定欄位  
            BetUpSprite.depth = ButtonAreakground.depth - 1;
            BetDownSprite.depth = ButtonAreakground.depth - 1;
            BetSetUp.isEnabled = false; //關閉按鈕
            BetSetDown.isEnabled = false;
            BetSetMax.isEnabled = false;
            //圖片變押金(小)+文字
            BetBackground.spriteName = "btn_moneyback_2";
            BetValue.depth = BetBackground.depth + 1;
        }
    }

    void ClickBetSetUp()
    {
        if (NumberOfBets < GameConnet.m_PMBetMax)
        {
            NumberOfBets++;
            BetSetDown.isEnabled = true;
            if (NumberOfBets == GameConnet.m_PMBetMax) BetSetUp.isEnabled = false;
            IceAge_JP_Control.Inst.UpdateJP();  //更新JP
        }
        BetValue.text = (NumberOfBets * GameConnet.m_PMachineConfig.m_usBetMoney * GameConnet.m_PMachineConfig.m_byMaxLine).ToString();
        BetSetValue.text = BetValue.text;
    }

    void ClickBetSetDown()
    {
        if (NumberOfBets > 1)
        {
            NumberOfBets--;
            BetSetUp.isEnabled = true;
            if (NumberOfBets == 1) BetSetDown.isEnabled = false;
            IceAge_JP_Control.Inst.UpdateJP();  //更新JP
        }
        BetValue.text = (NumberOfBets * GameConnet.m_PMachineConfig.m_usBetMoney * GameConnet.m_PMachineConfig.m_byMaxLine).ToString();
        BetSetValue.text = BetValue.text;
    }

    void ClickBetSetMax()
    {
        NumberOfBets = GameConnet.m_PMBetMax;
        BetSetDown.isEnabled = false;
        if (NumberOfBets > 1)
            BetSetDown.isEnabled = true;
        BetValue.text = (NumberOfBets * GameConnet.m_PMachineConfig.m_usBetMoney * GameConnet.m_PMachineConfig.m_byMaxLine).ToString();
        BetSetValue.text = BetValue.text;
    }

    void OpenBet()
    {
        if (NowButton == ButtonStatus.Bet)  //開放投注調整 
        {
            if (NumberOfBets == GameConnet.m_PMBetMax) BetSetUp.isEnabled = false;
            else BetSetUp.isEnabled = true;
            if (NumberOfBets == 1) BetSetDown.isEnabled = false;
            else BetSetDown.isEnabled = true;
            BetSetMax.isEnabled = true;
        }
    }

    void CloseBet()
    {
        BetSetUp.isEnabled = false;
        BetSetDown.isEnabled = false;
        BetSetMax.isEnabled = false;
    }

    void ClickAuto()
    {
        if (NowButton != ButtonStatus.Auto)
        {
            //如果其他按鈕有出現 進行互斥 並更新目前狀態
            if (NowButton != ButtonStatus.Idle) StatusChange(NowButton);
            NowButton = ButtonStatus.Auto;
            //彈出設定欄位
            AutoPosition.PlayForward();
            if (IceAgeGameMain.Inst.GameStatus == IceAgeGameMain.Game_Status.FreeGame)
            {
                AutoUpButton.isEnabled = false;
                AutoDownButton.isEnabled = false;
                AutoInfinityButton.isEnabled = false;
            }
            else
            {
                AutoUpButton.isEnabled = true;
                AutoDownButton.isEnabled = true;
                AutoInfinityButton.isEnabled = true;
            }
            //圖片變自動(大)
            AutoBackground.spriteName = "btn_automatic_1";
            AutoValueLabel.enabled = false;
        }
        else
        {
            NowButton = ButtonStatus.Idle;
            //縮回設定欄位            
            AutoPosition.PlayReverse();
            AutoUpSprite.depth = ButtonAreakground.depth - 1;
            AutoDownSprite.depth = ButtonAreakground.depth - 1;
            AutoUpButton.isEnabled = false;
            AutoDownButton.isEnabled = false;
            AutoInfinityButton.isEnabled = false;
            if (AutoTimes == 0)
            {
                AutoBackground.spriteName = "btn_automatic_1";
                AutoValueLabel.enabled = false;
            }
            else
            {
                //圖片變自動(小)+文字
                AutoBackground.spriteName = "btn_automatic_2";
                if (AutoTimes == 1000)
                {
                    AutoValueLabel.enabled = false;
                    AutoValueLabel.text = "無限";
                }
                else
                {
                    AutoValueLabel.enabled = true;
                    AutoValueLabel.text = AutoTimes + "";
                }
            }
        }
    }

    void ClickAutoInfinity()
    {
        AutoTimes = 1000;
    }

    void ReleaseAutoSet()
    {
        AutoSeting = false;
    }

    void PressAutoSetUp()
    {
        AutoSeting = true;
        AutoUp = true;
        AutoTimes++;
    }

    void PressAutoSetDown()
    {
        AutoSeting = true;
        AutoDown = true;
        AutoTimes--;
    }

    void ShowAutoTimes()
    {
        if (AutoTimes > 60000) AutoTimes = 1000;
        else if (AutoTimes > 1000) AutoTimes = 0;
        else if (AutoTimes == 1000) AutoSetValueLabel.text = "無 限";
        else if (AutoTimes <= 999 && AutoTimes >= 0) AutoSetValueLabel.text = AutoTimes + "";

        if (NowButton != ButtonStatus.Auto)
        {
            if (AutoTimes == 0)
            {
                AutoBackground.spriteName = "btn_automatic_1";
                AutoValueLabel.enabled = false;
            }
            else
            {
                //圖片變自動(小)+文字
                AutoBackground.spriteName = "btn_automatic_2";
                if (AutoTimes == 1000)
                {
                    AutoValueLabel.enabled = true;
                    AutoValueLabel.text = "無 限";
                }
                else
                {
                    AutoValueLabel.enabled = true;
                    AutoValueLabel.text = AutoTimes + "";
                }
            }
        }
    }

    void ClickInfo()
    {
        if (NowButton != ButtonStatus.Info)
        {
            //如果其他按鈕有出現 進行互斥 並更新目前狀態
            if (NowButton != ButtonStatus.Idle) StatusChange(NowButton);
            NowButton = ButtonStatus.Info;
            InfoPosition.PlayForward();  //彈出設定欄位
            //開啟Collider
            InfoPosition.transform.GetChild(1).GetComponent<BoxCollider>().enabled = true;
            InfoPosition.transform.GetChild(2).GetComponent<BoxCollider>().enabled = true;
            InfoPosition.transform.GetChild(3).GetComponent<BoxCollider>().enabled = true;
        }
        else
        {
            NowButton = ButtonStatus.Idle;
            InfoPosition.PlayReverse();  //縮回設定欄位
            //關閉Collider
            InfoPosition.transform.GetChild(1).GetComponent<BoxCollider>().enabled = false;
            InfoPosition.transform.GetChild(2).GetComponent<BoxCollider>().enabled = false;
            InfoPosition.transform.GetChild(3).GetComponent<BoxCollider>().enabled = false;
        }
    }

    void GmaeInfoOnClick()
    {
        m_InfoObj.SetActive(true);
        m_MyAwardObj.SetActive(false);
        m_AllAwardObj.SetActive(false);
        IceAgeInstructions.Inst.OpenInstructions(true);
        if (NowButton == ButtonStatus.Info) StatusChange(ButtonStatus.Info);
    }

    void MyAwardOnClick()
    {
        m_InfoObj.SetActive(false);
        m_MyAwardObj.SetActive(true);
        m_AllAwardObj.SetActive(false);
        IceAgeManager.M_AwardPacket.m_bEnd = true;
        if (NowButton == ButtonStatus.Info) StatusChange(ButtonStatus.Info);
    }
    void MyAwardCloseOnClick()
    {
        m_MyAwardObj.SetActive(false);
    }
    void AllAwardOnClick()
    {
        if (!IceAgeManager.O_AwardPacket.m_bEnd && IceAgeManager.O_AwardStatus == IceAgeManager.O_AwardGetData.Idle)
            GameConnet.m_oGameClient.Send(GameCore.ENUM_GAME_FRAME.IceAge, (uint)GameCore.ENUM_COMMON_PACKID_GC.C2G_PMachine_ReqLobbyAwardRec, null);
        m_MyAwardObj.SetActive(false);
        m_InfoObj.SetActive(false);
        m_AllAwardObj.SetActive(true);
        IceAgeManager.O_AwardPacket.m_bEnd = true;
        if (NowButton == ButtonStatus.Info) StatusChange(ButtonStatus.Info);
    }
    void AllAwardCloseOnClick()
    {
        m_AllAwardObj.SetActive(false);
    }

    void ClickPack()
    {
        if (NowButton != ButtonStatus.Pack)
        {
            //如果其他按鈕有出現 進行互斥 並更新目前狀態
            if (NowButton != ButtonStatus.Idle) StatusChange(NowButton);
            NowButton = ButtonStatus.Pack;
            PackPosition.PlayForward();  //彈出設定欄位
            IceAgePackControl.Inst.BagMainOnClick();
        }
        else
        {
            NowButton = ButtonStatus.Idle;
            PackPosition.PlayReverse();  //縮回設定欄位
        }
    }

    public void AutoSetChangeComplete()
    {
        if (AutoPosition.transform.localPosition.y == 103)
        {
            AutoUpSprite.depth = ButtonAreakground.depth + 1;
            AutoDownSprite.depth = ButtonAreakground.depth + 1;
        }
    }
    public void BetSetChangeComplete()
    {
        if (BetPosition.transform.localPosition.y == 103)
        {
            BetUpSprite.depth = ButtonAreakground.depth + 1;
            BetDownSprite.depth = ButtonAreakground.depth + 1;
        }
    }
}
