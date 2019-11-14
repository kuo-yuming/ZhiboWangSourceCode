using UnityEngine;
using System.Collections;

public class GameOutAndDataNumber : MonoBehaviour {
    public UILabel SmallText_Label;
    public UILabel BigText_Label;

    public static uint SmallMinBet = 0;
    public static uint SmallMaxBet = 0;
    public static uint BigMinBet = 0;
    public static uint BigMaxBet = 0;
    private string Name1 = "押注限制";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (VersionDef.InternationalLanguageSystem)
        {
            Name1 = Font_Control.Instance.m_dicMsgStr[2008091];
        }
        else
        {
            Name1 = "押注限制";
        }

        if (SmallMinBet < 1000)
        {
            if (SmallMaxBet < 1000)
            {
                SmallText_Label.text = Name1 + SmallMinBet.ToString() + "~" + SmallMaxBet.ToString();
            }
            else
            {
                SmallText_Label.text = Name1 + SmallMinBet.ToString() + "~" + (SmallMaxBet / 1000).ToString() + "k";
            }
        }
        else
        {
            SmallText_Label.text = Name1 + (SmallMinBet / 1000).ToString() + "k" + "~" + (SmallMaxBet / 1000).ToString() + "k";
        }

        if (BigMinBet < 1000)
        {
            if (SmallMaxBet < 1000)
            {
                BigText_Label.text = Name1 + BigMinBet.ToString() + "~" + BigMaxBet.ToString();
            }
            else
            {
                BigText_Label.text = Name1 + BigMinBet.ToString() + "~" + (BigMaxBet / 1000).ToString() + "k";
            }
        }
        else
        {
            BigText_Label.text = Name1 + (BigMinBet / 1000).ToString() + "k" + "~" + (BigMaxBet / 1000).ToString() + "k";
        }

       
	}

    void OnClick()
    {
        //Application.ExternalCall("CloseWindow", Connet.WebID);
    }
}
