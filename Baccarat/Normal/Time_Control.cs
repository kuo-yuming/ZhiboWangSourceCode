using UnityEngine;
using System.Collections;
using GameCore.Manager.Baccarat;
using GameEnum;
public class Time_Control : MonoBehaviour {
    public UILabel TimeLabel;
    public UILabel TimeLabel2;

    public static byte MaxTime = 0;
    public static float MinusTime = 0;
    public GameObject TimeGameObject;
    public GameObject FCTimeGameObject;
	// Use this for initialization
	void Start () {
        MinusTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
        TimeLabel.text = (MaxTime - (byte)MinusTime).ToString();
        TimeLabel2.text = (MaxTime - (byte)MinusTime).ToString();

        if ((MaxTime - (byte)MinusTime) > 10)
        {
            TimeLabel.color = new Color32(255, 255, 255, 255);
            TimeLabel2.color = new Color32(255, 255, 255, 255);
            GameSound.TenSence_Bool = false;
        }
        else
        {
            TimeLabel.color = new Color32(255, 61, 61, 255);
            TimeLabel2.color = new Color32(255, 61, 61, 255);
            if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.WaitStop)
            {
                GameSound.TenSence_Bool = true;
            }
            else
            {
                GameSound.TenSence_Bool = false;
            }
        }

        if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.WaitStop)
        {
            TimeGameObject.SetActive(true);
            FCTimeGameObject.SetActive(false);
        }
        else if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.WaitFourCardTime)
        {
            FCTimeGameObject.SetActive(true);
            TimeGameObject.SetActive(false);
        }
        else
        {
             TimeGameObject.SetActive(false);
             FCTimeGameObject.SetActive(false);
        }

        if (MainGame_Control.NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.NewRound || MainGame_Control.NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.ShuffleNewRound || MainGame_Control.NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.WaitBet || MainGame_Control.NowGameState == (byte)ENUM_BACCARAT_TABLE_STATE.StartBid)
        {
            if (MaxTime > MinusTime)
            {
                MinusTime += Time.deltaTime;
            }
            else
            {
                MinusTime = MaxTime;
            }
        }
	}
}
