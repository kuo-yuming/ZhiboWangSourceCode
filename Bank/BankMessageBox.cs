using UnityEngine;
using System.Collections;
using GameCore.Manager.Common;
using BankEnum;
using System.Collections.Generic;

public class BankMessageBox : MonoBehaviour {
    public static bool m_BankMessageBoxOpen = false;
    public static ushort m_MsessageBoxStatus = 0;
    public GameObject[] MessageBoxTextObject;
    public GameObject[] MessageBoxButtonObject;
    public GameObject MessageBoxObject;
    public UILabel[] m_Label;
    public Vector3[] m_TextVector3;
    public Vector3[] m_ButtonVector3;
    public UIButton m_Button;
    public GameObject[] m_Background;
    public static List<ENUM_BANK_MESSAGE_STATUS> SaveBankError = new List<ENUM_BANK_MESSAGE_STATUS>();
	// Use this for initialization
	void Start () {
        m_BankMessageBoxOpen = false;
        m_MsessageBoxStatus = 0;
        MessageBoxTextObject[0].SetActive(true);
        MessageBoxTextObject[1].SetActive(false);
        MessageBoxTextObject[2].SetActive(false);
        MessageBoxButtonObject[0].SetActive(true);
        MessageBoxButtonObject[1].SetActive(true);
        MessageBoxObject.SetActive(false);
        m_Background[0].SetActive(false);
        m_Background[1].SetActive(false);
        SaveBankError.Clear();

    }
	
	// Update is called once per frame
	void Update () {
        if (m_BankMessageBoxOpen)
        {
            MessageBoxObject.SetActive(true);
            MessageBoxLabel();
        }
        else
        {
            MessageBoxTextObject[0].transform.localPosition = m_TextVector3[0];
            MessageBoxButtonObject[0].transform.localPosition = m_ButtonVector3[1];
            MessageBoxButtonObject[1].SetActive(true);
            MessageBoxTextObject[0].SetActive(true);
            MessageBoxTextObject[1].SetActive(false);
            MessageBoxTextObject[2].SetActive(false);
            MessageBoxObject.SetActive(false);
            m_Background[0].SetActive(false);
            m_Background[1].SetActive(false);
            m_Button.normalSprite = "btn_confirm_0";
        }
	}

