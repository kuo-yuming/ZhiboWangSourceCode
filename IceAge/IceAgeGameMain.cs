using UnityEngine;
using System;
using System.Collections;
using GameCore;
using GameCore.Manager.Common;
using GameCore.Manager.IceAge;
using GameCore.Machine;

public class IceAgeGameMain : MonoBehaviour
{
    public static IceAgeGameMain Inst;
    public UIGrid WinNumber;
    public Game_Status GameStatus = Game_Status.Idle;
    private float ReStartTime = 0.2f;
    private float BonusChangeWaitTime = 2.0f;
    public float ReStartTimer = 0;
    public string MiniGameName = "";
    public Transform BonusGameText;
    public byte BonusNumber = 0;

    public enum Game_Status
    {
        Idle,
        Start,
        SlotMoving,     //自動轉動
        WaitMoneySlot,  //等待金錢特效結束
        GetScore,       //按下得分
        MiniGame,       //小遊戲開始
        MiniGameSuccess,//小遊戲成功
        MiniGameFail,   //小遊戲失敗
        FreeGame,       //FreeGmae
        GetJP,          //得到JP
    }
    void Awake()
    {
        Inst = this;
    }
    void Start()
    {
        AllScenceLoad.LoadScence = false;
        IceAgeMoneySlot.Inst.TargetNumber = GameConnet.m_BuyInMoney;
        IceAgeMoneySlot.Inst.NowChange = true;
        BonusGameText.GetComponent<UISprite>().enabled = false;
        BonusGameText.GetComponent<UISpriteAnimation>().enabled = false;
    }
    void Update()
    {
        switch (GameStatus)
        {
            case Game_Status.SlotMoving:
                if (ReStartTimer != 0)
                {
                    ReStartTimer += Time.deltaTime;
                    if (ReStartTimer > ReStartTime)
                        IceAgeWinMoney.Inst.m_WinWord.Loop = false;
                }
                break;
            case Game_Status.WaitMoneySlot:
                if (ReStartTimer != 0)
                {
                    ReStartTimer += Time.deltaTime;
                    if (ReStartTimer > ReStartTime)
                        if (IceAgeButtonControl.Inst.AutoTimes == 0)
                            IceAgeButtonControl.Inst.SetStopButton();   //重置按鈕
                        else
                            IceAgeSlotControl.Inst.SlotStartReady();   //開始下一輪
                }
                break;
            case Game_Status.GetScore:
                if (ReStartTimer != 0)
                {
                    ReStartTimer += Time.deltaTime;
                    if (ReStartTimer > ReStartTime)
                    {
                        if (IceAgeButtonControl.Inst.AutoTimes == 0) IceAgeButtonControl.Inst.SetStopButton();   //重置按鈕
                        else IceAgeSlotControl.Inst.SlotStartReady();   //開始下一輪
                    }
                }
                break;
            case Game_Status.MiniGame:
                if (MiniGameName != "DoubleUp" && ReStartTimer != 0)
                {   //比倍會直接進入轉場 Bonus會等兩秒 再進入轉場
                    ReStartTimer += Time.deltaTime;
                    if (ReStartTimer > BonusChangeWaitTime)
                    {   //切換進場動畫
                        if (MiniGameName == "ShotGame") MiniGameChangeScene.Inst.GotoChangeScene("ShotGame");   //射擊
                        else if (MiniGameName == "BreakGame") MiniGameChangeScene.Inst.GotoChangeScene("BreakGame");    //敲蛋
                        MiniGameChangeScene.Inst.MiniGameEnd = false;   //遊戲中
                        IceAgeWinMoney.Inst.Reseat = true;  //重置WinMoney動畫        
                        IceAgeLineAnimControl.Inst.ReSetLineAnim(); //重置連線特效
                        ReStartTimer = 0.0f;
                    }
                }
                break;
            case Game_Status.MiniGameSuccess:
                if (ReStartTimer != 0)
                {
                    ReStartTimer += Time.deltaTime;
                    if (ReStartTimer > ReStartTime)
                    {
                        IceAgeWinMoney.Inst.m_WinWord.Loop = false;
                        ReStartTimer = 0.0f;
                    }
                }
                break;
            case Game_Status.FreeGame:
                if (ReStartTimer != 0)
                {
                    ReStartTimer += Time.deltaTime;
                    if (ReStartTimer > ReStartTime) IceAgeSlotControl.Inst.SlotStartReady();  //執行轉動
                }
                break;
            case Game_Status.GetJP:
                if (ReStartTimer != 0)
                {
                    ReStartTimer += Time.deltaTime;
                    if (ReStartTimer > ReStartTime) IceAgeButtonControl.Inst.SetGetScoreButton();   //開放比倍跟得分
                }
                break;
        }
    }

