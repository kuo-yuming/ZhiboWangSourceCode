using UnityEngine;
using System.Collections;
using BankEnum;
using GameCore;
using GameCore.Manager.Common;

public class BankMainPage : MonoBehaviour {
    public enum ENUM_BANK_MAIN_BUTTON
    {
        BusinessButton,
        KeyReviseButton,
        PointChangeButton,
        GiftButton,
        ToMainButton,
        KeyForgetButton,
        WaitTimeButton,
    }

    public ENUM_BANK_MAIN_BUTTON m_MainButton;
    public static bool BankMainPageOnClickBool = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (BankMainPageOnClickBool)
        {
            m_MainButton = ENUM_BANK_MAIN_BUTTON.BusinessButton;
            OnClick();
            BankMainPageOnClickBool = false;
        }
	}

    void OnClick()
    {
        if (m_MainButton == ENUM_BANK_MAIN_BUTTON.BusinessButton)
        {
            if (MainConnet.m_PlayerData.m_usLv >= Bank_Manager.m_CPACK_TransactionSysConfig.m_usNeedLv)
            {
                if (MainConnet.m_PlayerData.m_byApprovedType == (byte)BaseAttr.ENUM_APPROVED_TYPE.Phone || MainConnet.m_PlayerData.m_byApprovedType == (byte)BaseAttr.ENUM_APPROVED_TYPE.MailPhone)
                {
                    if (MainConnet.m_PlayerData.m_bSetBankPw)
                    {
                        Business_Control.PassEnactment = true;
                    }
                    else
                    {
                        Business_Control.PassEnactment = false;
                    }
                   
                    Bank_Control.BankPage = (byte)ENUM_BANK_PAGE.BusinessPage;
                }
                else
                {
                    BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.PhoneNoClear;
                    BankMessageBox.m_BankMessageBoxOpen = true;
                }
            }
            else
            {
                BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.LevelNoClear;
                BankMessageBox.m_BankMessageBoxOpen = true;
            }
        }
        else if (m_MainButton == ENUM_BANK_MAIN_BUTTON.KeyReviseButton)
        {
            if (MainConnet.m_PlayerData.m_usLv >= Bank_Manager.m_CPACK_TransactionSysConfig.m_usNeedLv)
            {
                if (MainConnet.m_PlayerData.m_bSetBankPw)
                {
                    Bank_Control.BankPage = (byte)ENUM_BANK_PAGE.KeyRevisePage;
                }
                else
                {
                    BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.PassNoClear;
                    BankMessageBox.m_BankMessageBoxOpen = true;
                }

            }
            else
            {
                BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.LevelNoClear;
                BankMessageBox.m_BankMessageBoxOpen = true;
            }
        }
        else if (m_MainButton == ENUM_BANK_MAIN_BUTTON.PointChangeButton)
        {
            Bank_Control.BankPage = (byte)ENUM_BANK_PAGE.PointChangePage;
            MainConnet.m_oMainClient.Send(ENUM_GAME_FRAME.Common, (uint)ENUM_COMMON_PACKID_GC.C2G_Transaction_ReqGetPoint,
                                      null);
            Debug.Log("傳送成功");
        }
        else if (m_MainButton == ENUM_BANK_MAIN_BUTTON.GiftButton)
        {
            if (VersionDef.CloseBuyGameMoney)
            {
                if (VersionDef.BuyGameMoney)
                {
                    if (MainConnet.DemoPlayer)
                    {
                        Message_Control.OpenMessage = true;
                        Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                        Message_Control.MessageStatus = Message_Control.MessageStatu.BankDemoPlayer;
                    }
                    else if (MainConnet.m_PlayerData.m_byVIPType == (byte)BaseAttr.ENUM_VIP_TYPE.Rookie)
                    {
                        Message_Control.OpenMessage = true;
                        Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_M;
                        Message_Control.MessageStatus = Message_Control.MessageStatu.BankPlayerVIPType;
                        Bank_Control.BankPage = (byte)ENUM_BANK_PAGE.GiftPage;
                    }
                    else
                    {
                        Bank_Control.BankPage = (byte)ENUM_BANK_PAGE.GiftPage;
                    }
                }
                else
                {
                     Net.ConnetToWeb(MainConnet.OpenWebStatus.deposit_myCard, true);
                  
                }
            }
            else
            {
                Message_Control.OpenMessage = true;
                Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
                Message_Control.MessageStatus = Message_Control.MessageStatu.Actor_CloseMoney;
                return;
            }
           
        }
        else if (m_MainButton == ENUM_BANK_MAIN_BUTTON.ToMainButton)
        {
            if (!CashBusiness.BusinessEnd)
            {
                if ((Bank_Control.BankPage == (byte)ENUM_BANK_PAGE.BusinessPage && Business_Control.PassEnter) || CashBusiness.BusinessPlayerName != "")
                {
                    MainConnet.m_oMainClient.Send(ENUM_GAME_FRAME.Common, (uint)ENUM_COMMON_PACKID_GC.C2G_Transaction_ReqCancelTransaction,
                              null);
                }
                if (Bank_Control.BankPage == (byte)ENUM_BANK_PAGE.BusinessPage)
                {
                    Business_Control.BusinessDataInit = true;
                }
                Bank_Control.BankPage = (byte)ENUM_BANK_PAGE.MainPage;
                CashBusiness.BusinessPlayerName = "";
                CashBusiness.PlayerDBID = 0;
            }
            else
            {
                CashBusiness.BusinessPlayerName = "";
                CashBusiness.PlayerDBID = 0;
                Business_Control.BusinessDataInit = true;
                Bank_Control.BankPage = (byte)ENUM_BANK_PAGE.MainPage;
            }
        }
        else if (m_MainButton == ENUM_BANK_MAIN_BUTTON.KeyForgetButton)
        {
            Bank_Control.BankPage = (byte)ENUM_BANK_PAGE.KeyForgetPage;
        }
        else if (m_MainButton == ENUM_BANK_MAIN_BUTTON.WaitTimeButton)
        {
            MainConnet.m_oMainClient.Send(ENUM_GAME_FRAME.Common, (uint)ENUM_COMMON_PACKID_GC.C2G_Transaction_ReqCancelTransaction,
                        null);
            Business_Control.BusinessWaitTime = false;
        }
        Bank_Control.BankPageCheck = true;
    }
}
