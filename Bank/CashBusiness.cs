using UnityEngine;
using System.Collections;
using BankEnum;
public class CashBusiness : MonoBehaviour {
    public static bool MyCashOutOk = false;
    public static bool PlayerCashOutOk = false;
    public static bool BusinessEnd = false;

    public static int MyCashOut = 0;
    public static uint PlayerCashOut = 0;

    public static string BusinessPlayerName = "";
    public static uint PlayerDBID = 0;
    public UILabel[] CashLabel;
    public UILabel PlayerNameLabel;
    public UIInput CashInput;
    public BoxCollider CashInputBox;
    public UILabel WaitTime;
    public GameObject WaitObject;
    public UIButton FinallButtonBut;
    public UISprite FinallButtonSpr;
    public BoxCollider FinallButtonBox;
    public GameObject CashButtonObject;

    public GameObject EndObject;
    public UILabel EndCashLabel;
    public UILabel FeeLabel;
    public GameObject[] LockObject;
    public static uint EndCash = 0;
	// Use this for initialization
	void Start () {
        BusinessPlayerName = "";
        PlayerDBID = 0;
        LockObject[0].SetActive(false);
        LockObject[1].SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        FeeLabel.text = Business_Control.BusinessFee.ToString() + "%";
        PlayerNameLabel.text = BusinessPlayerName;
        if (Bank_Control.BankPage == (byte)ENUM_BANK_PAGE.BusinessPage && Business_Control.BusinessStart)
        {
            if (CashInput.value != "")
            {
                MyCashOut = int.Parse(CashInput.value);
            }
            else
            {
                CashInput.value = "0";
            }
            CashLabel[0].text = MyCashOut.ToString();
            CashLabel[1].text = PlayerCashOut.ToString();
            EndCashLabel.enabled = false;
            //EndCashLabel.text = EndCash.ToString();
        }

        if (PlayerCashOutOk)
        {
            LockObject[1].SetActive(true);
        }
        else if (!PlayerCashOutOk)
        {
            LockObject[1].SetActive(false);
        }

        if (MyCashOutOk)
        {
            CashButtonObject.SetActive(false);
            LockObject[0].SetActive(true);
            CashInput.enabled = false;
            CashInputBox.enabled = false;
        }
        else if (!MyCashOutOk)
        {
            CashInput.enabled = true;
            CashInputBox.enabled = true;
            CashButtonObject.SetActive(true);
        }

        if (MyCashOutOk && PlayerCashOutOk)
        {
            FinallButtonBut.normalSprite = "btn_executeDeal_0";
            FinallButtonSpr.spriteName = "btn_executeDeal_0";
            FinallButtonSpr.color = new Color32(255,255,255,255);
            FinallButtonBut.enabled = true;
            FinallButtonBox.enabled = true;
        }
        else 
        {
            FinallButtonBut.normalSprite = "btn_executeDeal_1";
            FinallButtonSpr.spriteName = "btn_executeDeal_1";
            FinallButtonBut.enabled = false;
            FinallButtonBox.enabled = false;
        }

        if (Business_Control.BusinessWaitTime)
        {
            WaitObject.SetActive(true);
            WaitTime.text = "";
        }
        else if (!Business_Control.BusinessWaitTime)
        {
            WaitObject.SetActive(false);
        }

        if (BusinessEnd)
        {
            
            EndObject.SetActive(true);
        }
        else if (!BusinessEnd)
        {
          
            EndObject.SetActive(false);
        }

        if (Bank_Control.BankPage != (byte)ENUM_BANK_PAGE.BusinessPage)
        {
            LockObject[0].SetActive(false);
            LockObject[1].SetActive(false);
            CashLabel[0].text = "0";
            CashLabel[1].text = "0";
            MyCashOutOk = false;
            PlayerCashOutOk = false;
            BusinessEnd = false;
            MyCashOut = 0;
            PlayerCashOut = 0;
            EndCash = 0;
            CashInput.value = "0";
        }
	}
}
