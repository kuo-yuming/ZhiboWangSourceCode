using UnityEngine;
using System.Collections;
using GameCore.Manager.BlackJack;

public class StateShow_Control : MonoBehaviour {

    public static bool FirstCheck = false;//剛進遊戲時第一次狀態確認

    //背底透明度控制
    public static bool Backgrond_Bool = false;
    public UISprite Background_Sprite;
    public BoxCollider Background_Box;
    public TweenColor Background_Color;
    bool FirstShow = false;

    //新局開始
    public UISprite NewGameState_Sprite;
    public BoxCollider NewGameState_Box;
    public UIPanel NewGameState_Panel;
    public TweenColor NewGameState_Color;
    float NewGameDelayTimer = 0;
    int AddNumber = 0;
    public bool NewGameState_Bool = false;

    //請下注
    public UISprite PleaseBet_Sprite;
    public UISpriteAnimation PleaseBet_Animation;
    public static bool PleaseBetStateStart = false;
    float PleaseBetDelayTimer = 0;

    //遊戲進行中
    public GameObject WaitNextRound;
    public UISprite[] Point_Sprite = new UISprite[5];
    public byte PointNumber = 0;
    float PointDelayTimer = 0;

    //洗牌中
    public GameObject Shuffle_Object;
    float ShuffleTimer = 0;

    //等待新局開始
    public GameObject WaitNewGame_Object;
    public UISprite[] WaitPoint_Sprite = new UISprite[5];
    public byte WaitPointNumber = 0;
    float WaitPointDelayTimer = 0;
    public bool WaitNewGame_Bool = false;

    public static bool EndShow = false;
    bool SenceShow = false;
    float DelayTime = 0;
    // Use this for initialization
    void Start () {
        Background_Sprite.enabled = false;
        Background_Box.enabled = false;
        WaitNewGame_Bool = false;
        DataInit();
        EndShow = false;
        SenceShow = false;
    }
	
	// Update is called once per frame
	void Update () {
        //新局開始
        #region NewGame
        if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.NewRound)
        {
            if (!NewGameState_Bool)
            {
                Background_Sprite.enabled = true;
                Background_Box.enabled = true;
                NewGameState_Sprite.enabled = true;
                NewGameState_Box.enabled = true;
                NewGameState_Panel.clipRange = new Vector4(0, 0, 1 + AddNumber, 500);
                NewGameState_Color.PlayForward();
                if (AddNumber < 900)
                {
                    AddNumber += (int)(Time.deltaTime * 1000);
                }
                else
                {
                    AddNumber = 900;
                    if (NewGameDelayTimer < 1)
                    {
                        NewGameDelayTimer += Time.deltaTime;
                    }
                    else
                    {
                        NewGameState_Color.ResetToBeginning();
                        NewGameDelayTimer = 0;
                        AddNumber = 0;
                        NewGameState_Sprite.enabled = false;
                        NewGameState_Box.enabled = false;
                        Background_Sprite.enabled = false;
                        Background_Box.enabled = false;
                        NewGameState_Bool = true;
                        PleaseBetStateStart = true;
                        EndShow = false;
                    }
                }
            }
        }
        else
        {
            AddNumber = 0;
            NewGameDelayTimer = 0;
            NewGameState_Bool = false;
            NewGameState_Sprite.enabled = false;
            NewGameState_Box.enabled = false;
            NewGameState_Color.ResetToBeginning();
        }
        #endregion

        //請下注
        #region PleaseBet
        if (PleaseBetStateStart)
        {
            PleaseBetDelayTimer = 0;
            PleaseBet_Sprite.enabled = true;
            PleaseBet_Animation.ResetToBeginning();
            PleaseBet_Animation.enabled = true;
            PleaseBetStateStart = false;
            Background_Sprite.enabled = false;
            Background_Box.enabled = false;
        }
        else if (PleaseBet_Sprite.spriteName == "AR_Bet_10")
        {
            if (PleaseBetDelayTimer < 1)
            {
                PleaseBetDelayTimer += Time.deltaTime;
            }
            else
            {
                PleaseBetDelayTimer = 0;
                PleaseBet_Sprite.enabled = false;
                PleaseBet_Animation.enabled = false;
            }
        }

        #endregion

