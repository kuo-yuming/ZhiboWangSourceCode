using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore.Manager.Common;
using GameCore;
using GameCore.Machine;
using GameEnum;

public class RaceButton : MonoBehaviour {
    public ENUM_RACE_STATE ButtonType = ENUM_RACE_STATE.End;
    public bool ButtonSigned_Bool = false;
    public ushort ButtonID = 0;
    public ENUM_RACE_FEE_TYPE FeeType = ENUM_RACE_FEE_TYPE.Money;
    public ENUM_RACE_EVENT_TYPE RaceEventType = ENUM_RACE_EVENT_TYPE.Buying;
    public uint FeeVal = 0;
    public string RaceButtonName = "";
    public string RaceButtonTime = "";
    public ENUM_PUBLIC_BUTTON RaceButtontype = ENUM_PUBLIC_BUTTON.AutoMinus;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnClick()
    {
        if (!Competition.RaceButtonClick)
        {
            if (RaceButtontype == ENUM_PUBLIC_BUTTON.RaceNoData)
            {
                RaceWindowBox.RaceID = ButtonID;
                if (ButtonType == ENUM_RACE_STATE.Sign && !ButtonSigned_Bool)
                {
                    if (RaceEventType == ENUM_RACE_EVENT_TYPE.Once)
                    {
                        RaceWindowBox.RaceWindowState = 1;
                    }
                    else
                    {
                        RaceWindowBox.RaceWindowState = 2;
                    }
                    RaceWindowBox.LabelString_State2 = RaceButtonName;
                    RaceWindowBox.LabelString_State4 = RaceButtonTime;
                    RaceWindowBox.BoxFeeType = FeeType;
                    RaceWindowBox.FeeVal = FeeVal;
                    Debug.Log("報名");
                }
                else if (ButtonType == ENUM_RACE_STATE.Sign && ButtonSigned_Bool)
                {
                    RaceWindowBox.RaceWindowState = 3;
                    Debug.Log("取消報名");
                }
                else if (ButtonType == ENUM_RACE_STATE.WaitStart || ButtonType == ENUM_RACE_STATE.Racing)
                {
                    if (!ButtonSigned_Bool)
                    {
                        RaceWindowBox.RaceWindowState = 4;
                        RaceWindowBox.CodeID = 201;
                    }
                    else
                    {
                        AllScenceLoad.LoadScence = true;
                        GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Baccarat, (uint)ENUM_COMMON_PACKID_GC.C2G_Race_ReqGetTableID, GameConnet.m_oGameClient.DoSerialize<ushort>(ButtonID));
                        RaceWindowBox.RaceID = ButtonID;
                        RaceWindowBox.BoxEventType = RaceEventType;
                        Competition.RaceButtonClick = true;
                    }
                    Debug.Log("進入比賽" + " //ID: " + ButtonID);
                }
                else if (ButtonType == ENUM_RACE_STATE.End)
                {
                    GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Baccarat, (uint)ENUM_COMMON_PACKID_GC.C2G_Race_ReqGetRanking, GameConnet.m_oGameClient.DoSerialize<ushort>(ButtonID));
                    Competition.RaceButtonClick = true;
                    Debug.Log("排行紀錄// " + "按鈕ID: " + ButtonID);
                }
            }
            else if (RaceButtontype == ENUM_PUBLIC_BUTTON.RaceExplain)
            {
                RaceInfo.RaceInfoObject_bool = true;
                RaceInfo.RaceID = ButtonID;
                Debug.Log("賽事資料// " + "按鈕ID: " + ButtonID);
            }
        }   
    }
}
