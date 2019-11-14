using UnityEngine;
using System.Collections;
using System;
using GameCore;
using GameCore.Manager.Common;
using BankEnum;
public class Bank_Manager : MonoBehaviour {
    public static CPACK_TransactionSysConfig m_CPACK_TransactionSysConfig = new CPACK_TransactionSysConfig();
    public static float MinusTime = 0;
    public static byte MaxTime = 20;


    public void OnRcvBankData(uint uiPackID, byte[] byarData)
    {
       // Debug.Log(string.Format("OnRcvBankFrameData. PackID={0}", uiPackID));

        switch (uiPackID)
        {
            case (uint)ENUM_COMMON_PACKID_GC.G2C_Transaction_NotifySysConfig:
                //取得銀行設定
                GetBankConfig(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_Transaction_RplyVerifyPwBeginTransaction:
                //進入銀行結果
                BusinessInResult(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_Transaction_NotifyCancelTransaction:
                //取消交易
                BusinessCancel(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_Transaction_NotifyPoint:
                //取得玩家點數
                GetPlayerPoint(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_Transaction_RplyPoint2Money:
                //點數換金幣結果
                ChangePointResult(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_Transaction_RplyClearBankPw:
                //忘記密碼結果
                KeyForgetResult(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_Transaction_RplyChgBankPw:
                //修改密碼結果
                KeyReviseResult(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_Actor_RplyPlayerSimpleInfo:
                //取得玩家資訊
                if (Bank_Control.BankPage == (byte)ENUM_BANK_PAGE.BusinessPage)
                {
                    GetPlayerListInfo(byarData);
                }
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_Transaction_RplyInviteTransaction:
                //要求和其他玩家交易結果
                MyToPlayerBusinessResult(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_Transaction_NotifySetItem:
                //雙方開始交易
                Business_Control.BusinessStart = true;
                Business_Control.BusinessWaitTime = false;
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_Transaction_RplyExportItem:
                //匯出交易結果
                BusinessErrorResult(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_Transaction_NotifyExportItem:
                //匯入交易結果
                GetInCashData(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_Transaction_RplyExecTransaction:
                //執行交易結果
                GetInCashData(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_Transaction_NotifyTransactionComplete:
                //交易完成
                BusinessEnd(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_Transaction_NotifyInviteTransaction:
                //要求交易
                PleaseBusiness(byarData);
                break;
            case (uint)ENUM_COMMON_PACKID_GC.G2C_Transaction_NotifyActiveTransaction:
                //進入最後交易
                CashBusiness.PlayerCashOutOk = true;
                Business_Control.BusinessWaitTime = false;
                break;
            default:
              //  Debug.Log(string.Format("{0}:Unknown packid={1}", DateTime.Now, uiPackID));
                break;
        }
       // Debug.Log(string.Format("OnRcvBankFrameData. PackID={0} end", uiPackID));
    }

    //交易錯誤結果
    public void BusinessErrorResult(byte[] byarData)
    {
        int m_Result = MainConnet.m_oMainClient.DoDeSerialize<int>(byarData);
        if (m_Result != (int)ENUM_COMMON_ERROR_CODE.Success)
        {
            if (m_Result == (ushort)ENUM_COMMON_ERROR_CODE.Transaction_MoneyNotEnough)
            {
                // 餘額不足
                BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.NoCash;
                BankMessageBox.m_BankMessageBoxOpen = true;
            }
            else if (m_Result == (ushort)ENUM_COMMON_ERROR_CODE.Transaction_DayExport)
            {
                // 超出當日匯出上限
                BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.TodayOutError;
                BankMessageBox.m_BankMessageBoxOpen = true;
            }
            else if (m_Result == (ushort)ENUM_COMMON_ERROR_CODE.Transaction_DayImport)
            {
                // 超出當日匯入上限
                BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.TodayInError;
                BankMessageBox.m_BankMessageBoxOpen = true;
            }
            else if (m_Result == (ushort)ENUM_COMMON_ERROR_CODE.Transaction_SingleImport)
            {
                // 超出單次匯入上限
                BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.OutCashError;
                BankMessageBox.m_BankMessageBoxOpen = true;
            }
            else if (m_Result == (ushort)ENUM_COMMON_ERROR_CODE.Transaction_TargetNotNound)
            {
                //交易對象不存在. 可能已下線
                BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.NoPlayer;
                BankMessageBox.m_BankMessageBoxOpen = true;
            }
            else if (m_Result == (ushort)ENUM_COMMON_ERROR_CODE.Transaction_TargetIdentityNotMatch)
            {
                //交易對象的身份不符
                BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.InfoError;
                BankMessageBox.m_BankMessageBoxOpen = true;
            }
            else if (m_Result == (ushort)ENUM_COMMON_ERROR_CODE.Transaction_InTrading)
            {
                //自己/對象正在交易中
                BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.PlayerNowBusiness;
                BankMessageBox.m_BankMessageBoxOpen = true;
            }
            Business_Control.BusinessWaitTime = false;
        }
    }

    //取得銀行設定
    public void GetBankConfig(byte[] byarData)
    {
        m_CPACK_TransactionSysConfig = MainConnet.m_oMainClient.DoDeSerialize<CPACK_TransactionSysConfig>(byarData);

        CTransactionSet Data = Function_cs.GetTransactionSetWithLv(MainConnet.m_PlayerData.m_usLv);
        Business_Control.BusinessMaxExportMoney = Data.m_uiMaxExportMoney;
        Business_Control.BusinessFee = Data.m_byExportFee;
        Business_Control.BusinessKeepMoney = Data.m_uiKeepMoney;
        Debug.Log("有無手機認證: " + MainConnet.m_PlayerData.m_bSetBankPw);
        Debug.Log("玩家等級限制: " + m_CPACK_TransactionSysConfig.m_usNeedLv);
    }

    //進入銀行結果
    public void BusinessInResult(byte[] byarData)
    {
        int m_Result = MainConnet.m_oMainClient.DoDeSerialize<int>(byarData);
        if (m_Result != (int)ENUM_COMMON_ERROR_CODE.Success)
        {
            if (m_Result == (ushort)ENUM_COMMON_ERROR_CODE.Transaction_VerifyFial)
            {
                BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.KeyError;
                Business_Control.PassEnter = false;
            }
            BankMessageBox.m_BankMessageBoxOpen = true;
            BankButton.BankButtonClick2 = false;
        }
        else
        {
            PlayerListCheck.GetPlayerListData = true;
            MainConnet.m_PlayerData.m_bSetBankPw = true;
            BankButton.BankButtonClick2 = false;
        }

        if (CashBusiness.PlayerDBID != 0 && m_Result == (int)ENUM_COMMON_ERROR_CODE.Success)
        {
            MainConnet.m_oMainClient.Send(ENUM_GAME_FRAME.Common, (uint)ENUM_COMMON_PACKID_GC.C2G_Transaction_ReqInviteTransaction,
                            MainConnet.m_oMainClient.DoSerialize<uint>(CashBusiness.PlayerDBID));
            Business_Control.BusinessWaitTime = true;
        }
        if (CashBusiness.BusinessPlayerName != "" && CashBusiness.PlayerDBID == 0)
        {
            MainConnet.m_oMainClient.Send(ENUM_GAME_FRAME.Common, (uint)ENUM_COMMON_PACKID_GC.C2G_Transaction_ConfirmInviteTransaction,
                          null);
            Business_Control.BusinessWaitTime = true;
        }
      
        Debug.Log("進入銀行結果: " + m_Result);
    }

    //要求和其他玩家交易結果
    public void MyToPlayerBusinessResult(byte[] byarData)
    {
        int m_Result = MainConnet.m_oMainClient.DoDeSerialize<int>(byarData);
        if (m_Result != (ushort)ENUM_COMMON_ERROR_CODE.Success)
        {
            if (m_Result == (ushort)ENUM_COMMON_ERROR_CODE.Transaction_TargetNotNound)
            {
                //交易對象不存在. 可能已下線
                BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.NoPlayer;
                BankMessageBox.m_BankMessageBoxOpen = true;
            }
            else if (m_Result == (ushort)ENUM_COMMON_ERROR_CODE.Transaction_TargetIdentityNotMatch)
            {
                //交易對象的身份不符
                BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.InfoError;
                BankMessageBox.m_BankMessageBoxOpen = true;
            }
            else if (m_Result == (ushort)ENUM_COMMON_ERROR_CODE.Transaction_InTrading)
            {
                //自己/對象正在交易中
                BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.PlayerNowBusiness;
                BankMessageBox.m_BankMessageBoxOpen = true;
            }
            Business_Control.BusinessWaitTime = false;
        }
        BankButton.BankButtonClick = false;
    }

    //取消交易
    public void BusinessCancel(byte[] byarData)
    {

        CashBusiness.BusinessPlayerName = "";
        CashBusiness.PlayerDBID = 0;
        if (BankMessageBox.m_BankMessageBoxOpen)
        {
            BankMessageBox.SaveBankError.Add(ENUM_BANK_MESSAGE_STATUS.BusinessCancel);
        }
        else
        {
            BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.BusinessCancel;
            BankMessageBox.m_BankMessageBoxOpen = true;
        }
        Debug.Log("交易取消");
        if (Message_Control.MessageStatus == Message_Control.MessageStatu.BankBusiness)
        {
            Message_Control.OpenMessage = true;
        }
        Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
        Message_Control.OpenMessage = false;
        BankButton.BankButtonClick = false;
        Business_Control.BusinessWaitTime = false;
    }

    //取得玩家點數
    public void GetPlayerPoint(byte[] byarData)
    {
        Bank_Control.PlayerPoint = MainConnet.m_oMainClient.DoDeSerialize<uint>(byarData);
        Debug.Log("取得玩家點數: " + Bank_Control.PlayerPoint);
    }

    //點數換金幣結果
    public void ChangePointResult(byte[] byarData)
    {
        CPACK_TransactionPoint2MoneyResult m_Result = MainConnet.m_oMainClient.DoDeSerialize<CPACK_TransactionPoint2MoneyResult>(byarData);
        if (m_Result.m_enumResult != ENUM_COMMON_ERROR_CODE.Transaction_PointNotEnough)
        {
            BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.ChangeEndPoint;
        }
        else
        {
            BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.NoPoint;
        }
        PointChange.WaitChangePoint = false;
        Bank_Control.PlayerPoint = m_Result.m_uiPoint;
        BankMessageBox.m_BankMessageBoxOpen = true;
        PointChange.SaveChangePointNumber = 0;
        Debug.Log("取得點數交換結果: " + m_Result.m_enumResult);
    }

    //忘記密碼結果
    public void KeyForgetResult(byte[] byarData)
    {
        int m_Result = MainConnet.m_oMainClient.DoDeSerialize<int>(byarData);
        if (m_Result == (int)ENUM_COMMON_ERROR_CODE.Success)
        {
            BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.KeyForgetSuccess;
            MainConnet.m_PlayerData.m_bSetBankPw = false;
        }
        else
        {
            BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.KeyForgetVerifyFial;
        }
        BankButton.BankButtonClick = false;
        BankMessageBox.m_BankMessageBoxOpen = true;
        Debug.Log("密碼修改結果: " + m_Result);
    }

    //修改密碼
    public void KeyReviseResult(byte[] byarData)
    {
        int m_Result = MainConnet.m_oMainClient.DoDeSerialize<int>(byarData);
        if (m_Result == (int)ENUM_COMMON_ERROR_CODE.Success)
        {
            BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.KeyForgetSuccess;
        }
        else
        {
            BankMessageBox.m_MsessageBoxStatus = (ushort)ENUM_BANK_MESSAGE_STATUS.DataClickError;
        }
        BankButton.BankButtonClick = false;
        BankMessageBox.m_BankMessageBoxOpen = true;
        Debug.Log("密碼修改結果: " + m_Result);
    }

    //取得玩家資訊
    public void GetPlayerListInfo(byte[] byarData)
    {

        CPACK_GetPlayerSimpleInfoResult PlayerInfo = MainConnet.m_oMainClient.DoDeSerialize<CPACK_GetPlayerSimpleInfoResult>(byarData);
        PlayerListCheck.PlayerDataNumber--;
        if (PlayerInfo.m_enumResult == ENUM_COMMON_ERROR_CODE.Success)
        {
            if (PlayerListCheck.GetAllPlayerList.Count != 0)
            {
                if (PlayerListCheck.GetAllPlayerList.ContainsKey(PlayerInfo.m_uiDBID))
                {
                    PlayerListCheck.GetAllPlayerList[PlayerInfo.m_uiDBID] = PlayerInfo;
                }
                else
                {
                    PlayerListCheck.GetAllPlayerList.Add(PlayerInfo.m_uiDBID, PlayerInfo);
                }
            }
            else
            {
                PlayerListCheck.GetAllPlayerList.Add(PlayerInfo.m_uiDBID, PlayerInfo);
            }
            if (PlayerListCheck.PlayerDataNumber == 1)
            {
                BankButton.BankButtonClick2 = false;
                Business_Control.ListCheckBool = true;
            }
        }
    }

    //匯入結果
    public void GetInCashData(byte[] byarData)
    {
        CPACK_TransactionNotifyExportItem m_Data = MainConnet.m_oMainClient.DoDeSerialize<CPACK_TransactionNotifyExportItem>(byarData);
        if (m_Data.m_uiExportDBID != MainConnet.m_PlayerData.m_uiDBID)
        {
            CashBusiness.PlayerCashOut = m_Data.m_uiExportMoney;
            CashBusiness.PlayerCashOutOk = true;
        }
    }

    //交易完成
    public void BusinessEnd(byte[] byarData)
    {
        Business_Control.BusinessWaitTime = false;
        CashBusiness.BusinessPlayerName = "";
        CashBusiness.PlayerDBID = 0;
        CPACK_TransactionNotifyComplete m_Data = MainConnet.m_oMainClient.DoDeSerialize<CPACK_TransactionNotifyComplete>(byarData);
        CashBusiness.EndCash = m_Data.m_uiImportMoney;
        CashBusiness.BusinessEnd = true;
        Debug.Log("交易完成");
    }

    //希望交易
    public void PleaseBusiness(byte[] byarData)
    {
        CPACK_NotifyInviteTransaction m_Data = MainConnet.m_oMainClient.DoDeSerialize<CPACK_NotifyInviteTransaction>(byarData);
        CashBusiness.BusinessPlayerName = m_Data.m_strInviteName;
        MinusTime = 0;
        MaxTime = 20;
        Message_Control.OpenMessage = true;
        Message_Control.MessageStatus = Message_Control.MessageStatu.BankBusiness;
        Message_Control.MessageSize = Message_Control.BoxSizeStatu.BankBusinessCheck;
        Debug.Log("玩家: " + CashBusiness.BusinessPlayerName + " 要求交易");
    }
}
