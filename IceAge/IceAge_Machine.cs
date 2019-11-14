using UnityEngine;
using System.Collections;
using GameCore.Machine;
using GameCore;
using GameCore.Manager.IceAge;

public class IceAge_Machine : MonoBehaviour {
    public ushort ID = 0;
    ushort MaxID = 0;
    public uint NowMachineID = 0;
    int Page = int.MaxValue;
    bool M_enabled = true;
    public MachineType MachineStatus = MachineType.Idle;
    BoxCollider m_Collider = null;
    public UISprite m_Sprite;
    public UISprite m_Hundred;
    public UISprite m_Ten;
    public UISprite m_One;

    public enum MachineType
    {
        Idle,
        OnChoose,
        Other,
        OtherPlay,
        MyKeep,
    }
    // Use this for initialization
    void Start () {
        m_Collider = GetComponent<BoxCollider>();
        //SetUp();
    }
	
	// Update is called once per frame
	void Update () {
        if (IceAgeManager.LoadGameEnd)
        {
            if (GameConnet.m_PMachineConfig != null)
            {
                if (MaxID < ID || NowMachineID > GameConnet.m_PMachineConfig.m_uiMaxMachineCnt)
                {
                    m_Sprite.enabled = false;
                    m_Collider.enabled = false;
                    M_enabled = false;
                    m_Hundred.enabled = false;
                    m_Ten.enabled = false;
                    m_One.enabled = false;
                }
                else
                {
                    m_Sprite.enabled = true;
                    m_Collider.enabled = true;
                    M_enabled = true;
                    m_Hundred.enabled = true;
                    m_Ten.enabled = true;
                    m_One.enabled = true;
                }
            }
            if (Page != IceAgeManager.NowPage)
            {
                Page = IceAgeManager.NowPage;
                SetUp();
            }
            if (M_enabled)
            {
                if (GameConnet.m_PMachinesData.ContainsKey(NowMachineID))
                {
                    CPACK_PMachineData Data = GameConnet.m_PMachinesData[NowMachineID];
                    switch (Data.m_enumState)
                    {
                        case ENUM_PMACHINE_STATE.Idle:
                            if (MachineStatus != MachineType.Idle && MachineStatus != MachineType.OnChoose)
                            {
                                MachineStatus = MachineType.Idle;
                            }
                            if (MachineStatus == MachineType.Idle)
                            {
                                m_Sprite.spriteName = "Machine_Idle";
                            }
                            else if (MachineStatus == MachineType.OnChoose)
                            {
                                m_Sprite.spriteName = "Machine_OnChoose";
                            }
                            break;
                        case ENUM_PMACHINE_STATE.BuyinChk:
                            break;
                        case ENUM_PMACHINE_STATE.Use:
                            MachineStatus = MachineType.OtherPlay;
                            m_Sprite.spriteName = "Machine_OtherInSeat";
                            break;
                        case ENUM_PMACHINE_STATE.Keep:
                            if (GameConnet.m_uiKeepMID != NowMachineID)
                            {
                                MachineStatus = MachineType.Other;
                                m_Sprite.spriteName = "Machine_OtherSeat";
                            }
                            else
                            {
                                MachineStatus = MachineType.MyKeep;
                                m_Sprite.spriteName = "Machine_YouSeat";
                            }
                            break;
                        default:
                            break;
                    }

                }
            }
            if (Input.GetMouseButton(0))
            {
                if (MachineStatus == MachineType.OnChoose)
                    MachineStatus = MachineType.Idle;
            }

        }
        
    }
    public void SetUp()
    {
        MaxID = GameConnet.m_PMachineConfig.m_uiPageMachineCnt;
        NowMachineID = (uint)IceAgeManager.NowPage * MaxID + ID;
        Machine_Number();
    }
    public void OnClick()
    {
        Debug.Log("OnClick : " + ID);
        if ((GameConnet.m_PMachinesData[NowMachineID].m_enumState == ENUM_PMACHINE_STATE.Idle || MachineStatus == MachineType.MyKeep) && IceAgeManager.m_MachineInfo.m_uiMID == NowMachineID)
        {
            IceAgeManager.BuyInGame(NowMachineID);
            //幫她BuyIn
        }
        else if (MachineStatus == MachineType.Idle)
        {
            MachineStatus = MachineType.OnChoose;
            GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.IceAge, (uint)ENUM_ICEAGE_PACKID_GC.C2G_Machine_GetMachineInfo, GameConnet.m_oGameClient.DoSerialize<uint>(NowMachineID));
        }
        else
        {
            GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.IceAge, (uint)ENUM_ICEAGE_PACKID_GC.C2G_Machine_GetMachineInfo, GameConnet.m_oGameClient.DoSerialize<uint>(NowMachineID));
        }
       
    }
    //顯示機台號碼
    void Machine_Number()
    {
        m_Hundred.spriteName = ((((NowMachineID % 1000) - (NowMachineID % 100)) / 100) + "");
        m_Ten.spriteName = ((((NowMachineID % 100) - (NowMachineID % 10)) / 10) + "");
        m_One.spriteName = ((NowMachineID % 10) + "");

    }
    //-------------------------------------------------------------------------
}
