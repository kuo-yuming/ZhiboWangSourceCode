using UnityEngine;
using System.Collections;

public class SicBoChipControl : MonoBehaviour
{
    public SicBoChipUnit[] ChipUnit;    //籌碼

    public void SetNowChoose(byte ChipID)
    {   //設定目前選擇的籌碼ID跟面額
        SicBoGameMain.Inst.NowChipID = ChipID;
        SicBoGameMain.Inst.NowQuota = ChipUnit[ChipID].MyQuota;
        //切換籌碼顯示
        for (int i = 0; i < ChipUnit.Length; i++)
            ChipUnit[i].SetSwitch(false);   //關閉全部的
        ChipUnit[ChipID].SetSwitch(true);   //開放目前的
    }

    public void SetChipType(byte NowType)
    {   //根據選擇投注類型切換籌碼
        switch (NowType)
        {
            case 1:
                ChipUnit[0].SetQuota(100);
                ChipUnit[1].SetQuota(500);
                ChipUnit[2].SetQuota(1000);
                ChipUnit[3].SetQuota(5000);
                break;
            case 2:
                ChipUnit[0].SetQuota(1000);
                ChipUnit[1].SetQuota(5000);
                ChipUnit[2].SetQuota(10000);
                ChipUnit[3].SetQuota(100000);
                break;
        }
    }
    public void OpenChipChoose()
    {   //開啟籌碼選擇
        for (int i = 0; i < ChipUnit.Length; i++)
            ChipUnit[i].ChipCanChoose(true);
    }
    public void CloseChipChoose()
    {   //關閉籌碼選擇
        for (int i = 0; i < ChipUnit.Length; i++)
            ChipUnit[i].ChipCanChoose(false);
    }
}
