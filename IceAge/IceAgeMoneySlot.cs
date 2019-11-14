using UnityEngine;
using System.Collections;
using GameCore.Machine;
using System;

public class IceAgeMoneySlot : MonoBehaviour
{
    public static IceAgeMoneySlot Inst;
    public UILabel m_Label;
    public RunType m_Type = RunType.SameTime;
    public float RandgeNumber = 10;
    public bool NowChange = false;
    public ulong TargetNumber = 0;
    public ulong NowNumber = 0;
    public enum RunType
    {
        SameTime,
        SameRandge,
    }
    // Use this for initialization
    void Awake()
    {
        Inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        //m_Label.text = NowNumber + "";
        if (NowChange)
        {
            NowNumber = TargetNumber;
            LastWork();
            m_Label.text = NowNumber + "";
            NowChange = false;
            #region Slot式 顯示金錢 現階段捨棄
            /*if (m_Type == RunType.SameTime)
            {
                long Randge = (long)((TargetNumber - NowNumber) / RandgeNumber);
                NowNumber += Randge;
                if ((TargetNumber - NowNumber) <= RandgeNumber && (TargetNumber - NowNumber) >= 0)
                {
                    NowNumber = TargetNumber;
                    LastWork();
                    NowChange = false;                    
                }
                else if ((NowNumber - TargetNumber) <= RandgeNumber && (NowNumber - TargetNumber) >= 0)
                {
                    NowNumber = TargetNumber;
                    LastWork();
                    NowChange = false;
                }
            }
            else if (m_Type == RunType.SameRandge)
            {
                if (TargetNumber >= NowNumber)
                {
                    NowNumber += (long)RandgeNumber;
                    if (TargetNumber <= NowNumber)
                    {
                        NowNumber = TargetNumber;
                        LastWork();
                        NowChange = false;
                    }
                }
                if (NowNumber >= TargetNumber)
                {
                    NowNumber -= (long)RandgeNumber;
                    if (TargetNumber >= NowNumber)
                    {
                        NowNumber = TargetNumber;
                        LastWork();
                        NowChange = false;
                    }
                }
            }*/
            #endregion
        }
    }

    void LastWork()
    {        
        if (IceAgeGameMain.Inst.GameStatus == IceAgeGameMain.Game_Status.WaitMoneySlot
            || IceAgeGameMain.Inst.GameStatus == IceAgeGameMain.Game_Status.GetScore)
        {
            IceAgeGameMain.Inst.ReStartTimer = 1.0f;   // MainGame 金錢跳完後 兩秒CD
            AddAwardRecord();
        }
        else if (IceAgeGameMain.Inst.GameStatus == IceAgeGameMain.Game_Status.FreeGame)
        {
            IceAgeGameMain.Inst.ReStartTimer = 1.0f;    // FreeGame 金錢跳完後 一秒CD
            AddAwardRecord();
        }
    }

    void AddAwardRecord()
    {
        //加入個人大獎記錄
        foreach (var item in IceAgeManager.m_BetResult.m_dicLineAward)
        {
            if (IceAgeManager.m_BetResult.m_dicLineAward.Count != 0 && item.Value >= 27)
            {
                CPACK_PMachineAwardRecord AllIceAgeEnd = new CPACK_PMachineAwardRecord();
                AllIceAgeEnd.m_byComboCnt = 0;
                if (item.Value == 27) AllIceAgeEnd.m_byAllWinAwardID = 98;      //如果是射擊就改成98
                else if (item.Value == 36) AllIceAgeEnd.m_byAllWinAwardID = 99; //如果是敲蛋就改成99
                else AllIceAgeEnd.m_byAllWinAwardID = item.Value;               //其他獎項不變
                AllIceAgeEnd.m_strPlayerNickName = MainConnet.m_PlayerData.m_strNickName;
                AllIceAgeEnd.m_uiPlayerDBID = MainConnet.m_PlayerData.m_uiDBID;
                AllIceAgeEnd.m_uiMID = GameConnet.m_NowBuyInMachineID;
                AllIceAgeEnd.m_uiMoney = IceAgeManager.m_BonusResult.m_uiScoreSum;
                AllIceAgeEnd.m_ui64Time = ((ulong)DateTime.Now.Year * 100000000 + (ulong)DateTime.Now.Month * 1000000 + (ulong)DateTime.Now.Day * 10000 + (ulong)DateTime.Now.Hour * 100 + (ulong)DateTime.Now.Minute);
                IceAgeManager.M_AwardRecord.Add(AllIceAgeEnd);
                IceAgeManager.O_AwardRecord.Add(AllIceAgeEnd);
            }
        }
    }
}