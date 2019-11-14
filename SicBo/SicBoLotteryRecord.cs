using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SicBoRecordData
{
    public byte[] RecordDice = new byte[4] { 0, 0, 0, 0 };
    public byte RecordPoint = 0;
    public byte RecordType = 0;
}

public class SicBoLotteryRecord : MonoBehaviour
{
    public SicBoRecentHundred RecentHundred;    //近百局記錄
    public GameObject[] RecordContent;  //記錄內容 開關顯示用
    public UILabel[] RecordNumber;      //記錄編號
    public UISprite[,] RecordDice;      //骰子組成 0:Dices_1、1:Dices_2、2:Dices_3、3:Dices_4
    public GameObject[] RecordPointTrans;   //點數 開關用
    public UISprite[,] RecordPoint;     //點數總和 顯示用 0:Number0、1:Number00
    public UISprite[] RecordType;       //記錄類型 1:小、2:大、3:圍骰、4:四枚 5:破骰(小) 6:破骰(大) 7:破骰(圍骰)
    public List<SicBoRecordData> RecordData;    //開獎記錄資料

    public UILabel NowPageLabel;//目前頁面Label
    private byte NowPage;       //目前頁面
    private byte MinPage = 1;   //第一頁
    private byte MaxPage = 10;   //最後一頁
    public BoxCollider[] PageButton;   //按鈕   

