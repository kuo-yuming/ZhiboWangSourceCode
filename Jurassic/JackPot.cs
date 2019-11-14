using UnityEngine;
using System.Collections;

public class JackPot : MonoBehaviour {
	uint _RewardMoney = 200000;
	UILabel JPlabel;
	static JackPot _instance;
	public static JackPot instance{
		get{
			return _instance;
		}
	}
	uint jpMoney;
	uint RewardMoney{
		get{ return _RewardMoney;}
		set{
			_RewardMoney = value;
			JPlabel.text = _RewardMoney.ToString();
		}
	}
	public static bool isSet = false;

	// Use this for initialization
	void Start () {
		if (_instance == null)
			_instance = gameObject.GetComponent<JackPot> ();
		JPlabel = gameObject.GetComponent<UILabel> ();
		RewardMoney = JurassicManager.m_uiJPMoney;
	}
	
	// Update is called once per frame
	void Update()
	{
		if (!isSet) {
			uint jps = JurassicManager.m_uiJPMoney;
			jpMoney = (uint)((float)jps * ((float)(JurassicUIManager.instance.BetAmount * GameConnet.m_PMachineConfig.m_usBetMoney * GameConnet.m_PMachineConfig.m_byMaxLine) / (float)(GameConnet.m_PMBetMax * GameConnet.m_PMachineConfig.m_usBetMoney * GameConnet.m_PMachineConfig.m_byMaxLine)));
		}
		if (RewardMoney < jpMoney) {
			if (jpMoney - RewardMoney > 10000)
				RewardMoney += 10000;
			else if (jpMoney - RewardMoney > 1000)
				RewardMoney += 1000;
			else if(jpMoney - RewardMoney > 100)
				RewardMoney += 100;
			else
				RewardMoney += 1;
		}
		if (RewardMoney > jpMoney) {
			if ( RewardMoney - jpMoney > 10000)
				RewardMoney -= 10000;
			else if (RewardMoney - jpMoney > 1000)
				RewardMoney -= 1000;
			else if(RewardMoney - jpMoney > 100)
				RewardMoney -= 100;
			else
				RewardMoney -= 1;
		}
	}

	public void SetJP(uint jp)
	{
		isSet = true;
		jpMoney = jp;
	}

	public void ReSetJP()
	{
		isSet = false;
	}
}
