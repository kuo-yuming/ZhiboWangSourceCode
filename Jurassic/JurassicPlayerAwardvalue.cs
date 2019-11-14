using UnityEngine;
using System.Collections;
using System;
using GameCore.Manager.Jurassic;

public class JurassicPlayerAwardvalue : MonoBehaviour {
	public int ID;
	int RealDataID;
	UILabel TimeLabel;
	UILabel MachineIDLabel;
	UILabel AwardLabel;
	UILabel MoneyLabel;

	// Use this for initialization
	void Start () {
		TimeLabel = this.transform.FindChild ("Time").GetComponent<UILabel> ();
		MachineIDLabel = this.transform.FindChild ("MachineID").GetComponent<UILabel> ();
		AwardLabel = this.transform.FindChild ("Award").GetComponent<UILabel> ();
		MoneyLabel = this.transform.FindChild ("Money").GetComponent<UILabel> ();

	}

	// Update is called once per frame
	void Update () {
		RealDataID = ((Jurassic_PlayerAward.M_Page - 1) * 12) + ID;
		if (!SortAward.M_AwardShow.ContainsKey (RealDataID)) {
			TimeLabel.enabled = false;
			MachineIDLabel.enabled = false;
			AwardLabel.enabled = false;
			MoneyLabel.enabled = false;

		} else {
			TimeLabel.enabled = true;
			MachineIDLabel.enabled = true;
			AwardLabel.enabled = true;
			MoneyLabel.enabled = true;
		}
		if (SortAward.M_AwardShow.ContainsKey (RealDataID)) {
			int ToDay = DateTime.Now.Day;
			int DataDay = (int)(((SortAward.M_AwardShow[RealDataID].m_ui64Time % 1000000) - (SortAward.M_AwardShow[RealDataID].m_ui64Time % 10000)) / 10000);
			int DataDay_two = (int)((SortAward.M_AwardShow[RealDataID].m_ui64Time / 10000) % 100);
			//			Debug.Log("DataID : " + RealDataID + " Time64 " + SortAward.M_AwardShow[RealDataID].m_ui64Time + " toDay " + ToDay + " DataDay " + DataDay + " DataDay2 " + DataDay_two);
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
			ulong hour = (((SortAward.M_AwardShow[RealDataID].m_ui64Time % 10000) - (SortAward.M_AwardShow[RealDataID].m_ui64Time % 100))	/ 100);
			ulong min = (SortAward.M_AwardShow[RealDataID].m_ui64Time % 100);
			if(hour < 10 && min >= 10)
			{
				TimeLabel.text = "0"+hour+":"+min;
			}else if(hour >= 10 && min < 10)
			{
				TimeLabel.text = hour+":0"+min;
			}else if(hour < 10 && min < 10)
			{
				TimeLabel.text = "0"+hour+":0"+min;
			}else if(hour >= 10 && min >= 10)
			{
				TimeLabel.text = hour+":"+min;
			}

			MachineIDLabel.text = SortAward.M_AwardShow[RealDataID].m_uiMID+"";
			MoneyLabel.text = SortAward.M_AwardShow[RealDataID].m_uiMoney+"";
			if(SortAward.M_AwardShow[RealDataID].m_byComboCnt != 0)
			{
				AwardLabel.text = "連莊獎 X "+SortAward.M_AwardShow[RealDataID].m_byComboCnt;
			}else{
				switch(SortAward.M_AwardShow[RealDataID].m_byAllWinAwardID)
				{
				case (byte)ENUM_JURASSIC_AWARD_ID.Litchi_2:        // 10 二連線
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004002);
					else
					AwardLabel.text = "櫻桃二連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Grape_2:  
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004003);
					else// 10 三連線
					AwardLabel.text = "葡萄二連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Orange_2:
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004004);
					else
					AwardLabel.text = "柳丁二連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Pineapple_2:
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004005);
					else
					AwardLabel.text = "鳳梨二連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Watermelon_2:   
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004006);
					else// J 二連線
					AwardLabel.text = "西瓜二連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Pterosauria_2:  
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004007);
					else// J 三連線
					AwardLabel.text = "翼龍二連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Triceratops_2:
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004008);
					else
					AwardLabel.text = "三角龍二連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Rex_2:
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004009);
					else
					AwardLabel.text = "暴龍二連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Litchi_3:  
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004010);
					else// Q 二連線
					AwardLabel.text = "荔枝三連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Grape_3: 
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004011);
					else// Q 三連線
					AwardLabel.text = "葡萄三連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Orange_3:
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004012);
					else
					AwardLabel.text = "柳丁三連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Pineapple_3:
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004013);
					else
					AwardLabel.text = "鳳梨三連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Watermelon_3:  
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004014);
					else// K 二連線
					AwardLabel.text = "西瓜三連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Pterosauria_3:   
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004015);
					else// K 三連線
					AwardLabel.text = "翼龍三連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Triceratops_3:
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004016);
					else
					AwardLabel.text = "三角龍三連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Rex_3:
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004017);
					else
					AwardLabel.text = "暴龍三連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Litchi_4:   
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004019);
					else// A 二連線
					AwardLabel.text = "荔枝四連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Grape_4:   
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004020);
					else// A 三連線
					AwardLabel.text = "葡萄四連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Orange_4:
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004021);
					else
					AwardLabel.text = "柳丁四連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Pineapple_4:
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004022);
					else
					AwardLabel.text = "鳳梨四連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Watermelon_4: 
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004023);
					else// 泰坦鳥 二連線
					AwardLabel.text = "西瓜四連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Pterosauria_4: 
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004024);
					else// 泰坦鳥 三連線
					AwardLabel.text = "翼龍四連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Triceratops_4:
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004025);
					else
					AwardLabel.text = "三角龍四連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Rex_4:
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004026);
					else
					AwardLabel.text = "暴龍四連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Litchi_5:  
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004027);
					else// 劍齒虎 二連線
					AwardLabel.text = "荔枝五連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Grape_5:  
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004028);
					else// 劍齒虎 三連線
					AwardLabel.text = "葡萄五連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Orange_5:
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004029);
					else
					AwardLabel.text = "柳丁五連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Pineapple_5:
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004030);
					else
					AwardLabel.text = "鳳梨五連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Watermelon_5: 
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004031);
					else// 長毛象 二連線
					AwardLabel.text = "西瓜五連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Pterosauria_5:   // 長毛象 三連線
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004032);
					else
					AwardLabel.text = "翼龍五連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Triceratops_5:
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004033);
					else
					AwardLabel.text = "三角龍五連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Rex_5:
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004034);
					else
					AwardLabel.text = "暴龍五連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Diamond_5:      // 恐龍骨 二連線
					if (VersionDef.InternationalLanguageSystem)
						AwardLabel.text = Font_Control.Instance.GetMsgStrData (2004035);
					else
					AwardLabel.text = "鑽石五連線";
					break;
				default:
					AwardLabel.text = SortAward.O_AwardShow[RealDataID].m_byAllWinAwardID+"";
					break;
				}
			}
		}
	}
}
