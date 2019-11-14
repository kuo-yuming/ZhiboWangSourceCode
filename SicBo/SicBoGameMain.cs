using UnityEngine;
using System.Collections;
using GameCore;
using GameCore.Manager.SicBo;

public class SicBoGameMain : MonoBehaviour
{
    public static SicBoGameMain Inst;
    public UISprite TableBackground;    //桌檯背景
    public byte NowChipID;      //目前選擇的籌碼ID
    public uint NowQuota;       //目前選擇的籌碼面額
    public SicBoButtonControl ButtonControl;//按鈕控制
    public SicBoChipControl ChipControl;    //籌碼控制
    public SicBoBetAreaControl BetAreaControl;  //投注區域控制
    public SicBoWinAreaControl WinAreaControl;  //得獎區域控制
    public SicBoCircularScreen CircularControl; //圓形顯示器控制
    public SicBoRouletteTurnControl RouletteTurnControl;    //輪盤控制
    public SicBoPayoutResult PayoutResult;  //派彩結果控制
    public SicBoRecentHundred RecentHundred;  //近百局拉獎記錄
    public SicBoLotteryRecord LotteryRecord;    //開獎資訊
    public SicBoGameStatus NowStatus;    //目前遊戲狀態
    public uint StatusMSec; //狀態剩餘秒數    
    public bool GetStatusUpdate;    //接收到狀態更新
    public GameObject NowGamingPanel;   //目前正在遊戲中
    public float FristTimer = 0.0f; //首輪計時器
    public float WaitTimer = 0.0f;  //等待計時器    
    public bool[] Update3and4 = new bool[2] { false, false };   //更新未開局數
    public SicBoSoundControl SoundControl;  //音效

    public enum SicBoGameStatus
    {
        Idle = 0,       // 閒置 (沒人時的狀態)
        NewRound = 1,   // 新局開始
        WaitBet = 2,    // 等待押注 (通常是Buyin時會收到此狀態)
        StopBet = 3,    // 停止押注
        ShowAwards = 4, // 開獎中
        ShowWinArea = 5,// 顯示得獎區塊
        Payout = 6,     // 派彩中
        ShowPayoutResult = 7,   //顯示派彩結果
    }

    void Awake()
    {
        Inst = this;
    }

