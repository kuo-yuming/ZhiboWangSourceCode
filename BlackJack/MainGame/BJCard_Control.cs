using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CardTeamListClass;

namespace CardTeamListClass
{
    public enum CardTeamList
    {
        Card1Team1 = 0,
        Card1Team2 = 1,
        Card2Team1 = 2,
        Card2Team2 = 3,
        Card3Team1 = 4,
        Card3Team2 = 5,
        Card4Team1 = 6,
        Card4Team2 = 7,
        Card5Team1 = 8,
        Card5Team2 = 9,
        CardBanker = 10,
    }
}

public class BJCard_Control : MonoBehaviour {

    //储存每一區牌內容
    public static List<byte> Seat1Team1 = new List<byte>();//第1區第1組牌
    public static List<byte> Seat1Team2 = new List<byte>();//第1區第2組牌
    public static List<byte> Seat2Team1 = new List<byte>();//第2區第1組牌
    public static List<byte> Seat2Team2 = new List<byte>();//第2區第2組牌
    public static List<byte> Seat3Team1 = new List<byte>();//第3區第1組牌
    public static List<byte> Seat3Team2 = new List<byte>();//第3區第2組牌
    public static List<byte> Seat4Team1 = new List<byte>();//第4區第1組牌
    public static List<byte> Seat4Team2 = new List<byte>();//第4區第2組牌
    public static List<byte> Seat5Team1 = new List<byte>();//第5區第1組牌
    public static List<byte> Seat5Team2 = new List<byte>();//第5區第2組牌
    public static List<byte> SeatBanker = new List<byte>();//莊家牌組

    //储存每一區有多少張牌
    public static byte[] SeatTeamPoint = new byte[11];
    //每區是否增牌確認 1:確認 0:不確認
    public static byte[] SeatTeamAddCheck = new byte[11];
    //取得每區最後牌的位子 1:確認 0:不確認
    public static byte[] NowFinallCardSeat = new byte[11];
    //點數計算開始 1:開始 0:關閉
    public static byte[] PointCheckStart = new byte[11];
    //點數Tip開始 1:開始 0:關閉
    public static byte[] PointFinallSeatStart = new byte[11];
    //分牌確認 1:開始 0:關閉
    public static byte[] CardScoreboardStart = new byte[11];
    //分牌開始 1:開始 0:關閉
    public static byte[] ScoreBoardStart = new byte[11];

    //牌的開合BOOL確認
    public static bool ButtonOnClickBool = false;
    public static bool Seat1Team1OpenCloseCheck = false;
    public static bool Seat1Team2OpenCloseCheck = false;
    public static bool Seat2Team1OpenCloseCheck = false;
    public static bool Seat2Team2OpenCloseCheck = false;
    public static bool Seat3Team1OpenCloseCheck = false;
    public static bool Seat3Team2OpenCloseCheck = false;
    public static bool Seat4Team1OpenCloseCheck = false;
    public static bool Seat4Team2OpenCloseCheck = false;
    public static bool Seat5Team1OpenCloseCheck = false;
    public static bool Seat5Team2OpenCloseCheck = false;

    public TweenPosition[] ScoreBoard_TP = new TweenPosition[10];//分牌TWEEN
    public GameObject[] ScoreBoard_GO = new GameObject[10];
    public static Dictionary<CardTeamList, int> AddCard = new Dictionary<CardTeamList, int>();//另外储存卡片
    public static bool AddCardShow = false;//總分牌表演
    public static bool AddCard1Show = false;//分牌表演第一張
    public static bool AddCard2Show = false;//分牌表演第二張

    public UISprite[] ScoreBoard_Sprite = new UISprite[5];//分牌閃亮
    public UISpriteAnimation[] ScoreBoard_Animation = new UISpriteAnimation[5];//分牌動畫
    private byte[] ScoreBoardShow_Start = new byte[5];
   // private byte ScorBoardShowNumber = 1;
   // float ShowTime = 0;
    //強制結束 0:關 1:開
    public static byte[] CheckPoint = new byte[11];
    public static byte[] CardControl_End = new byte[11];
    public static byte[] CardMove_End = new byte[11];

    public static bool CardControlInit_Bool = false;
    // Use this for initialization
    void Start () {
        CardControlInit_Bool = false;
        BJCard_Control_Init();
    }

