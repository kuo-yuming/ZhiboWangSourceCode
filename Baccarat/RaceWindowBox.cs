using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore.Machine;
using GameCore;

public class RaceWindowBox : MonoBehaviour {
    public GameObject MainMessageBox;
    public GameObject[] RaceWindowBoxObject;
    public static byte RaceWindowState = 0;
    public UILabel[] RaceWindowLabel_State2;
    public UILabel[] RaceWindowLabel_State4;
    public static string LabelString_State2 = "";
    public static string LabelString_State4 = "";
    public static uint FeeVal = 0;
    public static ENUM_RACE_FEE_TYPE BoxFeeType = ENUM_RACE_FEE_TYPE.Money;
    public static ENUM_RACE_EVENT_TYPE BoxEventType = ENUM_RACE_EVENT_TYPE.Buying;
    public static ushort RaceID = 0;
    public GameObject[] LabelState4Object;
    public static int CodeID = 0;
    public static bool RaceMoneyBoxBool = false;
    public GameObject MoneyBoxGameObject;
    public static ulong MoneyBoxMoney = 0;
    public static uint MoneyBoxID = 0;
    public UILabel MoneyBoxLabel;
    private string Name1 = "競賽籌碼：";
    private string Name2 = "確認報名即支付";
    private string Name3 = "金幣報名費";
    private string Name4 = "鑽石報名費";
    private string Name5 = "報名成功";
    private string[] ResultName = new string[14];
    // Use this for initialization
    void Start () {
        RaceWindowState = 0;
        LabelString_State2 = "";
        LabelString_State4 = "";
        CodeID = 0;
        FeeVal = 0;
        RaceID = 0;
     
    }
	
