using UnityEngine;
using System.Collections;

public class SicBoCircularScreen : MonoBehaviour
{
    private CircularScreenStatus NowStatus = CircularScreenStatus.Idle; //顯示器目前狀態
    public UISprite CenterFrameSprite;  //中心輪盤外框
    public UIPanel RouletteScreenPanel; //大輪盤區Panel
    public GameObject MaskPanel;        //遮罩
    public UISprite CenterRouletteSprite;   //中心輪盤
    public TweenPosition SelfPosition;  //TweenPosition
    public UISprite TimerNumberTens;    //計時器 十位數
    public UISprite TimerNumberDigits;  //計時器 十位數
    public bool[] TimerBool;            //是否播放過10, 5 ,3, 2, 1的音效
    public UISprite MessageText;        //訊息文字
    public AudioSource MessageSound;    //訊息文字音效
    public AudioSource[] MessageClip;   //訊息文字音效
    public UISprite EffectText;             //特效文字
    public UISpriteAnimation EffectTextAnim;//特效文字動畫
    public UISprite[] EffectTextData;   //特效文字Atlas //0：出現雙骰 //1：出現紅雙骰
    public AudioSource[] EffectTextClip;//特效文字音效  //0：出現雙骰 //1：出現紅雙骰
    private float CountdownTimer = 0.0f;    //倒數計時器
    private float WaitTimer = 0.0f;     //等待計時器
    private bool RouletteScreenPanelToShow = false; //大輪盤區顯示
    public AudioSource RouletteTurnSound;   //輪盤轉動音效

    public enum CircularScreenStatus
    {
        Idle = 0,           //閒置
        Frist = 1,          //首輪
        NewRound = 10,      //新局開始
        PleaseBet = 11,     //請下注
        BetCountdown = 12,  //押注倒數
        StopBet = 13,       // 停止押注
    }

