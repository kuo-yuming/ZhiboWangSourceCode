using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore.Machine;

public class SortAward : MonoBehaviour {
	public static Dictionary<int,CPACK_PMachineAwardRecord> M_AwardShow = new Dictionary<int, CPACK_PMachineAwardRecord> ();
	public static Dictionary<int,CPACK_PMachineAwardRecord> O_AwardShow = new Dictionary<int, CPACK_PMachineAwardRecord> ();
	public M_SortingStatus MSorting = M_SortingStatus.Idle;
	public O_SortingStatus OSorting = O_SortingStatus.Idle;

	public enum M_SortingStatus
	{ 
		Idle                     = 0,     
		TimeFirst                = 1,  
		ReTimeFirst              = 2,  
		AwardFirst               = 3,
		ReAwardFirst             = 4, 
		MachineIDFirst           = 5,
		ReMachineIDFirst         = 6,
		MoneyFirst               = 7, 
		ReMoneyFirst             = 8, 



	};

	public enum O_SortingStatus
	{ 
		Idle                     = 0,     
		TimeFirst                = 1,  
		ReTimeFirst              = 2,  
		AwardFirst               = 3,
		ReAwardFirst             = 4, 
		MachineIDFirst           = 5,
		ReMachineIDFirst         = 6,
		MoneyFirst               = 7, 
		ReMoneyFirst             = 8,  
		NameFirst                = 9,
		ReNameFirst              = 10,



	};
	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{
		if (JurassicManager.M_AwardPacket.m_bEnd) {
			MSorting = M_SortingStatus.Idle;
			M_TimeFirst ();

			JurassicManager.M_AwardPacket.m_bEnd = false;
		}
		if (JurassicManager.O_AwardPacket.m_bEnd) {
			OSorting = O_SortingStatus.Idle;
			O_TimeFirst ();
			//OSorting = O_SortingStatus.TimeFirst;
			JurassicManager.O_AwardPacket.m_bEnd = false;
		}

	}
	//個人大獎排序

	public void M_TimeFirst ()
	{
		M_AwardShow.Clear ();
		List<CPACK_PMachineAwardRecord> Loca_Data = new List<CPACK_PMachineAwardRecord>(JurassicManager.M_AwardRecord);

		CPACK_PMachineAwardRecord Temp = new CPACK_PMachineAwardRecord ();

		for (int i = 0; i <=(Loca_Data.Count)-1; i++) {

			for (int j = i+1; j < Loca_Data.Count; j++) {

				if (Loca_Data [j].m_ui64Time > Loca_Data [i].m_ui64Time) {
					Temp = Loca_Data [i];
					Loca_Data [i] = Loca_Data [j];
					Loca_Data [j] = Temp;

				}

			}

		}

		if (MSorting == M_SortingStatus.TimeFirst) {
			Loca_Data.Reverse ();
			for (int i = 0; i < Loca_Data.Count; i++) {
				M_AwardShow.Add (i, Loca_Data [i]);

			}
			MSorting = M_SortingStatus.ReTimeFirst;
		} else {
			for (int i = 0; i < Loca_Data.Count; i++) {
				M_AwardShow.Add (i, Loca_Data [i]);

			}
			MSorting = M_SortingStatus.TimeFirst;
		}

	}

