using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IceAgeWinMoney : MonoBehaviour
{
    public static IceAgeWinMoney Inst;
    public bool StartPlay = false;
    public long WinMoney = 1000;
    public GameObject m_NumberParent;
    public GameObject m_NumberPrefeb;
    public UIGrid m_NumberCheck;
    float DelayTimeIndex = 0.3f;
    float NumberDelayTime = 0.1f;
    public bool PlayOver = false;
    public bool IsPlaying = false;
    public bool Reseat = false;
    public IceAgeAnaimationPlayer m_WinWord;
    bool PlayingCheck = false;
    List<GameObject> m_Number = new List<GameObject>();
    IceAgeWinAnimNumber m_FinalNumber;
    public TweenAlpha m_TwAlpha;
    public TweenPosition m_TwPos;
    public TweenScale m_TwScale;

    void Awake()
    {
        Inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (StartPlay)
        {
            m_TwPos.ResetToBeginning();
            m_TwAlpha.ResetToBeginning();
            m_TwScale.ResetToBeginning();
            StartPlay = false;
            m_WinWord.StartPlay = true;
            m_WinWord.Show = true;
            string DataNumber = WinMoney.ToString();
            foreach (var item in DataNumber)
            {
                GameObject Data = Instantiate(m_NumberPrefeb);
                Data.transform.parent = m_NumberParent.transform;
                Data.transform.localScale = new Vector3(1, 1, 1);
                IceAgeWinAnimNumber Data_cs = Data.GetComponent<IceAgeWinAnimNumber>();
                Data_cs.AnimDelay = DelayTimeIndex;
                DelayTimeIndex += NumberDelayTime;
                Data_cs.Final = int.Parse(item.ToString());
                m_Number.Add(Data);
            }
            m_FinalNumber = m_Number[m_Number.Count - 1].GetComponent<IceAgeWinAnimNumber>();
            PlayingCheck = true;
            m_NumberCheck.repositionNow = true;
        }
        if (PlayingCheck)
        {
            if (m_FinalNumber.Over && m_WinWord.OneceOver)
            {
                PlayingCheck = false;
                m_WinWord.OneceOver = false;
                m_TwPos.PlayForward();
                m_TwAlpha.PlayForward();
                m_TwScale.PlayForward();
            }
        }
        if (Reseat)
        {
            Reseat = false;
            m_WinWord.OneceOver = false;
            m_WinWord.Show = false;
            IsPlaying = false;
            PlayOver = false;
            m_WinWord.Loop = false;
            DelayTimeIndex = 0.1f;
            int Data = m_Number.Count;
            for (int i = 0; i < Data; i++)
            {
                Destroy(m_Number[0]);
                m_Number.RemoveAt(0);
            }
            m_Number.Clear();
        }
    }

    public void PlayAllOver()
    {
        PlayOver = true;
        IceAgeLineAnimControl.Inst.ReSetLineAnim(); //WinMoney完成後 重置動畫
        if (IceAgeGameMain.Inst.GameStatus == IceAgeGameMain.Game_Status.FreeGame)
        {
            IceAgeMoneySlot.Inst.TargetNumber = IceAgeManager.m_BetResult.m_ui64GameMoney;
            IceAgeMoneySlot.Inst.NowChange = true;
        }
        else if (IceAgeGameMain.Inst.GameStatus != IceAgeGameMain.Game_Status.MiniGame) //進小遊戲時 WinMoney要鎖定 所以不等於小遊戲時 才會執行
        {
            if (IceAgeGameMain.Inst.GameStatus != IceAgeGameMain.Game_Status.WaitMoneySlot)
            {   //動畫播畢  播放MoneySlot
                switch (IceAgeGameMain.Inst.GameStatus)
                {   //設定金錢數量
                    case IceAgeGameMain.Game_Status.MiniGameSuccess:
                        switch (IceAgeGameMain.Inst.MiniGameName)
                        {
                            case "DoubleUp":    //從比倍返回 比倍成功 
                            case "JP":          //JP比倍
                                IceAgeMoneySlot.Inst.TargetNumber = IceAgeManager.m_RplyDoubleResult.m_ui64GameMoney;
                                IceAgeManager.m_RplyDoubleResult = null;    //金錢特效流程跑完 讓 m_RplyDoubleResult = null 以利判斷
                                break;
                            case "ShotGame":    //從射擊返回
                            case "BreakGame":   //從敲蛋返回
                                IceAgeMoneySlot.Inst.TargetNumber = IceAgeManager.m_BonusResult.m_ui64GameMoney;
                                break;
                        }
                        break;
                    default:
                        if (MiniGameChangeScene.Inst.IsBonusDouble)
                        {   //BonusGame -> DoubleUp -> 離開 採用BonusGame的金額
                            IceAgeMoneySlot.Inst.TargetNumber = IceAgeManager.m_BonusResult.m_ui64GameMoney;
                            MiniGameChangeScene.Inst.IsBonusDouble = false; //重設參數
                        }
                        else  //MainGame -> DoubleUp -> 離開 採用MainGame的金額
                            IceAgeMoneySlot.Inst.TargetNumber = IceAgeManager.m_BetResult.m_ui64GameMoney;
                        break;
                }
                IceAgeMoneySlot.Inst.NowChange = true;
                IceAgeManager.m_RplyDoubleResult = null;    //遊戲金錢設定完後 設為null 以利下一輪判斷
                IceAgeGameMain.Inst.GameStatus = IceAgeGameMain.Game_Status.WaitMoneySlot;
                IceAgeGameMain.Inst.ReStartTimer = 0.2f;
            }
        }
        Reseat = true;
    }
}
