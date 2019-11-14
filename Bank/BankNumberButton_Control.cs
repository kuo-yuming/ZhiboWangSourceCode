using UnityEngine;
using System.Collections;
using BankEnum;

public class BankNumberButton_Control : MonoBehaviour {
    public enum ENUM_BANK_BUTTONNUMBER
    {
        Zero,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        nine,
        AllClear,
    }

    public UIInput m_Input1;
    public UIInput m_Input2;
    public UIInput m_Input3;
    
    public ENUM_BANK_BUTTONNUMBER m_Button;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        if (Bank_Control.BankPage == (byte)ENUM_BANK_PAGE.KeyRevisePage)
        {
            if (KeyRevise_Control.OldKeyClick)
            {
                Input1OnClick();
            }
            else if (KeyRevise_Control.NewKeyClick)
            {
                Input2OnClick();
            }
            else if (KeyRevise_Control.AgainNewClick)
            {
                Input3OnClick();
            }
        }
        else if (Bank_Control.BankPage == (byte)ENUM_BANK_PAGE.BusinessPage)
        {
            if (!Business_Control.PassEnactment)
            {
                if (Business_Control.FirstNewKey)
                {
                    Input1OnClick();
                }
                else if (Business_Control.FirstNewAgainKey)
                {
                    Input2OnClick();
                }
            }
            else
            {
                Input1OnClick();
            }
        }
    }

    void Input1OnClick()
    {
        if (m_Button == ENUM_BANK_BUTTONNUMBER.Zero)
        {
            m_Input1.value += "0";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.One)
        {
            m_Input1.value += "1";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.Two)
        {
            m_Input1.value += "2";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.Three)
        {
            m_Input1.value += "3";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.Four)
        {
            m_Input1.value += "4";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.Five)
        {
            m_Input1.value += "5";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.Six)
        {
            m_Input1.value += "6";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.Seven)
        {
            m_Input1.value += "7";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.Eight)
        {
            m_Input1.value += "8";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.nine)
        {
            m_Input1.value += "9";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.AllClear)
        {
            m_Input1.value = "";
        }
    }
    void Input2OnClick()
    {
        if (m_Button == ENUM_BANK_BUTTONNUMBER.Zero)
        {
            m_Input2.value += "0";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.One)
        {
            m_Input2.value += "1";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.Two)
        {
            m_Input2.value += "2";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.Three)
        {
            m_Input2.value += "3";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.Four)
        {
            m_Input2.value += "4";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.Five)
        {
            m_Input2.value += "5";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.Six)
        {
            m_Input2.value += "6";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.Seven)
        {
            m_Input2.value += "7";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.Eight)
        {
            m_Input2.value += "8";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.nine)
        {
            m_Input2.value += "9";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.AllClear)
        {
            m_Input2.value = "";
        }
    }
    void Input3OnClick()
    {
        if (m_Button == ENUM_BANK_BUTTONNUMBER.Zero)
        {
            m_Input3.value += "0";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.One)
        {
            m_Input3.value += "1";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.Two)
        {
            m_Input3.value += "2";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.Three)
        {
            m_Input3.value += "3";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.Four)
        {
            m_Input3.value += "4";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.Five)
        {
            m_Input3.value += "5";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.Six)
        {
            m_Input3.value += "6";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.Seven)
        {
            m_Input3.value += "7";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.Eight)
        {
            m_Input3.value += "8";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.nine)
        {
            m_Input3.value += "9";
        }
        else if (m_Button == ENUM_BANK_BUTTONNUMBER.AllClear)
        {
            m_Input3.value = "";
        }
    }
}
