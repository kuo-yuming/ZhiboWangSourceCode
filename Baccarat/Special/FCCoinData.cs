using UnityEngine;
using System.Collections;
using GameCore.Manager.Baccarat;
using GameEnum;

public class FCCoinData : MonoBehaviour {

    public TweenPosition FCCoinTweenPosition;
    public UITable FCBetTable;
    public bool FCMoveBool = true;
    public ENUM_BACCARAT_AWARD_AREA TableArea;

    //位子設定
    private Vector3 FCStartAndWinVector3 = new Vector3(0, -280, 0);//贏
    private Vector3 FCLoseVector3 = new Vector3(0, 470, 0);//輸

	// Use this for initialization
	void Start () {
        FCMoveBool = true;
	}
	
	// Update is called once per frame
	void Update () {
        FCMoveVoid();

        if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.FourCardMoneyShow && FCMoveBool)
        {
            FCCoinTweenPosition.enabled = true;
        }
        else if (!FCMoveBool)
        {
            FCMoveBool = true;
        }
	}

    void FCMoveVoid()
    {
        if (TableArea == ENUM_BACCARAT_AWARD_AREA.Banker)
        {
            if (MainGame_Control.WinArea[0] == 1 && FourCard_Control.BetMoney[0] != 0)
            {
                FCCoinTweenPosition.to = FCStartAndWinVector3;
            }
            else
            {
                FCCoinTweenPosition.to = FCLoseVector3;
            }
        }
        else if (TableArea == ENUM_BACCARAT_AWARD_AREA.Player)
        {
            if (MainGame_Control.WinArea[1] == 1 && FourCard_Control.BetMoney[1] != 0)
            {
                FCCoinTweenPosition.to = FCStartAndWinVector3;
            }
            else
            {
                FCCoinTweenPosition.to = FCLoseVector3;
            }
        }
        else if (TableArea == ENUM_BACCARAT_AWARD_AREA.Draw)
        {
            FCCoinTweenPosition.to = FCLoseVector3;
        }
    }

    public void FCMoveDataInit()
    {
        Transform[] Objs = FCBetTable.GetComponentsInChildren<Transform>();
        int Len = Objs.Length;
        for (int i = 0; i < Len; i++)
        {
            if (Objs[i].name != "FCBankerTable" && Objs[i].name != "FCPlayerTable")
            {
                Destroy(Objs[i].gameObject);
            }
        }
        MainGame_Control.StopModeState = ENUM_STOPMODE_STATE.FourCardEnd;
        FCMoveBool = false;
        FourCard_Control.FourCardBetShow = true;
        FCCoinTweenPosition.ResetToBeginning();
    }
}
