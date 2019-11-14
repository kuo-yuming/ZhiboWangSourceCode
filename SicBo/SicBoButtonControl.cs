using UnityEngine;
using System.Collections;

public class SicBoButtonControl : MonoBehaviour
{
    public ButtonStatus NowButton;
    //機台設定
    public UILabel MachineID;           //機台編號
    public UISprite ButtonAreakground;  //按鈕背景
    public UILabel[] CashText;          //Label元件
    private ulong AllMoney;             //金幣+押注的總額
    public ulong NowMoney;              //目前金幣
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
    public bool NowCanAuto = true;      //現在是否可開啟自動模式 在押注金額不夠時會停止
    public UIButton CancelBetButton;    //取消押注 按鈕
    //資訊
    public TweenPosition InfoPosition;
    public BoxCollider m_InstructionsCollider;
    public BoxCollider m_RecentHundredCollider;
    public BoxCollider m_LotteryRecordCollider;
    public UIPanel m_InstructionsPanel;
    public UIPanel m_RecentHundredPanel;
    public UIPanel m_LotteryRecordPanel;
    public GameObject m_InfoMask;
    public SicBoInstructions m_Instructions;
    //陰影區塊資訊
    public UILabel PlayersCount;//目前桌檯玩家人數
    public UILabel BetRange;    //押注範圍
    public UILabel NowAnyTriple;    //未開圍骰局數
    public UILabel NowAnyQuadruple; //未開四枚局數
    public bool InitShadowArea;     //初始化陰影區塊訊息
    public bool UpdatePlayersCnt;   //更新玩家人數
    public ushort[] LotteryRecord = new ushort[2] { 0, 0 };  //記錄 未開圍骰局數 / 未開四枚局數
    public bool UpdateNowAnyTriple; //更新未開圍骰局數
    public bool UpdateNowAnyQuadruple;  //更新未開四枚局數

    public enum ButtonStatus
    {
        Idle = 0,
        Bet = 11,
        Auto = 21,
        Pack = 31,
        Info = 41,
    }

    // Use this for initialization
    void Start()
    {
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
        CancelBetButton.isEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
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

        if (InitShadowArea)
        {
            InitShadowArea = false;
            DoInitShadowArea();
            SetMoney(GameConnet.m_BuyInMoney);
        }
        if (UpdatePlayersCnt)
        {
            UpdatePlayersCnt = false;
            DoUpdatePlayersCnt();
        }
        if (UpdateNowAnyTriple)
        {
            UpdateNowAnyTriple = false;
            DoUpdateNowAnyTriple();
        }
        if (UpdateNowAnyQuadruple)
        {
            UpdateNowAnyQuadruple = false;
            DoUpdateNowAnyQuadruple();
        }
        if (!NowCanAuto)
        {   //押注時餘額不足
            NowCanAuto = true;
            AutoTimes = 0;  //關閉自動押注
        }
    }

    public void SetMoney(ulong Money)
    {
        AllMoney = Money;  //取得總額
        NowMoney = AllMoney;    //剛進入機台與每局結算時 金幣=總額
        CashText[0].text = AllMoney.ToString(); //顯示金幣
        CashText[1].text = "0"; //押注歸零
    }

    public void SetBet(uint Bet)
    {
        NowMoney = (uint)AllMoney - Bet;    //計算目前金幣
        CashText[0].text = NowMoney.ToString(); //顯示金幣
        CashText[1].text = Bet.ToString(); //顯示押注
    }

    void DoInitShadowArea()
    {   //顯示押注範圍 //如果 押注範圍 < 1000 = 押注範圍  否則 押注範圍 = 押注範圍 / 1000 + K 例: 1000 = 1000 , 100000 = 100K
        string MinBet = (SicBoManager.m_MachineBuyInConfig.m_dicTableGroupSet[SicBoManager.NowGroup].m_uiMinBetMoney < 1000) ? SicBoManager.m_MachineBuyInConfig.m_dicTableGroupSet[SicBoManager.NowGroup].m_uiMinBetMoney.ToString() : SicBoManager.m_MachineBuyInConfig.m_dicTableGroupSet[SicBoManager.NowGroup].m_uiMinBetMoney / 1000 + "K";
        string MaxBet = (SicBoManager.m_MachineBuyInConfig.m_dicTableGroupSet[SicBoManager.NowGroup].m_uiMaxBetMoney < 1000) ? SicBoManager.m_MachineBuyInConfig.m_dicTableGroupSet[SicBoManager.NowGroup].m_uiMaxBetMoney.ToString() : SicBoManager.m_MachineBuyInConfig.m_dicTableGroupSet[SicBoManager.NowGroup].m_uiMaxBetMoney / 1000 + "K";
        BetRange.text = MinBet + "~" + MaxBet;
    }

    void DoUpdateNowAnyTriple()
    {   //顯示未開圍骰局數
        NowAnyTriple.text = LotteryRecord[0].ToString();
        LotteryRecord[0]++;
    }

    void DoUpdateNowAnyQuadruple()
    {   //顯示未開四枚局數
        NowAnyQuadruple.text = LotteryRecord[1].ToString();
        LotteryRecord[1]++;
    }

