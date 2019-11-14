using UnityEngine;
using System.Collections;
using GameEnum;
using GameCore;
public class EndWindow_Control : MonoBehaviour {
    public GameObject[] EndWindowObject;
    public static byte[] OddsList = new byte[5];
    public static bool EndWindowOpenBool = false;
    public static bool EndPlanningBool = false;
    public GameObject EndWindowGameObject;
    public UILabel[] BetMoneyLabel = new UILabel[7];
    public UILabel[] WinMoneyLabel = new UILabel[7];
    public UILabel[] ProfitAndLossMoneyLabel = new UILabel[7];
    public UILabel[] BetMoneyLabel_N = new UILabel[6];
    public UILabel[] WinMoneyLabel_N = new UILabel[6];
    public UILabel[] ProfitAndLossMoneyLabel_N = new UILabel[6];
    public static ulong TotalWinMoney = 0;

    private uint[] BetMoney = new uint[7];
    private int[] WinMoney = new int[7];
    private int[] ProfitAndLossMoney = new int[7];

    private uint[] BetMoney_N = new uint[6];
    private int[] WinMoney_N = new int[6];
    private int[] ProfitAndLossMoney_N = new int[6];

    public static uint[] FourCardBetMoney = new uint[2];
	// Use this for initialization
	void Start () {
        EndWindowGameObject.SetActive(false);
        for (int i = 0; i < 7; i++)
        {
            BetMoney[i] = 0;
            WinMoney[i] = 0;
            ProfitAndLossMoney[i] = 0;
        }
        for (int i = 0; i < 6; i++)
        {
            BetMoney_N[i] = 0;
            WinMoney_N[i] = 0;
            ProfitAndLossMoney_N[i] = 0;
        }
        EndWindowOpenBool = false;
        EndPlanningBool = false;
        TotalWinMoney = 0;
        FourCardBetMoney[0] = 0;
        FourCardBetMoney[1] = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (EndWindowOpenBool)
        {
            EndWindowGameObject.SetActive(true);
        }
        else
        {
            EndWindowGameObject.SetActive(false);
        }

        if (FourCard_Control.FourCard != 0)
        {
            EndWindowObject[1].SetActive(true);
            EndWindowObject[0].SetActive(false);
        }
        else
        {
            EndWindowObject[0].SetActive(true);
            EndWindowObject[1].SetActive(false);
        }

        if (EndPlanningBool)
        {
            MoneyPlanningVoid();
            EndPlanningBool = false;
        }

        if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.WaitStop)
        {
            for (int i = 0; i < 7; i++)
            {
                BetMoney[i] = 0;
                WinMoney[i] = 0;
                ProfitAndLossMoney[i] = 0;
            }
            for (int i = 0; i < 6; i++)
            {
                BetMoney_N[i] = 0;
                WinMoney_N[i] = 0;
                ProfitAndLossMoney_N[i] = 0;
            }
            FourCardBetMoney[0] = 0;
            FourCardBetMoney[1] = 0;
            TotalWinMoney = 0;
        }

