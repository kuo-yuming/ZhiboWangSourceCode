using UnityEngine;
using System.Collections;
using GameEnum;
public class CashButton_Control : MonoBehaviour {
    public BoxCollider[] ButtonBox;
    public UIButton[] ButtonButton;

    public static byte NowCashButton = 0;
    public static uint[] CashData = new uint[4];//押注金額
	// Use this for initialization
	void Start () {
        //大小底台金幣的變動
        if (!Competition.RaceGame_Bool)
        {
            if (BaccaratManager.m_MachineTableArea[GameConnet.m_TMachineBuyInGameData.m_uiTID] == ushort.Parse(AutoButton_Control.TableGroupID[0].ToString()))
            {
                ButtonButton[0].normalSprite = "bg_money_100_0";
                ButtonButton[0].hoverSprite = "bg_money_100";
                CashData[0] = 100;
                ButtonButton[1].normalSprite = "bg_money_500_0";
                ButtonButton[1].hoverSprite = "bg_money_500";
                CashData[1] = 500;
                ButtonButton[2].normalSprite = "bg_money_1k_0";
                ButtonButton[2].hoverSprite = "bg_money_1k";
                CashData[2] = 1000;
                ButtonButton[3].normalSprite = "bg_money_5k_0";
                ButtonButton[3].hoverSprite = "bg_money_5k";
                CashData[3] = 5000;
            }
            else
            {
                ButtonButton[0].normalSprite = "bg_money_1k_0";
                ButtonButton[0].hoverSprite = "bg_money_1k";
                CashData[0] = 1000;
                ButtonButton[1].normalSprite = "bg_money_5k_0";
                ButtonButton[1].hoverSprite = "bg_money_5k";
                CashData[1] = 5000;
                ButtonButton[2].normalSprite = "bg_money_10k_0";
                ButtonButton[2].hoverSprite = "bg_money_10k";
                CashData[2] = 10000;
                ButtonButton[3].normalSprite = "bg_money_100k_0";
                ButtonButton[3].hoverSprite = "bg_money_100k";
                CashData[3] = 100000;
            }
        }
        else
        {
            ButtonButton[0].normalSprite = "bg_money_1k_0";
            ButtonButton[0].hoverSprite = "bg_money_1k";
            CashData[0] = 1000;
            ButtonButton[1].normalSprite = "bg_money_5k_0";
            ButtonButton[1].hoverSprite = "bg_money_5k";
            CashData[1] = 5000;
            ButtonButton[2].normalSprite = "bg_money_10k_0";
            ButtonButton[2].hoverSprite = "bg_money_10k";
            CashData[2] = 10000;
            ButtonButton[3].normalSprite = "bg_money_100k_0";
            ButtonButton[3].hoverSprite = "bg_money_100k";
            CashData[3] = 100000;
        }
        
        CashButtonClick.SelectCash = CashData[0];
        NowCashButton = (byte)ENUM_PUBLIC_BUTTON.CashButtonSeat1;
	}
	
	// Update is called once per frame
    void Update()
    {
        if (!Competition.RaceGame_Bool)
        {
            if (BaccaratManager.m_MachineTableArea[GameConnet.m_TMachineBuyInGameData.m_uiTID] == ushort.Parse(AutoButton_Control.TableGroupID[0].ToString()))
            {
                SmallCoin();
            }
            else
            {
                BigCoin();
            }
        }
        else
        {
            BigCoin();
        }
    }

