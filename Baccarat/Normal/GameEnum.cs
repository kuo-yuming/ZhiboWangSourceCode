using UnityEngine;
using System.Collections;

namespace GameEnum
{
    public enum ENUM_PUBLIC_BUTTON
    {
        Banker = 1,
        Player = 2,
        Draw = 3,
        BankerPair = 4,
        PlayerPair = 5,
        CashButtonSeat1 = 6,
        CashButtonSeat2 = 7,
        CashButtonSeat3 = 8,
        CashButtonSeat4 = 9,
        BetClear = 10,
        GameOut = 11,
        AutoPlus = 12,
        AutoMinus = 13,
        AutoModeButton = 14,
        HistroyButton = 15,
        FourCardBankerButton = 16,
        FourCardPlayerButton = 17,
        FourCardBetClear = 18,
        EndButton = 19,
        FourCardButtonClick = 20,
        InfoButton = 21,
        Help = 22,
        HelpOut = 23,
        HelpNext = 24,
        HelpBack = 25,
        HistoryButton2 = 26,
        RaceButton = 27,
        RaceExplain = 28,
        RaceNoData = 29,
    }

    public enum ENUM_STOPMODE_STATE
    {
        CardShow = 0,
        MoneyShow = 1,
        EndShow = 2,
        ShuffleTimeShow = 3,
        WaitNextNewRound = 4,
        WaitStop = 5,
        FourCardShow = 6,
        WaitFourCardTime = 7,
        FourCardEnd = 8,
        FourCardMoneyShow = 9,
        Idle = 10,
    }
}
