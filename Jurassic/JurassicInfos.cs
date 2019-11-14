using UnityEngine;
using System.Collections;

public class JurassicInfos : MonoBehaviour {
	public TweenPosition m_TweenPos;
	public GameObject MyAward;
	public GameObject AllAward;
	static JurassicInfos _instance;
	public static JurassicInfos instance{get{ return _instance;}}
	bool _isClick = false;
	bool isClick {
		get {
			return _isClick;
		}
		set {
			_isClick = value;
			if (_isClick == true)
				m_TweenPos.PlayForward ();
			else
				m_TweenPos.PlayReverse ();
		}
	}
	
	// Update is called once per frame
	void Start()
	{
		if (_instance == null)
			_instance = gameObject.GetComponent<JurassicInfos> ();
	}

	void Update () {
	
	}

	public void CheckClick()
	{
		if (isClick == true)
			isClick = false;
	}

	public void InfoOnClick()
	{
		BetButton.instance.CheckClick ();
		AutoSpin.instance.CheckClick ();
		Jurassic_GameUIBag.instance.CheckClick ();
		isClick = !isClick;
	}

	public void GameInfo()
	{
		JurassicGameInfo.instance.IsShow = true;
		isClick = !isClick;
	}

	public void PlayerAward()
	{
		//AwardManger.instance.SelfAward.SetActive (true);
		MyAward.SetActive(true);
		JurassicManager.M_AwardPacket.m_bEnd = true;
		//m_GameInfo.IsShow = false;
		AllAward.SetActive(false);
		isClick = !isClick;
	}

	public void AllPlayer()
	{
		MyAward.SetActive(false);
		JurassicManager.M_AwardPacket.m_bEnd = true;
		//m_GameInfo.IsShow = false;
		AllAward.SetActive(true);
		//AwardManger.instance.AllfAward.SetActive (true);
		isClick = !isClick;
	}

}
