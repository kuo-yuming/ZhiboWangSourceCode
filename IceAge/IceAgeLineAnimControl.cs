using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class IceAgeLineAnimControl : MonoBehaviour
{
    public static IceAgeLineAnimControl Inst;
    public UISprite[] StaticPic;
    public Transform Line;
    public Transform Light;
    public Transform SlotPic;
    public Transform SlotBox;
    public float AnimStopTime = 4;
    private float PlayingTimer = 0;

    public byte[,] LineArray = new byte[,]
    { { 0, 1, 2, 3, 4 }, { 5, 6, 7, 8, 9 }, { 10, 11, 12, 13, 14 },
      { 0, 6, 12, 8, 4 }, { 10, 6, 2, 8, 14 }, { 5, 11, 7, 3, 9 },
      { 5, 1, 7, 13, 9 }, { 0, 1, 7, 13, 14 }, { 10, 11, 7, 3, 4 }
    };

    public Anim_Status AnimStatus = Anim_Status.Idle;
    public enum Anim_Status
    {
        Idle = 0,
        Playing = 1,
    }

    void Awake()
    {
        Inst = this;
    }

    public void ReSetLineAnim()
    {
        AnimStatus = Anim_Status.Idle;
        //開啟所有靜態圖
        for (int i = 0; i < transform.childCount; i++)
            for (int j = 0; j < transform.GetChild(i).childCount; j++)
                transform.GetChild(i).GetChild(j).GetComponent<UISprite>().enabled = true;
        //關閉動態圖
        for (int i = 0; i < Line.childCount; i++)
        {
            Line.GetChild(i).GetComponent<UISprite>().enabled = false;
            Line.GetChild(i).GetComponent<TweenColor>().enabled = false;
            Light.GetChild(i).GetComponent<UISprite>().enabled = false;
            Light.GetChild(i).GetComponent<UISpriteAnimation>().enabled = false;
        }
        for (int i = 0; i < SlotPic.childCount; i++)
        {
            SlotPic.GetChild(i).GetComponent<UISprite>().enabled = false;
            SlotPic.GetChild(i).GetComponent<UISpriteAnimation>().enabled = false;
            SlotBox.GetChild(i).GetComponent<UISprite>().enabled = false;
            SlotBox.GetChild(i).GetComponent<UISpriteAnimation>().enabled = false;

        }
    }

    public void PlayLineAnim(bool IsBonus)
    {
        AnimStatus = Anim_Status.Playing;
        foreach (KeyValuePair<byte, byte> item in IceAgeManager.m_BetResult.m_dicLineAward)
        {   //線跟數字的閃爍
            Line.GetChild(item.Key - 1).GetComponent<UISprite>().enabled = true;
            Line.GetChild(item.Key - 1).GetComponent<TweenColor>().enabled = true;
            Light.GetChild(item.Key - 1).GetComponent<UISprite>().enabled = true;
            Light.GetChild(item.Key - 1).GetComponent<UISpriteAnimation>().enabled = true;

            PlaySlotBox(item.Key, item.Value, IsBonus);
        }
        if (!IsBonus)   //非Bonus才會顯示金錢動畫
        {
            IceAgeWinMoney.Inst.WinMoney = IceAgeManager.m_BetResult.m_uiScore;
            IceAgeWinMoney.Inst.StartPlay = true;   //金錢動畫
        }        
    }

    void PlaySlotBox(byte Key, byte Value, bool IsBonus)
    {   //計算combo數
        int NumofCombo = 0;
        if (Value < 37) NumofCombo = (Value % 9 == 0) ? 1 + Value / 9 : 2 + Value / 9;
        else NumofCombo = 5;
        //JP //四連線 //五連線 //一般獎項
        if (Value == 37) IceAgeSoundControl.Inst.PlaySound(IceAgeSoundControl.SoundName.JPCombo, false);   //播放音效
        else if (NumofCombo == 4) IceAgeSoundControl.Inst.PlaySound(IceAgeSoundControl.SoundName.FourCombo, false);   //播放音效
        else if (NumofCombo == 5) IceAgeSoundControl.Inst.PlaySound(IceAgeSoundControl.SoundName.FiveCombo, false);   //播放音效
        else IceAgeSoundControl.Inst.PlaySound(IceAgeSoundControl.SoundName.GetAwards, false);   //播放音效
        //顯示
        string CardName = "";
        //圖示位置
        for (int i = 0; i < NumofCombo; i++)
        {   //啟用圖片
            SlotPic.GetChild(LineArray[(Key - 1), i]).GetComponent<UISprite>().enabled = true;
            SlotPic.GetChild(LineArray[(Key - 1), i]).GetComponent<UISpriteAnimation>().enabled = true;
            SlotBox.GetChild(LineArray[(Key - 1), i]).GetComponent<UISprite>().enabled = true;
            SlotBox.GetChild(LineArray[(Key - 1), i]).GetComponent<UISpriteAnimation>().enabled = true;
            //關閉靜態圖
            StaticPic[LineArray[(Key - 1), i]].GetComponent<UISprite>().enabled = false;
            //指定圖片  根據獎項圖片給予動畫圖名稱
            switch (IceAgeManager.m_BetResult.m_byarGridSymbol[LineArray[(Key - 1), i]])
            {
                case 0: CardName = "Sym10_";    break;
                case 1: CardName = "SymJ_";     break;
                case 2: CardName = "SymQ_";     break;
                case 3: CardName = "SymK_";     break;
                case 4: CardName = "SymA_";     break;
                case 5: CardName = "SymbolB_";  break;
                case 6: CardName = "SymbolT_";  break;
                case 7: CardName = "SymbolZ_";  break;
                case 8: CardName = "SymbolD_";  break;
                case 9: CardName = "SymDM_";    break;
                case 10:CardName = "Symwild_";  break;
                default:    break;
            }
            SlotPic.GetChild(LineArray[(Key - 1), i]).GetComponent<UISprite>().spriteName = CardName + "01";
            SlotPic.GetChild(LineArray[(Key - 1), i]).GetComponent<UISpriteAnimation>().namePrefix = CardName;
            if (IsBonus)
            {   //如果是Bonus獎 SlotBox為紅色
                SlotBox.GetChild(LineArray[(Key - 1), i]).GetComponent<UISprite>().spriteName = "spbox_01";
                SlotBox.GetChild(LineArray[(Key - 1), i]).GetComponent<UISpriteAnimation>().namePrefix = "spbox_";
            } 
            else
            {   //如果是一般獎 SlotBox為藍色
                SlotBox.GetChild(LineArray[(Key - 1), i]).GetComponent<UISprite>().spriteName = "winbox01";
                SlotBox.GetChild(LineArray[(Key - 1), i]).GetComponent<UISpriteAnimation>().namePrefix = "winbox";
            }
        }        
    }

    public void PlayFreeGameSlotBox()
    {
        AnimStatus = Anim_Status.Playing;
        for (int i = 0; i < IceAgeManager.m_BetResult.m_byarGridSymbol.Length; i++)
        {
            if (IceAgeManager.m_BetResult.m_byarGridSymbol[i] == 9)
            {   //啟用圖片
                SlotPic.GetChild(i).GetComponent<UISprite>().enabled = true;
                SlotPic.GetChild(i).GetComponent<UISpriteAnimation>().enabled = true;
                SlotBox.GetChild(i).GetComponent<UISprite>().enabled = true;
                SlotBox.GetChild(i).GetComponent<UISpriteAnimation>().enabled = true;
                //關閉靜態圖
                StaticPic[i].GetComponent<UISprite>().enabled = false;
                //指定圖片  根據獎項圖片給予動畫圖名稱
                SlotPic.GetChild(i).GetComponent<UISprite>().spriteName = "SymDM_01";
                SlotPic.GetChild(i).GetComponent<UISpriteAnimation>().namePrefix = "SymDM_";
                SlotBox.GetChild(i).GetComponent<UISprite>().spriteName = "spbox_01";
                SlotBox.GetChild(i).GetComponent<UISpriteAnimation>().namePrefix = "spbox_";
            }
        }
    }
}