        for (int i = 0; i < 7; i++)
        {
            BetMoneyLabel[i].text = BetMoney[i].ToString();
            WinMoneyLabel[i].text = WinMoney[i].ToString();
            ProfitAndLossMoneyLabel[i].text = ProfitAndLossMoney[i].ToString();
        }
        for (int i = 0; i < 6; i++)
        {
            BetMoneyLabel_N[i].text = BetMoney_N[i].ToString();
            WinMoneyLabel_N[i].text = WinMoney_N[i].ToString();
            ProfitAndLossMoneyLabel_N[i].text = ProfitAndLossMoney_N[i].ToString();
        }
	}

    void MoneyPlanningVoid()
    {
        //押金
        //FC
        BetMoney[0] = AutoMode_Control.SaveMoney[0];
        BetMoney[1] = AutoMode_Control.SaveMoney[1];
        BetMoney[2] = AutoMode_Control.SaveMoney[2];
        BetMoney[3] = AutoMode_Control.SaveMoney[3];
        BetMoney[4] = AutoMode_Control.SaveMoney[4];
        BetMoney[5] = (uint)FourCardBetMoney[0] + (uint)FourCardBetMoney[1];
        BetMoney[6] = BetMoney[0] + BetMoney[1] + BetMoney[2] + BetMoney[3] + BetMoney[4] + BetMoney[5];

        //Normal
        BetMoney_N[0] = AutoMode_Control.SaveMoney[0];
        BetMoney_N[1] = AutoMode_Control.SaveMoney[1];
        BetMoney_N[2] = AutoMode_Control.SaveMoney[2];
        BetMoney_N[3] = AutoMode_Control.SaveMoney[3];
        BetMoney_N[4] = AutoMode_Control.SaveMoney[4];
        BetMoney_N[5] = BetMoney_N[0] + BetMoney_N[1] + BetMoney_N[2] + BetMoney_N[3] + BetMoney_N[4];


        //贏金
        if (MainGame_Control.WinArea[0] == 1)
        {
            if (MainGame_Control.WinArea[2] == 1)
            {
                WinMoney[0] = (int)((float)BetMoney[0]);
                WinMoney_N[0] = (int)((float)BetMoney[0]);
            }
            else
            {
                WinMoney[0] = (int)((float)BetMoney[0] * CCommonDef._DEF_Baccarat_MakersOdds);
                WinMoney_N[0] = (int)((float)BetMoney[0] * CCommonDef._DEF_Baccarat_MakersOdds);
            }
          
        }
        else
        {
            WinMoney[0] = 0;
            WinMoney_N[0] = 0;
        }
        if (MainGame_Control.WinArea[1] == 1)
        {
            if (MainGame_Control.WinArea[2] == 1)
            {
                WinMoney[1] = (int)((float)BetMoney[1]);
                WinMoney_N[1] = (int)((float)BetMoney[1]);
            }
            else
            {
                WinMoney[1] = (int)((float)BetMoney[1] * CCommonDef._DEF_Baccarat_PlayerOdds);
                WinMoney_N[1] = (int)((float)BetMoney[1] * CCommonDef._DEF_Baccarat_PlayerOdds);
            }  
        }
        else
        {
            WinMoney[1] = 0;
            WinMoney_N[1] = 0;
        }
        if (MainGame_Control.WinArea[2] == 1)
        {
            WinMoney[2] = (int)((float)BetMoney[2] * CCommonDef._DEF_Baccarat_GentleOdds);
            WinMoney_N[2] = (int)((float)BetMoney[2] * CCommonDef._DEF_Baccarat_GentleOdds);
        }
        else
        {
            WinMoney[2] = 0;
            WinMoney_N[2] = 0;
        }
        if (MainGame_Control.WinArea[3] == 1)
        {
            WinMoney[3] = (int)((float)BetMoney[3] * CCommonDef._DEF_Baccarat_MakersPairOdds);
            WinMoney_N[3] = (int)((float)BetMoney[3] * CCommonDef._DEF_Baccarat_MakersPairOdds);
        }
        else
        {
            WinMoney[3] = 0;
            WinMoney_N[3] = 0;
        }
        if (MainGame_Control.WinArea[4] == 1)
        {
            WinMoney[4] = (int)((float)BetMoney[4] * CCommonDef._DEF_Baccarat_PlayerPairOdds);
            WinMoney_N[4] = (int)((float)BetMoney[4] * CCommonDef._DEF_Baccarat_PlayerPairOdds);
        }
        else
        {
            WinMoney[4] = 0;
            WinMoney_N[4] = 0;
        }

        WinMoney[5] = (int)TotalWinMoney - (WinMoney[0] + WinMoney[1] + WinMoney[2] + WinMoney[3] + WinMoney[4]);

        WinMoney[6] = (int)TotalWinMoney;
        WinMoney_N[5] = (int)TotalWinMoney;

        //結算
        //FC
        ProfitAndLossMoney[0] = WinMoney[0] - (int)BetMoney[0];
        ProfitAndLossMoney[1] = WinMoney[1] - (int)BetMoney[1];
        ProfitAndLossMoney[2] = WinMoney[2] - (int)BetMoney[2];
        ProfitAndLossMoney[3] = WinMoney[3] - (int)BetMoney[3];
        ProfitAndLossMoney[4] = WinMoney[4] - (int)BetMoney[4];
        ProfitAndLossMoney[5] = WinMoney[5] - (int)BetMoney[5];
        ProfitAndLossMoney[6] = WinMoney[6] - (int)BetMoney[6];

        //Normal
        ProfitAndLossMoney_N[0] = WinMoney_N[0] - (int)BetMoney_N[0];
        ProfitAndLossMoney_N[1] = WinMoney_N[1] - (int)BetMoney_N[1];
        ProfitAndLossMoney_N[2] = WinMoney_N[2] - (int)BetMoney_N[2];
        ProfitAndLossMoney_N[3] = WinMoney_N[3] - (int)BetMoney_N[3];
        ProfitAndLossMoney_N[4] = WinMoney_N[4] - (int)BetMoney_N[4];
        ProfitAndLossMoney_N[5] = WinMoney_N[5] - (int)BetMoney_N[5];
    }
}
