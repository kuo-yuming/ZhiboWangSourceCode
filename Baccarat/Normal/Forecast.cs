using UnityEngine;
using System.Collections;

public class Forecast : MonoBehaviour {
    public static byte BankerWinParkwaySeatX = 0;
    public static byte BankerWinParkwaySeatY = 0;
    private byte BankerWinPopeyesSaveColor = 0;//0:沒紀錄 1:紅 2:藍
    private byte BankerWinLaneColor = 0;
    private byte BankerWinZadzhaColor = 0;

    public static byte PlayerWinParkwaySeatX = 0;
    public static byte PlayerWinParkwaySeatY = 0;
    private byte PlayerWinPopeyesSaveColor = 0;//0:沒紀錄 1:紅 2:藍
    private byte PlayerWinLaneColor = 0;
    private byte PlayerWinZadzhaColor = 0;

    public static bool Init_Bool = false;
    public static bool BankerForecastCheck = false;
    public static bool PlayerForecastCheck = false;

    public UISprite[] BankerSprite;
    public UISprite[] PlayerSprite;
    // Use this for initialization
    void Start () {
        Init_Bool = false;
        BankerForecastCheck = false;
        PlayerForecastCheck = false;
        DataInit();
    }
	
	// Update is called once per frame
	void Update () {
        if (BankerForecastCheck)
        {
            BankerPopeyesVoid();
            if (BankerWinParkwaySeatX >= 2)
            {
                if (BankerWinParkwaySeatX == 2 && BankerWinParkwaySeatY >= 1)
                {
                    BankerLaneVoid();
                }
                else if (BankerWinParkwaySeatX > 2)
                {
                    BankerLaneVoid();
                }
            }
            if (BankerWinParkwaySeatX >= 3)
            {
                if (BankerWinParkwaySeatX == 3 && BankerWinParkwaySeatY >= 1)
                {
                    BankerZadzhaVoid();
                }
                else if (BankerWinParkwaySeatX > 3)
                {
                    BankerZadzhaVoid();
                }
            }
            BankerForecastCheck = false;
        }
        if (PlayerForecastCheck)
        {
            PlayerPopeyesVoid();
            if (PlayerWinParkwaySeatX >= 2)
            {
                if (PlayerWinParkwaySeatX == 2 && PlayerWinParkwaySeatY >= 1)
                {
                    PlayerLaneVoid();
                }
                else if (PlayerWinParkwaySeatX > 2)
                {
                    PlayerLaneVoid();
                }
            }
            if (PlayerWinParkwaySeatX >= 3)
            {
                if (PlayerWinParkwaySeatX == 3 && PlayerWinParkwaySeatY >= 1)
                {
                    PlayerZadzhaVoid();
                }
                else if (PlayerWinParkwaySeatX > 3)
                {
                    PlayerZadzhaVoid();
                }
            }
            PlayerForecastCheck = false;
        }

        if (Init_Bool)
        {
            DataInit();
            Init_Bool = false;
        }
    }

    //Popeyes大眼仔
    void BankerPopeyesVoid()
    {
        if (BankerWinParkwaySeatX >= 1)
        {
            if (BankerWinParkwaySeatX == 1 && BankerWinParkwaySeatY >= 1)
            {
                if (BankerWinParkwaySeatY <= History_Control.HistoryParkwayData[(byte)(BankerWinParkwaySeatX - 1)])
                {
                    BankerWinPopeyesSaveColor = 1;
                }
                else
                {
                    if (BankerWinParkwaySeatY == 1)
                    {
                        BankerWinPopeyesSaveColor = 2;
                    }
                    else if ((BankerWinParkwaySeatY - History_Control.HistoryParkwayData[(byte)(BankerWinParkwaySeatX - 1)]) == 1)
                    {
                        BankerWinPopeyesSaveColor = 2;
                    }
                    else
                    {
                        BankerWinPopeyesSaveColor = 1;
                    }
                }
            }
            else
            {
                if (BankerWinParkwaySeatY == 0)
                {
                    if (History_Control.HistoryParkwayData[(byte)(BankerWinParkwaySeatX - 1)] == History_Control.HistoryParkwayData[(byte)(BankerWinParkwaySeatX - 2)])
                    {
                        BankerWinPopeyesSaveColor = 1;
                    }
                    else
                    {
                        BankerWinPopeyesSaveColor = 2;
                    }
                }
                else
                {
                    if (BankerWinParkwaySeatY <= History_Control.HistoryParkwayData[(byte)(BankerWinParkwaySeatX - 1)])
                    {
                        BankerWinPopeyesSaveColor = 1;
                    }
                    else
                    {
                        if ((BankerWinParkwaySeatY - History_Control.HistoryParkwayData[(byte)(BankerWinParkwaySeatX - 1)]) == 1)
                        {
                            BankerWinPopeyesSaveColor = 2;
                        }
                        else
                        { 
                            BankerWinPopeyesSaveColor = 1;
                        }
                    }
                }
            }
        }
        BankerSprite[0].enabled = true;
        if (BankerWinPopeyesSaveColor == 1)
        {
            BankerSprite[0].spriteName = "bg_j_4";
        }
        else if (BankerWinPopeyesSaveColor == 2)
        {
            BankerSprite[0].spriteName = "bg_j_3";
        }
    }

