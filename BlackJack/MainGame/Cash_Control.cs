using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CardTeamListClass;
using MoneyTable;
using GameCore.Manager.BlackJack;

public class Cash_Control : MonoBehaviour {

    public static byte SavePlayerSeat = 0;
    public UILabel[] CashLabelList = new UILabel[10];//各區押注金額顯示
    public UISprite[] CashBar = new UISprite[10];//各區金額Bar顯示
    public static int[] TableCash = new int[10];//储存各區錢幣狀態
    public UISprite[] PlayerRound_Sprite = new UISprite[5];//換個玩家
    public static byte[] PlayerRound = new byte[5];//1 = 玩家回合
    public static byte[] ThisTablePlayerIn = new byte[5];//各區是否有玩家進入 0:沒有 1:有
    public static uint[] PlayerDBID = new uint[5];//各區玩家DBID
    public static byte[] PlayerRealSeat = new byte[5];//玩家真實座位储存
    public static bool OnBetClick = false;//按下押注
    public static bool CashMoveStart = false;//錢幣移動開始
    public static byte[] CashMoveEnd = new byte[5];//錢幣移動結束,BAR錢幣生成開始
    public static byte[] BuyInsure = new byte[5];//購買保險
    public UISprite[] Insure_Sprite = new UISprite[5];//購買保險後圖示
    public static string[] PlayerName = new string[5];//玩家姓名
    public UILabel[] PlayerName_Label = new UILabel[5];
    public UISprite[] PlayerOut = new UISprite[5];//玩家已離開(圖)
    public static byte[] PlayerOutSave = new byte[5];//玩家已離開(1: 離開)
    

    //玩家頭像設定
    public UISprite[] PlayerPicture_Sprite;//玩家頭像顯示
    public byte[] FirstPictrueCheck = new byte[5];

    public static bool CashInit_Bool = false;

    public UISprite[] PlayerRoundPicturt_Sprite = new UISprite[10];
    public static byte[] PlayerRoundPlicture_Number = new byte[5];
    // Use this for initialization
    void Start ()
    {
        DataInit();
    }

    // Update is called once per frame
    void Update()
    {
        if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.NewRound
                       || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.ShuffleNewRound
                       || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.WaitBet)
        {
            for (int i = 0; i < 5; i++)
            {
                PlayerOut[i].enabled = false;
                PlayerOutSave[i] = 0;
            }
        }
        else
        {
            PlayerOutCheck();
        }

