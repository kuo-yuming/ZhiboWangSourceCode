using UnityEngine;
using System.Collections;
using CardTeamListClass;
using WinLoseListClass;
using GameCore.Manager.BlackJack;

public class Point_Control : MonoBehaviour {

    public CardTeamList CardTeam_Control;

    public UISprite Tip1_Sprite;
    public UISprite Tip2_Sprite;
    public UILabel Tip1_Label;
    public UILabel Tip2_Label;

    // Use this for initialization
    void Start () {
        Tip1_Sprite.enabled = false;
        Tip1_Label.enabled = false;
        Tip2_Sprite.enabled = false;
        Tip2_Label.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        PointCheckStart();

        if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.NewRound
           || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.ShuffleNewRound
           || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.WaitBet
           || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.GameOver || StateShow_Control.Backgrond_Bool)
        {
            PointInit();
        }
    }

    //點數計算
    #region PointCheck
    void PointCheck()
    {
        if (CardTeam_Control == CardTeamList.Card1Team1)
        {
            #region Card1Team1
            byte Point1 = 0;
            byte Point2 = 0;

            for (int i = 0; i < BJCard_Control.Seat1Team1.Count; i++)
            {
                if ((((BJCard_Control.Seat1Team1[i] - 1) % 13) + 1) == 1)
                {
                    Point1 += 1;
                    if (Point2 < 11)
                        Point2 += 11;
                    else
                        Point2 += 1;
                }
                else
                {
                    if ((((BJCard_Control.Seat1Team1[i] - 1) % 13) + 1) > 10)
                    {
                        Point1 += 10;
                        Point2 += 10;
                    }
                    else
                    {
                        Point1 += (byte)(((BJCard_Control.Seat1Team1[i] - 1) % 13) + 1);
                        Point2 += (byte)(((BJCard_Control.Seat1Team1[i] - 1) % 13) + 1);
                    }
                }
            }

            if (Point2 >= 11 && Point2 <= 21 && Point1 != Point2)
            {
                Tip1_Sprite.enabled = false;
                Tip1_Label.enabled = false;
                Tip2_Sprite.enabled = true;
                Tip2_Label.enabled = true;
            }
            else
            {
                Tip1_Sprite.enabled = true;
                Tip1_Label.enabled = true;
                Tip2_Sprite.enabled = false;
                Tip2_Label.enabled = false;
            }
            Tip1_Label.text = Point1.ToString();
            Tip2_Label.text = Point1.ToString() + "/" + Point2.ToString();

            if (Point1 > 21)
            {
                BJMainGame_Control.TableWinLose[CardTeamList.Card1Team1] = WinLoseList.PointOut;
                if (BJCard_Control.Seat1Team2.Count != 0)
                {
                    BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card1Team1] = 1;                  
                }
                else
                {
                    BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.CardBanker] = 1;
                    BJMainGame_Control.MainWinLoseShow = true;
                    BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState = ENUM_BLACKJACK_TABLE_STATE.PlayerOver;
                }

                if (BJMainGame_Control.CardSenceList_Bool && CardTeam_Control == CardTeamList.Card1Team1)
                {
                    BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState = ENUM_BLACKJACK_TABLE_STATE.CheckPlayer;
                    BJMainGame_Control.WaitAngle = true;
                    BJMainGame_Control.CardSenceList_Bool = false;
                }
            }
            BJMainGame_Control.NowCardPoint = Point1;
            BJMainGame_Control.NowCardPoint2 = Point2;
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card1Team1] = 0;
            #endregion
        }
        else if (CardTeam_Control == CardTeamList.Card1Team2)
        {
            #region Card1Team2
            byte Point1 = 0;
            byte Point2 = 0;

            for (int i = 0; i < BJCard_Control.Seat1Team2.Count; i++)
            {
                if ((((BJCard_Control.Seat1Team2[i] - 1) % 13) + 1) == 1)
                {
                    Point1 += 1;
                    if (Point2 < 11)
                        Point2 += 11;
                    else
                        Point2 += 1;
                }
                else
                {
                    if ((((BJCard_Control.Seat1Team2[i] - 1) % 13) + 1) > 10)
                    {
                        Point1 += 10;
                        Point2 += 10;
                    }
                    else
                    {
                        Point1 += (byte)(((BJCard_Control.Seat1Team2[i] - 1) % 13) + 1);
                        Point2 += (byte)(((BJCard_Control.Seat1Team2[i] - 1) % 13) + 1);
                    }
                }
            }

            if (Point2 >= 11 && Point2 <= 21 && Point1 != Point2)
            {
                Tip1_Sprite.enabled = false;
                Tip1_Label.enabled = false;
                Tip2_Sprite.enabled = true;
                Tip2_Label.enabled = true;
            }
            else
            {
                Tip1_Sprite.enabled = true;
                Tip1_Label.enabled = true;
                Tip2_Sprite.enabled = false;
                Tip2_Label.enabled = false;
            }

            if (Point1 > 21)
            {
                BJMainGame_Control.TableWinLose[CardTeamList.Card1Team2] = WinLoseList.PointOut;
                BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card1Team2] = 1;
                BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState = ENUM_BLACKJACK_TABLE_STATE.PlayerOver;
            }
            

            Tip1_Label.text = Point1.ToString();
            Tip2_Label.text = Point1.ToString() + "/" + Point2.ToString();
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card1Team2] = 0;
            #endregion
        }
        else if (CardTeam_Control == CardTeamList.Card2Team1)
        {
            #region Card2Team1
            byte Point1 = 0;
            byte Point2 = 0;

            for (int i = 0; i < BJCard_Control.Seat2Team1.Count; i++)
            {
                if ((((BJCard_Control.Seat2Team1[i] - 1) % 13) + 1) == 1)
                {
                    Point1 += 1;
                    if (Point2 < 11)
                        Point2 += 11;
                    else
                        Point2 += 1;
                }
                else
                {
                    if ((((BJCard_Control.Seat2Team1[i] - 1) % 13) + 1) > 10)
                    {
                        Point1 += 10;
                        Point2 += 10;
                    }
                    else
                    {
                        Point1 += (byte)(((BJCard_Control.Seat2Team1[i] - 1) % 13) + 1);
                        Point2 += (byte)(((BJCard_Control.Seat2Team1[i] - 1) % 13) + 1);
                    }
                }
            }

            if (Point2 >= 11 && Point2 <= 21 && Point1 != Point2)
            {
                Tip1_Sprite.enabled = false;
                Tip1_Label.enabled = false;
                Tip2_Sprite.enabled = true;
                Tip2_Label.enabled = true;
            }
            else
            {
                Tip1_Sprite.enabled = true;
                Tip1_Label.enabled = true;
                Tip2_Sprite.enabled = false;
                Tip2_Label.enabled = false;
            }

            if (Point1 > 21)
            {
                BJMainGame_Control.TableWinLose[CardTeamList.Card2Team1] = WinLoseList.PointOut;
                BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card2Team1] = 1;
            }

            Tip1_Label.text = Point1.ToString();
            Tip2_Label.text = Point1.ToString() + "/" + Point2.ToString();
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card2Team1] = 0;
            #endregion
        }
        else if (CardTeam_Control == CardTeamList.Card2Team2)
        {
            #region Card2Team2
            byte Point1 = 0;
            byte Point2 = 0;

            for (int i = 0; i < BJCard_Control.Seat2Team2.Count; i++)
            {
                if ((((BJCard_Control.Seat2Team2[i] - 1) % 13) + 1) == 1)
                {
                    Point1 += 1;
                    if (Point2 < 11)
                        Point2 += 11;
                    else
                        Point2 += 1;
                }
                else
                {
                    if ((((BJCard_Control.Seat2Team2[i] - 1) % 13) + 1) > 10)
                    {
                        Point1 += 10;
                        Point2 += 10;
                    }
                    else
                    {
                        Point1 += (byte)(((BJCard_Control.Seat2Team2[i] - 1) % 13) + 1);
                        Point2 += (byte)(((BJCard_Control.Seat2Team2[i] - 1) % 13) + 1);
                    }
                }
            }

            if (Point2 >= 11 && Point2 <= 21 && Point1 != Point2)
            {
                Tip1_Sprite.enabled = false;
                Tip1_Label.enabled = false;
                Tip2_Sprite.enabled = true;
                Tip2_Label.enabled = true;
            }
            else
            {
                Tip1_Sprite.enabled = true;
                Tip1_Label.enabled = true;
                Tip2_Sprite.enabled = false;
                Tip2_Label.enabled = false;
            }

            if (Point1 > 21)
            {
                BJMainGame_Control.TableWinLose[CardTeamList.Card2Team2] = WinLoseList.PointOut;
                BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card2Team2] = 1;
            }

            Tip1_Label.text = Point1.ToString();
            Tip2_Label.text = Point1.ToString() + "/" + Point2.ToString();
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card2Team2] = 0;
            #endregion
        }
        else if (CardTeam_Control == CardTeamList.Card3Team1)
        {
            #region Card3Team1
            byte Point1 = 0;
            byte Point2 = 0;

            for (int i = 0; i < BJCard_Control.Seat3Team1.Count; i++)
            {
                if ((((BJCard_Control.Seat3Team1[i] - 1) % 13) + 1) == 1)
                {
                    Point1 += 1;
                    if (Point2 < 11)
                        Point2 += 11;
                    else
                        Point2 += 1;
                }
                else
                {
                    if ((((BJCard_Control.Seat3Team1[i] - 1) % 13) + 1) > 10)
                    {
                        Point1 += 10;
                        Point2 += 10;
                    }
                    else
                    {
                        Point1 += (byte)(((BJCard_Control.Seat3Team1[i] - 1) % 13) + 1);
                        Point2 += (byte)(((BJCard_Control.Seat3Team1[i] - 1) % 13) + 1);
                    }
                }
            }

            if (Point2 >= 11 && Point2 <= 21 && Point1 != Point2)
            {
                Tip1_Sprite.enabled = false;
                Tip1_Label.enabled = false;
                Tip2_Sprite.enabled = true;
                Tip2_Label.enabled = true;
            }
            else
            {
                Tip1_Sprite.enabled = true;
                Tip1_Label.enabled = true;
                Tip2_Sprite.enabled = false;
                Tip2_Label.enabled = false;
            }

            if (Point1 > 21)
            {
                BJMainGame_Control.TableWinLose[CardTeamList.Card3Team1] = WinLoseList.PointOut;
                BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card3Team1] = 1;
            }

            Tip1_Label.text = Point1.ToString();
            Tip2_Label.text = Point1.ToString() + "/" + Point2.ToString();
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card3Team1] = 0;
            #endregion
        }
        else if (CardTeam_Control == CardTeamList.Card3Team2)
        {
            #region Card3Team2
            byte Point1 = 0;
            byte Point2 = 0;

            for (int i = 0; i < BJCard_Control.Seat3Team2.Count; i++)
            {
                if ((((BJCard_Control.Seat3Team2[i] - 1) % 13) + 1) == 1)
                {
                    Point1 += 1;
                    if (Point2 < 11)
                        Point2 += 11;
                    else
                        Point2 += 1;
                }
                else
                {
                    if ((((BJCard_Control.Seat3Team2[i] - 1) % 13) + 1) > 10)
                    {
                        Point1 += 10;
                        Point2 += 10;
                    }
                    else
                    {
                        Point1 += (byte)(((BJCard_Control.Seat3Team2[i] - 1) % 13) + 1);
                        Point2 += (byte)(((BJCard_Control.Seat3Team2[i] - 1) % 13) + 1);
                    }
                }
            }

            if (Point2 >= 11 && Point2 <= 21 && Point1 != Point2)
            {
                Tip1_Sprite.enabled = false;
                Tip1_Label.enabled = false;
                Tip2_Sprite.enabled = true;
                Tip2_Label.enabled = true;
            }
            else
            {
                Tip1_Sprite.enabled = true;
                Tip1_Label.enabled = true;
                Tip2_Sprite.enabled = false;
                Tip2_Label.enabled = false;
            }

            if (Point1 > 21)
            {
                BJMainGame_Control.TableWinLose[CardTeamList.Card3Team2] = WinLoseList.PointOut;
                BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card3Team2] = 1;
            }

            Tip1_Label.text = Point1.ToString();
            Tip2_Label.text = Point1.ToString() + "/" + Point2.ToString();
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card3Team2] = 0;
            #endregion
        }
        else if (CardTeam_Control == CardTeamList.Card4Team1)
        {
            #region Card4Team1
            byte Point1 = 0;
            byte Point2 = 0;

            for (int i = 0; i < BJCard_Control.Seat4Team1.Count; i++)
            {
                if ((((BJCard_Control.Seat4Team1[i] - 1) % 13) + 1) == 1)
                {
                    Point1 += 1;
                    if (Point2 < 11)
                        Point2 += 11;
                    else
                        Point2 += 1;
                }
                else
                {
                    if ((((BJCard_Control.Seat4Team1[i] - 1) % 13) + 1) > 10)
                    {
                        Point1 += 10;
                        Point2 += 10;
                    }
                    else
                    {
                        Point1 += (byte)(((BJCard_Control.Seat4Team1[i] - 1) % 13) + 1);
                        Point2 += (byte)(((BJCard_Control.Seat4Team1[i] - 1) % 13) + 1);
                    }
                }
            }

            if (Point2 >= 11 && Point2 <= 21 && Point1 != Point2)
            {
                Tip1_Sprite.enabled = false;
                Tip1_Label.enabled = false;
                Tip2_Sprite.enabled = true;
                Tip2_Label.enabled = true;
            }
            else
            {
                Tip1_Sprite.enabled = true;
                Tip1_Label.enabled = true;
                Tip2_Sprite.enabled = false;
                Tip2_Label.enabled = false;
            }

            if (Point1 > 21)
            {
                BJMainGame_Control.TableWinLose[CardTeamList.Card4Team1] = WinLoseList.PointOut;
                BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card4Team1] = 1;
            }

            Tip1_Label.text = Point1.ToString();
            Tip2_Label.text = Point1.ToString() + "/" + Point2.ToString();
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card4Team1] = 0;
            #endregion
        }
        else if (CardTeam_Control == CardTeamList.Card4Team2)
        {
            #region Card4Team2
            byte Point1 = 0;
            byte Point2 = 0;

            for (int i = 0; i < BJCard_Control.Seat4Team2.Count; i++)
            {
                if ((((BJCard_Control.Seat4Team2[i] - 1) % 13) + 1) == 1)
                {
                    Point1 += 1;
                    if (Point2 < 11)
                        Point2 += 11;
                    else
                        Point2 += 1;
                }
                else
                {
                    if ((((BJCard_Control.Seat4Team2[i] - 1) % 13) + 1) > 10)
                    {
                        Point1 += 10;
                        Point2 += 10;
                    }
                    else
                    {
                        Point1 += (byte)(((BJCard_Control.Seat4Team2[i] - 1) % 13) + 1);
                        Point2 += (byte)(((BJCard_Control.Seat4Team2[i] - 1) % 13) + 1);
                    }
                }
            }

            if (Point2 >= 11 && Point2 <= 21 && Point1 != Point2)
            {
                Tip1_Sprite.enabled = false;
                Tip1_Label.enabled = false;
                Tip2_Sprite.enabled = true;
                Tip2_Label.enabled = true;
            }
            else
            {
                Tip1_Sprite.enabled = true;
                Tip1_Label.enabled = true;
                Tip2_Sprite.enabled = false;
                Tip2_Label.enabled = false;
            }

            if (Point1 > 21)
            {
                BJMainGame_Control.TableWinLose[CardTeamList.Card4Team2] = WinLoseList.PointOut;
                BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card4Team2] = 1;
            }

            Tip1_Label.text = Point1.ToString();
            Tip2_Label.text = Point1.ToString() + "/" + Point2.ToString();
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card4Team2] = 0;
            #endregion
        }
        else if (CardTeam_Control == CardTeamList.Card5Team1)
        {
            #region Card5Team1
            byte Point1 = 0;
            byte Point2 = 0;

            for (int i = 0; i < BJCard_Control.Seat5Team1.Count; i++)
            {
                if ((((BJCard_Control.Seat5Team1[i] - 1) % 13) + 1) == 1)
                {
                    Point1 += 1;
                    if (Point2 < 11)
                        Point2 += 11;
                    else
                        Point2 += 1;
                }
                else
                {
                    if ((((BJCard_Control.Seat5Team1[i] - 1) % 13) + 1) > 10)
                    {
                        Point1 += 10;
                        Point2 += 10;
                    }
                    else
                    {
                        Point1 += (byte)(((BJCard_Control.Seat5Team1[i] - 1) % 13) + 1);
                        Point2 += (byte)(((BJCard_Control.Seat5Team1[i] - 1) % 13) + 1);
                    }
                }
            }

            if (Point2 >= 11 && Point2 <= 21 && Point1 != Point2)
            {
                Tip1_Sprite.enabled = false;
                Tip1_Label.enabled = false;
                Tip2_Sprite.enabled = true;
                Tip2_Label.enabled = true;
            }
            else
            {
                Tip1_Sprite.enabled = true;
                Tip1_Label.enabled = true;
                Tip2_Sprite.enabled = false;
                Tip2_Label.enabled = false;
            }

            if (Point1 > 21)
            {
                BJMainGame_Control.TableWinLose[CardTeamList.Card5Team1] = WinLoseList.PointOut;
                BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card5Team1] = 1;
            }

            Tip1_Label.text = Point1.ToString();
            Tip2_Label.text = Point1.ToString() + "/" + Point2.ToString();
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card5Team1] = 0;
            #endregion
        }
        else if (CardTeam_Control == CardTeamList.Card5Team2)
        {
            #region Card5Team2
            byte Point1 = 0;
            byte Point2 = 0;

            for (int i = 0; i < BJCard_Control.Seat5Team2.Count; i++)
            {
                if ((((BJCard_Control.Seat5Team2[i] - 1) % 13) + 1) == 1)
                {
                    Point1 += 1;
                    if (Point2 < 11)
                        Point2 += 11;
                    else
                        Point2 += 1;
                }
                else
                {
                    if ((((BJCard_Control.Seat5Team2[i] - 1) % 13) + 1) > 10)
                    {
                        Point1 += 10;
                        Point2 += 10;
                    }
                    else
                    {
                        Point1 += (byte)(((BJCard_Control.Seat5Team2[i] - 1) % 13) + 1);
                        Point2 += (byte)(((BJCard_Control.Seat5Team2[i] - 1) % 13) + 1);
                    }
                }
            }

            if (Point2 >= 11 && Point2 <= 21 && Point1 != Point2)
            {
                Tip1_Sprite.enabled = false;
                Tip1_Label.enabled = false;
                Tip2_Sprite.enabled = true;
                Tip2_Label.enabled = true;
            }
            else
            {
                Tip1_Sprite.enabled = true;
                Tip1_Label.enabled = true;
                Tip2_Sprite.enabled = false;
                Tip2_Label.enabled = false;
            }

            if (Point1 > 21)
            {
                BJMainGame_Control.TableWinLose[CardTeamList.Card5Team2] = WinLoseList.PointOut;
                BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card5Team2] = 1;
            }

            Tip1_Label.text = Point1.ToString();
            Tip2_Label.text = Point1.ToString() + "/" + Point2.ToString();
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card5Team2] = 0;
            #endregion
        }
        else if (CardTeam_Control == CardTeamList.CardBanker)
        {
            #region SeatBanker
            byte Point1 = 0;
            byte Point2 = 0;

            for (int i = 0; i < BJCard_Control.SeatBanker.Count; i++)
            {
                if ((((BJCard_Control.SeatBanker[i] - 1) % 13) + 1) == 1)
                {
                    Point1 += 1;
                    if (Point2 < 11)
                        Point2 += 11;
                    else
                        Point2 += 1;
                }
                else
                {
                    if ((((BJCard_Control.SeatBanker[i] - 1) % 13) + 1) > 10)
                    {
                        Point1 += 10;
                        Point2 += 10;
                    }
                    else
                    {
                        Point1 += (byte)(((BJCard_Control.SeatBanker[i] - 1) % 13) + 1);
                        Point2 += (byte)(((BJCard_Control.SeatBanker[i] - 1) % 13) + 1);
                    }
                }
            }

            if (Point2 >= 11 && Point2 <= 21 && Point1 != Point2)
            {
                Tip1_Sprite.enabled = false;
                Tip1_Label.enabled = false;
                Tip2_Sprite.enabled = true;
                Tip2_Label.enabled = true;
            }
            else
            {
                Tip1_Sprite.enabled = true;
                Tip1_Label.enabled = true;
                Tip2_Sprite.enabled = false;
                Tip2_Label.enabled = false;
            }
            Tip1_Label.text = Point1.ToString();
            Tip2_Label.text = Point1.ToString() + "/" + Point2.ToString();
            BJCard_Control.PointCheckStart[(byte)CardTeamList.CardBanker] = 0;
            #endregion
        }
    }
    #endregion

    //Init
    void PointInit()
    {
        byte Point1 = 0;
        byte Point2 = 0;
        Tip1_Sprite.enabled = false;
        Tip1_Label.enabled = false;
        Tip2_Sprite.enabled = false;
        Tip2_Label.enabled = false;
        Tip1_Label.text = Point1.ToString();
        Tip2_Label.text = Point1.ToString() + "/" + Point2.ToString();
        //if (CardTeam_Control == CardTeamList.Card1Team1)
        //{
        //    #region Card1Team1
        //    byte Point1 = 0;
        //    byte Point2 = 0;
        //    Tip1_Sprite.enabled = false;
        //    Tip1_Label.enabled = false;
        //    Tip2_Sprite.enabled = true;
        //    Tip2_Label.enabled = true;
        //    Tip1_Label.text = Point1.ToString();
        //    Tip2_Label.text = Point1.ToString() + "/" + Point2.ToString();
        //    #endregion
        //}
        //else if (CardTeam_Control == CardTeamList.Card1Team2)
        //{
        //    #region Card1Team2
        //    byte Point1 = 0;
        //    byte Point2 = 0;
        //    Tip1_Sprite.enabled = false;
        //    Tip1_Label.enabled = false;
        //    Tip2_Sprite.enabled = true;
        //    Tip2_Label.enabled = true;
        //    Tip1_Label.text = Point1.ToString();
        //    Tip2_Label.text = Point1.ToString() + "/" + Point2.ToString();
        //    #endregion
        //}
        //else if (CardTeam_Control == CardTeamList.Card2Team1)
        //{
        //    #region Card2Team1
        //    byte Point1 = 0;
        //    byte Point2 = 0;
        //    Tip1_Sprite.enabled = false;
        //    Tip1_Label.enabled = false;
        //    Tip2_Sprite.enabled = true;
        //    Tip2_Label.enabled = true;
        //    Tip1_Label.text = Point1.ToString();
        //    Tip2_Label.text = Point1.ToString() + "/" + Point2.ToString();
        //    #endregion
        //}
        //else if (CardTeam_Control == CardTeamList.Card2Team2)
        //{
        //    #region Card2Team2
        //    byte Point1 = 0;
        //    byte Point2 = 0;
        //    Tip1_Sprite.enabled = false;
        //    Tip1_Label.enabled = false;
        //    Tip2_Sprite.enabled = true;
        //    Tip2_Label.enabled = true;
        //    Tip1_Label.text = Point1.ToString();
        //    Tip2_Label.text = Point1.ToString() + "/" + Point2.ToString();
        //    #endregion
        //}
        //else if (CardTeam_Control == CardTeamList.Card3Team1)
        //{
        //    #region Card3Team1
        //    byte Point1 = 0;
        //    byte Point2 = 0;
        //    Tip1_Sprite.enabled = false;
        //    Tip1_Label.enabled = false;
        //    Tip2_Sprite.enabled = true;
        //    Tip2_Label.enabled = true;
        //    Tip1_Label.text = Point1.ToString();
        //    Tip2_Label.text = Point1.ToString() + "/" + Point2.ToString();
        //    #endregion
        //}
        //else if (CardTeam_Control == CardTeamList.Card3Team2)
        //{
        //    #region Card3Team2
        //    byte Point1 = 0;
        //    byte Point2 = 0;
        //    Tip1_Sprite.enabled = false;
        //    Tip1_Label.enabled = false;
        //    Tip2_Sprite.enabled = true;
        //    Tip2_Label.enabled = true;
        //    Tip1_Label.text = Point1.ToString();
        //    Tip2_Label.text = Point1.ToString() + "/" + Point2.ToString();
        //    #endregion
        //}
        //else if (CardTeam_Control == CardTeamList.Card4Team1)
        //{
        //    #region Card4Team1
        //    byte Point1 = 0;
        //    byte Point2 = 0;
        //    Tip1_Sprite.enabled = false;
        //    Tip1_Label.enabled = false;
        //    Tip2_Sprite.enabled = true;
        //    Tip2_Label.enabled = true;
        //    Tip1_Label.text = Point1.ToString();
        //    Tip2_Label.text = Point1.ToString() + "/" + Point2.ToString();
        //    #endregion
        //}
        //else if (CardTeam_Control == CardTeamList.Card4Team2)
        //{
        //    #region Card4Team2
        //    byte Point1 = 0;
        //    byte Point2 = 0;
        //    Tip1_Sprite.enabled = false;
        //    Tip1_Label.enabled = false;
        //    Tip2_Sprite.enabled = true;
        //    Tip2_Label.enabled = true;
        //    Tip1_Label.text = Point1.ToString();
        //    Tip2_Label.text = Point1.ToString() + "/" + Point2.ToString();
        //    #endregion
        //}
        //else if (CardTeam_Control == CardTeamList.Card5Team1)
        //{
        //    #region Card5Team1
        //    byte Point1 = 0;
        //    byte Point2 = 0;
        //    Tip1_Sprite.enabled = false;
        //    Tip1_Label.enabled = false;
        //    Tip2_Sprite.enabled = true;
        //    Tip2_Label.enabled = true;
        //    Tip1_Label.text = Point1.ToString();
        //    Tip2_Label.text = Point1.ToString() + "/" + Point2.ToString();
        //    #endregion
        //}
        //else if (CardTeam_Control == CardTeamList.Card5Team2)
        //{
        //    #region Card5Team2
        //    byte Point1 = 0;
        //    byte Point2 = 0;
        //    Tip1_Sprite.enabled = false;
        //    Tip1_Label.enabled = false;
        //    Tip2_Sprite.enabled = true;
        //    Tip2_Label.enabled = true;
        //    Tip1_Label.text = Point1.ToString();
        //    Tip2_Label.text = Point1.ToString() + "/" + Point2.ToString();
        //    #endregion
        //}
        //else if (CardTeam_Control == CardTeamList.CardBanker)
        //{
        //    #region SeatBanker
        //    byte Point1 = 0;
        //    byte Point2 = 0;
        //    Tip1_Sprite.enabled = false;
        //    Tip1_Label.enabled = false;
        //    Tip2_Sprite.enabled = true;
        //    Tip2_Label.enabled = true;
        //    Tip1_Label.text = Point1.ToString();
        //    Tip2_Label.text = Point1.ToString() + "/" + Point2.ToString();
        //    #endregion
        //}
    }

    //其他計算
    void PointCheckStart()
    {
        if (CardTeam_Control == CardTeamList.Card1Team1 && BJCard_Control.PointCheckStart[(byte)CardTeamList.Card1Team1] == 1)
            PointCheck();
        if (CardTeam_Control == CardTeamList.Card1Team2 && BJCard_Control.PointCheckStart[(byte)CardTeamList.Card1Team2] == 1)
            PointCheck();
        if (CardTeam_Control == CardTeamList.Card2Team1 && BJCard_Control.PointCheckStart[(byte)CardTeamList.Card2Team1] == 1)
            PointCheck();
        if (CardTeam_Control == CardTeamList.Card2Team2 && BJCard_Control.PointCheckStart[(byte)CardTeamList.Card2Team2] == 1)
            PointCheck();
        if (CardTeam_Control == CardTeamList.Card3Team1 && BJCard_Control.PointCheckStart[(byte)CardTeamList.Card3Team1] == 1)
            PointCheck();
        if (CardTeam_Control == CardTeamList.Card3Team2 && BJCard_Control.PointCheckStart[(byte)CardTeamList.Card3Team2] == 1)
            PointCheck();
        if (CardTeam_Control == CardTeamList.Card4Team1 && BJCard_Control.PointCheckStart[(byte)CardTeamList.Card4Team1] == 1)
            PointCheck();
        if (CardTeam_Control == CardTeamList.Card4Team2 && BJCard_Control.PointCheckStart[(byte)CardTeamList.Card4Team2] == 1)
            PointCheck();
        if (CardTeam_Control == CardTeamList.Card5Team1 && BJCard_Control.PointCheckStart[(byte)CardTeamList.Card5Team1] == 1)
            PointCheck();
        if (CardTeam_Control == CardTeamList.Card5Team2 && BJCard_Control.PointCheckStart[(byte)CardTeamList.Card5Team2] == 1)
            PointCheck();
        if (CardTeam_Control == CardTeamList.CardBanker && BJCard_Control.PointCheckStart[(byte)CardTeamList.CardBanker] == 1)
            PointCheck();
    }
}
