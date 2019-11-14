using UnityEngine;
using System.Collections;
using GameCore.Manager.SicBo;
using GameCore.Machine;
using GameCore;

public class SicBoTableUnit : MonoBehaviour
{
    public uint ID;
    public uint MenberCnt = 1000;
    uint MaxMemberCnt = 0;
    public uint MachineID = 0;
    public ushort m_MachineType = 0;
    public UISprite[] MachineNumber = new UISprite[2];      //桌檯ID
    public UISprite[] MachineMemnerNumber = new UISprite[4];//桌檯人數
    public GameObject[] MemberObjs = new GameObject[20];    //桌檯人頭圖

    public UISprite m_Sprite;
    public BoxCollider m_Collider;
    public uint BuyInMoney = 100000;

    // Use this for initialization
    void Start()
    {
        m_Sprite = transform.GetComponent<UISprite>();
        m_Collider = transform.GetComponent<BoxCollider>();
        MachineNumber[0] = transform.GetChild(0).GetChild(0).transform.GetComponent<UISprite>();
        MachineNumber[1] = transform.GetChild(0).GetChild(1).transform.GetComponent<UISprite>();
        MachineMemnerNumber[0] = transform.GetChild(1).GetChild(0).transform.GetComponent<UISprite>();
        MachineMemnerNumber[1] = transform.GetChild(1).GetChild(1).transform.GetComponent<UISprite>();
        MachineMemnerNumber[2] = transform.GetChild(2).GetChild(0).transform.GetComponent<UISprite>();
        MachineMemnerNumber[3] = transform.GetChild(2).GetChild(1).transform.GetComponent<UISprite>();
        for (int i = 0; i < MemberObjs.Length; i++)
            MemberObjs[i] = transform.GetChild(3).GetChild(i).gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        if (SicBoManager.MachineList.Count != 0 && SicBoManager.MachineList.Count > (SicBoLobby.Inst.TableControl.NowPage * 2 + ID))
        {
            MachineID = SicBoManager.MachineList[(int)(SicBoLobby.Inst.TableControl.NowPage * 2 + ID)];
            ShowMachine();
        }
        else
        {
            HideManchine();
        }

        if (m_MachineType != SicBoManager.NowGroup)
        {
            m_MachineType = SicBoManager.NowGroup;
            ChangeImage();
        }
    }
    void HideManchine()
    {
        m_Sprite.enabled = false;
        m_Collider.enabled = false;
        MachineNumber[0].enabled = false;
        MachineNumber[1].enabled = false;
        MachineMemnerNumber[0].enabled = false;
        MachineMemnerNumber[1].enabled = false;
        MachineMemnerNumber[2].enabled = false;
        MachineMemnerNumber[3].enabled = false;
        MenberCnt = 0;
        for (int i = 0; i < 20; i++)
        {
            MemberObjs[i].SetActive(false);
        }
    }

    void ShowMachine()
    {
        m_Sprite.enabled = true;
        m_Collider.enabled = true;
        MachineNumber[0].enabled = true;
        MachineNumber[1].enabled = true;
        MachineMemnerNumber[0].enabled = true;
        MachineMemnerNumber[1].enabled = true;
        MachineMemnerNumber[2].enabled = true;
        MachineMemnerNumber[3].enabled = true;
        int One = (int)MachineID % 10;
        int Ten = (int)(MachineID / 10) % 10;

        int MaxMemberOne = (int)MaxMemberCnt % 10;
        int MaxMemberTen = (int)(MaxMemberCnt / 10) % 10;
        MachineNumber[0].spriteName = "numberA_" + One;
        MachineNumber[1].spriteName = "numberA_" + Ten;

        MachineMemnerNumber[2].spriteName = "numberB_" + MaxMemberOne;
        MachineMemnerNumber[3].spriteName = "numberB_" + MaxMemberTen;

        ChangeMember();
    }

    void ChangeImage()
    {
        m_Sprite.spriteName = "table" + m_MachineType;
        MaxMemberCnt = SicBoManager.m_MachineBuyInConfig.m_dicTableGroupSet[m_MachineType].m_usCapacity;
    }

    void ChangeMember()
    {
        MenberCnt = SicBoManager.m_MachineDatas[MachineID].m_usMemberCnt;
        int MemberOne = (int)MenberCnt % 10;
        int MemberTen = (int)(MenberCnt / 10) % 10;
        MachineMemnerNumber[0].spriteName = "numberB_" + MemberOne;
        MachineMemnerNumber[1].spriteName = "numberB_" + MemberTen;
        for (int i = 0; i < 20; i++)
        {
            if (MenberCnt > i)
                MemberObjs[i].SetActive(true);
            else
                MemberObjs[i].SetActive(false);
        }
    }

    void ClickSelf()
    {
        if (SicBoManager.MachineInfo.m_uiTID != MachineID)
            OnClickOnce();
        else
            OnClickDouble();
    }

    void OnClickOnce()
    {
        GameConnet.m_oGameClient.Send(GameCore.ENUM_GAME_FRAME.SicBo, (uint)ENUM_SICBO_PACKID_GC.C2G_Machine_GetMachineInfo, GameConnet.m_oGameClient.DoSerialize<uint>(MachineID));
        SicBoLobby.Inst.NameList.ClickTarget = ID;
    }

    void OnClickDouble()
    {   //如果玩家等級大於BuyIn等級
        if (MainConnet.m_PlayerData.m_usLv >= SicBoManager.m_MachineBuyInConfig.m_usBuyinLv)
        {   //如果玩家的金錢大於MaxBuyinMoney，BuyinMoney = MaxBuyinMoney
            if (MainConnet.m_PlayerData.m_ui64OwnMoney >= SicBoManager.m_MachineBuyInConfig.m_uiMaxBuyinMoney)
                BuyInMoney = SicBoManager.m_MachineBuyInConfig.m_uiMaxBuyinMoney;
            else  //否則，BuyinMoney = 玩家金錢        
                BuyInMoney = (uint)MainConnet.m_PlayerData.m_ui64OwnMoney;

            if (BuyInMoney >= SicBoManager.m_MachineBuyInConfig.m_uiMinBuyinMoney)
            {   //BuyinMoney >= MinBuyinMoney，Buyin
                CPACK_TMachineBuyin m_BuyInMoney = new CPACK_TMachineBuyin();
                m_BuyInMoney.m_uiTID = MachineID;
                m_BuyInMoney.m_uiBuyinMoney = BuyInMoney;
                Debug.Log("要BUYIN的機台 : " + m_BuyInMoney.m_uiTID + " 金錢 : " + m_BuyInMoney.m_uiBuyinMoney);
                AllScenceLoad.LoadScence = true;
                GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.SicBo, (uint)ENUM_COMMON_PACKID_GC.C2G_TMachine_ReqBuyin, GameConnet.m_oGameClient.DoSerialize<CPACK_TMachineBuyin>(m_BuyInMoney));
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