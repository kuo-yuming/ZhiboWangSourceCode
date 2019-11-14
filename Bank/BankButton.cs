using UnityEngine;
using System.Collections;
using GameCore;
using GameCore.Manager.Common;
using BankEnum;

public class BankButton : MonoBehaviour {

    public enum ENUM_BANK_BUTTON
    {
        KeyForgetEnter,
        KeyReviseOld,
        KeyReviseNew,
        KeyReviseAgain,
        KeyReviseEnter,
        BusinessFirstKey,
        BusinessFirstAgainKey,
        BusinessFirstKeyEnter,
        BusinessKey,
        BusinessKeyEnter,
        BusinessFriendList,
        BusinessPlayerList,
        MyCashOut,
        FinallCashOut,
        keyForget,
    }

    public ENUM_BANK_BUTTON m_Button;
    public static bool BankButtonClick = false;
    public static bool BankButtonClick2 = false;
	// Use this for initialization
	void Start () {
        BankButtonClick = false;
        BankButtonClick2 = false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        if (Bank_Control.BankPage == (byte)ENUM_BANK_PAGE.KeyForgetPage)
        {
            if (!BankButtonClick)
            {
                MKeyForgetEnter();
            }
        }
        else if (Bank_Control.BankPage == (byte)ENUM_BANK_PAGE.KeyRevisePage)
        {
            if (m_Button == ENUM_BANK_BUTTON.KeyReviseEnter)
            {
                if (KeyRevise_Control.NewKey == KeyRevise_Control.Againkey)
                {
                    if (!BankButtonClick)
                    {
                        MKeyReviseEnter();
                    }
                }
                else
                {
                    BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.DataClickError;
                    BankMessageBox.m_BankMessageBoxOpen = true;
                }
            }
            else
            {
                MKeyRevise();
            }
        }
        else if (Bank_Control.BankPage == (byte)ENUM_BANK_PAGE.BusinessPage)
        {
            if (m_Button == ENUM_BANK_BUTTON.keyForget)
            {
                CashBusiness.BusinessPlayerName = "";
                CashBusiness.PlayerDBID = 0;
                Business_Control.BusinessDataInit = true;
                Bank_Control.BankPage = (byte)ENUM_BANK_PAGE.KeyForgetPage;
                Bank_Control.BankPageCheck = true;
            }
            if (!Business_Control.BusinessStart)
            {
                if (!Business_Control.PassEnactment)
                {
                    if (!BankButtonClick)
                    {
                        MBusinessFirst();
                        if (m_Button == ENUM_BANK_BUTTON.BusinessFirstKeyEnter)
                        {
                            if (Business_Control.FirstNewKeyString == Business_Control.FirstNewAgainKeyString && Business_Control.FirstNewKeyString != "")
                            {
                                MBusinessFirstEnter();
                            }
                            else
                            {
                                BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.DataClickError;
                                BankMessageBox.m_BankMessageBoxOpen = true;
                            }
                        }
                    }
                }
                else
                {
                    if (!BankButtonClick)
                    {
                        if (m_Button == ENUM_BANK_BUTTON.BusinessKeyEnter)
                        {
                            if (Business_Control.PassKeyString != "")
                            {
                                MBusinessKeyEnter();
                            }
                            else
                            {
                                BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.DataClickError;
                                BankMessageBox.m_BankMessageBoxOpen = true;
                            }
                        }
                    }
                }

                if (Business_Control.PassEnter && !BankButtonClick)
                {

                    BusinessListChange();
                }
            }
            else
            {
                if (m_Button == ENUM_BANK_BUTTON.MyCashOut)
                {
                    int ExportFee = CashBusiness.MyCashOut * Business_Control.BusinessFee / 100;
                    uint CheckCash = (uint)CashBusiness.MyCashOut + (uint)ExportFee + Business_Control.BusinessKeepMoney;
                    //所持金大於0
                    if (CashBusiness.MyCashOut >= 0)
                    {
                        //所持金是否足夠
                        if (MainConnet.m_PlayerData.m_ui64OwnMoney >= (ulong)CashBusiness.MyCashOut)
                        {
                            //超過匯出上限
                            if (CashBusiness.MyCashOut <= VersionDef.BankMaxMoney)
                            {

                                if (MainConnet.m_PlayerData.m_ui64OwnMoney >= CheckCash || CashBusiness.MyCashOut == 0)
                                {
                                    //千元單位
                                    if ((CashBusiness.MyCashOut % 1000) == 0)
                                    {
                                        CPACK_TransactionReqExportItem m_Data = new CPACK_TransactionReqExportItem();
                                        m_Data.m_uiExportMoney = (uint)CashBusiness.MyCashOut;
                                        Debug.Log("匯出金額: " + m_Data.m_uiExportMoney);
                                        MainConnet.m_oMainClient.Send(ENUM_GAME_FRAME.Common, (uint)ENUM_COMMON_PACKID_GC.C2G_Transaction_ReqExportItem,
                                                                                 MainConnet.m_oMainClient.DoSerialize<CPACK_TransactionReqExportItem>(m_Data));
                                        CashBusiness.MyCashOutOk = true;
                                        Business_Control.BusinessWaitTime = true;
                                    }
                                    else
                                    {
                                        BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.Thousand;
                                        BankMessageBox.m_BankMessageBoxOpen = true;
                                        BankButton.BankButtonClick = false;
                                    }
                                }
                                else
                                {
                                    BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.NoHoldCash;
                                    BankMessageBox.m_BankMessageBoxOpen = true;
                                    BankButton.BankButtonClick = false;
                                }
                            }
                            else
                            {
                                BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.BankOutMax;
                                BankMessageBox.m_BankMessageBoxOpen = true;
                                BankButton.BankButtonClick = false;
                            }
                        }
                        else
                        {
                            BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.NoHoldCash;
                            BankMessageBox.m_BankMessageBoxOpen = true;
                            BankButton.BankButtonClick = false;
                        }
                    }
                    else
                    {
                        BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.LessThanZero;
                        BankMessageBox.m_BankMessageBoxOpen = true;
                        BankButton.BankButtonClick = false;
                    }
                }

                if (m_Button == ENUM_BANK_BUTTON.FinallCashOut)
                {
                    MainConnet.m_oMainClient.Send(ENUM_GAME_FRAME.Common, (uint)ENUM_COMMON_PACKID_GC.C2G_Transaction_ReqExecTransaction,
                                                                           null);
                    Business_Control.BusinessWaitTime = true;
                }
            }
        }
    }

