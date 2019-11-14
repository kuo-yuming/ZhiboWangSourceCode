using UnityEngine;
using System.Collections;
using GameCore;

public class IceAgePackControl : MonoBehaviour
{
    public static IceAgePackControl Inst;
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
    public bool ScrollColse = false;

    void Awake()
    {
        Inst = this;
    }
    // Use this for initialization
    void Start()
    {
        if (MainConnet.m_dicGameItemList.ContainsKey(GameCore.ENUM_GAME_FRAME.IceAge))
        {
            if (MainConnet.m_dicGameItemList[GameCore.ENUM_GAME_FRAME.IceAge].Count > 0)
            {
                foreach (uint item in MainConnet.m_dicGameItemList[GameCore.ENUM_GAME_FRAME.IceAge])
                {
                    GameObject Data = Instantiate(m_ItemIcon);
                    Data.transform.parent = m_Tabel.transform;
                    Data.transform.localScale = new Vector3(1, 1, 1);
                    IceAgeItemIcon Data_cs = Data.GetComponent<IceAgeItemIcon>();
                    Data_cs.m_ItemID = item;
                    Data_cs.Init();
                }
                m_Tabel.repositionNow = true;
                if (MainConnet.m_dicGameItemList[GameCore.ENUM_GAME_FRAME.IceAge].Count <= 3)
                    ScrollColse = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IceAgeItemUse.IsUseing)
        {
            if (m_Collider.enabled)
            {
                m_Sprite.spriteName = "btn_bag_2";
                m_Collider.enabled = false;
            }
        }
        else
        {
            if (!m_Collider.enabled)
            {
                m_Sprite.spriteName = "btn_bag";
                m_Collider.enabled = true;
            }
        }
    }
    public void BagMainOnClick()
    {
        if (ScrollColse)
        {   //如果道具小於三個 把Scroll關閉
            m_ScrollBar.value = 1.0f;
            MainCollider.enabled = false;
            m_Tabel.enabled = false;
            m_ScrollView.enabled = false;
            m_DragScrollView.enabled = false;
        }
        else MainCollider.enabled = true;   //大於三個 則啟用   
    }
    public void IconClick(uint ID, Vector3 NowPos, string SpriteName)
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
        IceAgeItemUse.FirstPlayerAnim = false;
        GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.Common, (uint)ENUM_COMMON_PACKID_GC.C2G_Item_ReqUseItem,
                      GameConnet.m_oGameClient.DoSerialize<uint>(ID));
        Debug.Log("要求使用道具 : " + ID);
    }
    public void ItemAnimPlayOver()
    {
        TweenPosition Data = m_ItemAnim.GetComponent<TweenPosition>();
        TweenScale Data2 = m_ItemAnim.GetComponent<TweenScale>();
        m_ItemAnim.transform.localPosition = Vector3.zero;
        Data2.ResetToBeginning();
        IceAgeItemUse.FirstPlayerAnim = true;
    }
}
