using UnityEngine;
using System.Collections;
using GameCore.Manager.Baccarat;
using GameEnum;

public class CoinData : MonoBehaviour {

    public TweenPosition CoinTweenPosition;
    public UITable BetTable;
    public bool MoveBool = true;
    public ENUM_BACCARAT_AWARD_AREA TableArea;

    //位子設定
    private Vector3 StartAndWinVector3 = new Vector3(0,-280,0);//贏
    private Vector3 LoseVector3 = new Vector3(0,470,0);//輸
   
	// Use this for initialization
	void Start () {
        MoveBool = true;
	}
	
	// Update is called once per frame
	void Update () {

        MoveVoid();

        if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.MoneyShow && MoveBool)
        {
            CoinTweenPosition.enabled = true;
        }
        else if (!MoveBool)
        {
            MoveBool = true;
        }
	}

    void MoveVoid()
    {
        if (TableArea == ENUM_BACCARAT_AWARD_AREA.Banker)
        {
            if ((MainGame_Control.LastWin == ENUM_BACCARAT_AWARD.WinBanker || MainGame_Control.LastWin == ENUM_BACCARAT_AWARD.WinDraw) && BetTable_Control.MyBetMoneySeat[0] != 0)
            {
                CoinTweenPosition.to = StartAndWinVector3;
            }
            else
            {
                CoinTweenPosition.to = LoseVector3;
            }
        }
        else if (TableArea == ENUM_BACCARAT_AWARD_AREA.Player)
        {
            if ((MainGame_Control.LastWin == ENUM_BACCARAT_AWARD.WinPlayer || MainGame_Control.LastWin == ENUM_BACCARAT_AWARD.WinDraw) && BetTable_Control.MyBetMoneySeat[1] != 0)
            {
                CoinTweenPosition.to = StartAndWinVector3;
            }
            else
            {
                CoinTweenPosition.to = LoseVector3;
            }
        }
        else if (TableArea == ENUM_BACCARAT_AWARD_AREA.Draw)
        {
            if (MainGame_Control.LastWin == ENUM_BACCARAT_AWARD.WinDraw && BetTable_Control.MyBetMoneySeat[2] != 0)
            {
                CoinTweenPosition.to = StartAndWinVector3;
            }
            else
            {
                CoinTweenPosition.to = LoseVector3;
            }
        }
        else if (TableArea == ENUM_BACCARAT_AWARD_AREA.BankerPair)
        {
            if (MainGame_Control.WinArea[3] == 1 && BetTable_Control.MyBetMoneySeat[3] != 0)
            {
                CoinTweenPosition.to = StartAndWinVector3;
            }
            else
            {
                CoinTweenPosition.to = LoseVector3;
            }
        }
        else if (TableArea == ENUM_BACCARAT_AWARD_AREA.PlayerPair)
        {
            if (MainGame_Control.WinArea[4] == 1 && BetTable_Control.MyBetMoneySeat[4] != 0)
            {
                CoinTweenPosition.to = StartAndWinVector3;
            }
            else
            {
                CoinTweenPosition.to = LoseVector3;
            }
        }
    }

    public void MoveDataInit()
    {
        Transform[] Objs = BetTable.GetComponentsInChildren<Transform>();
        int Len = Objs.Length;
        for (int i = 0; i < Len; i++)
        {
            if (Objs[i].name != "BankerTable" && Objs[i].name != "PlayerTable" && Objs[i].name != "DrawTable" && Objs[i].name != "BankerPairTable" && Objs[i].name != "PlayerPairTable")
            {
                Destroy(Objs[i].gameObject);
            }
        }
        if (BetTable_Control.TableAllBetMoneySeat[0] != 0 || BetTable_Control.TableAllBetMoneySeat[1] != 0 || BetTable_Control.TableAllBetMoneySeat[2] != 0 || BetTable_Control.TableAllBetMoneySeat[3] != 0 || BetTable_Control.TableAllBetMoneySeat[4] != 0)
        {
            GameSound.CashBack_Bool = true;
        }
        MainGame_Control.StopModeState = ENUM_STOPMODE_STATE.EndShow;
        MyBetCashTip_Control.TipCashInitBool = true;
        EndWindow_Control.EndWindowOpenBool = true;
        MoveBool = false;
        CoinTweenPosition.enabled = false;
        Money_Control.CoinInitBool = true;
        History_Control.HistorySaveOkBool = true;
        FourCardHistory_Control.FCHistorySaveOkBool = true;
        CoinTweenPosition.ResetToBeginning();
    }
}
