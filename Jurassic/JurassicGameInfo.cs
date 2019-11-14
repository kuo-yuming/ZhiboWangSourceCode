using UnityEngine;
using System.Collections;

public class JurassicGameInfo : MonoBehaviour {

	// Use this for initialization
	private bool _IsShow = false;
	public UISprite InfoSprite;
	public GameObject m_GameInfoObj;
	int _NowPage = 1;
	int MaxPage = 6;
	private static JurassicGameInfo _instance;

	public static JurassicGameInfo instance
	{
		get{

			return _instance;
		}
	}

	int NowPage{
		get{
			return _NowPage;
		}
		set{
			_NowPage = value;
			InfoSprite.spriteName = "bg_aboutB_0" + _NowPage.ToString();
		}
	}

	public bool IsShow{
		get{
			return _IsShow;
		}
		set{
			_IsShow = value;
			m_GameInfoObj.SetActive (_IsShow);
		}
	}

	void Start()
	{
		if (_instance == null)
			_instance = this.gameObject.GetComponent<JurassicGameInfo> ();
	}

	void NextPage()
	{
		NowPage = NowPage == 6 ? 1 : NowPage + 1;
	}

	void BackPage()
	{
		NowPage = NowPage == 1 ? 6 : NowPage - 1;
	}

	void Close()
	{
		IsShow = false;
		NowPage = 1;
	}

}
