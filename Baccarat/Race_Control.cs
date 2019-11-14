using UnityEngine;
using System.Collections;
using GameCore.Manager.Common;
using GameCore;
using GameCore.Machine;
using System.Collections.Generic;
using System;

public class Race_Control : MonoBehaviour {
    public GameObject EasyObject;
    public GameObject NormalObject;
    public static Dictionary<ushort, CRaceRanking> RaceRankingList = new Dictionary<ushort, CRaceRanking>();
    public static Dictionary<ushort, GameObject> RankingObject = new Dictionary<ushort, GameObject>();
    public static uint NowPlayerRanking = 0;
    public static bool RaceRankingOpen = false;
    public static bool RaceRankingCheck = false;
    public static uint TotalPeople = 0;
    public static ushort RankingNumber = 0;
    public static bool RaceEnd_Bool = false;

    public UILabel[] Ranking1;
    public UILabel[] Ranking2;
    public UILabel[] Ranking3;

    public UILabel[] MyRankDataLabel;
    public static Int64 NowPlayerScore = 0;
    public UIScrollBar RankingBar;
    public UIScrollView m_ScrollView;

    public GameObject Racking_Object;
    public UITable RacnkingTable;
    public static uint GetMoney = 0;
    public static uint GetDiamond = 0;
    public GameObject EndObject;
    public UILabel Money;
    public UILabel Diamond;
    bool ScrollBarCheckBool = false;
    float DelayTime = 0;
    // Use this for initialization
    void Start () {
        RaceRankingList.Clear();
        NowPlayerRanking = 0;
        NowPlayerScore = 0;
        RaceRankingOpen = false;
        RaceRankingCheck = false;
        Ranking1[0].enabled = false;
        Ranking1[1].enabled = false;
        Ranking2[0].enabled = false;
        Ranking2[1].enabled = false;
        Ranking3[0].enabled = false;
        Ranking3[1].enabled = false;
        RaceEnd_Bool = false;
        GetMoney = 0;
        GetDiamond = 0;
        MyRankDataLabel[1].text = NowPlayerRanking.ToString();
        MyRankDataLabel[3].text = NowPlayerScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Competition.RaceGame_Bool)
        {
            EasyObject.SetActive(false);
            NormalObject.SetActive(false);
            Ranking1[0].enabled = false;
            Ranking1[1].enabled = false;
            Ranking2[0].enabled = false;
            Ranking2[1].enabled = false;
            Ranking3[0].enabled = false;
            Ranking3[1].enabled = false;
        }
        else
        {
            EasyObject.SetActive(true);
            
            if (RaceEnd_Bool && MainGame_Control.StopModeState == GameEnum.ENUM_STOPMODE_STATE.EndShow && !EndWindow_Control.EndWindowOpenBool)
            {
                EndObject.SetActive(true);
                NormalObject.SetActive(true);
                RaceRankingOpen = false;
                //   EndWindow_Control.EndWindowOpenBool = false;
            }
            else
            {
                EndObject.SetActive(false);
                if (!RaceRankingOpen)
                {
                    NormalObject.SetActive(false);
                }
                else
                {
                    NormalObject.SetActive(true);
                }
            }

            if (RaceRankingList.Count < 6)
            {
                RankingBar.barSize = 1;
                RankingBar.value = 0;
            }
          
            MyRankDataLabel[0].text = TotalPeople.ToString();
            MyRankDataLabel[2].text = MainConnet.m_PlayerData.m_strNickName;
            Money.text = GetMoney.ToString();
            Diamond.text = GetDiamond.ToString();
            if (RaceRankingCheck && (MainGame_Control.StopModeState == GameEnum.ENUM_STOPMODE_STATE.WaitStop || MainGame_Control.StopModeState == GameEnum.ENUM_STOPMODE_STATE.EndShow))
            {
                MyRankDataLabel[1].text = NowPlayerRanking.ToString();   
                MyRankDataLabel[3].text = NowPlayerScore.ToString();
                DataCheck_void();
                ObjcetAdd_void();
                RaceRankingCheck = false;
            }
        }

