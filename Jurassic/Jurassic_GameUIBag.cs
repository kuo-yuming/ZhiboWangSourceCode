using UnityEngine;
using System.Collections;
using GameCore;

public class Jurassic_GameUIBag : MonoBehaviour {
	public TweenPosition m_TweenPos;
	public UITable m_Tabel;
	public UIScrollView m_ScrollView;
	public UIDragScrollView m_DragScrollView;
	public BoxCollider MainCollider;
	public GameObject m_ItemIcon;
	public UIScrollBar m_ScrollBar;
	public GameObject m_ItemAnim;
	public UISprite m_Sprite;
	public BoxCollider m_Collider;
	public static bool ItemOnClick = false;
	public bool Btn_Type = false;
	public bool ScrollColse = false;


	static Jurassic_GameUIBag _instance;
	public static Jurassic_GameUIBag instance{get{ return _instance;}}
	// Use this for initialization
	void Start()
	{
		if (_instance == null)
			_instance = gameObject.GetComponent<Jurassic_GameUIBag> ();
		ItemOnClick = false;
		if (MainConnet.m_dicGameItemList.ContainsKey(GameCore.ENUM_GAME_FRAME.Jurassic))
		{
			if (MainConnet.m_dicGameItemList[GameCore.ENUM_GAME_FRAME.Jurassic].Count > 0)
			{
				foreach (uint item in MainConnet.m_dicGameItemList[GameCore.ENUM_GAME_FRAME.Jurassic])
				{
					GameObject Data = Instantiate(m_ItemIcon);
					Data.transform.parent = m_Tabel.transform;
					Data.transform.localScale = new Vector3(1, 1, 1);
					Jurassic_Game_BagIcon Data_cs = Data.GetComponent<Jurassic_Game_BagIcon>();
					Data_cs.m_ItemID = item;
					Data_cs.Init();
				}
				m_Tabel.repositionNow = true;
				//m_ScrollView.UpdatePosition();
				//m_ScrollView.UpdateScrollbars();
				//m_ScrollView.Scroll(0.0f);
				if (MainConnet.m_dicGameItemList[GameCore.ENUM_GAME_FRAME.Jurassic].Count <= 3)
				{
					ScrollColse = true;
					//MainCollider.enabled = false;
					//m_Tabel.enabled = false;
					//m_ScrollView.enabled = false;
					//m_DragScrollView.enabled = false;
				}

			}
		}


	}

	// Update is called once per frame
	void Update()
	{
		//m_Tabel.repositionNow = true;
		if (Jurassic_GameUIItem.IsUseing || /*Fruit_GameControl.IsCombo ||*/ ItemOnClick)
		{
			if (m_Collider.enabled)
			{
				m_Sprite.spriteName = "btn_bag_2";
				m_Collider.enabled = false;
			}

		}
		else
		{
			if(!m_Collider.enabled)
			{
				m_Sprite.spriteName = "btn_bag";
				m_Collider.enabled = true;
			}

		}

	}

	public void CheckClick()
	{
		if (Btn_Type == true) {
			Btn_Type = false;
			m_TweenPos.PlayReverse();
			MainCollider.enabled = false;
		}
	}

	public void BagMainOnClick()
	{

		BetButton.instance.CheckClick ();
		AutoSpin.instance.CheckClick ();
		JurassicInfos.instance.CheckClick ();
		if (!Btn_Type)
		{
			m_TweenPos.PlayForward();

			if (ScrollColse)
			{

				//m_ScrollBar.value = 1.0f;
				MainCollider.enabled = false;
				m_Tabel.enabled = false;
				m_ScrollView.enabled = false;
				m_DragScrollView.enabled = false;
			}
			else
				MainCollider.enabled = true;
		}
		else
		{
			m_TweenPos.PlayReverse();
			MainCollider.enabled = false;
		}

		Btn_Type = !Btn_Type;
	}
	public void IconClick(uint ID,Vector3 NowPos,string SpriteName)
	{
		m_ItemAnim.transform.position = NowPos;
		TweenPosition Data = m_ItemAnim.GetComponent<TweenPosition>();
		TweenScale Data2 = m_ItemAnim.GetComponent<TweenScale>();
		UISprite m_sprite = m_ItemAnim.GetComponent<UISprite>();
		m_sprite.spriteName = SpriteName;
		Data.from = m_ItemAnim.transform.localPosition;
		Data.ResetToBeginning();
		Data.PlayForward();
		Data2.PlayForward();
		BagMainOnClick();
		Jurassic_GameUIItem.FirstPlayerAnim = false;
		GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Common, (uint)ENUM_COMMON_PACKID_GC.C2G_Item_ReqUseItem,
			GameConnet.m_oGameClient.DoSerialize<uint>(ID));
		Debug.Log("要求使用道具 : "+ID);
		ItemOnClick = true;
		Jurassic_GameUIItem.m_ItemUse = true;
		Jurassic_GameUIItem.FirstPlayerAnim = true;
	}
	public void ItemAnimPlayOver()
	{
		TweenPosition Data = m_ItemAnim.GetComponent<TweenPosition>();
		TweenScale Data2 = m_ItemAnim.GetComponent<TweenScale>();
		m_ItemAnim.transform.localPosition = Vector3.zero;
		Data2.ResetToBeginning();
		Jurassic_GameUIItem.FirstPlayerAnim = true;
		UISprite m_sprite = m_ItemAnim.GetComponent<UISprite>();
		Jurassic_GameUIItem.m_SpriteName = m_sprite.spriteName;
	}
}