    void DoUpdatePlayersCnt()
    {
        PlayersCount.text = SicBoManager.m_MachineDatas[GameConnet.m_TMachineBuyInGameData.m_uiTID].m_usMemberCnt + "/" + SicBoManager.m_MachineBuyInConfig.m_dicTableGroupSet[SicBoManager.NowGroup].m_usCapacity;
    }

    public void SetMachineID()
    {
        MachineID.text = GameConnet.m_TMachineBuyInGameData.m_uiTID.ToString("000");
    }

    public void StatusChange(ButtonStatus NowBS)
    {
        switch (NowBS)
        {
            case ButtonStatus.Idle:
                if (NowButton != ButtonStatus.Idle) StatusChange(NowButton);
                NowButton = ButtonStatus.Idle;
                break;
            case ButtonStatus.Auto:
                ClickAuto();
                break;
            case ButtonStatus.Info:
                ClickInfo();
                break;
        }
    }

    void ClickBack()
    {
        if (NowButton != ButtonStatus.Idle) StatusChange(NowButton);
        NowButton = ButtonStatus.Idle;
        SicBoManager.BackLobbySetting = true;
        GameConnet.BuyOut_GameLobbySuccess = true;
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
            AutoUpButton.isEnabled = true;
            AutoDownButton.isEnabled = true;
            AutoInfinityButton.isEnabled = true;
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

    void ClickInfo()
    {
        if (NowButton != ButtonStatus.Info)
        {
            //如果其他按鈕有出現 進行互斥 並更新目前狀態
            if (NowButton != ButtonStatus.Idle) StatusChange(NowButton);
            NowButton = ButtonStatus.Info;
            InfoPosition.PlayForward();  //彈出設定欄位
            //開啟Collider
            m_InstructionsCollider.enabled = true;
            m_RecentHundredCollider.enabled = true;
            m_LotteryRecordCollider.enabled = true;
        }
        else
        {
            NowButton = ButtonStatus.Idle;
            InfoPosition.PlayReverse();  //縮回設定欄位
            //關閉Collider
            m_InstructionsCollider.enabled = false;
            m_RecentHundredCollider.enabled = false;
            m_LotteryRecordCollider.enabled = false;
        }
    }

    void InstructionsOnClick()
    {   //點擊遊戲說明
        m_Instructions.OpenInstructions();  //遊戲說明初始頁面
        m_InstructionsPanel.enabled = true;     //開啟遊戲說明
        m_RecentHundredPanel.enabled = false;   //關閉近百局
        m_LotteryRecordPanel.enabled = false;   //關閉開獎記錄
        m_InfoMask.SetActive(true); //開啟遮罩
        if (NowButton == ButtonStatus.Info) StatusChange(ButtonStatus.Info);    //對按鈕進行互斥
    }

    void InstructionsOnClose()
    {   //關閉遊戲說明
        m_Instructions.CloseInstructions(); //關閉遊戲說明按鈕
        m_InstructionsPanel.enabled = false;
        m_InfoMask.SetActive(false);
    }

    void RecentHundredOnClick()
    {   //點擊近百局
        SicBoGameMain.Inst.RecentHundred.ShowRecentHundred();
        m_RecentHundredPanel.enabled = true;    //開啟近百局
        m_InstructionsPanel.enabled = false;    //關閉遊戲說明
        m_LotteryRecordPanel.enabled = false;   //關閉開獎記錄
        m_InfoMask.SetActive(true); //開啟遮罩
        if (NowButton == ButtonStatus.Info) StatusChange(ButtonStatus.Info);    //對按鈕進行互斥
    }
    void RecentHundredOnClose()
    {   //關閉近百局
        SicBoGameMain.Inst.RecentHundred.HideRecentHundred();  //關閉按鈕
        m_RecentHundredPanel.enabled = false;
        m_InfoMask.SetActive(false);
    }
    void LotteryRecordOnClick()
    {   //點擊開獎記錄
        SicBoGameMain.Inst.LotteryRecord.ShowLotteryRecord();
        m_LotteryRecordPanel.enabled = true;    //開啟開獎記錄
        m_RecentHundredPanel.enabled = false;   //關閉近百局
        m_InstructionsPanel.enabled = false;    //關閉近百局
        m_InfoMask.SetActive(true); //開啟遮罩
        if (NowButton == ButtonStatus.Info) StatusChange(ButtonStatus.Info);    //對按鈕進行互斥
    }
    void LotteryRecordOnClose()
    {   //關閉開獎記錄
        SicBoGameMain.Inst.LotteryRecord.HideLotteryRecord();   //關閉按鈕
        m_LotteryRecordPanel.enabled = false;
        m_InfoMask.SetActive(false);
    }

    public void AutoSetChangeComplete()
    {
        if (AutoPosition.transform.localPosition.y == 103)
        {
            AutoUpSprite.depth = ButtonAreakground.depth + 1;
            AutoDownSprite.depth = ButtonAreakground.depth + 1;
        }
    }
}
