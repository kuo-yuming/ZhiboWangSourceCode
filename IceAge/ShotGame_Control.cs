using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ShotGame_Control : MonoBehaviour
{
    public static ShotGame_Control Inst;
    private Dictionary<byte, SymbolData> AwardData; //獎項清單
    private string[] AwardName = new string[8] { "Miss", "PEgg", "Litchi", "Grape", "Orange", "Pineapple", "Watermelon", "Dinornis" };
    private ushort[] AwardMoney = new ushort[8] { 0, 5, 20, 30, 40, 50, 60, 200 };
    public Leaf_Control[] Leaf;     //樹葉
    public UISprite Background;     //背景
    public UISprite GameTimesSprite;//遊戲次數 圖
    public GameObject Clock;        //時鐘
    private byte GameTimes = 0;     //遊戲次數
    private float GameTimer = 5.0f; //遊戲計時器
    public bool IsGetShot = false;  //是否射擊
    private float WaitTime = 3.0f;  //等待時間
    private float WaitTimer = 0.0f; //等待計時器
    public char WhichLeafWasShotIn; //哪一個樹被射中

    void Awake()
    {
        Inst = this;
    }
    // Use this for initialization
    void Start()
    {
        CloseGame();
        //建立獎項清單
        AwardData = new Dictionary<byte, SymbolData>();
        for (byte i = 1; i <= 8; i++)
        {
            SymbolData tmpData = new SymbolData();
            tmpData.Name = AwardName[i - 1];
            tmpData.Money = AwardMoney[i - 1];
            AwardData.Add(i, tmpData);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameTimer < 5.0f)
        {   //遊戲時間倒數
            GameTimer -= Time.deltaTime;
            if (GameTimer > 0)
            {   //時間還沒到 持續改變時鐘秒數
                Clock.transform.GetChild(0).GetComponent<UISprite>().spriteName = ("ClockNumber_" + (int)GameTimer).ToString();
                if (GameTimer < 0.5f) foreach (var item in Leaf) item.CloseShot();  //小於0.5秒 鎖定射擊 防止BUG
            }
            else
            {   //時間到 重置時間  並且自動射擊
                GameTimer = 5.0f;
                //設定種子 確保亂數重複率降低
                System.Random rndVal = new System.Random(Guid.NewGuid().GetHashCode());
                ushort WhoShot = (ushort)rndVal.Next(0, 5); //亂數決定射擊
                Leaf[WhoShot].OnClick();   //傳遞射擊事件
            }
        }

        if (IsGetShot && IceAgeManager.BounsGameDateGet)
        {
            WaitTimer = 0.01f;  //等待數秒後 繼續下一次射擊 或回到 MainGame
            foreach (var item in Leaf) item.GetShot(WhichLeafWasShotIn, AwardData[IceAgeManager.m_BonusResult.m_byAwardID].Name, AwardData[IceAgeManager.m_BonusResult.m_byAwardID].Money);
            GameTimes--;   //遊戲次數
            GameTimesSprite.spriteName = "ChooseTimesNum_" + GameTimes;
            if (GameTimes == 0) MiniGameChangeScene.Inst.MiniGameEnd = true;    //設定狀態
            IceAgeManager.BounsGameDateGet = false;
        }
        
        if (WaitTimer != 0.0f)
        {
            WaitTimer += Time.deltaTime;
            if (WaitTimer > WaitTime)
            {
                if (MiniGameChangeScene.Inst.MiniGameEnd)
                {
                    BackMainGame();     //回到MainGame
                    WaitTimer = 0.0f;
                    IsGetShot = false;
                }
                else
                {                    
                    SetGameBackground();//設定靜態場景
                    BackgroundPlay();   //播放
                    IsGetShot = false;
                    WaitTimer = 0.0f;
                }
            }
        }
    }

    void BackMainGame()
    {   //回到MainGame
        MiniGameChangeScene.Inst.GotoChangeScene("MainGame");
    }

    public void CloseGame()
    {
        foreach (var item in Leaf) item.Close();    //各樹葉關閉
        Background.enabled = false; //背景
        GameTimesSprite.enabled = false;  //遊戲次數
        //時鐘
        Clock.GetComponent<UISpriteAnimation>().enabled = false;
        Clock.SetActive(false);
        GameTimes = 0;
    }

    public void SetGameBackground()
    {
        foreach (var item in Leaf) item.Show(); //各個樹葉顯示
        Background.enabled = true;  //背景
        if (GameTimes == 0) GameTimes = IceAgeManager.BonusGameTimes;   //遊戲次數
        GameTimesSprite.spriteName = "ChooseTimesNum_" + GameTimes;    
        GameTimesSprite.enabled = true;
        //時鐘
        Clock.SetActive(true);
        Clock.GetComponent<UISprite>().spriteName = "MiniGameClock_01";
        Clock.GetComponent<UISpriteAnimation>().enabled = false;
        Clock.transform.GetChild(0).GetComponent<UISprite>().spriteName = "ClockNumber_5";
    }

    public void BackgroundPlay()
    {
        foreach (var item in Leaf) item.Play();
        //時鐘
        Clock.GetComponent<UISpriteAnimation>().enabled = true;
        GameTimer -= 0.01f;
    }

    public void GetShot()
    {
        Clock.transform.GetChild(0).GetComponent<UISprite>().spriteName = "ClockNumber_0";
        Clock.GetComponent<UISprite>().spriteName = "MiniGameClock_01";
        Clock.GetComponent<UISpriteAnimation>().enabled = false;
        IsGetShot = true;
        GameTimer = 5.0f;  //重置計時器        
        IceAgeSoundControl.Inst.PlaySound(IceAgeSoundControl.SoundName.Shot, false);    //播放音效
    }
}