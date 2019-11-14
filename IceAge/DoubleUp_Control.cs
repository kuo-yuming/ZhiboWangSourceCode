using UnityEngine;
using System.Collections;
using GameCore.Manager.IceAge;
using GameCore;

public class DoubleUp_Control : MonoBehaviour
{
    public static DoubleUp_Control Inst;
    public GameObject BackgroundAnim;
    public GameObject ManAnim;
    public GameObject Clock;
    public GameObject RightArrow;
    public GameObject LeftArrow;
    public GameObject Word;
    public float GameTimer = 10.0f;
    private bool IsClickJumpButton = false;
    private bool ManIsRightJump;
    public float WaitTime = 2.0f;
    public float WaitTimer = 0.0f;

    void Awake()
    {
        Inst = this;
    }
    
    // Update is called once per frame
    void Update()
    {   
        if (GameTimer < 10.0f)
        {   //遊戲時間倒數
            GameTimer -= Time.deltaTime;
            if (GameTimer > 0)
            {   //時間還沒到 持續改變時鐘秒數
                Clock.transform.GetChild(0).GetComponent<UISprite>().spriteName = ("ClockNumber_" + (int)GameTimer).ToString();
                if (GameTimer < 0.5f)
                {   //小於0.5秒 鎖定離開比倍按鈕 箭頭 防止BUG
                    IceAgeButtonControl.Inst.SetDoubleUpGameButtonShow();
                    //左右箭頭
                    RightArrow.SetActive(false);
                    LeftArrow.SetActive(false);
                }
            }
            else
            {   //時間到 重置時間  並且等同按下離開比倍
                GameTimer = 10.0f;
                IceAgeGameMain.Inst.OnLeaveDoubleUp();
            }
        }
        //決定跳躍方向 且 收到比倍結果
        if (IsClickJumpButton && IceAgeManager.GetDoubleRestle)
        {   //跳右邊
            if (ManIsRightJump)
            {   //成功
                if (IceAgeManager.m_RplyDoubleResult.m_uiScore != 0) GetResult(1);
                else GetResult(3);  //失敗
            }
            else //跳左邊
            {   //成功
                if (IceAgeManager.m_RplyDoubleResult.m_uiScore != 0) GetResult(2);
                else GetResult(4);  //失敗
            }
            //重置變數
            IsClickJumpButton = false;
            IceAgeManager.GetDoubleRestle = false;
        }

        if (WaitTimer != 0.0f)
        {
            WaitTimer += Time.deltaTime;
            if (WaitTimer > WaitTime)
            {
                if (MiniGameChangeScene.Inst.MiniGameEnd)
                {
                    BackMainGame();
                    WaitTimer = 0.0f;
                }
                else
                {
                    SetGameBackground();
                    BackgroundPlay();
                    WaitTimer = 0.0f;
                }
            }
        }
    }

    public void CloseGame()
    {   //背景+恐龍
        BackgroundAnim.GetComponent<IceAgeAnimAdapter>().m_AnimControl.SetInteger("Status", 0);
        BackgroundAnim.GetComponent<IceAgeAnimAdapter>().TextureShow.enabled = false;
        BackgroundAnim.GetComponent<IceAgeAnimAdapter>().SpriteShow.enabled = false;
        //人
        ManAnim.transform.localPosition = new Vector3(0, -88, 0);
        ManAnim.GetComponent<IceAgeAnimAdapter>().m_AnimControl.SetInteger("Move", 0);
        ManAnim.GetComponent<IceAgeAnimAdapter>().TextureShow.enabled = false;
        ManAnim.GetComponent<IceAgeAnimAdapter>().SpriteShow.enabled = false;
        //時鐘
        Clock.GetComponent<UISpriteAnimation>().enabled = false;
        Clock.SetActive(false);
        //左右箭頭
        RightArrow.GetComponent<UISprite>().spriteName = "right_arrow_00";
        RightArrow.GetComponent<UISpriteAnimation>().enabled = false;
        RightArrow.SetActive(false);
        LeftArrow.GetComponent<UISprite>().spriteName = "left_arrow_00";
        LeftArrow.GetComponent<UISpriteAnimation>().enabled = false;
        LeftArrow.SetActive(false);
        //文字
        Word.transform.GetComponent<UISpriteAnimation>().enabled = false;
        Word.SetActive(false);
    }