    void MKeyForgetEnter()
    {
        if (m_Button == ENUM_BANK_BUTTON.KeyForgetEnter)
        {
            CPACK_TransactionClearBankPw m_Data = new CPACK_TransactionClearBankPw();
            m_Data.m_strMemberAcc = KeyForget_Control.MemberAccountNumber;
            m_Data.m_strMemberPw = KeyForget_Control.MemberKey;
            m_Data.m_strPhone = KeyForget_Control.PhoneNumber;

            MainConnet.m_oMainClient.Send(ENUM_GAME_FRAME.Common, (uint)ENUM_COMMON_PACKID_GC.C2G_Transaction_ReqClearBankPw,
                                MainConnet.m_oMainClient.DoSerialize<CPACK_TransactionClearBankPw>(m_Data));
            BankButtonClick = true;
            Debug.Log("帳號: " + KeyForget_Control.MemberAccountNumber + "  //密碼: " + KeyForget_Control.MemberKey + "  //手機號碼: " + KeyForget_Control.PhoneNumber);
        }
    }

    void MKeyRevise()
    {
        if (m_Button == ENUM_BANK_BUTTON.KeyReviseOld)
        {
            KeyRevise_Control.OldKeyClick = true;
            KeyRevise_Control.NewKeyClick = false;
            KeyRevise_Control.AgainNewClick = false;
        }
        else if (m_Button == ENUM_BANK_BUTTON.KeyReviseNew)
        {
            KeyRevise_Control.OldKeyClick = false;
            KeyRevise_Control.NewKeyClick = true;
            KeyRevise_Control.AgainNewClick = false;
        }
        else if (m_Button == ENUM_BANK_BUTTON.KeyReviseAgain)
        {
            KeyRevise_Control.OldKeyClick = false;
            KeyRevise_Control.NewKeyClick = false;
            KeyRevise_Control.AgainNewClick = true;
        }
    }

