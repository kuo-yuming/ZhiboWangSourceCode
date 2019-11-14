using UnityEngine;
using System.Collections;
using GameCore.Manager.Common;
using BankEnum;
public class PointChange : MonoBehaviour {

    public ENUM_TRANSACTION_POINT2MONEY m_Enum_Button;
    public static byte SaveChangePointNumber = 0;
    public static bool WaitChangePoint = false;
	// Use this for initialization
	void Start () {
        SaveChangePointNumber = 0;
        WaitChangePoint = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        if (!WaitChangePoint)
        {
            if (m_Enum_Button == ENUM_TRANSACTION_POINT2MONEY.P10ToM1000)
            {
                if (Bank_Control.PlayerPoint >= 10)
                {
                    SaveChangePointNumber = (byte)ENUM_TRANSACTION_POINT2MONEY.P10ToM1000;
                    BankMessageBox.m_BankMessageBoxOpen = true;
                    BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.CheckPoint;
                }
                else
                {
                    BankMessageBox.m_BankMessageBoxOpen = true;
                    BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.NoPoint;
                }
            }
            else if (m_Enum_Button == ENUM_TRANSACTION_POINT2MONEY.P50ToM5000)
            {
                if (Bank_Control.PlayerPoint >= 50)
                {
                    SaveChangePointNumber = (byte)ENUM_TRANSACTION_POINT2MONEY.P50ToM5000;
                    BankMessageBox.m_BankMessageBoxOpen = true;
                    BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.CheckPoint;
                }
                else
                {
                    BankMessageBox.m_BankMessageBoxOpen = true;
                    BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.NoPoint;
                }
            }
            else if (m_Enum_Button == ENUM_TRANSACTION_POINT2MONEY.P100ToM10000)
            {
                if (Bank_Control.PlayerPoint >= 100)
                {
                    SaveChangePointNumber = (byte)ENUM_TRANSACTION_POINT2MONEY.P100ToM10000;
                    BankMessageBox.m_BankMessageBoxOpen = true;
                    BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.CheckPoint;
                }
                else
                {
                    BankMessageBox.m_BankMessageBoxOpen = true;
                    BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.NoPoint;
                }
            }
            else if (m_Enum_Button == ENUM_TRANSACTION_POINT2MONEY.P500ToM50000)
            {
                if (Bank_Control.PlayerPoint >= 500)
                {
                    SaveChangePointNumber = (byte)ENUM_TRANSACTION_POINT2MONEY.P500ToM50000;
                    BankMessageBox.m_BankMessageBoxOpen = true;
                    BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.CheckPoint;
                }
                else
                {
                    BankMessageBox.m_BankMessageBoxOpen = true;
                    BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.NoPoint;
                }
            }
            else if (m_Enum_Button == ENUM_TRANSACTION_POINT2MONEY.P1000ToM100000)
            {
                if (Bank_Control.PlayerPoint >= 1000)
                {
                    SaveChangePointNumber = (byte)ENUM_TRANSACTION_POINT2MONEY.P1000ToM100000;
                    BankMessageBox.m_BankMessageBoxOpen = true;
                    BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.CheckPoint;
                }
                else
                {
                    BankMessageBox.m_BankMessageBoxOpen = true;
                    BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.NoPoint;
                }
            }
        }
    }
}
