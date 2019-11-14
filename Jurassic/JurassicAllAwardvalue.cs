using UnityEngine;
using System.Collections;
using GameCore.Manager.Jurassic;
using System;

public class JurassicAllAwardvalue : MonoBehaviour {
	public int ID;
	int RealDataID;
	UILabel TimeLabel;
	UILabel MachineIDLabel;
	UILabel PlayerNickName;
	UILabel AwardLabel;
	UILabel MoneyLabel;

	// Use this for initialization
	void Start () {
		TimeLabel = this.transform.FindChild ("Time").GetComponent<UILabel> ();
		MachineIDLabel = this.transform.FindChild ("MachineID").GetComponent<UILabel> ();
		AwardLabel = this.transform.FindChild ("Award").GetComponent<UILabel> ();
		MoneyLabel = this.transform.FindChild ("Money").GetComponent<UILabel> ();
		PlayerNickName = this.transform.FindChild("PlayerNickName").GetComponent<UILabel> ();
	}

	// Update is called once per frame
	void Update () {
		RealDataID = ((Jurassic_AllAward.O_Page - 1) * 12) + ID;
		if (!SortAward.O_AwardShow.ContainsKey (RealDataID)) {
			TimeLabel.enabled = false;
			MachineIDLabel.enabled = false;
			AwardLabel.enabled = false;
			MoneyLabel.enabled = false;
			PlayerNickName.enabled = false;
		} else {
			TimeLabel.enabled = true;
			MachineIDLabel.enabled = true;
			AwardLabel.enabled = true;
			MoneyLabel.enabled = true;
			PlayerNickName.enabled = true;
		}
		if (SortAward.O_AwardShow.ContainsKey (RealDataID)) {
			int ToDay = DateTime.Now.Day;
			int DataDay = (int)(((SortAward.O_AwardShow[RealDataID].m_ui64Time % 1000000) - (SortAward.O_AwardShow[RealDataID].m_ui64Time % 10000)) / 10000);

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


			ulong hour = (((SortAward.O_AwardShow[RealDataID].m_ui64Time % 10000) - (SortAward.O_AwardShow[RealDataID].m_ui64Time % 100))	/ 100);
			ulong min = (SortAward.O_AwardShow[RealDataID].m_ui64Time % 100);
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
			MachineIDLabel.text = SortAward.O_AwardShow[RealDataID].m_uiMID+"";
			MoneyLabel.text = SortAward.O_AwardShow[RealDataID].m_uiMoney+"";
			PlayerNickName.text = SortAward.O_AwardShow[RealDataID].m_strPlayerNickName;
			if(SortAward.O_AwardShow[RealDataID].m_byComboCnt != 0)
			{
				AwardLabel.text = "連莊獎 X "+SortAward.O_AwardShow[RealDataID].m_byComboCnt;
			}else{
				switch(SortAward.O_AwardShow[RealDataID].m_byAllWinAwardID)
				{
				case (byte)ENUM_JURASSIC_AWARD_ID.Litchi_2:        // 10 二連線
					AwardLabel.text = "荔枝二連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Grape_2:        // 10 三連線
					AwardLabel.text = "葡萄二連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Orange_2:
					AwardLabel.text = "柳丁二連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Pineapple_2:
					AwardLabel.text = "鳳梨二連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Watermelon_2:          // J 二連線
					AwardLabel.text = "西瓜二連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Pterosauria_2:          // J 三連線
					AwardLabel.text = "翼龍二連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Triceratops_2:
					AwardLabel.text = "三角龍二連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Rex_2:
					AwardLabel.text = "暴龍二連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Litchi_3:          // Q 二連線
					AwardLabel.text = "荔枝三連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Grape_3:          // Q 三連線
					AwardLabel.text = "葡萄三連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Orange_3:
					AwardLabel.text = "柳丁三連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Pineapple_3:
					AwardLabel.text = "鳳梨三連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Watermelon_3:         // K 二連線
					AwardLabel.text = "西瓜三連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Pterosauria_3:         // K 三連線
					AwardLabel.text = "翼龍三連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Triceratops_3:
					AwardLabel.text = "三角龍三連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Rex_3:
					AwardLabel.text = "暴龍三連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Litchi_4:         // A 二連線
					AwardLabel.text = "荔枝四連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Grape_4:         // A 三連線
					AwardLabel.text = "葡萄四連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Orange_4:
					AwardLabel.text = "柳丁四連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Pineapple_4:
					AwardLabel.text = "鳳梨四連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Watermelon_4:   // 泰坦鳥 二連線
					AwardLabel.text = "西瓜四連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Pterosauria_4:   // 泰坦鳥 三連線
					AwardLabel.text = "翼龍四連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Triceratops_4:
					AwardLabel.text = "三角龍四連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Rex_4:
					AwardLabel.text = "暴龍四連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Litchi_5:  // 劍齒虎 二連線
					AwardLabel.text = "荔枝五連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Grape_5:  // 劍齒虎 三連線
					AwardLabel.text = "葡萄五連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Orange_5:
					AwardLabel.text = "柳丁五連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Pineapple_5:
					AwardLabel.text = "鳳梨五連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Watermelon_5:   // 長毛象 二連線
					AwardLabel.text = "西瓜五連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Pterosauria_5:   // 長毛象 三連線
					AwardLabel.text = "翼龍五連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Triceratops_5:
					AwardLabel.text = "三角龍五連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Rex_5:
					AwardLabel.text = "暴龍五連線";
					break;
				case (byte)ENUM_JURASSIC_AWARD_ID.Diamond_5:      // 恐龍骨 二連線
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