    // Update is called once per frame
    void Update()
    {
        ScoreBoardShow();
        CardShow();
        EndShow();

        #region ScoreBoardAnimation
        if (!ScoreBoard_Animation[0].isPlaying)
        {
            ScoreBoard_Sprite[0].enabled = false;
            ScoreBoard_Animation[0].ResetToBeginning();
            ScoreBoard_Animation[0].enabled = false;
            //ScoreBoard();
        }
        if (!ScoreBoard_Animation[1].isPlaying)
        {
            ScoreBoard_Sprite[1].enabled = false;
            ScoreBoard_Animation[1].ResetToBeginning();
            ScoreBoard_Animation[1].enabled = false;
            // ScoreBoard();
        }
        if (!ScoreBoard_Animation[2].isPlaying)
        {
            ScoreBoard_Sprite[2].enabled = false;
            ScoreBoard_Animation[2].ResetToBeginning();
            ScoreBoard_Animation[2].enabled = false;
            //   ScoreBoard();
        }
        if (!ScoreBoard_Animation[3].isPlaying)
        {
            ScoreBoard_Sprite[3].enabled = false;
            ScoreBoard_Animation[3].ResetToBeginning();
            ScoreBoard_Animation[3].enabled = false;
            //  ScoreBoard();
        }
        if (!ScoreBoard_Animation[4].isPlaying)
        {
            ScoreBoard_Sprite[4].enabled = false;
            ScoreBoard_Animation[4].ResetToBeginning();
            ScoreBoard_Animation[4].enabled = false;
            //   ScoreBoard();
        }
        #endregion

        if (CardControlInit_Bool)
        {
            BJCard_Control_Init();
            CardControlInit_Bool = false;
        }
    }

    //資料初始化
    void BJCard_Control_Init()
    {
        Seat1Team1.Clear();
        Seat1Team2.Clear();
        Seat2Team1.Clear();
        Seat2Team2.Clear();
        Seat3Team1.Clear();
        Seat3Team2.Clear();
        Seat4Team1.Clear();
        Seat4Team2.Clear();
        Seat5Team1.Clear();
        Seat5Team2.Clear();
        SeatBanker.Clear();
        for (int i = 0; i < 11; i++)
        {
            SeatTeamPoint[i] = 0;
            NowFinallCardSeat[i] = 0;
            SeatTeamAddCheck[i] = 0;
            PointCheckStart[i] = 0;
            PointFinallSeatStart[i] = 0;
            CardScoreboardStart[i] = 0;
            ScoreBoardStart[i] = 0;
            CardControl_End[i] = 0;
            CheckPoint[i] = 0;
            if (i < 10)
            {
                ScoreBoard_TP[i].ResetToBeginning();
            }
        }
        ButtonOnClickBool = false;
        Seat1Team1OpenCloseCheck = false;
        Seat1Team2OpenCloseCheck = false;
        Seat2Team1OpenCloseCheck = false;
        Seat2Team2OpenCloseCheck = false;
        Seat3Team1OpenCloseCheck = false;
        Seat3Team2OpenCloseCheck = false;
        Seat4Team1OpenCloseCheck = false;
        Seat4Team2OpenCloseCheck = false;
        Seat5Team1OpenCloseCheck = false;
        Seat5Team2OpenCloseCheck = false;
        for (int i = 0; i < 5; i++)
        {
            ScoreBoard_Sprite[i].enabled = false;
            ScoreBoard_Animation[i].ResetToBeginning();
            CardMove_End[i] = 0;
            ScoreBoardShow_Start[i] = 0;
        }
        AddCardShow = false;
    //    ScorBoardShowNumber = 1;
    }

