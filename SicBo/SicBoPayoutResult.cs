using UnityEngine;
using System.Collections;
using GameCore.Manager.SicBo;

public class SicBoPayoutResult : MonoBehaviour
{
    public SicBoBetAreaControl BetAreaControl;  //押注區控制
    public UIPanel m_Panel; //派彩結果面板
    public GameObject m_Mask;   //遮罩
    public UISprite TimeSprite; //倒數圖片
    public UILabel[] BetLabel;  //押注Label
    public UILabel[] PayLabel;  //派彩Label
    public UILabel[] TotalLabel;//總和Label
    private uint[] BetInt;  //押注計算
    private uint[] PayInt;  //派彩計算
    private int[] TotalInt; //總和計算
    private float Timer;    //計時器
    private uint[] BetMoney;//押注金額
    private uint[] PayMoney;//派彩金額

    // Update is called once per frame
    void Update()
    {
        if (Timer != 0)
        {
            Timer -= Time.deltaTime;
            TimeSprite.spriteName = "numberD_" + (byte)Timer;
            if (Timer < 0.5f)
            {
                Timer = 0.0f;
                ClickClose();
            }
        }
    }

    public void ShowPanel()
    {
        m_Panel.enabled = true; //打開面板
        m_Mask.SetActive(true); //打開遮罩
        Timer = 5.5f;   //開始計時
    }

    public void ClickClose()
    {
        Timer = 0.0f;   //計時器歸零
        m_Panel.enabled = false;    //關閉面板
        m_Mask.SetActive(false);    //關閉遮罩
    }

    public void CheckBet()
    {
        BetMoney = new uint[BetAreaControl.BetUnit.Length]; //宣告陣列
        BetInt = new uint[BetLabel.Length];
        for (int i = 0; i < BetAreaControl.BetUnit.Length; i++)
        {   //紀錄押注金額
            if (BetAreaControl.BetUnit[i].MyChipBack.enabled)
                BetMoney[i] = BetAreaControl.BetUnit[i].MyChipUint;
            else
                BetMoney[i] = 0;
        }
        //設定押注結果
        BetInt[0] = BetMoney[1];    //大
        BetInt[1] = BetMoney[0];    //小        
        uint tmpMoney = 0;  //點數和
        for (int i = 16; i <= 29; i++) tmpMoney += BetMoney[i]; //累加
        BetInt[2] = tmpMoney;   //設定點數和
        tmpMoney = 0;   //單骰
        for (int i = 30; i <= 35; i++) tmpMoney += BetMoney[i]; //累加
        BetInt[3] = tmpMoney;   //設定單骰
        BetInt[4] = BetMoney[8];    //任意圍骰
        tmpMoney = 0;   //圍骰
        for (int i = 2; i <= 7; i++) tmpMoney += BetMoney[i]; //累加
        BetInt[5] = tmpMoney;   //設定圍骰
        BetInt[6] = BetMoney[15];  //任意四枚
        tmpMoney = 0;   //四枚
        for (int i = 9; i <= 14; i++) tmpMoney += BetMoney[i]; //累加
        BetInt[7] = tmpMoney;   //設定四枚
        tmpMoney = 0;   //總和
        for (int i = 0; i < BetLabel.Length - 1; i++) tmpMoney += BetInt[i]; //累加
        BetInt[8] = tmpMoney;   //設定總和
        for (int i = 0; i < BetLabel.Length; i++) BetLabel[i].text = BetInt[i] + "";    //輸出金額
    }