    public void SetGameBackground()
    {
        //背景+恐龍 顯示 但是不動
        BackgroundAnim.GetComponent<IceAgeAnimAdapter>().m_AnimControl.SetInteger("Status", 1);
        BackgroundAnim.GetComponent<IceAgeAnimAdapter>().m_AnimControl.speed = 0.0f;
        BackgroundAnim.GetComponent<IceAgeAnimAdapter>().TextureShow.enabled = true;
        BackgroundAnim.GetComponent<IceAgeAnimAdapter>().SpriteShow.enabled = true;
        //人
        ManAnim.GetComponent<IceAgeAnimAdapter>().m_AnimControl.SetInteger("Move", 0);
        ManAnim.GetComponent<IceAgeAnimAdapter>().TextureShow.enabled = true;
        ManAnim.GetComponent<IceAgeAnimAdapter>().SpriteShow.enabled = true;
        //時鐘
        Clock.SetActive(true);
        Clock.GetComponent<UISprite>().spriteName = "MiniGameClock_01";
        Clock.GetComponent<UISpriteAnimation>().enabled = false;
        Clock.transform.GetChild(0).GetComponent<UISprite>().spriteName = "ClockNumber_9";
        //左右箭頭 顯示 但不動
        RightArrow.SetActive(true);
        RightArrow.GetComponent<UISprite>().spriteName = "right_arrow_00";
        RightArrow.GetComponent<UISpriteAnimation>().enabled = false;
        LeftArrow.SetActive(true);
        LeftArrow.GetComponent<UISprite>().spriteName = "left_arrow_00";
        LeftArrow.GetComponent<UISpriteAnimation>().enabled = false;
        //文字
        Word.transform.GetComponent<UISpriteAnimation>().enabled = false;
        Word.SetActive(false);
    }

    public void BackgroundPlay()
    {   //開放 離開比倍按鈕
        IceAgeButtonControl.Inst.SetDoubleUpGameButton();
        //背景播放
        BackgroundAnim.GetComponent<IceAgeAnimAdapter>().m_AnimControl.SetInteger("Status", 1);
        BackgroundAnim.GetComponent<IceAgeAnimAdapter>().m_AnimControl.speed = 1.0f;
        //人
        ManAnim.transform.localPosition = new Vector3(0, -88, 0);
        ManAnim.GetComponent<IceAgeAnimAdapter>().m_AnimControl.SetInteger("Move", 1);
        //時鐘
        Clock.GetComponent<UISpriteAnimation>().enabled = true;
        GameTimer -= 0.01f;        
        //箭頭播放
        RightArrow.SetActive(true);
        RightArrow.GetComponent<UISprite>().spriteName = "right_arrow_00";
        RightArrow.GetComponent<UISpriteAnimation>().enabled = true;
        LeftArrow.SetActive(true);
        LeftArrow.GetComponent<UISprite>().spriteName = "left_arrow_00";
        LeftArrow.GetComponent<UISpriteAnimation>().enabled = true;
        //播放音效
        IceAgeSoundControl.Inst.PlaySound(IceAgeSoundControl.SoundName.DoubleUpBGM, true);   
    }