    void PlayerPopeyesVoid()
    {
        if (PlayerWinParkwaySeatX >= 1)
        {
            if (PlayerWinParkwaySeatX == 1 && PlayerWinParkwaySeatY >= 1)
            {
                if (PlayerWinParkwaySeatY <= History_Control.HistoryParkwayData[(byte)(PlayerWinParkwaySeatX - 1)])
                {
                    PlayerWinPopeyesSaveColor = 1;
                }
                else
                {
                    if (PlayerWinParkwaySeatY == 1)
                    {
                        PlayerWinPopeyesSaveColor = 2;
                    }
                    else if ((PlayerWinParkwaySeatY - History_Control.HistoryParkwayData[(byte)(PlayerWinParkwaySeatX - 1)]) == 1)
                    {
                        PlayerWinPopeyesSaveColor = 2;
                    }
                    else
                    {
                        PlayerWinPopeyesSaveColor = 1;
                    }
                }
            }
            else
            {
                if (PlayerWinParkwaySeatY == 0)
                {
                    if (History_Control.HistoryParkwayData[(byte)(PlayerWinParkwaySeatX - 1)] == History_Control.HistoryParkwayData[(byte)(PlayerWinParkwaySeatX - 2)])
                    {
                        PlayerWinPopeyesSaveColor = 1;
                    }
                    else
                    {
                        PlayerWinPopeyesSaveColor = 2;
                    }
                }
                else
                {
                    if (PlayerWinParkwaySeatY <= History_Control.HistoryParkwayData[(byte)(PlayerWinParkwaySeatX - 1)])
                    {
                        PlayerWinPopeyesSaveColor = 1;
                    }
                    else
                    {
                        if ((PlayerWinParkwaySeatY - History_Control.HistoryParkwayData[(byte)(PlayerWinParkwaySeatX - 1)]) == 1)
                        {
                            PlayerWinPopeyesSaveColor = 2;
                        }
                        else
                        {
                            PlayerWinPopeyesSaveColor = 1;
                        }
                    }
                }
            }
        }
        PlayerSprite[0].enabled = true;
        if (PlayerWinPopeyesSaveColor == 1)
        {
            PlayerSprite[0].spriteName = "bg_j_4";
        }
        else if (PlayerWinPopeyesSaveColor == 2)
        {
            PlayerSprite[0].spriteName = "bg_j_3";
        }
    }

