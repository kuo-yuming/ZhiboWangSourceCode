using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Race_Info_Data;
using GameCore.Machine;

namespace Race_Info_Data
{
    public class RaceInfoDataSave
    {
        public ENUM_RACE_EVENT_TYPE DataType = ENUM_RACE_EVENT_TYPE.Once;
        public Dictionary<ushort, CPACK_RACE_WinMoneySet> RaceData = new Dictionary<ushort, CPACK_RACE_WinMoneySet>();//名次
    }

    public class RaceInfoShowData
    {
        public ENUM_RACE_EVENT_TYPE usDataType = ENUM_RACE_EVENT_TYPE.Once;
        public uint Winter1 = 0;
        public uint Winter2 = 0;
        public uint Winter3 = 0;
        public uint EnterMoney = 0;
    }

}


public class RaceInfo : MonoBehaviour {
    public static bool RaceInfoObject_bool = false;
    public static bool RaceInfoDataChange_bool = false;
    public static ushort RaceID = 0;
    public GameObject RaceInfoObject;

    public GameObject RaceInfoBarObject;
    public GameObject RaceInfoDataObject;
    public GameObject RaceInfoEndObject;

    public UITable RaceInfoTable;
    public UIScrollBar RaceInfoScrollBar;//資料小於4時為0
    public UISprite RaceInfoSprite;

    public static Dictionary<ushort, RaceInfoDataSave> RaceInfoData = new Dictionary<ushort, RaceInfoDataSave>();//所有賽事資訊資料
    public static Dictionary<uint, RaceInfoShowData> m_RaceInfoShowData = new Dictionary<uint, RaceInfoShowData>();//要顯示的賽事資料  人數 獎項
    public static Dictionary<ushort, GameObject> RaceInfoListObject = new Dictionary<ushort, GameObject>();

    bool Open_Bool = false;
    bool AddScrollBarDelay_Bool = false;
    float DelayTimer = 0;
    // Use this for initialization
    void Start () {
        RaceInfoObject_bool = false;
        RaceInfoDataChange_bool = false;
        RaceInfoObject.SetActive(false);
        m_RaceInfoShowData.Clear();
        RaceInfoListObject.Clear();
        Open_Bool = false;
        RaceID = 0;
    }
	
