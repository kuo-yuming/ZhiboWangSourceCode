using UnityEngine;
using System.Collections;

public class IceAgeItemUse : MonoBehaviour
{
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
    void Start()
    {
        m_ItemUse = false;
        m_ItemClose = false;
        FirstPlayerAnim = false;
        IsUseing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_ItemUse && FirstPlayerAnim)
        {
            m_ItemUse = false;
            FirstPlayerAnim = false;
            IsUseing = true;
            m_Sp.spriteName = m_SpriteName;
            m_ItemAnim.SetActive(true);
        }
        if (m_ItemClose)
        {
            m_ItemClose = false;
            IsUseing = false;
            if (m_ItemAnim.activeSelf)
                m_ItemAnim.SetActive(false);
        }
    }
}
