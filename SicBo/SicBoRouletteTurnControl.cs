using UnityEngine;
using System.Collections;

public class SicBoRouletteTurnControl : MonoBehaviour
{   //輪盤物件與參數
    public SicBoRouletteUnit[] RouletteUnit;    //輪盤陣列 輪盤 1 ~ 3
    public byte[] RouletteDiceNumber = new byte[3]; //各輪盤點數
    public float[] RouletteStopTime = new float[3]; //各輪盤停止時間
    public byte[] RouletteDiceID = new byte[3]; //各輪盤骰子ID
    public bool[] RouletteStops = new bool[3];  //各輪盤停止狀態
    public SicBoRouletteUnit Roulette4Unit; //輪盤4
    public byte Roulette4DiceNumber;//各輪盤點數
    public float Roulette4StopTime; //輪盤4停止時間
    public byte Roulette4DiceID;    //輪盤4骰子ID
    public UISprite Roulette4Sprite;//輪盤4圖片
    public float GoldLeopardNoticeTime; //黃金豹預告時間
    public AudioSource Roulette4BGM;    //四號輪盤BGM
    //旋轉與停止參數
    public float NowSpeed = 0.0f;   //目前速度
    private float MaxSpeed = 16.0f; //最高速度
    private float MinSpeed = 6.0f;  //最低速度 停止前速度
    private float Speed = 2.0f;     //漸進速度
    public bool StartShowAwsrds = false;//開始開獎
    public bool StartShowRoulette4 = false; //開始四號輪盤開獎
    public bool CheckRouletteStops = false; //確認輪盤停止狀態
    private byte DiceDoubleLevel = 0;   //雙骰等級 //0：無雙骰 //1：雙骰 //2：紅雙骰
    //切換輪盤物件與參數
    public UIPanel ShadowPanel; //輪盤陰影Panel
    public UISprite RouletteShadow; //輪盤陰影
    public UISprite ShutterSprite;  //輪盤快門
    public AudioSource ShutterSound;//輪盤快門音效
    public UISprite ChangeRouletteEffect;   //切換輪盤特效    
    private bool ChangeRoulette4;   //開啟四號輪盤
    private bool ChangeEffect;      //開啟切換輪盤特效
    private byte ShutterSpriteNumber;   //輪盤快門圖片編號
    private byte RouletteEffectNumber;  //輪盤切換特效圖片編號  
    private float ChangeShutterTimer;   //輪盤快門計時器
    private float ChangeEffectTimer;    //輪盤切換特效計時器
    public TweenRotation[] RouletteRotation;//切換效果TweenRotation
    public TweenScale[] RouletteScale;      //切換效果TweenScale
    //黃金豹預告動化
    public SicBoGoldLeopardAnim GoldLeopardAnim;    //預告動畫控制
    public bool EffectHasPlayer = false;    //特效文字是否啟動
    public bool HasGoldLeopard = false;     //是否開啟黃金豹預告動化

