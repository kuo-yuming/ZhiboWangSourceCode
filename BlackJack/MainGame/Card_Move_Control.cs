using UnityEngine;
using System.Collections;
using CardTeamListClass;
using GameCore.Manager.BlackJack;

public class Card_Move_Control : MonoBehaviour {

    public GameObject Card_Object;
    public GameObject InstantiateSeat;
    public GameObject Main_Object;
    public CardTeamList CardTeam_Control;
    public byte CardPoint = 0;
    public bool OpenClose_Bool = false;
    public UIGrid TeamGrid;
    public bool OpenCloseStart = false;
    public Vector3 FinallVector3 = new Vector3(0,0,0);
    public static bool ShowOK = false;
    public static bool FinallSeat = false;

    public UISprite Tip1_Sprite;
    public UISprite Tip2_Sprite;
    public Vector3 TipFromV3 = new Vector3(0, 0, 0);
    public Vector3 TipToV3 = new Vector3(0, 0, 0);
    public bool TipFromBool = false;
    public bool TipToBool = false;

    bool Init_Bool = false;
    //float DelayTime = 0;
    // Use this for initialization
    void Start () {
        DataInit();
    }

    // Update is called once per frame
    void Update()
    {
        if (OpenCloseStart)
        {
            if (CardTeam_Control == CardTeamList.Card1Team1 || CardTeam_Control == CardTeamList.Card1Team2)
            {
                if (OpenClose_Bool)
                {
                    TeamGrid.cellWidth = 0;
                    Tip1_Sprite.transform.localPosition = new Vector3(43, 47, 1);
                    Tip2_Sprite.transform.localPosition = new Vector3(43 - 10, 47, 1);
                }
                else
                {
                    TeamGrid.cellWidth = 24;
                    Tip1_Sprite.transform.localPosition = new Vector3((12 * (CardPoint - 1)) + 43, 47, 1);
                    Tip2_Sprite.transform.localPosition = new Vector3((12 * (CardPoint - 1)) + (43 - 10), 47, 1);
                }
            }
            else
            {
                if (OpenClose_Bool)
                {
                    TeamGrid.cellWidth = 0;
                    Tip1_Sprite.transform.localPosition = new Vector3(43, 30, 1);
                    Tip2_Sprite.transform.localPosition = new Vector3(43 - 10, 30, 1);
                }
                else
                {
                    TeamGrid.cellWidth = 24;
                    Tip1_Sprite.transform.localPosition = new Vector3((12 * (CardPoint - 1)) + 43, 30, 1);
                    Tip2_Sprite.transform.localPosition = new Vector3((12 * (CardPoint - 1)) + (43 - 10), 30, 1);
                }
            }
            TeamGrid.enabled = true;
            OpenCloseStart = false;
        }

        //卡牌開合確認
        if (BJCard_Control.ButtonOnClickBool)
        {
            CardOpenCloseCheck();
        }

        ////卡片增加&&取該區最後一張卡牌位子
        CardOtherCheck();

        //卡牌總點數位子確認
        if (TipFromBool || TipToBool)
            TipSeatCheck();

        //
        if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.NewRound
            || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.ShuffleNewRound
            || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.WaitBet
            || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.GameOver || StateShow_Control.Backgrond_Bool)
        {
            if (Init_Bool)
            {
                DataInit();
                Init_Bool = false;
            }
        }
        else
        {
            Init_Bool = true;
        }
    }

    //追加卡牌
    #region CardAdd
    void CardAdd(int usCardNumber, int usNumber)
    {
        GameObject Data = Instantiate(Card_Object);
        Data.transform.parent = InstantiateSeat.transform;
        if (CardPoint < 10)
            Data.name = "0" + usNumber.ToString();
        else
            Data.name = usNumber.ToString();
        Data.transform.localScale = new Vector3(1, 1, 1);
        Data.transform.localPosition = this.transform.localPosition;
        CardButton_Control Data_Control = Data.GetComponent<CardButton_Control>();
        Data_Control.NowCardTeam = CardTeam_Control;

        //正式
        byte ChangePoint = (byte)(((usCardNumber - 1) % 13) + 1);
        if (usCardNumber < 14)
        {
            Data_Control.CardSprite.spriteName = "bg_poker_d" + ChangePoint;
        }
        else if (usCardNumber >= 14 && usCardNumber < 27)
        {
            Data_Control.CardSprite.spriteName = "bg_poker_c" + ChangePoint;
        }
        else if (usCardNumber >= 27 && usCardNumber < 40)
        {
            Data_Control.CardSprite.spriteName = "bg_poker_b" + ChangePoint;
        }
        else if (usCardNumber >= 40)
        {
            Data_Control.CardSprite.spriteName = "bg_poker_a" + ChangePoint;
        }
        Data_Control.CardSprite.depth = CardPoint - 1;

        if (CardTeam_Control == CardTeamList.CardBanker)
        {
            Data_Control.BankerCardPoint = (byte)usCardNumber;
            if (usNumber == 1)
            {
                DealerCard_Move.BankerSenceCard = true;
            }
        }

        if (CardTeam_Control == CardTeamList.Card1Team1 && (BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card1Team1] == 1 || BJCard_Control.CardControl_End[(byte)CardTeamList.Card1Team1] == 1))
        {
            Data_Control.CardSprite.enabled = true;
        }
        else if (CardTeam_Control == CardTeamList.Card1Team2 && (BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card1Team2] == 1 || BJCard_Control.CardControl_End[(byte)CardTeamList.Card1Team2] == 1))
        {
            Data_Control.CardSprite.enabled = true;
        }
        else if (CardTeam_Control == CardTeamList.Card2Team1 && (BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card2Team1] == 1 || BJCard_Control.CardControl_End[(byte)CardTeamList.Card2Team1] == 1))
        {
            Data_Control.CardSprite.enabled = true;
        }
        else if (CardTeam_Control == CardTeamList.Card2Team2 && (BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card2Team2] == 1 || BJCard_Control.CardControl_End[(byte)CardTeamList.Card2Team2] == 1))
        {
            Data_Control.CardSprite.enabled = true;
        }
        else if (CardTeam_Control == CardTeamList.Card3Team1 && (BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card3Team1] == 1 || BJCard_Control.CardControl_End[(byte)CardTeamList.Card3Team1] == 1))
        {
            Data_Control.CardSprite.enabled = true;
        }
        else if (CardTeam_Control == CardTeamList.Card3Team2 && (BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card3Team2] == 1 || BJCard_Control.CardControl_End[(byte)CardTeamList.Card3Team2] == 1))
        {
            Data_Control.CardSprite.enabled = true;
        }
        else if (CardTeam_Control == CardTeamList.Card4Team1 && (BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card4Team1] == 1 || BJCard_Control.CardControl_End[(byte)CardTeamList.Card4Team1] == 1))
        {
            Data_Control.CardSprite.enabled = true;
        }
        else if (CardTeam_Control == CardTeamList.Card4Team2 && (BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card4Team2] == 1 || BJCard_Control.CardControl_End[(byte)CardTeamList.Card4Team2] == 1))
        {
            Data_Control.CardSprite.enabled = true;
        }
        else if (CardTeam_Control == CardTeamList.Card5Team1 && (BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card5Team1] == 1 || BJCard_Control.CardControl_End[(byte)CardTeamList.Card1Team1] == 1))
        {
            Data_Control.CardSprite.enabled = true;
        }
        else if (CardTeam_Control == CardTeamList.Card5Team2 && (BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card5Team2] == 1 || BJCard_Control.CardControl_End[(byte)CardTeamList.Card1Team1] == 1))
        {
            Data_Control.CardSprite.enabled = true;
        }
        else
        {
            Data_Control.CardSprite.enabled = false;
        }

        if (usNumber == 5 && CardTeam_Control != CardTeamList.CardBanker)
        {
            OpenClose_Bool = true;
            OpenCloseStart = true;
        }

        TeamGrid.enabled = true;
      //  ShowOK = false;
        FinallSeat = true;
    }
    #endregion

    //卡牌開合確認
    #region CardOC
    void CardOpenCloseCheck()
    {
        if (BJCard_Control.Seat1Team1OpenCloseCheck && CardTeam_Control == CardTeamList.Card1Team1)
        {
            if (CardPoint > 5)
            {
                if (OpenClose_Bool)
                    OpenClose_Bool = false;
                else
                    OpenClose_Bool = true;
            }

            OpenCloseStart = true;
            BJCard_Control.Seat1Team1OpenCloseCheck = false;
            BJCard_Control.ButtonOnClickBool = false;
        }
        if (BJCard_Control.Seat1Team2OpenCloseCheck && CardTeam_Control == CardTeamList.Card1Team2)
        {
            if (CardPoint > 5)
            {
                if (OpenClose_Bool)
                    OpenClose_Bool = false;
                else
                    OpenClose_Bool = true;
            }

            BJCard_Control.Seat1Team2OpenCloseCheck = false;
            BJCard_Control.ButtonOnClickBool = false;
            OpenCloseStart = true;
        }
        if (BJCard_Control.Seat2Team1OpenCloseCheck && CardTeam_Control == CardTeamList.Card2Team1)
        {
            if (CardPoint > 5)
            {
                if (OpenClose_Bool)
                    OpenClose_Bool = false;
                else
                    OpenClose_Bool = true;
            }

            BJCard_Control.Seat2Team1OpenCloseCheck = false;
            BJCard_Control.ButtonOnClickBool = false;
            OpenCloseStart = true;
        }
        if (BJCard_Control.Seat2Team2OpenCloseCheck && CardTeam_Control == CardTeamList.Card2Team2)
        {
            if (CardPoint > 5)
            {
                if (OpenClose_Bool)
                    OpenClose_Bool = false;
                else
                    OpenClose_Bool = true;
            }

            BJCard_Control.Seat2Team2OpenCloseCheck = false;
            BJCard_Control.ButtonOnClickBool = false;
            OpenCloseStart = true;
        }
        if (BJCard_Control.Seat3Team1OpenCloseCheck && CardTeam_Control == CardTeamList.Card3Team1)
        {
            if (CardPoint > 5)
            {
                if (OpenClose_Bool)
                    OpenClose_Bool = false;
                else
                    OpenClose_Bool = true;
            }

            BJCard_Control.Seat3Team1OpenCloseCheck = false;
            BJCard_Control.ButtonOnClickBool = false;
            OpenCloseStart = true;
        }
        if (BJCard_Control.Seat3Team2OpenCloseCheck && CardTeam_Control == CardTeamList.Card3Team2)
        {
            if (CardPoint > 5)
            {
                if (OpenClose_Bool)
                    OpenClose_Bool = false;
                else
                    OpenClose_Bool = true;
            }

            BJCard_Control.Seat3Team2OpenCloseCheck = false;
            BJCard_Control.ButtonOnClickBool = false;
            OpenCloseStart = true;
        }
        if (BJCard_Control.Seat4Team1OpenCloseCheck && CardTeam_Control == CardTeamList.Card4Team1)
        {
            if (CardPoint > 5)
            {
                if (OpenClose_Bool)
                    OpenClose_Bool = false;
                else
                    OpenClose_Bool = true;
            }

            BJCard_Control.Seat4Team1OpenCloseCheck = false;
            BJCard_Control.ButtonOnClickBool = false;
            OpenCloseStart = true;
        }
        if (BJCard_Control.Seat4Team2OpenCloseCheck && CardTeam_Control == CardTeamList.Card4Team2)
        {
            if (CardPoint > 5)
            {
                if (OpenClose_Bool)
                    OpenClose_Bool = false;
                else
                    OpenClose_Bool = true;
            }

            BJCard_Control.Seat4Team2OpenCloseCheck = false;
            BJCard_Control.ButtonOnClickBool = false;
            OpenCloseStart = true;
        }
        if (BJCard_Control.Seat5Team1OpenCloseCheck && CardTeam_Control == CardTeamList.Card5Team1)
        {
            if (CardPoint > 5)
            {
                if (OpenClose_Bool)
                    OpenClose_Bool = false;
                else
                    OpenClose_Bool = true;
            }

            BJCard_Control.Seat5Team1OpenCloseCheck = false;
            BJCard_Control.ButtonOnClickBool = false;
            OpenCloseStart = true;
        }
        if (BJCard_Control.Seat5Team2OpenCloseCheck && CardTeam_Control == CardTeamList.Card5Team2)
        {
            if (CardPoint > 5)
            {
                if (OpenClose_Bool)
                    OpenClose_Bool = false;
                else
                    OpenClose_Bool = true;
            }

            BJCard_Control.Seat5Team2OpenCloseCheck = false;
            BJCard_Control.ButtonOnClickBool = false;
            OpenCloseStart = true;
        }
    }
    #endregion

    //清除卡牌
    #region DelCard
    void DeleteCard()
    {
        Transform[] Objs = InstantiateSeat.GetComponentsInChildren<Transform>();
        int Len = Objs.Length;
        for (int i = 0; i < Len; i++)
        {
            if (Objs[i].name != "SeatTeam")
            {
                Destroy(Objs[i].gameObject);
            }
        }

        CardPoint = 0;
        OpenClose_Bool = false;
    }
    #endregion

    //是否要追加卡牌
    #region AddCheck
    void CardAddCheck()
    {
        if (CardTeam_Control == CardTeamList.Card1Team1)
        {
            for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card1Team1] - CardPoint); i++)
            {
               
                CardAdd(BJCard_Control.Seat1Team1[CardPoint + i], CardPoint + i);
            }
            CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card1Team1];
            BJCard_Control.SeatTeamAddCheck[(byte)CardTeamList.Card1Team1] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card1Team2)
        {
            for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card1Team2] - CardPoint); i++)
            {
                CardAdd(BJCard_Control.Seat1Team2[CardPoint + i], CardPoint + i);
            }
            CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card1Team2];
            BJCard_Control.SeatTeamAddCheck[(byte)CardTeamList.Card1Team2] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card2Team1)
        {
            for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card2Team1] - CardPoint); i++)
            {
                CardAdd(BJCard_Control.Seat2Team1[CardPoint + i], CardPoint + i);
            }
            CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card2Team1];
            BJCard_Control.SeatTeamAddCheck[(byte)CardTeamList.Card2Team1] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card2Team2)
        {
            for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card2Team2] - CardPoint); i++)
            {
                CardAdd(BJCard_Control.Seat2Team2[CardPoint + i], CardPoint + i);
            }
            CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card2Team2];
            BJCard_Control.SeatTeamAddCheck[(byte)CardTeamList.Card2Team2] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card3Team1)
        {
            for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card3Team1] - CardPoint); i++)
            {
                CardAdd(BJCard_Control.Seat3Team1[CardPoint + i], CardPoint + i);
            }
            CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card3Team1];
            BJCard_Control.SeatTeamAddCheck[(byte)CardTeamList.Card3Team1] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card3Team2)
        {
            for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card3Team2] - CardPoint); i++)
            {
                CardAdd(BJCard_Control.Seat3Team2[CardPoint + i], CardPoint + i);
            }
            CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card3Team2];
            BJCard_Control.SeatTeamAddCheck[(byte)CardTeamList.Card3Team2] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card4Team1)
        {
            for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card4Team1] - CardPoint); i++)
            {
                CardAdd(BJCard_Control.Seat4Team1[CardPoint + i], CardPoint + i);
            }
            CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card4Team1];
            BJCard_Control.SeatTeamAddCheck[(byte)CardTeamList.Card4Team1] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card4Team2)
        {
            for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card4Team2] - CardPoint); i++)
            {
                CardAdd(BJCard_Control.Seat4Team2[CardPoint + i], CardPoint + i);
            }
            CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card4Team2];
            BJCard_Control.SeatTeamAddCheck[(byte)CardTeamList.Card4Team2] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card5Team1)
        {
            for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card5Team1] - CardPoint); i++)
            {
                CardAdd(BJCard_Control.Seat5Team1[CardPoint + i], CardPoint + i);
            }
            CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card5Team1];
            BJCard_Control.SeatTeamAddCheck[(byte)CardTeamList.Card5Team1] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card5Team2)
        {
            for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card5Team2] - CardPoint); i++)
            {
                CardAdd(BJCard_Control.Seat5Team2[CardPoint + i], CardPoint + i);
            }
            CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card5Team2];
            BJCard_Control.SeatTeamAddCheck[(byte)CardTeamList.Card5Team2] = 0;
        }
        else if (CardTeam_Control == CardTeamList.CardBanker)
        {
            for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.CardBanker] - CardPoint); i++)
            {
                CardAdd(BJCard_Control.SeatBanker[CardPoint + i], CardPoint + i);
            }
            CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.CardBanker];
            BJCard_Control.SeatTeamAddCheck[(byte)CardTeamList.CardBanker] = 0;
        }
    }
    #endregion

    //卡片最後位子確認
    #region FinallCardSeatGet
    void FinallCardSeatCheck()
    {
        Transform[] Objs = InstantiateSeat.GetComponentsInChildren<Transform>();
        int Len = Objs.Length;

        string ObjName = "";
        string ObjName2 = "";
        int PointAdd = 0;

        if (OpenClose_Bool)
        {
            PointAdd = 12;
        }
        else
        {
            if (CardPoint == 1)
                PointAdd = 12;
            else
                PointAdd = (int)(TeamGrid.cellWidth / 2);
        }

        if ((CardPoint - 1) < 10)
            ObjName = "0" + (CardPoint - 1);
        else
            ObjName = (CardPoint - 1).ToString();

        if (CardPoint > 1)
        {
            if ((CardPoint - 2) < 10)
                ObjName2 = "0" + (CardPoint - 2);
            else
                ObjName2 = (CardPoint - 2).ToString();
        }

        for (int i = 0; i < Len; i++)
        {
            if (Objs[i].name == ObjName)
            {
                DealerCard_Move.ThisCardList = CardTeam_Control;
                DealerCard_Move.FinallDealerCardMove = new Vector3(Objs[i].transform.localPosition.x + Main_Object.transform.localPosition.x + PointAdd - 12, Objs[i].transform.localPosition.y + Main_Object.transform.localPosition.y - 17, 1);
                TipToV3 = new Vector3(Objs[i].transform.localPosition.x, Objs[i].transform.localPosition.y, 1);
             //   Debug.Log("objs: " + Objs[i].name + " //Name: " + ObjName + " //x: " + DealerCard_Move.FinallDealerCardMove.x + " //y: " + DealerCard_Move.FinallDealerCardMove.y);
            }

            if (Objs[i].name == ObjName2)
                TipFromV3 = new Vector3(Objs[i].transform.localPosition.x, Objs[i].transform.localPosition.y, 1);
        }

        if (CardTeam_Control == CardTeamList.Card1Team1 && BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card1Team1] == 1)
            BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card1Team1] = 0;
        else if (CardTeam_Control == CardTeamList.Card1Team2 && BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card1Team2] == 1)
            BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card1Team2] = 0;
        else if (CardTeam_Control == CardTeamList.Card2Team1 && BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card2Team1] == 1)
            BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card2Team1] = 0;
        else if (CardTeam_Control == CardTeamList.Card2Team2 && BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card2Team2] == 1)
            BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card2Team2] = 0;
        else if (CardTeam_Control == CardTeamList.Card3Team1 && BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card3Team1] == 1)
            BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card3Team1] = 0;
        else if (CardTeam_Control == CardTeamList.Card3Team2 && BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card3Team2] == 1)
            BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card3Team2] = 0;
        else if (CardTeam_Control == CardTeamList.Card4Team1 && BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card4Team1] == 1)
            BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card4Team1] = 0;
        else if (CardTeam_Control == CardTeamList.Card4Team2 && BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card4Team2] == 1)
            BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card4Team2] = 0;
        else if (CardTeam_Control == CardTeamList.Card5Team1 && BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card5Team1] == 1)
            BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card5Team1] = 0;
        else if (CardTeam_Control == CardTeamList.Card5Team2 && BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card5Team2] == 1)
            BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card5Team2] = 0;
        else if (CardTeam_Control == CardTeamList.CardBanker && BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.CardBanker] == 1)
            BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.CardBanker] = 0;

        DealerCard_Move.MoveOK = true;
        FinallSeat = false;
        TipFromBool = true;
    }
    #endregion

    //卡牌其他處理
    #region Other
    void CardOtherCheck()
    {
        ////卡片增加
        if (CardTeam_Control == CardTeamList.Card1Team1 && BJCard_Control.SeatTeamAddCheck[(byte)CardTeamList.Card1Team1] == 1)
            CardAddCheck();
        else if (CardTeam_Control == CardTeamList.Card1Team2 && BJCard_Control.SeatTeamAddCheck[(byte)CardTeamList.Card1Team2] == 1)
            CardAddCheck();
        else if (CardTeam_Control == CardTeamList.Card2Team1 && BJCard_Control.SeatTeamAddCheck[(byte)CardTeamList.Card2Team1] == 1)
            CardAddCheck();
        else if (CardTeam_Control == CardTeamList.Card2Team2 && BJCard_Control.SeatTeamAddCheck[(byte)CardTeamList.Card2Team2] == 1)
            CardAddCheck();
        else if (CardTeam_Control == CardTeamList.Card3Team1 && BJCard_Control.SeatTeamAddCheck[(byte)CardTeamList.Card3Team1] == 1)
            CardAddCheck();
        else if (CardTeam_Control == CardTeamList.Card3Team2 && BJCard_Control.SeatTeamAddCheck[(byte)CardTeamList.Card3Team2] == 1)
            CardAddCheck();
        else if (CardTeam_Control == CardTeamList.Card4Team1 && BJCard_Control.SeatTeamAddCheck[(byte)CardTeamList.Card4Team1] == 1)
            CardAddCheck();
        else if (CardTeam_Control == CardTeamList.Card4Team2 && BJCard_Control.SeatTeamAddCheck[(byte)CardTeamList.Card4Team2] == 1)
            CardAddCheck();
        else if (CardTeam_Control == CardTeamList.Card5Team1 && BJCard_Control.SeatTeamAddCheck[(byte)CardTeamList.Card5Team1] == 1)
            CardAddCheck();
        else if (CardTeam_Control == CardTeamList.Card5Team2 && BJCard_Control.SeatTeamAddCheck[(byte)CardTeamList.Card5Team2] == 1)
            CardAddCheck();
        else if (CardTeam_Control == CardTeamList.CardBanker && BJCard_Control.SeatTeamAddCheck[(byte)CardTeamList.CardBanker] == 1)
            CardAddCheck();

        //分牌
        if (CardTeam_Control == CardTeamList.Card1Team1 && BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card1Team1] == 1)
            CardPointAngle();
        else if (CardTeam_Control == CardTeamList.Card1Team2 && BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card1Team2] == 1)
            CardPointAngle();
        else if (CardTeam_Control == CardTeamList.Card2Team1 && BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card2Team1] == 1)
            CardPointAngle();
        else if (CardTeam_Control == CardTeamList.Card2Team2 && BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card2Team2] == 1)
            CardPointAngle();
        else if (CardTeam_Control == CardTeamList.Card3Team1 && BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card3Team1] == 1)
            CardPointAngle();
        else if (CardTeam_Control == CardTeamList.Card3Team2 && BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card3Team2] == 1)
            CardPointAngle();
        else if (CardTeam_Control == CardTeamList.Card4Team1 && BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card4Team1] == 1)
            CardPointAngle();
        else if (CardTeam_Control == CardTeamList.Card4Team2 && BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card4Team2] == 1)
            CardPointAngle();
        else if (CardTeam_Control == CardTeamList.Card5Team1 && BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card5Team1] == 1)
            CardPointAngle();
        else if (CardTeam_Control == CardTeamList.Card5Team2 && BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card5Team2] == 1)
            CardPointAngle();
        else if (CardTeam_Control == CardTeamList.CardBanker && BJCard_Control.CardScoreboardStart[(byte)CardTeamList.CardBanker] == 1)
            CardPointAngle();

        ////取該區最後一張卡牌位子
        if (FinallSeat && !TeamGrid.enabled)
        {
            if (CardTeam_Control == CardTeamList.Card1Team1 && BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card1Team1] == 1)
                FinallCardSeatCheck();
            else if (CardTeam_Control == CardTeamList.Card1Team2 && BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card1Team2] == 1)
                FinallCardSeatCheck();
            else if (CardTeam_Control == CardTeamList.Card2Team1 && BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card2Team1] == 1)
                FinallCardSeatCheck();
            else if (CardTeam_Control == CardTeamList.Card2Team2 && BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card2Team2] == 1)
                FinallCardSeatCheck();
            else if (CardTeam_Control == CardTeamList.Card3Team1 && BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card3Team1] == 1)
                FinallCardSeatCheck();
            else if (CardTeam_Control == CardTeamList.Card3Team2 && BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card3Team2] == 1)
                FinallCardSeatCheck();
            else if (CardTeam_Control == CardTeamList.Card4Team1 && BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card4Team1] == 1)
                FinallCardSeatCheck();
            else if (CardTeam_Control == CardTeamList.Card4Team2 && BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card4Team2] == 1)
                FinallCardSeatCheck();
            else if (CardTeam_Control == CardTeamList.Card5Team1 && BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card5Team1] == 1)
                FinallCardSeatCheck();
            else if (CardTeam_Control == CardTeamList.Card5Team2 && BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.Card5Team2] == 1)
                FinallCardSeatCheck();
            else if (CardTeam_Control == CardTeamList.CardBanker && BJCard_Control.NowFinallCardSeat[(byte)CardTeamList.CardBanker] == 1)
                FinallCardSeatCheck();
        }

        ////Tip最後位子移動
        if (CardTeam_Control == CardTeamList.Card1Team1 && BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card1Team1] == 1)
            TipToBool = true;
        else if (CardTeam_Control == CardTeamList.Card1Team2 && BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card1Team2] == 1)
            TipToBool = true;
        else if (CardTeam_Control == CardTeamList.Card2Team1 && BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card2Team1] == 1)
            TipToBool = true;
        else if (CardTeam_Control == CardTeamList.Card2Team2 && BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card2Team2] == 1)
            TipToBool = true;
        else if (CardTeam_Control == CardTeamList.Card3Team1 && BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card3Team1] == 1)
            TipToBool = true;
        else if (CardTeam_Control == CardTeamList.Card3Team2 && BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card3Team2] == 1)
            TipToBool = true;
        else if (CardTeam_Control == CardTeamList.Card4Team1 && BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card4Team1] == 1)
            TipToBool = true;
        else if (CardTeam_Control == CardTeamList.Card4Team2 && BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card4Team2] == 1)
            TipToBool = true;
        else if (CardTeam_Control == CardTeamList.Card5Team1 && BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card5Team1] == 1)
            TipToBool = true;
        else if (CardTeam_Control == CardTeamList.Card5Team2 && BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card5Team2] == 1)
            TipToBool = true;
        else if (CardTeam_Control == CardTeamList.CardBanker && BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.CardBanker] == 1)
            TipToBool = true;

        ////強制表演結束
        if (CardTeam_Control == CardTeamList.Card1Team1 && BJCard_Control.CheckPoint[(byte)CardTeamList.Card1Team1] == 1)
            CancelCheck();
        else if (CardTeam_Control == CardTeamList.Card1Team2 && BJCard_Control.CheckPoint[(byte)CardTeamList.Card1Team2] == 1)
            CancelCheck();
        else if (CardTeam_Control == CardTeamList.Card2Team1 && BJCard_Control.CheckPoint[(byte)CardTeamList.Card2Team1] == 1)
            CancelCheck();
        else if (CardTeam_Control == CardTeamList.Card2Team2 && BJCard_Control.CheckPoint[(byte)CardTeamList.Card2Team2] == 1)
            CancelCheck();
        else if (CardTeam_Control == CardTeamList.Card3Team1 && BJCard_Control.CheckPoint[(byte)CardTeamList.Card3Team1] == 1)
            CancelCheck();
        else if (CardTeam_Control == CardTeamList.Card3Team2 && BJCard_Control.CheckPoint[(byte)CardTeamList.Card3Team2] == 1)
            CancelCheck();
        else if (CardTeam_Control == CardTeamList.Card4Team1 && BJCard_Control.CheckPoint[(byte)CardTeamList.Card4Team1] == 1)
            CancelCheck();
        else if (CardTeam_Control == CardTeamList.Card4Team2 && BJCard_Control.CheckPoint[(byte)CardTeamList.Card4Team2] == 1)
            CancelCheck();
        else if (CardTeam_Control == CardTeamList.Card5Team1 && BJCard_Control.CheckPoint[(byte)CardTeamList.Card5Team1] == 1)
            CancelCheck();
        else if (CardTeam_Control == CardTeamList.Card5Team2 && BJCard_Control.CheckPoint[(byte)CardTeamList.Card5Team2] == 1)
            CancelCheck();
        else if (CardTeam_Control == CardTeamList.CardBanker && BJCard_Control.CheckPoint[(byte)CardTeamList.CardBanker] == 1)
            CancelCheck();
    }
    #endregion

    //Tip位子確認
    #region TipSeat
    void TipSeatCheck()
    {
        if (CardTeam_Control == CardTeamList.Card1Team1)
        {
            if (TipFromBool)
            {
                Tip1_Sprite.transform.localPosition = new Vector3(TipFromV3.x + 43, TipFromV3.y + 47, TipFromV3.z);
                Tip2_Sprite.transform.localPosition = new Vector3(TipFromV3.x + (43 - 10), TipFromV3.y + 47, TipFromV3.z);
            }
            else if (TipToBool)
            {
                Tip1_Sprite.transform.localPosition = new Vector3(TipToV3.x + 43, TipToV3.y + 47, TipToV3.z);
                Tip2_Sprite.transform.localPosition = new Vector3(TipToV3.x + (43 - 10), TipToV3.y + 47, TipToV3.z);
                BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card1Team1] = 0;
            }
            TipFromBool = false;
            TipToBool = false;
        }
        else if (CardTeam_Control == CardTeamList.Card1Team2)
        {
            if (TipFromBool)
            {
                Tip1_Sprite.transform.localPosition = new Vector3(TipFromV3.x + 43, TipFromV3.y + 47, TipFromV3.z);
                Tip2_Sprite.transform.localPosition = new Vector3(TipFromV3.x + (43 - 10), TipFromV3.y + 47, TipFromV3.z);       
            }
            else if (TipToBool)
            {
                Tip1_Sprite.transform.localPosition = new Vector3(TipToV3.x + 43, TipToV3.y + 47, TipToV3.z);
                Tip2_Sprite.transform.localPosition = new Vector3(TipToV3.x + (43 - 10), TipToV3.y + 47, TipToV3.z);
                BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card1Team2] = 0;
            }
            TipFromBool = false;
            TipToBool = false;
        }
        else if (CardTeam_Control == CardTeamList.Card2Team1)
        {
            if (TipFromBool)
            {
                Tip1_Sprite.transform.localPosition = new Vector3(TipFromV3.x + 43, TipFromV3.y + 30, TipFromV3.z);
                Tip2_Sprite.transform.localPosition = new Vector3(TipFromV3.x + (43 - 10), TipFromV3.y + 30, TipFromV3.z);
            }
            else if (TipToBool)
            {
                Tip1_Sprite.transform.localPosition = new Vector3(TipToV3.x + 43, TipToV3.y + 30, TipToV3.z);
                Tip2_Sprite.transform.localPosition = new Vector3(TipToV3.x + (43 - 10), TipToV3.y + 30, TipToV3.z);
                BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card2Team1] = 0;
            }
            TipFromBool = false;
            TipToBool = false;
        }
        else if (CardTeam_Control == CardTeamList.Card2Team2)
        {
            if (TipFromBool)
            {
                Tip1_Sprite.transform.localPosition = new Vector3(TipFromV3.x + 43, TipFromV3.y + 30, TipFromV3.z);
                Tip2_Sprite.transform.localPosition = new Vector3(TipFromV3.x + (43 - 10), TipFromV3.y + 47, TipFromV3.z);
            }
            else if (TipToBool)
            {
                Tip1_Sprite.transform.localPosition = new Vector3(TipToV3.x + 43, TipToV3.y + 30, TipToV3.z);
                Tip2_Sprite.transform.localPosition = new Vector3(TipToV3.x + (43 - 10), TipToV3.y + 30, TipToV3.z);
                BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card2Team2] = 0;
            }
            TipFromBool = false;
            TipToBool = false;
        }
        else if (CardTeam_Control == CardTeamList.Card3Team1)
        {
            if (TipFromBool)
            {
                Tip1_Sprite.transform.localPosition = new Vector3(TipFromV3.x + 43, TipFromV3.y + 30, TipFromV3.z);
                Tip2_Sprite.transform.localPosition = new Vector3(TipFromV3.x + (43 - 10), TipFromV3.y + 30, TipFromV3.z);
            }
            else if (TipToBool)
            {
                Tip1_Sprite.transform.localPosition = new Vector3(TipToV3.x + 43, TipToV3.y + 30, TipToV3.z);
                Tip2_Sprite.transform.localPosition = new Vector3(TipToV3.x + (43 - 10), TipToV3.y + 30, TipToV3.z);
                BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card3Team1] = 0;
            }
            TipFromBool = false;
            TipToBool = false;
        }
        else if (CardTeam_Control == CardTeamList.Card3Team2)
        {
            if (TipFromBool)
            {
                Tip1_Sprite.transform.localPosition = new Vector3(TipFromV3.x + 43, TipFromV3.y + 30, TipFromV3.z);
                Tip2_Sprite.transform.localPosition = new Vector3(TipFromV3.x + (43 - 10), TipFromV3.y + 30, TipFromV3.z);
            }
            else if (TipToBool)
            {
                Tip1_Sprite.transform.localPosition = new Vector3(TipToV3.x + 43, TipToV3.y + 30, TipToV3.z);
                Tip2_Sprite.transform.localPosition = new Vector3(TipToV3.x + (43 - 10), TipToV3.y + 30, TipToV3.z);
                BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card3Team2] = 0;
            }
            TipFromBool = false;
            TipToBool = false;
        }
        else if (CardTeam_Control == CardTeamList.Card4Team1)
        {
            if (TipFromBool)
            {
                Tip1_Sprite.transform.localPosition = new Vector3(TipFromV3.x + 43, TipFromV3.y + 30, TipFromV3.z);
                Tip2_Sprite.transform.localPosition = new Vector3(TipFromV3.x + (43 - 10), TipFromV3.y + 30, TipFromV3.z);
            }
            else if (TipToBool)
            {
                Tip1_Sprite.transform.localPosition = new Vector3(TipToV3.x + 43, TipToV3.y + 30, TipToV3.z);
                Tip2_Sprite.transform.localPosition = new Vector3(TipToV3.x + (43 - 10), TipToV3.y + 30, TipToV3.z);
                BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card4Team1] = 0;
            }
            TipFromBool = false;
            TipToBool = false;
        }
        else if (CardTeam_Control == CardTeamList.Card4Team2)
        {
            if (TipFromBool)
            {
                Tip1_Sprite.transform.localPosition = new Vector3(TipFromV3.x + 43, TipFromV3.y + 30, TipFromV3.z);
                Tip2_Sprite.transform.localPosition = new Vector3(TipFromV3.x + (43 - 10), TipFromV3.y + 30, TipFromV3.z);
            }
            else if (TipToBool)
            {
                Tip1_Sprite.transform.localPosition = new Vector3(TipToV3.x + 43, TipToV3.y + 30, TipToV3.z);
                Tip2_Sprite.transform.localPosition = new Vector3(TipToV3.x + (43 - 10), TipToV3.y + 30, TipToV3.z);
                BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card4Team2] = 0;
            }
            TipFromBool = false;
            TipToBool = false;
        }
        else if (CardTeam_Control == CardTeamList.Card5Team1)
        {
            if (TipFromBool)
            {
                Tip1_Sprite.transform.localPosition = new Vector3(TipFromV3.x + 43, TipFromV3.y + 30, TipFromV3.z);
                Tip2_Sprite.transform.localPosition = new Vector3(TipFromV3.x + (43 - 10), TipFromV3.y + 30, TipFromV3.z);
            }
            else if (TipToBool)
            {
                Tip1_Sprite.transform.localPosition = new Vector3(TipToV3.x + 43, TipToV3.y + 30, TipToV3.z);
                Tip2_Sprite.transform.localPosition = new Vector3(TipToV3.x + (43 - 10), TipToV3.y + 30, TipToV3.z);
                BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card5Team1] = 0;
            }
            TipFromBool = false;
            TipToBool = false;
        }
        else if (CardTeam_Control == CardTeamList.Card5Team2)
        {
            if (TipFromBool)
            {
                Tip1_Sprite.transform.localPosition = new Vector3(TipFromV3.x + 43, TipFromV3.y + 30, TipFromV3.z);
                Tip2_Sprite.transform.localPosition = new Vector3(TipFromV3.x + (43 - 10), TipFromV3.y + 30, TipFromV3.z);
            }
            else if (TipToBool)
            {
                Tip1_Sprite.transform.localPosition = new Vector3(TipToV3.x + 43, TipToV3.y + 30, TipToV3.z);
                Tip2_Sprite.transform.localPosition = new Vector3(TipToV3.x + (43 - 10), TipToV3.y + 30, TipToV3.z);
                BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card5Team2] = 0;
            }
            TipFromBool = false;
            TipToBool = false;
        }
        else if (CardTeam_Control == CardTeamList.CardBanker)
        {
            if (TipFromBool)
            {
                Tip1_Sprite.transform.localPosition = new Vector3(TipFromV3.x + 43, TipFromV3.y + 47, TipFromV3.z);
                Tip2_Sprite.transform.localPosition = new Vector3(TipFromV3.x + (43 - 10), TipFromV3.y + 47, TipFromV3.z);
            }
            else if (TipToBool)
            {
                Tip1_Sprite.transform.localPosition = new Vector3(TipToV3.x + 43, TipToV3.y + 47, TipToV3.z);
                Tip2_Sprite.transform.localPosition = new Vector3(TipToV3.x + (43 - 10), TipToV3.y + 47, TipToV3.z);
                BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.CardBanker] = 0;
            }
            TipFromBool = false;
            TipToBool = false;
        }
    }
    #endregion

    //分牌重新確認卡片數量
    #region CardPointAngle
    void CardPointAngle()
    {
        if (CardTeam_Control == CardTeamList.Card1Team1 && BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card1Team1] == 1)
        {
            CardPoint = 0;
            BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card1Team1] = (byte)BJCard_Control.Seat1Team1.Count;
            DeleteCard();
            for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card1Team1] - CardPoint); i++)
            {
                CardAdd(BJCard_Control.Seat1Team1[CardPoint + i], CardPoint + i);
            }
            CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card1Team1];
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card1Team1] = 1;
            BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card1Team1] = 1;
            BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card1Team1] = 1;
            BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card1Team1] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card1Team2 && BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card1Team2] == 1)
        {
            CardPoint = 0;
            BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card1Team2] = (byte)BJCard_Control.Seat1Team2.Count;
            DeleteCard();
            for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card1Team2] - CardPoint); i++)
            {
                CardAdd(BJCard_Control.Seat1Team2[CardPoint + i], CardPoint + i);
            }
            CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card1Team2];
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card1Team2] = 1;
            BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card1Team2] = 1;
            BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card1Team2] = 1;
            BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card1Team2] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card2Team1 && BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card2Team1] == 1)
        {
            CardPoint = 0;
            BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card2Team1] = (byte)BJCard_Control.Seat2Team1.Count;
            DeleteCard();
            for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card2Team1] - CardPoint); i++)
            {
                CardAdd(BJCard_Control.Seat2Team1[CardPoint + i], CardPoint + i);
            }
            CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card2Team1];
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card2Team1] = 1;
            BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card2Team1] = 1;
            BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card2Team1] = 1;
            BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card2Team1] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card2Team2 && BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card2Team2] == 1)
        {
            CardPoint = 0;
            BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card2Team2] = (byte)BJCard_Control.Seat2Team2.Count;
            DeleteCard();
            for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card2Team2] - CardPoint); i++)
            {
                CardAdd(BJCard_Control.Seat2Team2[CardPoint + i], CardPoint + i);
            }
            CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card2Team2];
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card2Team2] = 1;
            BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card2Team2] = 1;
            BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card2Team2] = 1;
            BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card2Team2] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card3Team1 && BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card3Team1] == 1)
        {
            CardPoint = 0;
            BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card3Team1] = (byte)BJCard_Control.Seat3Team1.Count;
            DeleteCard();
            for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card3Team1] - CardPoint); i++)
            {
                CardAdd(BJCard_Control.Seat3Team1[CardPoint + i], CardPoint + i);
            }
            CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card3Team1];
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card3Team1] = 1;
            BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card3Team1] = 1;
            BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card3Team1] = 1;
            BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card3Team1] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card3Team2 && BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card3Team2] == 1)
        {
            CardPoint = 0;
            BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card3Team2] = (byte)BJCard_Control.Seat3Team2.Count;
            DeleteCard();
            for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card3Team2] - CardPoint); i++)
            {
                CardAdd(BJCard_Control.Seat3Team2[CardPoint + i], CardPoint + i);
            }
            CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card3Team2];
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card3Team2] = 1;
            BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card3Team2] = 1;
            BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card3Team2] = 1;
            BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card3Team2] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card4Team1 && BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card4Team1] == 1)
        {
            CardPoint = 0;
            BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card4Team1] = (byte)BJCard_Control.Seat4Team1.Count;
            DeleteCard();
            for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card4Team1] - CardPoint); i++)
            {
                CardAdd(BJCard_Control.Seat4Team1[CardPoint + i], CardPoint + i);
            }
            CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card4Team1];
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card4Team1] = 1;
            BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card4Team1] = 1;
            BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card4Team1] = 1;
            BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card4Team1] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card4Team2 && BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card4Team2] == 1)
        {
            CardPoint = 0;
            BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card4Team2] = (byte)BJCard_Control.Seat4Team2.Count;
            DeleteCard();
            for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card4Team2] - CardPoint); i++)
            {
                CardAdd(BJCard_Control.Seat4Team2[CardPoint + i], CardPoint + i);
            }
            CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card4Team2];
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card4Team2] = 1;
            BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card4Team2] = 1;
            BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card4Team2] = 1;
            BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card4Team2] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card5Team1 && BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card5Team1] == 1)
        {
            CardPoint = 0;
            BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card5Team1] = (byte)BJCard_Control.Seat5Team1.Count;
            DeleteCard();
            for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card5Team1] - CardPoint); i++)
            {
                CardAdd(BJCard_Control.Seat5Team1[CardPoint + i], CardPoint + i);
            }
            CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card5Team1];
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card5Team1] = 1;
            BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card5Team1] = 1;
            BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card5Team1] = 1;
            BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card5Team1] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card5Team2 && BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card5Team2] == 1)
        {
            CardPoint = 0;
            BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card5Team2] = (byte)BJCard_Control.Seat5Team2.Count;
            DeleteCard();
            for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card5Team2] - CardPoint); i++)
            {
                CardAdd(BJCard_Control.Seat5Team2[CardPoint + i], CardPoint + i);
            }
            CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card5Team2];
            BJCard_Control.PointCheckStart[(byte)CardTeamList.Card5Team2] = 1;
            BJCard_Control.PointFinallSeatStart[(byte)CardTeamList.Card5Team2] = 1;
            BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card5Team2] = 1;
            BJCard_Control.CardScoreboardStart[(byte)CardTeamList.Card5Team2] = 0;
        }
    }
    #endregion

    //確認是否強制結束
    #region CheckPoint
    void CancelCheck()
    {
        if (CardTeam_Control == CardTeamList.Card1Team1)
        {
            if (BJCard_Control.Seat1Team1.Count != CardPoint)
            {
                BJCard_Control.CardControl_End[(byte)CardTeamList.Card1Team1] = 1;
                BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card1Team1] = (byte)BJCard_Control.Seat1Team1.Count;
                DeleteCard();
                for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card1Team1] - CardPoint); i++)
                {
                    CardAdd(BJCard_Control.Seat1Team1[CardPoint + i], CardPoint + i);
                }
                CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card1Team1];
                BJCard_Control.PointCheckStart[(byte)CardTeamList.Card1Team1] = 1;
                BJCard_Control.CardMove_End[(byte)CardTeamList.Card1Team1] = 1;
                Tip1_Sprite.transform.localPosition = new Vector3((12 * (CardPoint - 1)) + 43, 47, 1);
                Tip2_Sprite.transform.localPosition = new Vector3((12 * (CardPoint - 1)) + (43 - 10), 47, 1);
            }
            BJCard_Control.CardControl_End[(byte)CardTeamList.Card1Team1] = 0;
            BJCard_Control.CheckPoint[(byte)CardTeamList.Card1Team1] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card1Team2)
        {
            if (BJCard_Control.Seat1Team2.Count != CardPoint)
            {
                BJCard_Control.CardControl_End[(byte)CardTeamList.Card1Team2] = 1;
                BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card1Team2] = (byte)BJCard_Control.Seat1Team2.Count;
                DeleteCard();
                for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card1Team2] - CardPoint); i++)
                {
                    CardAdd(BJCard_Control.Seat1Team2[CardPoint + i], CardPoint + i);
                }
                CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card1Team2];

                BJCard_Control.PointCheckStart[(byte)CardTeamList.Card1Team2] = 1;
                BJCard_Control.CardMove_End[(byte)CardTeamList.Card1Team2] = 1;
                Tip1_Sprite.transform.localPosition = new Vector3((12 * (CardPoint - 1)) + 43, 47, 1);
                Tip2_Sprite.transform.localPosition = new Vector3((12 * (CardPoint - 1)) + (43 - 10), 47, 1);
            }
            BJCard_Control.CardControl_End[(byte)CardTeamList.Card1Team2] = 0;
            BJCard_Control.CheckPoint[(byte)CardTeamList.Card1Team2] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card2Team1)
        {
            if (BJCard_Control.Seat2Team1.Count != CardPoint)
            {
                BJCard_Control.CardControl_End[(byte)CardTeamList.Card2Team1] = 1;
                BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card2Team1] = (byte)BJCard_Control.Seat2Team1.Count;
                DeleteCard();
                for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card2Team1] - CardPoint); i++)
                {
                    CardAdd(BJCard_Control.Seat2Team1[CardPoint + i], CardPoint + i);
                }
                CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card2Team1];
                BJCard_Control.PointCheckStart[(byte)CardTeamList.Card2Team1] = 1;
                BJCard_Control.CardMove_End[(byte)CardTeamList.Card2Team1] = 1;
                Tip1_Sprite.transform.localPosition = new Vector3((12 * (CardPoint - 1)) + 43, 47, 1);
                Tip2_Sprite.transform.localPosition = new Vector3((12 * (CardPoint - 1)) + (43 - 10), 47, 1);
            }
            BJCard_Control.CardControl_End[(byte)CardTeamList.Card2Team1] = 0;
            BJCard_Control.CheckPoint[(byte)CardTeamList.Card2Team1] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card2Team2)
        {
            if (BJCard_Control.Seat2Team2.Count != CardPoint)
            {
                BJCard_Control.CardControl_End[(byte)CardTeamList.Card2Team2] = 1;
                BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card2Team2] = (byte)BJCard_Control.Seat2Team2.Count;
                DeleteCard();
                for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card2Team2] - CardPoint); i++)
                {
                    CardAdd(BJCard_Control.Seat2Team2[CardPoint + i], CardPoint + i);
                }
                CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card2Team2];

                BJCard_Control.PointCheckStart[(byte)CardTeamList.Card2Team2] = 1;
                BJCard_Control.CardMove_End[(byte)CardTeamList.Card2Team2] = 1;
                Tip1_Sprite.transform.localPosition = new Vector3((12 * (CardPoint - 1)) + 43, 47, 1);
                Tip2_Sprite.transform.localPosition = new Vector3((12 * (CardPoint - 1)) + (43 - 10), 47, 1);
            }
            BJCard_Control.CardControl_End[(byte)CardTeamList.Card2Team2] = 0;
            BJCard_Control.CheckPoint[(byte)CardTeamList.Card2Team2] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card3Team1)
        {
            if (BJCard_Control.Seat3Team1.Count != CardPoint)
            {
                BJCard_Control.CardControl_End[(byte)CardTeamList.Card3Team1] = 1;
                BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card3Team1] = (byte)BJCard_Control.Seat3Team1.Count;
                DeleteCard();
                for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card3Team1] - CardPoint); i++)
                {
                    CardAdd(BJCard_Control.Seat3Team1[CardPoint + i], CardPoint + i);
                }
                CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card3Team1];
                BJCard_Control.PointCheckStart[(byte)CardTeamList.Card3Team1] = 1;
                BJCard_Control.CardMove_End[(byte)CardTeamList.Card3Team1] = 1;
                Tip1_Sprite.transform.localPosition = new Vector3((12 * (CardPoint - 1)) + 43, 47, 1);
                Tip2_Sprite.transform.localPosition = new Vector3((12 * (CardPoint - 1)) + (43 - 10), 47, 1);
            }
            BJCard_Control.CardControl_End[(byte)CardTeamList.Card3Team1] = 0;
            BJCard_Control.CheckPoint[(byte)CardTeamList.Card3Team1] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card3Team2)
        {
            if (BJCard_Control.Seat3Team2.Count != CardPoint)
            {
                BJCard_Control.CardControl_End[(byte)CardTeamList.Card3Team2] = 1;
                BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card3Team2] = (byte)BJCard_Control.Seat3Team2.Count;
                DeleteCard();
                for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card3Team2] - CardPoint); i++)
                {
                    CardAdd(BJCard_Control.Seat3Team2[CardPoint + i], CardPoint + i);
                }
                CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card3Team2];

                BJCard_Control.PointCheckStart[(byte)CardTeamList.Card3Team2] = 1;
                BJCard_Control.CardMove_End[(byte)CardTeamList.Card3Team2] = 1;
                Tip1_Sprite.transform.localPosition = new Vector3((12 * (CardPoint - 1)) + 43, 47, 1);
                Tip2_Sprite.transform.localPosition = new Vector3((12 * (CardPoint - 1)) + (43 - 10), 47, 1);
            }
            BJCard_Control.CardControl_End[(byte)CardTeamList.Card3Team2] = 0;
            BJCard_Control.CheckPoint[(byte)CardTeamList.Card3Team2] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card4Team1)
        {
            if (BJCard_Control.Seat4Team1.Count != CardPoint)
            {
                BJCard_Control.CardControl_End[(byte)CardTeamList.Card4Team1] = 1;
                BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card4Team1] = (byte)BJCard_Control.Seat4Team1.Count;
                DeleteCard();
                for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card4Team1] - CardPoint); i++)
                {
                    CardAdd(BJCard_Control.Seat4Team1[CardPoint + i], CardPoint + i);
                }
                CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card4Team1];
                BJCard_Control.PointCheckStart[(byte)CardTeamList.Card4Team1] = 1;
                BJCard_Control.CardMove_End[(byte)CardTeamList.Card4Team1] = 1;
                Tip1_Sprite.transform.localPosition = new Vector3((12 * (CardPoint - 1)) + 43, 47, 1);
                Tip2_Sprite.transform.localPosition = new Vector3((12 * (CardPoint - 1)) + (43 - 10), 47, 1);
            }
            BJCard_Control.CardControl_End[(byte)CardTeamList.Card4Team1] = 0;
            BJCard_Control.CheckPoint[(byte)CardTeamList.Card4Team1] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card4Team2)
        {
            if (BJCard_Control.Seat4Team2.Count != CardPoint)
            {
                BJCard_Control.CardControl_End[(byte)CardTeamList.Card4Team2] = 1;
                BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card4Team2] = (byte)BJCard_Control.Seat4Team2.Count;
                DeleteCard();
                for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card4Team2] - CardPoint); i++)
                {
                    CardAdd(BJCard_Control.Seat4Team2[CardPoint + i], CardPoint + i);
                }
                CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card4Team2];

                BJCard_Control.PointCheckStart[(byte)CardTeamList.Card4Team2] = 1;
                BJCard_Control.CardMove_End[(byte)CardTeamList.Card4Team2] = 1;
                Tip1_Sprite.transform.localPosition = new Vector3((12 * (CardPoint - 1)) + 43, 47, 1);
                Tip2_Sprite.transform.localPosition = new Vector3((12 * (CardPoint - 1)) + (43 - 10), 47, 1);
            }
            BJCard_Control.CardControl_End[(byte)CardTeamList.Card4Team2] = 0;
            BJCard_Control.CheckPoint[(byte)CardTeamList.Card4Team2] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card5Team1)
        {
            if (BJCard_Control.Seat5Team1.Count != CardPoint)
            {
                BJCard_Control.CardControl_End[(byte)CardTeamList.Card5Team1] = 1;
                BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card5Team1] = (byte)BJCard_Control.Seat5Team1.Count;
                DeleteCard();
                for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card5Team1] - CardPoint); i++)
                {
                    CardAdd(BJCard_Control.Seat5Team1[CardPoint + i], CardPoint + i);
                }
                CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card5Team1];
                BJCard_Control.PointCheckStart[(byte)CardTeamList.Card5Team1] = 1;
                BJCard_Control.CardMove_End[(byte)CardTeamList.Card5Team1] = 1;
                Tip1_Sprite.transform.localPosition = new Vector3((12 * (CardPoint - 1)) + 43, 47, 1);
                Tip2_Sprite.transform.localPosition = new Vector3((12 * (CardPoint - 1)) + (43 - 10), 47, 1);
            }
            BJCard_Control.CardControl_End[(byte)CardTeamList.Card5Team1] = 0;
            BJCard_Control.CheckPoint[(byte)CardTeamList.Card5Team1] = 0;
        }
        else if (CardTeam_Control == CardTeamList.Card5Team2)
        {
            if (BJCard_Control.Seat5Team2.Count != CardPoint)
            {
                BJCard_Control.CardControl_End[(byte)CardTeamList.Card5Team2] = 1;
                BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card5Team2] = (byte)BJCard_Control.Seat5Team2.Count;
                DeleteCard();
                for (int i = 0; i < (int)(BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card5Team2] - CardPoint); i++)
                {
                    CardAdd(BJCard_Control.Seat5Team2[CardPoint + i], CardPoint + i);
                }
                CardPoint = BJCard_Control.SeatTeamPoint[(byte)CardTeamList.Card5Team2];

                BJCard_Control.PointCheckStart[(byte)CardTeamList.Card5Team2] = 1;
                BJCard_Control.CardMove_End[(byte)CardTeamList.Card5Team2] = 1;
                Tip1_Sprite.transform.localPosition = new Vector3((12 * (CardPoint - 1)) + 43, 47, 1);
                Tip2_Sprite.transform.localPosition = new Vector3((12 * (CardPoint - 1)) + (43 - 10), 47, 1);
            }
            BJCard_Control.CardControl_End[(byte)CardTeamList.Card5Team2] = 0;
            BJCard_Control.CheckPoint[(byte)CardTeamList.Card5Team2] = 0;
        }
    }
    #endregion

    //分牌移動結束
    public void MoveEnd()
    {
        if (CardTeam_Control == CardTeamList.Card1Team1 && BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card1Team1] == 1)
            BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card1Team1] = 0;
        else if (CardTeam_Control == CardTeamList.Card1Team2 && BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card1Team2] == 1)
            BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card1Team2] = 0;
        else if (CardTeam_Control == CardTeamList.Card2Team1 && BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card2Team1] == 1)
            BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card2Team1] = 0;
        else if (CardTeam_Control == CardTeamList.Card2Team2 && BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card2Team2] == 1)
            BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card2Team2] = 0;
        else if (CardTeam_Control == CardTeamList.Card3Team1 && BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card3Team1] == 1)
            BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card3Team1] = 0;
        else if (CardTeam_Control == CardTeamList.Card3Team2 && BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card3Team2] == 1)
            BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card3Team2] = 0;
        else if (CardTeam_Control == CardTeamList.Card4Team1 && BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card4Team1] == 1)
            BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card4Team1] = 0;
        else if (CardTeam_Control == CardTeamList.Card4Team2 && BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card4Team2] == 1)
            BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card4Team2] = 0;
        else if (CardTeam_Control == CardTeamList.Card5Team1 && BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card5Team1] == 1)
            BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card5Team1] = 0;
        else if (CardTeam_Control == CardTeamList.Card5Team2 && BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card5Team2] == 1)
            BJCard_Control.ScoreBoardStart[(byte)CardTeamList.Card5Team2] = 0;
        else if (CardTeam_Control == CardTeamList.CardBanker && BJCard_Control.ScoreBoardStart[(byte)CardTeamList.CardBanker] == 1)
            BJCard_Control.ScoreBoardStart[(byte)CardTeamList.CardBanker] = 0;
    }

    void DataInit()
    {
        CardPoint = 0;
        OpenClose_Bool = false;
        OpenCloseStart = false;
        ShowOK = false;
        FinallSeat = false;
        TipFromBool = false;
        TipToBool = false;
        FinallVector3 = new Vector3(0, 0, 0);
        DeleteCard();
    }
}
