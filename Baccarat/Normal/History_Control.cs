using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore.Manager.Baccarat;
using GameEnum;
using GameCore;
using GameCore.Manager.Common;
public class History_Control : MonoBehaviour
{

    public static Dictionary<ushort, CAllBetAward> HistoryData = new Dictionary<ushort, CAllBetAward>();
    public static Dictionary<byte, byte> HistoryParkwayData = new Dictionary<byte, byte>();
    public static bool HistoryDataGetBool = false;
    public static bool HistoryOpenBool = false;
    public static bool HistoryInit = true;
    public static bool HistorySaveBool = true;
    public static bool HistorySaveOkBool = false;
    public static byte HistoryNumber = 0;
    public byte NowHistoryNumber = 0;
    public GameObject MainObject;
    public GameObject MainObject2;
    public GameObject PearlHandicapGO;//珠盤路生成OBJECT
    public GameObject PearlHandicapTable;//珠盤路生成位子
    public UIScrollBar ParkwayScrollBar;
    public GameObject ParkwayGO;//大路生成OBJECT
    public GameObject ParkwayTable;//大路生成位子
    public UIScrollBar PopeyesScrollBar;
    public GameObject PopeyesGO;//大眼仔生成OBJECT
    public GameObject PopeyesTable;//大眼仔生成位子
    public UIScrollBar LaneScrollBar;
    public GameObject LaneGO;
    public GameObject LaneTable;
    public UIScrollBar ZadzhaScrollBar;
    public GameObject ZadzhaGO;
    public GameObject ZadzhaTable;
    private byte ParkwaySeatX = 0;
    private byte ParkwaySeatY = 0;
    private byte PopeyesSeatX = 0;
    private byte PopeyesSeatY = 0;
    private byte LaneSeatX = 0;
    private byte LaneSeatY = 0;
    private byte ZadzhaSeatX = 0;
    private byte ZadzhaSeatY = 0;
    private byte PopeyesSaveColor = 0;//0:沒紀錄 1:紅 2:藍
    private byte LaneColor = 0;
    private byte ZadzhaColor = 0;
    public UIButton NextButton_Button;
    public UISprite NextButton_Sprite;
    public static byte NormalTotalPoint = 0;
    public static bool ChangeButton = true;

    public static bool HistoryStart_Bool = false;
    public GameObject PHLGameObject;
    public static Dictionary<byte, GameObject> SaveParkwayTableData = new Dictionary<byte, GameObject>();
    public static Dictionary<byte, GameObject> SavePopeyesTableData = new Dictionary<byte, GameObject>();
    public static Dictionary<byte, GameObject> SaveLaneTableData = new Dictionary<byte, GameObject>();
    public static Dictionary<byte, GameObject> SaveZadzhaTableData = new Dictionary<byte, GameObject>();
    public GameObject NextButton;
    public GameObject BackButton;

    public static bool AngleInit = false;
    bool AngleInitBool = false;
    // Use this for initialization
    void Start()
    {
        ChangeButton = true;
        DataInitVoid();
        HistoryStart_Bool = false;
        AngleInit = true;
        AngleInitBool = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (HistoryOpenBool)
        {
            MainObject.SetActive(true);

            if (!FourCardHistory_Control.FCHistoryClickBool)
            {
                MainObject2.SetActive(true);
            }
            else
            {
                MainObject2.SetActive(false);
            }
        }
        else
        {
            MainObject.SetActive(false);
            MainObject2.SetActive(false);
        }

        if (MainGame_Control.FourCardPlay_Bool)
        {
            if (!FourCardHistory_Control.FCHistoryClickBool)
            {
                NextButton.SetActive(true);
                BackButton.SetActive(false);
            }
            else
            {
                NextButton.SetActive(false);
                BackButton.SetActive(true);
            }
        }
        else
        {
            NextButton.SetActive(false);
            BackButton.SetActive(false);
        }

        if (HistoryInit)
        {
            MainObject.SetActive(true);
            MainObject2.SetActive(true);
            HistoryNumber = 0;
            HistoryData.Clear();
            HistoryParkwayData.Clear();
            DataInitVoid();
            HistoryInit = false;
        }

        ScrollBarVoid();

        if (HistoryDataGetBool)
        {
            FirstDataVoid();
            //////////追加
            if (Forecast.BankerWinParkwaySeatX >= 1)
            {
                if (Forecast.BankerWinParkwaySeatX == 1 && Forecast.BankerWinParkwaySeatY >= 1)
                {
                    Forecast.BankerForecastCheck = true;
                }
                else if (Forecast.BankerWinParkwaySeatX > 1)
                {
                    Forecast.BankerForecastCheck = true;
                }
            }
            if (Forecast.PlayerWinParkwaySeatX >= 1)
            {
                if (Forecast.PlayerWinParkwaySeatX == 1 && Forecast.PlayerWinParkwaySeatY >= 1)
                {
                    Forecast.PlayerForecastCheck = true;
                }
                else if (Forecast.PlayerWinParkwaySeatX > 1)
                {
                    Forecast.PlayerForecastCheck = true;
                }
            }
            HistoryDataGetBool = false;
        }

        if (HistorySaveBool && HistorySaveOkBool && HistoryStart_Bool)
        {
            HistorySaveVoid();
            if (Forecast.BankerWinParkwaySeatX >= 1)
            {
                if (Forecast.BankerWinParkwaySeatX == 1 && Forecast.BankerWinParkwaySeatY >= 1)
                {
                    Forecast.BankerForecastCheck = true;
                }
                else if (Forecast.BankerWinParkwaySeatX > 1)
                {
                    Forecast.BankerForecastCheck = true;
                }
            }
            if (Forecast.PlayerWinParkwaySeatX >= 1)
            {
                if (Forecast.PlayerWinParkwaySeatX == 1 && Forecast.PlayerWinParkwaySeatY >= 1)
                {
                    Forecast.PlayerForecastCheck = true;
                }
                else if (Forecast.PlayerWinParkwaySeatX > 1)
                {
                    Forecast.PlayerForecastCheck = true;
                }
            }
            HistorySaveBool = false;
            HistoryStart_Bool = false;
        }
        else if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.WaitStop)
        {
            HistorySaveOkBool = false;
            HistorySaveBool = true;
        }

