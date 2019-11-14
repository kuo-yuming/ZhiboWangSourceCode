using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SicBoDiceData
{
    public byte Dice1 = 0;
    public byte Dice2 = 0;
    public byte Dice3 = 0;
    public byte Dice4 = 0;
}

public class SicBoRecentHundred : MonoBehaviour
{
    public SicBoLotteryRecord LotteryRecord;    //開獎記錄
    public UILabel[] RecentHundredUnit; //近百局Label
    private byte[] RecentHundredbyte;   //近百局byte
    public bool ServerHundredUpdate = false;    //收到伺服器近百局名單
    private List<SicBoDiceData> HundredData;    //近百局List
    public BoxCollider CloseButton; //按鈕    

    void Update()
    {
        if (ServerHundredUpdate)
        {   //收到伺服器近百局名單
            ServerHundredUpdate = false;
            DoServerHundredUpdate();
        }
    }

    public void DoServerHundredUpdate()
    {   //收到伺服器傳來名單 更新近百局名單
        HundredData = new List<SicBoDiceData>();
        foreach (var item in SicBoManager.RecentHundredData.m_listAwardList)
        {   //名單轉存到陣列
            SicBoDiceData tmpDice = new SicBoDiceData();
            tmpDice.Dice1 = item.m_byarDiceNumber[0];
            tmpDice.Dice2 = item.m_byarDiceNumber[1];
            tmpDice.Dice3 = item.m_byarDiceNumber[2];
            tmpDice.Dice4 = item.m_byarDiceNumber[3];
            HundredData.Add(tmpDice);
        }
        HundredData.Reverse();  //反轉List 新的在前
        print("百局名單轉換完畢  HundredDataCount = " + HundredData.Count);
        ServerHundredConversion();  //根據根據伺服器給的資料轉換成List資料
    }

