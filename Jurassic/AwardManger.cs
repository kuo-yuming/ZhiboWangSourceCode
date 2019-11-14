using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore.Machine;
using System.Linq;

public class AwardManger : MonoBehaviour {

	public GameObject SelfAward;
	public GameObject AllfAward;
	public List<CPACK_PMachineAwardRecord> SelfAwardRecord = new List<CPACK_PMachineAwardRecord>();
	public List<CPACK_PMachineAwardRecord> AllAwardRecord = new List<CPACK_PMachineAwardRecord>();

	private static AwardManger _instance;

	public static AwardManger instance
	{
		get{

			return _instance;
		}
	}

	void Start()
	{
		if (_instance == null)
			_instance = this.gameObject.GetComponent<AwardManger> ();
	}

	public void SortInTime()
	{
		SelfAwardRecord = SelfAwardRecord.OrderBy (x => x.m_ui64Time).ToList ();
		AllAwardRecord = AllAwardRecord.OrderBy (x => x.m_ui64Time).ToList ();
	}

	public void SortInMachineId()
	{
		SelfAwardRecord = SelfAwardRecord.OrderBy (x => x.m_uiMID).ToList ();
		AllAwardRecord = AllAwardRecord.OrderBy (x => x.m_uiMID).ToList ();
	}

	public void SortInName()
	{
		SelfAwardRecord = SelfAwardRecord.OrderBy (x => x.m_strPlayerNickName).ToList ();
		AllAwardRecord = AllAwardRecord.OrderBy (x => x.m_strPlayerNickName).ToList ();
	}

	public void SortInMoney()
	{
		SelfAwardRecord = SelfAwardRecord.OrderBy (x => x.m_uiMoney).ToList ();
		AllAwardRecord = AllAwardRecord.OrderBy (x => x.m_uiMoney).ToList ();
	}

	public void SortInAward()
	{
		//SelfAwardRecord = SelfAwardRecord.OrderBy (x => x.m_byAllWinAwardID).ToList ();
		//AllAwardRecord = AllAwardRecord.OrderBy (x => x.m_byAllWinAwardID).ToList ();
		SelfAwardRecord.Clear();
		List<CPACK_PMachineAwardRecord> Loca_Data = new List<CPACK_PMachineAwardRecord>(JurassicManager.M_AwardRecord);
		int Index = 0;
		int Long_all = Loca_Data.Count;
		foreach (CPACK_PMachineAwardRecord Data in Loca_Data)
		{
			if (Data.m_byAllWinAwardID == 12)
			{
				SelfAwardRecord.Add(Data);
				Index++;
			}
		}
	
		List<CPACK_PMachineAwardRecord> Loca_Data_ComboOnly = new List<CPACK_PMachineAwardRecord>();
		for (int i = 0; i < Long_all; i++)
		{
			if (Loca_Data[i].m_byComboCnt != 0)
			{
				Loca_Data_ComboOnly.Add(Loca_Data[i]);
			}

		}

		if (Loca_Data_ComboOnly.Count > 0)
		{

			CPACK_PMachineAwardRecord Temp = new CPACK_PMachineAwardRecord();
			Loca_Data_ComboOnly = Loca_Data_ComboOnly.OrderBy (x => x.m_byComboCnt).ToList ();
//			for (int i = 0; i <= (Loca_Data_ComboOnly.Count) - 1; i++)
//			{
//
//				for (int j = i + 1; j < Loca_Data_ComboOnly.Count; j++)
//				{
//
//					if (Loca_Data_ComboOnly[j].m_byComboCnt > Loca_Data_ComboOnly[i].m_byComboCnt)
//					{
//						Temp = Loca_Data_ComboOnly[i];
//						Loca_Data_ComboOnly[i] = Loca_Data_ComboOnly[j];
//						Loca_Data_ComboOnly[j] = Temp;
//					}
//
//				}
//			}
			for (int i = 0; i < Loca_Data_ComboOnly.Count; i++)
			{
				SelfAwardRecord.Add(Loca_Data_ComboOnly[i]);
				Index++;

			}

		}

		//	int Long_NoDiamondNoCombo = Loca_Data.Count;
		List<CPACK_PMachineAwardRecord> Loca_Data_FINAL = new List<CPACK_PMachineAwardRecord>();
		for (int i = 0; i < Loca_Data.Count; i++)
		{
			if (Loca_Data[i].m_byComboCnt == 0 && Loca_Data[i].m_byAllWinAwardID < 12)
			{
				Loca_Data_FINAL.Add(Loca_Data[i]);
			}
		}

		if (Loca_Data_FINAL.Count > 0)
		{
			Loca_Data_FINAL = Loca_Data_FINAL.OrderBy (x => x.m_byAllWinAwardID).ToList ();
//			for (int i = 0; i <= (Loca_Data_FINAL.Count) - 1; i++)
//			{
//
//				for (int j = i + 1; j < Loca_Data_FINAL.Count; j++)
//				{
//
//					if (Loca_Data_FINAL[j].m_byAllWinAwardID > Loca_Data_FINAL[i].m_byAllWinAwardID)
//					{
//						CPACK_PMachineAwardRecord Temp = Loca_Data_FINAL[i];
//						Loca_Data_FINAL[i] = Loca_Data_FINAL[j];
//						Loca_Data_FINAL[j] = Temp;
//					}
//
//				}
//			}
			for (int i = 0; i < Loca_Data_FINAL.Count; i++)
			{
				SelfAwardRecord.Add(Loca_Data_FINAL[i]);
				Index++;
			}


		}

//		Loca_Data.Clear();
//		for (int i = 0; i < SelfAwardRecord.Count; i++)
//		{
//			Loca_Data.Add(SelfAwardRecord[i]);
//		}
//		SelfAwardRecord.Clear();
//
//		Loca_Data.Reverse();
//		for (int i = 0; i < Loca_Data.Count; i++)
//		{
//			SelfAwardRecord.Add(Loca_Data[i]);
//
//		}
	}

