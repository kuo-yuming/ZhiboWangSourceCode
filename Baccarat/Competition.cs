using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore.Machine;

public class Competition : MonoBehaviour
{
    public static bool CompetitionBoxOpen_Bool = false;
    public static bool BeforeRankingBoxOpen_Bool = false;
    public GameObject Competition_Object;
    public UITable CompetitionTable;
    public GameObject BeforeRanking_Object;
    public static Dictionary<ushort, CPACK_RACE_EventData> CompetitionData = new Dictionary<ushort, CPACK_RACE_EventData>();
    public static Dictionary<ushort, CPACK_RACE_EventData> SequenceData = new Dictionary<ushort, CPACK_RACE_EventData>();
    public static Dictionary<ushort, GameObject> ListObject = new Dictionary<ushort, GameObject>();
    public static bool DataChange_Bool = false;
    public GameObject RaceObject;
    public static bool RaceButtonClick = false;
    public static bool RaceGame_Bool = false;
    public UIScrollBar BoxBar;
    public UILabel[] NextRace_Label;
    private string Name1 = "";
    private string Name2 = "";
    public UIScrollView m_Scrollview;

    bool BarCheck_Bool = false;
    float DelayTime = 0;
    float DelayTime2 = 0;
    bool FirstClick = false;
    bool FirstClick2 = false;
    public static bool SinedOK_Bool = false;
    public static Dictionary<ushort, CPACK_RACE_SwitchState> SaveCompetition = new Dictionary<ushort, CPACK_RACE_SwitchState>();
    public static bool LockObject_Bool = false;
    public static Dictionary<ushort, CPACK_RACE_UpdatePlayerCnt> SaveRacePlayerData = new Dictionary<ushort, CPACK_RACE_UpdatePlayerCnt>();

    public static object CompetitionLockObject = new object();

    // Use this for initialization
    void Start()
    {
        //   CompetitionData.Clear();
        //   SequenceData.Clear();
        //   ListObject.Clear();
        DataChange_Bool = true;
        NextRace_Label[0].text = "";
        NextRace_Label[1].text = "";
        CompetitionBoxOpen_Bool = false;
        // DataChange_Bool = false;
        RaceButtonClick = false;
        RaceGame_Bool = false;
        BeforeRankingBoxOpen_Bool = false;
        SaveCompetition.Clear();
        LockObject_Bool = false;
        SaveRacePlayerData.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (VersionDef.InternationalLanguageSystem)
        {
            Name1 = "(" + Font_Control.Instance.m_dicMsgStr[2008025] + ")";
            Name2 = "(" + Font_Control.Instance.m_dicMsgStr[2008026] + ")";
        }
        else
        {
            Name1 = "(" + "報名中" + ")";
            Name2 = "(" + "比賽中" + ")";
        }

        if (SaveRacePlayerData.Count != 0 /*&& !DataChange_Bool && !LockObject_Bool*/)
        {
            lock (CompetitionLockObject)
            {
                foreach (var item in SaveRacePlayerData)
                {
                    CompetitionData[item.Key].m_uiNowPlayerCnt = item.Value.m_uiPlayerCnt;
                    if (CompetitionData[item.Key].m_oSwitchState.m_enumState == ENUM_RACE_STATE.End)
                    {
                        item.Value.m_uiPlayerCnt = 0;
                        CompetitionData[item.Key].m_uiNowPlayerCnt = 0;
                    }
                    if (CompetitionData[item.Key].m_bSigned)
                        ListObject[item.Key].GetComponent<RaceData>().Signed_Bool = true;
                    else
                        ListObject[item.Key].GetComponent<RaceData>().Signed_Bool = false;

                    ListObject[item.Key].GetComponent<RaceData>().People_Label.text = CompetitionData[item.Key].m_uiNowPlayerCnt + "人 / " + CompetitionData[item.Key].m_uiMaxPlayerCnt + "人";
                }

                SaveRacePlayerData.Clear();

                //if (MainConnet.m_Scence == MainConnet.SecnceType.Baccarat_Lobby)
                //    DataChange_Bool = true;
            }
        }

        //if (!DataChange_Bool && LockObject_Bool)

        if (SaveCompetition.Count != 0)
        {
            lock (CompetitionLockObject)
            {
                foreach (var item in SaveCompetition)
                {
                    CompetitionData[item.Key].m_oSwitchState.m_timeNext = item.Value.m_timeNext;
                    CompetitionData[item.Key].m_oSwitchState.m_enumState = item.Value.m_enumState;

                    if (item.Value.m_enumState == ENUM_RACE_STATE.Sign)
                    {
                        CompetitionData[item.Key].m_bSigned = false;
                    }
                    else if (item.Value.m_enumState == ENUM_RACE_STATE.End)
                    {
                        CompetitionData[item.Key].m_uiNowPlayerCnt = 0;
                    }

                    DataChange_Bool = true;
                }
                SaveCompetition.Clear();
                DataChange_Bool = true;
                LockObject_Bool = false;
            }
        }

        if (CompetitionBoxOpen_Bool)
        {
            Competition_Object.SetActive(true);
        }
        else
        {
            Competition_Object.SetActive(false);
        }

        if (BeforeRankingBoxOpen_Bool)
        {
            BeforeRanking_Object.SetActive(true);
        }
        else
        {
            BeforeRanking_Object.SetActive(false);
        }

        if (DataChange_Bool && MainConnet.m_Scence == MainConnet.SecnceType.Baccarat_Lobby)
        {
            lock (CompetitionLockObject)
            {
                SequenceData_Void();
                Debug.Log("賽事更新成功");
                DataChange_Bool = false;
            }
        }

        if (BarCheck_Bool)
        {
            Bar_Void();
        }

        if (!FirstClick && !BarCheck_Bool && FirstClick2)
        {
            if (DelayTime2 < 1)
            {
                DelayTime2 += Time.deltaTime;
                m_Scrollview.UpdatePosition();
                m_Scrollview.UpdateScrollbars();
                BoxBar.value = 0;
            }
            else
            {
                FirstClick = true;
            }

        }

        if (SequenceData.Count < 3)
        {
            BoxBar.barSize = 1;
            BoxBar.value = 0;
        }
    }