        if (ScrollBarCheckBool)
        {
            if (DelayTime < 1f)
            {
                DelayTime += Time.deltaTime;
                RacnkingTable.repositionNow = true;
                RankingBar.value = 0;
                m_ScrollView.UpdatePosition();
                m_ScrollView.UpdateScrollbars();
            }
            else
            {
                DelayTime = 0;
                ScrollBarCheckBool = false;
            }
        }
    }

    void DataCheck_void()
    {
        if (RaceRankingList == null || RaceRankingList.Count == 0)
        {
            Ranking1[0].enabled = false;
            Ranking1[1].enabled = false;
            Ranking2[0].enabled = false;
            Ranking2[1].enabled = false;
            Ranking3[0].enabled = false;
            Ranking3[1].enabled = false;
        }
        else if (RaceRankingList.Count == 1)
        {
            Ranking1[0].enabled = true;
            Ranking1[1].enabled = true;
            Ranking2[0].enabled = false;
            Ranking2[1].enabled = false;
            Ranking3[0].enabled = false;
            Ranking3[1].enabled = false;
            Ranking1[0].text = RaceRankingList[1].m_strNickName;
            Ranking1[1].text = RaceRankingList[1].m_i64Score.ToString();
        }
        else if (RaceRankingList.Count == 2)
        {
            Ranking1[0].enabled = true;
            Ranking1[1].enabled = true;
            Ranking2[0].enabled = true;
            Ranking2[1].enabled = true;
            Ranking3[0].enabled = false;
            Ranking3[1].enabled = false;
            Ranking1[0].text = RaceRankingList[1].m_strNickName;
            Ranking1[1].text = RaceRankingList[1].m_i64Score.ToString();
            Ranking2[0].text = RaceRankingList[2].m_strNickName;
            Ranking2[1].text = RaceRankingList[2].m_i64Score.ToString();
        }
        else
        {
            Ranking1[0].enabled = true;
            Ranking1[1].enabled = true;
            Ranking2[0].enabled = true;
            Ranking2[1].enabled = true;
            Ranking3[0].enabled = true;
            Ranking3[1].enabled = true;
            Ranking1[0].text = RaceRankingList[1].m_strNickName;
            Ranking1[1].text = RaceRankingList[1].m_i64Score.ToString();
            Ranking2[0].text = RaceRankingList[2].m_strNickName;
            Ranking2[1].text = RaceRankingList[2].m_i64Score.ToString();
            Ranking3[0].text = RaceRankingList[3].m_strNickName;
            Ranking3[1].text = RaceRankingList[3].m_i64Score.ToString();
        }
    }

    void ObjcetAdd_void()
    {
        if (RankingObject.Count != 0)
        {
            foreach (GameObject item in RankingObject.Values)
            {
                Destroy(item);
            }
            RankingObject.Clear();
        }

        List<CRaceRanking> SaveDataList = new List<CRaceRanking>(RaceRankingList.Values);

        SaveDataList.Sort(delegate (CRaceRanking x, CRaceRanking y)
        {
            if (x.m_uiRank == 0 && y.m_uiRank == 0) return 0;
            else if (x.m_uiRank == 0) return -1;
            else if (y.m_uiRank == 0) return 1;
            else return x.m_uiRank.CompareTo(y.m_uiRank);
        });

        ushort Number = 0;
        foreach (var item in SaveDataList)
        {
            GameObject Data = Instantiate(Racking_Object);
            Data.transform.parent = RacnkingTable.transform;
            Data.transform.localScale = new Vector3(1, 1, 1);
            if (Number < 10)
            {
                Data.name = "0" + Number.ToString();
            }
            else
            {
                Data.name = Number.ToString();
            }
            RackingOD m_RackingOD = Data.GetComponent<RackingOD>();
            m_RackingOD.Rank_Label.text = item.m_uiRank.ToString();
            m_RackingOD.Name_Label.text = item.m_strNickName.ToString();
            m_RackingOD.Score_Label.text = item.m_i64Score.ToString();
            RankingObject.Add(Number, Data);
            Number++;
        }
        ScrollBarCheckBool = true;
    }
}