        CashState();
        BuyInsureCheck();
        PlayerRoundBar();
        if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.NewRound
                          || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.ShuffleNewRound
                          || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.WaitBet
                          || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.GameOver)
        {
            PlyaerPictureState();

            PlayerName_Label[(byte)TableList.MyTable].text = PlayerName[(byte)TableList.MyTable];
            PlayerName_Label[(byte)TableList.PlayerTable1].text = PlayerName[(byte)TableList.PlayerTable1];
            PlayerName_Label[(byte)TableList.PlayerTable2].text = PlayerName[(byte)TableList.PlayerTable2];
            PlayerName_Label[(byte)TableList.PlayerTable3].text = PlayerName[(byte)TableList.PlayerTable3];
            PlayerName_Label[(byte)TableList.PlayerTable4].text = PlayerName[(byte)TableList.PlayerTable4];

        }


        if (PlayerRound[(byte)TableList.MyTable] == 1)
            PlayerRound_Sprite[(byte)TableList.MyTable].enabled = true;
        else
            PlayerRound_Sprite[(byte)TableList.MyTable].enabled = false;

        if (PlayerRound[(byte)TableList.PlayerTable1] == 1)
            PlayerRound_Sprite[(byte)TableList.PlayerTable1].enabled = true;
        else
            PlayerRound_Sprite[(byte)TableList.PlayerTable1].enabled = false;

        if (PlayerRound[(byte)TableList.PlayerTable2] == 1)
            PlayerRound_Sprite[(byte)TableList.PlayerTable2].enabled = true;
        else
            PlayerRound_Sprite[(byte)TableList.PlayerTable2].enabled = false;

        if (PlayerRound[(byte)TableList.PlayerTable3] == 1)
            PlayerRound_Sprite[(byte)TableList.PlayerTable3].enabled = true;
        else
            PlayerRound_Sprite[(byte)TableList.PlayerTable3].enabled = false;

        if (PlayerRound[(byte)TableList.PlayerTable4] == 1)
            PlayerRound_Sprite[(byte)TableList.PlayerTable4].enabled = true;
        else
            PlayerRound_Sprite[(byte)TableList.PlayerTable4].enabled = false;


        if (CashInit_Bool || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.GameOver)
        {
            for (int i = 0; i < 10; i++)
            {
                TableCash[i] = 0;
            }

            for (int i = 0; i < 5; i++)
            {
                CashMoveEnd[i] = 1;
                PlayerRound[i] = 0;
                BuyInsure[i] = 0;
                PlayerOutSave[i] = 0;
            }

            CashInit_Bool = false;
        }

        if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.WaitBet
            || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.NewRound
            || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.ShuffleNewRound)
        {
            for (int i = 0; i < 5; i++)
            {
                CashMoveEnd[i] = 0;
            }
        }

    }

    //各區錢幣顯示狀態
    #region CashState
    void CashState()
    {
        //自己
        CashBar[(byte)CardTeamList.Card1Team1].enabled = true;

        CashLabelList[(byte)CardTeamList.Card1Team1].text = ((float)TableCash[(byte)CardTeamList.Card1Team1] / 1000).ToString() + "K";
        CashLabelList[(byte)CardTeamList.Card1Team2].text = ((float)TableCash[(byte)CardTeamList.Card1Team2] / 1000).ToString() + "K";
        CashLabelList[(byte)CardTeamList.Card2Team1].text = ((float)TableCash[(byte)CardTeamList.Card2Team1] / 1000).ToString() + "K";
        CashLabelList[(byte)CardTeamList.Card2Team2].text = ((float)TableCash[(byte)CardTeamList.Card2Team2] / 1000).ToString() + "K";
        CashLabelList[(byte)CardTeamList.Card3Team1].text = ((float)TableCash[(byte)CardTeamList.Card3Team1] / 1000).ToString() + "K";
        CashLabelList[(byte)CardTeamList.Card3Team2].text = ((float)TableCash[(byte)CardTeamList.Card3Team2] / 1000).ToString() + "K";
        CashLabelList[(byte)CardTeamList.Card4Team1].text = ((float)TableCash[(byte)CardTeamList.Card4Team1] / 1000).ToString() + "K";
        CashLabelList[(byte)CardTeamList.Card4Team2].text = ((float)TableCash[(byte)CardTeamList.Card4Team2] / 1000).ToString() + "K";
        CashLabelList[(byte)CardTeamList.Card5Team1].text = ((float)TableCash[(byte)CardTeamList.Card5Team1] / 1000).ToString() + "K";
        CashLabelList[(byte)CardTeamList.Card5Team2].text = ((float)TableCash[(byte)CardTeamList.Card5Team2] / 1000).ToString() + "K";

        if (TableCash[(byte)CardTeamList.Card1Team1] == 0)
            CashLabelList[(byte)CardTeamList.Card1Team1].enabled = false;
        else
            CashLabelList[(byte)CardTeamList.Card1Team1].enabled = true;

        if (TableCash[(byte)CardTeamList.Card1Team2] == 0)
        {
            CashBar[(byte)CardTeamList.Card1Team2].enabled = false;
            CashLabelList[(byte)CardTeamList.Card1Team2].enabled = false;
        }
        else
        {
            CashBar[(byte)CardTeamList.Card1Team2].enabled = true;
            CashLabelList[(byte)CardTeamList.Card1Team2].enabled = true;
        }

        //玩家一
        if (TableCash[(byte)CardTeamList.Card2Team1] == 0)
            CashLabelList[(byte)CardTeamList.Card2Team1].enabled = false;
        else
            CashLabelList[(byte)CardTeamList.Card2Team1].enabled = true;

        if (TableCash[(byte)CardTeamList.Card2Team2] == 0)
        {
            CashBar[(byte)CardTeamList.Card2Team2].enabled = false;
            CashLabelList[(byte)CardTeamList.Card2Team2].enabled = false;
        }
        else
        {
            CashBar[(byte)CardTeamList.Card2Team2].enabled = true;
            CashLabelList[(byte)CardTeamList.Card2Team2].enabled = true;
        }

        //玩家二
        if (TableCash[(byte)CardTeamList.Card3Team1] == 0)
            CashLabelList[(byte)CardTeamList.Card3Team1].enabled = false;
        else
            CashLabelList[(byte)CardTeamList.Card3Team1].enabled = true;

        if (TableCash[(byte)CardTeamList.Card3Team2] == 0)
        {
            CashBar[(byte)CardTeamList.Card3Team2].enabled = false;
            CashLabelList[(byte)CardTeamList.Card3Team2].enabled = false;
        }
        else
        {
            CashBar[(byte)CardTeamList.Card3Team2].enabled = true;
            CashLabelList[(byte)CardTeamList.Card3Team2].enabled = true;
        }

        //玩家三
        if (TableCash[(byte)CardTeamList.Card4Team1] == 0)
            CashLabelList[(byte)CardTeamList.Card4Team1].enabled = false;
        else
            CashLabelList[(byte)CardTeamList.Card4Team1].enabled = true;

        if (TableCash[(byte)CardTeamList.Card4Team2] == 0)
        {
            CashBar[(byte)CardTeamList.Card4Team2].enabled = false;
            CashLabelList[(byte)CardTeamList.Card4Team2].enabled = false;
        }
        else
        {
            CashBar[(byte)CardTeamList.Card4Team2].enabled = true;
            CashLabelList[(byte)CardTeamList.Card4Team2].enabled = true;
        }
        //玩家四
        if (TableCash[(byte)CardTeamList.Card5Team1] == 0)
            CashLabelList[(byte)CardTeamList.Card5Team1].enabled = false;
        else
            CashLabelList[(byte)CardTeamList.Card5Team1].enabled = true;

        if (TableCash[(byte)CardTeamList.Card5Team2] == 0)
        {
            CashBar[(byte)CardTeamList.Card5Team2].enabled = false;
            CashLabelList[(byte)CardTeamList.Card5Team2].enabled = false;
        }
        else
        {
            CashBar[(byte)CardTeamList.Card5Team2].enabled = true;
            CashLabelList[(byte)CardTeamList.Card5Team2].enabled = true;
        }
    }
    #endregion

    //各區玩家頭像顯示狀態
    #region PictureState
    void PlyaerPictureState()
    {
        //自己
        if (ThisTablePlayerIn[(byte)TableList.MyTable] == 1 && FirstPictrueCheck[(byte)TableList.MyTable] == 0)
        {
            int RanNumber = Random.Range(1,5);
            PlayerPicture_Sprite[(byte)TableList.MyTable].spriteName = "icon_portrait_" + RanNumber;
            FirstPictrueCheck[(byte)TableList.MyTable] = 1;
        }
        else if (ThisTablePlayerIn[(byte)TableList.MyTable] == 0 && FirstPictrueCheck[(byte)TableList.MyTable] == 1)
        {
            PlayerPicture_Sprite[(byte)TableList.MyTable].spriteName = "icon_portrait_0";
            PlayerName[(byte)TableList.MyTable] = "";
            FirstPictrueCheck[(byte)TableList.MyTable] = 0;
            PlayerDBID[(byte)TableList.MyTable] = 0;
            PlayerRealSeat[(byte)TableList.MyTable] = 0;
        }

        //玩家一
        if (ThisTablePlayerIn[(byte)TableList.PlayerTable1] == 1 && FirstPictrueCheck[(byte)TableList.PlayerTable1] == 0)
        {
            if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.NewRound
           || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.ShuffleNewRound
           || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.WaitBet)
            {
                int RanNumber = Random.Range(1, 5);
                PlayerPicture_Sprite[(byte)TableList.PlayerTable1].spriteName = "icon_portrait_" + RanNumber;
                FirstPictrueCheck[(byte)TableList.PlayerTable1] = 1;
                CashBar[(byte)CardTeamList.Card2Team1].enabled = true;
            }
        }
        else if (ThisTablePlayerIn[(byte)TableList.PlayerTable1] == 0 && FirstPictrueCheck[(byte)TableList.PlayerTable1] == 1)
        {
            PlayerPicture_Sprite[(byte)TableList.PlayerTable1].spriteName = "icon_portrait_0";
            FirstPictrueCheck[(byte)TableList.PlayerTable1] = 0;
            PlayerName[(byte)TableList.PlayerTable1] = "";
            CashBar[(byte)CardTeamList.Card2Team1].enabled = false;
            CashBar[(byte)CardTeamList.Card2Team2].enabled = false;
            PlayerDBID[(byte)TableList.PlayerTable1] = 0;
            PlayerRealSeat[(byte)TableList.PlayerTable1] = 0;
        }

        //玩家二
        if (ThisTablePlayerIn[(byte)TableList.PlayerTable2] == 1 && FirstPictrueCheck[(byte)TableList.PlayerTable2] == 0)
        {
            if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.NewRound
           || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.ShuffleNewRound
           || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.WaitBet)
            {
                int RanNumber = Random.Range(1, 5);
                PlayerPicture_Sprite[(byte)TableList.PlayerTable2].spriteName = "icon_portrait_" + RanNumber;
                FirstPictrueCheck[(byte)TableList.PlayerTable2] = 1;
                CashBar[(byte)CardTeamList.Card3Team1].enabled = true;
            }
        }
        else if (ThisTablePlayerIn[(byte)TableList.PlayerTable2] == 0 && FirstPictrueCheck[(byte)TableList.PlayerTable2] == 1)
        {
            PlayerPicture_Sprite[(byte)TableList.PlayerTable2].spriteName = "icon_portrait_0";
            FirstPictrueCheck[(byte)TableList.PlayerTable2] = 0;
            PlayerName[(byte)TableList.PlayerTable2] = "";
            CashBar[(byte)CardTeamList.Card3Team1].enabled = false;
            CashBar[(byte)CardTeamList.Card3Team2].enabled = false;
            PlayerDBID[(byte)TableList.PlayerTable2] = 0;
            PlayerRealSeat[(byte)TableList.PlayerTable2] = 0;
        }

        //玩家三
        if (ThisTablePlayerIn[(byte)TableList.PlayerTable3] == 1 && FirstPictrueCheck[(byte)TableList.PlayerTable3] == 0)
        {
            if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.NewRound
           || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.ShuffleNewRound
           || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.WaitBet)
            {
                int RanNumber = Random.Range(1, 5);
                PlayerPicture_Sprite[(byte)TableList.PlayerTable3].spriteName = "icon_portrait_" + RanNumber;
                FirstPictrueCheck[(byte)TableList.PlayerTable3] = 1;
                CashBar[(byte)CardTeamList.Card4Team1].enabled = true;
            }
        }
        else if (ThisTablePlayerIn[(byte)TableList.PlayerTable3] == 0 && FirstPictrueCheck[(byte)TableList.PlayerTable3] == 1)
        {
            PlayerPicture_Sprite[(byte)TableList.PlayerTable3].spriteName = "icon_portrait_0";
            FirstPictrueCheck[(byte)TableList.PlayerTable3] = 0;
            PlayerName[(byte)TableList.PlayerTable3] = "";
            CashBar[(byte)CardTeamList.Card4Team1].enabled = false;
            CashBar[(byte)CardTeamList.Card4Team2].enabled = false;
            PlayerDBID[(byte)TableList.PlayerTable3] = 0;
            PlayerRealSeat[(byte)TableList.PlayerTable3] = 0;
        }

        //玩家四
        if (ThisTablePlayerIn[(byte)TableList.PlayerTable4] == 1 && FirstPictrueCheck[(byte)TableList.PlayerTable4] == 0)
        {
            if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.NewRound
           || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.ShuffleNewRound
           || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.WaitBet)
            {
                int RanNumber = Random.Range(1, 5);
                PlayerPicture_Sprite[(byte)TableList.PlayerTable4].spriteName = "icon_portrait_" + RanNumber;
                FirstPictrueCheck[(byte)TableList.PlayerTable4] = 1;
                CashBar[(byte)CardTeamList.Card5Team1].enabled = true;
            }
        }
        else if (ThisTablePlayerIn[(byte)TableList.PlayerTable4] == 0 && FirstPictrueCheck[(byte)TableList.PlayerTable4] == 1)
        {
            PlayerPicture_Sprite[(byte)TableList.PlayerTable4].spriteName = "icon_portrait_0";
            FirstPictrueCheck[(byte)TableList.PlayerTable4] = 0;
            PlayerName[(byte)TableList.PlayerTable4] = "";
            CashBar[(byte)CardTeamList.Card5Team1].enabled = false;
            CashBar[(byte)CardTeamList.Card5Team2].enabled = false;
            PlayerDBID[(byte)TableList.PlayerTable4] = 0;
            PlayerRealSeat[(byte)TableList.PlayerTable4] = 0;
        }
    }
    #endregion

    //是否有買保險
    #region Insure
    void BuyInsureCheck()
    {
        if (BuyInsure[(byte)TableList.MyTable] == 1)
            Insure_Sprite[(byte)TableList.MyTable].enabled = true;
        else
            Insure_Sprite[(byte)TableList.MyTable].enabled = false;

        if (BuyInsure[(byte)TableList.PlayerTable1] == 1)
            Insure_Sprite[(byte)TableList.PlayerTable1].enabled = true;
        else
            Insure_Sprite[(byte)TableList.PlayerTable1].enabled = false;

        if (BuyInsure[(byte)TableList.PlayerTable2] == 1)
            Insure_Sprite[(byte)TableList.PlayerTable2].enabled = true;
        else
            Insure_Sprite[(byte)TableList.PlayerTable2].enabled = false;

        if (BuyInsure[(byte)TableList.PlayerTable3] == 1)
            Insure_Sprite[(byte)TableList.PlayerTable3].enabled = true;
        else
            Insure_Sprite[(byte)TableList.PlayerTable3].enabled = false;

        if (BuyInsure[(byte)TableList.PlayerTable4] == 1)
            Insure_Sprite[(byte)TableList.PlayerTable4].enabled = true;
        else
            Insure_Sprite[(byte)TableList.PlayerTable4].enabled = false;
    }
    #endregion

    void PlayerOutCheck()
    {
        if (PlayerOutSave[0] == 1)
            PlayerOut[0].enabled = true;
        else
            PlayerOut[0].enabled = false;

        if (PlayerOutSave[1] == 1)
            PlayerOut[1].enabled = true;
        else
            PlayerOut[1].enabled = false;

        if (PlayerOutSave[2] == 1)
            PlayerOut[2].enabled = true;
        else
            PlayerOut[2].enabled = false;

        if (PlayerOutSave[3] == 1)
            PlayerOut[3].enabled = true;
        else
            PlayerOut[3].enabled = false;

        if (PlayerOutSave[4] == 1)
            PlayerOut[4].enabled = true;
        else
            PlayerOut[4].enabled = false;
    }

    void PlayerRoundBar()
    {
        if ((PlayerRoundPlicture_Number[(byte)TableList.MyTable] == 0 || PlayerRoundPlicture_Number[(byte)TableList.MyTable] == 2) && PlayerRound[(byte)TableList.MyTable] == 1)
        {
            PlayerRoundPicturt_Sprite[0].enabled = true;
            PlayerRoundPicturt_Sprite[1].enabled = false;
        }
        else if (PlayerRoundPlicture_Number[(byte)TableList.MyTable] == 1 && PlayerRound[(byte)TableList.MyTable] == 1)
        {
            PlayerRoundPicturt_Sprite[0].enabled = false;
            PlayerRoundPicturt_Sprite[1].enabled = true;
        }
        else if (PlayerRound[(byte)TableList.MyTable] == 0)
        {
            PlayerRoundPicturt_Sprite[0].enabled = false;
            PlayerRoundPicturt_Sprite[1].enabled = false;
        }
        ////
        if ((PlayerRoundPlicture_Number[(byte)TableList.PlayerTable1] == 0 || PlayerRoundPlicture_Number[(byte)TableList.PlayerTable1] == 2) && PlayerRound[(byte)TableList.PlayerTable1] == 1)
        {
            PlayerRoundPicturt_Sprite[2].enabled = true;
            PlayerRoundPicturt_Sprite[3].enabled = false;
        }
        else if (PlayerRoundPlicture_Number[(byte)TableList.PlayerTable1] == 1 && PlayerRound[(byte)TableList.PlayerTable1] == 1)
        {
            PlayerRoundPicturt_Sprite[2].enabled = false;
            PlayerRoundPicturt_Sprite[3].enabled = true;
        }
        else if (PlayerRound[(byte)TableList.PlayerTable1] == 0)
        {
            PlayerRoundPicturt_Sprite[2].enabled = false;
            PlayerRoundPicturt_Sprite[3].enabled = false;
        }
        ////
        if ((PlayerRoundPlicture_Number[(byte)TableList.PlayerTable2] == 0 || PlayerRoundPlicture_Number[(byte)TableList.PlayerTable2] == 2) && PlayerRound[(byte)TableList.PlayerTable2] == 1)
        {
            PlayerRoundPicturt_Sprite[4].enabled = true;
            PlayerRoundPicturt_Sprite[5].enabled = false;
        }
        else if (PlayerRoundPlicture_Number[(byte)TableList.PlayerTable2] == 1 && PlayerRound[(byte)TableList.PlayerTable2] == 1)
        {
            PlayerRoundPicturt_Sprite[4].enabled = false;
            PlayerRoundPicturt_Sprite[5].enabled = true;
        }
        else if (PlayerRound[(byte)TableList.PlayerTable2] == 0)
        {
            PlayerRoundPicturt_Sprite[4].enabled = false;
            PlayerRoundPicturt_Sprite[5].enabled = false;
        }
        ////
        if ((PlayerRoundPlicture_Number[(byte)TableList.PlayerTable3] == 0 || PlayerRoundPlicture_Number[(byte)TableList.PlayerTable3] == 2) && PlayerRound[(byte)TableList.PlayerTable3] == 1)
        {
            PlayerRoundPicturt_Sprite[6].enabled = true;
            PlayerRoundPicturt_Sprite[7].enabled = false;
        }
        else if (PlayerRoundPlicture_Number[(byte)TableList.PlayerTable3] == 1 && PlayerRound[(byte)TableList.PlayerTable3] == 1)
        {
            PlayerRoundPicturt_Sprite[6].enabled = false;
            PlayerRoundPicturt_Sprite[7].enabled = true;
        }
        else if (PlayerRound[(byte)TableList.PlayerTable3] == 0)
        {
            PlayerRoundPicturt_Sprite[6].enabled = false;
            PlayerRoundPicturt_Sprite[7].enabled = false;
        }
        ////
        if ((PlayerRoundPlicture_Number[(byte)TableList.PlayerTable4] == 0 || PlayerRoundPlicture_Number[(byte)TableList.PlayerTable4] == 2) && PlayerRound[(byte)TableList.PlayerTable4] == 1)
        {
            PlayerRoundPicturt_Sprite[8].enabled = true;
            PlayerRoundPicturt_Sprite[9].enabled = false;
        }
        else if (PlayerRoundPlicture_Number[(byte)TableList.PlayerTable4] == 1 && PlayerRound[(byte)TableList.PlayerTable4] == 1)
        {
            PlayerRoundPicturt_Sprite[8].enabled = false;
            PlayerRoundPicturt_Sprite[9].enabled = true;
        }
        else if (PlayerRound[(byte)TableList.PlayerTable4] == 0)
        {
            PlayerRoundPicturt_Sprite[8].enabled = false;
            PlayerRoundPicturt_Sprite[9].enabled = false;
        }
    }

    void DataInit()
    {
        for (int i = 0; i < 10; i++)
        {
            PlayerRoundPicturt_Sprite[i].enabled = false;
            TableCash[i] = 0;
            CashBar[i].enabled = false;
            CashLabelList[i].enabled = false;
            if (i < 5)
            {
                ThisTablePlayerIn[i] = 0;
                FirstPictrueCheck[i] = 0;
                BuyInsure[i] = 0;
                Insure_Sprite[i].enabled = false;
                PlayerName[i] = "";
                PlayerName_Label[i].text = "";
                PlayerRound[i] = 0;
                PlayerDBID[i] = 0;
                PlayerRealSeat[i] = 0;
                CashMoveEnd[i] = 0;
                PlayerOutSave[i] = 0;
                PlayerOut[i].enabled = false;
                PlayerRoundPlicture_Number[i] = 0;
            }
        }
        CashMoveStart = false;
        ////測試
        //ThisTablePlayerIn[(byte)TableList.MyTable] = 1;
    }
}