    void Start()
    {
        Roulette4Sprite.enabled = false;    //四號輪盤圖片關閉
        ChangeRouletteEffect.enabled = false;   //輪盤切換特效關閉        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (NowSpeed != 0)
        {   //如果目前速度不是0 代表旋轉中
            if (NowSpeed > MinSpeed)    //速度遞減
                NowSpeed -= Speed * Time.fixedDeltaTime * 1.0f;
            else //最小速度
                NowSpeed = MinSpeed;
        }

        if (StartShowAwsrds)
        {   //開始開獎
            for (int i = 0; i < 3; i++)
            {
                RouletteStopTime[i] -= Time.deltaTime;
                if (RouletteStopTime[i] < 0.0f)
                {   //輪盤停止
                    RouletteStopTime[i] = 0.0f;
                    RouletteUnit[i].StopSelf(RouletteDiceID[i]);
                }
            }
        }

        if (StartShowRoulette4)
        {
            Roulette4StopTime -= Time.deltaTime;
            //預告黃金豹
            if (HasGoldLeopard && GoldLeopardNoticeTime != 0 && Roulette4StopTime < GoldLeopardNoticeTime)
            {
                GoldLeopardNoticeTime = 0.0f;   //初始化時間                
                GoldLeopardAnim.GoldLeopardAnim.SetBool("PlayAnim", true);  //播放黃金豹動畫
            }
            if (Roulette4StopTime < 0.0f)
            {   //輪盤停止
                NowSpeed = 0.0f;    //初始化速度
                Roulette4StopTime = 0.0f;   //初始化計時器
                Roulette4Unit.StopSelf(Roulette4DiceID);//輪盤停止
                ShadowPanel.depth = 7;  //設定陰影Panel層級
                RouletteShadow.enabled = true;  //顯示輪盤陰影
                SicBoGameMain.Inst.DoSettlement();  //進行結算
                StartShowRoulette4 = false;
            }
        }

        if (CheckRouletteStops)
        {   //確認輪盤停止狀態
            CheckRouletteStops = false;
            DoCheckRouletteStops(); //執行確認
        }

        if (ChangeRoulette4)
        {   //開始切換輪盤 每0.01秒切換一次圖片
            if (ChangeShutterTimer < 0.01f)
                ChangeShutterTimer += Time.deltaTime;
            else
                DoChangeRoulette4();
        }

        if (ChangeEffect)
        {   //開始切換特效
            if (ChangeEffectTimer < 0.01f)
                ChangeEffectTimer += Time.deltaTime;
            else
                DoChangeEffect();
        }
    }

    //輪盤開始轉動
    public void RouletteTurnStart()
    {
        NowSpeed = MaxSpeed;    //速度 = 最高速度
        for (int i = 0; i < RouletteUnit.Length; i++)
            RouletteUnit[i].NowRotating = true; //1~3號輪盤開始轉動
        RouletteStops = new bool[3] { false, false, false }; //設定各輪盤停止狀態
        DiceDoubleLevel = 0;    //雙骰等級
        EffectHasPlayer = false;//特效文字沒有啟用
    }

    void DoCheckRouletteStops()
    {   //確認輪盤停止狀態
        if (RouletteStops[0] && RouletteStops[1] && RouletteStops[2])
        {   //如果所有輪盤都停止
            SicBoGameMain.Inst.PayoutResult.CheckPayout();  //確認派彩金額
            StartShowAwsrds = false;//關閉輪盤轉動
            NowSpeed = 0.0f;    //輪盤速度歸零
            SicBoGameMain.Inst.CircularControl.RouletteTurnSound.Stop();    //關閉輪盤音效
            if (Roulette4DiceNumber != 0)
            {   //如果第四骰有點數 = 開中心輪盤
                HasGoldLeopard = false;
                if (RouletteDiceNumber[0] == Roulette4DiceNumber)
                    HasGoldLeopard = true;  //開啟黃金豹預告
                ChangeShutterTimer = 0.0f;  //初始化計時器
                ShutterSpriteNumber = 1;    //輪盤快門圖片編號 = 1
                RouletteEffectNumber = 1;   //切換輪盤特效圖片編號 = 1
                ChangeRoulette4 = true;     //開始切換四號輪盤
                ShutterSound.Play();    //播放快門音效
            }
            else
            {   //否則進行結算
                ShadowPanel.depth = 5;  //設定陰影Panel層級
                RouletteShadow.enabled = true;  //顯示輪盤陰影
                SicBoGameMain.Inst.DoSettlement();
            }
        }
        else if (RouletteStops[0] && RouletteStops[1])
        {   //輪盤 1,2 停止 判斷是否雙骰
            if (RouletteDiceNumber[0] == RouletteDiceNumber[1])
            {   //如果兩個點數相同 = 雙骰
                DiceDoubleLevel = 1;    //雙骰等級 = 1 //如果兩個骰子ID相同 且 ID為3的倍數 = 紅雙骰
                if (RouletteDiceID[0] == RouletteDiceID[1] && RouletteDiceID[0] % 3 == 0)
                    DiceDoubleLevel = 2;    //雙骰等級 = 2
            }
        }
        else if (RouletteStops[1] && RouletteStops[2])
        {   //輪盤 2,3 停止 判斷是否雙骰
            if (RouletteDiceNumber[1] == RouletteDiceNumber[2])
            {   //如果兩個點數相同 = 雙骰
                DiceDoubleLevel = 1;    //雙骰等級 = 1 //如果兩個骰子ID相同 且 ID為3的倍數 = 紅雙骰
                if (RouletteDiceID[1] == RouletteDiceID[2] && RouletteDiceID[1] % 3 == 0)
                    DiceDoubleLevel = 2;    //雙骰等級 = 2
            }
        }
        else if (RouletteStops[2] && RouletteStops[0])
        {   //輪盤 3,1 停止 判斷是否雙骰
            if (RouletteDiceNumber[2] == RouletteDiceNumber[0])
            {   //如果兩個點數相同 = 雙骰
                DiceDoubleLevel = 1;    //雙骰等級 = 1 //如果兩個骰子ID相同 且 ID為3的倍數 = 紅雙骰
                if (RouletteDiceID[2] == RouletteDiceID[0] && RouletteDiceID[2] % 3 == 0)
                    DiceDoubleLevel = 2;    //雙骰等級 = 2
            }
        }
        //根據雙骰等級來進行特效文字的演示
        if (DiceDoubleLevel > 0 && !EffectHasPlayer)
            SicBoGameMain.Inst.CircularControl.ShowEffectText(DiceDoubleLevel);
    }

