using UnityEngine;
using System.Collections;

public class SetMachineId : MonoBehaviour {
	public UILabel Machine;
	// Use this for initialization
	void Start () {
		Machine.text = GameConnet.m_NowBuyInMachineID.ToString ("000");
	}
}
