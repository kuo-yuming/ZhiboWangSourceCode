using UnityEngine;
using System.Collections;

public class IceAgeItemIcon : MonoBehaviour
{
    IceAgePackControl m_Target;
    public UIButton m_Btn;
    public UISprite m_Sprite;
    public UILabel m_Label;
    public BoxCollider m_Collider;
    public uint m_ItemID = 0;
    byte ItemCnt = 0;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (MainConnet.m_dicPlayerItemIndex.ContainsKey(m_ItemID))
        {
            ItemCnt = MainConnet.m_dicPlayerItemIndex[m_ItemID];
        }
        if (ItemCnt <= 0)
        {
            m_Btn.isEnabled = false;
        }
        else
        {
            m_Btn.isEnabled = true;
        }

        if (m_Btn.isEnabled)
        {
            m_Label.text = ItemCnt + "";
        }
        else
        {
            m_Label.text = "0";
        }


    }
    public void Init()
    {
        m_Target = GetComponentInParent<IceAgePackControl>();
        if (MainConnet.m_dicItemDatas.ContainsKey(m_ItemID))
        {
            m_Sprite.spriteName = MainConnet.m_dicItemDatas[m_ItemID].m_strIconName;
            m_Btn.normalSprite = MainConnet.m_dicItemDatas[m_ItemID].m_strIconName;
            m_Btn.disabledSprite = MainConnet.m_dicItemDatas[m_ItemID].m_strIconName + "_D";
        }

    }
    void OnClick()
    {
        IceAgeButtonControl.Inst.StatusChange(IceAgeButtonControl.ButtonStatus.Pack);
        IceAgePackControl.Inst.m_Collider.enabled = false;
        if (m_Btn.isEnabled && ItemCnt != 0)
            m_Target.IconClick(m_ItemID, this.transform.position, MainConnet.m_dicItemDatas[m_ItemID].m_strIconName);
    }
}
