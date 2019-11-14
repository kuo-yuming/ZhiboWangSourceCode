using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Jurassic_WinMoney : MonoBehaviour {
	public bool StartPlay = false;
	public long WinMoney = 1000;
	public GameObject m_NumberParent;
	public GameObject m_NumberPrefeb;
	public UIGrid m_NumberCheck;
	float DelayTimeIndex = 0.3f;
	float NumberDelayTime = 0.05f;
	public bool PlayOver = false;
	public bool IsPlaying = false;
	public bool Reseat = false;
	public Animation_Player m_WinWord;
	bool PlayingCheck = false;
	List<GameObject> m_Number = new List<GameObject>();
	Jurassic_WinAnimNumber m_FinalNumber;
	public TweenAlpha m_TwAlpha;
	public TweenPosition m_TwPos;
	public TweenScale m_TwScale;
	// Use this for initialization
	static Jurassic_WinMoney _instance;
	public static Jurassic_WinMoney instance{
		get{
			return _instance;
		}
	}
	void Start () {
		_instance = this.gameObject.GetComponent<Jurassic_WinMoney> ();
	}

	// Update is called once per frame
	void Update () {
		if (StartPlay)
		{
			m_TwPos.ResetToBeginning();
			m_TwAlpha.ResetToBeginning();
			m_TwScale.ResetToBeginning();
			StartPlay = false;
			m_WinWord.StartPlay = true;
			m_WinWord.Show = true;
			string DataNumber = WinMoney.ToString();
			foreach (var item in DataNumber)
			{
				GameObject Data = Instantiate(m_NumberPrefeb);
				Data.transform.parent = m_NumberParent.transform;
				Data.transform.localScale = new Vector3(1, 1, 1);
				Jurassic_WinAnimNumber Data_cs = Data.GetComponent<Jurassic_WinAnimNumber>();
				Data_cs.AnimDelay = DelayTimeIndex;
				//Data_cs.enabled = true;
				DelayTimeIndex += NumberDelayTime;
				Data_cs.Final = int.Parse(item.ToString());
				m_Number.Add(Data);
			}
			m_FinalNumber = m_Number[m_Number.Count - 1].GetComponent<Jurassic_WinAnimNumber>();
			PlayingCheck = true;
			m_NumberCheck.repositionNow = true;
		}
		if (PlayingCheck)
		{
			if (m_FinalNumber.Over && m_WinWord.OneceOver)
			{
				m_WinWord.OneceOver = false;
				//m_TwPos.ResetToBeginning();
				//m_TwAlpha.ResetToBeginning();
				m_TwPos.PlayForward();
				m_TwAlpha.PlayForward();
				m_TwScale.PlayForward();
			}
		}
		if (Reseat)
		{
			Reseat = false;
			m_WinWord.StartPlay = false;
			m_WinWord.Reseat();
			m_WinWord.OneceOver = false;
			m_WinWord.Show = false;
			IsPlaying = false;
			PlayOver = false;
			DelayTimeIndex = 0.1f;
			int Data = m_Number.Count;
			for (int i = 0; i < Data; i++)
			{
				Destroy(m_Number[0]);
				m_Number.RemoveAt(0);
			}
			m_Number.Clear();

		}

	}
	public void PlayAllOver()
	{
		if (Fruit_GameControl.PlayerClickWin)
		{

			return;
		}
		PlayOver = true;

	}
}
