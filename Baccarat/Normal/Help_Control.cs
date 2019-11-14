using UnityEngine;
using System.Collections;

public class Help_Control : MonoBehaviour {
    public static bool HelpOpen_Bool = false;
    public static byte HelpePage = 1;
    public GameObject HelpObject;
    public UISprite Page_Sprite;
    public UILabel[] PageNumber_Label;
	// Use this for initialization
	void Start () {
        HelpOpen_Bool = false;
        HelpePage = 1;
        HelpObject.SetActive(false);
        if (!GameConnet.GameInfoOpen)
        {
            HelpOpen_Bool = true;
            HelpePage = 1;
            HelpObject.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (HelpOpen_Bool)
        {
            HelpObject.SetActive(true);
        }
        else
        {
            HelpObject.SetActive(false);
        }
        PageVoid();

        if (MainGame_Control.FourCardPlay_Bool)
        {
            PageNumber_Label[1].text = "11";
        }
        else
        {
            PageNumber_Label[1].text = "09";
        }
    }

    void PageVoid()
    {
        switch (HelpePage)
        {
            case 1:
                PageNumber_Label[0].text = "01";
                Page_Sprite.spriteName = "bg_aboutbgl_01";
                break;
            case 2:
                PageNumber_Label[0].text = "02";
                Page_Sprite.spriteName = "bg_aboutbgl_02";
                break;
            case 3:
                PageNumber_Label[0].text = "03";
                Page_Sprite.spriteName = "bg_aboutbgl_03";
                break;
            case 4:
                PageNumber_Label[0].text = "04";
                Page_Sprite.spriteName = "bg_aboutbgl_04";
                break;
            case 5:
                PageNumber_Label[0].text = "05";
                Page_Sprite.spriteName = "bg_aboutbgl_05";
                break;
            case 6:
                PageNumber_Label[0].text = "06";
                Page_Sprite.spriteName = "bg_aboutbgl_06";
                break;
            case 7:
                PageNumber_Label[0].text = "07";
                Page_Sprite.spriteName = "bg_aboutbgl_07";
                break;
            case 8:
                PageNumber_Label[0].text = "08";
                Page_Sprite.spriteName = "bg_aboutbgl_08";
                break;
            case 9:
                PageNumber_Label[0].text = "09";
                Page_Sprite.spriteName = "bg_aboutbgl_09";
                break;
            case 10:
                PageNumber_Label[0].text = "10";
                Page_Sprite.spriteName = "bg_aboutbgl_010";
                break;
            case 11:
                PageNumber_Label[0].text = "11";
                Page_Sprite.spriteName = "bg_aboutbgl_011";
                break;

        }

    }
}
