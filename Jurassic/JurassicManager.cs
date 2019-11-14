using UnityEngine;
using System.Collections;
using System;
using GameCore;
using GameCore.Machine;
using GameCore.Manager.Jurassic;
using System.Collections.Generic;


public class JurassicManager : MonoBehaviour {
	private static Dictionary<int, bool> PageCheck = new Dictionary<int, bool>();   //確認有無要過資料

	public static CPACK_Jurassic_MachineInfo m_MachineInfo = new CPACK_Jurassic_MachineInfo();
	public static List<CPACK_PMachineAwardRecord> M_AwardRecord = new List<CPACK_PMachineAwardRecord>();
	public static CPACK_PMachineAwardRecordList M_AwardPacket = new CPACK_PMachineAwardRecordList();
	public static List<CPACK_PMachineAwardRecord> O_AwardRecord = new List<CPACK_PMachineAwardRecord>();
	public static CPACK_PMachineAwardRecordList O_AwardPacket = new CPACK_PMachineAwardRecordList();
	public static PK5_2Manager.O_AwardGetData O_AwardStatus = PK5_2Manager.O_AwardGetData.Idle;
	public static CPACK_Jurassic_GameConfig Gameconfig = null;
	public static CPACK_Jurassic_BetResult m_BetReRack = null;

	public static int page = 0;
	public static int sumPage = 3;
	public static uint m_uiJPMoney = 0;
	public static bool isInit = false;
	public static bool M_AwardU2C = false;
	public static bool m_boGetBetRe = false;
	public static int machineNumber = 0;


	public void OnJurassicData(uint uipackId, byte[] byarData)
	{
		Debug.Log(string.Format("OnRcvJurassicFrameData. PackID={0}", uipackId));

		switch (uipackId)
		{
		case (uint)ENUM_COMMON_PACKID_GC.G2C_PMachine_NotifyGameConfig:
			RcvPMGameConfig(byarData);
			break;
		case (uint)ENUM_JURASSIC_PACKID_GC.G2C_NotifyGameConfig:
			RcvGameConfig(byarData);
			break;
		case (uint)ENUM_COMMON_PACKID_GC.G2C_PMachine_NotifyKeepMID:
			RcvKeepMachine(byarData);
			break;
		case (uint)ENUM_COMMON_PACKID_GC.G2C_PMachine_NotifyMachineList:
			RcvPMachinesData(byarData);
			break;
		case (uint)ENUM_COMMON_PACKID_GC.G2C_PMachine_NotifyMachineData:
			RcvOnePMachineData(byarData);
			break;
		case (uint)ENUM_JURASSIC_PACKID_GC.G2C_Machine_NotifyMachineInfo:
			RcvMachineInfo(byarData);
			break;
		case (uint)ENUM_COMMON_PACKID_GC.G2C_PMachine_AutoSelectResult:
			RcvRadomMachine(byarData);
			break;
		case (uint)ENUM_COMMON_PACKID_GC.G2C_PMachine_NotifyStartGame:
			RcvBuyInOk(byarData);
			break;
		case (uint)ENUM_JURASSIC_PACKID_GC.G2C_Bet_RplyBetResult:
			RcvBetAward(byarData);
			break;
//		case (uint)ENUM_JURASSIC_PACKID_GC.G2C_Machine_UpdateCherryBell:
//			RcvBonusConfig(byarData);
//			break;
		case (uint)ENUM_JURASSIC_PACKID_GC.G2C_Game_UpdateJPVal:
			RcvJPMoney(byarData);
			break;
		case (uint)ENUM_COMMON_PACKID_GC.G2C_PMachine_RplyPersonalAwardRec:
			RcvM_AwardRecord(byarData);
			break;
		case (uint)ENUM_COMMON_PACKID_GC.G2C_PMachine_RplyLobbyAwardRec:
			RcvO_AwardRecord(byarData);
			break;                
		default:
			Debug.Log(string.Format("{0}:Unknown packid={1}", DateTime.Now, uipackId));
			break;
		}
		Debug.Log(string.Format("OnRcvJurassicFrameData. PackID={0} end", uipackId));
	}

