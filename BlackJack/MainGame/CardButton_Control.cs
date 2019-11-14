using UnityEngine;
using System.Collections;
using CardTeamListClass;
using GameCore.Manager.BlackJack;

public class CardButton_Control : MonoBehaviour {

    public UISprite CardSprite;
    public CardTeamList NowCardTeam = CardTeamList.Card1Team1;
    public byte BankerCardPoint = 0;
    bool FinallOpen = false;
    float DelayTimer = 0;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Card_Move_Control.ShowOK)
        {
            if (!CardSprite.enabled)
            {
                ThisTeam();
                CardSprite.enabled = true;
                Card_Move_Control.ShowOK = false;
            }
        }

        //莊家第二張牌
        if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.PlayerOver && NowCardTeam == CardTeamList.CardBanker
            && BJMainGame_Control.BankerFinallData_Bool && BJCard_Control.SeatBanker[1] != 0 && BJMainGame_Control.FirstCardShow_Bool && BankerCardPoint == 0)
        {
            if (!FinallOpen)
            {
                BankerCardPoint = BJCard_Control.SeatBanker[1];
                byte ChangePoint = (byte)(((BankerCardPoint - 1) % 13) + 1);
                if (BankerCardPoint == 0)
                {
                    CardSprite.spriteName = "bg_pokerback_1";
                }
                else if (BankerCardPoint > 0 && BankerCardPoint < 14)
                {
                    CardSprite.spriteName = "bg_poker_d" + ChangePoint;
                }
                else if (BankerCardPoint >= 14 && BankerCardPoint < 27)
                {
                    CardSprite.spriteName = "bg_poker_c" + ChangePoint;
                }
                else if (BankerCardPoint >= 27 && BankerCardPoint < 40)
                {
                    CardSprite.spriteName = "bg_poker_b" + ChangePoint;
                }
                else if (BankerCardPoint >= 40)
                {
                    CardSprite.spriteName = "bg_poker_a" + ChangePoint;
                }
                BJMainGame_Control.FirstCardShow_Bool = false;
                FinallOpen = true;
            }
        }
        else if (NowCardTeam == CardTeamList.CardBanker && BankerCardPoint == 0)
        {
            CardSprite.spriteName = "bg_pokerback_1";
        }
        else if (BankerCardPoint != 0)
        {
            byte ChangePoint = (byte)(((BankerCardPoint - 1) % 13) + 1);
            if (BankerCardPoint == 0)
            {
                CardSprite.spriteName = "bg_pokerback_1";
            }
            else if (BankerCardPoint > 0 && BankerCardPoint < 14)
            {
                CardSprite.spriteName = "bg_poker_d" + ChangePoint;
            }
            else if (BankerCardPoint >= 14 && BankerCardPoint < 27)
            {
                CardSprite.spriteName = "bg_poker_c" + ChangePoint;
            }
            else if (BankerCardPoint >= 27 && BankerCardPoint < 40)
            {
                CardSprite.spriteName = "bg_poker_b" + ChangePoint;
            }
            else if (BankerCardPoint >= 40)
            {
                CardSprite.spriteName = "bg_poker_a" + ChangePoint;
            }
        }
    }

    void OnClick()
    {
        if (!BJCard_Control.ButtonOnClickBool)
        {
            if (NowCardTeam == CardTeamList.Card1Team1)
                BJCard_Control.Seat1Team1OpenCloseCheck = true;
            if (NowCardTeam == CardTeamList.Card1Team2)
                BJCard_Control.Seat1Team2OpenCloseCheck = true;
            if (NowCardTeam == CardTeamList.Card2Team1)
                BJCard_Control.Seat2Team1OpenCloseCheck = true;
            if (NowCardTeam == CardTeamList.Card2Team2)
                BJCard_Control.Seat2Team2OpenCloseCheck = true;
            if (NowCardTeam == CardTeamList.Card3Team1)
                BJCard_Control.Seat3Team1OpenCloseCheck = true;
            if (NowCardTeam == CardTeamList.Card3Team2)
                BJCard_Control.Seat3Team2OpenCloseCheck = true;
            if (NowCardTeam == CardTeamList.Card4Team1)
                BJCard_Control.Seat4Team1OpenCloseCheck = true;
            if (NowCardTeam == CardTeamList.Card4Team2)
                BJCard_Control.Seat4Team2OpenCloseCheck = true;
            if (NowCardTeam == CardTeamList.Card5Team1)
                BJCard_Control.Seat5Team1OpenCloseCheck = true;
            if (NowCardTeam == CardTeamList.Card5Team2)
                BJCard_Control.Seat5Team2OpenCloseCheck = true;

            BJCard_Control.ButtonOnClickBool = true;
        }
    }

    void ThisTeam()
    {
        if (NowCardTeam == CardTeamList.Card1Team1)
        {
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card1Team1] = 1;
            BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card1Team1] = 1;
        }
        else if (NowCardTeam == CardTeamList.Card1Team2)
        {
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card1Team2] = 1;
            BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card1Team2] = 1;
        }
        else if (NowCardTeam == CardTeamList.Card2Team1)
        {
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card2Team1] = 1;
            BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card2Team1] = 1;
        }
        else if (NowCardTeam == CardTeamList.Card2Team2)
        {
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card2Team2] = 1;
            BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card2Team2] = 1;
        }
        else if (NowCardTeam == CardTeamList.Card3Team1)
        {
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card3Team1] = 1;
            BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card3Team1] = 1;
        }
        else if (NowCardTeam == CardTeamList.Card3Team2)
        {
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card3Team2] = 1;
            BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card3Team2] = 1;
        }
        else if (NowCardTeam == CardTeamList.Card4Team1)
        {
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card4Team1] = 1;
            BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card4Team1] = 1;
        }
        else if (NowCardTeam == CardTeamList.Card4Team2)
        {
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card4Team2] = 1;
            BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card4Team2] = 1;
        }
        else if (NowCardTeam == CardTeamList.Card5Team1)
        {
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card5Team1] = 1;
            BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card5Team1] = 1;
        }
        else if (NowCardTeam == CardTeamList.Card5Team2)
        {
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card5Team2] = 1;
            BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card5Team2] = 1;
        }
        else if (NowCardTeam == CardTeamList.CardBanker)
        {
            BJCard_Control.PointCheckStart[(byte)CardTeamList.CardBanker] = 1;
            BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.CardBanker] = 1;
        }
    }
}