    // Update is called once per frame
    void Update()
    {
        if (CountdownTimer > 0)
        {   //有接收到倒數時間 持續倒數
            CountdownTimer -= Time.deltaTime;
            if (CountdownTimer < 0) CountdownTimer = 0.0f;
        }

        switch (NowStatus)
        {
            case CircularScreenStatus.Frist:    //首輪
                if (CountdownTimer > 0.5)
                {   //時間 > 0.5 正常顯示
                    TimerNumberTens.spriteName = "numberA_" + (byte)CountdownTimer / 10;
                    TimerNumberDigits.spriteName = "numberA_" + (byte)CountdownTimer % 10;
                    //播放音效
                    if (CountdownTimer < 11 && !TimerBool[0])
                    {
                        TimerBool[0] = true;
                        MessageSound.clip = MessageClip[2].clip;
                        MessageSound.Play();                        
                    }
                    else if (CountdownTimer < 6 && !TimerBool[1])
                    {
                        TimerBool[1] = true;
                        MessageSound.clip = MessageClip[3].clip;
                        MessageSound.Play();
                    }
                    else if (CountdownTimer < 4 && !TimerBool[2])
                    {
                        TimerBool[2] = true;
                        MessageSound.clip = MessageClip[4].clip;
                        MessageSound.Play();
                    }
                    else if (CountdownTimer < 3 && !TimerBool[3])
                    {
                        TimerBool[3] = true;
                        MessageSound.clip = MessageClip[5].clip;
                        MessageSound.Play();
                    }
                    else if (CountdownTimer < 2 && !TimerBool[4])
                    {
                        TimerBool[4] = true;
                        MessageSound.clip = MessageClip[6].clip;
                        MessageSound.Play();
                    }
                }
                else if (CountdownTimer > 0.0f)
                {   //時間 > 0 但 < 0.5 關閉押注
                    SicBoGameMain.Inst.BetAreaControl.CloseBet();
                }
                else //時間 < 0 隱藏時間
                {
                    NowStatus = CircularScreenStatus.StopBet;   //變更狀態 停止押注
                    TimerNumberTens.enabled = false;
                    TimerNumberDigits.enabled = false;
                    MessageText.enabled = true; //顯示停止押注
                    MessageText.spriteName = "text_stop";
                    MessageSound.clip = MessageClip[1].clip;
                    MessageSound.Play();    //播放音效
                }
                break;
            case CircularScreenStatus.NewRound: //新局開始
                if (WaitTimer > 0)
                {
                    WaitTimer -= Time.deltaTime;
                    if (WaitTimer < 0)
                    {
                        MessageText.spriteName = "text_bet";    //顯示請下注
                        MessageSound.clip = MessageClip[0].clip;
                        MessageSound.Play();    //播放音效
                        WaitTimer = 0.5f;   //0.5秒後 顯示時間倒數
                        NowStatus = CircularScreenStatus.PleaseBet;
                    }
                }
                break;
            case CircularScreenStatus.PleaseBet:    //請下注
                if (WaitTimer > 0)
                {
                    WaitTimer -= Time.deltaTime;
                    if (WaitTimer < 0)
                    {
                        SicBoGameMain.Inst.BetAreaControl.OpenBet();   //開放投注
                        if (SicBoGameMain.Inst.BetAreaControl.AutoBetArea.Count != 0 && SicBoGameMain.Inst.ButtonControl.AutoTimes > 0)
                        {   //如果自動押注List有資料 且 自動次數 > 0
                            SicBoGameMain.Inst.BetAreaControl.DoAutoBet();  //進行自動押注
                            if (SicBoGameMain.Inst.ButtonControl.AutoTimes < 1000)
                                SicBoGameMain.Inst.ButtonControl.AutoTimes--;   //自動自數-1
                        }
                        MessageText.enabled = false;    //隱藏訊息
                        TimerNumberTens.enabled = true; //顯示時間倒數
                        TimerNumberDigits.enabled = true;
                        TimerNumberTens.spriteName = "numberA_" + (byte)CountdownTimer / 10;
                        TimerNumberDigits.spriteName = "numberA_" + (byte)CountdownTimer % 10;
                        TimerBool = new bool[5] { false, false, false, false, false };    //是否播放過10, 5 ,3, 2, 1的音效
                        WaitTimer = 0.0f;   //關閉等待計時器
                        NowStatus = CircularScreenStatus.BetCountdown;
                    }
                }
                break;
            case CircularScreenStatus.BetCountdown: //押注倒數
                if (CountdownTimer > 0.5)
                {   //時間 > 0.5 正常顯示
                    TimerNumberTens.spriteName = "numberA_" + (byte)CountdownTimer / 10;
                    TimerNumberDigits.spriteName = "numberA_" + (byte)CountdownTimer % 10;//播放音效
                    if (CountdownTimer < 11 && !TimerBool[0])
                    {
                        TimerBool[0] = true;
                        MessageSound.clip = MessageClip[2].clip;
                        MessageSound.Play();
                    }
                    else if (CountdownTimer < 6 && !TimerBool[1])
                    {
                        TimerBool[1] = true;
                        MessageSound.clip = MessageClip[3].clip;
                        MessageSound.Play();
                    }
                    else if (CountdownTimer < 4 && !TimerBool[2])
                    {
                        TimerBool[2] = true;
                        MessageSound.clip = MessageClip[4].clip;
                        MessageSound.Play();
                    }
                    else if (CountdownTimer < 3 && !TimerBool[3])
                    {
                        TimerBool[3] = true;
                        MessageSound.clip = MessageClip[5].clip;
                        MessageSound.Play();
                    }
                    else if (CountdownTimer < 2 && !TimerBool[4])
                    {
                        TimerBool[4] = true;
                        MessageSound.clip = MessageClip[6].clip;
                        MessageSound.Play();
                    }
                }
                else if (CountdownTimer > 0.0f)
                {   //時間 > 0 但 < 0.5 關閉押注
                    SicBoGameMain.Inst.BetAreaControl.CloseBet();
                }
                else //時間 < 0 隱藏時間
                {
                    TimerNumberTens.enabled = false;
                    TimerNumberDigits.enabled = false;
                    MessageText.enabled = true; //顯示停止押注
                    MessageText.spriteName = "text_stop";
                    MessageSound.clip = MessageClip[1].clip;
                    MessageSound.Play();    //播放音效
                    NowStatus = CircularScreenStatus.StopBet;   //變更狀態 停止押注
                }
                break;
            case CircularScreenStatus.StopBet:  //停止押注
                break;
        }

        if (RouletteScreenPanelToShow)
        {   //大輪盤區顯示
            if (RouletteScreenPanel.alpha < 1.0f)
            {   //大輪盤區Alpha < 1
                float ChangeAtlasValue = Time.deltaTime / 0.5f; //計算變量
                RouletteScreenPanel.alpha += ChangeAtlasValue;  //增加大輪盤區Alpha //減少中心輪盤外框Alpha
            }
            else
            {   //切換完成後 設定Alpha
                RouletteScreenPanelToShow = false;  //關閉變數
                RouletteScreenPanel.alpha = 1.0f;   //設定PanelAlpha
                SicBoGameMain.Inst.NowStatus = SicBoGameMain.SicBoGameStatus.ShowAwards;//設定狀態 開獎中
                SicBoGameMain.Inst.GetStatusUpdate = true;  //切換狀態
                MessageText.spriteName = "text_lottery";    //設定文字訊息 開獎中
                MessageSound.clip = MessageClip[10].clip;
                MessageSound.Play();    //播放音效
            }
        }

        if (EffectText.enabled)
        {   //特效文字播放完畢 隱藏特效文字
            if (EffectText.spriteName == "doubleEffect_45")
            {   //根據特效文字Atlas 選擇訊息文字
                if (EffectText.atlas == EffectTextData[0].atlas)
                {
                    MessageText.spriteName = "text_double";
                    MessageSound.clip = MessageClip[7].clip;
                }
                else if (EffectText.atlas == EffectTextData[1].atlas)
                {
                    MessageText.spriteName = "text_redDouble";
                    MessageSound.clip = MessageClip[8].clip;
                }
                EffectText.enabled = false; //關閉特效文字
                MessageText.enabled = true; //開啟訊息文字
                MessageSound.Play();    //播放音效
            }
        }
    }

