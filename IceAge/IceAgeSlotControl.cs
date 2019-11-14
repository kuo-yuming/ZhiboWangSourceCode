using UnityEngine;
using System.Collections;
using GameCore.Manager.IceAge;
using GameCore;
using System;

public class IceAgeSlotControl : MonoBehaviour
{
    public static IceAgeSlotControl Inst;
    public IceAgeSlotMove[] IceAgeSM;
    public float ShowStopTimer;
    private float CanStopTime = 0.5f;
    private float AutoStopTime = 1.5f;
    private float OverMoveTimer = 0.0f;
    private float StopSpeedMode = 0.3f;
    public float speed = 4000.0f;  //初始速度
    public float SpeedRange;    //速度

    public bool Moving = false;  //開始轉動    
    public bool StopMove = false;   //停止轉動
    public bool OverMove = false;   //減緩轉動 (即將停止轉動)
    public ushort MoveRange = 276;
    public byte NumOfStopSlot = 0;  //已經停止的Slot
    public float AutoStopTimer = 0.0f;
    float EverySlotStopTime = 0.3f;
    byte NowStopSlot = 0;

    void Awake()
    {
        Inst = this;
    }
    // Update is called once per frame
    void Update()
    {
        MoveSpeed();
        if (ShowStopTimer != 0)
        {
            ShowStopTimer += Time.deltaTime;
            /*if (IceAgeManager.GetAward && !IceAgeButtonControl.Inst.StopButton.isEnabled)
                IceAgeButtonControl.Inst.UnhideStopButton(); //可以進行手動停止*/

            if (ShowStopTimer > CanStopTime && !IceAgeButtonControl.Inst.StopButton.isEnabled)
            {   //可以進行手動停止
                IceAgeButtonControl.Inst.UnhideStopButton();
            }
            else if (ShowStopTimer > AutoStopTime)
            {   //進行自動停止
                IceAgeButtonControl.Inst.SetMovingButton(); //  轉動時的按鈕狀態
                OverMoveTimer = 0.001f; //減緩轉動 計時器 
                AutoStopTimer = 0.01f;  //自動停止 計時器開啟
                ShowStopTimer = 0.0f;   //停止按鈕 計時器歸零
            }
        }

        if (OverMoveTimer != 0)
        {
            if (OverMoveTimer > StopSpeedMode)
            {
                OverMove = true;
                OverMoveTimer = 0;
            }
            else
                OverMoveTimer += Time.deltaTime;
        }

        if (AutoStopTimer != 0)
        {
            AutoStopTimer += Time.deltaTime;
            if (NowStopSlot == 0)   //如果是第一個 計算位置 給圖 直接停止
            {
                IceAgeSM[0].SelfMoving = false; //停止自控轉動
                IceAgeSM[0].SetSelfSprite();  //指定圖片
                IceAgeSM[0].SelfStopMove = true;//打開自控停止
                AutoStopTimer = 0.01f;
                NowStopSlot++;
            }
            else if (AutoStopTimer > EverySlotStopTime) //依序停止
            {
                IceAgeSM[NowStopSlot].SelfMoving = false;
                IceAgeSM[NowStopSlot].SetSelfSprite();
                IceAgeSM[NowStopSlot].SelfStopMove = true;
                AutoStopTimer = 0.01f;
                NowStopSlot++;

                if (NowStopSlot == 5)  //如果全部停止 歸零
                {
                    AutoStopTimer = 0.0f;
                    NowStopSlot = 0;
                    OverMove = false;
                }
            }
        }
    }

    void MoveSpeed()
    {
        if (Moving)
        {
            if (!OverMove)  //正常轉動時的速度
            {
                speed = speed + SpeedRange * Time.deltaTime;
                if (speed > 5000.0f)
                    speed = 5000.0f;
            }
            else //減緩轉動時的速度
            {
                speed = speed - SpeedRange * Time.deltaTime;
                if (speed <= 4000)
                {
                    speed = 4000;
                    Moving = false;
                    StopMove = true;
                }
            }
        }
    }

