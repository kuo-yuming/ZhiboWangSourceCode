using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore.Manager.BlackJack;
using WinLoseListClass;
using MoneyTable;

public class FinallData
{
    public uint PlayerDBID = 0;
    public string PlayerName = "";
    public WinLoseList Card1Award = WinLoseList.NoCheck;
    public WinLoseList Card2Award = WinLoseList.NoCheck;
    public long WinLoseMoney = 0;
}


public class BJEndWindow_Control : MonoBehaviour {

    public GameObject AwardBar_Object;
    public GameObject InstantiateSeat;
    public UIGrid AwardBar_Grid;

    public static bool EndWindow_Bool = false;
    public GameObject MainObject;
    public static Dictionary<byte, FinallData> m_BetAward = new Dictionary<byte, FinallData>();//byte = 座位
    public static byte[] WinLoseShowSave = new byte[10];

    public UISprite TimerName;
    public float TimerNumber = 0;
    public int MaxTimer = 5;

    bool BarCheck = true;

    bool InitCheck_Bool = false;
    float DelayTimer = 0;
	// Use this for initialization
	void Start () {
        DeleteAward();
        InitData();
    }
	
	// Update is called once per frame
	void Update () {
        if (EndWindow_Bool)
        {
            if (DelayTimer < 5)
            {
                DelayTimer += Time.deltaTime;
            }
            else
            {
                InitCheck_Bool = false;
                MainObject.SetActive(true);
                BJMainGame_Control.MoneyEndShow_Bool = true;
                //時間倒數
                TimerName.spriteName = "numberC_time_" + (MaxTimer - (int)TimerNumber);
                if (TimerNumber < 5)
                {
                    TimerNumber += Time.deltaTime;
                }
                else
                {
                    TimerNumber = 0;
                    EndWindow_Bool = false;
                    StateShow_Control.Backgrond_Bool = true;
                    BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState = ENUM_BLACKJACK_TABLE_STATE.GameOver;
                }

                if (BarCheck)
                {
                    AwardBar();
                    AwardBar_Grid.enabled = true;
                    BarCheck = false;
                }
            }
        }
        else
        {
            MainObject.SetActive(false);
            if (!InitCheck_Bool)
            {
                DelayTimer = 0;
                DeleteAward();
                InitData();
                InitCheck_Bool = true;
            }
        }
	}

    //生成獎項
    void AwardBar()
    {
        foreach (var item in m_BetAward)
        {
            GameObject Data = Instantiate(AwardBar_Object);
            Data.transform.parent = InstantiateSeat.transform;
            Data.name = item.Key.ToString();
            Data.transform.localScale = new Vector3(1, 1, 1);
            Data.transform.localPosition = this.transform.localPosition;
            AwardList_Control Data_Control = Data.GetComponent<AwardList_Control>();
            Data_Control.PlayerName_Label.text = item.Value.PlayerName;
            Data_Control.WinLoseMoney_Label.text = item.Value.WinLoseMoney.ToString();
            if (item.Value.WinLoseMoney < 0)
            {
                Data_Control.WinLoseMoney_Label.text = item.Value.WinLoseMoney.ToString();
                Data_Control.WinLoseMoney_Label.color = new Color32(255, 77, 79, 255);
            }
            else
            {
                Data_Control.WinLoseMoney_Label.text = "+" + item.Value.WinLoseMoney.ToString();
                Data_Control.WinLoseMoney_Label.color = new Color32(112, 255, 255, 255);    
            }

            if (item.Value.PlayerDBID == Cash_Control.PlayerDBID[(byte)TableList.MyTable])
                Data_Control.PlayerBar.enabled = true;
            else
                Data_Control.PlayerBar.enabled = false;

            if (item.Value.Card2Award != WinLoseList.NoCheck)
            {
                Data_Control.TwoWinLose_Sprite1.enabled = true;
                Data_Control.TwoWinLose_Sprite2.enabled = true;
                Data_Control.WinLose_Sprite.enabled = false;
                if (item.Value.Card1Award == WinLoseList.BlackJack || BJMainGame_Control.BlackJack_Bool[item.Key] == 1)
                    Data_Control.TwoWinLose_Sprite1.spriteName = "icon_PrizeBJ";
                else if (item.Value.Card1Award == WinLoseList.WinBanker)
                    Data_Control.TwoWinLose_Sprite1.spriteName = "icon_PrizeLose";
                else if (item.Value.Card1Award == WinLoseList.WinPlayer)
                    Data_Control.TwoWinLose_Sprite1.spriteName = "icon_PrizeWin";
                else if (item.Value.Card1Award == WinLoseList.WinDraw)
                    Data_Control.TwoWinLose_Sprite1.spriteName = "icon_PrizePush";
                else if (item.Value.Card1Award == WinLoseList.PointOut)
                    Data_Control.TwoWinLose_Sprite1.spriteName = "icon_PrizeBust";

                if (item.Value.Card2Award == WinLoseList.BlackJack)
                    Data_Control.TwoWinLose_Sprite2.spriteName = "icon_PrizeBJ";
                else if (item.Value.Card2Award == WinLoseList.WinBanker)
                    Data_Control.TwoWinLose_Sprite2.spriteName = "icon_PrizeLose";
                else if (item.Value.Card2Award == WinLoseList.WinPlayer)
                    Data_Control.TwoWinLose_Sprite2.spriteName = "icon_PrizeWin";
                else if (item.Value.Card2Award == WinLoseList.WinDraw)
                    Data_Control.TwoWinLose_Sprite2.spriteName = "icon_PrizePush";
                else if (item.Value.Card2Award == WinLoseList.PointOut)
                    Data_Control.TwoWinLose_Sprite2.spriteName = "icon_PrizeBust";
            }
            else
            {
                Data_Control.TwoWinLose_Sprite1.enabled = false;
                Data_Control.TwoWinLose_Sprite2.enabled = false;
                Data_Control.B_Bar.enabled = false;
                Data_Control.WinLose_Sprite.enabled = true;
                if (item.Value.Card1Award == WinLoseList.BlackJack)
                    Data_Control.WinLose_Sprite.spriteName = "icon_PrizeBJ";
                else if (item.Value.Card1Award == WinLoseList.WinBanker)
                    Data_Control.WinLose_Sprite.spriteName = "icon_PrizeLose";
                else if (item.Value.Card1Award == WinLoseList.WinPlayer)
                    Data_Control.WinLose_Sprite.spriteName = "icon_PrizeWin";
                else if (item.Value.Card1Award == WinLoseList.WinDraw)
                    Data_Control.WinLose_Sprite.spriteName = "icon_PrizePush";
                else if (item.Value.Card1Award == WinLoseList.PointOut)
                    Data_Control.WinLose_Sprite.spriteName = "icon_PrizeBust";
            }
        }
    }

    //刪除獎項
    void DeleteAward()
    {
        Transform[] Objs = InstantiateSeat.GetComponentsInChildren<Transform>();
        int Len = Objs.Length;
        for (int i = 0; i < Len; i++)
        {
            if (Objs[i].name != "AwardList_Control")
            {
                Destroy(Objs[i].gameObject);
            }
        }
    }

    void InitData()
    {
        EndWindow_Bool = false;
        TimerName.spriteName = "numberC_time_5";
        TimerNumber = 0;
        m_BetAward.Clear();
        for (byte i = 0; i < 10; i++)
            WinLoseShowSave[i] = 0;
        BarCheck = true;
        InitCheck_Bool = false;
    }
}