    //分牌表演
    void ScoreBoardShow()
    {
        if (ScoreBoardStart[(byte)CardTeamList.Card1Team1] == 1 && ScoreBoardStart[(byte)CardTeamList.Card1Team2] == 1
            && PointCheckStart[(byte)CardTeamList.Card1Team1] == 0 && PointCheckStart[(byte)CardTeamList.Card1Team2] == 0
            && PointFinallSeatStart[(byte)CardTeamList.Card1Team1] == 0 && PointFinallSeatStart[(byte)CardTeamList.Card1Team2] == 0 && !AddCardShow)
        {
            ScoreBoard_TP[(byte)CardTeamList.Card1Team1].PlayForward();
            ScoreBoard_TP[(byte)CardTeamList.Card1Team2].PlayForward();
            ScoreBoard_Sprite[0].enabled = true;
            ScoreBoard_Animation[0].enabled = true;
            ScoreBoard_Animation[0].Play();
            //   ScoreBoardShow_Start[0] = 1;
            AddCardShow = true;
            AddCard1Show = true;
            AddCard2Show = true;
        }
        else if (ScoreBoardStart[(byte)CardTeamList.Card2Team1] == 1 && ScoreBoardStart[(byte)CardTeamList.Card2Team2] == 1
                && PointCheckStart[(byte)CardTeamList.Card2Team1] == 0 && PointCheckStart[(byte)CardTeamList.Card2Team2] == 0
            && PointFinallSeatStart[(byte)CardTeamList.Card2Team1] == 0 && PointFinallSeatStart[(byte)CardTeamList.Card2Team2] == 0 && !AddCardShow)
        {
            ScoreBoard_TP[(byte)CardTeamList.Card2Team1].PlayForward();
            ScoreBoard_TP[(byte)CardTeamList.Card2Team2].PlayForward();
            ScoreBoard_Sprite[1].enabled = true;
            ScoreBoard_Animation[1].enabled = true;
            ScoreBoard_Animation[1].Play();
            //     ScoreBoardShow_Start[1] = 1;
            AddCardShow = true;
            AddCard1Show = true;
            AddCard2Show = true;
        }
        else if (ScoreBoardStart[(byte)CardTeamList.Card3Team1] == 1 && ScoreBoardStart[(byte)CardTeamList.Card3Team2] == 1
                && PointCheckStart[(byte)CardTeamList.Card3Team1] == 0 && PointCheckStart[(byte)CardTeamList.Card3Team2] == 0
            && PointFinallSeatStart[(byte)CardTeamList.Card3Team1] == 0 && PointFinallSeatStart[(byte)CardTeamList.Card3Team2] == 0 && !AddCardShow)
        {
            ScoreBoard_TP[(byte)CardTeamList.Card3Team1].PlayForward();
            ScoreBoard_TP[(byte)CardTeamList.Card3Team2].PlayForward();
            ScoreBoard_Sprite[2].enabled = true;
            ScoreBoard_Animation[2].enabled = true;
            ScoreBoard_Animation[2].Play();
            //  ScoreBoardShow_Start[2] = 1;
            AddCardShow = true;
            AddCard1Show = true;
            AddCard2Show = true;
        }
        else if (ScoreBoardStart[(byte)CardTeamList.Card4Team1] == 1 && ScoreBoardStart[(byte)CardTeamList.Card4Team2] == 1
                && PointCheckStart[(byte)CardTeamList.Card4Team1] == 0 && PointCheckStart[(byte)CardTeamList.Card4Team2] == 0
            && PointFinallSeatStart[(byte)CardTeamList.Card4Team1] == 0 && PointFinallSeatStart[(byte)CardTeamList.Card4Team2] == 0 && !AddCardShow)
        {
            ScoreBoard_TP[(byte)CardTeamList.Card4Team1].PlayForward();
            ScoreBoard_TP[(byte)CardTeamList.Card4Team2].PlayForward();
            ScoreBoard_Sprite[3].enabled = true;
            ScoreBoard_Animation[3].enabled = true;
            ScoreBoard_Animation[3].Play();
            //    ScoreBoardShow_Start[3] = 1;
            AddCardShow = true;
            AddCard1Show = true;
            AddCard2Show = true;
        }
        else if (ScoreBoardStart[(byte)CardTeamList.Card5Team1] == 1 && ScoreBoardStart[(byte)CardTeamList.Card5Team2] == 1
                && PointCheckStart[(byte)CardTeamList.Card5Team1] == 0 && PointCheckStart[(byte)CardTeamList.Card5Team2] == 0
            && PointFinallSeatStart[(byte)CardTeamList.Card5Team1] == 0 && PointFinallSeatStart[(byte)CardTeamList.Card5Team2] == 0 && !AddCardShow)
        {
            ScoreBoard_TP[(byte)CardTeamList.Card5Team1].PlayForward();
            ScoreBoard_TP[(byte)CardTeamList.Card5Team2].PlayForward();
            ScoreBoard_Sprite[4].enabled = true;
            ScoreBoard_Animation[4].enabled = true;
            ScoreBoard_Animation[4].Play();
            //  ScoreBoardShow_Start[4] = 1;
            AddCardShow = true;
            AddCard1Show = true;
            AddCard2Show = true;
        }

    }

