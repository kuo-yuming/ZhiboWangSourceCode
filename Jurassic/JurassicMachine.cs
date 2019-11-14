using UnityEngine;
using System.Collections;
using GameCore.Machine;
using GameCore;
using GameCore.Manager.Jurassic;
using System;

public class JurassicMachine : MonoBehaviour {
	public ushort number = 0;
	public UISprite m_Sprite;
	public UISprite m_Hundred;
	public UISprite m_Ten;
	public UISprite m_One;
	ushort pageAmount = 20;
	public BoxCollider collider;
	public static ushort clickId;
	CPACK_PMachineData Data;
	public static event Action machineInIdle;
	public enum MachineType
	{
		Idle,
		hold,
		Other,
		OtherPlay,
		MyKeep,
	}
	/// <summary>
	/// Gets or sets the machine status.
	/// </summary>
	/// <value>The machine status.</value>
	public MachineType MachineStatus{
		get{ 
			return _MachineStatus;
		}
		set{ 
			switch (value) {
			case MachineType.Idle:
				m_Sprite.spriteName = "常態";
				break;
			case MachineType.hold:
				m_Sprite.spriteName = "感應";
				break;
			case MachineType.MyKeep:
				m_Sprite.spriteName = "自己保留";
				break;
			case MachineType.Other:
				m_Sprite.spriteName = "非本人保留";
				break;
			case MachineType.OtherPlay:
				m_Sprite.spriteName = "非本人入座";
				break;
			}
			_MachineStatus = value;
		}
	}
	private MachineType _MachineStatus = MachineType.Idle;
	/// <summary>
	/// Gets or sets the machine identifier.
	/// </summary>
	/// <value>The machine identifier.</value>
	ushort machineId {
		get {
			return _machineId;
		}
		set	{
			_machineId = value;
			m_Hundred.spriteName = ((((_machineId % 1000) - (_machineId % 100)) / 100) + "");
			m_Ten.spriteName = ((((_machineId % 100) - (_machineId % 10)) / 10) + "");
			m_One.spriteName = ((_machineId % 10) + "");
			}
		}
	ushort _machineId = 0;

	void Awake()
	{
		JurassicLobby.change += change;
		JurassicMachine.machineInIdle += ChekState;
	}

	void OnDestroy()
	{
		JurassicLobby.change -= change;
		JurassicMachine.machineInIdle -= ChekState;
	}

	void change(int page)
	{
		machineId = (ushort)(page * pageAmount + number);
		SetMachineState ();
	}

	void ChekState()
	{
		if (MachineStatus == MachineType.hold)
			MachineStatus = MachineType.Idle;
	}
		
	IEnumerator SetMachineState()
	{
        while (!GameConnet.m_PMachinesData.ContainsKey(machineId))
            yield return null;
		Debug.LogWarning (machineId);
		CPACK_PMachineData Data = GameConnet.m_PMachinesData[machineId];
		switch(Data.m_enumState)
		{
		case ENUM_PMACHINE_STATE.Idle:
			MachineStatus = MachineType.Idle;
			break;
		case ENUM_PMACHINE_STATE.Keep:
			MachineStatus = GameConnet.m_uiKeepMID != machineId ? MachineStatus = MachineType.Other : MachineStatus = MachineType.MyKeep;
			break;
		case ENUM_PMACHINE_STATE.BuyinChk:
			break;
		case ENUM_PMACHINE_STATE.Use:
			MachineStatus = GameConnet.m_uiKeepMID != machineId ? MachineStatus = MachineType.OtherPlay : MachineStatus = MachineType.MyKeep;
			break;
		}
		
	}

	/// <summary>
	/// Raises the click event.
	/// </summary>
	public void OnClick()
	{
		JurassicManager.machineNumber = machineId;
		bool buyin = false;
		switch(MachineStatus)
		{
		case MachineType.hold:
			JurassicManager.BuyInGame(machineId);
			break;
		case MachineType.Idle:
			if (machineInIdle != null)
				machineInIdle ();
			MachineStatus = MachineType.hold;
			GameConnet.m_oGameClient.Send (ENUM_GAME_FRAME.Jurassic, (uint)ENUM_JURASSIC_PACKID_GC.C2G_Machine_GetMachineInfo, GameConnet.m_oGameClient.DoSerialize<uint> (machineId));
			break;
		case MachineType.MyKeep:
			//Debug.LogWarning (clickId+"---"+ machineId);
			if (clickId == machineId) {
				clickId = 0;
				buyin = true;
				JurassicManager.BuyInGame (machineId);
			}
			else {
				GameConnet.m_oGameClient.Send (ENUM_GAME_FRAME.Jurassic, (uint)ENUM_JURASSIC_PACKID_GC.C2G_Machine_GetMachineInfo, GameConnet.m_oGameClient.DoSerialize<uint> (machineId));
			}
			break;
		default:
			GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Jurassic, (uint)ENUM_JURASSIC_PACKID_GC.C2G_Machine_GetMachineInfo, GameConnet.m_oGameClient.DoSerialize<uint>(machineId));
			break;
		}
		if(!buyin)
			clickId = machineId;
	}

}
