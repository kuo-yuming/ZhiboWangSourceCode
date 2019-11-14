using UnityEngine;
using System.Collections;
using GameEnum;
using GameCore.Manager.Baccarat;
public class FourCardHistory_Control : MonoBehaviour
{
    public byte NowFourCardHistoryNumber = 0;
    public static ushort FCLeftOverCardPoint = 0;
    public GameObject FCPearlHandicapGO;
    public GameObject FCPearlHandicapTable;
    public GameObject MainFCPearHandicap;
    public static bool FCHistoryClickBool = false;
    public static bool FCHistoryDataGetBool = false;
    public static bool FCHistorySaveBool = true;
    public static bool FCHistorySaveOkBool = false;
    public static bool FCHistoryDataInit = true;
    public static byte FCCardPoint = 0;
    public static byte SaveFcCardPoint = 0;
    public static float NowSizeFloat = 1;

    public static bool FirstStautGet = false;
    // Use this for initialization
    void Start()
    {
        FCHistoryDataInit = true;
        FCHistoryDataInitVoid();
        FCCardPoint = 0;
        SaveFcCardPoint = 0;
        NowSizeFloat = 1;
        FirstStautGet = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (FCHistoryClickBool)
        {
            MainFCPearHandicap.SetActive(true);
        }
        else
        {
            MainFCPearHandicap.SetActive(false);
        }

        if (FCHistoryDataGetBool && FirstStautGet)
        {
            foreach (var item in History_Control.HistoryData)
            {
                if (History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_enumAward == ENUM_BACCARAT_AWARD.WinBanker)
                {
                    SaveFcCardPoint = item.Value.m_oLastAward.m_byBankerPoint;
                }
                else if (History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_enumAward == ENUM_BACCARAT_AWARD.WinPlayer)
                {
                    SaveFcCardPoint = item.Value.m_oLastAward.m_byPlayerPoint;
                }
                else if (History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_enumAward == ENUM_BACCARAT_AWARD.WinDraw)
                {
                    SaveFcCardPoint = item.Value.m_oLastAward.m_byBankerPoint;
                }

                if (MainGame_Control.StopModeState != ENUM_STOPMODE_STATE.WaitNextNewRound)
                {
                    FCPearlHandicapVoid();
                }
                else if (item.Key + 1 < History_Control.HistoryData.Count)
                {
                    FCPearlHandicapVoid();
                }
                else if (item.Value.m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinDraw)
                {
                    FCPearlHandicapVoid();
                }
                else if ((item.Value.m_oNormalAward.m_enumAward == item.Value.m_oLastAward.m_enumAward) && (item.Value.m_oNormalAward.m_byBankerPoint == item.Value.m_oLastAward.m_byBankerPoint) && (item.Value.m_oNormalAward.m_byPlayerPoint == item.Value.m_oLastAward.m_byPlayerPoint))
                {
                    FCPearlHandicapVoid();
                }
            }
            FCHistoryDataGetBool = false;
        }

        if (FCHistorySaveBool && FCHistorySaveOkBool)
        {
            FCPearlHandicapVoid();
            FCHistorySaveBool = false;
        }
        else if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.WaitStop)
        {
            FCHistorySaveOkBool = false;
            FCHistorySaveBool = true;
        }

        if (FCHistoryDataInit)
        {
            FCHistoryDataInitVoid();
            FCCardPoint = 0;
            SaveFcCardPoint = 0;
            FCLeftOverCardPoint = 52;
            FCHistoryDataInit = false;
        }
    }

    void FCPearlHandicapVoid()
    {
        byte SeatX = (byte)(NowFourCardHistoryNumber / 6);
        byte SeatY = (byte)(NowFourCardHistoryNumber % 6);
        GameObject Data = Instantiate(FCPearlHandicapGO);
        Data.transform.parent = FCPearlHandicapTable.transform;
        PearlHandicap Data_cs = Data.GetComponent<PearlHandicap>();
        if (History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_enumAward == ENUM_BACCARAT_AWARD.WinBanker && !History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_bBankerOnePair && !History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_r_0";
        }
        else if (History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_enumAward == ENUM_BACCARAT_AWARD.WinBanker && History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_bBankerOnePair && !History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_r_1";
        }
        else if (History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_enumAward == ENUM_BACCARAT_AWARD.WinBanker && !History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_bBankerOnePair && History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_r_2";
        }
        else if (History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_enumAward == ENUM_BACCARAT_AWARD.WinBanker && History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_bBankerOnePair && History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_r_3";
        }
        else if (History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_enumAward == ENUM_BACCARAT_AWARD.WinPlayer && !History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_bBankerOnePair && !History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_b_0";
        }
        else if (History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_enumAward == ENUM_BACCARAT_AWARD.WinPlayer && History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_bBankerOnePair && !History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_b_1";
        }
        else if (History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_enumAward == ENUM_BACCARAT_AWARD.WinPlayer && !History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_bBankerOnePair && History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_b_2";
        }
        else if (History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_enumAward == ENUM_BACCARAT_AWARD.WinPlayer && History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_bBankerOnePair && History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_b_3";
        }
        else if (History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_enumAward == ENUM_BACCARAT_AWARD.WinDraw && !History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_bBankerOnePair && !History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_g_0";
        }
        else if (History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_enumAward == ENUM_BACCARAT_AWARD.WinDraw && History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_bBankerOnePair && !History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_g_1";
        }
        else if (History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_enumAward == ENUM_BACCARAT_AWARD.WinDraw && !History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_bBankerOnePair && History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_g_2";
        }
        else if (History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_enumAward == ENUM_BACCARAT_AWARD.WinDraw && History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_bBankerOnePair && History_Control.HistoryData[NowFourCardHistoryNumber].m_oLastAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_g_3";
        }
        if (FCHistoryDataGetBool || MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.WaitNextNewRound)
        {
            Data_cs.LabelNumber.text = SaveFcCardPoint.ToString();
        }
        else
        {
            Data_cs.LabelNumber.text = SaveFcCardPoint.ToString();
        }
        Data.transform.localPosition = new Vector3(-501.5f + (SeatX * 32.3f), -45.5f - (31.4f * SeatY), 1);
        Data.transform.localScale = new Vector3(1, 1, 1);
        NowFourCardHistoryNumber++;
        //x=15
        //y=-12
    }

    void FCHistoryDataInitVoid()
    {
        NowSizeFloat = 1;
        FCHistoryClickBool = false;
        FCHistorySaveOkBool = false;
        FCHistorySaveBool = true;
        NowFourCardHistoryNumber = 0;

        Transform[] Objs = FCPearlHandicapTable.GetComponentsInChildren<Transform>();
        int Len = Objs.Length;
        for (int i = 0; i < Len; i++)
        {
            if (Objs[i].name != "FCPearlHandicap_Control")
            {
                Destroy(Objs[i].gameObject);
            }
        }
    }
}