	// Update is called once per frame
	void Update () {
        if (VersionDef.InternationalLanguageSystem)
        {
            Name1 = Font_Control.Instance.m_dicMsgStr[2008007];
            Name2 = Font_Control.Instance.m_dicMsgStr[2008008];
            Name3 = Font_Control.Instance.m_dicMsgStr[2008009];
            Name4 = Font_Control.Instance.m_dicMsgStr[2008010];
            Name5 = Font_Control.Instance.m_dicMsgStr[2008011];
            ResultName[0] = Font_Control.Instance.m_dicMsgStr[2008012];
            ResultName[1] = Font_Control.Instance.m_dicMsgStr[2008013];
            ResultName[2] = Font_Control.Instance.m_dicMsgStr[2008014];
            ResultName[3] = Font_Control.Instance.m_dicMsgStr[2008015];
            ResultName[4] = Font_Control.Instance.m_dicMsgStr[2008016];
            ResultName[5] = Font_Control.Instance.m_dicMsgStr[2008017];
            ResultName[6] = Font_Control.Instance.m_dicMsgStr[2008018];
            ResultName[7] = Font_Control.Instance.m_dicMsgStr[2008019];
            ResultName[8] = Font_Control.Instance.m_dicMsgStr[2008020];
            ResultName[9] = Font_Control.Instance.m_dicMsgStr[2008021];
            ResultName[10] = Font_Control.Instance.m_dicMsgStr[2008100];
            ResultName[11] = Font_Control.Instance.m_dicMsgStr[2008101];
            ResultName[12] = Font_Control.Instance.m_dicMsgStr[2008105];
            ResultName[13] = Font_Control.Instance.m_dicMsgStr[2008104];
        }
        else
        {
            Name1 = "競賽籌碼：";
            Name2 = "確認報名即支付";
            Name3 = "金幣報名費";
            Name4 = "鑽石報名費";
            Name5 = "報名成功";
            ResultName[0] = "取消成功";
            ResultName[1] = "報名失敗";
            ResultName[2] = "您已成功取消報名賽事";
            ResultName[3] = "報名時間已結束";
            ResultName[4] = "參加費不夠";
            ResultName[5] = "重複報名";
            ResultName[6] = "已參與過一生一次賽";
            ResultName[7] = "未報名此賽局";
            ResultName[8] = "賽局即將結束";
            ResultName[9] = "進入失敗";
            ResultName[10] = "賽局已結束";
            ResultName[11] = "已達報名人數上限";
            ResultName[12] = "賽事統計中,請稍後";
            ResultName[13] = "錯誤訊息";
        }

        if (RaceMoneyBoxBool)
        {
            MoneyBoxGameObject.SetActive(true);
            MoneyBoxLabel.text = Name1 + MoneyBoxMoney.ToString();
        }
        else
        {
            MoneyBoxGameObject.SetActive(false);
        }

        switch (RaceWindowState)
        {
            case 0:
                MainMessageBox.SetActive(false);
                RaceWindowBoxObject[0].SetActive(false);
                RaceWindowBoxObject[1].SetActive(false);
                RaceWindowBoxObject[2].SetActive(false);
                RaceWindowBoxObject[3].SetActive(false);
                break;
            case 1:
                MainMessageBox.SetActive(true);
                RaceWindowBoxObject[0].SetActive(true);
                RaceWindowBoxObject[1].SetActive(false);
                RaceWindowBoxObject[2].SetActive(false);
                RaceWindowBoxObject[3].SetActive(false);
                break;
            case 2:
                MainMessageBox.SetActive(true);
                RaceWindowBoxObject[0].SetActive(false);
                RaceWindowBoxObject[1].SetActive(true);
                RaceWindowBoxObject[2].SetActive(false);
                RaceWindowBoxObject[3].SetActive(false);
                RaceWindowLabel_State2[0].text = LabelString_State2;
                if (BoxFeeType == ENUM_RACE_FEE_TYPE.Money)
                {
                    RaceWindowLabel_State2[1].text = Name2 + FeeVal + Name3;
                }
                else if (BoxFeeType == ENUM_RACE_FEE_TYPE.Diamond)
                {
                    RaceWindowLabel_State2[1].text = Name2 + FeeVal + Name4;
                }
                break;
            case 3:
                MainMessageBox.SetActive(true);
                RaceWindowBoxObject[0].SetActive(false);
                RaceWindowBoxObject[1].SetActive(false);
                RaceWindowBoxObject[2].SetActive(true);
                RaceWindowBoxObject[3].SetActive(false);
                break;
            case 4:
                MainMessageBox.SetActive(true);
                RaceWindowBoxObject[0].SetActive(false);
                RaceWindowBoxObject[1].SetActive(false);
                RaceWindowBoxObject[2].SetActive(false);
                RaceWindowBoxObject[3].SetActive(true);
                if (CodeID == 0)
                {
                    LabelState4Object[0].SetActive(true);
                    LabelState4Object[1].SetActive(false);
                    RaceWindowLabel_State4[0].text = Name5;
                    RaceWindowLabel_State4[1].text = LabelString_State4;
                }
                else
                {
                    LabelState4Object[0].SetActive(false);
                    LabelState4Object[1].SetActive(true);
                    switch (CodeID)
                    {
                        case 1:
                            RaceWindowLabel_State4[0].text = ResultName[0];
                            RaceWindowLabel_State4[2].text = ResultName[2];
                            break;
                        case (byte)ENUM_COMMON_ERROR_CODE.Race_OverTime:
                            RaceWindowLabel_State4[0].text = ResultName[1];
                            RaceWindowLabel_State4[2].text = ResultName[3];
                            break;
                        case (byte)ENUM_COMMON_ERROR_CODE.Race_Sign_FeeNotEnough:
                            RaceWindowLabel_State4[0].text = ResultName[1];
                            RaceWindowLabel_State4[2].text = ResultName[4];
                            break;
                        case (byte)ENUM_COMMON_ERROR_CODE.Race_Sign_HaveSigned:
                            RaceWindowLabel_State4[0].text = ResultName[1];
                            RaceWindowLabel_State4[2].text = ResultName[5];
                            break;
                        case (byte)ENUM_COMMON_ERROR_CODE.Race_Sign_OnceRacePlayed:
                            RaceWindowLabel_State4[0].text = ResultName[1];
                            RaceWindowLabel_State4[2].text = ResultName[6];
                            break;
                        case (byte)ENUM_COMMON_ERROR_CODE.Race_CancelSign_NotSign:
                            RaceWindowLabel_State4[0].text = ResultName[9];
                            RaceWindowLabel_State4[2].text = ResultName[7];
                            break;
                        case (byte)ENUM_COMMON_ERROR_CODE.Race_Buyin_EventWillEnd:
                            RaceWindowLabel_State4[0].text = ResultName[9];
                            RaceWindowLabel_State4[2].text = ResultName[8];
                            break;
                        case (byte)ENUM_COMMON_ERROR_CODE.Race_Buyin_EventEnd:
                            RaceWindowLabel_State4[0].text = ResultName[9];
                            RaceWindowLabel_State4[2].text = ResultName[10];
                            break;
                        case 1000:
                            RaceWindowLabel_State4[0].text = ResultName[13];
                            RaceWindowLabel_State4[2].text = ResultName[12];
                            break;
                    }
                }
                break;
        }
    }
}
