using UnityEngine;
using System.Collections;
using GameCore.Manager.BlackJack;

public class ButtonSprite_Control : MonoBehaviour {

    //押注和硬幣
    public BoxCollider[] MoneyBetButton_Box = new BoxCollider[6];//押注金額BOX
    public UISprite[] MoneyBetButton_Sprite = new UISprite[4];//押注金額Sprite
    public UIButton[] MoneyBetButton_Button = new UIButton[2];//押注和確認

    //要牌相關按鈕
    public UIButton[] GetCardButton_Button = new UIButton[4];
    public BoxCollider[] GetCardButton_Box = new BoxCollider[4];

    //分牌,報到,保險
    public GameObject[] OtherObject = new GameObject[3];
    public static bool Scoreboard_Bool = false;//詢問是否分牌
    public static bool BJ21_Bool = false;//詢問是否報到
    public static bool BetInsure_Bool = false;//詢問是否要買保險

    // Use this for initialization
    void Start()
    {
        Scoreboard_Bool = false;
        BJ21_Bool = false;
        BetInsure_Bool = false;
    }
	
	// Update is called once per frame
	void Update () {
        MainButton();
        GetCardButton();
        OtherButton();
    }

    //押注和硬幣
    #region BetAndCoin
    public void MainButton()
    {
        if (!BJMainGame_Control.EnterBetBool)
        {
            if (BJMainGame_Control.SelectCoin == 1000)
            {
                MoneyBetButton_Sprite[0].spriteName = "btn_money_1k";
                MoneyBetButton_Sprite[1].spriteName = "btn_money_5k_0";
                MoneyBetButton_Sprite[2].spriteName = "btn_money_10k_0";
                MoneyBetButton_Sprite[3].spriteName = "btn_money_100k_0";
            }
            else if (BJMainGame_Control.SelectCoin == 5000)
            {
                MoneyBetButton_Sprite[0].spriteName = "btn_money_1k_0";
                MoneyBetButton_Sprite[1].spriteName = "btn_money_5k";
                MoneyBetButton_Sprite[2].spriteName = "btn_money_10k_0";
                MoneyBetButton_Sprite[3].spriteName = "btn_money_100k_0";
            }
            else if (BJMainGame_Control.SelectCoin == 10000)
            {
                MoneyBetButton_Sprite[0].spriteName = "btn_money_1k_0";
                MoneyBetButton_Sprite[1].spriteName = "btn_money_5k_0";
                MoneyBetButton_Sprite[2].spriteName = "btn_money_10k";
                MoneyBetButton_Sprite[3].spriteName = "btn_money_100k_0";
            }
            else if (BJMainGame_Control.SelectCoin == 100000)
            {
                MoneyBetButton_Sprite[0].spriteName = "btn_money_1k_0";
                MoneyBetButton_Sprite[1].spriteName = "btn_money_5k_0";
                MoneyBetButton_Sprite[2].spriteName = "btn_money_10k_0";
                MoneyBetButton_Sprite[3].spriteName = "btn_money_100k";
            }

            MoneyBetButton_Button[0].isEnabled = true;
            MoneyBetButton_Button[1].isEnabled = true;
            for (int i = 0; i < 6; i++)
            {
                MoneyBetButton_Box[i].enabled = true;
            }
        }
        else
        {
            MoneyBetButton_Sprite[0].spriteName = "btn_money_1k_0";
            MoneyBetButton_Sprite[1].spriteName = "btn_money_5k_0";
            MoneyBetButton_Sprite[2].spriteName = "btn_money_10k_0";
            MoneyBetButton_Sprite[3].spriteName = "btn_money_100k_0";
            MoneyBetButton_Button[0].isEnabled = false;
            MoneyBetButton_Button[1].isEnabled = false;
            for (int i = 0; i < 6; i++)
            {
                MoneyBetButton_Box[i].enabled = false;
            }
        }
    }
    #endregion

    //要牌按鈕
    #region GetCard
    void GetCardButton()
    {
        if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.PlayerTime && BJMainGame_Control.FirstGetCard_Bool && !Cash_Control.OnBetClick)
        {
            //加倍押注And投降
            if (BJCard_Control.Seat1Team1.Count == 2 && BJCard_Control.Seat1Team2.Count == 0)
            {
                GetCardButton_Button[0].isEnabled = true;
                GetCardButton_Box[0].enabled = true;
                GetCardButton_Button[3].isEnabled = true;
                GetCardButton_Box[3].enabled = true;
            }
            else
            {
                GetCardButton_Button[0].isEnabled = false;
                GetCardButton_Box[0].enabled = false;
                GetCardButton_Button[3].isEnabled = false;
                GetCardButton_Box[3].enabled = false;
            }

            //要牌AndPass
            GetCardButton_Button[1].isEnabled = true;
            GetCardButton_Box[1].enabled = true;
            GetCardButton_Button[2].isEnabled = true;
            GetCardButton_Box[2].enabled = true;
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                GetCardButton_Button[i].isEnabled = false;
                GetCardButton_Box[i].enabled = false;
            }
        }
    }
    #endregion

    #region Other
    void OtherButton()
    {
        if (Scoreboard_Bool)
        {
            OtherObject[0].SetActive(true);
        }
        else
        {
            OtherObject[0].SetActive(false);
        }

        if (BJ21_Bool)
        {
            OtherObject[1].SetActive(true);
        }
        else
        {
            OtherObject[1].SetActive(false);
        }

        if (BetInsure_Bool)
        {
            OtherObject[2].SetActive(true);
        }
        else
        {
            OtherObject[2].SetActive(false);
        }
    }
    #endregion
}