    void MessageBoxLabel()
    {
        if (m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.NoPoint)
        {
            if (VersionDef.InternationalLanguageSystem)
            {
                m_Label[0].text = Font_Control.Instance.m_dicMsgStr[2504001];
            }
            else
            {
                m_Label[0].text = "您的點數不足";
            }
            m_Background[0].SetActive(true);
            MessageBoxButtonObject[0].transform.localPosition = m_ButtonVector3[0];
            MessageBoxButtonObject[1].SetActive(false);
        }
        else if (m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.ChangeEndPoint)
        {
            if (VersionDef.InternationalLanguageSystem)
            {
                m_Label[0].text = Font_Control.Instance.m_dicMsgStr[2504002];
            }
            else
            {
                m_Label[0].text = "點數兌換成功";
            }
            m_Background[0].SetActive(true);
            MessageBoxButtonObject[0].transform.localPosition = m_ButtonVector3[0];
            MessageBoxButtonObject[1].SetActive(false);
        }
        else if (m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.DataClickError)
        {
            if (VersionDef.InternationalLanguageSystem)
            {
                m_Label[0].text = Font_Control.Instance.m_dicMsgStr[2504003];
            }
            else
            {
                m_Label[0].text = "資料輸入錯誤";
            }
            m_Background[0].SetActive(true);
            MessageBoxButtonObject[0].transform.localPosition = m_ButtonVector3[0];
            MessageBoxButtonObject[1].SetActive(false);
        }
        else if (m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.KeyForgetSuccess)
        {
            if (VersionDef.InternationalLanguageSystem)
            {
                m_Label[0].text = Font_Control.Instance.m_dicMsgStr[2504004];
            }
            else
            {
                m_Label[0].text = "密碼重置成功";
            }  
            m_Background[0].SetActive(true);
            MessageBoxButtonObject[0].transform.localPosition = m_ButtonVector3[0];
            MessageBoxButtonObject[1].SetActive(false);
        }
        else if (m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.KeyForgetVerifyFial)
        {
            if (VersionDef.InternationalLanguageSystem)
            {
                m_Label[0].text = Font_Control.Instance.m_dicMsgStr[2504005];
            }
            else
            {
                m_Label[0].text = "會員資料錯誤";
            }
            m_Background[0].SetActive(true);
            MessageBoxButtonObject[0].transform.localPosition = m_ButtonVector3[0];
            MessageBoxButtonObject[1].SetActive(false);
        }
        else if (m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.LevelNoClear)
        {
            if (VersionDef.InternationalLanguageSystem)
            {
                m_Label[0].text = Font_Control.Instance.m_dicMsgStr[2504006] + Bank_Manager.m_CPACK_TransactionSysConfig.m_usNeedLv + Font_Control.Instance.m_dicMsgStr[2504007];
            }
            else
            {
                m_Label[0].text = "尚未達到" + Bank_Manager.m_CPACK_TransactionSysConfig.m_usNeedLv + "級";
            }
         
            m_Background[0].SetActive(true);
            MessageBoxButtonObject[0].transform.localPosition = m_ButtonVector3[0];
            MessageBoxButtonObject[1].SetActive(false);
        }
        else if (m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.PhoneNoClear)
        {
            MessageBoxTextObject[0].SetActive(false);
            MessageBoxTextObject[2].SetActive(true);
            m_Background[0].SetActive(true);
            m_Button.normalSprite = "btn_mobileVerification_0";
            MessageBoxButtonObject[0].transform.localPosition = m_ButtonVector3[1];
            MessageBoxButtonObject[1].SetActive(true);
        }
        else if (m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.PassNoClear)
        {
            if (VersionDef.InternationalLanguageSystem)
            {
                m_Label[0].text = Font_Control.Instance.m_dicMsgStr[2504008];
            }
            else
            {
                m_Label[0].text = "密碼尚未設定";
            }
            m_Background[0].SetActive(true);
            MessageBoxButtonObject[0].transform.localPosition = m_ButtonVector3[0];
            MessageBoxButtonObject[1].SetActive(false);
        }
        else if (m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.KeyError)
        {
            if (VersionDef.InternationalLanguageSystem)
            {
                m_Label[0].text = Font_Control.Instance.m_dicMsgStr[2504009];
            }
            else
            {
                m_Label[0].text = "密碼錯誤";
            }
            m_Background[0].SetActive(true);
            MessageBoxButtonObject[0].transform.localPosition = m_ButtonVector3[0];
            MessageBoxButtonObject[1].SetActive(false);
        }
        else if (m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.BusinessCancel)
        {
            if (VersionDef.InternationalLanguageSystem)
            {
                m_Label[0].text = Font_Control.Instance.m_dicMsgStr[2504010];
            }
            else
            {
                m_Label[0].text = "交易取消";
            }
           // m_Button.normalSprite = "btn_backToBank_0";
            m_Background[0].SetActive(true);
            MessageBoxButtonObject[0].transform.localPosition = m_ButtonVector3[0];
            MessageBoxButtonObject[1].SetActive(false);
        }
        else if (m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.OneAbove)
        {
            if (VersionDef.InternationalLanguageSystem)
            {
                m_Label[0].text = Font_Control.Instance.m_dicMsgStr[2504011];
            }
            else
            {
                m_Label[0].text = "密碼最少要四碼";
            }
            // m_Button.normalSprite = "btn_backToBank_0";
            m_Background[0].SetActive(true);
            MessageBoxButtonObject[0].transform.localPosition = m_ButtonVector3[0];
            MessageBoxButtonObject[1].SetActive(false);
        }
        else if (m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.NoPlayer)
        {
            if (VersionDef.InternationalLanguageSystem)
            {
                m_Label[0].text = Font_Control.Instance.m_dicMsgStr[2504012];
            }
            else
            {
                m_Label[0].text = "交易對象不存在";
            }
            m_Background[0].SetActive(true);
            MessageBoxButtonObject[0].transform.localPosition = m_ButtonVector3[0];
            MessageBoxButtonObject[1].SetActive(false);
        }
        else if (m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.InfoError)
        {
            if (VersionDef.InternationalLanguageSystem)
            {
                m_Label[0].text = Font_Control.Instance.m_dicMsgStr[2504013];
            }
            else
            {
                m_Label[0].text = "交易對象的身份不符";
            }
            m_Background[0].SetActive(true);
            MessageBoxButtonObject[0].transform.localPosition = m_ButtonVector3[0];
            MessageBoxButtonObject[1].SetActive(false);
        }
        else if (m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.PlayerNowBusiness)
        {
            if (VersionDef.InternationalLanguageSystem)
            {
                m_Label[0].text = Font_Control.Instance.m_dicMsgStr[2504014];
            }
            else
            {
                m_Label[0].text = "對象正在交易中";
            }
            m_Background[0].SetActive(true);
            MessageBoxButtonObject[0].transform.localPosition = m_ButtonVector3[0];
            MessageBoxButtonObject[1].SetActive(false);
        }
        else if (m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.LessThanZero)
        {
            if (VersionDef.InternationalLanguageSystem)
            {
                m_Label[0].text = Font_Control.Instance.m_dicMsgStr[2504015];
            }
            else
            {
                m_Label[0].text = "交易金額不得小於零";
            }      
            m_Background[0].SetActive(true);
            MessageBoxButtonObject[0].transform.localPosition = m_ButtonVector3[0];
            MessageBoxButtonObject[1].SetActive(false);
        }
        else if (m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.NoHoldCash)
        {
            if (VersionDef.InternationalLanguageSystem)
            {
                m_Label[0].text = Font_Control.Instance.m_dicMsgStr[2504016];
            }
            else
            {
                m_Label[0].text = "持有金不足";
            }
            m_Background[0].SetActive(true);
            MessageBoxButtonObject[0].transform.localPosition = m_ButtonVector3[0];
            MessageBoxButtonObject[1].SetActive(false);
        }
        else if (m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.Thousand)
        {
            if (VersionDef.InternationalLanguageSystem)
            {
                m_Label[0].text = Font_Control.Instance.m_dicMsgStr[2504017];
            }
            else
            {
                m_Label[0].text = "交易金額以千為單位";
            }
            m_Background[0].SetActive(true);
            MessageBoxButtonObject[0].transform.localPosition = m_ButtonVector3[0];
            MessageBoxButtonObject[1].SetActive(false);
        }
        else if (m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.NoCash)
        {
            if (VersionDef.InternationalLanguageSystem)
            {
                m_Label[0].text = Font_Control.Instance.m_dicMsgStr[2504018];
            }
            else
            {
                m_Label[0].text = "餘額不足";
            }
            m_Background[0].SetActive(true);
            MessageBoxButtonObject[0].transform.localPosition = m_ButtonVector3[0];
            MessageBoxButtonObject[1].SetActive(false);
        }
        else if (m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.TodayOutError)
        {
            if (VersionDef.InternationalLanguageSystem)
            {
                m_Label[0].text = Font_Control.Instance.m_dicMsgStr[2504019];
            }
            else
            {
                m_Label[0].text = "超出當日匯出上限";
            }
            m_Background[0].SetActive(true);
            MessageBoxButtonObject[0].transform.localPosition = m_ButtonVector3[0];
            MessageBoxButtonObject[1].SetActive(false);
        }
        else if (m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.TodayInError)
        {
            if (VersionDef.InternationalLanguageSystem)
            {
                m_Label[0].text = Font_Control.Instance.m_dicMsgStr[2504020];
            }
            else
            {
                m_Label[0].text = "超出當日匯入上限";
            }
            MessageBoxButtonObject[0].transform.localPosition = m_ButtonVector3[0];
            MessageBoxButtonObject[1].SetActive(false);
        }
        else if (m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.OutCashError)
        {
            if (VersionDef.InternationalLanguageSystem)
            {
                m_Label[0].text = Font_Control.Instance.m_dicMsgStr[2504021];
            }
            else
            {
                m_Label[0].text = "超出單次匯入上限";
            }
            m_Background[0].SetActive(true);
            MessageBoxButtonObject[0].transform.localPosition = m_ButtonVector3[0];
            MessageBoxButtonObject[1].SetActive(false);
        }
        else if (m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.BankOutMax)
        {
            if (VersionDef.InternationalLanguageSystem)
            {
                m_Label[0].text = Font_Control.Instance.m_dicMsgStr[2504067];
            }
            else
            {
                m_Label[0].text = "超出匯出上限";
            }
            m_Background[0].SetActive(true);
            MessageBoxButtonObject[0].transform.localPosition = m_ButtonVector3[0];
            MessageBoxButtonObject[1].SetActive(false);
        }
        else if (m_MsessageBoxStatus == (ushort)ENUM_BANK_MESSAGE_STATUS.CheckPoint)
        {
            if (VersionDef.InternationalLanguageSystem)
            {
                m_Label[0].text = Font_Control.Instance.m_dicMsgStr[2504022];
            }
            else
            {
                m_Label[0].text = "您兌換的金幣項目：";
            }
            m_Background[1].SetActive(true);
            if (PointChange.SaveChangePointNumber == (byte)ENUM_TRANSACTION_POINT2MONEY.P10ToM1000)
            {
                if (VersionDef.InternationalLanguageSystem)
                {
                    m_Label[1].text = Font_Control.Instance.m_dicMsgStr[2504023];
                }
                else
                {
                    m_Label[1].text = "10點  →  1000金幣";
                }
            }
            else if (PointChange.SaveChangePointNumber == (byte)ENUM_TRANSACTION_POINT2MONEY.P50ToM5000)
            {
                if (VersionDef.InternationalLanguageSystem)
                {
                    m_Label[1].text = Font_Control.Instance.m_dicMsgStr[2504024];
                }
                else
                {
                    m_Label[1].text = "50點  →  5000金幣";
                }
            }
            else if (PointChange.SaveChangePointNumber == (byte)ENUM_TRANSACTION_POINT2MONEY.P100ToM10000)
            {
                if (VersionDef.InternationalLanguageSystem)
                {
                    m_Label[1].text = Font_Control.Instance.m_dicMsgStr[2504025];
                }
                else
                {
                    m_Label[1].text = "100點  →  10000金幣";
                }
            }
            else if (PointChange.SaveChangePointNumber == (byte)ENUM_TRANSACTION_POINT2MONEY.P500ToM50000)
            {
                if (VersionDef.InternationalLanguageSystem)
                {
                    m_Label[1].text = Font_Control.Instance.m_dicMsgStr[2504026];
                }
                else
                {
                    m_Label[1].text = "500點  →  50000金幣";
                }
            }
            else if (PointChange.SaveChangePointNumber == (byte)ENUM_TRANSACTION_POINT2MONEY.P1000ToM100000)
            {
                if (VersionDef.InternationalLanguageSystem)
                {
                    m_Label[1].text = Font_Control.Instance.m_dicMsgStr[2504027];
                }
                else
                {
                    m_Label[1].text = "1000點  →  100000金幣";
                }    
            }
            MessageBoxTextObject[1].SetActive(true);
            MessageBoxTextObject[0].transform.localPosition = m_TextVector3[1];
        }
    }
}