	public void SortInAllAward()
	{
		//SelfAwardRecord = SelfAwardRecord.OrderBy (x => x.m_byAllWinAwardID).ToList ();
		//AllAwardRecord = AllAwardRecord.OrderBy (x => x.m_byAllWinAwardID).ToList ();
		AllAwardRecord.Clear();
		List<CPACK_PMachineAwardRecord> Loca_Data = new List<CPACK_PMachineAwardRecord>(FruitManager.M_AwardRecord);
		int Index = 0;
		int Long_all = Loca_Data.Count;
		foreach (CPACK_PMachineAwardRecord Data in Loca_Data)
		{
			if (Data.m_byAllWinAwardID == 12)
			{
				AllAwardRecord.Add(Data);
				Index++;
			}
		}

		List<CPACK_PMachineAwardRecord> Loca_Data_ComboOnly = new List<CPACK_PMachineAwardRecord>();
		for (int i = 0; i < Long_all; i++)
		{
			if (Loca_Data[i].m_byComboCnt != 0)
			{
				Loca_Data_ComboOnly.Add(Loca_Data[i]);
			}

		}

		if (Loca_Data_ComboOnly.Count > 0)
		{

			CPACK_PMachineAwardRecord Temp = new CPACK_PMachineAwardRecord();
			Loca_Data_ComboOnly = Loca_Data_ComboOnly.OrderBy (x => x.m_byComboCnt).ToList ();
			//			for (int i = 0; i <= (Loca_Data_ComboOnly.Count) - 1; i++)
			//			{
			//
			//				for (int j = i + 1; j < Loca_Data_ComboOnly.Count; j++)
			//				{
			//
			//					if (Loca_Data_ComboOnly[j].m_byComboCnt > Loca_Data_ComboOnly[i].m_byComboCnt)
			//					{
			//						Temp = Loca_Data_ComboOnly[i];
			//						Loca_Data_ComboOnly[i] = Loca_Data_ComboOnly[j];
			//						Loca_Data_ComboOnly[j] = Temp;
			//					}
			//
			//				}
			//			}
			for (int i = 0; i < Loca_Data_ComboOnly.Count; i++)
			{
				AllAwardRecord.Add(Loca_Data_ComboOnly[i]);
				Index++;

			}

		}

		//	int Long_NoDiamondNoCombo = Loca_Data.Count;
		List<CPACK_PMachineAwardRecord> Loca_Data_FINAL = new List<CPACK_PMachineAwardRecord>();
		for (int i = 0; i < Loca_Data.Count; i++)
		{
			if (Loca_Data[i].m_byComboCnt == 0 && Loca_Data[i].m_byAllWinAwardID < 12)
			{
				Loca_Data_FINAL.Add(Loca_Data[i]);
			}
		}

		if (Loca_Data_FINAL.Count > 0)
		{
			Loca_Data_FINAL = Loca_Data_FINAL.OrderBy (x => x.m_byAllWinAwardID).ToList ();
			//			for (int i = 0; i <= (Loca_Data_FINAL.Count) - 1; i++)
			//			{
			//
			//				for (int j = i + 1; j < Loca_Data_FINAL.Count; j++)
			//				{
			//
			//					if (Loca_Data_FINAL[j].m_byAllWinAwardID > Loca_Data_FINAL[i].m_byAllWinAwardID)
			//					{
			//						CPACK_PMachineAwardRecord Temp = Loca_Data_FINAL[i];
			//						Loca_Data_FINAL[i] = Loca_Data_FINAL[j];
			//						Loca_Data_FINAL[j] = Temp;
			//					}
			//
			//				}
			//			}
			for (int i = 0; i < Loca_Data_FINAL.Count; i++)
			{
				AllAwardRecord.Add(Loca_Data_FINAL[i]);
				Index++;
			}


		}

//		Loca_Data.Clear();
//		for (int i = 0; i < AllAwardRecord.Count; i++)
//		{
//			Loca_Data.Add(AllAwardRecord[i]);
//		}
//		AllAwardRecord.Clear();
//
//		Loca_Data.Reverse();
//		for (int i = 0; i < Loca_Data.Count; i++)
//		{
//			AllAwardRecord.Add(Loca_Data[i]);
//
//		}
	}
}