	// Update is called once per frame
	void Update () {

        if (RaceInfoObject_bool)
        {
            if (!Open_Bool)
            {
                ////清除資料
                //if (RaceInfoListObject.Count != 0)
                //{
                //    foreach (GameObject item in RaceInfoListObject.Values)
                //    {
                //        Destroy(item);
                //        Debug.Log("刪除名稱: " + item);
                //    }
                //    RaceInfoListObject.Clear();
                //    m_RaceInfoShowData.Clear();
                //}

                //先生成一小條線
                byte Number = 0;
                GameObject Data = Instantiate(RaceInfoBarObject);
                Data.transform.parent = RaceInfoTable.transform;
                Data.transform.localScale = new Vector3(1, 1, 1);
                Data.name = "0" + Number.ToString();
                RaceInfoListObject.Add(Number, Data);
                Number++;

                //生成各獎項資料
                foreach (var item in RaceInfoData)
                {
                    if (item.Key == RaceID)
                    {
                        
                        if (item.Value.DataType == ENUM_RACE_EVENT_TYPE.Once)
                            RaceInfoSprite.spriteName = "bg_raceabout_0-2";
                        else
                            RaceInfoSprite.spriteName = "bg_raceabout_0-1";

                        
                        #region CheckData
                        //先储存起來
                        foreach (var item2 in item.Value.RaceData)
                        {
                            foreach (var item3 in item2.Value.m_dicRaceMoney)
                            {
                                if (!m_RaceInfoShowData.ContainsKey(item3.Key))
                                {
                                    RaceInfoShowData usData = new RaceInfoShowData();
                                    usData.usDataType = item.Value.DataType;
                                    m_RaceInfoShowData.Add(item3.Key, usData);
                                }
                                   
                            }
                        }
                        foreach (var item2 in item.Value.RaceData)
                        {
                            foreach (var item3 in item2.Value.m_dicRaceMoney)
                            {
                                RaceInfoShowData usData = new RaceInfoShowData();
                                if (item2.Key == 1)
                                {
                                    m_RaceInfoShowData[item3.Key].Winter1 = item3.Value;
                                }
                                else if (item2.Key == 2)
                                {
                                    m_RaceInfoShowData[item3.Key].Winter2 = item3.Value;
                                }
                                else if (item2.Key == 3)
                                {
                                    m_RaceInfoShowData[item3.Key].Winter3 = item3.Value;
                                }
                                else if (item2.Key == 0)
                                {
                                    m_RaceInfoShowData[item3.Key].EnterMoney = item3.Value;
                                }

                       //         Debug.Log("報名人數: " + item3.Key + "  //比賽名次1: " + m_RaceInfoShowData[item3.Key].Winter1 + "  //比賽名次2: " + m_RaceInfoShowData[item3.Key].Winter2 + "  //比賽名次3: " + m_RaceInfoShowData[item3.Key].Winter3);
                            }
                        }
                        #endregion

                        //foreach (var item2 in m_RaceInfoShowData)
                        //{
                        //    Debug.Log("報名人數: " + item2.Key + "  //比賽名次1: " + item2.Value.Winter1 + "  //比賽名次2: " + item2.Value.Winter2 + "  //比賽名次3: " + item2.Value.Winter3);
                        //}
                        #region AddObject
                        foreach (var item2 in m_RaceInfoShowData)
                        {
                            GameObject Data2 = Instantiate(RaceInfoDataObject);
                            Data2.transform.parent = RaceInfoTable.transform;
                            Data2.transform.localScale = new Vector3(1, 1, 1);
                            if (Number < 10)
                                Data2.name = "0" + Number.ToString();
                            else
                                Data2.name = Number.ToString();

                            RaceInfoObject m_RaceData = Data2.GetComponent<RaceInfoObject>();

                            m_RaceData.ObjectType = item2.Value.usDataType;
                            if (item2.Value.usDataType == ENUM_RACE_EVENT_TYPE.Once)
                            {
                                m_RaceData.OnlyOneEntryPeople_Label.text = item2.Key.ToString();
                                m_RaceData.OnlyOneWinterOne_Label.text = item2.Value.Winter1.ToString();
                                m_RaceData.OnlyOneWinterTwo_Label.text = item2.Value.Winter2.ToString();
                                m_RaceData.OnlyOneWinterThree_Label.text = item2.Value.Winter3.ToString();
                                m_RaceData.OnlyOneEntryMoney_Label.text = item2.Value.EnterMoney.ToString();
                            }
                            else
                            {
                                m_RaceData.NormalEntryPeople_Label.text = item2.Key.ToString();
                                m_RaceData.NormalWinterOne_Label.text = item2.Value.Winter1.ToString();
                                m_RaceData.NormalWinterTwo_Label.text = item2.Value.Winter2.ToString();
                                m_RaceData.NormalWinterThree_Label.text = item2.Value.Winter3.ToString();
                            }
                            RaceInfoListObject.Add(Number, Data2);
                            Number++;
                        }
                        #endregion
                    }               
                }
                //生成最後訊息
                GameObject Data3 = Instantiate(RaceInfoEndObject);
                Data3.transform.parent = RaceInfoTable.transform;
                Data3.transform.localScale = new Vector3(1, 1, 1);
                if (Number < 10)
                    Data3.name = "0" + Number.ToString();
                else
                    Data3.name = Number.ToString();

                RaceInfoListObject.Add(Number, Data3);

                Open_Bool = true;
                AddScrollBarDelay_Bool = true;
            }
            RaceInfoObject.SetActive(true);
        }
        else
        {
            if (Open_Bool)
            {
                //清除資料
                if (RaceInfoListObject.Count != 0)
                {
                    foreach (GameObject item in RaceInfoListObject.Values)
                    {
                        Destroy(item);
                    }
                    RaceInfoListObject.Clear();
                    m_RaceInfoShowData.Clear();
                }
            }
            RaceInfoObject.SetActive(false);
            RaceInfoScrollBar.value = 0;
            DelayTimer = 0;
            AddScrollBarDelay_Bool = false;
            Open_Bool = false;
        }

        if (AddScrollBarDelay_Bool)
        {
            if (DelayTimer < 1)
            {
                RaceInfoTable.repositionNow = true;
                RaceInfoScrollBar.value = 0;
                DelayTimer += Time.deltaTime;
            }
            else
            {
                DelayTimer = 0;
                AddScrollBarDelay_Bool = false;
            }
        }

        if (m_RaceInfoShowData.Count < 4)
        {
            RaceInfoScrollBar.value = 0;
        }
	}
}