    //進入遊戲設定桌檯與籌碼
    void DoSetTableType()
    {
        AllScenceLoad.LoadScence = false;
        TableBackground.spriteName = "GameTable" + SicBoManager.NowGroup;   //設定桌面
        ChipControl.SetChipType(SicBoManager.NowGroup); //設定籌碼
        ChipControl.SetNowChoose(1);    //預設選擇籌碼
        ButtonControl.SetMachineID();   //桌檯編號
        ButtonControl.UpdatePlayersCnt = true;  //更新玩家人數
        ButtonControl.InitShadowArea = true;    //初始化陰影區塊資訊        
        BetAreaControl.InitBetUnit();   //初始化各投注區
        WinAreaControl.InitWinUnit();   //初始化各閃爍區
        CircularControl.InitShow();     //初始化顯示器
        LotteryRecord.InitLotteryRecord();  //初始化開獎清單
        NowGamingPanel.SetActive(true); //目前正在遊戲中   遮罩開啟
        FristTimer = 1.0f;   //一秒後進行狀態判斷
        //設定完畢 通知伺服器
        GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.SicBo, (uint)ENUM_COMMON_PACKID_GC.C2G_TMachine_GameReady, null);
    }

    // Update is called once per frame
    void Update()
    {
        if (FristTimer > 0)
        {
            FristTimer -= Time.deltaTime;
            if (FristTimer < 0.0f) FristTimer = 0.0f;
        }
        if (SicBoManager.SetTableType)
        {   //進入遊戲先執行桌檯背景與籌碼設定
            SicBoManager.SetTableType = false;
            DoSetTableType();
        }

        if (FristTimer == 0.0f)
        {
            switch (NowStatus)
            {
                case SicBoGameStatus.Idle:       //閒置
                    break;
                case SicBoGameStatus.NewRound:   //新局開始
                case SicBoGameStatus.WaitBet:    //等待押注 (通常是Buyin時會收到此狀態)
                    if (GetStatusUpdate)
                    {
                        GetStatusUpdate = false;
                        if (SicBoManager.BuyInFrist)
                        {   //如果是進遊戲的第一個判斷 不管是新局開始或等待押注 都同一個處理
                            SicBoManager.BuyInFrist = false;
                            if (StatusMSec / 1000 > 5.0f)
                            {   //如果等待投注時間 > 5秒  正常開放
                                BetAreaControl.OpenBet();   //開放投注
                                CircularControl.FristBetCountdownSetting(StatusMSec / 1000);  //圓形顯示器 首次倒數設定
                                NowGamingPanel.SetActive(false);    //目前正在遊戲中 遮罩關閉
                            }
                            else //等待投注時間 <= 5秒
                            {
                                //不做事 等待下一輪
                            }
                        }
                        else //第二次判斷以後 
                        {
                            if (NowStatus == SicBoGameStatus.NewRound)
                            {
                                if (NowGamingPanel.activeSelf) NowGamingPanel.SetActive(false); //如果遮罩開啟中 遮罩關閉
                                CircularControl.BetCountdownSetting(StatusMSec / 1000);  //圓形顯示器 倒數設定
                            }
                            else if (NowStatus == SicBoGameStatus.WaitBet)
                            {
                                //第二次判斷以後 通常不會給 WaitBet 所以不做事
                            }
                        }
                    }
                    break;
                case SicBoGameStatus.StopBet:    //停止押注
                    if (SicBoManager.BuyInFrist)
                    {   //如果是進遊戲的第一個判斷 持續遮擋畫面
                        SicBoManager.BuyInFrist = false;
                        GetStatusUpdate = false;
                    }
                    else if (GetStatusUpdate && BetAreaControl.BetData_Hold.Count == 0)
                    {   //如果沒有投注資料待處理 GetStatusUpdate = false 並開始執行開獎動畫
                        GetStatusUpdate = false;
                        PayoutResult.CheckBet();  //紀錄押注結果
                        RouletteTurnControl.RouletteTurnStart();//開始移動輪盤
                        CircularControl.MoveCircularScreen();   //切換成開獎模式                        
                        if (ButtonControl.AutoTimes > 0)    //如果在自動模式下 
                            BetAreaControl.CheckAutoBet();  //記錄目前押注區與金額 已利自動押注
                        else //自動次數 = 0
                            BetAreaControl.AutoBetArea.Clear(); //清空自動押注
                    }
                    break;
                case SicBoGameStatus.ShowAwards:    //開獎中
                    if (GetStatusUpdate)
                    {
                        GetStatusUpdate = false;
                        RouletteTurnControl.StartShowAwsrds = true; //輪盤開始開獎
                    }
                    break;
                case SicBoGameStatus.ShowWinArea:   //顯示中獎區域
                    if (WaitTimer != 0.0f)
                    {
                        WaitTimer -= Time.deltaTime;
                        if (WaitTimer < 0.0f)
                        {
                            WaitTimer = 0.0f;
                            DoReturnTable();    //恢復到桌檯畫面
                        }
                    }
                    break;
                case SicBoGameStatus.Payout:    //派彩中
                    if (WaitTimer > 0.0f)
                    {
                        WaitTimer -= Time.deltaTime;
                        if (WaitTimer < 0.0f)
                        {
                            WaitTimer = 0.0f;
                            CircularControl.MessageText.spriteName = "text_balance";    //顯示訊息文字 - 派彩中
                            BetAreaControl.DoPayout();  //開始派彩 籌碼移動
                            CircularControl.MessageSound.clip = CircularControl.MessageClip[11].clip;
                            CircularControl.MessageSound.Play();    //播放音效
                            NowStatus = SicBoGameStatus.ShowPayoutResult; //設定狀態 - 顯示派彩結果
                            WaitTimer = 2.3f;   //設定等待時間
                        }
                    }
                    break;
                case SicBoGameStatus.ShowPayoutResult:  //顯示派彩結果
                    if (WaitTimer > 0.0f)
                    {
                        WaitTimer -= Time.deltaTime;
                        if (WaitTimer < 0.0f)
                        {
                            WaitTimer = 0.0f;
                            WinAreaControl.CloseFlashing(); //關閉得獎區塊閃爍
                            CircularControl.MessageText.spriteName = "text_wait";    //顯示訊息文字 - 等待中
                            CircularControl.MessageSound.clip = CircularControl.MessageClip[12].clip;
                            CircularControl.MessageSound.Play();    //播放音效
                            PayoutResult.ShowPanel();   //顯示派彩結果
                            ButtonControl.SetMoney(SicBoManager.NoitfyAwardData.m_ui64GameMoney);   //設定金幣與押注
                        }
                    }
                    break;
            }
        }
    }

    public void DoSettlement()
    {   //執行結算
        NowStatus = SicBoGameStatus.ShowWinArea;    //切換狀態
        WaitTimer = 0.0f;   //設定計時器
        SoundControl.PlayResultSound(RouletteTurnControl.RouletteDiceNumber, RouletteTurnControl.Roulette4DiceNumber);  //播放音效
        BetAreaControl.InitPayForWho(); //初始化派彩對象
        BetAreaControl.CheckPayForWho();//確認派彩對象
        WinAreaControl.DetermineWinArea();  //判斷得獎區塊            //更新百局名單
        RecentHundred.UpdateHundredArray(RouletteTurnControl.RouletteDiceNumber[0], RouletteTurnControl.RouletteDiceNumber[1], RouletteTurnControl.RouletteDiceNumber[2], RouletteTurnControl.Roulette4DiceNumber);
        LotteryRecord.UpdateLotteryRecord(RouletteTurnControl.RouletteDiceNumber[0], RouletteTurnControl.RouletteDiceNumber[1], RouletteTurnControl.RouletteDiceNumber[2], RouletteTurnControl.Roulette4DiceNumber);
        //更新未開局數
        if (Update3and4[0])
            ButtonControl.UpdateNowAnyTriple = true;
        else
        {
            ButtonControl.LotteryRecord[0] = 0;
            ButtonControl.UpdateNowAnyTriple = true;
        }
        if (Update3and4[1])
            ButtonControl.UpdateNowAnyQuadruple = true;
        else
        {
            ButtonControl.LotteryRecord[1] = 0;
            ButtonControl.UpdateNowAnyQuadruple = true;
        }
    }

    void DoReturnTable()
    {   //恢復到桌檯畫面
        CircularControl.transform.localPosition = new Vector3(0, 257, 0);   //輪盤歸位
        CircularControl.SelfPosition.ResetToBeginning();
        CircularControl.SelfPosition.enabled = false;   //TweenPosition關閉並初始化
        CircularControl.RouletteScreenPanel.alpha = 0.0f;   //輪盤區隱藏
        CircularControl.MaskPanel.SetActive(false); //遮罩隱藏
        CircularControl.MessageText.transform.localScale = Vector3.one; //調整訊息文字
        CircularControl.MessageText.transform.localRotation = Quaternion.Euler(Vector3.zero);    //調整訊息文字
        RouletteTurnControl.Roulette4Sprite.transform.localScale = Vector3.zero;    //隱藏四號輪盤
        RouletteTurnControl.RouletteShadow.enabled = false; //隱藏輪盤陰影        
        WinAreaControl.DoFlashing();    //顯示得獎區塊
        NowStatus = SicBoGameStatus.Payout; //設定狀態 - 派彩中
        WaitTimer = 2.0f;   //設定等待時間
    }
}