	public void M_AwardFirst ()
	{
		M_AwardShow.Clear ();
		List<CPACK_PMachineAwardRecord> Loca_Data = new List<CPACK_PMachineAwardRecord>(JurassicManager.M_AwardRecord);
		int Index = 0;
		int Long_all = Loca_Data.Count;
		foreach (CPACK_PMachineAwardRecord Data in Loca_Data) {
			if (Data.m_byAllWinAwardID == 33) {
				M_AwardShow.Add (Index, Data);
				Index++;
			}
		}

		List<CPACK_PMachineAwardRecord> Loca_Data_ComboOnly = new List<CPACK_PMachineAwardRecord> ();
		for (int i = 0; i < Long_all; i++) {
			if (Loca_Data [i].m_byComboCnt != 0) {
				Loca_Data_ComboOnly.Add (Loca_Data [i]);
			}	
		}
		if (Loca_Data_ComboOnly.Count > 0) {

			CPACK_PMachineAwardRecord Temp = new CPACK_PMachineAwardRecord ();

			for (int i = 0; i <=(Loca_Data_ComboOnly.Count)-1; i++) {

				for (int j = i+1; j < Loca_Data_ComboOnly.Count; j++) {

					if (Loca_Data_ComboOnly [j].m_byComboCnt > Loca_Data_ComboOnly [i].m_byComboCnt) {
						Temp = Loca_Data_ComboOnly [i];
						Loca_Data_ComboOnly [i] = Loca_Data_ComboOnly [j];
						Loca_Data_ComboOnly [j] = Temp;
					}
				}
			}
			for (int i = 0; i < Loca_Data_ComboOnly.Count; i++) {
				M_AwardShow.Add (Index, Loca_Data_ComboOnly [i]);
				Index++;
			} 
		}

		//	int Long_NoDiamondNoCombo = Loca_Data.Count;
		List<CPACK_PMachineAwardRecord> Loca_Data_FINAL = new List<CPACK_PMachineAwardRecord> ();
		for (int i = 0; i < Loca_Data.Count; i++) {
			if (Loca_Data [i].m_byComboCnt == 0 && Loca_Data [i].m_byAllWinAwardID < 33) {
				Loca_Data_FINAL.Add (Loca_Data [i]);
			}
		}

		if (Loca_Data_FINAL.Count > 0) {
			for (int i = 0; i <=(Loca_Data_FINAL.Count)-1; i++) {

				for (int j = i+1; j < Loca_Data_FINAL.Count; j++) {

					if (Loca_Data_FINAL [j].m_byAllWinAwardID > Loca_Data_FINAL [i].m_byAllWinAwardID) {
						CPACK_PMachineAwardRecord Temp = Loca_Data_FINAL [i];
						Loca_Data_FINAL [i] = Loca_Data_FINAL [j];
						Loca_Data_FINAL [j] = Temp;
					}
				}
			}
			for (int i = 0; i < Loca_Data_FINAL.Count; i++) {
				M_AwardShow.Add (Index, Loca_Data_FINAL [i]);
				Index++;
			} 	
		}
		Loca_Data.Clear ();
		for (int i = 0; i < M_AwardShow.Count; i++) {
			Loca_Data.Add(M_AwardShow[i]);
		}
		M_AwardShow.Clear ();
		if (MSorting == M_SortingStatus.AwardFirst) {
			Loca_Data.Reverse();
			for (int i = 0; i < Loca_Data.Count; i++) {
				M_AwardShow.Add (i, Loca_Data [i]);
			}
			MSorting = M_SortingStatus.ReAwardFirst;
		}else{
			for (int i = 0; i < Loca_Data.Count; i++) {
				M_AwardShow.Add (i, Loca_Data [i]);
			}
			MSorting = M_SortingStatus.AwardFirst;
		}
	}

	public void M_MachineIDFirst ()
	{
		M_AwardShow.Clear ();
		List<CPACK_PMachineAwardRecord> Loca_Data = new List<CPACK_PMachineAwardRecord>(JurassicManager.M_AwardRecord);
		CPACK_PMachineAwardRecord Temp = new CPACK_PMachineAwardRecord ();

		for (int i = 0; i <=(Loca_Data.Count)-1; i++) {

			for (int j = i+1; j < Loca_Data.Count; j++) {
				if (Loca_Data [j].m_uiMID > Loca_Data [i].m_uiMID) {
					Temp = Loca_Data [i];
					Loca_Data [i] = Loca_Data [j];
					Loca_Data [j] = Temp;
				}
			}

		}
		if (MSorting == M_SortingStatus.MachineIDFirst) {
			Loca_Data.Reverse ();
			for (int i = 0; i < Loca_Data.Count; i++) {
				M_AwardShow.Add (i, Loca_Data [i]);

			}
			MSorting = M_SortingStatus.ReMachineIDFirst;
		} else {
			for (int i = 0; i < Loca_Data.Count; i++) {
				M_AwardShow.Add (i, Loca_Data [i]);

			}
			MSorting = M_SortingStatus.MachineIDFirst;
		}
	}

