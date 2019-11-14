using UnityEngine;
using System.Collections;
using GameEnum;

public class CashButtonClick : MonoBehaviour {
    public ENUM_PUBLIC_BUTTON ButtonEnum;
    public static uint SelectCash = 0;

    void Start()
    {
        
    }

    void OnClick()
    {
        switch (ButtonEnum)
        {
            case ENUM_PUBLIC_BUTTON.CashButtonSeat1:
                CashButton_Control.NowCashButton = (byte)ENUM_PUBLIC_BUTTON.CashButtonSeat1;
                SelectCash = CashButton_Control.CashData[0];
                break;
            case ENUM_PUBLIC_BUTTON.CashButtonSeat2:
                CashButton_Control.NowCashButton = (byte)ENUM_PUBLIC_BUTTON.CashButtonSeat2;
                SelectCash = CashButton_Control.CashData[1];
                break;
            case ENUM_PUBLIC_BUTTON.CashButtonSeat3:
                CashButton_Control.NowCashButton = (byte)ENUM_PUBLIC_BUTTON.CashButtonSeat3;
                SelectCash = CashButton_Control.CashData[2];
                break;
            case ENUM_PUBLIC_BUTTON.CashButtonSeat4:
                CashButton_Control.NowCashButton = (byte)ENUM_PUBLIC_BUTTON.CashButtonSeat4;
                SelectCash = CashButton_Control.CashData[3];
                break;
        }
    }
}