    void OnClickStart()
    {
        if (GameStatus == Game_Status.Idle) GameStatus = Game_Status.Start;
        IceAgeSlotControl.Inst.SlotStartReady();  //執行轉動
    }

    void OnClickStop()
    {   //判斷是否已經接收到獎項
        if (IceAgeManager.GetAward)
        {
            IceAgeManager.GetAward = false;
            IceAgeSlotControl.Inst.AutoStopTimer = 0.0f;   //自動停止 計時器歸零
            IceAgeSlotControl.Inst.ShowStopTimer = 0.0f;   //停止按鈕 計時器歸零
            IceAgeButtonControl.Inst.SetMovingButton();
            foreach (var item in IceAgeSlotControl.Inst.IceAgeSM)
            {
                item.SelfMoving = false;    //停止自控轉動
                item.SetSelfSprite();       //指定圖片
                item.SelfStopMove = true;   //打開自控停止
            }
        }
    }

    void OnClickMoneyDouble()
    {   //按下比倍
        GameStatus = Game_Status.MiniGame;
        if (MiniGameName == "ShotGame" || MiniGameName == "BreakGame") MiniGameChangeScene.Inst.IsBonusDouble = true;
        if (MiniGameName != "JP") MiniGameName = "DoubleUp";
        MiniGameChangeScene.Inst.GotoChangeScene("DoubleUp");   //切換進場動畫
        MiniGameChangeScene.Inst.MiniGameEnd = false;
        IceAgeButtonControl.Inst.SetDoubleUpGameButtonShow();   //設定按鈕
        IceAgeWinMoney.Inst.Reseat = true;  //重置WinMoney動畫        
        IceAgeLineAnimControl.Inst.ReSetLineAnim(); //重置連線特效
        IceAge_JP_Control.Inst.JPAnim.GetComponent<UISprite>().enabled = false; //關閉JP特效
        IceAge_JP_Control.Inst.JPAnim.GetComponent<UISpriteAnimation>().enabled = false;
    }

    void OnGetScore()
    {   //按下得分
        ReStartTimer = 0.0f;
        IceAgeButtonControl.Inst.SetMovingButton();
        if (GameStatus == Game_Status.GetJP)
        {   //更新狀態
            GameStatus = Game_Status.GetScore;
            //關閉特效            
            IceAgeLineAnimControl.Inst.ReSetLineAnim();
            IceAge_JP_Control.Inst.JPAnim.GetComponent<UISprite>().enabled = false;
            IceAge_JP_Control.Inst.JPAnim.GetComponent<UISpriteAnimation>().enabled = false;
            //顯示WinMoney & 重設JP顯示
            IceAgeWinMoney.Inst.WinMoney = (long)IceAgeManager.m_BetResult.m_uiScore;
            IceAgeWinMoney.Inst.StartPlay = true;
            IceAge_JP_Control.Inst.TargetNumber = 0;
            IceAge_JP_Control.Inst.NowChange = true;
        }
        else
        {
            if (GameStatus != Game_Status.GetScore && GameStatus != Game_Status.MiniGameSuccess)
            {
                GameStatus = Game_Status.GetScore;
                IceAgeMoneySlot.Inst.TargetNumber = IceAgeManager.m_BetResult.m_ui64GameMoney;
                IceAgeMoneySlot.Inst.NowChange = true;
            }
            IceAgeLineAnimControl.Inst.ReSetLineAnim();
            IceAgeWinMoney.Inst.m_WinWord.Loop = false;
            IceAgeWinMoney.Inst.m_WinWord.OneceOver = true;
        }
    }

    public void OnLeaveDoubleUp()
    {   //離開比倍
        IceAgeButtonControl.Inst.LeaveDoubleUpButton.isEnabled = false; //關閉離開比倍按鈕
        MiniGameChangeScene.Inst.MiniGameEnd = true;    //設定狀態與Timer 回到MainGame
        DoubleUp_Control.Inst.WaitTimer = DoubleUp_Control.Inst.WaitTime + 1.0f;    //等待時間+1 直接開始返回MainGame流程
        DoubleUp_Control.Inst.GameTimer = 10.0f;    //重置計時器
    }