    public void CheckPayout()
    {
        PayMoney = new uint[BetAreaControl.BetUnit.Length]; //宣告陣列
        PayInt = new uint[PayLabel.Length];
        foreach (var item in SicBoManager.NoitfyAwardData.m_listAwardAreaID)
        {   //根據得獎區域計算彩金
            switch (item.m_byAwardAreaID)
            {
                case (byte)ENUM_SicBo_AWARD_AREA.SumSmall:  //小
                    if (BetMoney[0] != 0)
                        PayMoney[0] = BetMoney[0] * SicBoManager.Table_usOdds[23];
                    break;
                case (byte)ENUM_SicBo_AWARD_AREA.SumBig:  //大
                    if (BetMoney[1] != 0)
                        PayMoney[1] = BetMoney[1] * SicBoManager.Table_usOdds[22];
                    break;
                case (byte)ENUM_SicBo_AWARD_AREA.AnyTriple:  //任意圍骰
                    if (BetMoney[8] != 0)
                        PayMoney[8] = BetMoney[8] * SicBoManager.Table_usOdds[20];
                    break;
                case (byte)ENUM_SicBo_AWARD_AREA.EachTriple: //圍骰
                    if (item.m_byAwardNumber == 1 && BetMoney[2] != 0)
                        PayMoney[2] = BetMoney[2] * SicBoManager.Table_usOdds[18];
                    else if (item.m_byAwardNumber == 2 && BetMoney[3] != 0)
                        PayMoney[3] = BetMoney[3] * SicBoManager.Table_usOdds[18];
                    else if (item.m_byAwardNumber == 3 && BetMoney[4] != 0)
                        PayMoney[4] = BetMoney[4] * SicBoManager.Table_usOdds[18];
                    else if (item.m_byAwardNumber == 4 && BetMoney[5] != 0)
                        PayMoney[5] = BetMoney[5] * SicBoManager.Table_usOdds[18];
                    else if (item.m_byAwardNumber == 5 && BetMoney[6] != 0)
                        PayMoney[6] = BetMoney[6] * SicBoManager.Table_usOdds[18];
                    else if (item.m_byAwardNumber == 6 && BetMoney[7] != 0)
                        PayMoney[7] = BetMoney[7] * SicBoManager.Table_usOdds[18];
                    break;
                case (byte)ENUM_SicBo_AWARD_AREA.AnyQuadruple:  //任意四枚
                    if (BetMoney[15] != 0)
                        PayMoney[15] = BetMoney[15] * SicBoManager.Table_usOdds[21];
                    break;
                case (byte)ENUM_SicBo_AWARD_AREA.EachQuadruple: //四枚
                    if (item.m_byAwardNumber == 1 && BetMoney[9] != 0)
                        PayMoney[9] = BetMoney[9] * SicBoManager.Table_usOdds[19];
                    else if (item.m_byAwardNumber == 2 && BetMoney[10] != 0)
                        PayMoney[10] = BetMoney[10] * SicBoManager.Table_usOdds[19];
                    else if (item.m_byAwardNumber == 3 && BetMoney[11] != 0)
                        PayMoney[11] = BetMoney[11] * SicBoManager.Table_usOdds[19];
                    else if (item.m_byAwardNumber == 4 && BetMoney[12] != 0)
                        PayMoney[12] = BetMoney[12] * SicBoManager.Table_usOdds[19];
                    else if (item.m_byAwardNumber == 5 && BetMoney[13] != 0)
                        PayMoney[13] = BetMoney[13] * SicBoManager.Table_usOdds[19];
                    else if (item.m_byAwardNumber == 6 && BetMoney[14] != 0)
                        PayMoney[14] = BetMoney[14] * SicBoManager.Table_usOdds[19];
                    break;
                case (byte)ENUM_SicBo_AWARD_AREA.SumFour:   //4
                    if (BetMoney[16] != 0)
                        PayMoney[16] = BetMoney[16] * SicBoManager.Table_usOdds[4];
                    break;
                case (byte)ENUM_SicBo_AWARD_AREA.SumFive:   //5
                    if (BetMoney[17] != 0)
                        PayMoney[17] = BetMoney[17] * SicBoManager.Table_usOdds[5];
                    break;
                case (byte)ENUM_SicBo_AWARD_AREA.SumSix:    //6
                    if (BetMoney[18] != 0)
                        PayMoney[18] = BetMoney[18] * SicBoManager.Table_usOdds[6];
                    break;
                case (byte)ENUM_SicBo_AWARD_AREA.SumSeven:  //7
                    if (BetMoney[19] != 0)
                        PayMoney[19] = BetMoney[19] * SicBoManager.Table_usOdds[7];
                    break;
                case (byte)ENUM_SicBo_AWARD_AREA.SumEight:  //8
                    if (BetMoney[20] != 0)
                        PayMoney[20] = BetMoney[20] * SicBoManager.Table_usOdds[8];
                    break;
                case (byte)ENUM_SicBo_AWARD_AREA.SumNine:    //9
                    if (BetMoney[21] != 0)
                        PayMoney[21] = BetMoney[21] * SicBoManager.Table_usOdds[9];
                    break;
                case (byte)ENUM_SicBo_AWARD_AREA.SumTen:    //10
                    if (BetMoney[22] != 0)
                        PayMoney[22] = BetMoney[22] * SicBoManager.Table_usOdds[10];
                    break;
                case (byte)ENUM_SicBo_AWARD_AREA.SumEleven: //11
                    if (BetMoney[23] != 0)
                        PayMoney[23] = BetMoney[23] * SicBoManager.Table_usOdds[11];
                    break;
                case (byte)ENUM_SicBo_AWARD_AREA.SumTwelve: //12
                    if (BetMoney[24] != 0)
                        PayMoney[24] = BetMoney[24] * SicBoManager.Table_usOdds[12];
                    break;
                case (byte)ENUM_SicBo_AWARD_AREA.SumThirteen:   //13
                    if (BetMoney[25] != 0)
                        PayMoney[25] = BetMoney[25] * SicBoManager.Table_usOdds[13];
                    break;
                case (byte)ENUM_SicBo_AWARD_AREA.SumFourteen:   //14
                    if (BetMoney[26] != 0)
                        PayMoney[26] = BetMoney[26] * SicBoManager.Table_usOdds[14];
                    break;
                case (byte)ENUM_SicBo_AWARD_AREA.SumFifteen:    //15
                    if (BetMoney[27] != 0)
                        PayMoney[27] = BetMoney[27] * SicBoManager.Table_usOdds[15];
                    break;
                case (byte)ENUM_SicBo_AWARD_AREA.SumSixteen:    //16
                    if (BetMoney[28] != 0)
                        PayMoney[28] = BetMoney[28] * SicBoManager.Table_usOdds[16];
                    break;
                case (byte)ENUM_SicBo_AWARD_AREA.SumSeventeen:  //17
                    if (BetMoney[29] != 0)
                        PayMoney[29] = BetMoney[29] * SicBoManager.Table_usOdds[17];
                    break;
                case (byte)ENUM_SicBo_AWARD_AREA.OneDice:   //任意單骰
                    if (item.m_byAwardNumber == 1 && BetMoney[30] != 0)
                        PayMoney[30] = BetMoney[30] * SicBoManager.Table_usOdds[1];
                    else if (item.m_byAwardNumber == 2 && BetMoney[31] != 0)
                        PayMoney[31] = BetMoney[31] * SicBoManager.Table_usOdds[1];
                    else if (item.m_byAwardNumber == 3 && BetMoney[32] != 0)
                        PayMoney[32] = BetMoney[32] * SicBoManager.Table_usOdds[1];
                    else if (item.m_byAwardNumber == 4 && BetMoney[33] != 0)
                        PayMoney[33] = BetMoney[33] * SicBoManager.Table_usOdds[1];
                    else if (item.m_byAwardNumber == 5 && BetMoney[34] != 0)
                        PayMoney[34] = BetMoney[34] * SicBoManager.Table_usOdds[1];
                    else if (item.m_byAwardNumber == 6 && BetMoney[35] != 0)
                        PayMoney[35] = BetMoney[35] * SicBoManager.Table_usOdds[1];
                    break;
                case (byte)ENUM_SicBo_AWARD_AREA.TwoDice:   //任意雙骰
                    if (item.m_byAwardNumber == 1 && BetMoney[30] != 0)
                        PayMoney[30] = BetMoney[30] * SicBoManager.Table_usOdds[2];
                    else if (item.m_byAwardNumber == 2 && BetMoney[31] != 0)
                        PayMoney[31] = BetMoney[31] * SicBoManager.Table_usOdds[2];
                    else if (item.m_byAwardNumber == 3 && BetMoney[32] != 0)
                        PayMoney[32] = BetMoney[32] * SicBoManager.Table_usOdds[2];
                    else if (item.m_byAwardNumber == 4 && BetMoney[33] != 0)
                        PayMoney[33] = BetMoney[33] * SicBoManager.Table_usOdds[2];
                    else if (item.m_byAwardNumber == 5 && BetMoney[34] != 0)
                        PayMoney[34] = BetMoney[34] * SicBoManager.Table_usOdds[2];
                    else if (item.m_byAwardNumber == 6 && BetMoney[35] != 0)
                        PayMoney[35] = BetMoney[35] * SicBoManager.Table_usOdds[2];
                    break;
                case (byte)ENUM_SicBo_AWARD_AREA.ThreeDice: //任意圍骰
                    if (item.m_byAwardNumber == 1 && BetMoney[30] != 0)
                        PayMoney[30] = BetMoney[30] * SicBoManager.Table_usOdds[3];
                    else if (item.m_byAwardNumber == 2 && BetMoney[31] != 0)
                        PayMoney[31] = BetMoney[31] * SicBoManager.Table_usOdds[3];
                    else if (item.m_byAwardNumber == 3 && BetMoney[32] != 0)
                        PayMoney[32] = BetMoney[32] * SicBoManager.Table_usOdds[3];
                    else if (item.m_byAwardNumber == 4 && BetMoney[33] != 0)
                        PayMoney[33] = BetMoney[33] * SicBoManager.Table_usOdds[3];
                    else if (item.m_byAwardNumber == 5 && BetMoney[34] != 0)
                        PayMoney[34] = BetMoney[34] * SicBoManager.Table_usOdds[3];
                    else if (item.m_byAwardNumber == 6 && BetMoney[35] != 0)
                        PayMoney[35] = BetMoney[35] * SicBoManager.Table_usOdds[3];
                    break;
            }
        }
        //計算彩金總和
        PayInt[0] = PayMoney[1];    //大
        PayInt[1] = PayMoney[0];    //小
        uint tmpMoney = 0;  //點數和
        for (int i = 16; i <= 29; i++) tmpMoney += PayMoney[i]; //累加
        PayInt[2] = tmpMoney;   //設定點數和
        tmpMoney = 0;   //單骰
        for (int i = 30; i <= 35; i++) tmpMoney += PayMoney[i]; //累加
        PayInt[3] = tmpMoney;   //設定單骰
        PayInt[4] = PayMoney[8];    //任意圍骰
        tmpMoney = 0;   //圍骰
        for (int i = 2; i <= 7; i++) tmpMoney += PayMoney[i]; //累加
        PayInt[5] = tmpMoney;   //設定圍骰
        PayInt[6] = PayMoney[15];  //任意四枚
        tmpMoney = 0;   //四枚
        for (int i = 9; i <= 14; i++) tmpMoney += PayMoney[i]; //累加
        PayInt[7] = tmpMoney;   //設定四枚
        tmpMoney = 0;   //總和
        for (int i = 0; i < PayLabel.Length - 1; i++) tmpMoney += PayInt[i]; //累加
        PayInt[8] = tmpMoney;   //設定總和
        for (int i = 0; i < PayLabel.Length; i++) PayLabel[i].text = PayInt[i] + "";    //輸出金額

        //計算總和
        CheckTotal();
    }

    public void CheckTotal()
    {
        TotalInt = new int[TotalLabel.Length];  //宣告陣列
        for (int i = 0; i < TotalLabel.Length - 1; i++) //計算總和
            TotalInt[i] = (int)PayInt[i] - (int)BetInt[i];
        for (int i = 0; i < TotalLabel.Length - 1; i++) //計算總和的總和
            TotalInt[8] += (int)TotalInt[i];

        for (int i = 0; i < TotalLabel.Length; i++) //輸出金額
            TotalLabel[i].text = TotalInt[i] + "";
        //根據玩家盈餘改變字體顏色
        if (TotalInt[8] >= 0)
            TotalLabel[8].color = new Color32(0, 153, 0, 255);
        else
            TotalLabel[8].color = new Color32(255, 0, 85, 255);
    }
}