    //Lane小路
    void BankerLaneVoid()
    {
        if (BankerWinParkwaySeatX >= 2)
        {
            if (BankerWinParkwaySeatX == 2 && BankerWinParkwaySeatY >= 1)
            {
                if (BankerWinParkwaySeatY <= History_Control.HistoryParkwayData[(byte)(BankerWinParkwaySeatX - 2)])
                { 
                    BankerWinLaneColor = 1;
                }
                else
                {
                    if (BankerWinParkwaySeatY == 1)
                    {     
                        BankerWinLaneColor = 2;
                    }
                    else if ((BankerWinParkwaySeatY - History_Control.HistoryParkwayData[(byte)(BankerWinParkwaySeatX - 2)]) == 1)
                    {
                        BankerWinLaneColor = 2;
                    }
                    else
                    {
                        BankerWinLaneColor = 1;
                    }
                }
            }
            else
            {
                if (BankerWinParkwaySeatY == 0)
                {
                    if (History_Control.HistoryParkwayData[(byte)(BankerWinParkwaySeatX - 1)] == History_Control.HistoryParkwayData[(byte)(BankerWinParkwaySeatX - 3)])
                    {
                        BankerWinLaneColor = 1;
                    }
                    else
                    {
                        BankerWinLaneColor = 2;
                    }
                }
                else
                {
                    if (BankerWinParkwaySeatY <= History_Control.HistoryParkwayData[(byte)(BankerWinParkwaySeatX - 2)])
                    {
                        BankerWinLaneColor = 1;
                    }
                    else
                    {
                        if ((BankerWinParkwaySeatY - History_Control.HistoryParkwayData[(byte)(BankerWinParkwaySeatX - 2)]) == 1)
                        {
                            BankerWinLaneColor = 2;
                        }
                        else
                        {
                            BankerWinLaneColor = 1;
                        }
                    }
                }
            }
        }
        BankerSprite[1].enabled = true;
        if (BankerWinLaneColor == 1)
        {
            BankerSprite[1].spriteName = "bg_j_2";
        }
        else if (BankerWinLaneColor == 2)
        {
            BankerSprite[1].spriteName = "bg_j_1";
        }
    }

    void PlayerLaneVoid()
    {
        if (PlayerWinParkwaySeatX >= 2)
        {
            if (PlayerWinParkwaySeatX == 2 && PlayerWinParkwaySeatY >= 1)
            {
                if (PlayerWinParkwaySeatY <= History_Control.HistoryParkwayData[(byte)(PlayerWinParkwaySeatX - 2)])
                {
                    PlayerWinLaneColor = 1;
                }
                else
                {
                    if (PlayerWinParkwaySeatY == 1)
                    {
                        PlayerWinLaneColor = 2;
                    }
                    else if ((PlayerWinParkwaySeatY - History_Control.HistoryParkwayData[(byte)(PlayerWinParkwaySeatX - 2)]) == 1)
                    {
                        PlayerWinLaneColor = 2;
                    }
                    else
                    {
                        PlayerWinLaneColor = 1;
                    }
                }
            }
            else
            {
                if (PlayerWinParkwaySeatY == 0)
                {
                    if (History_Control.HistoryParkwayData[(byte)(PlayerWinParkwaySeatX - 1)] == History_Control.HistoryParkwayData[(byte)(PlayerWinParkwaySeatX - 3)])
                    { 
                        PlayerWinLaneColor = 1;
                    }
                    else
                    {
                        PlayerWinLaneColor = 2;
                    }
                }
                else
                {
                    if (PlayerWinParkwaySeatY <= History_Control.HistoryParkwayData[(byte)(PlayerWinParkwaySeatX - 2)])
                    {
                        PlayerWinLaneColor = 1;
                    }
                    else
                    {
                        if ((PlayerWinParkwaySeatY - History_Control.HistoryParkwayData[(byte)(PlayerWinParkwaySeatX - 2)]) == 1)
                        {
                            PlayerWinLaneColor = 2;
                        }
                        else
                        {
                            PlayerWinLaneColor = 1;
                        }
                    }
                }
            }
        }
        PlayerSprite[1].enabled = true;
        if (PlayerWinLaneColor == 1)
        {
            PlayerSprite[1].spriteName = "bg_j_2";
        }
        else if (PlayerWinLaneColor == 2)
        {
            PlayerSprite[1].spriteName = "bg_j_1";
        }
    }