        if (AngleInit)
        {
            if (HistoryData.Count != 0)
            {
                AngleInitBool = true;
                return;
            }
            Transform[] Objs = PearlHandicapTable.GetComponentsInChildren<Transform>();
            int Len = Objs.Length;
            for (int i = 0; i < Len; i++)
            {
                if (Objs[i].name != "PearlHandicap_Control")
                {
                    AngleInitBool = true;
                    return;
                }
            }
            Transform[] Objs1 = ParkwayTable.GetComponentsInChildren<Transform>();
            int Len1 = Objs1.Length;
            for (int i = 0; i < Len1; i++)
            {
                if (Objs1[i].name != "Parkway_Control")
                {
                    AngleInitBool = true;
                    return;
                }
            }
            Transform[] Objs2 = PopeyesTable.GetComponentsInChildren<Transform>();
            int Len2 = Objs2.Length;
            for (int i = 0; i < Len2; i++)
            {
                if (Objs2[i].name != "Popeyes_Control")
                {
                    AngleInitBool = true;
                    return;
                }
            }
            Transform[] Objs3 = LaneTable.GetComponentsInChildren<Transform>();
            int Len3 = Objs3.Length;
            for (int i = 0; i < Len3; i++)
            {
                if (Objs3[i].name != "Lane_Control")
                {
                    AngleInitBool = true;
                    return;
                }
            }
            Transform[] Objs4 = ZadzhaTable.GetComponentsInChildren<Transform>();
            int Len4 = Objs4.Length;
            for (int i = 0; i < Len4; i++)
            {
                if (Objs4[i].name != "Zadzha_Control")
                {
                    AngleInitBool = true;
                    return;
                }
            }
            AngleInit = false;
        }