    public void InitShow()
    {   //初始化顯示
        TimerNumberTens.enabled = false;
        TimerNumberDigits.enabled = false;
        MessageText.enabled = false;
        RouletteScreenPanel.alpha = 0.0f;
        CenterRouletteSprite.enabled = false;
        MaskPanel.SetActive(false);
    }

    public void FristBetCountdownSetting(float CDTime)
    {   //首輪 直接倒數
        NowStatus = CircularScreenStatus.Frist;
        CountdownTimer = CDTime;    //顯示倒數
        TimerNumberTens.enabled = true;
        TimerNumberDigits.enabled = true;
        TimerBool = new bool[5] { false, false, false, false, false };    //是否播放過10, 5 ,3, 2, 1的音效
    }

    public void BetCountdownSetting(float CDTime)
    {   //新局開始
        NowStatus = CircularScreenStatus.NewRound;
        CountdownTimer = CDTime;
        MessageText.enabled = true; //顯示新局開始
        MessageText.spriteName = "text_start";
        MessageSound.clip = MessageClip[9].clip;
        MessageSound.Play();    //播放音效
        WaitTimer = 1.5f;   //0.5秒後 顯示請下注
    }
    public void MoveCircularScreen()
    {   //移動到顯示號碼模式
        //時間倒數隱藏 顯示文字
        TimerNumberTens.enabled = false;
        TimerNumberDigits.enabled = false;
        MessageText.enabled = true; //顯示停止押注
        MaskPanel.SetActive(true);
        SelfPosition.ResetToBeginning();    //TweenPos 初始化
        SelfPosition.enabled = true;        //播放TweenPos
    }
    public void MoveCircularScreenEnd()
    {   //移動到目的地後 顯示骰子顯示器
        CenterRouletteSprite.enabled = false;
        //顯示大轉盤區 同時隱藏一般顯示區
        RouletteScreenPanelToShow = true;
        //播放輪盤音效
        RouletteTurnSound.Play();
    }
    public void ShowEffectText(byte EffectTextID)
    {   //顯示特效文字
        SicBoGameMain.Inst.RouletteTurnControl.EffectHasPlayer = true;
        EffectText.enabled = true;  //顯示特效文字
        MessageText.enabled = false;    //關閉訊息文字
        EffectText.atlas = EffectTextData[(int)(EffectTextID - 1)].atlas;   //指定Atlas
        EffectText.spriteName = "doubleEffect_01";  //特效文字
        EffectText.MakePixelPerfect();  //適應圖片大小
        EffectTextAnim.ResetToBeginning();  //特效文字動畫
        EffectTextAnim.enabled = true;
        MessageSound.clip = EffectTextClip[(int)(EffectTextID - 1)].clip;   //指定音效
        MessageSound.Play();    //播放音效
    }
}