	public void M_MoneyFirst ()
	{
		M_AwardShow.Clear ();
		List<CPACK_PMachineAwardRecord> Loca_Data = new List<CPACK_PMachineAwardRecord>(JurassicManager.M_AwardRecord);
		CPACK_PMachineAwardRecord Temp = new CPACK_PMachineAwardRecord ();

		for (int i = 0; i <=(Loca_Data.Count)-1; i++) {

			for (int j = i+1; j < Loca_Data.Count; j++) {
				if (Loca_Data [j].m_uiMoney > Loca_Data [i].m_uiMoney) {
					Temp = Loca_Data [i];
					Loca_Data [i] = Loca_Data [j];
					Loca_Data [j] = Temp;

				}

			}

		}
		if (MSorting == M_SortingStatus.MoneyFirst) {
			Loca_Data.Reverse ();
			for (int i = 0; i < Loca_Data.Count; i++) {
				M_AwardShow.Add (i, Loca_Data [i]);

			}
			MSorting = M_SortingStatus.ReMoneyFirst;
		} else {
			for (int i = 0; i < Loca_Data.Count; i++) {
				M_AwardShow.Add (i, Loca_Data [i]);

			}
			MSorting = M_SortingStatus.MoneyFirst;
		}
	}

	//----------------------------------------------------------------------------------------------------------------------------------
	//本廳大獎排序
	public void O_TimeFirst ()
	{
		O_AwardShow.Clear ();
		List<CPACK_PMachineAwardRecord> Loca_Data = new List<CPACK_PMachineAwardRecord> (JurassicManager.O_AwardRecord);


		CPACK_PMachineAwardRecord Temp = new CPACK_PMachineAwardRecord ();

		for (int i = 0; i <=(Loca_Data.Count)-1; i++) {

			for (int j = i+1; j < Loca_Data.Count; j++) {

				if (Loca_Data [j].m_ui64Time > Loca_Data [i].m_ui64Time) {
					Temp = Loca_Data [i];
					Loca_Data [i] = Loca_Data [j];
					Loca_Data [j] = Temp;

				}

			}

		}
		if (OSorting == O_SortingStatus.TimeFirst) {
			Loca_Data.Reverse ();
			for (int i = 0; i < Loca_Data.Count; i++) {
				O_AwardShow.Add (i, Loca_Data [i]);

			}
			OSorting = O_SortingStatus.ReTimeFirst;
		} else {
			for (int i = 0; i < Loca_Data.Count; i++) {
				O_AwardShow.Add (i, Loca_Data [i]);

			}
			OSorting = O_SortingStatus.TimeFirst;
		}
	}

