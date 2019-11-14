using UnityEngine;
using System.Collections;
using GameCore.Manager.IceAge;
using GameCore;

public class Egg_Control : MonoBehaviour
{
    public byte EggNumber = 0;
    private Transform EggSprite;
    private Transform Symbol;
    private Transform Multiple;
    private Transform Hammer;
    public bool SelfIsBroken = false;
    private string SelfAwardName;
    private ushort SelfAwardMoney;

    void Awake()
    {
        EggSprite = transform.GetChild(0);
        Symbol = transform.GetChild(1);
        Multiple = transform.GetChild(2);
        Hammer = transform.GetChild(3);
    }

    // Update is called once per frame
    void Update()
    {
        if (SelfIsBroken && EggSprite.GetComponent<UISprite>().enabled && EggSprite.GetComponent<UISprite>().spriteName == "BrokenEgg_14")
        {   //蛋殼跟槌子播放到最後面 隱藏 並顯示標誌與倍率
            EggSprite.GetComponent<UISprite>().enabled = false;
            Hammer.GetComponent<UISprite>().enabled = false;
            Symbol.GetComponent<UISpriteAnimation>().ResetToBeginning();
            Multiple.GetComponent<UISpriteAnimation>().ResetToBeginning();
            //顯示標誌與倍率
            Symbol.GetComponent<UISprite>().spriteName = "Symbol_" + SelfAwardName + "_00"; //標誌
            Symbol.GetComponent<UISprite>().enabled = true;
            Symbol.GetComponent<UISpriteAnimation>().namePrefix = "Symbol_" + SelfAwardName + "_";
            Symbol.GetComponent<UISpriteAnimation>().enabled = true;
            if (SelfAwardMoney != 0)
            {
                Multiple.GetComponent<UISprite>().spriteName = "MultipleNumber_" + SelfAwardMoney + "_00";  //倍率
                Multiple.GetComponent<UISprite>().enabled = true;
                Multiple.GetComponent<UISpriteAnimation>().namePrefix = "MultipleNumber_" + SelfAwardMoney + "_";
                Multiple.GetComponent<UISpriteAnimation>().enabled = true;
            }
        }
    }

    public void Close()
    {
        transform.GetComponent<BoxCollider>().enabled = false;          //Collider
        EggSprite.GetComponent<UISprite>().enabled = false;             //蛋
        EggSprite.GetComponent<UISpriteAnimation>().enabled = false;
        Symbol.GetComponent<UISprite>().enabled = false;                //標誌
        Symbol.GetComponent<UISpriteAnimation>().enabled = false;
        Multiple.GetComponent<UISprite>().enabled = false;              //倍率
        Multiple.GetComponent<UISpriteAnimation>().enabled = false;
        Hammer.GetComponent<UISprite>().enabled = false;                //槌子
        Hammer.GetComponent<UISpriteAnimation>().enabled = false;
    }

    public void Show()
    {
        if (!SelfIsBroken)
        {   //如果還沒破掉 設定物件
            EggSprite.GetComponent<UISprite>().spriteName = "BrokenEgg_00"; //蛋
            EggSprite.GetComponent<UISprite>().enabled = true;
            Symbol.GetComponent<UISprite>().enabled = false;                //標誌
            Symbol.GetComponent<UISpriteAnimation>().enabled = false;
            Multiple.GetComponent<UISprite>().enabled = false;              //倍率
            Multiple.GetComponent<UISpriteAnimation>().enabled = false;
            Hammer.GetComponent<UISprite>().enabled = false;                //槌子
            Hammer.GetComponent<UISpriteAnimation>().enabled = false;
        }
        else Close();  //破掉 則全部關閉
    }

    public void Play()
    {
        if (!SelfIsBroken) transform.GetComponent<BoxCollider>().enabled = true;    //Collider
    }

    public void CloseBreak()
    {
        transform.GetComponent<BoxCollider>().enabled = false;
    }

    public void GetBreak(byte BreakEggNumber, string AwardName, ushort AwardMoney)
    {
        //敲擊後 如果是自己被射中 演示動畫
        if (EggNumber == BreakEggNumber)
        {
            transform.GetComponent<BoxCollider>().enabled = false;      //Collider
            EggSprite.GetComponent<UISpriteAnimation>().ResetToBeginning();
            EggSprite.GetComponent<UISpriteAnimation>().enabled = true; //蛋動畫    
            Hammer.GetComponent<UISprite>().enabled = true;             //槌子圖片顯示
            Hammer.GetComponent<UISpriteAnimation>().ResetToBeginning();
            Hammer.GetComponent<UISprite>().spriteName = "Hammer_0";
            Hammer.GetComponent<UISpriteAnimation>().namePrefix = "Hammer";                       
            Hammer.GetComponent<UISpriteAnimation>().enabled = true;    //槌子動畫
            SelfAwardName = AwardName;  //獎項名稱
            SelfAwardMoney = AwardMoney;//獎項金額
        }
        else if (!SelfIsBroken) //否則 物件全部消失 只顯示蛋圖片
        {
            Close();
            EggSprite.GetComponent<UISprite>().spriteName = "BrokenEgg_00"; //蛋圖片
            EggSprite.GetComponent<UISprite>().enabled = true;
        }
    }

    public void OnClick()
    {
        SelfIsBroken = true;
        GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.IceAge, (uint)ENUM_ICEAGE_PACKID_GC.C2G_Bet_ReqKnockEgg, null);
        BreakGame_Control.Inst.WhichEggIsBreak = EggNumber;
        BreakGame_Control.Inst.GetBreak();
    }
}