    void SequenceData_Void()
    {
        List<CPACK_RACE_EventData> SaveDataList = new List<CPACK_RACE_EventData>(CompetitionData.Values);

        List<CPACK_RACE_EventData> FitstList = new List<CPACK_RACE_EventData>();
        List<CPACK_RACE_EventData> SignList = new List<CPACK_RACE_EventData>();
        List<CPACK_RACE_EventData> OtherList = new List<CPACK_RACE_EventData>();
        List<CPACK_RACE_EventData> EndList = new List<CPACK_RACE_EventData>();
        SinedOK_Bool = false;

        foreach (var item in SaveDataList)
        {
            if (item.m_bSigned && (item.m_oSwitchState.m_enumState == ENUM_RACE_STATE.WaitStart || item.m_oSwitchState.m_enumState == ENUM_RACE_STATE.Racing))
                FitstList.Add(item);
            else if (item.m_oSwitchState.m_enumState == ENUM_RACE_STATE.Sign)
                SignList.Add(item);
            else if (item.m_oSwitchState.m_enumState == ENUM_RACE_STATE.End)
                EndList.Add(item);
            else if (!item.m_bSigned)
                OtherList.Add(item);
        }
        SaveDataList.Clear();

        if (ListObject.Count != 0)
        {
            foreach (GameObject item in ListObject.Values)
            {
                Destroy(item);
            }
            ListObject.Clear();
            SequenceData.Clear();
        }

        if (FitstList.Count != 0)
        {
            for (ushort i = 0; i < FitstList.Count; i++)
                SequenceData.Add(FitstList[i].m_oSwitchState.m_usEventID, FitstList[i]);
        }

        //對報名做排序
        if (SignList.Count != 0)
        {
            SignList.Sort(delegate (CPACK_RACE_EventData x, CPACK_RACE_EventData y)
            {
                if (x.m_oSwitchState.m_timeNext == null && y.m_oSwitchState.m_timeNext == null) return 0;
                else if (x.m_oSwitchState.m_timeNext == null) return -1;
                else if (y.m_oSwitchState.m_timeNext == null) return 1;
                else
                {
                    if (y.m_oSwitchState.m_enumState == ENUM_RACE_STATE.Sign)
                    {
                        return (x.m_oSwitchState.m_timeNext.AddMinutes(-x.m_uiWaitSignMinute)).CompareTo(y.m_oSwitchState.m_timeNext.AddMinutes(-y.m_uiWaitSignMinute));
                    }
                    else if (y.m_oSwitchState.m_enumState == ENUM_RACE_STATE.End)
                    {
                        return (x.m_oSwitchState.m_timeNext.AddMinutes(-x.m_uiWaitSignMinute)).CompareTo(y.m_oSwitchState.m_timeNext.AddMinutes(y.m_uiWaitSignMinute));
                    }
                    else
                    {
                        return x.m_oSwitchState.m_timeNext.CompareTo(y.m_oSwitchState.m_timeNext);
                    }
                }
            });

            for (ushort i = 0; i < SignList.Count; i++)
                SequenceData.Add(SignList[i].m_oSwitchState.m_usEventID, SignList[i]);
        }

        //對比賽結束做排序
        if (EndList.Count != 0)
        {
            EndList.Sort(delegate (CPACK_RACE_EventData x, CPACK_RACE_EventData y)
            {
                if (x.m_oSwitchState.m_timeNext == null && y.m_oSwitchState.m_timeNext == null) return 0;
                else if (x.m_oSwitchState.m_timeNext == null) return -1;
                else if (y.m_oSwitchState.m_timeNext == null) return 1;
                else
                {
                    if (y.m_oSwitchState.m_enumState == ENUM_RACE_STATE.Sign)
                    {
                        return (x.m_oSwitchState.m_timeNext.AddMinutes(x.m_uiWaitSignMinute)).CompareTo(y.m_oSwitchState.m_timeNext.AddMinutes(-y.m_uiWaitSignMinute));
                    }
                    else if (y.m_oSwitchState.m_enumState == ENUM_RACE_STATE.End)
                    {
                        return (x.m_oSwitchState.m_timeNext.AddMinutes(x.m_uiWaitSignMinute)).CompareTo(y.m_oSwitchState.m_timeNext.AddMinutes(y.m_uiWaitSignMinute));
                    }
                    else
                    {
                        return x.m_oSwitchState.m_timeNext.CompareTo(y.m_oSwitchState.m_timeNext);
                    }
                }

            });

            for (ushort i = 0; i < EndList.Count; i++)
                SequenceData.Add(EndList[i].m_oSwitchState.m_usEventID, EndList[i]);
        }

        //對比賽中做排序
        if (OtherList.Count != 0)
        {
            OtherList.Sort(delegate (CPACK_RACE_EventData x, CPACK_RACE_EventData y)
            {
                if (x.m_oSwitchState.m_timeNext == null && y.m_oSwitchState.m_timeNext == null) return 0;
                else if (x.m_oSwitchState.m_timeNext == null) return -1;
                else if (y.m_oSwitchState.m_timeNext == null) return 1;
                else return x.m_oSwitchState.m_timeNext.CompareTo(y.m_oSwitchState.m_timeNext);
            });

            for (ushort i = 0; i < OtherList.Count; i++)
                SequenceData.Add(OtherList[i].m_oSwitchState.m_usEventID, OtherList[i]);
        }
        #region ListSort
        //SaveDataList.Sort(delegate (CPACK_RACE_EventData x, CPACK_RACE_EventData y)
        //{
        //    if (x.m_oSwitchState.m_timeNext == null && y.m_oSwitchState.m_timeNext == null) return 0;
        //    else if (x.m_oSwitchState.m_timeNext == null) return -1;
        //    else if (y.m_oSwitchState.m_timeNext == null) return 1;
        //    else if (x.m_oSwitchState.m_enumState == ENUM_RACE_STATE.End)
        //    {
        //        if (y.m_oSwitchState.m_enumState == ENUM_RACE_STATE.Sign)
        //        {
        //            return (x.m_oSwitchState.m_timeNext.AddMinutes(x.m_uiWaitSignMinute)).CompareTo(y.m_oSwitchState.m_timeNext.AddMinutes(-y.m_uiWaitSignMinute));
        //        }
        //        else if (y.m_oSwitchState.m_enumState == ENUM_RACE_STATE.End)
        //        {
        //            return (x.m_oSwitchState.m_timeNext.AddMinutes(x.m_uiWaitSignMinute)).CompareTo(y.m_oSwitchState.m_timeNext.AddMinutes(y.m_uiWaitSignMinute));
        //        }
        //        else
        //        {
        //            return x.m_oSwitchState.m_timeNext.CompareTo(y.m_oSwitchState.m_timeNext);
        //        }
        //    }
        //    else if (x.m_oSwitchState.m_enumState == ENUM_RACE_STATE.Sign)
        //    {
        //        if (y.m_oSwitchState.m_enumState == ENUM_RACE_STATE.Sign)
        //        {
        //            return (x.m_oSwitchState.m_timeNext.AddMinutes(-x.m_uiWaitSignMinute)).CompareTo(y.m_oSwitchState.m_timeNext.AddMinutes(-y.m_uiWaitSignMinute));
        //        }
        //        else if (y.m_oSwitchState.m_enumState == ENUM_RACE_STATE.End)
        //        {
        //            return (x.m_oSwitchState.m_timeNext.AddMinutes(-x.m_uiWaitSignMinute)).CompareTo(y.m_oSwitchState.m_timeNext.AddMinutes(y.m_uiWaitSignMinute));
        //        }
        //        else
        //        {
        //            return x.m_oSwitchState.m_timeNext.CompareTo(y.m_oSwitchState.m_timeNext);
        //        }
        //    }
        //    else return x.m_oSwitchState.m_timeNext.CompareTo(y.m_oSwitchState.m_timeNext);
        //});
        #endregion

        //for (ushort i = 0; i < SaveDataList.Count; i++)
        //{
        //    SequenceData.Add(SaveDataList[i].m_oSwitchState.m_usEventID, SaveDataList[i]);
        //}

        #region ObjectAdd
        byte Number = 0;
        foreach (var item in SequenceData)
        {
            GameObject Data = Instantiate(RaceObject);

            Data.transform.parent = CompetitionTable.transform;
            Data.transform.localScale = new Vector3(1, 1, 1);
            if (Number < 10)
            {
                Data.name = "0" + Number.ToString();
            }
            else
            {
                Data.name = Number.ToString();
            }
            RaceData m_RaceData = Data.GetComponent<RaceData>();
            RaceButton Data_cs = m_RaceData.RaceDataButton.GetComponent<RaceButton>();
            string SaveDate = item.Value.m_oSwitchState.m_timeNext.ToString("M/d");
            string SaveStartTime = "";
            string SaveEndTime = "";
            string SaveStartTime2 = "";
            string SaveEndTime2 = "";
            if (item.Value.m_oSwitchState.m_enumState == ENUM_RACE_STATE.Sign)
            {
                SaveStartTime = item.Value.m_oSwitchState.m_timeNext.AddMinutes(-item.Value.m_uiWaitSignMinute).ToString("HH：mm");
                SaveEndTime = item.Value.m_oSwitchState.m_timeNext.ToString("HH：mm");
                SaveStartTime2 = item.Value.m_oSwitchState.m_timeNext.AddMinutes(-item.Value.m_uiWaitSignMinute).ToString("HH:mm");
                SaveEndTime2 = item.Value.m_oSwitchState.m_timeNext.ToString("HH:mm");
                m_RaceData.People_Label.text = item.Value.m_uiNowPlayerCnt + "人 / " + item.Value.m_uiMaxPlayerCnt + "人";
            }
            else if (item.Value.m_oSwitchState.m_enumState == ENUM_RACE_STATE.End)
            {
                SaveStartTime = item.Value.m_oSwitchState.m_timeNext.ToString("HH：mm");
                SaveEndTime = item.Value.m_oSwitchState.m_timeNext.AddMinutes(item.Value.m_uiWaitSignMinute).ToString("HH：mm");
                SaveStartTime2 = item.Value.m_oSwitchState.m_timeNext.ToString("HH:mm");
                SaveEndTime2 = item.Value.m_oSwitchState.m_timeNext.AddMinutes(item.Value.m_uiWaitSignMinute).ToString("HH:mm");
                m_RaceData.People_Label.text = "0人 / " + item.Value.m_uiMaxPlayerCnt + "人";
            }
            else
            {
                SaveStartTime = "0";
                SaveEndTime = "0";
                m_RaceData.People_Label.text = item.Value.m_uiNowPlayerCnt + "人 / " + item.Value.m_uiMaxPlayerCnt + "人";
            }

            m_RaceData.ID = item.Key;
            m_RaceData.RaceTime = SaveDate + " " + SaveEndTime;
            m_RaceData.RACE_EVENT_TYPE = item.Value.m_oSwitchState.m_enumState;
            m_RaceData.m_RACE_EVENT_TYPE = item.Value.m_enumEventType;
            m_RaceData.Date_Label.text = SaveDate + "  " + item.Value.m_strEventName;
            m_RaceData.ApplyTime_Label.text = SaveDate + " " + SaveStartTime;
            m_RaceData.StartTime_Label.text = SaveEndTime;
            m_RaceData.Round_Label.text = item.Value.m_uiEventRound.ToString();
            m_RaceData.MaxPlayerCnt = item.Value.m_uiMaxPlayerCnt;
            m_RaceData.Cost_Label.text = item.Value.m_uiFeeVal.ToString();
            m_RaceData.Explanation_Label.text = item.Value.m_strInfo;

            m_RaceData.RaceName = item.Value.m_strEventName;
            m_RaceData.ENUM_RACE_FEE_TYPE = item.Value.m_enumFeeType;
            m_RaceData.uiFeeVal = item.Value.m_uiFeeVal;
            m_RaceData.Signed_Bool = item.Value.m_bSigned;
            if (item.Value.m_bSigned && item.Value.m_oSwitchState.m_enumState != ENUM_RACE_STATE.End)
                SinedOK_Bool = true;

            ListObject.Add(m_RaceData.ID, Data);
            if (Number == 0)
            {
                if (item.Value.m_oSwitchState.m_enumState == ENUM_RACE_STATE.Sign)
                {
                    NextRace_Label[0].text = SaveDate + "  " + item.Value.m_strEventName + Name1;
                }
                else if (item.Value.m_oSwitchState.m_enumState == ENUM_RACE_STATE.WaitStart || item.Value.m_oSwitchState.m_enumState == ENUM_RACE_STATE.Racing)
                {
                    NextRace_Label[0].text = SaveDate + "  " + item.Value.m_strEventName + Name2;
                }
                else if (item.Value.m_oSwitchState.m_enumState == ENUM_RACE_STATE.End)
                {
                    NextRace_Label[0].text = SaveDate + "  " + SaveStartTime2 + "～" + SaveEndTime2 + "  " + item.Value.m_strEventName;
                }

            }
            else if (Number == 1)
            {
                if (item.Value.m_oSwitchState.m_enumState == ENUM_RACE_STATE.Sign)
                {
                    NextRace_Label[1].text = SaveDate + "  " + item.Value.m_strEventName + Name1;
                }
                else if (item.Value.m_oSwitchState.m_enumState == ENUM_RACE_STATE.WaitStart || item.Value.m_oSwitchState.m_enumState == ENUM_RACE_STATE.Racing)
                {
                    NextRace_Label[1].text = SaveDate + "  " + item.Value.m_strEventName + Name2;
                }
                else if (item.Value.m_oSwitchState.m_enumState == ENUM_RACE_STATE.End)
                {
                    NextRace_Label[1].text = SaveDate + "  " + SaveStartTime2 + "～" + SaveEndTime2 + "  " + item.Value.m_strEventName;
                }
            }
            Number++;
        }
        #endregion
        BarCheck_Bool = true;
    }

    void Bar_Void()
    {
        if (DelayTime < 0.5f)
        {
            DelayTime += Time.deltaTime;
            CompetitionTable.repositionNow = true;
            //BoxView.UpdatePosition();
            //BoxView.UpdateScrollbars();

        }
        else
        {
            //BoxView.ResetPosition();
            FirstClick2 = true;
            DelayTime = 0;
            BarCheck_Bool = false;
        }
    }
}
