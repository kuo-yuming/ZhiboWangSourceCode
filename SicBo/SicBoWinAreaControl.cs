using UnityEngine;
using System.Collections;
using GameCore.Manager.SicBo;
using System.Collections.Generic;

public class SicBoWinAreaControl : MonoBehaviour
{
    public SicBoBetAreaControl BetAreaControl;  //投注區塊控制 初始化得獎區塊用
    public SicBoWinAreaUnit[] WinAreaUnit;  //中獎區塊圖片陣列
    public bool IsFlashing = false; //是否閃爍
    private byte AlphaNumber = 0;   //計算Alpah用
    private bool AlphaIncrement;    //Alpha是否遞增

    // Update is called once per frame
    void Update()
    {
        if (IsFlashing) Flashing(); //執行閃爍
    }

    public void InitWinUnit()
    {   //初始化閃爍區
        for (int i = 0; i < WinAreaUnit.Length; i++)
        {   //根據押注額度給予圖片
            WinAreaUnit[i].SelfSprite.spriteName = "WinArea" + SicBoManager.NowGroup + "_" + (i + 1).ToString("00");
            WinAreaUnit[i].SelfSprite.color = new Color(1, 1, 1, 0);    //隱藏圖片
            WinAreaUnit[i].AwardType = BetAreaControl.BetUnit[i].AwardType; //設定獎項類別
            WinAreaUnit[i].Offset = BetAreaControl.BetUnit[i].Offset;   //設定獎項補正值
        }
        CloseFlashing();    //關閉閃爍
    }

    public void CloseFlashing()
    {   //關閉閃爍
        IsFlashing = false; //關閉閃爍
        for (int i = 0; i < WinAreaUnit.Length; i++)
        {   //關閉所有區塊的閃爍與隱藏圖片
            WinAreaUnit[i].IamFlashing = false;
            WinAreaUnit[i].SelfSprite.color = new Color(1, 1, 1, 0);
        }
    }

    public void DetermineWinArea()
    {   //判斷得獎區塊
        foreach (CPACK_SicBo_AwardArea item in SicBoManager.NoitfyAwardData.m_listAwardAreaID)
        {
            for (int i = 0; i < WinAreaUnit.Length; i++)
                if ((byte)WinAreaUnit[i].AwardType == item.m_byAwardAreaID && WinAreaUnit[i].Offset == item.m_byAwardNumber)
                    WinAreaUnit[i].IamFlashing = true;
        }
    }

    public void DoFlashing()
    {
        IsFlashing = true;  //開始閃爍
        AlphaNumber = 0;    //Alpha = 0
        AlphaIncrement = true;  //Alpha為遞增狀態
    }

    public void Flashing()
    {   //執行閃爍
        for (int i = 0; i < WinAreaUnit.Length; i++)    //判斷每個區塊
            if (WinAreaUnit[i].IamFlashing) //如果區塊閃爍            
                WinAreaUnit[i].SelfSprite.color = new Color32(255, 255, 255, AlphaNumber);  //設定Alpha
        //更新Alpha //Alpha = 249 <-> 6
        if (AlphaNumber < 249 && AlphaIncrement)
            AlphaNumber += (byte)(Time.deltaTime * 600.0f);
        else if (AlphaNumber > 6 && !AlphaIncrement)
            AlphaNumber -= (byte)(Time.deltaTime * 600.0f);
        else
            AlphaIncrement = !AlphaIncrement;
    }
}