    public void GetBetResult()
    {
        if (IceAgeManager.IsBonus)
        {
            BonusNumber = IceAgeManager.BonusNumber;
            switch (BonusNumber)
            {
                case 27:    //射擊
                    BonusGameText.GetComponent<UISprite>().enabled = true;
                    BonusGameText.GetComponent<UISpriteAnimation>().ResetToBeginning();
                    BonusGameText.GetComponent<UISpriteAnimation>().enabled = true;
                    GameStatus = Game_Status.MiniGame;
                    MiniGameName = "ShotGame";
                    ReStartTimer = 0.01f;
                    IceAgeLineAnimControl.Inst.PlayLineAnim(true);  //顯示得獎特效
                    IceAgeButtonControl.Inst.SetMoneyDoubleButtonShow();    //設定按鈕
                    break;
                case 36:    //敲蛋
                    BonusGameText.GetComponent<UISprite>().enabled = true;
                    BonusGameText.GetComponent<UISpriteAnimation>().ResetToBeginning();
                    BonusGameText.GetComponent<UISpriteAnimation>().enabled = true;
                    GameStatus = Game_Status.MiniGame;
                    MiniGameName = "BreakGame";
                    ReStartTimer = 0.01f;
                    IceAgeLineAnimControl.Inst.PlayLineAnim(true);  //顯示得獎特效
                    IceAgeButtonControl.Inst.SetMoneyDoubleButtonShow();    //設定按鈕
                    break;
                case 37:    //JP
                    GameStatus = Game_Status.GetJP;
                    MiniGameName = "JP";
                    IceAgeLineAnimControl.Inst.PlayLineAnim(true);  //顯示得獎特效
                    IceAgeButtonControl.Inst.SetMoneyDoubleButtonShow();    //設定按鈕
                    IceAge_JP_Control.Inst.JPAnim.GetComponent<UISprite>().enabled = true;
                    IceAge_JP_Control.Inst.JPAnim.GetComponent<UISpriteAnimation>().ResetToBeginning();
                    IceAge_JP_Control.Inst.JPAnim.GetComponent<UISpriteAnimation>().enabled = true;
                    ReStartTimer = 1.0f;    //一秒後 開放比倍與得分按鈕
                    break;
                default:
                    break;
            }
        }
        else
        {
            if (IceAgeManager.m_BetResult.m_byFreeRoundCnt != 0)
            {
                if (IceAgeManager.m_BetResult.m_byFreeRoundCnt != 20 && IceAgeManager.m_BetResult.m_dicLineAward.Count != 0)
                {   //FreeGame中 正常得獎
                    IceAgeLineAnimControl.Inst.PlayLineAnim(false);  //顯示得獎特效
                }
                else
                {   //轉到FreeGame時 FreeRoundCnt = 20
                    GameStatus = Game_Status.FreeGame;  //進入FreeGame
                    IceAgeLineAnimControl.Inst.PlayFreeGameSlotBox();  //顯示鑽石Icon的紅外框
                    FreeGame_Control.Inst.ChangeGameTitle(false);   //調整背景 與 遊戲標題
                    IceAgeBackControl.Inst.m_AnimControl.SetBool("IsDay", false);
                    //鎖定按鈕
                    IceAgeButtonControl.Inst.SetFreeGameButton();
                    ReStartTimer = 0.01f;
                }
            }
            else
            {
                if (GameStatus == Game_Status.FreeGame)
                {   //調整背景 與 遊戲標題
                    FreeGame_Control.Inst.ChangeGameTitle(true);
                    IceAgeBackControl.Inst.m_AnimControl.SetBool("IsDay", true);
                    GameStatus = Game_Status.SlotMoving;    //變更遊戲狀態
                    //恢復顯示投注金額
                    IceAgeButtonControl.Inst.BetValue.text = (IceAgeButtonControl.Inst.NumberOfBets * GameConnet.m_PMachineConfig.m_usBetMoney * GameConnet.m_PMachineConfig.m_byMaxLine).ToString();
                    IceAgeButtonControl.Inst.BetSetValue.text = IceAgeButtonControl.Inst.BetValue.text;
                }

                if (IceAgeManager.m_BetResult.m_dicLineAward.Count != 0)
                {
                    IceAgeLineAnimControl.Inst.PlayLineAnim(false); //顯示得獎特效                    
                    IceAgeButtonControl.Inst.SetMoneyDoubleButtonShow();    //顯示按鈕
                    IceAgeWinMoney.Inst.m_WinWord.Loop = false; //WinMoney 特效停在畫面上Loop
                }
                else
                {   //沒有獎項
                    IceAgeSoundControl.Inst.PlaySound(IceAgeSoundControl.SoundName.NotGetAwards, false);   //播放音效
                    if (IceAgeButtonControl.Inst.AutoTimes == 0) IceAgeButtonControl.Inst.SetStopButton(); // 次數 = 0  結束轉動 設定按鈕狀態
                    else
                    {
                        GameStatus = Game_Status.WaitMoneySlot;
                        ReStartTimer = 1.0f; //若還有自動次數 等待一秒後 轉動
                    }
                }
            }
        }
        //使用道具結束 把道具使用中顯示關閉
        if (IceAgeManager.m_BetResult.m_uiUseItemID > 0)
            IceAgeItemUse.m_ItemClose = true;
        //IceAgeManager.GetAward = false;
    }
}
