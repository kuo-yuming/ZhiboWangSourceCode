#define _VERSION_DEF_IAP_  
using UnityEngine;
using System.Collections;

public class GiftButton_Control : MonoBehaviour
{
#if _VERSION_DEF_IAP_
    public IAPControl m_IAPControl;
    public enum ButtonNumber
    {
        Buy1 = 0,
        Buy2 = 1,
        Buy3 = 2,
        Buy4 = 3,
        Buy5 = 4,
        Buy6 = 5,
        Buy7 = 6,
    }

    public ButtonNumber GiftButton;
    public UISprite GiftButton_Sprite;
    public UILabel GiftButton_Label;

    // Use this for initialization
    void Start()
    {
        GameObject Data = GameObject.FindGameObjectWithTag("Client_MainConnet");
        if (Data == null)
        {

            return;
        }


        m_IAPControl = Data.GetComponent<IAPControl>();


    }

    // Update is called once per frame
    void Update()
    {
        EnumCheck();
    }

    void EnumCheck()
    {
        if (GiftButton == ButtonNumber.Buy1)
        {
            GiftButton_Sprite.spriteName = "icon_nt150";
            if (VersionDef.InternationalLanguageSystem)
            {
                GiftButton_Label.text = Font_Control.Instance.m_dicMsgStr[2504052];
            }
            else
            {
                GiftButton_Label.text = "15000金幣";
            }
        }
        else if (GiftButton == ButtonNumber.Buy2)
        {
            GiftButton_Sprite.spriteName = "icon_nt300";
            if (VersionDef.InternationalLanguageSystem)
            {
                GiftButton_Label.text = Font_Control.Instance.m_dicMsgStr[2504053];
            }
            else
            {
                GiftButton_Label.text = "30000金幣";
            }
        }
        else if (GiftButton == ButtonNumber.Buy3)
        {
            GiftButton_Sprite.spriteName = "icon_nt500";
            if (VersionDef.InternationalLanguageSystem)
            {
                GiftButton_Label.text = Font_Control.Instance.m_dicMsgStr[2504054];
            }
            else
            {
                GiftButton_Label.text = "50000金幣";
            }
        }
        else if (GiftButton == ButtonNumber.Buy4)
        {
            GiftButton_Sprite.spriteName = "icon_nt1000";
            if (VersionDef.InternationalLanguageSystem)
            {
                GiftButton_Label.text = Font_Control.Instance.m_dicMsgStr[2504055];
            }
            else
            {
                GiftButton_Label.text = "100000金幣";
            }
        }
        else if (GiftButton == ButtonNumber.Buy5)
        {
            GiftButton_Sprite.spriteName = "icon_nt2000";
            if (VersionDef.InternationalLanguageSystem)
            {
                GiftButton_Label.text = Font_Control.Instance.m_dicMsgStr[2504056];
            }
            else
            {
                GiftButton_Label.text = "200000金幣";
            }
        }
        else if (GiftButton == ButtonNumber.Buy6)
        {
            GiftButton_Sprite.spriteName = "icon_nt5000";
            if (VersionDef.InternationalLanguageSystem)
            {
                GiftButton_Label.text = Font_Control.Instance.m_dicMsgStr[2504057];
            }
            else
            {
                GiftButton_Label.text = "500000金幣";
            }
        }
    }

    void OnClick()
    {
        if (m_IAPControl == null)
            return;
        if (GiftButton == ButtonNumber.Buy1)
        {
            m_IAPControl.BuyFormalOrder(CompleteProject.UnityAPI.FormalOrder_01);
        }
        else if (GiftButton == ButtonNumber.Buy2)
        {
            m_IAPControl.BuyFormalOrder(CompleteProject.UnityAPI.FormalOrder_02);
        }
        else if (GiftButton == ButtonNumber.Buy3)
        {
            m_IAPControl.BuyFormalOrder(CompleteProject.UnityAPI.FormalOrder_03);
        }
        else if (GiftButton == ButtonNumber.Buy4)
        {
            m_IAPControl.BuyFormalOrder(CompleteProject.UnityAPI.FormalOrder_04);
        }
        else if (GiftButton == ButtonNumber.Buy5)
        {
            m_IAPControl.BuyFormalOrder(CompleteProject.UnityAPI.FormalOrder_05);
        }
        else if (GiftButton == ButtonNumber.Buy6)
        {
            m_IAPControl.BuyFormalOrder(CompleteProject.UnityAPI.FormalOrder_06);
        }
        else if (GiftButton == ButtonNumber.Buy7)
        {
            m_IAPControl.BuyFormalOrder(CompleteProject.UnityAPI.FormalOrder_07);
        }
    }
#endif
}
