using UnityEngine;
using System.Collections;
using BankEnum;
public class Bank_Control : MonoBehaviour {
    public static bool BankPageCheck = false;
    public static byte BankPage = 0;
    public GameObject[] BankPageObject;

    public GameObject DataDeliverObject;
    public GameObject DataDeliverObject2;
    public GameObject[] MainObject;
    //銀行資料設定


    //點數換金幣
    public UILabel Point_Label;
    public static uint PlayerPoint = 0;
	// Use this for initialization
	void Start () {
        BankPage = (byte)ENUM_BANK_PAGE.MainPage;
        BankPageCheck = true;
        PlayerPoint = 0;
       
	}
	
	// Update is called once per frame
	void Update () {
        if (VersionDef.CN_LogInPack)
        {
            MainObject[0].SetActive(false);
            MainObject[1].SetActive(true);
        }
        else
        {
            MainObject[0].SetActive(true);
            MainObject[1].SetActive(false);
        }

        if (VersionDef.InternationalLanguageSystem)
        {
            Point_Label.text = PlayerPoint.ToString() + Font_Control.Instance.m_dicMsgStr[2504000];
        }
        else
        {
            Point_Label.text = PlayerPoint.ToString() + "點";
        }
       
        if (BankPageCheck)
        {
            PageCheck();
            BankPageCheck = false;
        }

        if (BankButton.BankButtonClick)
        {
            DataDeliverObject.SetActive(true);
        }
        else
        {
            DataDeliverObject.SetActive(false);
        }

        if (BankButton.BankButtonClick2)
        {
            DataDeliverObject2.SetActive(true);
        }
        else
        {
            DataDeliverObject2.SetActive(false);
        }
    }

    void PageCheck()
    {
        switch (BankPage)
        {
            case (byte)ENUM_BANK_PAGE.MainPage:
                BankPageObject[(int)ENUM_BANK_PAGE.MainPage].SetActive(true);
                BankPageObject[(int)ENUM_BANK_PAGE.BusinessPage].SetActive(false);
                BankPageObject[(int)ENUM_BANK_PAGE.KeyRevisePage].SetActive(false);
                BankPageObject[(int)ENUM_BANK_PAGE.PointChangePage].SetActive(false);
                BankPageObject[(int)ENUM_BANK_PAGE.GiftPage].SetActive(false);
                BankPageObject[(int)ENUM_BANK_PAGE.KeyForgetPage].SetActive(false);
                break;
            case (byte)ENUM_BANK_PAGE.BusinessPage:
                BankPageObject[(int)ENUM_BANK_PAGE.MainPage].SetActive(false);
                BankPageObject[(int)ENUM_BANK_PAGE.BusinessPage].SetActive(true);
                BankPageObject[(int)ENUM_BANK_PAGE.KeyRevisePage].SetActive(false);
                BankPageObject[(int)ENUM_BANK_PAGE.PointChangePage].SetActive(false);
                BankPageObject[(int)ENUM_BANK_PAGE.GiftPage].SetActive(false);
                BankPageObject[(int)ENUM_BANK_PAGE.KeyForgetPage].SetActive(false);
                break;
            case (byte)ENUM_BANK_PAGE.KeyRevisePage:
                BankPageObject[(int)ENUM_BANK_PAGE.MainPage].SetActive(false);
                BankPageObject[(int)ENUM_BANK_PAGE.BusinessPage].SetActive(false);
                BankPageObject[(int)ENUM_BANK_PAGE.KeyRevisePage].SetActive(true);
                BankPageObject[(int)ENUM_BANK_PAGE.PointChangePage].SetActive(false);
                BankPageObject[(int)ENUM_BANK_PAGE.GiftPage].SetActive(false);
                BankPageObject[(int)ENUM_BANK_PAGE.KeyForgetPage].SetActive(false);
                break;
            case (byte)ENUM_BANK_PAGE.PointChangePage:
                BankPageObject[(int)ENUM_BANK_PAGE.MainPage].SetActive(false);
                BankPageObject[(int)ENUM_BANK_PAGE.BusinessPage].SetActive(false);
                BankPageObject[(int)ENUM_BANK_PAGE.KeyRevisePage].SetActive(false);
                BankPageObject[(int)ENUM_BANK_PAGE.PointChangePage].SetActive(true);
                BankPageObject[(int)ENUM_BANK_PAGE.GiftPage].SetActive(false);
                BankPageObject[(int)ENUM_BANK_PAGE.KeyForgetPage].SetActive(false);
                break;
            case (byte)ENUM_BANK_PAGE.GiftPage:
                BankPageObject[(int)ENUM_BANK_PAGE.MainPage].SetActive(false);
                BankPageObject[(int)ENUM_BANK_PAGE.BusinessPage].SetActive(false);
                BankPageObject[(int)ENUM_BANK_PAGE.KeyRevisePage].SetActive(false);
                BankPageObject[(int)ENUM_BANK_PAGE.PointChangePage].SetActive(false);
                BankPageObject[(int)ENUM_BANK_PAGE.GiftPage].SetActive(true);
                BankPageObject[(int)ENUM_BANK_PAGE.KeyForgetPage].SetActive(false);
                break;
            case (byte)ENUM_BANK_PAGE.KeyForgetPage:
                BankPageObject[(int)ENUM_BANK_PAGE.MainPage].SetActive(false);
                BankPageObject[(int)ENUM_BANK_PAGE.BusinessPage].SetActive(false);
                BankPageObject[(int)ENUM_BANK_PAGE.KeyRevisePage].SetActive(false);
                BankPageObject[(int)ENUM_BANK_PAGE.PointChangePage].SetActive(false);
                BankPageObject[(int)ENUM_BANK_PAGE.GiftPage].SetActive(false);
                BankPageObject[(int)ENUM_BANK_PAGE.KeyForgetPage].SetActive(true);
                break;
        }
    }
}
