using UnityEngine;
using System.Collections;

public class Jurassic_GameUIItem : MonoBehaviour {
	public TweenAlpha m_BG;
	public TweenAlpha m_Label;
	public TweenAlpha m_SpTwA;
	public UISprite m_Sp;
	public GameObject m_ItemAnim;
	public static string m_SpriteName = "";
	public static bool FirstPlayerAnim = false;
	public static bool m_ItemUse = false;
	public static bool m_ItemClose = false;
	public static bool IsUseing = false;
	// Use this for initialization

	static Jurassic_GameUIItem _instance;
	public static Jurassic_GameUIItem instance{get{return _instance;}}



	void Start()
	{
		if (_instance == null)
			_instance = gameObject.GetComponent<Jurassic_GameUIItem> ();
		print(transform.name);
		m_ItemUse = false;
		m_ItemClose = false;
		FirstPlayerAnim = false;
		IsUseing = false;
	}

	// Update is called once per frame
//	void Update()
//	{
//		if (m_ItemUse && FirstPlayerAnim)
//		{
//			m_ItemUse = false;
//			FirstPlayerAnim = false;
//			IsUseing = true;
//			m_Sp.spriteName = m_SpriteName;
//			m_ItemAnim.SetActive(true);
//		}
//		if (m_ItemClose)
//		{
//			m_ItemClose = false;
//			IsUseing = false;
//			if (m_ItemAnim.activeSelf)
//				m_ItemAnim.SetActive(false);
//		}
//	}

	public void PlayItem()
	{
		m_ItemUse = false;
		FirstPlayerAnim = false;
		IsUseing = true;
		m_Sp.spriteName = m_SpriteName;
		m_ItemAnim.SetActive(true);
	}

	public void CloseItem()
	{
		m_ItemClose = false;
		IsUseing = false;
		if (m_ItemAnim.activeSelf)
			m_ItemAnim.SetActive(false);
	}



}
