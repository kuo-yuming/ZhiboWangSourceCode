using UnityEngine;
using System.Collections;
using GameCore.Manager.IceAge;
using GameCore;

public class Leaf_Control : MonoBehaviour
{
    public char LeafName;    
    public Transform LeafSprite;
    public Transform LeafLight;
    public Transform Arrow;
    public Transform Crosshair;
    public Transform Symbol;
    public Transform Multiple;

    void Update()
    {
        if (ShotGame_Control.Inst.IsGetShot && Crosshair.GetComponent<UISprite>().spriteName == "Crosshair_9")
            Crosshair.GetComponent<UISprite>().enabled = false;
    }
    public void Close()
    {
        transform.GetComponent<BoxCollider>().enabled = false;          //Collider
        LeafSprite.GetComponent<UISprite>().enabled = false;            //樹葉圖片
        LeafSprite.GetComponent<UISpriteAnimation>().enabled = false;
        LeafLight.GetComponent<UISprite>().enabled = false;             //螢火蟲光
        LeafLight.GetComponent<UISpriteAnimation>().enabled = false;
        Arrow.GetComponent<UISprite>().enabled = false;                 //箭頭
        Arrow.GetComponent<TweenScale>().enabled = false;
        Crosshair.GetComponent<UISprite>().enabled = false;             //準心
        Crosshair.GetComponent<UISpriteAnimation>().enabled = false;
        Symbol.GetComponent<UISprite>().enabled = false;                //標誌
        Symbol.GetComponent<UISpriteAnimation>().enabled = false;
        Symbol.GetComponent<TweenPosition>().enabled = false;
        if (LeafName == 'A')    //倍率
        {
            Multiple.GetComponent<UISprite>().enabled = false;
            Multiple.GetComponent<TweenPosition>().enabled = false;
        }
        else
        {
            Multiple.GetComponent<UISprite>().enabled = false;              
            Multiple.GetComponent<UISpriteAnimation>().enabled = false;
        }
    }
    public void Show()
    {
        LeafSprite.GetComponent<UISprite>().spriteName = "Leaf_" + LeafName + "_0"; //樹葉圖片
        LeafSprite.GetComponent<UISprite>().enabled = true;
        LeafLight.GetComponent<UISprite>().spriteName = "FireflyLight_Leaf_" + LeafName + "_00";    //螢火蟲光
        Arrow.GetComponent<UISprite>().enabled = true;                  //箭頭
        Crosshair.GetComponent<UISprite>().spriteName = "Crosshair_0";  //準心
        Symbol.GetComponent<UISprite>().enabled = false;                //標誌
        Symbol.GetComponent<UISpriteAnimation>().enabled = false;
        Symbol.GetComponent<TweenPosition>().enabled = false;
        if (LeafName == 'A')    //倍率
        {
            Multiple.GetComponent<UISprite>().enabled = false;
            Multiple.GetComponent<TweenPosition>().enabled = false;
        }
        else
        {
            Multiple.GetComponent<UISprite>().enabled = false;
            Multiple.GetComponent<UISpriteAnimation>().enabled = false;
        }
    }

    public void Play()
    {
        transform.GetComponent<BoxCollider>().enabled = true;           //Collider
        LeafLight.GetComponent<UISprite>().enabled = true;              //螢火蟲光
        LeafLight.GetComponent<UISpriteAnimation>().enabled = true;
        Arrow.GetComponent<TweenScale>().enabled = true;                //箭頭
    }

    public void CloseShot()
    {
        transform.GetComponent<BoxCollider>().enabled = false;
    }

    public void GetShot(char ShotInLeafName, string AwardName, ushort AwardMoney)
    {   //開槍後 如果是自己被射中 演示動畫
        if (LeafName == ShotInLeafName)
        {
            transform.GetComponent<BoxCollider>().enabled = false;          //Collider
            LeafSprite.GetComponent<UISpriteAnimation>().ResetToBeginning();//樹葉圖片
            LeafSprite.GetComponent<UISpriteAnimation>().enabled = true;
            LeafLight.GetComponent<UISprite>().enabled = false;             //螢火蟲光
            LeafLight.GetComponent<UISpriteAnimation>().enabled = false;
            Arrow.GetComponent<UISprite>().enabled = false;                 //箭頭
            Arrow.GetComponent<TweenScale>().enabled = false;
            Crosshair.GetComponent<UISprite>().enabled = true;              //準心
            Crosshair.GetComponent<UISpriteAnimation>().ResetToBeginning();
            Crosshair.GetComponent<UISpriteAnimation>().enabled = true;
            Symbol.GetComponent<UISprite>().spriteName = "Symbol_" + AwardName + "_00"; //標誌
            Symbol.GetComponent<UISprite>().enabled = true;                 
            Symbol.GetComponent<UISpriteAnimation>().namePrefix = "Symbol_" + AwardName + "_";
            Symbol.GetComponent<UISpriteAnimation>().ResetToBeginning();
            Symbol.GetComponent<UISpriteAnimation>().enabled = true;
            Symbol.GetComponent<TweenPosition>().ResetToBeginning();
            Symbol.GetComponent<TweenPosition>().enabled = true;
            if (AwardMoney != 0)
            {
                if (LeafName == 'A')
                {
                    Multiple.GetComponent<UISprite>().spriteName = "MultipleNumber_" + AwardMoney + "_14";  //倍率
                    Multiple.GetComponent<UISprite>().enabled = true;
                    Multiple.GetComponent<TweenPosition>().ResetToBeginning();
                    Multiple.GetComponent<TweenPosition>().enabled = true;
                }
                else
                {
                    Multiple.GetComponent<UISprite>().spriteName = "MultipleNumber_" + AwardMoney + "_00";  //倍率
                    Multiple.GetComponent<UISprite>().enabled = true;
                    Multiple.GetComponent<UISpriteAnimation>().namePrefix = "MultipleNumber_" + AwardMoney + "_";
                    Multiple.GetComponent<UISpriteAnimation>().ResetToBeginning();
                    Multiple.GetComponent<UISpriteAnimation>().enabled = true;
                }
            }
        }
        else  //否則 物件全部消失 只顯示樹葉圖片
        {
            Close();
            LeafSprite.GetComponent<UISprite>().spriteName = "Leaf_" + LeafName + "_0"; //樹葉圖片
            LeafSprite.GetComponent<UISprite>().enabled = true;
        }
    }

    public void OnClick()
    {
        GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.IceAge, (uint)ENUM_ICEAGE_PACKID_GC.C2G_Bet_ReqShoot, null);
        ShotGame_Control.Inst.WhichLeafWasShotIn = LeafName;
        ShotGame_Control.Inst.GetShot();        
    }
}