    void ClickRightArrow()
    {   //傳送要求
        GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.IceAge, (uint)ENUM_ICEAGE_PACKID_GC.C2G_Bet_ReqDouble, null);
        //紀錄方向
        IsClickJumpButton = true;
        ManIsRightJump = true;
        //關閉箭頭
        RightArrow.SetActive(false);
        LeftArrow.SetActive(false);
        //固定時鐘
        Clock.transform.GetChild(0).GetComponent<UISprite>().spriteName = "ClockNumber_0";
        Clock.GetComponent<UISprite>().spriteName = "MiniGameClock_01";
        //鎖定離開比倍按鈕
        IceAgeButtonControl.Inst.SetDoubleUpGameButtonShow();
        //停止音效
        IceAgeSoundControl.Inst.IceAgeSound.Stop(); 
    }

    void ClickLeftArrow()
    {   //傳送要求
        GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.IceAge, (uint)ENUM_ICEAGE_PACKID_GC.C2G_Bet_ReqDouble, null);
        //紀錄方向
        IsClickJumpButton = true;
        ManIsRightJump = false;
        //關閉箭頭
        RightArrow.SetActive(false);
        LeftArrow.SetActive(false);
        //固定時鐘
        Clock.transform.GetChild(0).GetComponent<UISprite>().spriteName = "ClockNumber_0";
        Clock.GetComponent<UISprite>().spriteName = "MiniGameClock_01";
        //鎖定離開比倍按鈕
        IceAgeButtonControl.Inst.SetDoubleUpGameButtonShow();
        //停止音效
        IceAgeSoundControl.Inst.IceAgeSound.Stop();
    }

    void GetResult(byte JumpResult)
    {
        GameTimer = 10.0f;   //按下方向後 時間停止
        switch (JumpResult)
        {
            case 1: //跳右邊 成功
                BackgroundAnim.GetComponent<IceAgeAnimAdapter>().m_AnimControl.SetInteger("Status", 3); //背景 咬左邊              
                ManAnim.GetComponent<IceAgeAnimAdapter>().m_AnimControl.SetInteger("Move", 2);  //人 跳右邊 成功
                break;
            case 2: //跳左邊 成功
                BackgroundAnim.GetComponent<IceAgeAnimAdapter>().m_AnimControl.SetInteger("Status", 2); //背景 咬右邊
                ManAnim.GetComponent<IceAgeAnimAdapter>().m_AnimControl.SetInteger("Move", 3);  //人 跳左邊 成功
                break;
            case 3: //跳右邊 失敗
                BackgroundAnim.GetComponent<IceAgeAnimAdapter>().m_AnimControl.SetInteger("Status", 2); //背景 咬右邊
                ManAnim.GetComponent<IceAgeAnimAdapter>().m_AnimControl.SetInteger("Move", 4);  //人 跳右邊 失敗
                break;
            case 4: //跳左邊 失敗
                BackgroundAnim.GetComponent<IceAgeAnimAdapter>().m_AnimControl.SetInteger("Status", 3); //背景 咬左邊
                ManAnim.GetComponent<IceAgeAnimAdapter>().m_AnimControl.SetInteger("Move", 5);  //人 跳左邊 失敗
                break;
            default:
                break;
        }        
    }

    void BackMainGame()
    {   //回到MainGame
        MiniGameChangeScene.Inst.GotoChangeScene("MainGame");
    }

    public void ChangeAnimLocation(int No)
    {
        if (No == 1)
            ManAnim.transform.localPosition = new Vector3(200, 40, 0);
        else if (No == 2)
            ManAnim.transform.localPosition = new Vector3(-260, 40, 0);
    }

    public void ShowWordSuccess()   //成功 繼續遊戲
    {   //成功  繼續遊戲
        WaitTimer = 0.01f;
        //設定文字
        Word.SetActive(true);
        Word.transform.GetComponent<UISprite>().spriteName = "DoubleUp_Success_00";
        Word.transform.GetComponent<UISpriteAnimation>().namePrefix = "DoubleUp_Success_";
        Word.transform.GetComponent<UISpriteAnimation>().ResetToBeginning();
        Word.transform.GetComponent<UISpriteAnimation>().enabled = true;
        //播放音效
        IceAgeSoundControl.Inst.PlaySound(IceAgeSoundControl.SoundName.DoubleUpSuccess, false);
    }

    public void ShowWordFail()  //失敗  離開遊戲
    {   //設定變數
        MiniGameChangeScene.Inst.MiniGameEnd = true;
        //等待數秒後 回到MainGame
        WaitTimer = 0.01f;
        //設定文字
        Word.SetActive(true);
        Word.transform.GetComponent<UISprite>().spriteName = "DoubleUp_Fail_00";
        Word.transform.GetComponent<UISpriteAnimation>().namePrefix = "DoubleUp_Fail_";
        Word.transform.GetComponent<UISpriteAnimation>().ResetToBeginning();
        Word.transform.GetComponent<UISpriteAnimation>().enabled = true;
        //播放音效
        IceAgeSoundControl.Inst.PlaySound(IceAgeSoundControl.SoundName.DoubleUpFail, false);
    }
}