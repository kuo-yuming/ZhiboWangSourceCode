using UnityEngine;
using System.Collections;

public class IceAge_MachineAward : MonoBehaviour
{
    public UILabel m_MachineID;
    public UILabel m_MachineStatus;
    public UILabel m_PlayerName;
    public UILabel m_RunCnt;
    public UILabel m_Line;
    public UILabel m_Mammoth;
    public UILabel m_Smilodon;
    public UILabel m_Titanis;
    public UILabel m_Shoot;
    public UILabel m_Egg;
    bool AwardType = false;
    public GameObject m_AwardBG;
    uint NowMachineID = uint.MinValue;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IceAgeManager.m_MachineInfo != null)
        {
            if (IceAgeManager.m_MachineInfo.m_uiMID != NowMachineID)
            {
                if (GameConnet.m_PMachinesData.ContainsKey(IceAgeManager.m_MachineInfo.m_uiMID))
                {
                    NowMachineID = IceAgeManager.m_MachineInfo.m_uiMID;
                    m_MachineID.text = NowMachineID + "";
                    switch (GameConnet.m_PMachinesData[NowMachineID].m_enumState)
                    {
                        case GameCore.Machine.ENUM_PMACHINE_STATE.Idle:
                            if (VersionDef.InternationalLanguageSystem)
                                m_MachineStatus.text = Font_Control.Instance.GetMsgStrData(2000004);
                            else
                                m_MachineStatus.text = "閒置中";
                            break;
                        case GameCore.Machine.ENUM_PMACHINE_STATE.BuyinChk:
                            m_MachineStatus.text = "";
                            break;
                        case GameCore.Machine.ENUM_PMACHINE_STATE.Use:
                            if (VersionDef.InternationalLanguageSystem)
                                m_MachineStatus.text = Font_Control.Instance.GetMsgStrData(2000005);
                            else
                                m_MachineStatus.text = "使用中";
                            break;
                        case GameCore.Machine.ENUM_PMACHINE_STATE.Keep:
                            if (VersionDef.InternationalLanguageSystem)
                                m_MachineStatus.text = Font_Control.Instance.GetMsgStrData(2000006);
                            else
                                m_MachineStatus.text = "保留中";
                            break;
                        default:
                            m_MachineStatus.text = "";
                            break;
                    }

                    m_PlayerName.text = GameConnet.m_PMachinesData[NowMachineID].m_strName;
                    if (AwardType)
                    {
                        AwardOneOnClick();
                    }
                    else
                    {
                        ChacgeAwardText(0);
                    }

                }
            }
        }

    }
    public void AwardThreeOnClick()
    {
        if (!AwardType)
        {
            m_AwardBG.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
            ChacgeAwardText(1);
            AwardType = !AwardType;
        }

    }
    public void AwardOneOnClick()
    {
        if (AwardType)
        {
            m_AwardBG.transform.Rotate(new Vector3(0.0f, -180.0f, 0.0f));
            ChacgeAwardText(0);
            AwardType = !AwardType;
        }
    }

    //0--> 一天  1-->三天
    void ChacgeAwardText(int Type = 0)
    {
        if (IceAgeManager.m_MachineInfo.m_uiMID != 0)
        {
            if (Type == 0)
            {
                m_RunCnt.text = IceAgeManager.m_MachineInfo.m_oTodayCredit.m_uiRoundCnt + "";
                m_Line.text = IceAgeManager.m_MachineInfo.m_oTodayCredit.m_uiLineCnt + "";
                m_Mammoth.text = IceAgeManager.m_MachineInfo.m_oTodayCredit.m_uiMammothCnt + "";
                m_Smilodon.text = IceAgeManager.m_MachineInfo.m_oTodayCredit.m_uiSmilodonCnt + "";
                m_Titanis.text = IceAgeManager.m_MachineInfo.m_oTodayCredit.m_uiTitanisCnt + "";
                m_Shoot.text = IceAgeManager.m_MachineInfo.m_oTodayCredit.m_uiShootCnt + "";
                m_Egg.text = IceAgeManager.m_MachineInfo.m_oTodayCredit.m_uiEggCnt + "";
            }
            else
            {
                m_RunCnt.text = IceAgeManager.m_MachineInfo.m_oDaysCredit.m_uiRoundCnt + "";
                m_Line.text = IceAgeManager.m_MachineInfo.m_oDaysCredit.m_uiLineCnt + "";
                m_Mammoth.text = IceAgeManager.m_MachineInfo.m_oDaysCredit.m_uiMammothCnt + "";
                m_Smilodon.text = IceAgeManager.m_MachineInfo.m_oDaysCredit.m_uiSmilodonCnt + "";
                m_Titanis.text = IceAgeManager.m_MachineInfo.m_oDaysCredit.m_uiTitanisCnt + "";
                m_Shoot.text = IceAgeManager.m_MachineInfo.m_oDaysCredit.m_uiShootCnt + "";
                m_Egg.text = IceAgeManager.m_MachineInfo.m_oDaysCredit.m_uiEggCnt + "";
            }
        }
    }
}