	public static void BuyInGame(uint machineID)
	{
		if (MainConnet.m_PlayerData.m_ui64OwnMoney >= GameConnet.m_PMachineConfig.m_uiMinBuyinMoney)
		{
			AllScenceLoad.LoadScence = true;
			ulong BuyInMoney = MainConnet.m_PlayerData.m_ui64OwnMoney;
			if (BuyInMoney > GameConnet.m_PMachineConfig.m_uiMaxBuyinMoney)
				BuyInMoney = GameConnet.m_PMachineConfig.m_uiMaxBuyinMoney;
			CPACK_PMachineBuyin m_BuyInMoney = new CPACK_PMachineBuyin();
			m_BuyInMoney.m_uiMID = machineID;
			m_BuyInMoney.m_uiBuyinMoney = (uint)BuyInMoney;
			GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Jurassic, (uint)ENUM_COMMON_PACKID_GC.C2G_PMachine_ReqBuyin, GameConnet.m_oGameClient.DoSerialize<CPACK_PMachineBuyin>(m_BuyInMoney));
		}
		else
		{
			Message_Control.OpenMessage = true;
			Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
			Message_Control.MessageStatus = Message_Control.MessageStatu.Buyin_MoneyNotEnough;
		}
	}

	void RcvPMGameConfig(byte[] byarData)
	{
		GameConnet.m_PMachineConfig = GameConnet.m_oGameClient.DoDeSerialize<CPACK_PMGameConfig>(byarData);
		Debug.Log(string.Format("{0}:收到機台設定.最大機台數 = {1}, 每頁機台數={2}, BuyIn最大金額={3}, BuyIn最小金額={4}, BetMoney={5}", DateTime.Now
			, GameConnet.m_PMachineConfig.m_uiMaxMachineCnt, GameConnet.m_PMachineConfig.m_uiPageMachineCnt, GameConnet.m_PMachineConfig.m_uiMaxBuyinMoney, GameConnet.m_PMachineConfig.m_uiMinBuyinMoney, GameConnet.m_PMachineConfig.m_usBetMoney));
		//Reseat();
		MachinePageCheck();
		GetMachineDataU2G();
		GetM_AwardRecord();
		isInit = true;
	}

	public void MachinePageCheck()
	{
		PageCheck.Clear();
		if ((GameConnet.m_PMachineConfig.m_uiMaxMachineCnt % GameConnet.m_PMachineConfig.m_uiPageMachineCnt) != 0)
		{
			for (int i = 0; i <= (GameConnet.m_PMachineConfig.m_uiMaxMachineCnt / GameConnet.m_PMachineConfig.m_uiPageMachineCnt); i++)
			{
				PageCheck.Add(i, false);
			}
		}
		else
		{
			for (int i = 0; i < (GameConnet.m_PMachineConfig.m_uiMaxMachineCnt / GameConnet.m_PMachineConfig.m_uiPageMachineCnt); i++)
			{
				PageCheck.Add(i, false);
			}

		}
		sumPage = PageCheck.Count - 1;
	}

	public static void GetMachineDataU2G()
	{

		CPACK_GetPMachineList StartMachine_U2G = new CPACK_GetPMachineList();
		uint OnePageMax = GameConnet.m_PMachineConfig.m_uiPageMachineCnt;
		Debug.Log("當頁最大機台數 : " + OnePageMax);

		if (!PageCheck[page])
		{
			Debug.Log("要求第" + page + "頁機台資料");
			StartMachine_U2G.m_uiStartMID = (uint)((page * OnePageMax) + 1);
			StartMachine_U2G.m_uiEndMID = (uint)((page * OnePageMax) + OnePageMax);
			PageCheck[page] = true;
			GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Jurassic, (uint)ENUM_COMMON_PACKID_GC.C2G_PMachine_GetMachineList,
			GameConnet.m_oGameClient.DoSerialize<CPACK_GetPMachineList>(StartMachine_U2G));
		}
		else
		{
			Debug.Log("已有機台資料");
			return;
		}
	}

	public void GetM_AwardRecord()
	{
		if (!M_AwardU2C)
			GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Jurassic, (uint)ENUM_COMMON_PACKID_GC.C2G_PMachine_ReqPersonalAwardRec,null);
	}

	public void RcvKeepMachine(byte[] byarData)
	{
		GameConnet.m_uiKeepMID = GameConnet.m_oGameClient.DoDeSerialize<uint>(byarData);
		Debug.Log(string.Format("{0}:收到玩家保留機台, Keep MachineID={1}", DateTime.Now, GameConnet.m_uiKeepMID));

	}

	void RcvGameConfig(byte[] byarData)
	{
		Gameconfig = GameConnet.m_oGameClient.DoDeSerialize<CPACK_Jurassic_GameConfig>(byarData);
//		Debug.Log(string.Format("{0}:收到遊戲設定.雙櫻桃彩金初始值 = {1}, 三銅鐘彩金初始值={2}", DateTime.Now
//			, Gameconfig.m_uiDoubleCherryMoney, Gameconfig.m_uiThreeBellMoney));
	}

	public void RcvPMachinesData(byte[] byarData)
	{
		Debug.Log(DateTime.Now + "  Get MachineData...........");
		CPACK_PMachineDataList m_LocalMachineDatas = GameConnet.m_oGameClient.DoDeSerialize<CPACK_PMachineDataList>(byarData);
		foreach (CPACK_PMachineData Datas in m_LocalMachineDatas.m_listMachineData)
		{
			if (!GameConnet.m_PMachinesData.ContainsKey(Datas.m_uiMID))
			{
				GameConnet.m_PMachinesData.Add(Datas.m_uiMID, Datas);
				Debug.Log("Mach : " + GameConnet.m_PMachinesData[Datas.m_uiMID].m_uiMID + "  DBID : " + GameConnet.m_PMachinesData[Datas.m_uiMID].m_uiDBID);
			}
			else
			{
				GameConnet.m_PMachinesData[Datas.m_uiMID] = Datas;
			}
			if (GameConnet.m_PMachinesData[Datas.m_uiMID].m_uiDBID == MainConnet.m_PlayerData.m_uiDBID)
			{
				GameConnet.m_uiKeepMID = GameConnet.m_PMachinesData[Datas.m_uiMID].m_uiMID;
			}
		}
	}

	public void RcvOnePMachineData(byte[] byarData)
	{
		CPACK_PMachineData OneMachineData = GameConnet.m_oGameClient.DoDeSerialize<CPACK_PMachineData>(byarData);
		//uint OnePageMax = m_MachineConfig.m_uiPageMachineCnt;
		if (!GameConnet.m_PMachinesData.ContainsKey(OneMachineData.m_uiMID))
		{
			GameConnet.m_PMachinesData.Add(OneMachineData.m_uiMID, OneMachineData);
			Debug.Log("Mach : " + GameConnet.m_PMachinesData[OneMachineData.m_uiMID].m_uiMID + "  DBID : " + GameConnet.m_PMachinesData[OneMachineData.m_uiMID].m_uiDBID
				+ "機台狀態 : " + GameConnet.m_PMachinesData[OneMachineData.m_uiMID].m_enumState);
		}
		else
		{
			GameConnet.m_PMachinesData[OneMachineData.m_uiMID] = OneMachineData;
			Debug.Log("Mach : " + GameConnet.m_PMachinesData[OneMachineData.m_uiMID].m_uiMID + "  DBID : " + GameConnet.m_PMachinesData[OneMachineData.m_uiMID].m_uiDBID);
		}

		if (GameConnet.m_PMachinesData[OneMachineData.m_uiMID].m_uiDBID == MainConnet.m_PlayerData.m_uiDBID && MainConnet.m_PlayerData.m_byVIPType != 0)
		{
			GameConnet.m_uiKeepMID = GameConnet.m_PMachinesData[OneMachineData.m_uiMID].m_uiMID;
		}
		if (GameConnet.m_PMachinesData[OneMachineData.m_uiMID].m_uiMID == GameConnet.m_uiKeepMID && GameConnet.m_PMachinesData[OneMachineData.m_uiMID].m_enumState == ENUM_PMACHINE_STATE.Idle)
		{
			GameConnet.m_uiKeepMID = 0;

		}

	}

	public void RcvMachineInfo(byte[] byarData)
	{
		m_MachineInfo = GameConnet.m_oGameClient.DoDeSerialize<CPACK_Jurassic_MachineInfo>(byarData);
		Debug.Log("GetMachineInfo");
	}

	public void RcvRadomMachine(byte[] byarData)
	{
		Debug.Log(DateTime.Now + "Get RadomMachineID...................G2U");
		CPACK_PMachineAutoSelectResult RadomPack = GameConnet.m_oGameClient.DoDeSerialize<CPACK_PMachineAutoSelectResult>(byarData);
		if (RadomPack.m_iResultCode == 0)
		{
			BuyInGame(RadomPack.m_uiMID);
			Debug.Log("Get RadomMachineID...................ID : " + RadomPack.m_uiMID);
		}
		else if (RadomPack.m_iResultCode == 40)
		{
			if (GameConnet.m_uiKeepMID != 0)
			{
				Message_Control.OpenMessage = true;
				Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
				Message_Control.MessageStatus = Message_Control.MessageStatu.Machine_NoneIdleMachineBacKToKeep;
			}
			else
			{
				Debug.Log(string.Format("{0}:無可用機台", DateTime.Now));
				Message_Control.OpenMessage = true;
				Message_Control.MessageSize = Message_Control.BoxSizeStatu.Box_S_Yes;
				Message_Control.MessageStatus = Message_Control.MessageStatu.Machine_NoneIdleMachine;
			}
		}
		Debug.Log("Get RadomMachineID...................END");

	}
		
	public void RcvBuyInOk(byte[] byarData)
	{
		Debug.Log(DateTime.Now + "BuyInGameOK");
		GameConnet.m_PMachineBuyInGameData = GameConnet.m_oGameClient.DoDeSerialize<CPACK_PMachineEnter>(byarData);
		GameConnet.m_BuyInMoney = GameConnet.m_PMachineBuyInGameData.m_uiGameMoney;
		GameConnet.m_NowBuyInMachineID = GameConnet.m_PMachineBuyInGameData.m_uiMID;
		GameConnet.LogIn_GameSuccess = true;
	}
 
	public void RcvBetAward(byte[] byarData)
	{
		Debug.Log(DateTime.Now + "GetBetAward");
		m_BetReRack = GameConnet.m_oGameClient.DoDeSerialize<CPACK_Jurassic_BetResult>(byarData);
		for (int i = 0; i < m_BetReRack.m_byarGridSymbol.Length; i++)
		{
			Debug.Log(SymbolIDToString(m_BetReRack.m_byarGridSymbol[i]));
		}
	MathAward();

		m_boGetBetRe = true;

	}

	public void RcvJPMoney(byte[] byarData)
	{
		m_uiJPMoney = GameConnet.m_oGameClient.DoDeSerialize<uint>(byarData);
		//Fruit_GameUI.JPChange = true;
	}

	public void RcvM_AwardRecord(byte[] byarData)
	{
		if (!M_AwardU2C)
		{
			M_AwardRecord.Clear();
		}
		M_AwardU2C = true;
		Debug.Log(DateTime.Now + " ; Get M_AwardRecord...........");
		M_AwardPacket = GameConnet.m_oGameClient.DoDeSerialize<CPACK_PMachineAwardRecordList>(byarData);
		foreach (CPACK_PMachineAwardRecord item in M_AwardPacket.m_listAwardRec)
		{
			M_AwardRecord.Add(item);
		}
		Debug.Log("M_AwardRecord..........." + M_AwardPacket.m_listAwardRec.Count);
		if (M_AwardPacket.m_bEnd)
		{
			Debug.Log(DateTime.Now + " : Get M_AwardRecord...........END");
		}

	}

	public void RcvO_AwardRecord(byte[] byarData)
	{
		if (O_AwardStatus == PK5_2Manager.O_AwardGetData.Idle)
		{
			O_AwardRecord.Clear();
			O_AwardStatus = PK5_2Manager.O_AwardGetData.GetingData;
		}
		Debug.Log(DateTime.Now + " : Get O_AwardRecord...........");
		O_AwardPacket = GameConnet.m_oGameClient.DoDeSerialize<CPACK_PMachineAwardRecordList>(byarData);
		//O_AwardRecord.Clear ();
		foreach (CPACK_PMachineAwardRecord item in O_AwardPacket.m_listAwardRec)
		{
			O_AwardRecord.Add(item);
		}
		Debug.Log("O_AwardRecord..........." + O_AwardPacket.m_listAwardRec.Count);
		if (O_AwardPacket.m_bEnd)
		{
			Debug.Log(DateTime.Now + " : Get O_AwardRecord...........END");
			O_AwardStatus = PK5_2Manager.O_AwardGetData.GetDataEnd;
		}

	}

	private string SymbolIDToString(byte bySymbolID)
	{
		switch (bySymbolID)
		{
		case 1:
			return "荔枝";
		case 2:
			return "葡萄";
		case 3:
			return "柳丁";
		case 4:
			return "鳳梨";
		case 5:
			return "西瓜";
		case 6:
			return "翼龍";
		case 7:
			return "三角龍";
		case 8:
			return "暴龍";
		case 9:
			return "鑽石";
		case 10:
			return "WILD";
		default:
			return "???";
		}
	}

	void MathAward()
	{
		List<int> Lines = new List<int> {0,0,0,0,0,0,0,0,0,0};
		List<int> symbol = new List<int> ();
		List<int> comboline = new List<int> {0,4,7,10,14};
		int comboi = 0;
		int remainder = 0;

		for (int i = 0; i < Lines.Count; i++){
			if (m_BetReRack.m_dicLineAward.ContainsKey ((byte)(i + 1))) {
				int dividend = (int)m_BetReRack.m_dicLineAward[(byte)(i + 1)];
				if (dividend <= 32) {
					int quotient = Math.DivRem (dividend, 8, out remainder);
					quotient++;
					if (remainder != 0)
						quotient++;
					Lines [i] = quotient;
				} else if (dividend == 33) {
					Lines [i] = 5;
					SlotManager.slotdata.JackPot = true;
				} else {
					Debug.LogWarning ("TestLog");
//					int quotient = Math.DivRem (dividend,248,out remainder);
//					Math.DivRem (remainder, 4, out remainder);
//					remainder += 2;
//					Lines [i] = remainder;
				}
			}
		}

		foreach (var sym in m_BetReRack.m_byarGridSymbol) {
			if ((int)sym == 0)
				symbol.Add (1);
			else
				symbol.Add ((int)sym);
		}

		foreach (var obj in comboline) {
			if (symbol [obj] >= 6 && symbol [obj] <= 8)
				comboi++;
			else
				break;
		}

		SlotManager.slotdata.isUse = m_BetReRack.m_uiUseItemID > (uint)0;
		SlotManager.playerMoney = m_BetReRack.m_ui64GameMoney;
		//SlotManager.RewardData = m_BetReRack;
		SlotManager.slotdata.SetData (Lines,symbol,m_BetReRack.m_byFreeRoundCnt);
		SlotManager.RewardMoney = (int)m_BetReRack.m_uiScore;
		SlotManager.slotdata.SetExcited (comboi == 4,m_BetReRack.m_bComboMode);
		SlotManager.slotdata.UseItem = (int)m_BetReRack.m_uiUseItemID > 0;
		//BugUse.AddMessage ("GET Package");
	}

}