    void SmallCoin()
    {
        switch (NowCashButton)
        {
            case (byte)ENUM_PUBLIC_BUTTON.CashButtonSeat1:
                ButtonBox[0].enabled = false;
                ButtonButton[0].enabled = false;
                ButtonButton[0].normalSprite = "bg_money_100";
                ButtonBox[1].enabled = true;
                ButtonButton[1].enabled = true;
                ButtonButton[1].normalSprite = "bg_money_500_0";
                ButtonBox[2].enabled = true;
                ButtonButton[2].enabled = true;
                ButtonButton[2].normalSprite = "bg_money_1k_0";
                ButtonBox[3].enabled = true;
                ButtonButton[3].enabled = true;
                ButtonButton[3].normalSprite = "bg_money_5k_0";
                CashButtonClick.SelectCash = CashData[0];
                break;
            case (byte)ENUM_PUBLIC_BUTTON.CashButtonSeat2:
                ButtonBox[0].enabled = true;
                ButtonButton[0].enabled = true;
                ButtonButton[0].normalSprite = "bg_money_100_0";
                ButtonBox[1].enabled = false;
                ButtonButton[1].enabled = false;
                ButtonButton[1].normalSprite = "bg_money_500";
                ButtonBox[2].enabled = true;
                ButtonButton[2].enabled = true;
                ButtonButton[2].normalSprite = "bg_money_1k_0";
                ButtonBox[3].enabled = true;
                ButtonButton[3].enabled = true;
                ButtonButton[3].normalSprite = "bg_money_5k_0";
                CashButtonClick.SelectCash = CashData[1];
                break;
            case (byte)ENUM_PUBLIC_BUTTON.CashButtonSeat3:
                ButtonBox[0].enabled = true;
                ButtonButton[0].enabled = true;
                ButtonButton[0].normalSprite = "bg_money_100_0";
                ButtonBox[1].enabled = true;
                ButtonButton[1].enabled = true;
                ButtonButton[1].normalSprite = "bg_money_500_0";
                ButtonBox[2].enabled = false;
                ButtonButton[2].enabled = false;
                ButtonButton[2].normalSprite = "bg_money_1k";
                ButtonBox[3].enabled = true;
                ButtonButton[3].enabled = true;
                ButtonButton[3].normalSprite = "bg_money_5k_0";
                CashButtonClick.SelectCash = CashData[2];
                break;
            case (byte)ENUM_PUBLIC_BUTTON.CashButtonSeat4:
                ButtonBox[0].enabled = true;
                ButtonButton[0].enabled = true;
                ButtonButton[0].normalSprite = "bg_money_100_0";
                ButtonBox[1].enabled = true;
                ButtonButton[1].enabled = true;
                ButtonButton[1].normalSprite = "bg_money_500_0";
                ButtonBox[2].enabled = true;
                ButtonButton[2].enabled = true;
                ButtonButton[2].normalSprite = "bg_money_1k_0";
                ButtonBox[3].enabled = false;
                ButtonButton[3].enabled = false;
                ButtonButton[3].normalSprite = "bg_money_5k";
                CashButtonClick.SelectCash = CashData[3];
                break;
        }
    }

    void BigCoin()
    {
        switch (NowCashButton)
        {
            case (byte)ENUM_PUBLIC_BUTTON.CashButtonSeat1:
                ButtonBox[0].enabled = false;
                ButtonButton[0].enabled = false;
                ButtonButton[0].normalSprite = "bg_money_1k";
                ButtonBox[1].enabled = true;
                ButtonButton[1].enabled = true;
                ButtonButton[1].normalSprite = "bg_money_5k_0";
                ButtonBox[2].enabled = true;
                ButtonButton[2].enabled = true;
                ButtonButton[2].normalSprite = "bg_money_10k_0";
                ButtonBox[3].enabled = true;
                ButtonButton[3].enabled = true;
                ButtonButton[3].normalSprite = "bg_money_100k_0";
                CashButtonClick.SelectCash = CashData[0];
                break;
            case (byte)ENUM_PUBLIC_BUTTON.CashButtonSeat2:
                ButtonBox[0].enabled = true;
                ButtonButton[0].enabled = true;
                ButtonButton[0].normalSprite = "bg_money_1k_0";
                ButtonBox[1].enabled = false;
                ButtonButton[1].enabled = false;
                ButtonButton[1].normalSprite = "bg_money_5k";
                ButtonBox[2].enabled = true;
                ButtonButton[2].enabled = true;
                ButtonButton[2].normalSprite = "bg_money_10k_0";
                ButtonBox[3].enabled = true;
                ButtonButton[3].enabled = true;
                ButtonButton[3].normalSprite = "bg_money_100k_0";
                CashButtonClick.SelectCash = CashData[1];
                break;
            case (byte)ENUM_PUBLIC_BUTTON.CashButtonSeat3:
                ButtonBox[0].enabled = true;
                ButtonButton[0].enabled = true;
                ButtonButton[0].normalSprite = "bg_money_1k_0";
                ButtonBox[1].enabled = true;
                ButtonButton[1].enabled = true;
                ButtonButton[1].normalSprite = "bg_money_5k_0";
                ButtonBox[2].enabled = false;
                ButtonButton[2].enabled = false;
                ButtonButton[2].normalSprite = "bg_money_10k";
                ButtonBox[3].enabled = true;
                ButtonButton[3].enabled = true;
                ButtonButton[3].normalSprite = "bg_money_100k_0";
                CashButtonClick.SelectCash = CashData[2];
                break;
            case (byte)ENUM_PUBLIC_BUTTON.CashButtonSeat4:
                ButtonBox[0].enabled = true;
                ButtonButton[0].enabled = true;
                ButtonButton[0].normalSprite = "bg_money_1k_0";
                ButtonBox[1].enabled = true;
                ButtonButton[1].enabled = true;
                ButtonButton[1].normalSprite = "bg_money_5k_0";
                ButtonBox[2].enabled = true;
                ButtonButton[2].enabled = true;
                ButtonButton[2].normalSprite = "bg_money_10k_0";
                ButtonBox[3].enabled = false;
                ButtonButton[3].enabled = false;
                ButtonButton[3].normalSprite = "bg_money_100k";
                CashButtonClick.SelectCash = CashData[3];
                break;
        }
    }
}
