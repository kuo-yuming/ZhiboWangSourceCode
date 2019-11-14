using UnityEngine;
using System.Collections;

public class CardOpen_Control : MonoBehaviour
{
    public UISprite CardOpen1_Sprite;
    public UISprite CardOpen2_Sprite;
    public UISprite CardOpen3_Sprite;
    public UISprite CardOpenBlack_Sprite;
    public UISprite MoveCard_Sprite;
    public TweenPosition FirstMoveCard;
    public TweenPosition FirstMoveCard2;
    public TweenPosition FirstMoveCard3;
    public TweenPosition FirstMoveCard4;
    public TweenPosition MoveCard_Position;
    public TweenScale MoveCard_Scale;
    //牌移動
    private Vector3 Banker1_V3 = new Vector3(361, 200, 0);//165
    private Vector3 Banker2_V3 = new Vector3(418, 200, 0);
    private Vector3 Banker3_V3 = new Vector3(475, 200, 0);
    private Vector3 Player1_V3 = new Vector3(-361, 200, 0);
    private Vector3 Player2_V3 = new Vector3(-304, 200, 0);
    private Vector3 Player3_V3 = new Vector3(-247, 200, 0);

    public static byte[] CardOpenSeat = new byte[6];//0-2莊  3-5閒  //0沒牌 1有牌

    public static bool CardAnimationShow_Bool = false;

    int Open1Number = 0;
    float OpenTime = 0.0f;
    bool ShowTwoBool = false;
    bool NextCardOpenBool = false;
    bool CardMoveBool = false;
    bool CardMoveTimeBool = false;

    bool OpenSound = false;
    bool MoveSound = false;

