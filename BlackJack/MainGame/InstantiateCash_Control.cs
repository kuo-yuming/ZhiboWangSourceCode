using UnityEngine;
using System.Collections;
using MoneyTable;
using GameCore.Manager.BlackJack;

namespace MoneyTable
{
    public enum TableList
    {
        MyTable = 0,
        PlayerTable1 = 1,
        PlayerTable2 = 2,
        PlayerTable3 = 3,
        PlayerTable4 = 4,
    }
}
public class InstantiateCash_Control : MonoBehaviour {

    public GameObject InstantiateCash;//生成金幣
    public GameObject InstantiateSeat;//生成位址
    public TableList ThisTable;

    public int ThisTableMoney = 0;
    public bool InitStart_Bool = false;

    bool Init_Bool = false;
    // Use this for initialization
    void Start () {
        DeleteCoin();
    }
	
	// Update is called once per frame
	void Update () {
        //if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.CheckPlayer || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.GameOver
        //    || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.GameSettlement)
        //{
        //    DeleteCoin();
        //}


        if (BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.NewRound
                          || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.ShuffleNewRound
                          || BJMainGame_Control.TableState[BJMainGame_Control.TableID].m_enumState == ENUM_BLACKJACK_TABLE_STATE.WaitBet)
        {
            if (ThisTable == TableList.MyTable)
            {
                if (!BJMainGame_Control.BetCancel_Bool)
                {
                    if (ThisTableMoney != Cash_Control.TableCash[0])
                    {
                        DeleteCoin();
                        ThisTableMoney = Cash_Control.TableCash[0];
                        CoinCheck();
                    }
                }
                else if (BJMainGame_Control.BetCancel_Bool && ThisTableMoney != 0)
                {
                    DeleteCoin();
                    Cash_Control.TableCash[0] = 0;
                    BJMainGame_Control.BetCancel_Bool = false;
                }
            }
            else if (ThisTable == TableList.PlayerTable1)
            {
                if (ThisTableMoney != Cash_Control.TableCash[2])
                {
                    DeleteCoin();
                    ThisTableMoney = Cash_Control.TableCash[2];
                    CoinCheck();
                }
            }
            else if (ThisTable == TableList.PlayerTable2)
            {
                if (ThisTableMoney != Cash_Control.TableCash[4])
                {
                    DeleteCoin();
                    ThisTableMoney = Cash_Control.TableCash[4];
                    CoinCheck();
                }
            }
            else if (ThisTable == TableList.PlayerTable3)
            {
                if (ThisTableMoney != Cash_Control.TableCash[6])
                {
                    DeleteCoin();
                    ThisTableMoney = Cash_Control.TableCash[6];
                    CoinCheck();
                }
            }
            else if (ThisTable == TableList.PlayerTable4)
            {
                if (ThisTableMoney != Cash_Control.TableCash[8])
                {
                    DeleteCoin();
                    ThisTableMoney = Cash_Control.TableCash[8];
                    CoinCheck();
                }
            }
        }
      
    }

    void DeleteCoin()
    {
        Transform[] Objs = InstantiateSeat.GetComponentsInChildren<Transform>();
        int Len = Objs.Length;
        for (int i = 0; i < Len; i++)
        {
            if (Objs[i].name != "CashSeat")
            {
                Destroy(Objs[i].gameObject);
            }
        }
        ThisTableMoney = 0;
    }

    void CoinCheck()
    {
        byte Coin100K = 0;
        byte Coin10K = 0;
        byte Coin5K = 0;
        byte Coin1K = 0;
        byte TotalCoin = 0;

        Coin100K = (byte)(ThisTableMoney / 100000);
        Coin10K = (byte)((ThisTableMoney / 10000) % 10);
        Coin5K = (byte)((ThisTableMoney / 5000) % 2);
        Coin1K = (byte)((ThisTableMoney / 1000) % 5);

        TotalCoin = (byte)(Coin100K + Coin10K + Coin5K + Coin1K);

        if (TotalCoin > 40)
            TotalCoin = 40;

        for (int i = 0; i < TotalCoin; i++)
        {
            if (TotalCoin <= 10)
            {
                OneCoinLIst(i, Coin100K, Coin10K, Coin5K, Coin1K);
            }
            else if (TotalCoin > 10 && TotalCoin <= 20)
            {
                TwoCoinLIst(i, Coin100K, Coin10K, Coin5K, Coin1K);
            }
            else if (TotalCoin > 20 && TotalCoin <= 30)
            {
                ThreeCoinLIst(i, Coin100K, Coin10K, Coin5K, Coin1K);
            }
            else if (TotalCoin > 30)
            {
                FourCoinLIst(i, Coin100K, Coin10K, Coin5K, Coin1K);
            }
        }
    }