    void DoChangeRoulette4()
    {
        ChangeShutterTimer = 0.0f; //計時器歸零
        if (ShutterSpriteNumber < 49)
        {
            if (ShutterSpriteNumber == 1)
            {   //快門啟動第1張 啟動訊息文字的旋轉與縮小
                RouletteRotation[0].ResetToBeginning();
                RouletteRotation[0].enabled = true;
                RouletteScale[0].ResetToBeginning();
                RouletteScale[0].enabled = true;
            }
            //根據ShutterSpriteNumber切換快門圖片
            ShutterSprite.spriteName = "closeDisc4_" + ShutterSpriteNumber.ToString("00");
            if (ShutterSpriteNumber == 25)
            {   //快門啟動第25張 啟動四號輪盤的旋轉與放大
                Roulette4Sprite.enabled = true; //開啟圖片
                RouletteRotation[1].ResetToBeginning();
                RouletteRotation[1].enabled = true;
                RouletteScale[1].ResetToBeginning();
                RouletteScale[1].enabled = true;
            }
            //快門啟動第32張 啟動輪盤切換特效
            if (ShutterSpriteNumber == 25)
                ChangeEffect = true;
            ShutterSpriteNumber++;
        }
        else
        {
            ChangeShutterTimer = 0.0f;  //初始化計時器
            ChangeRoulette4 = false;    //關閉切換            
            NowSpeed = MaxSpeed;    //設定輪盤速度
            Roulette4Unit.NowRotating = true;   //開始四號輪盤轉動
            StartShowRoulette4 = true;  //四號輪盤開始轉動
            Roulette4BGM.Play();    //播放BGM
        }
    }

    void DoChangeEffect()
    {
        if (RouletteEffectNumber < 46)
        {
            if (RouletteEffectNumber == 1)
            {   //切換特效第1張  開啟特效圖片
                ChangeRouletteEffect.enabled = true;
            }
            //根據ShutterSpriteNumber切換特效圖片
            ChangeRouletteEffect.spriteName = "openDisc4Effect_" + RouletteEffectNumber.ToString("00");
            RouletteEffectNumber++;
        }
        else
        {
            ChangeRouletteEffect.enabled = false;   //關閉特效圖片
            ChangeEffectTimer = 0.0f;   //初始化計時器
            ChangeEffect = false;   //關閉切換
        }
    }
}