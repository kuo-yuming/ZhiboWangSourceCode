using UnityEngine;
using System.Collections;
using CardTeamListClass;
using WinLoseListClass;
using GameCore.Manager.BlackJack;

public class WinLose_Control : MonoBehaviour {

    public UISpriteAnimation[] WinLoseAnimation = new UISpriteAnimation[5];
    public UISprite[] WinLoseSprite = new UISprite[5];
    public GameObject[] WinLoseObject = new GameObject[5];

    public CardTeamList ThisTableTeamList;

    bool Init_Bool = false;
    float DelayTime = 0;
    // Use this for initialization
    void Start () {
        Init();
    }
	
	// Update is called once per frame
	void Update () {
        ShowChange();

        if (ThisTableTeamList == CardTeamList.Card1Team1 && BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card1Team1] == 1)
        {
            Show();
            if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.BlackJack)
            {
                if (BJCard_Control.Seat1Team2.Count != 0)
                    WinLoseObject[0].transform.localScale = new Vector3(0.3f, 0.3f, 1);
                else
                    WinLoseObject[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinPlayer)
            {
                if (BJCard_Control.Seat1Team2.Count != 0)
                    WinLoseObject[1].transform.localScale = new Vector3(0.3f, 0.3f, 1);
                else
                    WinLoseObject[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinBanker)
            {
                if (BJCard_Control.Seat1Team2.Count != 0)
                    WinLoseObject[2].transform.localScale = new Vector3(0.3f, 0.3f, 1);
                else
                    WinLoseObject[2].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinDraw)
            {
                if (BJCard_Control.Seat1Team2.Count != 0)
                    WinLoseObject[3].transform.localScale = new Vector3(0.3f, 0.3f, 1);
                else
                    WinLoseObject[3].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.PointOut)
            {
                if (BJCard_Control.Seat1Team2.Count != 0)
                    WinLoseObject[4].transform.localScale = new Vector3(0.3f, 0.3f, 1);
                else
                    WinLoseObject[4].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            }
            BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card1Team1] = 0;
        }
        else if (ThisTableTeamList == CardTeamList.Card1Team2 && BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card1Team2] == 1)
        {
            Show();
            if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.BlackJack)
            {
                if (BJCard_Control.Seat1Team2.Count != 0)
                    WinLoseObject[0].transform.localScale = new Vector3(0.3f, 0.3f, 1);
                else
                    WinLoseObject[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinPlayer)
            {
                if (BJCard_Control.Seat1Team2.Count != 0)
                    WinLoseObject[1].transform.localScale = new Vector3(0.3f, 0.3f, 1);
                else
                    WinLoseObject[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinBanker)
            {
                if (BJCard_Control.Seat1Team2.Count != 0)
                    WinLoseObject[2].transform.localScale = new Vector3(0.3f, 0.3f, 1);
                else
                    WinLoseObject[2].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinDraw)
            {
                if (BJCard_Control.Seat1Team2.Count != 0)
                    WinLoseObject[3].transform.localScale = new Vector3(0.3f, 0.3f, 1);
                else
                    WinLoseObject[3].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.PointOut)
            {
                if (BJCard_Control.Seat1Team2.Count != 0)
                    WinLoseObject[4].transform.localScale = new Vector3(0.3f, 0.3f, 1);
                else
                    WinLoseObject[4].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            }
            BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card1Team2] = 0;
        }
        else if (ThisTableTeamList == CardTeamList.Card2Team1 && BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card2Team1] == 1)
        {
            Show();
            if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.BlackJack)
            {
                if (BJCard_Control.Seat2Team2.Count != 0)
                    WinLoseObject[0].transform.localScale = new Vector3(0.35f, 0.35f, 1);
                else
                    WinLoseObject[0].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinPlayer)
            {
                if (BJCard_Control.Seat2Team2.Count != 0)
                    WinLoseObject[1].transform.localScale = new Vector3(0.35f, 0.35f, 1);
                else
                    WinLoseObject[1].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinBanker)
            {
                if (BJCard_Control.Seat2Team2.Count != 0)
                    WinLoseObject[2].transform.localScale = new Vector3(0.43f, 0.43f, 1);
                else
                    WinLoseObject[2].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinDraw)
            {
                if (BJCard_Control.Seat2Team2.Count != 0)
                    WinLoseObject[3].transform.localScale = new Vector3(0.43f, 0.43f, 1);
                else
                    WinLoseObject[3].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.PointOut)
            {
                if (BJCard_Control.Seat2Team2.Count != 0)
                    WinLoseObject[4].transform.localScale = new Vector3(0.43f, 0.43f, 1);
                else
                    WinLoseObject[4].transform.localScale = new Vector3(0.83f, 0.83f, 1);
            }
            BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card2Team1] = 0;
        }
        else if (ThisTableTeamList == CardTeamList.Card2Team2 && BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card2Team2] == 1)
        {
            Show();
            if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.BlackJack)
            {
                if (BJCard_Control.Seat2Team2.Count != 0)
                    WinLoseObject[0].transform.localScale = new Vector3(0.35f, 0.35f, 1);
                else
                    WinLoseObject[0].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinPlayer)
            {
                if (BJCard_Control.Seat2Team2.Count != 0)
                    WinLoseObject[1].transform.localScale = new Vector3(0.35f, 0.35f, 1);
                else
                    WinLoseObject[1].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinBanker)
            {
                if (BJCard_Control.Seat2Team2.Count != 0)
                    WinLoseObject[2].transform.localScale = new Vector3(0.43f, 0.43f, 1);
                else
                    WinLoseObject[2].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinDraw)
            {
                if (BJCard_Control.Seat2Team2.Count != 0)
                    WinLoseObject[3].transform.localScale = new Vector3(0.43f, 0.43f, 1);
                else
                    WinLoseObject[3].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.PointOut)
            {
                if (BJCard_Control.Seat2Team2.Count != 0)
                    WinLoseObject[4].transform.localScale = new Vector3(0.43f, 0.43f, 1);
                else
                    WinLoseObject[4].transform.localScale = new Vector3(0.83f, 0.83f, 1);
            }
            BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card2Team2] = 0;
        }
        else if (ThisTableTeamList == CardTeamList.Card3Team1 && BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card3Team1] == 1)
        {
            Show();
            if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.BlackJack)
            {
                if (BJCard_Control.Seat3Team2.Count != 0)
                    WinLoseObject[0].transform.localScale = new Vector3(0.35f, 0.35f, 1);
                else
                    WinLoseObject[0].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinPlayer)
            {
                if (BJCard_Control.Seat3Team2.Count != 0)
                    WinLoseObject[1].transform.localScale = new Vector3(0.35f, 0.35f, 1);
                else
                    WinLoseObject[1].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinBanker)
            {
                if (BJCard_Control.Seat3Team2.Count != 0)
                    WinLoseObject[2].transform.localScale = new Vector3(0.43f, 0.43f, 1);
                else
                    WinLoseObject[2].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinDraw)
            {
                if (BJCard_Control.Seat2Team2.Count != 0)
                    WinLoseObject[3].transform.localScale = new Vector3(0.43f, 0.43f, 1);
                else
                    WinLoseObject[3].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.PointOut)
            {
                if (BJCard_Control.Seat2Team2.Count != 0)
                    WinLoseObject[4].transform.localScale = new Vector3(0.43f, 0.43f, 1);
                else
                    WinLoseObject[4].transform.localScale = new Vector3(0.83f, 0.83f, 1);
            }
            BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card3Team1] = 0;
        }
        else if (ThisTableTeamList == CardTeamList.Card3Team2 && BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card3Team2] == 1)
        {
            Show();
            if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.BlackJack)
            {
                if (BJCard_Control.Seat3Team2.Count != 0)
                    WinLoseObject[0].transform.localScale = new Vector3(0.35f, 0.35f, 1);
                else
                    WinLoseObject[0].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinPlayer)
            {
                if (BJCard_Control.Seat3Team2.Count != 0)
                    WinLoseObject[1].transform.localScale = new Vector3(0.35f, 0.35f, 1);
                else
                    WinLoseObject[1].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinBanker)
            {
                if (BJCard_Control.Seat3Team2.Count != 0)
                    WinLoseObject[2].transform.localScale = new Vector3(0.43f, 0.43f, 1);
                else
                    WinLoseObject[2].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinDraw)
            {
                if (BJCard_Control.Seat2Team2.Count != 0)
                    WinLoseObject[3].transform.localScale = new Vector3(0.43f, 0.43f, 1);
                else
                    WinLoseObject[3].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.PointOut)
            {
                if (BJCard_Control.Seat2Team2.Count != 0)
                    WinLoseObject[4].transform.localScale = new Vector3(0.43f, 0.43f, 1);
                else
                    WinLoseObject[4].transform.localScale = new Vector3(0.83f, 0.83f, 1);
            }
            BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card3Team2] = 0;
        }
        else if (ThisTableTeamList == CardTeamList.Card4Team1 && BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card4Team1] == 1)
        {
            Show();
            if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.BlackJack)
            {
                if (BJCard_Control.Seat4Team2.Count != 0)
                    WinLoseObject[0].transform.localScale = new Vector3(0.35f, 0.35f, 1);
                else
                    WinLoseObject[0].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinPlayer)
            {
                if (BJCard_Control.Seat4Team2.Count != 0)
                    WinLoseObject[1].transform.localScale = new Vector3(0.35f, 0.35f, 1);
                else
                    WinLoseObject[1].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinBanker)
            {
                if (BJCard_Control.Seat4Team2.Count != 0)
                    WinLoseObject[2].transform.localScale = new Vector3(0.43f, 0.43f, 1);
                else
                    WinLoseObject[2].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinDraw)
            {
                if (BJCard_Control.Seat4Team2.Count != 0)
                    WinLoseObject[3].transform.localScale = new Vector3(0.43f, 0.43f, 1);
                else
                    WinLoseObject[3].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.PointOut)
            {
                if (BJCard_Control.Seat4Team2.Count != 0)
                    WinLoseObject[4].transform.localScale = new Vector3(0.43f, 0.43f, 1);
                else
                    WinLoseObject[4].transform.localScale = new Vector3(0.83f, 0.83f, 1);
            }
            BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card4Team1] = 0;
        }
        else if (ThisTableTeamList == CardTeamList.Card4Team2 && BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card4Team2] == 1)
        {
            Show();
            if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.BlackJack)
            {
                if (BJCard_Control.Seat4Team2.Count != 0)
                    WinLoseObject[0].transform.localScale = new Vector3(0.35f, 0.35f, 1);
                else
                    WinLoseObject[0].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinPlayer)
            {
                if (BJCard_Control.Seat4Team2.Count != 0)
                    WinLoseObject[1].transform.localScale = new Vector3(0.35f, 0.35f, 1);
                else
                    WinLoseObject[1].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinBanker)
            {
                if (BJCard_Control.Seat4Team2.Count != 0)
                    WinLoseObject[2].transform.localScale = new Vector3(0.43f, 0.43f, 1);
                else
                    WinLoseObject[2].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinDraw)
            {
                if (BJCard_Control.Seat4Team2.Count != 0)
                    WinLoseObject[3].transform.localScale = new Vector3(0.43f, 0.43f, 1);
                else
                    WinLoseObject[3].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.PointOut)
            {
                if (BJCard_Control.Seat4Team2.Count != 0)
                    WinLoseObject[4].transform.localScale = new Vector3(0.43f, 0.43f, 1);
                else
                    WinLoseObject[4].transform.localScale = new Vector3(0.83f, 0.83f, 1);
            }
            BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card4Team2] = 0;
        }
        else if (ThisTableTeamList == CardTeamList.Card5Team1 && BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card5Team1] == 1)
        {
            Show();
            if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.BlackJack)
            {
                if (BJCard_Control.Seat5Team2.Count != 0)
                    WinLoseObject[0].transform.localScale = new Vector3(0.35f, 0.35f, 1);
                else
                    WinLoseObject[0].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinPlayer)
            {
                if (BJCard_Control.Seat5Team2.Count != 0)
                    WinLoseObject[1].transform.localScale = new Vector3(0.35f, 0.35f, 1);
                else
                    WinLoseObject[1].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinBanker)
            {
                if (BJCard_Control.Seat5Team2.Count != 0)
                    WinLoseObject[2].transform.localScale = new Vector3(0.43f, 0.43f, 1);
                else
                    WinLoseObject[2].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinDraw)
            {
                if (BJCard_Control.Seat5Team2.Count != 0)
                    WinLoseObject[3].transform.localScale = new Vector3(0.43f, 0.43f, 1);
                else
                    WinLoseObject[3].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.PointOut)
            {
                if (BJCard_Control.Seat5Team2.Count != 0)
                    WinLoseObject[4].transform.localScale = new Vector3(0.43f, 0.43f, 1);
                else
                    WinLoseObject[4].transform.localScale = new Vector3(0.83f, 0.83f, 1);
            }
            BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card5Team1] = 0;
        }
        else if (ThisTableTeamList == CardTeamList.Card5Team2 && BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card5Team2] == 1)
        {
            Show();
            if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.BlackJack)
            {
                if (BJCard_Control.Seat5Team2.Count != 0)
                    WinLoseObject[0].transform.localScale = new Vector3(0.35f, 0.35f, 1);
                else
                    WinLoseObject[0].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinPlayer)
            {
                if (BJCard_Control.Seat5Team2.Count != 0)
                    WinLoseObject[1].transform.localScale = new Vector3(0.35f, 0.35f, 1);
                else
                    WinLoseObject[1].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinBanker)
            {
                if (BJCard_Control.Seat5Team2.Count != 0)
                    WinLoseObject[2].transform.localScale = new Vector3(0.43f, 0.43f, 1);
                else
                    WinLoseObject[2].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinDraw)
            {
                if (BJCard_Control.Seat5Team2.Count != 0)
                    WinLoseObject[3].transform.localScale = new Vector3(0.43f, 0.43f, 1);
                else
                    WinLoseObject[3].transform.localScale = new Vector3(0.72f, 0.72f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.PointOut)
            {
                if (BJCard_Control.Seat5Team2.Count != 0)
                    WinLoseObject[4].transform.localScale = new Vector3(0.43f, 0.43f, 1);
                else
                    WinLoseObject[4].transform.localScale = new Vector3(0.83f, 0.83f, 1);
            }
            BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card5Team2] = 0;
        }
        else if (ThisTableTeamList == CardTeamList.CardBanker && BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.CardBanker] == 1)
        {
            MainShow();
            BJMainGame_Control.OneTeamWinLoseShow = true;
            BJMainGame_Control.MainWinLoseShow = false;
            BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.CardBanker] = 0;
        }


        if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.NewRound
        || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.ShuffleNewRound
        || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.WaitBet 
        || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.GameOver)
        {
            if (Init_Bool)
            {
                Init();
                Init_Bool = false;
            }
        }
        else
        {
            Init_Bool = true;
        }
    }

    void Init()
    {
        WinLoseSprite[0].spriteName = "AR_BJ_01";
        WinLoseSprite[1].spriteName = "AR_Win_01";
        WinLoseSprite[2].spriteName = "AR_lose_01";
        WinLoseSprite[3].spriteName = "AR_push_01";
        WinLoseSprite[4].spriteName = "AR_Bust_01";
        for (int i = 0; i < 5; i++)
        {
            WinLoseAnimation[i].ResetToBeginning();
            WinLoseAnimation[i].enabled = false;
            WinLoseSprite[i].enabled = false;
        }
        DelayTime = 0;
    }

    //輸贏表演
    #region show
    void Show()
    {
        if (BJMainGame_Control.OneTeamWinLoseShow && ThisTableTeamList == CardTeamList.Card1Team1)
        {
            if (BJMainGame_Control.TableWinLose[CardTeamList.Card1Team1] == WinLoseList.BlackJack)
            {
                WinLoseSprite[0].enabled = true;
                WinLoseSprite[0].spriteName = "AR_BJ_21";
                if (BJCard_Control.Seat1Team2.Count != 0)
                    WinLoseObject[0].transform.localScale = new Vector3(0.3f,0.3f,1);
                else
                    WinLoseObject[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[CardTeamList.Card1Team1] == WinLoseList.WinPlayer)
            {
                WinLoseSprite[1].enabled = true;
                WinLoseSprite[1].spriteName = "AR_Win_13";
                if (BJCard_Control.Seat1Team2.Count != 0)
                    WinLoseObject[1].transform.localScale = new Vector3(0.3f, 0.3f, 1);
                else
                    WinLoseObject[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[CardTeamList.Card1Team1] == WinLoseList.WinBanker)
            {
                WinLoseSprite[2].enabled = true;
                WinLoseSprite[2].spriteName = "AR_lose_16";
                if (BJCard_Control.Seat1Team2.Count != 0)
                    WinLoseObject[2].transform.localScale = new Vector3(0.35f, 0.35f, 1);
                else
                    WinLoseObject[2].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[CardTeamList.Card1Team1] == WinLoseList.WinDraw)
            {
                WinLoseSprite[3].enabled = true;
                WinLoseSprite[3].spriteName = "AR_push_13";
                if (BJCard_Control.Seat1Team2.Count != 0)
                    WinLoseObject[3].transform.localScale = new Vector3(0.35f, 0.35f, 1);
                else
                    WinLoseObject[3].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            }
            else if (BJMainGame_Control.TableWinLose[CardTeamList.Card1Team1] == WinLoseList.PointOut)
            {
                WinLoseSprite[4].enabled = true;
                WinLoseSprite[4].spriteName = "AR_Bust_15";
                if (BJCard_Control.Seat1Team2.Count != 0)
                    WinLoseObject[4].transform.localScale = new Vector3(0.35f, 0.35f, 1);
                else
                    WinLoseObject[4].transform.localScale = new Vector3(0.6f, 0.6f, 1);
            }
            BJMainGame_Control.OneTeamWinLoseShow = false;
        }
        else
        {
            if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.BlackJack)
            {
                WinLoseAnimation[0].ResetToBeginning();
                WinLoseAnimation[0].enabled = true;
                WinLoseSprite[0].enabled = true;
                WinLoseAnimation[0].Play();
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinPlayer)
            {
                WinLoseAnimation[1].ResetToBeginning();
                WinLoseAnimation[1].enabled = true;
                WinLoseSprite[1].enabled = true;
                WinLoseAnimation[1].Play();
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinBanker)
            {
                WinLoseAnimation[2].ResetToBeginning();
                WinLoseAnimation[2].enabled = true;
                WinLoseSprite[2].enabled = true;
                WinLoseAnimation[2].Play();
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.WinDraw)
            {
                WinLoseAnimation[3].ResetToBeginning();
                WinLoseAnimation[3].enabled = true;
                WinLoseSprite[3].enabled = true;
                WinLoseAnimation[3].Play();
            }
            else if (BJMainGame_Control.TableWinLose[ThisTableTeamList] == WinLoseList.PointOut)
            {
                WinLoseAnimation[4].ResetToBeginning();
                WinLoseAnimation[4].enabled = true;
                WinLoseSprite[4].enabled = true;
                WinLoseAnimation[4].Play();
            }
        }
    }
    #endregion

    //主要輸贏表演
    #region MainShow
    void MainShow()
    {
        if (BJMainGame_Control.TableWinLose[CardTeamList.Card1Team1] == WinLoseList.BlackJack)
        {
            WinLoseAnimation[0].ResetToBeginning();
            WinLoseAnimation[0].enabled = true;
            WinLoseSprite[0].enabled = true;
            WinLoseAnimation[0].Play();
        }
        else if (BJMainGame_Control.TableWinLose[CardTeamList.Card1Team1] == WinLoseList.WinPlayer)
        {
            WinLoseAnimation[1].ResetToBeginning();
            WinLoseAnimation[1].enabled = true;
            WinLoseSprite[1].enabled = true;
            WinLoseAnimation[1].Play();
        }
        else if (BJMainGame_Control.TableWinLose[CardTeamList.Card1Team1] == WinLoseList.WinBanker)
        {
            WinLoseAnimation[2].ResetToBeginning();
            WinLoseAnimation[2].enabled = true;
            WinLoseSprite[2].enabled = true;
            WinLoseAnimation[2].Play();
        }
        else if (BJMainGame_Control.TableWinLose[CardTeamList.Card1Team1] == WinLoseList.WinDraw)
        {
            WinLoseAnimation[3].ResetToBeginning();
            WinLoseAnimation[3].enabled = true;
            WinLoseSprite[3].enabled = true;
            WinLoseAnimation[3].Play();
        }
        else if (BJMainGame_Control.TableWinLose[CardTeamList.Card1Team1] == WinLoseList.PointOut)
        {
            WinLoseAnimation[4].ResetToBeginning();
            WinLoseAnimation[4].enabled = true;
            WinLoseSprite[4].enabled = true;
            WinLoseAnimation[4].Play();
        }
    }
    #endregion

    //主要表演後切換
    #region showchange
    void ShowChange()
    {
        if (ThisTableTeamList == CardTeamList.CardBanker)
        {
            if (!WinLoseAnimation[0].isPlaying && BJMainGame_Control.OneTeamWinLoseShow)
            {
                if (DelayTime < 1)
                {
                    DelayTime += Time.deltaTime;
                }
                else
                {
                    WinLoseAnimation[0].ResetToBeginning();
                    WinLoseAnimation[0].enabled = false;
                    WinLoseSprite[0].enabled = false;
                    BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card1Team1] = 1;
                    DelayTime = 0;
                }
            }
            else if (!WinLoseAnimation[1].isPlaying)
            {
                if (DelayTime < 1)
                {
                    DelayTime += Time.deltaTime;
                }
                else
                {
                    WinLoseAnimation[1].ResetToBeginning();
                    WinLoseAnimation[1].enabled = false;
                    WinLoseSprite[1].enabled = false;
                    BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card1Team1] = 1;
                    DelayTime = 0;
                }
            }
            else if (!WinLoseAnimation[2].isPlaying)
            {
                if (DelayTime < 1)
                {
                    DelayTime += Time.deltaTime;
                }
                else
                {
                    WinLoseAnimation[2].ResetToBeginning();
                    WinLoseAnimation[2].enabled = false;
                    WinLoseSprite[2].enabled = false;
                    BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card1Team1] = 1;
                    DelayTime = 0;
                }
            }
            else if (!WinLoseAnimation[3].isPlaying)
            {
                if (DelayTime < 1)
                {
                    DelayTime += Time.deltaTime;
                }
                else
                {
                    WinLoseAnimation[3].ResetToBeginning();
                    WinLoseAnimation[3].enabled = false;
                    WinLoseSprite[3].enabled = false;
                    BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card1Team1] = 1;
                    DelayTime = 0;
                }
            }
            else if (!WinLoseAnimation[4].isPlaying)
            {
                if (DelayTime < 1)
                {
                    DelayTime += Time.deltaTime;
                }
                else
                {
                    WinLoseAnimation[4].ResetToBeginning();
                    WinLoseAnimation[4].enabled = false;
                    WinLoseSprite[4].enabled = false;
                    BJMainGame_Control.TableWinLoseCheck[(byte)CardTeamList.Card1Team1] = 1;
                    DelayTime = 0;
                }
            }
        }
    }
    #endregion

}