    public TweenPosition[] BankerCardTP = new TweenPosition[3];
    public TweenPosition[] PlayerCardTP = new TweenPosition[3];
    public Vector3[] BankerCard1Move;
    public Vector3[] BankerCard2Move;
    public Vector3[] BankerCard3Move;
    public Vector3[] PlayerCard1Move;
    public Vector3[] PlayerCard2Move;
    public Vector3[] PlayerCard3Move;
    public static bool FourCardWiner_Bool = false;
    public TweenScale OpenCardSize;
    bool OpenSize = false;
    // Use this for initialization
    void Start()
    {
        CardOpen1_Sprite.spriteName = "CardOpen_01";
        MoveCard_Sprite.spriteName = "01";
        CardOpen1_Sprite.enabled = false;
        CardOpen2_Sprite.enabled = false;
        CardOpen3_Sprite.enabled = false;
        CardOpenBlack_Sprite.enabled = false;
        MoveCard_Sprite.enabled = false;
        FirstMoveCard.ResetToBeginning();
        FirstMoveCard2.ResetToBeginning();
        FirstMoveCard3.ResetToBeginning();
        FirstMoveCard4.ResetToBeginning();
        MoveCard_Position.ResetToBeginning();
        MoveCard_Scale.ResetToBeginning();
        Open1Number = 1;
        OpenTime = 0.0f;
        NextCardOpenBool = false;
        CardMoveBool = false;
        CardMoveTimeBool = false;
        ShowTwoBool = false;
        CardAnimationShow_Bool = false;
        FourCardWiner_Bool = false;

        for (int i = 0; i < 6; i++)
        {
            CardOpenSeat[i] = 0;
        }

        if (!VersionDef.BaccaratCardSize)
        {
            Banker1_V3 = new Vector3(361, 200, 0);//165
            Banker2_V3 = new Vector3(418, 200, 0);
            Banker3_V3 = new Vector3(475, 200, 0);
            Player1_V3 = new Vector3(-361, 200, 0);
            Player2_V3 = new Vector3(-304, 200, 0);
            Player3_V3 = new Vector3(-247, 200, 0);
            MoveCard_Scale.from = new Vector3(1.02f, 1.05f, 1);
            MoveCard_Position.from = new Vector3(0, -2, 0);
        }
        else
        {
            Banker1_V3 = new Vector3(361, 200, 0);//165
            Banker2_V3 = new Vector3(418, 200, 0);
            Banker3_V3 = new Vector3(475, 200, 0);
            Player1_V3 = new Vector3(-361, 200, 0);
            Player2_V3 = new Vector3(-304, 200, 0);
            Player3_V3 = new Vector3(-247, 200, 0);
            MoveCard_Scale.from = new Vector3(2.04f, 2.1f, 1);
            MoveCard_Position.from = new Vector3(0, 0, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CardAnimationShow_Bool)
        {
            CardAnimationVoid();
        }

        if (Open1Number < 10)
        {
            CardOpen1_Sprite.spriteName = "CardOpen_0" + Open1Number;
        }
        else
        {
            CardOpen1_Sprite.spriteName = "CardOpen_" + Open1Number;
        }

        if (FourCardWiner_Bool)
        {
            if (FourCard_Control.AllFCBetMoney[0] > FourCard_Control.AllFCBetMoney[1])
            {
                BankerCardTP[0].from = BankerCard1Move[2];
                BankerCardTP[0].to = BankerCard1Move[3];
                BankerCardTP[0].ResetToBeginning();
                BankerCardTP[1].from = BankerCard2Move[1];
                BankerCardTP[1].to = BankerCard2Move[2];
                BankerCardTP[1].ResetToBeginning();
                BankerCardTP[0].PlayForward();
                BankerCardTP[1].PlayForward();
                BankerCardTP[2].PlayForward();
            }
            else if (FourCard_Control.AllFCBetMoney[0] < FourCard_Control.AllFCBetMoney[1])
            {
                PlayerCardTP[0].from = PlayerCard1Move[2];
                PlayerCardTP[0].to = PlayerCard1Move[3];
                PlayerCardTP[0].ResetToBeginning();
                PlayerCardTP[1].from = PlayerCard2Move[1];
                PlayerCardTP[1].to = PlayerCard2Move[2];
                PlayerCardTP[1].ResetToBeginning();
                PlayerCardTP[0].PlayForward();
                PlayerCardTP[1].PlayForward();
                PlayerCardTP[2].PlayForward();
            }

            FourCardWiner_Bool = false;
        }
    }

    void CardAnimationVoid()
    {
        ////////開牌表演一
        if (OpenTime < 0.03f && !NextCardOpenBool && !CardMoveBool && !CardMoveTimeBool)
        {
            CardOpen1_Sprite.enabled = true;
            OpenTime += Time.deltaTime;
            if (!OpenSize && VersionDef.BaccaratCardSize)
            {
                OpenCardSize.ResetToBeginning();
                OpenCardSize.enabled = true;
                OpenSize = true;
            }
        }
        else if (!NextCardOpenBool && !CardMoveBool && !CardMoveTimeBool)
        {
            CardNameVoid();
            if (Open1Number < 17)
            {
                if (Open1Number == 1)
                {
                    GameSound.DrawCard_Bool = true;
                }
                Open1Number++;
            }
            else
            {
                NextCardOpenBool = true;
                CardOpen1_Sprite.enabled = false;
                Open1Number = 1;
                FirstMoveCard.ResetToBeginning();
                CardOpen2_Sprite.enabled = true;
                CardSeatSave();
            }
            OpenTime = 0;
        }

        ////////開牌表演二
        if (OpenTime < 0.5f && NextCardOpenBool && !CardMoveBool && !CardMoveTimeBool && !ShowTwoBool)
        {
            OpenTime += Time.deltaTime;

        }
        else if (NextCardOpenBool && !CardMoveBool && !CardMoveTimeBool && !ShowTwoBool)
        {
            if (!OpenSound)
            {
                GameSound.OpenCard_Bool = true;
                OpenSound = true;
            }
            ShowTwoBool = true;
            CardOpen3_Sprite.enabled = true;
            CardOpenBlack_Sprite.enabled = true;
            MoveCard_Sprite.enabled = false;
            FirstMoveCard2.ResetToBeginning();
            FirstMoveCard3.ResetToBeginning();
            FirstMoveCard4.ResetToBeginning();
            FirstMoveCard.PlayForward();
            FirstMoveCard2.PlayForward();
            FirstMoveCard3.PlayForward();
            FirstMoveCard4.PlayForward();
            MoveCard_Position.ResetToBeginning();
            MoveCard_Scale.ResetToBeginning();
            OpenTime = 0;
            OpenVoid();
        }

        ////////////開牌表演三
        if (OpenTime < 1 && NextCardOpenBool && CardMoveBool && !CardMoveTimeBool)
        {
            OpenSize = false;
            OpenTime += Time.deltaTime;
            if (OpenTime > 0.7f)
            {
                if (!MoveSound)
                {
                    GameSound.CardMove_Bool = true;
                    MoveSound = true;
                } 
            }
        }
        else if (NextCardOpenBool && CardMoveBool && !CardMoveTimeBool)
        {
            CardMoveTimeBool = true;
            MoveCard_Position.PlayForward();
            MoveCard_Scale.PlayForward();
            OpenTime = 0;
            if (CardOpenSeat[3] != 0)
            {
            }
            else if (CardOpenSeat[0] != 0)
            {
            }
            else if (CardOpenSeat[4] != 0)
            {
                PlayerCardTP[0].PlayForward();
            }
            else if (CardOpenSeat[1] != 0)
            {
                BankerCardTP[0].PlayForward();
            }
            else if (CardOpenSeat[5] != 0)
            {
                PlayerCardTP[0].PlayForward();
                PlayerCardTP[1].PlayForward();
            }
            else if (CardOpenSeat[2] != 0)
            {
                BankerCardTP[0].PlayForward();
                BankerCardTP[1].PlayForward();
            }
        }
    }

    void CardNameVoid()
    {
        if (CardOpenSeat[3] != 0)
        {
            if (Card_Control.PlayerCard[0] < 10)
            {
                MoveCard_Sprite.spriteName = "0" + Card_Control.PlayerCard[0].ToString();
                CardOpen3_Sprite.spriteName = "0" + Card_Control.PlayerCard[0].ToString();
            }
            else
            {
                MoveCard_Sprite.spriteName = Card_Control.PlayerCard[0].ToString();
                CardOpen3_Sprite.spriteName = Card_Control.PlayerCard[0].ToString();
            }
        }
        else if (CardOpenSeat[0] != 0)
        {
            if (Card_Control.BankerCard[0] < 10)
            {
                MoveCard_Sprite.spriteName = "0" + Card_Control.BankerCard[0].ToString();
                CardOpen3_Sprite.spriteName = "0" + Card_Control.BankerCard[0].ToString();
            }
            else
            {
                MoveCard_Sprite.spriteName = Card_Control.BankerCard[0].ToString();
                CardOpen3_Sprite.spriteName = Card_Control.BankerCard[0].ToString();
            }
        }
        else if (CardOpenSeat[4] != 0)
        {
            if (Card_Control.PlayerCard[1] < 10)
            {
                MoveCard_Sprite.spriteName = "0" + Card_Control.PlayerCard[1].ToString();
                CardOpen3_Sprite.spriteName = "0" + Card_Control.PlayerCard[1].ToString();
            }
            else
            {
                MoveCard_Sprite.spriteName = Card_Control.PlayerCard[1].ToString();
                CardOpen3_Sprite.spriteName = Card_Control.PlayerCard[1].ToString();
            }
        }
        else if (CardOpenSeat[1] != 0)
        {
            if (Card_Control.BankerCard[1] < 10)
            {
                MoveCard_Sprite.spriteName = "0" + Card_Control.BankerCard[1].ToString();
                CardOpen3_Sprite.spriteName = "0" + Card_Control.BankerCard[1].ToString();
            }
            else
            {
                MoveCard_Sprite.spriteName = Card_Control.BankerCard[1].ToString();
                CardOpen3_Sprite.spriteName = Card_Control.BankerCard[1].ToString();
            }
        }
        else if (CardOpenSeat[5] != 0)
        {
            if (Card_Control.PlayerCard[2] < 10)
            {
                MoveCard_Sprite.spriteName = "0" + Card_Control.PlayerCard[2].ToString();
                CardOpen3_Sprite.spriteName = "0" + Card_Control.PlayerCard[2].ToString();
            }
            else
            {
                MoveCard_Sprite.spriteName = Card_Control.PlayerCard[2].ToString();
                CardOpen3_Sprite.spriteName = Card_Control.PlayerCard[2].ToString();
            }
        }
        else if (CardOpenSeat[2] != 0)
        {
            if (Card_Control.BankerCard[2] < 10)
            {
                MoveCard_Sprite.spriteName = "0" + Card_Control.BankerCard[2].ToString();
                CardOpen3_Sprite.spriteName = "0" + Card_Control.BankerCard[2].ToString();
            }
            else
            {
                MoveCard_Sprite.spriteName = Card_Control.BankerCard[2].ToString();
                CardOpen3_Sprite.spriteName = Card_Control.BankerCard[2].ToString();
            }
        }
    }

    public void MoveEndVoid()
    {
        CloseVoid();
        MoveCard_Sprite.enabled = false;
        CardAnimationShow_Bool = false;
        NextCardOpenBool = false;
        CardMoveTimeBool = false;
        CardMoveBool = false;
        ShowTwoBool = false;
        MoveSound = false;
    }

    void OpenVoid()
    {
        if (CardOpenSeat[3] != 0)
        {
            MoveCard_Position.to = Player1_V3;
        }
        else if (CardOpenSeat[0] != 0)
        {
            MoveCard_Position.to = Banker1_V3;
        }
        else if (CardOpenSeat[4] != 0)
        {
            MoveCard_Position.to = Player2_V3;
        }
        else if (CardOpenSeat[1] != 0)
        {
            MoveCard_Position.to = Banker2_V3;
        }
        else if (CardOpenSeat[5] != 0)
        {
            MoveCard_Position.to = Player3_V3;
        }
        else if (CardOpenSeat[2] != 0)
        {
            MoveCard_Position.to = Banker3_V3;
        }
    }

    void CloseVoid()
    {
        if (CardOpenSeat[3] != 0)
        {
            CardOpenSeat[3] = 0;
            Card_Control.DelayTimerMax = 2;
            Card_Control.PlayerCardOpenNumber[0] = 1;
        }
        else if (CardOpenSeat[0] != 0)
        {
            CardOpenSeat[0] = 0;
            Card_Control.DelayTimerMax = 2;
            Card_Control.BankerCardOpenNumber[0] = 1;
        }
        else if (CardOpenSeat[4] != 0)
        {
            CardOpenSeat[4] = 0;
            Card_Control.DelayTimerMax = 2;
            Card_Control.PlayerCardOpenNumber[1] = 1;
        }
        else if (CardOpenSeat[1] != 0)
        {
            CardOpenSeat[1] = 0;
            Card_Control.DelayTimerMax = 2.5f;
            Card_Control.BankerCardOpenNumber[1] = 1;
        }
        else if (CardOpenSeat[5] != 0)
        {
            CardOpenSeat[5] = 0;
            Card_Control.DelayTimerMax = 2.5f;
            Card_Control.PlayerCardOpenNumber[2] = 1;
        }
        else if (CardOpenSeat[2] != 0)
        {
            CardOpenSeat[2] = 0;
            Card_Control.BankerCardOpenNumber[2] = 1;
        }

        if (CardOpenSeat[0] == 0 && CardOpenSeat[1] == 0 && CardOpenSeat[2] == 0 && CardOpenSeat[3] == 0 && CardOpenSeat[4] == 0 && CardOpenSeat[5] == 0)
        {
            Card_Control.CardShowOverBool = true;
            Card_Control.DelayTimerMax = 2;
        }
    }

    public void FirstMoveEndVoid()
    {
        CardOpen2_Sprite.enabled = false;
        CardOpen3_Sprite.enabled = false;
        CardOpenBlack_Sprite.enabled = false;
        MoveCard_Sprite.enabled = true;
        CardMoveBool = true;
        OpenSound = false;
    }

    void CardSeatSave()
    {
        if (CardOpenSeat[3] != 0)
        {
            PlayerCardTP[0].from = PlayerCard1Move[0];
            PlayerCardTP[0].to = PlayerCard1Move[1];
            PlayerCardTP[0].ResetToBeginning();
        }
        else if (CardOpenSeat[0] != 0)
        {
            BankerCardTP[0].from = BankerCard1Move[0];
            BankerCardTP[0].to = BankerCard1Move[1];
            BankerCardTP[0].ResetToBeginning();
        }
        else if (CardOpenSeat[4] != 0)
        {
            PlayerCardTP[1].from = PlayerCard2Move[0];
            PlayerCardTP[1].to = PlayerCard2Move[1];
            PlayerCardTP[1].ResetToBeginning();
        }
        else if (CardOpenSeat[1] != 0)
        {
            BankerCardTP[1].from = BankerCard2Move[0];
            BankerCardTP[1].to = BankerCard2Move[1];
            BankerCardTP[1].ResetToBeginning();
        }
        else if (CardOpenSeat[5] != 0)
        {
            PlayerCardTP[0].from = PlayerCard1Move[1];
            PlayerCardTP[0].to = PlayerCard1Move[2];
            PlayerCardTP[0].ResetToBeginning();
            PlayerCardTP[2].from = PlayerCard3Move[0];
            PlayerCardTP[2].to = PlayerCard3Move[1];
            PlayerCardTP[2].ResetToBeginning();
        }
        else if (CardOpenSeat[2] != 0)
        {
            BankerCardTP[0].from = BankerCard1Move[1];
            BankerCardTP[0].to = BankerCard1Move[2];
            BankerCardTP[0].ResetToBeginning();
            BankerCardTP[2].from = BankerCard3Move[0];
            BankerCardTP[2].to = BankerCard3Move[1];
            BankerCardTP[2].ResetToBeginning();
        }
    }
}
