using UnityEngine;
using System.Collections;

public class BJHelp : MonoBehaviour {

    public static bool BarOpen_Bool = false;
    public static bool BJHelpOpen_Bool = false;
    public static byte BJHelpPageNumber = 1;
    public GameObject BJHelpObject;
    public UISprite BJHelp_Sprite;
    public UILabel BJHelp_Label;

    public static byte InfoStaut = 0;
    public TweenPosition InfoTweenPosition;
    // Use this for initialization
    void Start () {
        BarOpen_Bool = false;
        BJHelpOpen_Bool = false;
        BJHelpObject.SetActive(false);
        BJHelpPageNumber = 1;
        InfoStaut = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (BJHelpOpen_Bool)
        {
            BJHelpObject.SetActive(true);
        }
        else
        {
            BJHelpObject.SetActive(false);
        }

        if (BarOpen_Bool && InfoStaut == 0)
        {
            InfoTweenPosition.PlayForward();
        }
        else if (!BarOpen_Bool && InfoStaut == 1)
        {
            InfoTweenPosition.PlayReverse();
        }

        BJHelp_Label.text = "0" + BJHelpPageNumber.ToString();
        BJHelp_Sprite.spriteName = "bg_aboutbgl_0" + BJHelpPageNumber.ToString();


    }

    public void InfoChangeVoid()
    {
        if (BarOpen_Bool)
        {
            InfoStaut = 1;
        }
        else if (!BarOpen_Bool)
        {
            InfoStaut = 0;
        }
        MainGame_Control.AutoAndInfoClickBool = false;
    }
}