    public void InitLotteryRecord()
    {
        RecordDice = new UISprite[10, 4];   //宣告陣列長度
        RecordPoint = new UISprite[10, 2];  //宣告陣列長度
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 4; j++)
            {   //指定陣列內骰子組成、點數總和的物件
                RecordDice[i, j] = RecordContent[i].transform.GetChild(1).GetChild(j).GetComponent<UISprite>();
                if (j == 0 || j == 1)
                    RecordPoint[i, j] = RecordContent[i].transform.GetChild(2).GetChild(j).GetComponent<UISprite>();
            }
    }

    public void InitMaxPage()
    {   //計算最大頁數
        MaxPage = (RecordData.Count % 10 == 0) ? (byte)(RecordData.Count / 10) : (byte)(RecordData.Count / 10 + 1);
    }

    public void ShowLotteryRecord()
    {   //啟用按鈕
        PageButton[0].enabled = true;
        PageButton[1].enabled = true;
        PageButton[2].enabled = true;
        NowPage = MinPage;  //初始目前頁數
        ShowContent(NowPage);
    }

    public void HideLotteryRecord()
    {   //關閉按鈕
        PageButton[0].enabled = false;
        PageButton[1].enabled = false;
        PageButton[2].enabled = false;
    }

    void NextPage()
    {
        NowPage = (byte)((NowPage == MaxPage) ? MinPage : NowPage + 1);
        ShowContent(NowPage);
    }

    void LastPage()
    {
        NowPage = (byte)((NowPage == MinPage) ? MaxPage : NowPage - 1);
        ShowContent(NowPage);
    }

    void ShowContent(byte PageNumber)
    {   //開啟目前頁面
        NowPageLabel.text = PageNumber.ToString("00") + "/" + MaxPage.ToString("00");  //顯示頁面編號
        PageNumber = (byte)((PageNumber - 1) * 10); //將頁碼轉成資料範圍 例：1 = 0 資料範圍 = 0 + 1~10、5 = 40 資料範圍 = 40 + 1~10
        for (int i = 0; i < 10; i++)
        {
            if (PageNumber + i <= RecordData.Count - 1)
                RecordContent[i].SetActive(true);   //如果資料沒超出上限 顯示該筆內容
            else
                RecordContent[i].SetActive(false);  //否則 隱藏該筆內容
            //如果該筆內容開啟
            if (RecordContent[i].activeSelf)
            {   //設定內容資訊
                RecordNumber[i].text = PageNumber + i + 1 + ""; //設定資料編號
                //如果點數和 = 0 隱藏點數
                if (RecordData[PageNumber + i].RecordPoint == 0)
                    RecordPointTrans[i].SetActive(false);
                else
                {   //否則 顯示並設定點數和
                    RecordPointTrans[i].SetActive(true);    //顯示點數和
                    RecordPoint[i, 1].enabled = (RecordData[PageNumber + i].RecordPoint >= 10); //如果點數和 > 10 顯示十位數
                    RecordPoint[i, 0].spriteName = "numberC_" + (RecordData[PageNumber + i].RecordPoint % 10);  //設定個位數數字
                }
                //顯示骰子點數 與 顯示記錄類型 1:小、2:大、3:圍骰、4:四枚 5:破骰(小) 6:破骰(大) 7:破骰(圍骰)
                switch (RecordData[PageNumber + i].RecordType)
                {
                    case 1: //小
                        RecordType[i].spriteName = "text_small";
                        RecordType[i].MakePixelPerfect();
                        RecordDice[i, 0].spriteName = "numberB1_" + RecordData[PageNumber + i].RecordDice[0];//白
                        RecordDice[i, 1].spriteName = "numberB1_" + RecordData[PageNumber + i].RecordDice[1];//白
                        RecordDice[i, 2].enabled = true;    //顯示
                        RecordDice[i, 2].spriteName = "numberB1_" + RecordData[PageNumber + i].RecordDice[2];//白
                        RecordDice[i, 3].enabled = false;   //隱藏
                        break;
                    case 2: //大
                        RecordType[i].spriteName = "text_big";
                        RecordType[i].MakePixelPerfect();
                        RecordDice[i, 0].spriteName = "numberB1_" + RecordData[PageNumber + i].RecordDice[0];//白
                        RecordDice[i, 1].spriteName = "numberB1_" + RecordData[PageNumber + i].RecordDice[1];//白
                        RecordDice[i, 2].enabled = true;    //顯示
                        RecordDice[i, 2].spriteName = "numberB1_" + RecordData[PageNumber + i].RecordDice[2];//白
                        RecordDice[i, 3].enabled = false;   //隱藏
                        break;
                    case 3: //圍骰
                        RecordType[i].spriteName = "text_triple";
                        RecordType[i].MakePixelPerfect();
                        RecordDice[i, 0].spriteName = "numberB1_" + RecordData[PageNumber + i].RecordDice[0];//白
                        RecordDice[i, 1].spriteName = "numberB1_" + RecordData[PageNumber + i].RecordDice[1];//白
                        RecordDice[i, 2].enabled = true;    //顯示
                        RecordDice[i, 2].spriteName = "numberB1_" + RecordData[PageNumber + i].RecordDice[2];//白
                        RecordDice[i, 3].enabled = false;   //隱藏
                        break;
                    case 4: //四枚
                        RecordType[i].spriteName = "text_quadruple";
                        RecordType[i].MakePixelPerfect();
                        RecordDice[i, 0].spriteName = "numberB3_" + RecordData[PageNumber + i].RecordDice[0];//黃
                        RecordDice[i, 1].spriteName = "numberB3_" + RecordData[PageNumber + i].RecordDice[1];//黃
                        RecordDice[i, 2].enabled = true;    //顯示
                        RecordDice[i, 2].spriteName = "numberB3_" + RecordData[PageNumber + i].RecordDice[2];//黃
                        RecordDice[i, 3].enabled = true;    //顯示
                        RecordDice[i, 3].spriteName = "numberB3_" + RecordData[PageNumber + i].RecordDice[3];//黃
                        break;
                    case 5: //破骰(小)
                        RecordType[i].spriteName = "text_small";
                        RecordType[i].MakePixelPerfect();
                        RecordDice[i, 0].spriteName = "numberB1_" + RecordData[PageNumber + i].RecordDice[0];//白
                        RecordDice[i, 1].spriteName = "numberB1_" + RecordData[PageNumber + i].RecordDice[1];//白
                        RecordDice[i, 2].enabled = false;   //隱藏
                        RecordDice[i, 3].enabled = true;    //顯示
                        RecordDice[i, 3].spriteName = "numberB2_" + RecordData[PageNumber + i].RecordDice[3];//紅
                        break;
                    case 6: //破骰(大)
                        RecordType[i].spriteName = "text_big";
                        RecordType[i].MakePixelPerfect();
                        RecordDice[i, 0].spriteName = "numberB1_" + RecordData[PageNumber + i].RecordDice[0];//白
                        RecordDice[i, 1].spriteName = "numberB1_" + RecordData[PageNumber + i].RecordDice[1];//白
                        RecordDice[i, 2].enabled = false;   //隱藏
                        RecordDice[i, 3].enabled = true;    //顯示
                        RecordDice[i, 3].spriteName = "numberB2_" + RecordData[PageNumber + i].RecordDice[3];//紅
                        break;
                    case 7: //破骰(圍骰)
                        RecordType[i].spriteName = "text_triple";
                        RecordType[i].MakePixelPerfect();
                        RecordDice[i, 0].spriteName = "numberB1_" + RecordData[PageNumber + i].RecordDice[0];//白
                        RecordDice[i, 1].spriteName = "numberB1_" + RecordData[PageNumber + i].RecordDice[1];//白
                        RecordDice[i, 2].enabled = true;    //顯示
                        RecordDice[i, 2].spriteName = "numberB1_" + RecordData[PageNumber + i].RecordDice[2];//白
                        RecordDice[i, 3].enabled = false;   //隱藏
                        break;
                }
            }
        }
    }

    public void UpdateLotteryRecord(byte dice1, byte dice2, byte dice3, byte dice4)
    {
        if (RecordData.Count == 100)    //如果資料筆數 = 100 先刪除最後一筆
            RecordData.RemoveAt(99);
        else if (RecordData.Count > 100)    //如果資料筆數 > 100
        {   //且最後兩筆 都為破骰 
            if (RecordData[RecordData.Count - 1].RecordType > 4 && RecordData[RecordData.Count - 2].RecordType > 4)
            {   //刪除最後兩筆                
                RecordData.RemoveAt(RecordData.Count - 1);
                RecordData.RemoveAt(RecordData.Count - 2);                
            }
            else //否則只刪除一筆
                RecordData.RemoveAt(RecordData.Count - 1);
        }
        //加入新資料
        SicBoRecordData tmpData = new SicBoRecordData();       
        if (dice1 == dice2 && dice1 == dice3 && dice1 == dice4)
        {   //四豹  1111 = 111 四枚 圍骰
            tmpData.RecordDice[0] = dice1;  //骰子1
            tmpData.RecordDice[1] = dice2;  //骰子2
            tmpData.RecordDice[2] = dice3;  //骰子3
            tmpData.RecordDice[3] = dice4;  //骰子4
            tmpData.RecordPoint = 0;//骰子總和 - 四枚不用顯示
            tmpData.RecordType = 4; //開獎類型 - 四枚
            RecordData.Insert(0, tmpData);  //加入資料            
        }
        else if (dice1 == dice2 && dice1 == dice3 && dice1 != dice4 && dice4 != 0)
        {   //破骰 1112 = 111 & 112
            //先放 112
            tmpData.RecordDice[0] = dice1;  //骰子1
            tmpData.RecordDice[1] = dice2;  //骰子2
            tmpData.RecordDice[2] = dice3;  //骰子3
            tmpData.RecordDice[3] = dice4;  //骰子4
            tmpData.RecordPoint = (byte)(dice1 + dice2 + dice4);    //骰子總和
            if (tmpData.RecordPoint >= 4 && tmpData.RecordPoint <= 10)
                tmpData.RecordType = 5; //開獎類型 - 破骰(小)
            else if (tmpData.RecordPoint >= 11 && tmpData.RecordPoint <= 17)
                tmpData.RecordType = 6; //開獎類型 - 破骰(大)
            RecordData.Insert(0, tmpData);  //加入資料
            //再放 111
            tmpData = new SicBoRecordData();
            tmpData.RecordDice[0] = dice1;  //骰子1
            tmpData.RecordDice[1] = dice2;  //骰子2
            tmpData.RecordDice[2] = dice3;  //骰子3
            tmpData.RecordDice[3] = dice4;  //骰子4
            tmpData.RecordPoint = 0;//骰子總和 圍骰不用顯示
            tmpData.RecordType = 7; //開獎類型 - 破骰(圍骰)
            RecordData.Insert(0, tmpData);  //加入資料            
        }
        else if (dice1 == dice2 && dice1 == dice3 && dice4 == 0)
        {   //普豹
            tmpData.RecordDice[0] = dice1;  //骰子1
            tmpData.RecordDice[1] = dice2;  //骰子2
            tmpData.RecordDice[2] = dice3;  //骰子3
            tmpData.RecordDice[3] = dice4;  //骰子4
            tmpData.RecordPoint = 0;//骰子總和 圍骰不用顯示
            tmpData.RecordType = 3; //開獎類型 - 圍骰
            RecordData.Insert(0, tmpData);  //加入資料            
        }
        else
        {   //其他
            tmpData.RecordDice[0] = dice1;  //骰子1
            tmpData.RecordDice[1] = dice2;  //骰子2
            tmpData.RecordDice[2] = dice3;  //骰子3
            tmpData.RecordDice[3] = dice4;  //骰子4
            tmpData.RecordPoint = (byte)(dice1 + dice2 + dice3);    //骰子總和
            if (tmpData.RecordPoint >= 4 && tmpData.RecordPoint <= 10)
                tmpData.RecordType = 1; //開獎類型 - 小
            else if (tmpData.RecordPoint >= 11 && tmpData.RecordPoint <= 17)
                tmpData.RecordType = 2; //開獎類型 - 大
            RecordData.Insert(0, tmpData);  //加入資料            
        }
        //計算最大頁數
        MaxPage = (RecordData.Count % 10 == 0) ? (byte)(RecordData.Count / 10) : (byte)(RecordData.Count / 10 + 1);        
    }
}
