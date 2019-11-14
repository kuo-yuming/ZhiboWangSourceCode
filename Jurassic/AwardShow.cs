using UnityEngine;
using System.Collections;

public class AwardShow : MonoBehaviour {

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


}
