using UnityEngine;
using System.Collections;
using BankEnum;
using GameCore;
public class BankMessageBoxButton : MonoBehaviour {
    public enum ENUM_BANK_MESSAGE_BUTTON
    {
        PointNo,
        PointOk,

    }

    public ENUM_BANK_MESSAGE_BUTTON m_Button;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        if (m_Button == ENUM_BANK_MESSAGE_BUTTON.PointNo)
        {
            PointChange.SaveChangePointNumber = 0;
        }
        else if (m_Button == ENUM_BANK_MESSAGE_BUTTON.PointOk)
        {
            if (BankMessageBox.m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.CheckPoint)
            {
                MainConnet.m_oMainClient.Send(ENUM_GAME_FRAME.Common, (uint)ENUM_COMMON_PACKID_GC.C2G_Transaction_ReqPoint2Money,
                                   MainConnet.m_oMainClient.DoSerialize<byte>(PointChange.SaveChangePointNumber));
                PointChange.WaitChangePoint = true;
            }
            else if (BankMessageBox.m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.NoPoint)
            {
             
            }
            else if (BankMessageBox.m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.KeyForgetSuccess)
            {
                Bank_Control.BankPage = (byte)ENUM_BANK_PAGE.MainPage;
                Bank_Control.BankPageCheck = true;
            }
            else if (BankMessageBox.m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.PhoneNoClear)
            {
                if (CashBusiness.BusinessPlayerName != "")
                {
                    MainConnet.m_oMainClient.Send(ENUM_GAME_FRAME.Common, (uint)ENUM_COMMON_PACKID_GC.C2G_Transaction_ReqCancelTransaction,
                         null);
                }
                Net.ConnetToWeb(MainConnet.OpenWebStatus.Member_main, true);
            }
            else if (BankMessageBox.m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.LevelNoClear)
            {
                if (CashBusiness.BusinessPlayerName != "")
                {
                    MainConnet.m_oMainClient.Send(ENUM_GAME_FRAME.Common, (uint)ENUM_COMMON_PACKID_GC.C2G_Transaction_ReqCancelTransaction,
                         null);
                }
            }
            else if (BankMessageBox.m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.BusinessCancel)
            {
                Bank_Control.BankPage = (byte)ENUM_BANK_PAGE.MainPage;
                Business_Control.BusinessDataInit = true;
                Bank_Control.BankPageCheck = true;
                CashBusiness.BusinessPlayerName = "";
                CashBusiness.PlayerDBID = 0;
            }
        }

        if (BankMessageBox.SaveBankError.Count > 0)
        {
            BankMessageBox.m_MsessageBoxStatus = (ushort)BankMessageBox.SaveBankError[0];
            BankMessageBox.SaveBankError.Clear();
        }
        else
        {
            BankMessageBox.m_BankMessageBoxOpen = false;
        }
    }
}
