using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore.Machine;
using GameCore;

public class RaceEnterButton : MonoBehaviour {
    public enum ButtonType
    {
        EnterButton = 0,
        CancelButton = 1,
        BoxEnterButton = 2,
    }

    public ButtonType m_ButtonType;

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
            if (m_ButtonType == ButtonType.EnterButton)
            {
                if (RaceWindowBox.RaceWindowState == 1)
                {
                    if (RaceWindowBox.FeeVal != 0)
                    {
                        RaceWindowBox.RaceWindowState = 2;
                    }
                    else
                    {
                        Competition.RaceButtonClick = true;
                        GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Baccarat, (uint)ENUM_COMMON_PACKID_GC.C2G_Race_ReqSignEvent, GameConnet.m_oGameClient.DoSerialize<ushort>(RaceWindowBox.RaceID));
                    }
                }
                else if (RaceWindowBox.RaceWindowState == 2)
                {
                    Competition.RaceButtonClick = true;
                    if (MainConnet.m_PlayerData.m_usLv >= BaccaratManager.m_MachineBuyInConfig.m_usBuyinLv)
                    {
                        GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Baccarat, (uint)ENUM_COMMON_PACKID_GC.C2G_Race_ReqSignEvent, GameConnet.m_oGameClient.DoSerialize<ushort>(RaceWindowBox.RaceID));
                    }
                    else
                    {
                        Message_Control.OpenMessage = true;
                        Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                        Message_Control.MessageStatus = Message_Control.MessageStatu.Buyin_LVNotEnough;
                        Competition.RaceButtonClick = false;
                    }
                }
                else if (RaceWindowBox.RaceWindowState == 3)
                {
                    GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Baccarat, (uint)ENUM_COMMON_PACKID_GC.C2G_Race_ReqCancelSignEvent, GameConnet.m_oGameClient.DoSerialize<ushort>(RaceWindowBox.RaceID));
                    Competition.RaceButtonClick = true;
                }
                else if (RaceWindowBox.RaceWindowState == 4)
                {
                    RaceWindowBox.RaceWindowState = 0;
                }
            }
            else if (m_ButtonType == ButtonType.CancelButton)
            {
                RaceWindowBox.RaceWindowState = 0;
                Competition.RaceButtonClick = false;
            }
            else if (m_ButtonType == ButtonType.BoxEnterButton)
            {
                AllScenceLoad.LoadScence = true;
                CPACK_TMachineBuyin m_BuyInMoney = new CPACK_TMachineBuyin();
                m_BuyInMoney.m_uiTID = RaceWindowBox.MoneyBoxID;
                m_BuyInMoney.m_uiBuyinMoney = (uint)RaceWindowBox.MoneyBoxMoney;
                GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Baccarat, (uint)ENUM_COMMON_PACKID_GC.C2G_TMachine_ReqBuyin, GameConnet.m_oGameClient.DoSerialize<CPACK_TMachineBuyin>(m_BuyInMoney));
            }
        }
    }
}
