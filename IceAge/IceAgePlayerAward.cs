using UnityEngine;
using System.Collections;
using System;
using GameCore.Manager.IceAge;

public class IceAgePlayerAward : MonoBehaviour
{

    public int ID;
    int RealDataID;
    UILabel TimeLabel;
    UILabel MachineIDLabel;
    UILabel AwardLabel;
    UILabel MoneyLabel;

    // Use this for initialization
    void Start()
    {
        TimeLabel = this.transform.FindChild("Time").GetComponent<UILabel>();
        MachineIDLabel = this.transform.FindChild("MachineID").GetComponent<UILabel>();
        AwardLabel = this.transform.FindChild("Award").GetComponent<UILabel>();
        MoneyLabel = this.transform.FindChild("Money").GetComponent<UILabel>();

    }

    // Update is called once per frame
    void Update()
    {
        RealDataID = ((IceAgePlayerAwardControl.M_Page - 1) * 12) + ID;
        if (!IceAgePlayerAwardSort.M_AwardShow.ContainsKey(RealDataID))
        {
            TimeLabel.enabled = false;
            MachineIDLabel.enabled = false;
            AwardLabel.enabled = false;
            MoneyLabel.enabled = false;

        }
        else
        {
            TimeLabel.enabled = true;
            MachineIDLabel.enabled = true;
            AwardLabel.enabled = true;
            MoneyLabel.enabled = true;
        }
        if (IceAgePlayerAwardSort.M_AwardShow.ContainsKey(RealDataID))
        {
            int ToDay = DateTime.Now.Day;
            int DataDay = (int)(((IceAgePlayerAwardSort.M_AwardShow[RealDataID].m_ui64Time % 1000000) - (IceAgePlayerAwardSort.M_AwardShow[RealDataID].m_ui64Time % 10000)) / 10000);
            int DataDay_two = (int)((IceAgePlayerAwardSort.M_AwardShow[RealDataID].m_ui64Time / 10000) % 100);
            //Debug.Log("DataID : " + RealDataID + " Time64 " + Fruit_PlayerAwardSort.M_AwardShow[RealDataID].m_ui64Time + " toDay " + ToDay + " DataDay " + DataDay + " DataDay2 " + DataDay_two);
            if (ToDay == DataDay_two)
            {
                TimeLabel.color = new Color32(170, 216, 255, 255);
                MachineIDLabel.color = new Color32(170, 216, 255, 255);
                AwardLabel.color = new Color32(170, 216, 255, 255);
                MoneyLabel.color = new Color32(170, 216, 255, 255);


            }
            else
            {
                TimeLabel.color = new Color32(255, 255, 255, 255);
                MachineIDLabel.color = new Color32(255, 255, 255, 255);
                AwardLabel.color = new Color32(255, 255, 255, 255);
                MoneyLabel.color = new Color32(255, 255, 255, 255);


            }
            ulong hour = (((IceAgePlayerAwardSort.M_AwardShow[RealDataID].m_ui64Time % 10000) - (IceAgePlayerAwardSort.M_AwardShow[RealDataID].m_ui64Time % 100)) / 100);
            ulong min = (IceAgePlayerAwardSort.M_AwardShow[RealDataID].m_ui64Time % 100);
            if (hour < 10 && min >= 10)
            {
                TimeLabel.text = "0" + hour + ":" + min;
            }
            else if (hour >= 10 && min < 10)
            {
                TimeLabel.text = hour + ":0" + min;
            }
            else if (hour < 10 && min < 10)
            {
                TimeLabel.text = "0" + hour + ":0" + min;
            }
            else if (hour >= 10 && min >= 10)
            {
                TimeLabel.text = hour + ":" + min;
            }

            MachineIDLabel.text = IceAgePlayerAwardSort.M_AwardShow[RealDataID].m_uiMID + "";
            MoneyLabel.text = IceAgePlayerAwardSort.M_AwardShow[RealDataID].m_uiMoney + "";
            if (IceAgePlayerAwardSort.M_AwardShow[RealDataID].m_byComboCnt != 0)
            {
                if (VersionDef.InternationalLanguageSystem)
                    AwardLabel.text = Font_Control.Instance.GetMsgStrData(2003007) + " X " + IceAgePlayerAwardSort.M_AwardShow[RealDataID].m_byComboCnt;
                else
                    AwardLabel.text = "連莊獎 X " + IceAgePlayerAwardSort.M_AwardShow[RealDataID].m_byComboCnt;
            }
            else
            {
                switch (IceAgePlayerAwardSort.M_AwardShow[RealDataID].m_byAllWinAwardID)
                {
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Ten_TwoCombo:        // 10 二連線
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = "10" + Font_Control.Instance.GetMsgStrData(2003008);
                        else
                            AwardLabel.text = "10二連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Ten_ThreeCombo:        // 10 三連線
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = "10" + Font_Control.Instance.GetMsgStrData(2003009);
                        else
                            AwardLabel.text = "10三連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Ten_FourCombo:
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = "10" + Font_Control.Instance.GetMsgStrData(2003010);
                        else
                            AwardLabel.text = "10四連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Ten_FiveCombo:
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = "10" + Font_Control.Instance.GetMsgStrData(2003011);
                        else
                            AwardLabel.text = "10五連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_J_TwoCombo:          // J 二連線
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = "J" + Font_Control.Instance.GetMsgStrData(2003008);
                        else
                            AwardLabel.text = "J二連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_J_ThreeCombo:          // J 三連線
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = "J" + Font_Control.Instance.GetMsgStrData(2003009);
                        else
                            AwardLabel.text = "J三連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_J_FourCombo:
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = "J" + Font_Control.Instance.GetMsgStrData(2003010);
                        else
                            AwardLabel.text = "J四連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_J_FiveCombo:
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = "J" + Font_Control.Instance.GetMsgStrData(2003011);
                        else
                            AwardLabel.text = "J五連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Q_TwoCombo:          // Q 二連線
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = "Q" + Font_Control.Instance.GetMsgStrData(2003008);
                        else
                            AwardLabel.text = "Q二連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Q_ThreeCombo:          // Q 三連線
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = "Q" + Font_Control.Instance.GetMsgStrData(2003009);
                        else
                            AwardLabel.text = "Q三連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Q_FourCombo:
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = "Q" + Font_Control.Instance.GetMsgStrData(2003010);
                        else
                            AwardLabel.text = "Q四連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Q_FiveCombo:
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = "Q" + Font_Control.Instance.GetMsgStrData(2003011);
                        else
                            AwardLabel.text = "Q五連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_K_TwoCombo:         // K 二連線
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = "K" + Font_Control.Instance.GetMsgStrData(2003008);
                        else
                            AwardLabel.text = "K二連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_K_ThreeCombo:         // K 三連線
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = "K" + Font_Control.Instance.GetMsgStrData(2003009);
                        else
                            AwardLabel.text = "K三連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_K_FourCombo:
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = "K" + Font_Control.Instance.GetMsgStrData(2003010);
                        else
                            AwardLabel.text = "K四連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_K_FiveCombo:
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = "K" + Font_Control.Instance.GetMsgStrData(2003011);
                        else
                            AwardLabel.text = "K五連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_A_TwoCombo:         // A 二連線
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = "A" + Font_Control.Instance.GetMsgStrData(2003008);
                        else
                            AwardLabel.text = "A二連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_A_ThreeCombo:         // A 三連線
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = "A" + Font_Control.Instance.GetMsgStrData(2003009);
                        else
                            AwardLabel.text = "A三連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_A_FourCombo:
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = "A" + Font_Control.Instance.GetMsgStrData(2003010);
                        else
                            AwardLabel.text = "A四連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_A_FiveCombo:
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = "A" + Font_Control.Instance.GetMsgStrData(2003011);
                        else
                            AwardLabel.text = "A五連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Titanis_TwoCombo:   // 泰坦鳥 二連線
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = Font_Control.Instance.GetMsgStrData(2003012) + Font_Control.Instance.GetMsgStrData(2003008);
                        else
                            AwardLabel.text = "泰坦鳥二連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Titanis_ThreeCombo:   // 泰坦鳥 三連線
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = Font_Control.Instance.GetMsgStrData(2003012) + Font_Control.Instance.GetMsgStrData(2003009);
                        else
                            AwardLabel.text = "泰坦鳥三連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Titanis_FourCombo:
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = Font_Control.Instance.GetMsgStrData(2003012) + Font_Control.Instance.GetMsgStrData(2003010);
                        else
                            AwardLabel.text = "泰坦鳥四連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Titanis_FiveCombo:
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = Font_Control.Instance.GetMsgStrData(2003012) + Font_Control.Instance.GetMsgStrData(2003011);
                        else
                            AwardLabel.text = "泰坦鳥五連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Smilodon_TwoCombo:  // 劍齒虎 二連線
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = Font_Control.Instance.GetMsgStrData(2003013) + Font_Control.Instance.GetMsgStrData(2003008);
                        else
                            AwardLabel.text = "劍齒虎二連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Smilodon_ThreeCombo:  // 劍齒虎 三連線
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = Font_Control.Instance.GetMsgStrData(2003013) + Font_Control.Instance.GetMsgStrData(2003009);
                        else
                            AwardLabel.text = "劍齒虎三連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Smilodon_FourCombo:
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = Font_Control.Instance.GetMsgStrData(2003013) + Font_Control.Instance.GetMsgStrData(2003010);
                        else
                            AwardLabel.text = "劍齒虎四連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Smilodon_FiveCombo:
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = Font_Control.Instance.GetMsgStrData(2003013) + Font_Control.Instance.GetMsgStrData(2003011);
                        else
                            AwardLabel.text = "劍齒虎五連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Mammoth_TwoCombo:   // 長毛象 二連線
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = Font_Control.Instance.GetMsgStrData(2003014) + Font_Control.Instance.GetMsgStrData(2003008);
                        else
                            AwardLabel.text = "長毛象二連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Mammoth_ThreeCombo:   // 長毛象 三連線
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = Font_Control.Instance.GetMsgStrData(2003014) + Font_Control.Instance.GetMsgStrData(2003009);
                        else
                            AwardLabel.text = "長毛象三連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Mammoth_FourCombo:
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = Font_Control.Instance.GetMsgStrData(2003014) + Font_Control.Instance.GetMsgStrData(2003010);
                        else
                            AwardLabel.text = "長毛象四連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Mammoth_FiveCombo:
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = Font_Control.Instance.GetMsgStrData(2003014) + Font_Control.Instance.GetMsgStrData(2003011);
                        else
                            AwardLabel.text = "長毛象五連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Bone_TwoCombo:      // 恐龍骨 二連線
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = Font_Control.Instance.GetMsgStrData(2003015) + Font_Control.Instance.GetMsgStrData(2003008);
                        else
                            AwardLabel.text = "恐龍骨二連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Bone_ThreeCombo:      // 恐龍骨 三連線
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = Font_Control.Instance.GetMsgStrData(2003015) + Font_Control.Instance.GetMsgStrData(2003009);
                        else
                            AwardLabel.text = "恐龍骨三連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Bone_FourCombo:
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = Font_Control.Instance.GetMsgStrData(2003016);
                        else
                            AwardLabel.text = "射擊";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Bone_Five:
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = Font_Control.Instance.GetMsgStrData(2003017);
                        else
                            AwardLabel.text = "敲蛋";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Diamond_FiveCombo:    // 鑽石 五連線
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = Font_Control.Instance.GetMsgStrData(2003018);
                        else
                            AwardLabel.text = "鑽石五連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.BonusShoot:    // 鑽石 五連線
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = Font_Control.Instance.GetMsgStrData(2003016);
                        else
                            AwardLabel.text = "射擊";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.BonusEgg:    // 鑽石 五連線
                        if (VersionDef.InternationalLanguageSystem)
                            AwardLabel.text = Font_Control.Instance.GetMsgStrData(2003017);
                        else
                            AwardLabel.text = "敲蛋";
                        break;
                    default:
                        AwardLabel.text = IceAgePlayerAwardSort.O_AwardShow[RealDataID].m_byAllWinAwardID + "";
                        break;
                }
            }
        }
    }
}