    void ServerHundredConversion()
    {   //根據根據伺服器給的資料轉換近百局與開獎記錄
        RecentHundredbyte = new byte[RecentHundredUnit.Length]; //初始化陣列
        LotteryRecord.RecordData = new List<SicBoRecordData>(); //初始化開獎記錄資料
        for (int i = 0; i < RecentHundredbyte.Length; i++)
            RecentHundredbyte[i] = 0;   //byte = 0
        for (int i = 0; i < HundredData.Count; i++)
        {
            if (HundredData[i].Dice1 != 0 && HundredData[i] != null)
            {
                if (HundredData[i].Dice1 == HundredData[i].Dice2 && HundredData[i].Dice1 == HundredData[i].Dice3 && HundredData[i].Dice1 == HundredData[i].Dice4)
                {   //四豹  1111 = 111 四枚 圍骰                    
                    RecentHundredbyte[HundredData[i].Dice1 + 1]++;  //圍骰 1~6 + 1 = 2 ~ 7
                    RecentHundredbyte[8]++;     //任意圍骰
                    RecentHundredbyte[HundredData[i].Dice1 + 8]++;  //四枚 1~6 + 8 = 9 ~ 14
                    RecentHundredbyte[15]++;    //任意四枚
                    RecentHundredbyte[HundredData[i].Dice1 + 29] += 4;  //Number 1~6 + 29 = 30 ~ 35
                    if (HundredData[i].Dice1 != 1 && HundredData[i].Dice1 != 6)
                        RecentHundredbyte[12 + HundredData[i].Dice1 * 3]++; //Total 12 + Dice*3 = 18 21 24 27
                    //開獎記錄資料
                    SicBoRecordData tmpData = new SicBoRecordData();
                    tmpData.RecordDice[0] = HundredData[i].Dice1;   //骰子1
                    tmpData.RecordDice[1] = HundredData[i].Dice2;   //骰子2
                    tmpData.RecordDice[2] = HundredData[i].Dice3;   //骰子3
                    tmpData.RecordDice[3] = HundredData[i].Dice4;   //骰子4
                    tmpData.RecordPoint = 0;    //骰子總和 - 四枚不用顯示
                    tmpData.RecordType = 4; //開獎類型 - 四枚
                    LotteryRecord.RecordData.Add(tmpData);  //將資料加入List
                }
                else if (HundredData[i].Dice1 == HundredData[i].Dice2 && HundredData[i].Dice1 == HundredData[i].Dice3 && HundredData[i].Dice1 != HundredData[i].Dice4 && HundredData[i].Dice4 != 0)
                {   //破骰 1112 = 111 & 112
                    RecentHundredbyte[HundredData[i].Dice1 + 1]++;  //圍骰 1~6 + 1 = 2 ~ 7
                    RecentHundredbyte[8]++;     //任意圍骰
                    RecentHundredbyte[HundredData[i].Dice1 + 29] += 3;  //Number 1~6 + 29 = 30 ~ 35
                    RecentHundredbyte[HundredData[i].Dice4 + 29] += 1;  //Number 1~6 + 29 = 30 ~ 35
                    byte tmpTotal = (byte)(HundredData[i].Dice1 + HundredData[i].Dice2 + HundredData[i].Dice3);
                    if (tmpTotal >= 4 && tmpTotal <= 17)    //Total 1
                        RecentHundredbyte[12 + tmpTotal]++;
                    tmpTotal = (byte)(HundredData[i].Dice1 + HundredData[i].Dice2 + HundredData[i].Dice4);
                    if (tmpTotal >= 4 && tmpTotal <= 17)    //Total 2
                        RecentHundredbyte[12 + tmpTotal]++;
                    if (tmpTotal >= 4 && tmpTotal <= 10)    //小
                        RecentHundredbyte[0]++;
                    else if (tmpTotal >= 11 && tmpTotal <= 17)  //大
                        RecentHundredbyte[1]++;
                    //開獎記錄資料 破骰時 先放 111
                    SicBoRecordData tmpData = new SicBoRecordData();
                    tmpData.RecordDice[0] = HundredData[i].Dice1;   //骰子1
                    tmpData.RecordDice[1] = HundredData[i].Dice2;   //骰子2
                    tmpData.RecordDice[2] = HundredData[i].Dice3;   //骰子3
                    tmpData.RecordDice[3] = HundredData[i].Dice4;   //骰子4
                    tmpData.RecordPoint = 0;//骰子總和 圍骰不用顯示
                    tmpData.RecordType = 7; //開獎類型 - 破骰(圍骰)
                    LotteryRecord.RecordData.Add(tmpData);  //將資料加入List
                    //再放 112
                    tmpData = new SicBoRecordData();
                    tmpData.RecordDice[0] = HundredData[i].Dice1;   //骰子1
                    tmpData.RecordDice[1] = HundredData[i].Dice2;   //骰子2
                    tmpData.RecordDice[2] = HundredData[i].Dice3;   //骰子3
                    tmpData.RecordDice[3] = HundredData[i].Dice4;   //骰子4
                    tmpData.RecordPoint = (byte)(HundredData[i].Dice1 + HundredData[i].Dice2 + HundredData[i].Dice4);   //骰子總和
                    if (tmpData.RecordPoint >= 4 && tmpData.RecordPoint <= 10)
                        tmpData.RecordType = 5; //開獎類型 - 破骰(小)
                    else if (tmpData.RecordPoint >= 11 && tmpData.RecordPoint <= 17)
                        tmpData.RecordType = 6; //開獎類型 - 破骰(大)
                    LotteryRecord.RecordData.Add(tmpData);  //將資料加入List
                }
                else if (HundredData[i].Dice1 == HundredData[i].Dice2 && HundredData[i].Dice1 == HundredData[i].Dice3 && HundredData[i].Dice4 == 0)
                {   //普豹
                    RecentHundredbyte[HundredData[i].Dice1 + 1]++;  //圍骰 1~6 + 1 = 2 ~ 7
                    RecentHundredbyte[8]++;     //任意圍骰
                    RecentHundredbyte[HundredData[i].Dice1 + 29] += 3;  //Number 1~6 + 29 = 30 ~ 35
                    if (HundredData[i].Dice1 != 1 && HundredData[i].Dice1 != 6)
                        RecentHundredbyte[12 + HundredData[i].Dice1 * 3]++; //Total 12 + Dice*3 = 18 21 24 27
                    //開獎記錄資料
                    SicBoRecordData tmpData = new SicBoRecordData();
                    tmpData.RecordDice[0] = HundredData[i].Dice1;   //骰子1
                    tmpData.RecordDice[1] = HundredData[i].Dice2;   //骰子2
                    tmpData.RecordDice[2] = HundredData[i].Dice3;   //骰子3
                    tmpData.RecordDice[3] = HundredData[i].Dice4;   //骰子4
                    tmpData.RecordPoint = 0;//骰子總和 圍骰不用顯示
                    tmpData.RecordType = 3; //開獎類型 - 圍骰
                    LotteryRecord.RecordData.Add(tmpData);  //將資料加入List
                }
                else
                {   //其他
                    RecentHundredbyte[HundredData[i].Dice1 + 29] += 3;  //Number 1~6 + 29 = 30 ~ 35
                    RecentHundredbyte[HundredData[i].Dice2 + 29] += 3;  //Number 1~6 + 29 = 30 ~ 35
                    RecentHundredbyte[HundredData[i].Dice3 + 29] += 3;  //Number 1~6 + 29 = 30 ~ 35
                    byte tmpTotal = (byte)(HundredData[i].Dice1 + HundredData[i].Dice2 + HundredData[i].Dice3);
                    if (tmpTotal >= 4 && tmpTotal <= 17)    //Total
                        RecentHundredbyte[12 + tmpTotal]++;
                    if (tmpTotal >= 4 && tmpTotal <= 10)    //小
                        RecentHundredbyte[0]++;
                    else if (tmpTotal >= 11 && tmpTotal <= 17)  //大
                        RecentHundredbyte[1]++;
                    //開獎記錄資料
                    SicBoRecordData tmpData = new SicBoRecordData();
                    tmpData.RecordDice[0] = HundredData[i].Dice1;   //骰子1
                    tmpData.RecordDice[1] = HundredData[i].Dice2;   //骰子2
                    tmpData.RecordDice[2] = HundredData[i].Dice3;   //骰子3
                    tmpData.RecordDice[3] = HundredData[i].Dice4;   //骰子4
                    tmpData.RecordPoint = (byte)(HundredData[i].Dice1 + HundredData[i].Dice2 + HundredData[i].Dice3);   //骰子總和
                    if (tmpData.RecordPoint >= 4 && tmpData.RecordPoint <= 10)
                        tmpData.RecordType = 1; //開獎類型 - 小
                    else if (tmpData.RecordPoint >= 11 && tmpData.RecordPoint <= 17)
                        tmpData.RecordType = 2; //開獎類型 - 大
                    LotteryRecord.RecordData.Add(tmpData);  //將資料加入List
                }
            }
        }
        //計算最大頁數
        LotteryRecord.InitMaxPage();
    }