    //硬幣堆疊
    #region CoinList4
    void FourCoinLIst(int usNumber,byte usCoin100K, byte usCoin10K, byte usCoin5K, byte usCoin1K)
    {
        GameObject Data = Instantiate(InstantiateCash);
        Data.transform.parent = InstantiateSeat.transform;
        Data.transform.localScale = new Vector3(1, 1, 1);
        if (usNumber < 10)
        {
            Data.transform.localPosition = new Vector3(-75, usNumber * 3, 1);
        }
        else if (usNumber >= 10 && usNumber < 20)
        {
            Data.transform.localPosition = new Vector3(-25, (usNumber - 10) * 3, 1);
        }
        else if (usNumber >= 20 && usNumber < 30)
        {
            Data.transform.localPosition = new Vector3(25, (usNumber - 20) * 3, 1);
        }
        else
        {
            Data.transform.localPosition = new Vector3(75, (usNumber - 30) * 3, 1);
        }
        BJMoney_Control Data_Control = Data.GetComponent<BJMoney_Control>();
        Data_Control.Money_TweenPosition.from = Data.transform.localPosition;
        Data_Control.ThisTable = ThisTable;
        if ((usCoin100K - usNumber) > 0)
        {
            Data_Control.Money_Sprite.spriteName = "icon_money_100k";
        }
        else if (((usCoin10K + usCoin100K) - usNumber) > 0)
        {
            Data_Control.Money_Sprite.spriteName = "icon_money_10k";
        }
        else if (((usCoin5K + usCoin10K + usCoin100K) - usNumber) > 0)
        {
            Data_Control.Money_Sprite.spriteName = "icon_money_5k";
        }
        else if (((usCoin1K + usCoin5K + usCoin10K + usCoin100K) - usNumber) > 0)
        {
            Data_Control.Money_Sprite.spriteName = "icon_money_1k";
        }
    }
    #endregion

    #region CoinList3
    void ThreeCoinLIst(int usNumber, byte usCoin100K, byte usCoin10K, byte usCoin5K, byte usCoin1K)
    {
        GameObject Data = Instantiate(InstantiateCash);
        Data.transform.parent = InstantiateSeat.transform;
        Data.transform.localScale = new Vector3(1, 1, 1);
        if (usNumber < 10)
        {
            Data.transform.localPosition = new Vector3(-50, usNumber * 3, 1);
        }
        else if (usNumber >= 10 && usNumber < 20)
        {
            Data.transform.localPosition = new Vector3(0, (usNumber - 10) * 3, 1);
        }
        else
        {
            Data.transform.localPosition = new Vector3(50, (usNumber - 20) * 3, 1);
        }
        BJMoney_Control Data_Control = Data.GetComponent<BJMoney_Control>();
        Data_Control.Money_TweenPosition.from = Data.transform.localPosition;
        Data_Control.ThisTable = ThisTable;
        if ((usCoin100K - usNumber) > 0)
        {
            Data_Control.Money_Sprite.spriteName = "icon_money_100k";
        }
        else if (((usCoin10K + usCoin100K) - usNumber) > 0)
        {
            Data_Control.Money_Sprite.spriteName = "icon_money_10k";
        }
        else if (((usCoin5K + usCoin10K + usCoin100K) - usNumber) > 0)
        {
            Data_Control.Money_Sprite.spriteName = "icon_money_5k";
        }
        else if (((usCoin1K + usCoin5K + usCoin10K + usCoin100K) - usNumber) > 0)
        {
            Data_Control.Money_Sprite.spriteName = "icon_money_1k";
        }
    }
    #endregion

    #region CoinList2
    void TwoCoinLIst(int usNumber, byte usCoin100K, byte usCoin10K, byte usCoin5K, byte usCoin1K)
    {
        GameObject Data = Instantiate(InstantiateCash);
        Data.transform.parent = InstantiateSeat.transform;
        Data.transform.localScale = new Vector3(1, 1, 1);
        if (usNumber < 10)
        {
            Data.transform.localPosition = new Vector3(-25, usNumber * 3, 1);
        }
        else
        {
            Data.transform.localPosition = new Vector3(25, (usNumber - 10) * 3, 1);
        }
        BJMoney_Control Data_Control = Data.GetComponent<BJMoney_Control>();
        Data_Control.Money_TweenPosition.from = Data.transform.localPosition;
        Data_Control.ThisTable = ThisTable;
        if ((usCoin100K - usNumber) > 0)
        {
            Data_Control.Money_Sprite.spriteName = "icon_money_100k";
        }
        else if (((usCoin10K + usCoin100K) - usNumber) > 0)
        {
            Data_Control.Money_Sprite.spriteName = "icon_money_10k";
        }
        else if (((usCoin5K + usCoin10K + usCoin100K) - usNumber) > 0)
        {
            Data_Control.Money_Sprite.spriteName = "icon_money_5k";
        }
        else if (((usCoin1K + usCoin5K + usCoin10K + usCoin100K) - usNumber) > 0)
        {
            Data_Control.Money_Sprite.spriteName = "icon_money_1k";
        }
    }
    #endregion

    #region CoinList1
    void OneCoinLIst(int usNumber, byte usCoin100K, byte usCoin10K, byte usCoin5K, byte usCoin1K)
    {
        GameObject Data = Instantiate(InstantiateCash);
        Data.transform.parent = InstantiateSeat.transform;
        Data.transform.localScale = new Vector3(1, 1, 1);
        Data.transform.localPosition = new Vector3(0, usNumber * 3, 1);
        BJMoney_Control Data_Control = Data.GetComponent<BJMoney_Control>();
        Data_Control.Money_TweenPosition.from = Data.transform.localPosition;
        Data_Control.ThisTable = ThisTable;
        if ((usCoin100K - usNumber) > 0)
        {
            Data_Control.Money_Sprite.spriteName = "icon_money_100k";
        }
        else if (((usCoin10K + usCoin100K) - usNumber) > 0)
        {
            Data_Control.Money_Sprite.spriteName = "icon_money_10k";
        }
        else if (((usCoin5K + usCoin10K + usCoin100K) - usNumber) > 0)
        {
            Data_Control.Money_Sprite.spriteName = "icon_money_5k";
        }
        else if (((usCoin1K + usCoin5K + usCoin10K + usCoin100K) - usNumber) > 0)
        {
            Data_Control.Money_Sprite.spriteName = "icon_money_1k";
        }
    }
    #endregion
}