	public void O_NameFirst ()
	{
		O_AwardShow.Clear ();
		List<CPACK_PMachineAwardRecord> Loca_Data = new List<CPACK_PMachineAwardRecord> (JurassicManager.O_AwardRecord);
		List<CPACK_PMachineAwardRecord> Final_Data = new List<CPACK_PMachineAwardRecord> ();
		//	string Temp = new string;
		Dictionary<string,uint> NameDictionary = new Dictionary<string,uint> ();
		Dictionary<uint,List<int>> DataIndex = new Dictionary<uint, List<int>> ();
		for (int i = 0; i < Loca_Data.Count; i++) {
			if (!NameDictionary.ContainsKey (Loca_Data [i].m_strPlayerNickName)) {
				NameDictionary.Add (Loca_Data [i].m_strPlayerNickName, Loca_Data [i].m_uiPlayerDBID);
				List<int> Data = new List<int> ();
				Data.Add (i);
				DataIndex.Add (Loca_Data [i].m_uiPlayerDBID, Data);
			} else {
				if (DataIndex.ContainsKey (Loca_Data [i].m_uiPlayerDBID)) {
					DataIndex [Loca_Data [i].m_uiPlayerDBID].Add (i);
				}

			}
		}

		List<string> NameList = new List<string> (NameDictionary.Keys);
		if (OSorting == O_SortingStatus.NameFirst) {
			NameList.Sort ();
		} else {
			NameList.Sort ();
			//NameList.Reverse ();
		}

		for (int i = 0; i < NameList.Count; i++) {
			for (int j = 0; j < DataIndex[NameDictionary[NameList[i]]].Count; j++) {
				Final_Data.Add (Loca_Data [DataIndex [NameDictionary [NameList [i]]] [j]]);
			}
		}
		if (OSorting == O_SortingStatus.NameFirst) {

			for (int i = 0; i < Final_Data.Count; i++) {
				O_AwardShow.Add (i, Final_Data [i]);

			}
			OSorting = O_SortingStatus.ReNameFirst;
		} else {
			Final_Data.Reverse ();
			for (int i = 0; i < Final_Data.Count; i++) {
				O_AwardShow.Add (i, Final_Data [i]);

			}
			OSorting = O_SortingStatus.NameFirst;
		}
	}

	public void O_AwardFirst ()
	{
		O_AwardShow.Clear ();
		List<CPACK_PMachineAwardRecord> Loca_Data = new List < CPACK_PMachineAwardRecord >(JurassicManager.O_AwardRecord);
		int Index = 0;
		int Long_all = Loca_Data.Count;
		foreach (CPACK_PMachineAwardRecord Data in Loca_Data) {
			if (Data.m_byAllWinAwardID == 33) {
				O_AwardShow.Add (Index, Data);
				Index++;

			}


		}


		List<CPACK_PMachineAwardRecord> Loca_Data_ComboOnly = new List<CPACK_PMachineAwardRecord> ();
		for (int i = 0; i < Long_all; i++) {
			if (Loca_Data [i].m_byComboCnt != 0) {
				Loca_Data_ComboOnly.Add (Loca_Data [i]);
			}

		}
		if (Loca_Data_ComboOnly.Count > 0) {

			CPACK_PMachineAwardRecord Temp = new CPACK_PMachineAwardRecord ();

			for (int i = 0; i <=(Loca_Data_ComboOnly.Count)-1; i++) {

				for (int j = i+1; j < Loca_Data_ComboOnly.Count; j++) {

					if (Loca_Data_ComboOnly [j].m_byComboCnt > Loca_Data_ComboOnly [i].m_byComboCnt) {
						Temp = Loca_Data_ComboOnly [i];
						Loca_Data_ComboOnly [i] = Loca_Data_ComboOnly [j];
						Loca_Data_ComboOnly [j] = Temp;

					}

				}

			}
			for (int i = 0; i < Loca_Data_ComboOnly.Count; i++) {
				O_AwardShow.Add (Index, Loca_Data_ComboOnly [i]);
				Index++;

			} 

		}

		//	int Long_NoDiamondNoCombo = Loca_Data.Count;
		List<CPACK_PMachineAwardRecord> Loca_Data_FINAL = new List<CPACK_PMachineAwardRecord> ();
		for (int i = 0; i < Loca_Data.Count; i++) {
			if (Loca_Data [i].m_byComboCnt == 0 && Loca_Data [i].m_byAllWinAwardID < 33) {
				Loca_Data_FINAL.Add (Loca_Data [i]);
			}
		}

		if (Loca_Data_FINAL.Count > 0) {
			for (int i = 0; i <=(Loca_Data_FINAL.Count)-1; i++) {

				for (int j = i+1; j < Loca_Data_FINAL.Count; j++) {

					if (Loca_Data_FINAL [j].m_byAllWinAwardID > Loca_Data_FINAL [i].m_byAllWinAwardID) {
						CPACK_PMachineAwardRecord Temp = Loca_Data_FINAL [i];
						Loca_Data_FINAL [i] = Loca_Data_FINAL [j];
						Loca_Data_FINAL [j] = Temp;

					}

				}

			}
			for (int i = 0; i < Loca_Data_FINAL.Count; i++) {
				O_AwardShow.Add (Index, Loca_Data_FINAL [i]);
				Index++;

			} 	


		}

		Loca_Data.Clear ();
		for (int i = 0; i < O_AwardShow.Count; i++) {
			Loca_Data.Add(O_AwardShow[i]);
		}
		O_AwardShow.Clear ();
		if (OSorting == O_SortingStatus.AwardFirst) {
			Loca_Data.Reverse();
			for (int i = 0; i < Loca_Data.Count; i++) {
				O_AwardShow.Add (i, Loca_Data [i]);

			}
			OSorting = O_SortingStatus.ReAwardFirst;
		}else{
			for (int i = 0; i < Loca_Data.Count; i++) {
				O_AwardShow.Add (i, Loca_Data [i]);

			}
			OSorting = O_SortingStatus.AwardFirst;
		}



	}