    public void ShowRecentHundred()
    {
        CloseButton.enabled = true;
        for (int i = 0; i < RecentHundredbyte.Length; i++)
            RecentHundredUnit[i].text = RecentHundredbyte[i] + "";
    }

    public void HideRecentHundred()
    {
        CloseButton.enabled = false;
    }

    public void UpdateHundredArray(byte dice1, byte dice2, byte dice3, byte dice4)
    {   //更新近百局
        SicBoDiceData tmpDice = new SicBoDiceData();
        tmpDice.Dice1 = dice1;
        tmpDice.Dice2 = dice2;
        tmpDice.Dice3 = dice3;
        tmpDice.Dice4 = dice4;

        //如果資料筆數有一百 刪除舊資料 再新增新資料  否則 直接新增
        if (HundredData.Count == 100)
        {
            HundredConversion(false, HundredData[99]);  //減去最後一筆資料的開獎數
            HundredData.RemoveAt(99);   //刪除最後一筆骰子資料            
        }
        HundredData.Insert(0, tmpDice); //插入新資料
        HundredConversion(true, HundredData[0]);    //增加新資料的開獎數
    }

    void HundredConversion(bool Addition, SicBoDiceData tmpDice)
    {   //根據 增減模式(Addition) 與 骰子內容(tmpDice)，增減 RecentHundredbyte
        if (tmpDice.Dice1 == tmpDice.Dice2 && tmpDice.Dice1 == tmpDice.Dice3 && tmpDice.Dice1 == tmpDice.Dice4)
        {   //四豹  1111 = 111 四枚 圍骰
            if (Addition)
            {   //增加
                RecentHundredbyte[tmpDice.Dice1 + 1]++;  //圍骰 1~6 + 1 = 2 ~ 7
                RecentHundredbyte[8]++;     //任意圍骰
                RecentHundredbyte[tmpDice.Dice1 + 8]++;  //四枚 1~6 + 8 = 9 ~ 14
                RecentHundredbyte[15]++;    //任意四枚
                RecentHundredbyte[tmpDice.Dice1 + 29] += 4;  //Number 1~6 + 29 = 30 ~ 35
                if (tmpDice.Dice1 != 1 && tmpDice.Dice1 != 6)
                    RecentHundredbyte[12 + tmpDice.Dice1 * 3]++; //Total 12 + Dice*3 = 18 21 24 27
            }
            else
            {   //減少
                RecentHundredbyte[tmpDice.Dice1 + 1]--;  //圍骰 1~6 + 1 = 2 ~ 7
                RecentHundredbyte[8]--;     //任意圍骰
                RecentHundredbyte[tmpDice.Dice1 + 8]--;  //四枚 1~6 + 8 = 9 ~ 14
                RecentHundredbyte[15]--;    //任意四枚
                RecentHundredbyte[tmpDice.Dice1 + 29] -= 4;  //Number 1~6 + 29 = 30 ~ 35
                if (tmpDice.Dice1 != 1 && tmpDice.Dice1 != 6)
                    RecentHundredbyte[12 + tmpDice.Dice1 * 3]--; //Total 12 + Dice*3 = 18 21 24 27
            }
        }
        else if (tmpDice.Dice1 == tmpDice.Dice2 && tmpDice.Dice1 == tmpDice.Dice3 && tmpDice.Dice1 != tmpDice.Dice4 && tmpDice.Dice4 != 0)
        {   //破骰 1112 = 111 & 112
            if (Addition)
            {   //增加
                RecentHundredbyte[tmpDice.Dice1 + 1]++;  //圍骰 1~6 + 1 = 2 ~ 7
                RecentHundredbyte[8]++;     //任意圍骰
                RecentHundredbyte[tmpDice.Dice1 + 29] += 3;  //Number 1~6 + 29 = 30 ~ 35
                RecentHundredbyte[tmpDice.Dice4 + 29] += 1;  //Number 1~6 + 29 = 30 ~ 35
                byte tmpTotal = (byte)(tmpDice.Dice1 + tmpDice.Dice2 + tmpDice.Dice3);
                if (tmpTotal >= 4 && tmpTotal <= 17)    //Total 1
                    RecentHundredbyte[12 + tmpTotal]++;
                tmpTotal = (byte)(tmpDice.Dice1 + tmpDice.Dice2 + tmpDice.Dice4);
                if (tmpTotal >= 4 && tmpTotal <= 17)    //Total 2
                    RecentHundredbyte[12 + tmpTotal]++;
                if (tmpTotal >= 4 && tmpTotal <= 10)    //小
                    RecentHundredbyte[0]++;
                else if (tmpTotal >= 11 && tmpTotal <= 17)  //大
                    RecentHundredbyte[1]++;
            }
            else
            {   //減少
                RecentHundredbyte[tmpDice.Dice1 + 1]--;  //圍骰 1~6 + 1 = 2 ~ 7
                RecentHundredbyte[8]--;     //任意圍骰
                RecentHundredbyte[tmpDice.Dice1 + 29] -= 3;  //Number 1~6 + 29 = 30 ~ 35
                RecentHundredbyte[tmpDice.Dice4 + 29] -= 1;  //Number 1~6 + 29 = 30 ~ 35
                byte tmpTotal = (byte)(tmpDice.Dice1 + tmpDice.Dice2 + tmpDice.Dice3);
                if (tmpTotal >= 4 && tmpTotal <= 17)    //Total 1
                    RecentHundredbyte[12 + tmpTotal]--;
                tmpTotal = (byte)(tmpDice.Dice1 + tmpDice.Dice2 + tmpDice.Dice4);
                if (tmpTotal >= 4 && tmpTotal <= 17)    //Total 2
                    RecentHundredbyte[12 + tmpTotal]--;
                if (tmpTotal >= 4 && tmpTotal <= 10)    //小
                    RecentHundredbyte[0]--;
                else if (tmpTotal >= 11 && tmpTotal <= 17)  //大
                    RecentHundredbyte[1]--;

            }
        }
        else if (tmpDice.Dice1 == tmpDice.Dice2 && tmpDice.Dice1 == tmpDice.Dice3 && tmpDice.Dice4 == 0)
        {   //普豹
            if (Addition)
            {   //增加
                RecentHundredbyte[tmpDice.Dice1 + 1]++;  //圍骰 1~6 + 1 = 2 ~ 7
                RecentHundredbyte[8]++;     //任意圍骰
                RecentHundredbyte[tmpDice.Dice1 + 29] += 3;  //Number 1~6 + 29 = 30 ~ 35
                if (tmpDice.Dice1 != 1 && tmpDice.Dice1 != 6)
                    RecentHundredbyte[12 + tmpDice.Dice1 * 3]++; //Total 12 + Dice*3 = 18 21 24 27
            }
            else
            {   //減少
                RecentHundredbyte[tmpDice.Dice1 + 1]--;  //圍骰 1~6 + 1 = 2 ~ 7
                RecentHundredbyte[8]--;     //任意圍骰
                RecentHundredbyte[tmpDice.Dice1 + 29] -= 3;  //Number 1~6 + 29 = 30 ~ 35
                if (tmpDice.Dice1 != 1 && tmpDice.Dice1 != 6)
                    RecentHundredbyte[12 + tmpDice.Dice1 * 3]--; //Total 12 + Dice*3 = 18 21 24 27
            }
        }
        else
        {   //其他
            if (Addition)
            {   //增加
                RecentHundredbyte[tmpDice.Dice1 + 29]++;    //Number 1~6 + 29 = 30 ~ 35
                RecentHundredbyte[tmpDice.Dice2 + 29]++;    //Number 1~6 + 29 = 30 ~ 35
                RecentHundredbyte[tmpDice.Dice3 + 29]++;    //Number 1~6 + 29 = 30 ~ 35
                byte tmpTotal = (byte)(tmpDice.Dice1 + tmpDice.Dice2 + tmpDice.Dice3);
                if (tmpTotal >= 4 && tmpTotal <= 17)    //Total
                    RecentHundredbyte[12 + tmpTotal]++;
                if (tmpTotal >= 4 && tmpTotal <= 10)    //小
                    RecentHundredbyte[0]++;
                else if (tmpTotal >= 11 && tmpTotal <= 17)  //大
                    RecentHundredbyte[1]++;
            }
            else
            {   //減少
                RecentHundredbyte[tmpDice.Dice1 + 29]--;    //Number 1~6 + 29 = 30 ~ 35
                RecentHundredbyte[tmpDice.Dice2 + 29]--;    //Number 1~6 + 29 = 30 ~ 35
                RecentHundredbyte[tmpDice.Dice3 + 29]--;    //Number 1~6 + 29 = 30 ~ 35
                byte tmpTotal = (byte)(tmpDice.Dice1 + tmpDice.Dice2 + tmpDice.Dice3);
                if (tmpTotal >= 4 && tmpTotal <= 17)    //Total
                    RecentHundredbyte[12 + tmpTotal]--;
                if (tmpTotal >= 4 && tmpTotal <= 10)    //小
                    RecentHundredbyte[0]--;
                else if (tmpTotal >= 11 && tmpTotal <= 17)  //大
                    RecentHundredbyte[1]--;
            }
        }
    }
}