    //補牌表演
    void CardShow()
    {
        if (AddCardShow)
        {
            if (ScoreBoardStart[(byte)CardTeamList.Card1Team1] == 0 && ScoreBoardStart[(byte)CardTeamList.Card1Team2] == 0
                && ScoreBoardStart[(byte)CardTeamList.Card2Team1] == 0 && ScoreBoardStart[(byte)CardTeamList.Card2Team2] == 0
                 && ScoreBoardStart[(byte)CardTeamList.Card3Team1] == 0 && ScoreBoardStart[(byte)CardTeamList.Card3Team2] == 0
                  && ScoreBoardStart[(byte)CardTeamList.Card4Team1] == 0 && ScoreBoardStart[(byte)CardTeamList.Card4Team2] == 0
                   && ScoreBoardStart[(byte)CardTeamList.Card5Team1] == 0 && ScoreBoardStart[(byte)CardTeamList.Card5Team2] == 0 && (AddCard1Show || AddCard2Show))
            {
                CardTeamList SaveList = CardTeamList.Card1Team1;
                byte SaveCardNumber = 0;
                foreach (var item in AddCard)
                {
                    if (AddCard1Show && AddCard2Show)
                    {
                        if (item.Key == CardTeamList.Card1Team1 || item.Key == CardTeamList.Card2Team1 || item.Key == CardTeamList.Card3Team1 || item.Key == CardTeamList.Card4Team1 || item.Key == CardTeamList.Card5Team1)
                        {
                            SaveList = item.Key;
                            SaveCardNumber = (byte)item.Value;
                        }
                            
                    }
                    else if (!AddCard1Show && AddCard2Show)
                    {
                        if (item.Key == CardTeamList.Card1Team2 || item.Key == CardTeamList.Card2Team2 || item.Key == CardTeamList.Card3Team2 || item.Key == CardTeamList.Card4Team2 || item.Key == CardTeamList.Card5Team2)
                        {
                            SaveList = item.Key;
                            SaveCardNumber = (byte)item.Value;
                        }
                    }
                }
                DealerCard_Move.CardNumber = SaveCardNumber;

                if (SaveList == CardTeamList.Card1Team1)
                    Seat1Team1.Add(SaveCardNumber);
                else if (SaveList == CardTeamList.Card1Team2)
                    Seat1Team2.Add(SaveCardNumber);
                if (SaveList == CardTeamList.Card2Team1)
                    Seat2Team1.Add(SaveCardNumber);
                else if (SaveList == CardTeamList.Card2Team2)
                    Seat2Team2.Add(SaveCardNumber);
                if (SaveList == CardTeamList.Card3Team1)
                    Seat3Team1.Add(SaveCardNumber);
                else if (SaveList == CardTeamList.Card3Team2)
                    Seat3Team2.Add(SaveCardNumber);
                if (SaveList == CardTeamList.Card4Team1)
                    Seat4Team1.Add(SaveCardNumber);
                else if (SaveList == CardTeamList.Card4Team2)
                    Seat4Team2.Add(SaveCardNumber);
                if (SaveList == CardTeamList.Card5Team1)
                    Seat5Team1.Add(SaveCardNumber);
                else if (SaveList == CardTeamList.Card5Team2)
                    Seat5Team2.Add(SaveCardNumber);

                SeatTeamPoint[(byte)SaveList]++;
                SeatTeamAddCheck[(byte)SaveList] = 1;
                NowFinallCardSeat[(byte)SaveList] = 1;
                AddCardShow = false;
            }

            if (!AddCard1Show && !AddCard2Show)
            {
                AddCard.Clear();
                AddCardShow = false;
            }
        }
    }