        //等待下回新局開始
        #region WaitNextRound
        if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.Idle)
        {
            Background_Sprite.enabled = true;
            Background_Box.enabled = true;
            WaitNextRound.SetActive(true);

            if (PointNumber == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    Point_Sprite[i].enabled = false;
                }
            }
            else if (PointNumber == 1)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (i < 1)
                        Point_Sprite[i].enabled = true;
                    else
                        Point_Sprite[i].enabled = false;
                }
            }
            else if (PointNumber == 2)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (i < 2)
                        Point_Sprite[i].enabled = true;
                    else
                        Point_Sprite[i].enabled = false;
                }
            }
            else if (PointNumber == 3)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (i < 3)
                        Point_Sprite[i].enabled = true;
                    else
                        Point_Sprite[i].enabled = false;
                }
            }
            else if (PointNumber == 4)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (i < 4)
                        Point_Sprite[i].enabled = true;
                    else
                        Point_Sprite[i].enabled = false;
                }
            }
            else if (PointNumber == 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    Point_Sprite[i].enabled = true;
                }
            }

            if (PointDelayTimer < 0.2f)
            {
                PointDelayTimer += Time.deltaTime;
            }
            else
            {
                if (PointNumber <= 5)
                {
                    PointNumber++;
                }
                else
                {
                    PointNumber = 0;
                }
                PointDelayTimer = 0;
            }

        }
        else
        {
            WaitNextRound.SetActive(false);
        }
        #endregion

        //等待新局開始
        #region WaitNewGame
        if (WaitNewGame_Bool)
        {
            WaitNewGame_Object.SetActive(true);

            if (WaitPointNumber == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    WaitPoint_Sprite[i].enabled = false;
                }
            }
            else if (WaitPointNumber == 1)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (i < 1)
                        WaitPoint_Sprite[i].enabled = true;
                    else
                        WaitPoint_Sprite[i].enabled = false;
                }
            }
            else if (WaitPointNumber == 2)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (i < 2)
                        WaitPoint_Sprite[i].enabled = true;
                    else
                        WaitPoint_Sprite[i].enabled = false;
                }
            }
            else if (WaitPointNumber == 3)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (i < 3)
                        WaitPoint_Sprite[i].enabled = true;
                    else
                        WaitPoint_Sprite[i].enabled = false;
                }
            }
            else if (WaitPointNumber == 4)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (i < 4)
                        WaitPoint_Sprite[i].enabled = true;
                    else
                        WaitPoint_Sprite[i].enabled = false;
                }
            }
            else if (WaitPointNumber == 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    WaitPoint_Sprite[i].enabled = true;
                }
            }

            if (WaitPointDelayTimer < 0.2f)
            {
                WaitPointDelayTimer += Time.deltaTime;
            }
            else
            {
                if (WaitPointNumber <= 5)
                {
                    WaitPointNumber++;
                }
                else
                {
                    WaitPointNumber = 0;
                }
                WaitPointDelayTimer = 0;
            }

        }
        else
        {
            WaitNewGame_Object.SetActive(false);
        }
        #endregion

        //洗牌中
        #region Shuffle
        if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.ShuffleNewRound)
        {
            Background_Sprite.enabled = true;
            Background_Box.enabled = true;
            Shuffle_Object.SetActive(true);
            if (ShuffleTimer < 2.0f)
            {
                ShuffleTimer += Time.deltaTime;
            }
            else
            {
                BJMainGame_Control.NowStateSave = ENUM_BLACKJACK_TABLE_STATE.NewRound;
                BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState = BJMainGame_Control.NowStateSave;
            }
        }
        else
        {
            Shuffle_Object.SetActive(false);
            ShuffleTimer = 0;
        }
        #endregion

        //END
        if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.GameOver)
        {
            Background_Sprite.enabled = true;
            Background_Box.enabled = true;
        }
        if (Backgrond_Bool)
        {
            Background_Color.PlayForward();
            Backgrond_Bool = false;
        }

        //if (FirstShow && (BJMainGame_Control.NowStateSave == ENUM_BLACKJACK_TABLE_STATE.NewRound 
        //    || BJMainGame_Control.NowStateSave == ENUM_BLACKJACK_TABLE_STATE.WaitBet || BJMainGame_Control.NowStateSave == ENUM_BLACKJACK_TABLE_STATE.ShuffleNewRound))
        if (FirstShow)
        {
            BJMainGame_Control.MainInit_Bool = true;
            BJCard_Control.CardControlInit_Bool = true;
            Cash_Control.CashInit_Bool = true;
            Background_Color.PlayReverse();
            FirstShow = false;
            SenceShow = true;
            DelayTime = 0;
        }

        if ((BJMainGame_Control.NowStateSave == ENUM_BLACKJACK_TABLE_STATE.NewRound || BJMainGame_Control.NowStateSave == ENUM_BLACKJACK_TABLE_STATE.WaitBet || BJMainGame_Control.NowStateSave == ENUM_BLACKJACK_TABLE_STATE.ShuffleNewRound) && EndShow)
        {
            if (DelayTime < 1)
            {
                DelayTime += Time.deltaTime;
            }
            else
            {
                BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState = BJMainGame_Control.NowStateSave;
                EndShow = false;
                FirstShow = false;
                WaitNewGame_Bool = false;
                SenceShow = false;
                DelayTime = 0;
            }
        }
    }

    public void ColorShow()
    {    
        if (SenceShow)
        {
            // BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState = BJMainGame_Control.NowStateSave;   
            WaitNewGame_Bool = true;
            EndShow = true;
            SenceShow = false;
        }
        else if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.GameOver)
        {
            EndShow = false;
            FirstShow = true;
            
        }
    }

    void DataInit()
    {
        Backgrond_Bool = false;
        FirstShow = false;
        Background_Color.ResetToBeginning();
        NewGameState_Panel.clipRange = new Vector4(0, 0, 1, 500);
        AddNumber = 0;
        NewGameState_Bool = false;
        NewGameState_Sprite.enabled = false;
        NewGameState_Box.enabled = false;
        NewGameDelayTimer = 0;
        PleaseBet_Sprite.enabled = false;
        PleaseBetStateStart = false;
        PleaseBet_Animation.ResetToBeginning();
        PleaseBetDelayTimer = 0;
        WaitNextRound.SetActive(false);
        PointNumber = 0;
        PointDelayTimer = 0;
        FirstCheck = false;
        Shuffle_Object.SetActive(false);
        ShuffleTimer = 0;
        WaitNewGame_Object.SetActive(false);
        WaitPointNumber = 0;
        WaitPointDelayTimer = 0;
    }
}
