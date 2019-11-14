using UnityEngine;
using System.Collections;

public class SymbolData
{
    public string Name;
    public ushort Money;
}

public class MiniGameChangeScene : MonoBehaviour
{
    public static MiniGameChangeScene Inst;
    public ChangeSceneStep NowStep = ChangeSceneStep.Idle;
    IceAgeAnimAdapter MyAdapter;

    public Vector2 Size;
    public string NowGame = "MainGame";
    public bool MiniGameEnd = true;
    public bool IsBonusDouble = false;

    public enum ChangeSceneStep
    {
        Idle,
        ChangeStart,
        ChangeOver,
    }
    void Awake()
    {
        Inst = this;
    }
    // Use this for initialization
    void Start()
    {
        MyAdapter = GetComponent<IceAgeAnimAdapter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Size.x != 0 && Size.y != 0)
        {
            MyAdapter.TextureShow.width = (int)Size.x;
            MyAdapter.TextureShow.height = (int)Size.y;
        }
    }

    void SetScene()
    {
        if (NowStep == ChangeSceneStep.ChangeStart)
        {   //石頭全落下後  切換場景 但停用所有動畫
            switch (NowGame)
            {
                case "MainGame":
                    NowStep = ChangeSceneStep.ChangeOver;
                    CloseAllMiniGame();
                    break;
                case "DoubleUp":
                    NowStep = ChangeSceneStep.ChangeOver;
                    DoubleUp_Control.Inst.SetGameBackground();
                    break;
                case "ShotGame":
                    NowStep = ChangeSceneStep.ChangeOver;
                    ShotGame_Control.Inst.SetGameBackground();
                    IceAgeGameMain.Inst.BonusGameText.GetComponent<UISprite>().enabled = false;
                    break;
                case "BreakGame":
                    NowStep = ChangeSceneStep.ChangeOver;
                    BreakGame_Control.Inst.SetGameBackground();
                    IceAgeGameMain.Inst.BonusGameText.GetComponent<UISprite>().enabled = false;
                    break;
                default:
                    Debug.LogError("沒有對應的下一步: " + NowGame);
                    break;
            }
        }
    }

    void SetNext()
    {
        if (NowStep == ChangeSceneStep.ChangeOver)
        {
            ResetChangeScene(); //重置過場
            IceAgeButtonControl.Inst.SetMoneyDoubleButtonShow();
            switch (NowGame)
            {
                case "MainGame":    //返回 Main
                    NowStep = ChangeSceneStep.ChangeOver;
                    switch(IceAgeGameMain.Inst.MiniGameName)
                    {
                        case "DoubleUp":    //從比倍返回
                        case "JP":  //JP比倍
                            if (IceAgeGameMain.Inst.MiniGameName == "JP")
                            {   //JP比倍 不管成敗或離開 左上JP都會歸零 剩下按照比倍規則去判斷
                                IceAge_JP_Control.Inst.TargetNumber = 0;
                                IceAge_JP_Control.Inst.NowChange = true;
                            }
                            if (IceAgeManager.m_RplyDoubleResult != null)   //進入比倍後直接離開比倍 m_RplyDoubleResult會null
                            {
                                if (IceAgeManager.m_RplyDoubleResult.m_uiScore != 0)
                                {   //成功 播放WinMoney
                                    IceAgeGameMain.Inst.GameStatus = IceAgeGameMain.Game_Status.MiniGameSuccess;
                                    IceAgeWinMoney.Inst.WinMoney = IceAgeManager.m_RplyDoubleResult.m_uiScore;
                                    IceAgeWinMoney.Inst.StartPlay = true;   //金錢動畫
                                }
                                else
                                {   //失敗
                                    if (IceAgeButtonControl.Inst.AutoTimes == 0)    // 次數 = 0            
                                    {   //調整遊戲狀態與按鈕
                                        IceAgeGameMain.Inst.GameStatus = IceAgeGameMain.Game_Status.Idle;
                                        IceAgeButtonControl.Inst.SetStopButton();
                                    }
                                    else
                                    {   //若還有自動次數 準備下一盤
                                        IceAgeGameMain.Inst.GameStatus = IceAgeGameMain.Game_Status.GetScore;
                                        IceAgeGameMain.Inst.ReStartTimer = 0.01f;
                                    }
                                }
                            }
                            else if (!IsBonusDouble && IceAgeManager.m_BetResult.m_uiScore != 0)  //進入比倍後直接離開比倍 採用原本得分計算
                            {   //成功 播放WinMoney
                                IceAgeGameMain.Inst.GameStatus = IceAgeGameMain.Game_Status.SlotMoving;
                                IceAgeGameMain.Inst.ReStartTimer = 0.0f;
                                IceAgeWinMoney.Inst.WinMoney = IceAgeManager.m_BetResult.m_uiScore;
                                IceAgeWinMoney.Inst.StartPlay = true;   //金錢動畫
                            }
                            else if (IsBonusDouble && IceAgeManager.m_BonusResult.m_uiScoreSum != 0)    //BonusGame成功後 
                            {                                                                           //進入比倍後離開 採用BonusGame的得分計算
                                //成功 播放WinMoney
                                IceAgeGameMain.Inst.GameStatus = IceAgeGameMain.Game_Status.SlotMoving;
                                IceAgeGameMain.Inst.ReStartTimer = 0.0f;
                                IceAgeWinMoney.Inst.WinMoney = IceAgeManager.m_BonusResult.m_uiScoreSum;
                                IceAgeWinMoney.Inst.StartPlay = true;   //金錢動畫
                            }
                            break;
                        case "ShotGame":    //從射擊返回
                        case "BreakGame":   //從敲蛋返回
                            if (IceAgeManager.m_BonusResult.m_uiScoreSum != 0)
                            {
                                IceAgeGameMain.Inst.GameStatus = IceAgeGameMain.Game_Status.MiniGameSuccess;
                                IceAgeWinMoney.Inst.m_WinWord.Loop = false;  //WinMoney 特效停在畫面上Loop
                                IceAgeWinMoney.Inst.WinMoney = IceAgeManager.m_BonusResult.m_uiScoreSum;
                                IceAgeWinMoney.Inst.StartPlay = true;   //金錢動畫
                            }
                            else
                            {   //三次結果都是MISS
                                if (IceAgeButtonControl.Inst.AutoTimes == 0)    // 自動次數 = 0            
                                {   //調整遊戲狀態與按鈕
                                    IceAgeGameMain.Inst.GameStatus = IceAgeGameMain.Game_Status.Idle;
                                    IceAgeButtonControl.Inst.SetStopButton();
                                }
                                else
                                {   //若還有自動次數 準備下一盤
                                    IceAgeGameMain.Inst.GameStatus = IceAgeGameMain.Game_Status.GetScore;
                                    IceAgeGameMain.Inst.ReStartTimer = 0.01f;
                                }
                            }
                            break;
                    }
                    break;
                case "DoubleUp":
                    DoubleUp_Control.Inst.BackgroundPlay();                    
                    break;
                case "ShotGame":
                    ShotGame_Control.Inst.BackgroundPlay();
                    break;
                case "BreakGame":
                    BreakGame_Control.Inst.BackgroundPlay();
                    break;
                default:
                    Debug.LogError("沒有對應的下一步: " + NowGame);
                    break;
            }
        }
    }

    public void GotoChangeScene(string GameName)
    {
        NowGame = GameName;
        NowStep = ChangeSceneStep.ChangeStart;
        MyAdapter.m_AnimControl.SetBool("AnimStart", true);
        MyAdapter.TextureShow.enabled = true;
        MyAdapter.SpriteShow.enabled = true;
        IceAgeSoundControl.Inst.PlaySound(IceAgeSoundControl.SoundName.ChangeScene, false);   //播放音效
    }

    void ResetChangeScene()
    {
        NowStep = ChangeSceneStep.Idle;
        MyAdapter.m_AnimControl.SetBool("AnimStart", false);
        MyAdapter.TextureShow.enabled = false;
        MyAdapter.SpriteShow.enabled = false;
    }

    void CloseAllMiniGame()
    {
        DoubleUp_Control.Inst.CloseGame();
        ShotGame_Control.Inst.CloseGame();
        BreakGame_Control.Inst.CloseGame();
    }
}