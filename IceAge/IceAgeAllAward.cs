using UnityEngine;
using System.Collections;
using System;
using GameCore.Manager.IceAge;

public class IceAgeAllAward : MonoBehaviour
{
    public int ID;
    int RealDataID;
    UILabel TimeLabel;
    UILabel MachineIDLabel;
    UILabel PlayerNickName;
    UILabel AwardLabel;
    UILabel MoneyLabel;

    // Use this for initialization
    void Start()
    {
        TimeLabel = this.transform.FindChild("Time").GetComponent<UILabel>();
        MachineIDLabel = this.transform.FindChild("MachineID").GetComponent<UILabel>();
        AwardLabel = this.transform.FindChild("Award").GetComponent<UILabel>();
        MoneyLabel = this.transform.FindChild("Money").GetComponent<UILabel>();
        PlayerNickName = this.transform.FindChild("PlayerNickName").GetComponent<UILabel>();
    }

    // Update is called once per frame
    void Update()
    {


        RealDataID = ((IceAgeAllAwardControl.O_Page - 1) * 12) + ID;

        if (!IceAgePlayerAwardSort.O_AwardShow.ContainsKey(RealDataID))
        {
            // Debug.Log(ToDay + " , " + DataDay);
            TimeLabel.enabled = false;
            MachineIDLabel.enabled = false;
            AwardLabel.enabled = false;
            MoneyLabel.enabled = false;
            PlayerNickName.enabled = false;
        }
        else
        {
            TimeLabel.enabled = true;
            MachineIDLabel.enabled = true;
            AwardLabel.enabled = true;
            MoneyLabel.enabled = true;
            PlayerNickName.enabled = true;
        }
        if (IceAgePlayerAwardSort.O_AwardShow.ContainsKey(RealDataID))
        {
            int ToDay = DateTime.Now.Day;
            int DataDay = (int)(((IceAgePlayerAwardSort.O_AwardShow[RealDataID].m_ui64Time % 1000000) - (IceAgePlayerAwardSort.O_AwardShow[RealDataID].m_ui64Time % 10000)) / 10000);

            if (ToDay == DataDay)
            {
                TimeLabel.color = new Color32(170, 216, 255, 255);
                MachineIDLabel.color = new Color32(170, 216, 255, 255);
                AwardLabel.color = new Color32(170, 216, 255, 255);
                MoneyLabel.color = new Color32(170, 216, 255, 255);
                PlayerNickName.color = new Color32(170, 216, 255, 255);

            }
            else
            {
                TimeLabel.color = new Color32(255, 255, 255, 255);
                MachineIDLabel.color = new Color32(255, 255, 255, 255);
                AwardLabel.color = new Color32(255, 255, 255, 255);
                MoneyLabel.color = new Color32(255, 255, 255, 255);
                PlayerNickName.color = new Color32(255, 255, 255, 255);

            }

            ulong hour = (((IceAgePlayerAwardSort.O_AwardShow[RealDataID].m_ui64Time % 10000) - (IceAgePlayerAwardSort.O_AwardShow[RealDataID].m_ui64Time % 100)) / 100);
            ulong min = (IceAgePlayerAwardSort.O_AwardShow[RealDataID].m_ui64Time % 100);
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
            MachineIDLabel.text = IceAgePlayerAwardSort.O_AwardShow[RealDataID].m_uiMID + "";
            MoneyLabel.text = IceAgePlayerAwardSort.O_AwardShow[RealDataID].m_uiMoney + "";
            PlayerNickName.text = IceAgePlayerAwardSort.O_AwardShow[RealDataID].m_strPlayerNickName;
            if (IceAgePlayerAwardSort.O_AwardShow[RealDataID].m_byComboCnt != 0)
            {
                AwardLabel.text = "連莊獎 X " + IceAgePlayerAwardSort.O_AwardShow[RealDataID].m_byComboCnt;
            }
            else
            {
                switch (IceAgePlayerAwardSort.O_AwardShow[RealDataID].m_byAllWinAwardID)
                {
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Ten_TwoCombo:        // 10 二連線
                        AwardLabel.text = "10二連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Ten_ThreeCombo:        // 10 三連線
                        AwardLabel.text = "10三連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Ten_FourCombo:
                        AwardLabel.text = "10四連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Ten_FiveCombo:
                        AwardLabel.text = "10五連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_J_TwoCombo:          // J 二連線
                        AwardLabel.text = "J二連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_J_ThreeCombo:          // J 三連線
                        AwardLabel.text = "J三連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_J_FourCombo:
                        AwardLabel.text = "J四連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_J_FiveCombo:
                        AwardLabel.text = "J五連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Q_TwoCombo:          // Q 二連線
                        AwardLabel.text = "Q二連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Q_ThreeCombo:          // Q 三連線
                        AwardLabel.text = "Q三連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Q_FourCombo:
                        AwardLabel.text = "Q四連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Q_FiveCombo:
                        AwardLabel.text = "Q五連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_K_TwoCombo:         // K 二連線
                        AwardLabel.text = "K二連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_K_ThreeCombo:         // K 三連線
                        AwardLabel.text = "K三連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_K_FourCombo:
                        AwardLabel.text = "K四連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_K_FiveCombo:
                        AwardLabel.text = "K五連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_A_TwoCombo:         // A 二連線
                        AwardLabel.text = "A二連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_A_ThreeCombo:         // A 三連線
                        AwardLabel.text = "A三連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_A_FourCombo:
                        AwardLabel.text = "A四連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_A_FiveCombo:
                        AwardLabel.text = "A五連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Titanis_TwoCombo:   // 泰坦鳥 二連線
                        AwardLabel.text = "泰坦鳥二連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Titanis_ThreeCombo:   // 泰坦鳥 三連線
                        AwardLabel.text = "泰坦鳥三連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Titanis_FourCombo:
                        AwardLabel.text = "泰坦鳥四連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Titanis_FiveCombo:
                        AwardLabel.text = "泰坦鳥五連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Smilodon_TwoCombo:  // 劍齒虎 二連線
                        AwardLabel.text = "劍齒虎二連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Smilodon_ThreeCombo:  // 劍齒虎 三連線
                        AwardLabel.text = "劍齒虎三連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Smilodon_FourCombo:
                        AwardLabel.text = "劍齒虎四連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Smilodon_FiveCombo:
                        AwardLabel.text = "劍齒虎五連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Mammoth_TwoCombo:   // 長毛象 二連線
                        AwardLabel.text = "長毛象二連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Mammoth_ThreeCombo:   // 長毛象 三連線
                        AwardLabel.text = "長毛象三連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Mammoth_FourCombo:
                        AwardLabel.text = "長毛象四連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Mammoth_FiveCombo:
                        AwardLabel.text = "長毛象五連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Bone_TwoCombo:      // 恐龍骨 二連線
                        AwardLabel.text = "恐龍骨二連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Bone_ThreeCombo:      // 恐龍骨 三連線
                        AwardLabel.text = "恐龍骨三連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Bone_FourCombo:
                        AwardLabel.text = "射擊";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Bone_Five:
                        AwardLabel.text = "敲蛋";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.Card_Diamond_FiveCombo:    // 鑽石 五連線
                        AwardLabel.text = "鑽石五連線";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.BonusShoot:    // 鑽石 五連線
                        AwardLabel.text = "射擊";
                        break;
                    case (byte)ENUM_IceAge3X5_AWARD_ID.BonusEgg:    // 鑽石 五連線
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
