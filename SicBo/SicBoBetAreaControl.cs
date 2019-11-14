using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore.Manager.SicBo;
using GameCore;
using System;

public class AutoBetUnit
{
    public byte BetAreaID = 0;
    public int BetMoney = 0;
}

public class SicBoBetAreaControl : MonoBehaviour
{
    public SicBoBetUnit[] BetUnit = new SicBoBetUnit[36];   //投注區塊
    public object BetResultLock = new object();  //投注資訊更新鎖
    public List<CPACK_SicBo_NotifyBet> BetData_Hold = new List<CPACK_SicBo_NotifyBet>();   //收到偷注資訊暫存於此
    public List<AutoBetUnit> AutoBetArea = new List<AutoBetUnit>(); //自動投注區塊
    public AudioSource BetSound;    //投注音效
    public AudioSource PayoutSound; //派彩音效
    
    // Update is called once per frame
    void Update()
    {
        if (BetData_Hold.Count != 0)
        {
            lock (BetResultLock)
            {
                SicBoBetUnit NowUnit = new SicBoBetUnit();
                foreach (CPACK_SicBo_NotifyBet item in BetData_Hold)
                {   //判斷投注區塊
                    for (int i = 0; i < BetUnit.Length; i++)
                        if ((byte)BetUnit[i].AwardType == item.m_byAreaID && BetUnit[i].Offset == item.m_byOffset)
                            NowUnit = BetUnit[i];
                    //顯示全部投注金額
                    NowUnit.ShowAllBet(item.m_uiAllBetMoney);
                    //如果投注玩家等於自己 顯示數字
                    if (item.m_uiBetPlayerDBID == MainConnet.m_PlayerData.m_uiDBID)
                        NowUnit.ShowMyBet(item.m_uiPlayerBetMoney);
                    SicBoGameMain.Inst.BetAreaControl.CalculateBet();   //更新押注與金幣
                }
                BetData_Hold.Clear();   //顯示完畢後 清除BetData_Hold
                BetSound.Play();    //播放音效
                bool HasBet = false;//如果有押注 開放取消押注按鈕 否則關閉
                for (int i = 0; i < BetUnit.Length; i++)
                    if (BetUnit[i].MyChipBack.enabled)
                        HasBet = true;
                SicBoGameMain.Inst.ButtonControl.CancelBetButton.isEnabled = HasBet;
            }
        }
    }

    public void InitBetUnit()
    {   //初始化每個投注區
        for (int i = 0; i < BetUnit.Length; i++)
            BetUnit[i].InitMySelf();
    }

    public void OpenBet()
    {   //開放投注 取消投注
        for (int i = 0; i < BetUnit.Length; i++)
            BetUnit[i].OpenMySelf();
        SicBoGameMain.Inst.ButtonControl.CancelBetButton.isEnabled = true;
    }

    public void CloseBet()
    {   //關閉投注 取消投注
        for (int i = 0; i < BetUnit.Length; i++)
            BetUnit[i].CloseMySelf();
        SicBoGameMain.Inst.ButtonControl.CancelBetButton.isEnabled = false;
    }

    public void HideBet()
    {   //隱藏籌碼與投注
        Debug.LogError("隱藏籌碼與投注");
        for (int i = 0; i < BetUnit.Length; i++)
            BetUnit[i].HideMyBet();
    }

    public void CancelBet()
    {   //取消押注
        for (int i = 0; i < BetUnit.Length; i++)
            BetUnit[i].CancelBet();
    }

    public void InitPayForWho()
    {   //初始化派彩對象
        for (int i = 0; i < BetUnit.Length; i++)
            BetUnit[i].ChipTweenPos.to = BetUnit[i].BankerVector3;
    }

    public void CheckPayForWho()
    {   //確認派彩對象
        foreach (var item in SicBoManager.NoitfyAwardData.m_listAwardAreaID)
        {   //比對區塊ID與補正值
            for (int i = 0; i < BetUnit.Length; i++) //如果有相同區塊 且 該區塊金額背景啟用 代表該區塊玩家有得獎 
                if ((byte)BetUnit[i].AwardType == item.m_byAwardAreaID && BetUnit[i].Offset == item.m_byAwardNumber && BetUnit[i].MyChipBack.enabled)
                    BetUnit[i].ChipTweenPos.to = BetUnit[i].PlayerVector3; //派彩給玩家
        }
    }

    public void DoPayout()
    {   //開始派彩
        for (int i = 0; i < BetUnit.Length; i++)
        {
            if (BetUnit[i].MyChipSprite.enabled)
            {   //有籌碼圖片的區塊 移動籌碼
                BetUnit[i].HideMyBet(); //隱藏金額
                BetUnit[i].ChipTweenPos.ResetToBeginning();
                BetUnit[i].ChipTweenPos.enabled = true; //移動籌碼
            }
        }
        PayoutSound.Play(); //播放音效
    }

    public void CheckAutoBet()
    {   //確認自動押注
        AutoBetArea = new List<AutoBetUnit>();  //初始化List
        for (byte i = 0; i < BetUnit.Length; i++)
            if (BetUnit[i].MyChipBack.enabled)
            {   //如果有押注 加入List
                AutoBetUnit tmpBetUnit = new AutoBetUnit();
                tmpBetUnit.BetAreaID = i;   //投注區塊ID
                tmpBetUnit.BetMoney = (int)BetUnit[i].MyChipUint;  //投注金額
                AutoBetArea.Add(tmpBetUnit);
            }
    }

    public void DoAutoBet()
    {   //進行自動押注
        foreach (var item in AutoBetArea)
            BetUnit[item.BetAreaID].AutoBet(item.BetMoney); //根據每個陣列ID與金額 進行押注
    }

    public void CalculateBet()
    {   //每次收到更新押注 計算目前投注
        uint tmpBet = 0;    //押注暫存
        for (byte i = 0; i < BetUnit.Length; i++)   //檢查每個投注區
            if (BetUnit[i].MyChipBack.enabled)  //如果有押注
                tmpBet += BetUnit[i].MyChipUint;//將押金累計
        //累計完畢後 設定金幣與押注
        SicBoGameMain.Inst.ButtonControl.SetBet(tmpBet);
    }
}
