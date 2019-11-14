using UnityEngine;
using System.Collections;
using GameCore.Manager.Common;
using GameCore;
using GameCore.Machine;

public class SicBoLobby : MonoBehaviour
{
    public static SicBoLobby Inst;
    public SicBoTableInfo TableInfo;    //桌檯資訊
    public SicBoTableControl TableControl;  //桌檯控制
    public SicBoNameListControl NameList;   //玩家清單
    private UISprite LobbyBackground;   //大廳背景
    private UISprite AnteBackground;    //底台背景
    private UISprite LowAnteTabBackground;  //小底台頁籤背景
    private UISprite HighAnteTabBackground; //大底台頁籤背景
    public bool LobbyStart = false;

    void Awake()
    {
        Inst = this;
    }

    // Use this for initialization
    void Start()
    {
        LobbyBackground = transform.GetChild(0).GetComponent<UISprite>();    //大廳背景
        AnteBackground = transform.GetChild(1).GetChild(0).GetComponent<UISprite>();    //底台背景
        LowAnteTabBackground = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<UISprite>();  //小底台頁籤
        HighAnteTabBackground = transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<UISprite>(); //大底台頁籤
    }

    // Update is called once per frame
    void Update()
    {
        if (LobbyStart)
        {
            LobbyStart = false;
            LobbyBackgroundSetting();
        }
    }

    void LobbyBackgroundSetting()
    {
        LobbyBackground.spriteName = "bg" + SicBoManager.NowGroup; //大廳背景
        AnteBackground.spriteName = "tab_paging" + SicBoManager.NowGroup + "_1";    //底台背景
        if (SicBoManager.NowGroup == 1)
        {
            LowAnteTabBackground.enabled = false;   //小底台頁籤隱藏
            HighAnteTabBackground.enabled = true;   //大底台頁籤顯示
        }
        else if (SicBoManager.NowGroup == 2)
        {
            LowAnteTabBackground.enabled = true;   //小底台頁籤顯示
            HighAnteTabBackground.enabled = false;   //大底台頁籤隱藏
        }
    }

    public void ClickAnteLow()
    {
        SicBoManager.NowGroup = 1;
        TableControl.SortType = 0;
        LobbyBackgroundSetting();
        TableControl.ChangeMachineClass = true;
    }

    void ClickAnteHigh()
    {
        SicBoManager.NowGroup = 2;
        TableControl.SortType = 0;
        LobbyBackgroundSetting();
        TableControl.ChangeMachineClass = true;
    }

    void ClickBackButton()
    {
        GameConnet.CloseGameConnet();
    }

    void ClickAutoBuyIn()
    {   //如果玩家等級大於BuyIn等級
        if (MainConnet.m_PlayerData.m_usLv >= SicBoManager.m_MachineBuyInConfig.m_usBuyinLv)
        {
            uint BuyInMoney = 0;
            //如果玩家的金錢大於MaxBuyinMoney，BuyinMoney = MaxBuyinMoney
            if (MainConnet.m_PlayerData.m_ui64OwnMoney >= SicBoManager.m_MachineBuyInConfig.m_uiMaxBuyinMoney)
                BuyInMoney = SicBoManager.m_MachineBuyInConfig.m_uiMaxBuyinMoney;
            else  //否則，BuyinMoney = 玩家金錢        
                BuyInMoney = (uint)MainConnet.m_PlayerData.m_ui64OwnMoney;
            BuyInMoney = 100000;
            if (BuyInMoney >= SicBoManager.m_MachineBuyInConfig.m_uiMinBuyinMoney)
            {
                SicBoManager.AutoBuyInMoney = BuyInMoney;
                CPACK_TMachineAutoSelect Data = new CPACK_TMachineAutoSelect();
                Data.m_uiStartTID = SicBoManager.m_MachineBuyInConfig.m_dicTableGroupSet[SicBoManager.NowGroup].m_uiStartTableID;
                Data.m_uiEndTID = SicBoManager.m_MachineBuyInConfig.m_dicTableGroupSet[SicBoManager.NowGroup].m_uiEndTableID;
                Debug.Log("NowGroup：" + SicBoManager.NowGroup + "  StartID：" + Data.m_uiStartTID + "  EndID：" + Data.m_uiEndTID);
                AllScenceLoad.LoadScence = true;
                GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.SicBo, (uint)ENUM_COMMON_PACKID_GC.C2G_TMachine_AutoSelect, GameConnet.m_oGameClient.DoSerialize<CPACK_TMachineAutoSelect>(Data));
            }
            else //否則顯示提示訊息
            {
                Message_Control.OpenMessage = true;
                Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                Message_Control.MessageStatus = Message_Control.MessageStatu.Buyin_MoneyNotEnough;
            }
        }
        else //否則顯示提示訊息
        {
            Message_Control.OpenMessage = true;
            Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
            Message_Control.MessageStatus = Message_Control.MessageStatu.Buyin_LVNotEnough;
        }
    }
}