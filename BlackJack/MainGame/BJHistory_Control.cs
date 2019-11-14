using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WinLoseListClass;
using HistorySaveData;

namespace HistorySaveData
{
    public class HistoryData
    {
        public byte BankerPoint = 0;//莊家點數
        public byte PlayerPoint = 0;//玩家點數
        public WinLoseList m_WinLoseList = WinLoseList.NoCheck;//輸贏結果
        public long WinLoseMoney = 0;//最後輸贏金
        public bool SPLIT_Bool = false;//是否為分牌
    }
}

public class BJHistory_Control : MonoBehaviour {
    public static bool HistoryOpen_Bool = false;//開啟歷史紀錄
    public static uint HistoryPage = 1;//玩家頁面
    public static uint TotalHistoryPage = 1;//目前總頁面
    public static Dictionary<uint, HistoryData> History_Dic = new Dictionary<uint, HistoryData>();
    public static Dictionary<uint, HistoryData> History_DicSave = new Dictionary<uint, HistoryData>();
    public UILabel Page_Label;
    public UILabel TotalPage_Label;
    public static bool PagePlanning_Bool = false;
    public static bool HistorySaveOK_Bool = false;
    public GameObject HistoryList_Object;
    public GameObject InstantiateSeat;
    public GameObject HistoryMain_Object;
    public UIGrid SeatGrid;
    public static uint HistoryNowNumber = 0;

    bool Init_Bool = false;
    // Use this for initialization
    void Start()
    {
        HistoryOpen_Bool = false;
        HistoryPage = 1;
        TotalHistoryPage = 1;
        History_Dic.Clear();
        History_DicSave.Clear();
        PagePlanning_Bool = false;
        HistorySaveOK_Bool = false;
        DeleteList();
        HistoryNowNumber = 0;

        // 
        //for (byte i = 0; i < 20; i++)
        //{
        //    HistoryData Data = new HistoryData();
        //    Data.BankerPoint = i;
        //    Data.PlayerPoint = 20;
        //    Data.m_WinLoseList = WinLoseList.WinDraw;
        //    Data.SPLIT_Bool = false;
        //    Data.WinLoseMoney = 0;
        //    History_Dic.Add(HistoryNowNumber, Data);
        //    HistoryNowNumber++;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        Page_Label.text = HistoryPage.ToString();
        TotalPage_Label.text = TotalHistoryPage.ToString();

        if (!HistoryOpen_Bool)
        {
            HistoryMain_Object.SetActive(false);
            Init_Bool = true;
        }
        else
        {
            HistoryMain_Object.SetActive(true);
        }

        if (Init_Bool)
        {
            DeleteList();
            Init_Bool = false;
        }

        if (History_Dic == null)
        {
            HistoryPage = 1;
            TotalHistoryPage = 1;
        }
        else
        {
            //總頁面計算
            if (History_Dic.Count < 8)
            {
                TotalHistoryPage = 1;
            }
            else
            {
                TotalHistoryPage = (uint)((History_Dic.Count / 8) + 1);
            }

            //暫存檔案處理
            if (!PagePlanning_Bool && HistorySaveOK_Bool)
            {
                foreach (var item in History_DicSave)
                {
                    HistoryData Data = new HistoryData();
                    Data.BankerPoint = item.Value.BankerPoint;
                    Data.PlayerPoint = item.Value.PlayerPoint;
                    Data.m_WinLoseList = item.Value.m_WinLoseList;
                    Data.SPLIT_Bool = item.Value.SPLIT_Bool;
                    Data.WinLoseMoney = item.Value.WinLoseMoney;
                    History_Dic.Add(HistoryNowNumber, Data);
                    HistoryNowNumber++;
                }
                if (HistoryOpen_Bool)
                {
                    PagePlanning_Bool = true;
                }
                HistorySaveOK_Bool = false;
                History_DicSave.Clear();
            }

            //生成
            #region Add
            if (PagePlanning_Bool)
            {
                DeleteList();
                if (History_Dic != null)
                {
                    List<uint> Data = new List<uint>(History_Dic.Keys);
                    Data.Sort();
                    Data.Reverse();
                    uint AddNumber = 0;
                    uint StartNumber = (HistoryPage - 1) * 8;
                    byte ListNumber = 0;
                    foreach (var item in Data)
                    {
                        if (AddNumber >= StartNumber && ListNumber < 8)
                        {
                            GameObject AddData = Instantiate(HistoryList_Object);
                            AddData.transform.parent = InstantiateSeat.transform;
                            AddData.name = "ListObject";
                            AddData.transform.localScale = new Vector3(1, 1, 1);
                            AddData.transform.localPosition = this.transform.localPosition;
                            HistoryList Data_Control = AddData.GetComponent<HistoryList>();
                            Data_Control.BankerLabel.text = History_Dic[item].BankerPoint.ToString();

                            //玩家資料
                            if (History_Dic[item].SPLIT_Bool)
                            {
                                if (History_Dic[item].m_WinLoseList == WinLoseList.BlackJack)
                                    Data_Control.PlayerLabel.text = "(分牌)" + "Black Jack";
                                else
                                    Data_Control.PlayerLabel.text = "(分牌)" + History_Dic[item].PlayerPoint;
                            }
                            else
                            {
                                if (History_Dic[item].m_WinLoseList == WinLoseList.BlackJack)
                                    Data_Control.PlayerLabel.text = "Black Jack";
                                else
                                    Data_Control.PlayerLabel.text = History_Dic[item].PlayerPoint.ToString();
                            }

                            //金額結算
                            if (History_Dic[item].WinLoseMoney < 0)
                            {
                                Data_Control.MoneyLabel.color = new Color32(255, 75, 75, 255);
                                Data_Control.MoneyLabel.text = History_Dic[item].WinLoseMoney.ToString();
                            }
                            else
                            {
                                Data_Control.MoneyLabel.color = new Color32(114, 255, 255, 255);
                                Data_Control.MoneyLabel.text = "+" + History_Dic[item].WinLoseMoney;
                            }

                            ListNumber++;
                        }
                        AddNumber++;
                    }
                    SeatGrid.enabled = true;
                }
                PagePlanning_Bool = false;
            }
            #endregion
        }
    }

    void DeleteList()
    {
        Transform[] Objs = InstantiateSeat.GetComponentsInChildren<Transform>();
        int Len = Objs.Length;
        for (int i = 0; i < Len; i++)
        {
            if (Objs[i].name == "ListObject")
            {
                Destroy(Objs[i].gameObject);
            }
        }
    }
}