        if (AngleInitBool && !HistoryInit)
        {
            MainObject.SetActive(true);
            MainObject2.SetActive(true);
            HistoryNumber = 0;
            HistoryData.Clear();
            HistoryParkwayData.Clear();
            DataInitVoid();
            AngleInitBool = false;
        }

    }

    //初始資料
    void FirstDataVoid()
    {
        MainGame_Control.BankerWinPoint = 0;
        MainGame_Control.PlayerWinPoint = 0;
        MainGame_Control.DrawWinPoint = 0;
        MainGame_Control.BankerPairWinPoint = 0;
        MainGame_Control.PlayerPairWinPoint = 0;

        foreach (var item in HistoryData)
        {
           // Debug.Log("正常第" + item.Key + "筆: " + " 結果: " + item.Value.m_oNormalAward.m_enumAward + " 莊對: " + item.Value.m_oNormalAward.m_bBankerOnePair + " 閒對: " + item.Value.m_oNormalAward.m_bPlayerOnePair);
          //  Debug.Log("搶牌第" + item.Key + "筆: " + " 結果: " + item.Value.m_oLastAward.m_enumAward + " 莊對: " + item.Value.m_oLastAward.m_bBankerOnePair + " 閒對: " + item.Value.m_oLastAward.m_bPlayerOnePair);
            if (item.Value.m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinBanker)
            {
                NormalTotalPoint = item.Value.m_oNormalAward.m_byBankerPoint;
                MainGame_Control.BankerWinPoint++;
            }
            else if (item.Value.m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinPlayer)
            {
                NormalTotalPoint = item.Value.m_oNormalAward.m_byPlayerPoint;
                MainGame_Control.PlayerWinPoint++;
            }
            else if (item.Value.m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinDraw)
            {
                NormalTotalPoint = item.Value.m_oNormalAward.m_byBankerPoint;
                MainGame_Control.DrawWinPoint++;
            }

            if (item.Value.m_oNormalAward.m_bBankerOnePair)
            {
                MainGame_Control.BankerPairWinPoint++;
            }

            if (item.Value.m_oNormalAward.m_bPlayerOnePair)
            {
                MainGame_Control.PlayerPairWinPoint++;
            }

            PearlHandicapVoid();

            if (item.Value.m_oNormalAward.m_enumAward != ENUM_BACCARAT_AWARD.WinDraw)
            {
                ParkwayVoid();
                if (ParkwaySeatX >= 1)
                {
                    if (ParkwaySeatX == 1 && ParkwaySeatY >= 1)
                    {
                        PopeyesVoid();
                    }
                    else if (ParkwaySeatX > 1)
                    {
                        PopeyesVoid();
                    }
                }
                if (ParkwaySeatX >= 2)
                {
                    if (ParkwaySeatX == 2 && ParkwaySeatY >= 1)
                    {
                        LaneVoid();
                    }
                    else if (ParkwaySeatX > 2)
                    {
                        LaneVoid();
                    }
                }
                if (ParkwaySeatX >= 3)
                {
                    if (ParkwaySeatX == 3 && ParkwaySeatY >= 1)
                    {
                        ZadzhaVoid();
                    }
                    else if (ParkwaySeatX > 3)
                    {
                        ZadzhaVoid();
                    }
                }
            }

            NowHistoryNumber++;
        }
    }

    //歷史紀錄
    void HistorySaveVoid()
    {
        PearlHandicapVoid();
        if (HistoryData[NowHistoryNumber].m_oNormalAward.m_enumAward != ENUM_BACCARAT_AWARD.WinDraw)
        {
            ParkwayVoid();
            if (ParkwaySeatX >= 1)
            {
                if (ParkwaySeatX == 1 && ParkwaySeatY >= 1)
                {
                    PopeyesVoid();
                }
                else if (ParkwaySeatX > 1)
                {
                    PopeyesVoid();
                }
            }
            if (ParkwaySeatX >= 2)
            {
                if (ParkwaySeatX == 2 && ParkwaySeatY >= 1)
                {
                    LaneVoid();
                }
                else if (ParkwaySeatX > 2)
                {
                    LaneVoid();
                }
            }
            if (ParkwaySeatX >= 3)
            {
                if (ParkwaySeatX == 3 && ParkwaySeatY >= 1)
                {
                    ZadzhaVoid();
                }
                else if (ParkwaySeatX > 3)
                {
                    ZadzhaVoid();
                }
            }
        }

        NowHistoryNumber++;
    }

    //ScrollBar
    void ScrollBarVoid()
    {
        if (ParkwaySeatX > 12)
        {
            ParkwayScrollBar.value = 1;
            ParkwayScrollBar.barSize = 1;
        }
        else
        {
            ParkwayScrollBar.value = 0;
            ParkwayScrollBar.barSize = 0;
        }
        if (PopeyesSeatX > 10)
        {
            PopeyesScrollBar.value = 1;
            PopeyesScrollBar.barSize = 1;
        }
        else
        {
            PopeyesScrollBar.value = 0;
            PopeyesScrollBar.barSize = 0;
        }
        if (LaneSeatX > 10)
        {
            LaneScrollBar.value = 1;
            LaneScrollBar.barSize = 1;
        }
        else
        {
            LaneScrollBar.value = 0;
            LaneScrollBar.barSize = 0;
        }
        if (ZadzhaSeatX > 10)
        {
            ZadzhaScrollBar.value = 1;
            ZadzhaScrollBar.barSize = 1;
        }
        else
        {
            ZadzhaScrollBar.value = 0;
            ZadzhaScrollBar.barSize = 0;
        }
    }

    //PearlHandicap珠盤路
    void PearlHandicapVoid()
    {
        byte SeatX = (byte)(NowHistoryNumber / 6);
        byte SeatY = (byte)(NowHistoryNumber % 6);
        GameObject Data = Instantiate(PearlHandicapGO);
        Data.transform.parent = PearlHandicapTable.transform;
        PearlHandicap Data_cs = Data.GetComponent<PearlHandicap>();
        if (HistoryData[NowHistoryNumber].m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinBanker && !HistoryData[NowHistoryNumber].m_oNormalAward.m_bBankerOnePair && !HistoryData[NowHistoryNumber].m_oNormalAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_r_0";
        }
        else if (HistoryData[NowHistoryNumber].m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinBanker && HistoryData[NowHistoryNumber].m_oNormalAward.m_bBankerOnePair && !HistoryData[NowHistoryNumber].m_oNormalAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_r_1";
        }
        else if (HistoryData[NowHistoryNumber].m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinBanker && !HistoryData[NowHistoryNumber].m_oNormalAward.m_bBankerOnePair && HistoryData[NowHistoryNumber].m_oNormalAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_r_2";
        }
        else if (HistoryData[NowHistoryNumber].m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinBanker && HistoryData[NowHistoryNumber].m_oNormalAward.m_bBankerOnePair && HistoryData[NowHistoryNumber].m_oNormalAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_r_3";
        }
        else if (HistoryData[NowHistoryNumber].m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinPlayer && !HistoryData[NowHistoryNumber].m_oNormalAward.m_bBankerOnePair && !HistoryData[NowHistoryNumber].m_oNormalAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_b_0";
        }
        else if (HistoryData[NowHistoryNumber].m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinPlayer && HistoryData[NowHistoryNumber].m_oNormalAward.m_bBankerOnePair && !HistoryData[NowHistoryNumber].m_oNormalAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_b_1";
        }
        else if (HistoryData[NowHistoryNumber].m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinPlayer && !HistoryData[NowHistoryNumber].m_oNormalAward.m_bBankerOnePair && HistoryData[NowHistoryNumber].m_oNormalAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_b_2";
        }
        else if (HistoryData[NowHistoryNumber].m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinPlayer && HistoryData[NowHistoryNumber].m_oNormalAward.m_bBankerOnePair && HistoryData[NowHistoryNumber].m_oNormalAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_b_3";
        }
        else if (HistoryData[NowHistoryNumber].m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinDraw && !HistoryData[NowHistoryNumber].m_oNormalAward.m_bBankerOnePair && !HistoryData[NowHistoryNumber].m_oNormalAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_g_0";
        }
        else if (HistoryData[NowHistoryNumber].m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinDraw && HistoryData[NowHistoryNumber].m_oNormalAward.m_bBankerOnePair && !HistoryData[NowHistoryNumber].m_oNormalAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_g_1";
        }
        else if (HistoryData[NowHistoryNumber].m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinDraw && !HistoryData[NowHistoryNumber].m_oNormalAward.m_bBankerOnePair && HistoryData[NowHistoryNumber].m_oNormalAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_g_2";
        }
        else if (HistoryData[NowHistoryNumber].m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinDraw && HistoryData[NowHistoryNumber].m_oNormalAward.m_bBankerOnePair && HistoryData[NowHistoryNumber].m_oNormalAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_g_3";
        }
        Data_cs.LabelNumber.text = NormalTotalPoint.ToString();
        Data.transform.localPosition = new Vector3(-86.5f + (SeatX * 32.5f), -9.5f - (31.5f * SeatY), 1);
        Data.transform.localScale = new Vector3(1, 1, 1);
        //x=12
        //y=-9
    }

    //Parkway大路
    void ParkwayVoid()
    {
        if (NowHistoryNumber != 0)
        {
            if (HistoryData[NowHistoryNumber].m_oNormalAward.m_enumAward != ENUM_BACCARAT_AWARD.WinDraw)
            {
                for (int i = 1; i < HistoryData.Count; i++)
                {
                    if (HistoryParkwayData.Count == 0)
                    {
                        HistoryParkwayData.Add(ParkwaySeatX, ParkwaySeatY);
                        i = HistoryData.Count;
                    }
                    else
                    {
                        if (HistoryData[(ushort)(NowHistoryNumber - i)].m_oNormalAward.m_enumAward != ENUM_BACCARAT_AWARD.WinDraw)
                        {
                            if (HistoryData[NowHistoryNumber].m_oNormalAward.m_enumAward == HistoryData[(ushort)(NowHistoryNumber - i)].m_oNormalAward.m_enumAward)
                            {
                                ParkwaySeatY++;
                                HistoryParkwayData[ParkwaySeatX] = ParkwaySeatY;
                            }
                            else
                            {
                                ParkwaySeatX++;
                                ParkwaySeatY = 0;
                                HistoryParkwayData.Add(ParkwaySeatX, ParkwaySeatY);
                            }
                            ///////////////////////追加
                            if (HistoryData[NowHistoryNumber].m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinBanker)
                            {
                                //莊贏
                                Forecast.BankerWinParkwaySeatX = ParkwaySeatX;
                                Forecast.BankerWinParkwaySeatY = (byte)(ParkwaySeatY + 1);
                                //閒贏
                                Forecast.PlayerWinParkwaySeatX = (byte)(ParkwaySeatX + 1);
                                Forecast.PlayerWinParkwaySeatY = 0;
                            }
                            else if (HistoryData[NowHistoryNumber].m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinPlayer)
                            {
                                //莊贏
                                Forecast.BankerWinParkwaySeatX = (byte)(ParkwaySeatX + 1);
                                Forecast.BankerWinParkwaySeatY = 0;
                                //閒贏
                                Forecast.PlayerWinParkwaySeatX = ParkwaySeatX;
                                Forecast.PlayerWinParkwaySeatY = (byte)(ParkwaySeatY + 1);
                            }
                            i = HistoryData.Count;
                        }
                    }
                }
            }
        }
        else
        {
            HistoryParkwayData.Add(ParkwaySeatX, ParkwaySeatY);
        }



        GameObject Data = Instantiate(ParkwayGO);
        Data.transform.parent = ParkwayTable.transform;
        PearlHandicap Data_cs = Data.GetComponent<PearlHandicap>();
        if (HistoryData[NowHistoryNumber].m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinBanker && !HistoryData[NowHistoryNumber].m_oNormalAward.m_bBankerOnePair && !HistoryData[NowHistoryNumber].m_oNormalAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_r_0";

        }
        else if (HistoryData[NowHistoryNumber].m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinBanker && HistoryData[NowHistoryNumber].m_oNormalAward.m_bBankerOnePair && !HistoryData[NowHistoryNumber].m_oNormalAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_r_1";
        }
        else if (HistoryData[NowHistoryNumber].m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinBanker && !HistoryData[NowHistoryNumber].m_oNormalAward.m_bBankerOnePair && HistoryData[NowHistoryNumber].m_oNormalAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_r_2";
        }
        else if (HistoryData[NowHistoryNumber].m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinBanker && HistoryData[NowHistoryNumber].m_oNormalAward.m_bBankerOnePair && HistoryData[NowHistoryNumber].m_oNormalAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_r_3";
        }
        else if (HistoryData[NowHistoryNumber].m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinPlayer && !HistoryData[NowHistoryNumber].m_oNormalAward.m_bBankerOnePair && !HistoryData[NowHistoryNumber].m_oNormalAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_b_0";
        }
        else if (HistoryData[NowHistoryNumber].m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinPlayer && HistoryData[NowHistoryNumber].m_oNormalAward.m_bBankerOnePair && !HistoryData[NowHistoryNumber].m_oNormalAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_b_1";
        }
        else if (HistoryData[NowHistoryNumber].m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinPlayer && !HistoryData[NowHistoryNumber].m_oNormalAward.m_bBankerOnePair && HistoryData[NowHistoryNumber].m_oNormalAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_b_2";
        }
        else if (HistoryData[NowHistoryNumber].m_oNormalAward.m_enumAward == ENUM_BACCARAT_AWARD.WinPlayer && HistoryData[NowHistoryNumber].m_oNormalAward.m_bBankerOnePair && HistoryData[NowHistoryNumber].m_oNormalAward.m_bPlayerOnePair)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_b_3";
        }

        Data_cs.LabelNumber.text = NormalTotalPoint.ToString();
        Data.transform.localPosition = new Vector3(-51 + (ParkwaySeatX * 32.35f), 80.5f - (31.5f * ParkwaySeatY), 1);
        Data.transform.localScale = new Vector3(1, 1, 1);

        if (ParkwaySeatY >= 5)
        {
            Data_cs.PearlHandicapSprite.color = new Color32(255, 255, 255, 0);
            if (ParkwaySeatY == 5)
            {
                GameObject DataPHL = Instantiate(PHLGameObject);
                DataPHL.transform.parent = ParkwayTable.transform;
                DataPHL.transform.localScale = new Vector3(1, 1, 1);
                DataPHL.name = ParkwaySeatX.ToString();
                PHLabel DataPHL_cs = DataPHL.GetComponent<PHLabel>();
                DataPHL_cs.LabelNumber.text = (ParkwaySeatY + 1).ToString();
                DataPHL.transform.localPosition = new Vector3(-51 + (ParkwaySeatX * 32.35f), 80.5f - (32f * ParkwaySeatY), 1);
                SaveParkwayTableData.Add(ParkwaySeatX, DataPHL);
            }
            else
            {
                PHLabel PHLData = SaveParkwayTableData[ParkwaySeatX].GetComponent<PHLabel>();
                PHLData.LabelNumber.text = (ParkwaySeatY + 1).ToString();
            }
        }
        //x=-189
        //y=62
    }

    //Popeyes大眼仔
    void PopeyesVoid()
    {
        if (ParkwaySeatX >= 1)
        {
            if (ParkwaySeatX == 1 && ParkwaySeatY >= 1)
            {
                if (ParkwaySeatY <= HistoryParkwayData[(byte)(ParkwaySeatX - 1)])
                {
                    if (ParkwaySeatY == 1)
                    {
                        PopeyesSeatX = 0;
                        PopeyesSeatY = 0;
                    }
                    else
                    {
                        PopeyesSeatY++;
                    }
                    PopeyesSaveColor = 1;
                }
                else
                {
                    if (ParkwaySeatY == 1)
                    {
                        PopeyesSeatX = 0;
                        PopeyesSeatY = 0;
                        PopeyesSaveColor = 2;
                    }
                    else if ((ParkwaySeatY - HistoryParkwayData[(byte)(ParkwaySeatX - 1)]) == 1)
                    {
                        if (PopeyesSaveColor == 1)
                        {
                            PopeyesSeatX++;
                            PopeyesSeatY = 0;
                        }
                        else if (PopeyesSaveColor == 2)
                        {
                            PopeyesSeatX++;
                            PopeyesSeatY = 0;
                        }
                        PopeyesSaveColor = 2;
                    }
                    else
                    {
                        if (PopeyesSaveColor == 1)
                        {
                            PopeyesSeatY++;
                        }
                        else if (PopeyesSaveColor == 2)
                        {
                            PopeyesSeatX++;
                            PopeyesSeatY = 0;
                        }
                        PopeyesSaveColor = 1;
                    }
                }
            }
            else
            {
                if (ParkwaySeatY == 0)
                {
                    if (HistoryParkwayData[(byte)(ParkwaySeatX - 1)] == HistoryParkwayData[(byte)(ParkwaySeatX - 2)])
                    {
                        if (PopeyesSaveColor == 0)
                        {
                            PopeyesSeatX = 0;
                            PopeyesSeatY = 0;
                        }
                        else if (PopeyesSaveColor == 1)
                        {
                            PopeyesSeatY++;
                        }
                        else if (PopeyesSaveColor == 2)
                        {
                            PopeyesSeatX++;
                            PopeyesSeatY = 0;
                        }
                        PopeyesSaveColor = 1;
                    }
                    else
                    {
                        if (PopeyesSaveColor == 0)
                        {
                            PopeyesSeatX = 0;
                            PopeyesSeatY = 0;
                        }
                        else if (PopeyesSaveColor == 1)
                        {
                            PopeyesSeatX++;
                            PopeyesSeatY = 0;
                        }
                        else if (PopeyesSaveColor == 2)
                        {
                            PopeyesSeatY++;
                        }
                        PopeyesSaveColor = 2;
                    }
                }
                else
                {
                    if (ParkwaySeatY <= HistoryParkwayData[(byte)(ParkwaySeatX - 1)])
                    {
                        if (PopeyesSaveColor == 1)
                        {
                            PopeyesSeatY++;
                        }
                        else if (PopeyesSaveColor == 2)
                        {
                            PopeyesSeatX++;
                            PopeyesSeatY = 0;
                        }
                        PopeyesSaveColor = 1;
                    }
                    else
                    {
                        if ((ParkwaySeatY - HistoryParkwayData[(byte)(ParkwaySeatX - 1)]) == 1)
                        {
                            if (PopeyesSaveColor == 1)
                            {
                                PopeyesSeatX++;
                                PopeyesSeatY = 0;
                            }
                            else if (PopeyesSaveColor == 2)
                            {
                                PopeyesSeatY++;
                            }
                            PopeyesSaveColor = 2;
                        }
                        else
                        {
                            if (PopeyesSaveColor == 1)
                            {
                                PopeyesSeatY++;
                            }
                            else if (PopeyesSaveColor == 2)
                            {
                                PopeyesSeatX++;
                                PopeyesSeatY = 0;
                            }
                            PopeyesSaveColor = 1;
                        }
                    }
                }
            }
        }
        GameObject Data = Instantiate(PopeyesGO);
        Data.transform.parent = PopeyesTable.transform;
        PearlHandicap Data_cs = Data.GetComponent<PearlHandicap>();
        if (PopeyesSaveColor == 1)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_circle_0";

        }
        else if (PopeyesSaveColor == 2)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_circle_1";
        }
        Data_cs.LabelNumber.text = "";
        Data.transform.localPosition = new Vector3(-41.5f + (PopeyesSeatX * 31), 69.5f - (30.8f * PopeyesSeatY), 1);
        Data.transform.localScale = new Vector3(1, 1, 1);

        if (PopeyesSeatY >= 5)
        {
            Data_cs.PearlHandicapSprite.color = new Color32(255, 255, 255, 0);
            if (PopeyesSeatY == 5)
            {
                GameObject DataPHL = Instantiate(PHLGameObject);
                DataPHL.transform.parent = PopeyesTable.transform;
                DataPHL.transform.localScale = new Vector3(1, 1, 1);
                DataPHL.name = PopeyesSeatX.ToString();
                PHLabel DataPHL_cs = DataPHL.GetComponent<PHLabel>();
                DataPHL_cs.LabelNumber.text = (PopeyesSeatY + 1).ToString();
                DataPHL.transform.localPosition = new Vector3(-41.5f + (PopeyesSeatX * 31), 69.5f - (31.5f * PopeyesSeatY), 1);
                SavePopeyesTableData.Add(PopeyesSeatX, DataPHL);
            }
            else
            {
                PHLabel PHLData = SavePopeyesTableData[PopeyesSeatX].GetComponent<PHLabel>();
                PHLData.LabelNumber.text = (PopeyesSeatY + 1).ToString();
            }
        }
    }

    //Lane小路
    void LaneVoid()
    {
        if (ParkwaySeatX >= 2)
        {
            if (ParkwaySeatX == 2 && ParkwaySeatY >= 1)
            {
                if (ParkwaySeatY <= HistoryParkwayData[(byte)(ParkwaySeatX - 2)])
                {
                    if (ParkwaySeatY == 1)
                    {
                        LaneSeatX = 0;
                        LaneSeatY = 0;
                    }
                    else
                    {
                        LaneSeatY++;
                    }
                    LaneColor = 1;
                }
                else
                {
                    if (ParkwaySeatY == 1)
                    {
                        LaneSeatX = 0;
                        LaneSeatY = 0;
                        LaneColor = 2;
                    }
                    else if ((ParkwaySeatY - HistoryParkwayData[(byte)(ParkwaySeatX - 2)]) == 1)
                    {
                        if (LaneColor == 1)
                        {
                            LaneSeatX++;
                            LaneSeatY = 0;
                        }
                        else if (LaneColor == 2)
                        {
                            LaneSeatX++;
                            LaneSeatY = 0;
                        }
                        LaneColor = 2;
                    }
                    else
                    {
                        if (LaneColor == 1)
                        {
                            LaneSeatY++;
                        }
                        else if (LaneColor == 2)
                        {
                            LaneSeatX++;
                            LaneSeatY = 0;
                        }
                        LaneColor = 1;
                    }
                }
            }
            else
            {
                if (ParkwaySeatY == 0)
                {
                    if (HistoryParkwayData[(byte)(ParkwaySeatX - 1)] == HistoryParkwayData[(byte)(ParkwaySeatX - 3)])
                    {
                        if (LaneColor == 0)
                        {
                            LaneSeatX = 0;
                            LaneSeatY = 0;
                        }
                        else if (LaneColor == 1)
                        {
                            LaneSeatY++;
                        }
                        else if (LaneColor == 2)
                        {
                            LaneSeatX++;
                            LaneSeatY = 0;
                        }
                        LaneColor = 1;
                    }
                    else
                    {
                        if (LaneColor == 0)
                        {
                            LaneSeatX = 0;
                            LaneSeatY = 0;
                        }
                        else if (LaneColor == 1)
                        {
                            LaneSeatX++;
                            LaneSeatY = 0;
                        }
                        else if (LaneColor == 2)
                        {
                            LaneSeatY++;
                        }
                        LaneColor = 2;
                    }
                }
                else
                {
                    if (ParkwaySeatY <= HistoryParkwayData[(byte)(ParkwaySeatX - 2)])
                    {
                        if (LaneColor == 1)
                        {
                            LaneSeatY++;
                        }
                        else if (LaneColor == 2)
                        {
                            LaneSeatX++;
                            LaneSeatY = 0;
                        }
                        LaneColor = 1;
                    }
                    else
                    {
                        if ((ParkwaySeatY - HistoryParkwayData[(byte)(ParkwaySeatX - 2)]) == 1)
                        {
                            if (LaneColor == 1)
                            {
                                LaneSeatX++;
                                LaneSeatY = 0;
                            }
                            else if (LaneColor == 2)
                            {
                                LaneSeatY++;
                            }
                            LaneColor = 2;
                        }
                        else
                        {
                            if (LaneColor == 1)
                            {
                                LaneSeatY++;
                            }
                            else if (LaneColor == 2)
                            {
                                LaneSeatX++;
                                LaneSeatY = 0;
                            }
                            LaneColor = 1;
                        }
                    }
                }
            }
        }
        GameObject Data = Instantiate(LaneGO);
        Data.transform.parent = LaneTable.transform;
        PearlHandicap Data_cs = Data.GetComponent<PearlHandicap>();
        if (LaneColor == 1)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_r_0";
        }
        else if (LaneColor == 2)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_b_0";
        }
        Data_cs.LabelNumber.text = "";
        Data.transform.localPosition = new Vector3(-39.5f + (LaneSeatX * 31), 69 - (30.8f * LaneSeatY), 1);
        Data.transform.localScale = new Vector3(1, 1, 1);

        if (LaneSeatY >= 5)
        {
            Data_cs.PearlHandicapSprite.color = new Color32(255, 255, 255, 0);
            if (LaneSeatY == 5)
            {
                GameObject DataPHL = Instantiate(PHLGameObject);
                DataPHL.transform.parent = LaneTable.transform;
                DataPHL.transform.localScale = new Vector3(1, 1, 1);
                DataPHL.name = LaneSeatX.ToString();
                PHLabel DataPHL_cs = DataPHL.GetComponent<PHLabel>();
                DataPHL_cs.LabelNumber.text = (LaneSeatY + 1).ToString();
                DataPHL.transform.localPosition = new Vector3(-41.5f + (LaneSeatX * 31), 69.5f - (31.5f * LaneSeatY), 1);
                SaveLaneTableData.Add(LaneSeatX, DataPHL);
            }
            else
            {
                PHLabel PHLData = SaveLaneTableData[LaneSeatX].GetComponent<PHLabel>();
                PHLData.LabelNumber.text = (LaneSeatY + 1).ToString();
            }
        }
    }

    //Zadzha 小強路
    void ZadzhaVoid()
    {
        if (ParkwaySeatX >= 3)
        {
            if (ParkwaySeatX == 3 && ParkwaySeatY >= 1)
            {
                if (ParkwaySeatY <= HistoryParkwayData[(byte)(ParkwaySeatX - 3)])
                {
                    if (ParkwaySeatY == 1)
                    {
                        ZadzhaSeatX = 0;
                        ZadzhaSeatY = 0;
                    }
                    else
                    {
                        ZadzhaSeatY++;
                    }
                    ZadzhaColor = 1;
                }
                else
                {
                    if (ParkwaySeatY == 1)
                    {
                        ZadzhaSeatX = 0;
                        ZadzhaSeatY = 0;
                        ZadzhaColor = 2;
                    }
                    else if ((ParkwaySeatY - HistoryParkwayData[(byte)(ParkwaySeatX - 3)]) == 1)
                    {
                        if (ZadzhaColor == 1)
                        {
                            ZadzhaSeatX++;
                            ZadzhaSeatY = 0;
                        }
                        else if (LaneColor == 2)
                        {
                            ZadzhaSeatX++;
                            ZadzhaSeatY = 0;
                        }
                        ZadzhaColor = 2;
                    }
                    else
                    {
                        if (ZadzhaColor == 1)
                        {
                            ZadzhaSeatY++;
                        }
                        else if (ZadzhaColor == 2)
                        {
                            ZadzhaSeatX++;
                            ZadzhaSeatY = 0;
                        }
                        ZadzhaColor = 1;
                    }
                }
            }
            else
            {
                if (ParkwaySeatY == 0)
                {
                    if (HistoryParkwayData[(byte)(ParkwaySeatX - 1)] == HistoryParkwayData[(byte)(ParkwaySeatX - 4)])
                    {
                        if (ZadzhaColor == 0)
                        {
                            ZadzhaSeatX = 0;
                            ZadzhaSeatY = 0;
                        }
                        else if (ZadzhaColor == 1)
                        {
                            ZadzhaSeatY++;
                        }
                        else if (ZadzhaColor == 2)
                        {
                            ZadzhaSeatX++;
                            ZadzhaSeatY = 0;
                        }
                        ZadzhaColor = 1;
                    }
                    else
                    {
                        if (ZadzhaColor == 0)
                        {
                            ZadzhaSeatX = 0;
                            ZadzhaSeatY = 0;
                        }
                        else if (ZadzhaColor == 1)
                        {
                            ZadzhaSeatX++;
                            ZadzhaSeatY = 0;
                        }
                        else if (ZadzhaColor == 2)
                        {
                            ZadzhaSeatY++;
                        }
                        ZadzhaColor = 2;
                    }
                }
                else
                {
                    if (ParkwaySeatY <= HistoryParkwayData[(byte)(ParkwaySeatX - 3)])
                    {
                        if (ZadzhaColor == 1)
                        {
                            ZadzhaSeatY++;
                        }
                        else if (ZadzhaColor == 2)
                        {
                            ZadzhaSeatX++;
                            ZadzhaSeatY = 0;
                        }
                        ZadzhaColor = 1;
                    }
                    else
                    {
                        if ((ParkwaySeatY - HistoryParkwayData[(byte)(ParkwaySeatX - 3)]) == 1)
                        {
                            if (ZadzhaColor == 1)
                            {
                                ZadzhaSeatX++;
                                ZadzhaSeatY = 0;
                            }
                            else if (ZadzhaColor == 2)
                            {
                                ZadzhaSeatY++;
                            }
                            ZadzhaColor = 2;
                        }
                        else
                        {
                            if (ZadzhaColor == 1)
                            {
                                ZadzhaSeatY++;
                            }
                            else if (ZadzhaColor == 2)
                            {
                                ZadzhaSeatX++;
                                ZadzhaSeatY = 0;
                            }
                            ZadzhaColor = 1;
                        }
                    }
                }
            }
        }
        GameObject Data = Instantiate(ZadzhaGO);
        Data.transform.parent = ZadzhaTable.transform;
        PearlHandicap Data_cs = Data.GetComponent<PearlHandicap>();
        if (ZadzhaColor == 1)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_line_0";
        }
        else if (ZadzhaColor == 2)
        {
            Data_cs.PearlHandicapSprite.spriteName = "bg_line_1";
        }
        Data_cs.LabelNumber.text = "";
        Data.transform.localPosition = new Vector3(-45.5f + (ZadzhaSeatX * 31), 69 - (30.8f * ZadzhaSeatY), 1);
        Data.transform.localScale = new Vector3(1, 1, 1);

        if (ZadzhaSeatY >= 5)
        {
            Data_cs.PearlHandicapSprite.color = new Color32(255, 255, 255, 0);
            if (ZadzhaSeatY == 5)
            {
                GameObject DataPHL = Instantiate(PHLGameObject);
                DataPHL.transform.parent = ZadzhaTable.transform;
                DataPHL.transform.localScale = new Vector3(1, 1, 1);
                DataPHL.name = ZadzhaSeatX.ToString();
                PHLabel DataPHL_cs = DataPHL.GetComponent<PHLabel>();
                DataPHL_cs.LabelNumber.text = (ZadzhaSeatY + 1).ToString();
                DataPHL.transform.localPosition = new Vector3(-41.5f + (ZadzhaSeatX * 31), 69.5f - (31.5f * ZadzhaSeatY), 1);
                SaveZadzhaTableData.Add(ZadzhaSeatX, DataPHL);
            }
            else
            {
                PHLabel PHLData = SaveZadzhaTableData[ZadzhaSeatX].GetComponent<PHLabel>();
                PHLData.LabelNumber.text = (ZadzhaSeatY + 1).ToString();
            }
        }
    }

    void DataInitVoid()
    {
        HistoryOpenBool = false;
        NowHistoryNumber = 0;
        ParkwaySeatX = 0;
        ParkwaySeatY = 0;
        PopeyesSeatX = 0;
        PopeyesSeatY = 0;
        LaneSeatX = 0;
        LaneSeatY = 0;
        ZadzhaSeatX = 0;
        ZadzhaSeatY = 0;
        PopeyesSaveColor = 0;
        LaneColor = 0;
        ZadzhaColor = 0;
        SaveParkwayTableData.Clear();
        SavePopeyesTableData.Clear();
        SaveLaneTableData.Clear();
        SaveZadzhaTableData.Clear();

        Transform[] Objs = PearlHandicapTable.GetComponentsInChildren<Transform>();
        int Len = Objs.Length;
        for (int i = 0; i < Len; i++)
        {
            if (Objs[i].name != "PearlHandicap_Control")
            {
                Destroy(Objs[i].gameObject);
            }
        }
        Transform[] Objs1 = ParkwayTable.GetComponentsInChildren<Transform>();
        int Len1 = Objs1.Length;
        for (int i = 0; i < Len1; i++)
        {
            if (Objs1[i].name != "Parkway_Control")
            {
                Destroy(Objs1[i].gameObject);
            }
        }
        Transform[] Objs2 = PopeyesTable.GetComponentsInChildren<Transform>();
        int Len2 = Objs2.Length;
        for (int i = 0; i < Len2; i++)
        {
            if (Objs2[i].name != "Popeyes_Control")
            {
                Destroy(Objs2[i].gameObject);
            }
        }
        Transform[] Objs3 = LaneTable.GetComponentsInChildren<Transform>();
        int Len3 = Objs3.Length;
        for (int i = 0; i < Len3; i++)
        {
            if (Objs3[i].name != "Lane_Control")
            {
                Destroy(Objs3[i].gameObject);
            }
        }
        Transform[] Objs4 = ZadzhaTable.GetComponentsInChildren<Transform>();
        int Len4 = Objs4.Length;
        for (int i = 0; i < Len4; i++)
        {
            if (Objs4[i].name != "Zadzha_Control")
            {
                Destroy(Objs4[i].gameObject);
            }
        }
        HistoryData.Clear();
        HistoryParkwayData.Clear();
    }
}