    //強制表演結束
    void EndShow()
    {
        if (CardMove_End[(byte)CardTeamList.Card1Team1] == 1 && CardMove_End[(byte)CardTeamList.Card1Team2] == 1)
        {
            ScoreBoard_GO[(byte)CardTeamList.Card1Team1].transform.localPosition = ScoreBoard_TP[(byte)CardTeamList.Card1Team1].to;
            ScoreBoard_GO[(byte)CardTeamList.Card1Team2].transform.localPosition = ScoreBoard_TP[(byte)CardTeamList.Card1Team2].to;
            CardMove_End[(byte)CardTeamList.Card1Team1] = 0;
            CardMove_End[(byte)CardTeamList.Card1Team2] = 0;
        }
        if (CardMove_End[(byte)CardTeamList.Card2Team1] == 1 && CardMove_End[(byte)CardTeamList.Card2Team2] == 1)
        {
            ScoreBoard_GO[(byte)CardTeamList.Card2Team1].transform.localPosition = ScoreBoard_TP[(byte)CardTeamList.Card2Team1].to;
            ScoreBoard_GO[(byte)CardTeamList.Card2Team2].transform.localPosition = ScoreBoard_TP[(byte)CardTeamList.Card2Team2].to;
            CardMove_End[(byte)CardTeamList.Card2Team1] = 0;
            CardMove_End[(byte)CardTeamList.Card2Team2] = 0;
        }
        if (CardMove_End[(byte)CardTeamList.Card3Team1] == 1 && CardMove_End[(byte)CardTeamList.Card3Team2] == 1)
        {
            ScoreBoard_GO[(byte)CardTeamList.Card3Team1].transform.localPosition = ScoreBoard_TP[(byte)CardTeamList.Card3Team1].to;
            ScoreBoard_GO[(byte)CardTeamList.Card3Team2].transform.localPosition = ScoreBoard_TP[(byte)CardTeamList.Card3Team2].to;
            CardMove_End[(byte)CardTeamList.Card3Team1] = 0;
            CardMove_End[(byte)CardTeamList.Card3Team2] = 0;
        }
        if (CardMove_End[(byte)CardTeamList.Card4Team1] == 1 && CardMove_End[(byte)CardTeamList.Card4Team2] == 1)
        {
            ScoreBoard_GO[(byte)CardTeamList.Card4Team1].transform.localPosition = ScoreBoard_TP[(byte)CardTeamList.Card4Team1].to;
            ScoreBoard_GO[(byte)CardTeamList.Card4Team2].transform.localPosition = ScoreBoard_TP[(byte)CardTeamList.Card4Team2].to;
            CardMove_End[(byte)CardTeamList.Card4Team1] = 0;
            CardMove_End[(byte)CardTeamList.Card4Team2] = 0;
        }
        if (CardMove_End[(byte)CardTeamList.Card5Team1] == 1 && CardMove_End[(byte)CardTeamList.Card5Team2] == 1)
        {
            ScoreBoard_GO[(byte)CardTeamList.Card5Team1].transform.localPosition = ScoreBoard_TP[(byte)CardTeamList.Card5Team1].to;
            ScoreBoard_GO[(byte)CardTeamList.Card5Team2].transform.localPosition = ScoreBoard_TP[(byte)CardTeamList.Card5Team2].to;
            CardMove_End[(byte)CardTeamList.Card5Team1] = 0;
            CardMove_End[(byte)CardTeamList.Card5Team2] = 0;
        }
    }

    #region ScoreBoard
    //void ScoreBoard()
    //{
    //    if (ShowTime < 0.2f)
    //    {
    //        ShowTime += Time.deltaTime;
    //    }
    //    else
    //    {
    //        if (ScorBoardShowNumber < 9)
    //        {
    //            if (ScoreBoardShow_Start[0] == 1)
    //            {
    //                ScoreBoard_Sprite[0].spriteName = "";
    //            }
    //            else if (ScoreBoardShow_Start[1] == 1)
    //            {
    //                ScoreBoard_Sprite[1].spriteName = "";
    //            }
    //            else if (ScoreBoardShow_Start[2] == 1)
    //            {
    //                ScoreBoard_Sprite[2].spriteName = "";
    //            }
    //            else if (ScoreBoardShow_Start[3] == 1)
    //            {
    //                ScoreBoard_Sprite[3].spriteName = "";
    //            }
    //            else if (ScoreBoardShow_Start[4] == 1)
    //            {
    //                ScoreBoard_Sprite[4].spriteName = "";
    //            }
    //            ScorBoardShowNumber++;
    //        }
    //        else
    //        {
    //            ScorBoardShowNumber = 1;
    //            ScoreBoardShow_Start[0] = 0;
    //            ScoreBoardShow_Start[1] = 0;
    //            ScoreBoardShow_Start[2] = 0;
    //            ScoreBoardShow_Start[3] = 0;
    //            ScoreBoardShow_Start[4] = 0;
    //        }
    //    }
    //}
    #endregion
}
