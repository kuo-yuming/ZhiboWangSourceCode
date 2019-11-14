using UnityEngine;
using System.Collections;

public class JurassicMachineAward : MonoBehaviour {

	public UILabel m_MachineID;
	public UILabel m_MachineStatus;
	public UILabel m_PlayerName;
	public UILabel m_RunCnt;
	public UILabel m_AllWin;
	public UILabel m_Pterosaur;
	public UILabel m_Triceratops;
	public UILabel m_Tyrannosaurus;
	public UILabel m_Combo;
	bool AwardType = false;
	public GameObject m_AwardBG;
	uint NowMachineID = uint.MinValue;


	void Update()
	{
		if (JurassicManager.m_MachineInfo == null)
			return;
		if (JurassicManager.m_MachineInfo.m_uiMID == NowMachineID)
			return;
		if (GameConnet.m_PMachinesData.ContainsKey(JurassicManager.m_MachineInfo.m_uiMID))
		{
			NowMachineID = JurassicManager.m_MachineInfo.m_uiMID;
			m_MachineID.text = NowMachineID.ToString();

			switch (GameConnet.m_PMachinesData[NowMachineID].m_enumState)
			{
			case GameCore.Machine.ENUM_PMACHINE_STATE.Idle:
				if (VersionDef.InternationalLanguageSystem)
					m_MachineStatus.text = Font_Control.Instance.GetMsgStrData(2000004);
				else
					m_MachineStatus.text = "閒置中";
				break;
			case GameCore.Machine.ENUM_PMACHINE_STATE.BuyinChk:
				if (VersionDef.InternationalLanguageSystem)
					m_MachineStatus.text = string.Empty;
				else
					m_MachineStatus.text = string.Empty;
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
				m_MachineStatus.text = string.Empty;
				break;
			}

			m_PlayerName.text = GameConnet.m_PMachinesData[NowMachineID].m_strName;

			if (AwardType)
				AwardOneOnClick();
			else
				ChacgeAwardText(true);
		}
	}
	public void AwardThreeOnClick()
	{
		if (AwardType)
			return;
		m_AwardBG.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
		ChacgeAwardText(!AwardType);
		AwardType = !AwardType;
	}
	public void AwardOneOnClick()
	{
		if (!AwardType)
			return;
		m_AwardBG.transform.Rotate(new Vector3(0.0f, -180.0f, 0.0f));
		ChacgeAwardText(AwardType);
		AwardType = !AwardType;
	}

	//0--> 一天  1-->三天
	void ChacgeAwardText(bool Type = true)
	{
		if (JurassicManager.m_MachineInfo.m_uiMID == 0)
			return;
		if (Type)
		{
			m_RunCnt.text = JurassicManager.m_MachineInfo.m_oTodayCredit.m_uiRoundCnt.ToString();
			m_AllWin.text = JurassicManager.m_MachineInfo.m_oTodayCredit.m_uiRexCnt.ToString();
			m_Pterosaur.text = JurassicManager.m_MachineInfo.m_oTodayCredit.m_uiPterosauriaCnt.ToString();
			m_Triceratops.text = JurassicManager.m_MachineInfo.m_oTodayCredit.m_uiTriceratopsCnt.ToString();
			m_Tyrannosaurus.text = JurassicManager.m_MachineInfo.m_oTodayCredit.m_uiLineCnt.ToString();
			m_Combo.text = JurassicManager.m_MachineInfo.m_oTodayCredit.m_uiComboCnt.ToString();
		}
		else
		{
			m_RunCnt.text = JurassicManager.m_MachineInfo.m_oDaysCredit.m_uiRoundCnt.ToString();
			m_AllWin.text = JurassicManager.m_MachineInfo.m_oDaysCredit.m_uiRexCnt.ToString();
			m_Pterosaur.text = JurassicManager.m_MachineInfo.m_oDaysCredit.m_uiPterosauriaCnt.ToString();
			m_Triceratops.text = JurassicManager.m_MachineInfo.m_oDaysCredit.m_uiTriceratopsCnt.ToString();
			m_Tyrannosaurus.text = JurassicManager.m_MachineInfo.m_oDaysCredit.m_uiLineCnt.ToString();
			m_Combo.text = JurassicManager.m_MachineInfo.m_oDaysCredit.m_uiComboCnt.ToString();
		}

	}
}