	public void O_MachineIDFirst ()
	{
		O_AwardShow.Clear ();
		List<CPACK_PMachineAwardRecord> Loca_Data = new List<CPACK_PMachineAwardRecord> (JurassicManager.O_AwardRecord);
		CPACK_PMachineAwardRecord Temp = new CPACK_PMachineAwardRecord ();

		for (int i = 0; i <=(Loca_Data.Count)-1; i++) {

			for (int j = i+1; j < Loca_Data.Count; j++) {
				if (Loca_Data [j].m_uiMID > Loca_Data [i].m_uiMID) {
					Temp = Loca_Data [i];
					Loca_Data [i] = Loca_Data [j];
					Loca_Data [j] = Temp;

				}

			}

		}
		if (OSorting == O_SortingStatus.MachineIDFirst) {
			Loca_Data.Reverse ();
			for (int i = 0; i < Loca_Data.Count; i++) {
				O_AwardShow.Add (i, Loca_Data [i]);

			}
			OSorting = O_SortingStatus.ReMachineIDFirst;
		} else {
			for (int i = 0; i < Loca_Data.Count; i++) {
				O_AwardShow.Add (i, Loca_Data [i]);

			}
			OSorting = O_SortingStatus.MachineIDFirst;
		}
	}

	public void O_MoneyFirst ()
	{
		O_AwardShow.Clear ();
		List<CPACK_PMachineAwardRecord> Loca_Data = new List<CPACK_PMachineAwardRecord> (JurassicManager.O_AwardRecord);
		CPACK_PMachineAwardRecord Temp = new CPACK_PMachineAwardRecord ();

		for (int i = 0; i <=(Loca_Data.Count)-1; i++) {

			for (int j = i+1; j < Loca_Data.Count; j++) {
				if (Loca_Data [j].m_uiMoney > Loca_Data [i].m_uiMoney) {
					Temp = Loca_Data [i];
					Loca_Data [i] = Loca_Data [j];
					Loca_Data [j] = Temp;

				}

			}

		}
		if (OSorting == O_SortingStatus.MoneyFirst) {
			Loca_Data.Reverse ();
			for (int i = 0; i < Loca_Data.Count; i++) {
				O_AwardShow.Add (i, Loca_Data [i]);

			}
			OSorting = O_SortingStatus.ReMoneyFirst;
		} else {
			for (int i = 0; i < Loca_Data.Count; i++) {
				O_AwardShow.Add (i, Loca_Data [i]);

			}
			OSorting = O_SortingStatus.MoneyFirst;
		}
	}

	//------------------------------------------------------------------------------------
}