    public void SlotStartReady()
    {
        if (IceAgeGameMain.Inst.GameStatus != IceAgeGameMain.Game_Status.FreeGame)
        {   //MainGame
            if ((int)IceAgeMoneySlot.Inst.NowNumber - (IceAgeButtonControl.Inst.NumberOfBets * GameConnet.m_PMachineConfig.m_usBetMoney * GameConnet.m_PMachineConfig.m_byMaxLine) >= 0)
            {
                GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.IceAge, (uint)ENUM_ICEAGE_PACKID_GC.C2G_Bet_ReqBet, GameConnet.m_oGameClient.DoSerialize<byte>(IceAgeButtonControl.Inst.NumberOfBets));
                //ButtonStatus
                if (IceAgeButtonControl.Inst.AutoTimes > 0 && IceAgeButtonControl.Inst.AutoTimes != 1000 && IceAgeGameMain.Inst.GameStatus != IceAgeGameMain.Game_Status.Start)
                    IceAgeButtonControl.Inst.AutoTimes--; //如果自動次數 > 0 且 != 無限 且 IceAgeGameMain.Inst.GameStatus != IceAgeGameMain.Game_Status.Start
                IceAgeButtonControl.Inst.SetMovingButton(); //  轉動時的按鈕狀態
                ShowStopTimer = 0.01f;  // 停止按鈕的計時器
                                        //GameMain
                IceAgeGameMain.Inst.GameStatus = IceAgeGameMain.Game_Status.SlotMoving; //設定GameStatus
                IceAgeGameMain.Inst.MiniGameName = "MainGame";  //判斷是否BonusDouble使用
                IceAgeGameMain.Inst.ReStartTimer = 0.0f;    //重置ReStart計時器
                //WinMoney
                IceAgeWinMoney.Inst.Reseat = true;
                //MoneySlot
                ulong NowMoney = IceAgeMoneySlot.Inst.NowNumber - (ulong)(IceAgeButtonControl.Inst.NumberOfBets * GameConnet.m_PMachineConfig.m_usBetMoney * GameConnet.m_PMachineConfig.m_byMaxLine);
                IceAgeMoneySlot.Inst.TargetNumber = NowMoney;  //扣除賭金
                IceAgeMoneySlot.Inst.NowChange = true;  //設定Money
                //LineAnim
                IceAgeLineAnimControl.Inst.ReSetLineAnim(); //重置圖片
                //轉動開始
                foreach (var item in IceAgeSM) item.MoveReady();   // 播放轉動前動畫        
                speed = 4000.0f;    //轉動初始速度        
                NumOfStopSlot = 0;  //重置Slot停止中的數量
                IceAgeSoundControl.Inst.PlaySound(IceAgeSoundControl.SoundName.SlotMoving, true);   //播放音效
            }
            else //顯示警告 //金錢不足
            {
                Message_Control.OpenMessage = true;
                Message_Control.MessageStatus = Message_Control.MessageStatu.Buyin_MoneyNotEnough;
            }
        }
        else
        {   //FreeGame
            GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.IceAge, (uint)ENUM_ICEAGE_PACKID_GC.C2G_Bet_ReqBet,
               GameConnet.m_oGameClient.DoSerialize<byte>(IceAgeButtonControl.Inst.NumberOfBets));
            FreeGame_Control.Inst.ChangeGameNumber((byte)(IceAgeManager.m_BetResult.m_byFreeRoundCnt - 1)); //剩餘次數
            ShowStopTimer = 0.01f;  // 停止按鈕的計時器
            //GameMain
            IceAgeGameMain.Inst.MiniGameName = "MainGame";  //判斷是否BonusDouble使用
            IceAgeGameMain.Inst.ReStartTimer = 0.0f;    //重置ReStart計時器
            //WinMoney
            IceAgeWinMoney.Inst.Reseat = true;
            //LineAnim
            IceAgeLineAnimControl.Inst.ReSetLineAnim(); //重置圖片
            //轉動開始
            foreach (var item in IceAgeSM) item.MoveReady();   // 播放轉動前動畫        
            speed = 4000.0f;    //轉動初始速度        
            NumOfStopSlot = 0;  //重置Slot停止中的數量
        }
    }
}
