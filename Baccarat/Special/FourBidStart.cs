using UnityEngine;
using System.Collections;
using GameCore.Manager.Baccarat;
using GameEnum;

public class FourBidStart : MonoBehaviour {
    public static bool FourBidStart_Bool = false;
    public GameObject ShowGameObject;
    public UISprite Background;
    public UISprite WhiteBackground;
    public UISprite Writing;
    public TweenPosition Hammer_TP;
    public UISprite Light;
    public TweenScale Light_Scale;
    public TweenColor Light_Color;
    byte ShowStart = 0;
    bool ShowSence = false;
    bool ShowOver = false;
    bool ShowColor = false;
    bool StateCheck_Bool = false;
    // Use this for initialization
    void Start () {
        FourBidStart_Bool = false;
        ShowStart = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (FourBidStart_Bool)
        {
            ShowGameObject.SetActive(true);
            if (ShowStart == 0)
            {
                Background.spriteName = "bg_fback1";
                WhiteBackground.enabled = true;
                Writing.enabled = false;
                Light.enabled = false;
                Hammer_TP.ResetToBeginning();
                Hammer_TP.PlayForward();
                ShowStart = 1;
            }
            else if (ShowStart == 2)
            {
                Background.spriteName = "bg_fback2";
                WhiteBackground.enabled = false;
                Writing.enabled = true;
                Light.enabled = true;
                Light_Scale.ResetToBeginning();
                Light_Scale.PlayForward();
                ShowStart = 3;
            }
            else if (ShowStart == 4)
            {
                Light_Color.ResetToBeginning();
                Light_Color.PlayForward();
                ShowStart = 5;
            }

            if (StateCheck_Bool)
            {
                if (MainGame_Control.NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.StartBid)
                {
                    MainGame_Control.StopModeState = ENUM_STOPMODE_STATE.WaitFourCardTime;
                    FourCard_Control.FourCardObjectOpen = true;
                    FourCard_Control.FourCardStartShowBool = true;
                    FourBidStart_Bool = false;
                }
                else if (MainGame_Control.NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.StopBid)
                {
                    MainGame_Control.StopModeState = ENUM_STOPMODE_STATE.FourCardShow;
                    GameSound.BetStop_Bool = true;
                    FourCard_Control.FourCardObjectOpen = true;
                    FourCard_Control.FourCardStartShowBool = true;
                    FourBidStart_Bool = false;
                }
            }
        }
        else
        {
            ShowGameObject.SetActive(false);
            Background.spriteName = "bg_fback1";
            WhiteBackground.enabled = true;
            Writing.enabled = false;
            Light.enabled = false;
            ShowStart = 0;
            StateCheck_Bool = false;
        }
    }

    public void HammerOver()
    {
        ShowStart = 2;
        GameSound.TapStart_Bool = true;
    }

    public void StateCheck()
    {
        ShowStart = 4;
        GameSound.BidStart_Bool = true;
    }

    public void LightColor()
    {
        Light_Scale.ResetToBeginning();
        Light_Color.ResetToBeginning();
        Light.enabled = false;
        StateCheck_Bool = true;
    }
}