    void MKeyReviseEnter()
    {
        if (KeyRevise_Control.NewKey.Length == 4 && KeyRevise_Control.OldKey.Length == 4)
        {
            CPACK_TransactionChgBankPw m_Data = new CPACK_TransactionChgBankPw();
            m_Data.m_strOldPw = KeyRevise_Control.OldKey;
            m_Data.m_strNewPw = KeyRevise_Control.NewKey;

            MainConnet.m_oMainClient.Send(ENUM_GAME_FRAME.Common, (uint)ENUM_COMMON_PACKID_GC.C2G_Transaction_ReqChgBankPw,
                                MainConnet.m_oMainClient.DoSerialize<CPACK_TransactionChgBankPw>(m_Data));
            BankButtonClick = true;
            Debug.Log("舊密碼: " + m_Data.m_strOldPw + "  //新密碼: " + KeyRevise_Control.NewKey);
        }
        else
        {
            BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.OneAbove;
            BankMessageBox.m_BankMessageBoxOpen = true;
        }
    }

    void MBusinessFirst()
    {
        if (m_Button == ENUM_BANK_BUTTON.BusinessFirstKey)
        {
            Business_Control.FirstNewKey = true;
            Business_Control.FirstNewAgainKey = false;
            
        }
        else if (m_Button == ENUM_BANK_BUTTON.BusinessFirstAgainKey)
        {
            Business_Control.FirstNewKey = false;
            Business_Control.FirstNewAgainKey = true;
        }
    }

    void MBusinessFirstEnter()
    {
        if (Business_Control.FirstNewKeyString.Length == 4)
        {
            CPACK_TransactionVerifyPw m_Data = new CPACK_TransactionVerifyPw();
            m_Data.m_strPw = Business_Control.FirstNewKeyString;

            MainConnet.m_oMainClient.Send(ENUM_GAME_FRAME.Common, (uint)ENUM_COMMON_PACKID_GC.C2G_Transaction_ReqVerifyPwBeginTransaction,
                                MainConnet.m_oMainClient.DoSerialize<CPACK_TransactionVerifyPw>(m_Data));
            Business_Control.PassEnter = true;
            BankButtonClick2 = true;
            Debug.Log("密碼: " + m_Data.m_strPw);
        }
        else
        {
            BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.OneAbove;
            BankMessageBox.m_BankMessageBoxOpen = true;
        }
    }

    void MBusinessKeyEnter()
    {
        CPACK_TransactionVerifyPw m_Data = new CPACK_TransactionVerifyPw();
        m_Data.m_strPw = Business_Control.PassKeyString;

        MainConnet.m_oMainClient.Send(ENUM_GAME_FRAME.Common, (uint)ENUM_COMMON_PACKID_GC.C2G_Transaction_ReqVerifyPwBeginTransaction,
                            MainConnet.m_oMainClient.DoSerialize<CPACK_TransactionVerifyPw>(m_Data));
        Business_Control.PassEnter = true;
        BankButtonClick2 = true;
        Debug.Log("密碼: " + m_Data.m_strPw);
    }

    void BusinessListChange()
    {
        if (!Business_Control.ListCheckBool)
        {
            if (m_Button == ENUM_BANK_BUTTON.BusinessPlayerList && !Business_Control.PlayerListBool)
            {
                Business_Control.PlayerListBool = true;
                Business_Control.FriendListBool = false;
                Business_Control.ListCheckBool = true;
            }
            else if (m_Button == ENUM_BANK_BUTTON.BusinessFriendList && !Business_Control.FriendListBool)
            {
                Business_Control.PlayerListBool = false;
                Business_Control.FriendListBool = true;
                Business_Control.ListCheckBool = true;
            }
        }
    }
}
