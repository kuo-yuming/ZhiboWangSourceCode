using UnityEngine;
using System.Collections;
using CardTeamListClass;

public class DealerCard_Move : MonoBehaviour {
    public TweenPosition DealerCard_Position;
    public TweenRotation DealerCard_Rotation;
    public TweenScale DealerCard_Scale;
    public UISprite DealerCard_Sprite;
    public static CardTeamList ThisCardList = CardTeamList.Card1Team1;
    public static bool BankerSenceCard = false;

    public TweenPosition SenceCard_Position;

    public Vector3 DealerCardStartV3;
    public Vector3 DealerCardFirstPointV3;
    public static Vector3 FinallDealerCardMove = new Vector3(0,0,0);
    public static bool MoveOK = false;
    public static byte CardNumber = 0;

    public static bool DealerShowCancel_Bool = false;//強制結束

    float DelayTime = 0;
    bool FirstMoveEnd = false;
    bool SecneMove = false;
    bool FinallEnd = false;
    // Use this for initialization
    void Start () {
        //  DealerCard_Position.duration = 1;
        DataInit();
    }
	
	// Update is called once per frame
	void Update () {
        if (MoveOK)
        {
            FirstPoint_Move();
            MoveOK = false;
        }

        if (FirstMoveEnd)
        {
            CardOpen();
        }

        if (SecneMove)
        {
            SecneMove = false;
        }

        if (DealerShowCancel_Bool)
        {
            CancelShow();
            DealerShowCancel_Bool = false;
        }
	}

    void FirstPoint_Move()
    {
        DealerCard_Position.PlayForward();
        DealerCard_Rotation.PlayForward();
        SenceCard_Position.PlayForward();
    }

    void CardOpen()
    {
        byte ChangePoint = (byte)(((CardNumber - 1) % 13) + 1);    

        DealerCard_Position.from = DealerCardFirstPointV3;
        DealerCard_Position.to = FinallDealerCardMove;   
        
        if (DelayTime < 1)
        {
            DelayTime += Time.deltaTime;
            if (DelayTime > 0.4)
            {
                if (CardNumber == 0)
                {
                    DealerCard_Sprite.spriteName = "bg_pokerback_1";
                }
                else if (CardNumber > 0 && CardNumber < 14)
                {
                    DealerCard_Sprite.spriteName = "bg_poker_d" + ChangePoint;
                }
                else if (CardNumber >= 14 && CardNumber < 27)
                {
                    DealerCard_Sprite.spriteName = "bg_poker_c" + ChangePoint;
                }
                else if (CardNumber >= 27 && CardNumber < 40)
                {
                    DealerCard_Sprite.spriteName = "bg_poker_b" + ChangePoint;
                }
                else if (CardNumber >= 40)
                {
                    DealerCard_Sprite.spriteName = "bg_poker_a" + ChangePoint;
                }
            }
        }
        else
        {
            DelayTime = 0;
            SecneMove = true;
            DealerCard_Position.ResetToBeginning();
            DealerCard_Position.PlayForward();
            if (ThisCardList != CardTeamList.Card1Team1 && ThisCardList != CardTeamList.Card1Team2 && ThisCardList != CardTeamList.CardBanker)
            {
                DealerCard_Scale.PlayForward();
            }
            FirstMoveEnd = false;
            FinallEnd = true;
        }
    }

    public void Move_End()
    {
        if (!FinallEnd)
        {
            FirstMoveEnd = true;
        }
        else
        {
            if (BJMainGame_Control.CardSenceList_Bool)
            {
                if (BJCard_Control.AddCard1Show || BJCard_Control.AddCard2Show)
                {
                    if (BJCard_Control.AddCard1Show && BJCard_Control.AddCard2Show)
                    {
                        BJCard_Control.AddCard1Show = false;
                    }
                    else
                    {
                        BJCard_Control.AddCard2Show = false;
                        if (ThisCardList != CardTeamList.Card1Team1 && ThisCardList != CardTeamList.Card1Team2)
                            BJMainGame_Control.CardSenceList_Bool = false;
                        Cash_Control.OnBetClick = false;
                    }
                    BJCard_Control.AddCardShow = true;
                }
                else
                {
                    Cash_Control.OnBetClick = false;
                }
            }
            else
            {
                Cash_Control.OnBetClick = false;
            }
            Card_Move_Control.ShowOK = true;
            if (ThisCardList == CardTeamList.CardBanker && BankerSenceCard)
            {
                BJMainGame_Control.FirstGetCard_Bool = true;
                BankerSenceCard = false;
            }
            DataInit();
            FinallEnd = false;
        }
    }