    //Zadzha 小強路
    void BankerZadzhaVoid()
    {
        if (BankerWinParkwaySeatX >= 3)
        {
            if (BankerWinParkwaySeatX == 3 && BankerWinParkwaySeatY >= 1)
            {
                if (BankerWinParkwaySeatY <= History_Control.HistoryParkwayData[(byte)(BankerWinParkwaySeatX - 3)])
                {
                    BankerWinZadzhaColor = 1;
                }
                else
                {
                    if (BankerWinParkwaySeatY == 1)
                    {
                        BankerWinZadzhaColor = 2;
                    }
                    else if ((BankerWinParkwaySeatY - History_Control.HistoryParkwayData[(byte)(BankerWinParkwaySeatX - 3)]) == 1)
                    {
                        BankerWinZadzhaColor = 2;
                    }
                    else
                    {
                        BankerWinZadzhaColor = 1;
                    }
                }
            }
            else
            {
                if (BankerWinParkwaySeatY == 0)
                {
                    if (History_Control.HistoryParkwayData[(byte)(BankerWinParkwaySeatX - 1)] == History_Control.HistoryParkwayData[(byte)(BankerWinParkwaySeatX - 4)])
                    {
                        BankerWinZadzhaColor = 1;
                    }
                    else
                    {
                        BankerWinZadzhaColor = 2;
                    }
                }
                else
                {
                    if (BankerWinParkwaySeatY <= History_Control.HistoryParkwayData[(byte)(BankerWinParkwaySeatX - 3)])
                    {
                        BankerWinZadzhaColor = 1;
                    }
                    else
                    {
                        if ((BankerWinParkwaySeatY - History_Control.HistoryParkwayData[(byte)(BankerWinParkwaySeatX - 3)]) == 1)
                        {
                            BankerWinZadzhaColor = 2;
                        }
                        else
                        {
                            BankerWinZadzhaColor = 1;
                        }
                    }
                }
            }
        }
        BankerSprite[2].enabled = true;
        if (BankerWinLaneColor == 1)
        {
            BankerSprite[2].spriteName = "bg_j_6";
        }
        else if (BankerWinLaneColor == 2)
        {
            BankerSprite[2].spriteName = "bg_j_5";
        }
    }

    void PlayerZadzhaVoid()
    {
        if (PlayerWinParkwaySeatX >= 3)
        {
            if (PlayerWinParkwaySeatX == 3 && PlayerWinParkwaySeatY >= 1)
            {
                if (PlayerWinParkwaySeatY <= History_Control.HistoryParkwayData[(byte)(PlayerWinParkwaySeatX - 3)])
                {
                    PlayerWinZadzhaColor = 1;
                }
                else
                {
                    if (PlayerWinParkwaySeatY == 1)
                    {
                        PlayerWinZadzhaColor = 2;
                    }
                    else if ((PlayerWinParkwaySeatY - History_Control.HistoryParkwayData[(byte)(PlayerWinParkwaySeatX - 3)]) == 1)
                    {
                        PlayerWinZadzhaColor = 2;
                    }
                    else
                    {
                        PlayerWinZadzhaColor = 1;
                    }
                }
            }
            else
            {
                if (PlayerWinParkwaySeatY == 0)
                {
                    if (History_Control.HistoryParkwayData[(byte)(PlayerWinParkwaySeatX - 1)] == History_Control.HistoryParkwayData[(byte)(PlayerWinParkwaySeatX - 4)])
                    {
                        PlayerWinZadzhaColor = 1;
                    }
                    else
                    {
                        PlayerWinZadzhaColor = 2;
                    }
                }
                else
                {
                    if (PlayerWinParkwaySeatY <= History_Control.HistoryParkwayData[(byte)(PlayerWinParkwaySeatX - 3)])
                    {
                        PlayerWinZadzhaColor = 1;
                    }
                    else
                    {
                        if ((PlayerWinParkwaySeatY - History_Control.HistoryParkwayData[(byte)(PlayerWinParkwaySeatX - 3)]) == 1)
                        {
                            PlayerWinZadzhaColor = 2;
                        }
                        else
                        {
                            PlayerWinZadzhaColor = 1;
                        }
                    }
                }
            }
        }
        PlayerSprite[2].enabled = true;
        if (PlayerWinZadzhaColor == 1)
        {
            PlayerSprite[2].spriteName = "bg_j_6";
        }
        else if (PlayerWinZadzhaColor == 2)
        {
            PlayerSprite[2].spriteName = "bg_j_5";
        }
    }

    void DataInit()
    {
        BankerWinParkwaySeatX = 0;
        BankerWinParkwaySeatY = 0;
        BankerWinPopeyesSaveColor = 0;//0:沒紀錄 1:紅 2:藍
        BankerWinLaneColor = 0;
        BankerWinZadzhaColor = 0;
        PlayerWinParkwaySeatX = 0;
        PlayerWinParkwaySeatY = 0;
        PlayerWinPopeyesSaveColor = 0;//0:沒紀錄 1:紅 2:藍
        PlayerWinLaneColor = 0;
        PlayerWinZadzhaColor = 0;
        BankerSprite[0].enabled = false;
        BankerSprite[1].enabled = false;
        BankerSprite[2].enabled = false;
        PlayerSprite[0].enabled = false;
        PlayerSprite[1].enabled = false;
        PlayerSprite[2].enabled = false;
    }
}
