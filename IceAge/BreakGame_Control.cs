using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BreakGame_Control : MonoBehaviour
{
    public static BreakGame_Control Inst;
    private Dictionary<byte, SymbolData> AwardData; //獎項清單
    private string[] AwardName = new string[11] { "Miss", "Meat", "PEgg", "Litchi", "Grape", "Orange", "Pineapple", "Watermelon", "Dinornis", "Tiger", "Mammoth" };
    private ushort[] AwardMoney = new ushort[11] { 0, 5, 10, 20, 30, 40, 50, 60, 90, 180, 300 };
    public Transform Eggs;          //蛋的父物件
    private Egg_Control[] Egg;      //蛋的陣列
    public UISprite Background;     //背景
    public UISprite GameTimesSprite;//遊戲次數 圖
    public GameObject Clock;        //時鐘
    private List<byte> NotBrokenEgg = new List<byte>(); //還沒破掉的蛋
    private byte GameTimes = 0;     //遊戲次數 
    private float GameTimer = 5.0f; //遊戲計時器
    public bool IsEggBreak = false; //是否敲蛋
    private float WaitTime = 3.0f;  //等待時間
    public float WaitTimer = 0.0f;  //等待計時器
    public byte WhichEggIsBreak;    //哪一個蛋被敲破了

    void Awake()
    {
        Inst = this;
    }
    // Use this for initialization
    void Start()
    {
        Egg = new Egg_Control[12];  //初始化
        for (byte i = 0; i < Eggs.childCount; i++)
        {   //設定物件
            Egg[i] = Eggs.GetChild(i).GetComponent<Egg_Control>();
            Egg[i].GetComponent<Egg_Control>().EggNumber = i;
        }
        CloseGame();    //隱藏物件
        //建立獎項清單
        AwardData = new Dictionary<byte, SymbolData>();
        for (byte i = 1; i <= 11; i++)
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
                if (GameTimer < 0.5f) foreach (var item in Egg) item.CloseBreak();  //小於0.5秒 鎖定敲擊 防止BUG
            }
            else
            {   //時間到 重置時間  並且自動射擊
                GameTimer = 5.0f;
                //設定種子 確保亂數重複率降低
                System.Random rndVal = new System.Random(Guid.NewGuid().GetHashCode());
                ushort WhoShot = (ushort)rndVal.Next(0, NotBrokenEgg.Count); //亂數決定敲擊
                Egg[NotBrokenEgg[WhoShot]].OnClick();   //傳遞射擊事件
            }
        }

        if (IsEggBreak && IceAgeManager.BounsGameDateGet)
        {
            WaitTimer = 0.01f;  //等待數秒後 繼續下一次射擊 或回到 MainGame
            NotBrokenEgg.Remove(WhichEggIsBreak);
            print("編號：" + IceAgeManager.m_BonusResult.m_byAwardID + ", 名稱:" + AwardData[IceAgeManager.m_BonusResult.m_byAwardID].Name + ", 金額:" + AwardData[IceAgeManager.m_BonusResult.m_byAwardID].Money);
            foreach (var item in Egg)   //執行每個蛋的動作
                item.GetBreak(WhichEggIsBreak, AwardData[IceAgeManager.m_BonusResult.m_byAwardID].Name, AwardData[IceAgeManager.m_BonusResult.m_byAwardID].Money);
            GameTimes--;   //遊戲次數
            GameTimesSprite.spriteName = "ChooseTimesNum_" + GameTimes;
            if (GameTimes == 0) MiniGameChangeScene.Inst.MiniGameEnd = true;    //設定狀態
            IsEggBreak = false;
            IceAgeManager.BounsGameDateGet = false;
        }

        if (WaitTimer != 0.0f)
        {
            WaitTimer += Time.deltaTime;
            if (WaitTimer > WaitTime)
            {
                if (MiniGameChangeScene.Inst.MiniGameEnd)
                {
                    foreach (var item in Egg) item.SelfIsBroken = false;
                    BackMainGame();     //回到MainGame
                    WaitTimer = 0.0f;
                    IsEggBreak = false;
                }
                else
                {
                    SetGameBackground();    //設定靜態場景
                    BackgroundPlay();       //播放
                    IsEggBreak = false;
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
        NotBrokenEgg.Clear();
        foreach (var item in Egg)
        {
            item.Close();    //各蛋關閉
            NotBrokenEgg.Add(item.EggNumber);   //設定未破的蛋
        }
        Background.enabled = false; //背景
        GameTimesSprite.enabled = false;  //遊戲次數
        //時鐘
        Clock.GetComponent<UISpriteAnimation>().enabled = false;
        Clock.SetActive(false);
        GameTimes = 0;
    }

    public void SetGameBackground()
    {
        foreach (var item in Egg) item.Show(); //各個樹葉顯示
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
        foreach (var item in Egg) item.Play();
        //時鐘
        Clock.GetComponent<UISpriteAnimation>().enabled = true;
        GameTimer -= 0.01f;
    }

    public void GetBreak()
    {
        Clock.transform.GetChild(0).GetComponent<UISprite>().spriteName = "ClockNumber_0";
        Clock.GetComponent<UISprite>().spriteName = "MiniGameClock_01";
        Clock.GetComponent<UISpriteAnimation>().enabled = false;
        IsEggBreak = true;
        GameTimer = 5.0f;  //重置計時器
        IceAgeSoundControl.Inst.PlaySound(IceAgeSoundControl.SoundName.Knock, false);    //播放音效
    }
}