    //強制結束處理
    void CancelShow()
    {
        FirstMoveEnd = false;
        BJCard_Control.AddCard1Show = false;
        BJCard_Control.AddCard2Show = false;
        BJCard_Control.AddCardShow = false;
        Card_Move_Control.ShowOK = false;
        FinallEnd = false;
        DataInit();
        //分牌動畫還沒有表演完
        if (BJCard_Control.AddCard.Count != 0)
        {
            CardTeamList SaveList = CardTeamList.Card1Team1;
            byte SaveCardNumber = 0;
            foreach (var item in BJCard_Control.AddCard)
            {
                SaveList = item.Key;
                SaveCardNumber = (byte)item.Value;

                if (SaveList == CardTeamList.Card1Team1)
                    BJCard_Control.Seat1Team1.Add(SaveCardNumber);
                else if (SaveList == CardTeamList.Card1Team2)
                    BJCard_Control.Seat1Team2.Add(SaveCardNumber);
                if (SaveList == CardTeamList.Card2Team1)
                    BJCard_Control.Seat2Team1.Add(SaveCardNumber);
                else if (SaveList == CardTeamList.Card2Team2)
                    BJCard_Control.Seat2Team2.Add(SaveCardNumber);
                if (SaveList == CardTeamList.Card3Team1)
                    BJCard_Control.Seat3Team1.Add(SaveCardNumber);
                else if (SaveList == CardTeamList.Card3Team2)
                    BJCard_Control.Seat3Team2.Add(SaveCardNumber);
                if (SaveList == CardTeamList.Card4Team1)
                    BJCard_Control.Seat4Team1.Add(SaveCardNumber);
                else if (SaveList == CardTeamList.Card4Team2)
                    BJCard_Control.Seat4Team2.Add(SaveCardNumber);
                if (SaveList == CardTeamList.Card5Team1)
                    BJCard_Control.Seat5Team1.Add(SaveCardNumber);
                else if (SaveList == CardTeamList.Card5Team2)
                    BJCard_Control.Seat5Team2.Add(SaveCardNumber);
            }
            BJCard_Control.CheckPoint[(byte)CardTeamList.Card1Team1] = 1;
            BJCard_Control.CheckPoint[(byte)CardTeamList.Card1Team2] = 1;
            BJCard_Control.CheckPoint[(byte)CardTeamList.Card2Team1] = 1;
            BJCard_Control.CheckPoint[(byte)CardTeamList.Card2Team2] = 1;
            BJCard_Control.CheckPoint[(byte)CardTeamList.Card3Team1] = 1;
            BJCard_Control.CheckPoint[(byte)CardTeamList.Card3Team2] = 1;
            BJCard_Control.CheckPoint[(byte)CardTeamList.Card4Team1] = 1;
            BJCard_Control.CheckPoint[(byte)CardTeamList.Card4Team2] = 1;
            BJCard_Control.CheckPoint[(byte)CardTeamList.Card5Team1] = 1;
            BJCard_Control.CheckPoint[(byte)CardTeamList.Card5Team2] = 1;
        }
    }

    void DataInit()
    {
        DealerCard_Sprite.spriteName = "bg_pokerback_1";
        DealerCard_Position.from = DealerCardStartV3;
        DealerCard_Position.to = DealerCardFirstPointV3;
        DealerCard_Position.ResetToBeginning();
        DealerCard_Rotation.ResetToBeginning();
        SenceCard_Position.ResetToBeginning();
        DealerCard_Scale.enabled = false;
        DealerCard_Scale.ResetToBeginning();
        BJMainGame_Control.FirstCardShow_Bool = false;
    }
}
