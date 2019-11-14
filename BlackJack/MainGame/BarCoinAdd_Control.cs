using UnityEngine;
using System.Collections;
using CardTeamListClass;
using MoneyTable;

public class BarCoinAdd_Control : MonoBehaviour
{

    public GameObject InstantiateCash;//生成金幣
    public GameObject InstantiateSeat;//生成位址
    public CardTeamList CardTeam_Control;
    public TableList ThisTable;

    public int ThisTableMoney = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (CardTeam_Control == CardTeamList.Card1Team1 && Cash_Control.CashMoveEnd[(byte)ThisTable] == 1)
        {
            if (Cash_Control.TableCash[(byte)CardTeamList.Card1Team1] != ThisTableMoney)
            {
                DeleteCoin();
                ThisTableMoney = Cash_Control.TableCash[(byte)CardTeamList.Card1Team1];
                CoinAdd();
                Cash_Control.CashMoveEnd[(byte)ThisTable] = 0;
            }
        }
        else if (CardTeam_Control == CardTeamList.Card1Team2)
        {
            if (Cash_Control.TableCash[(byte)CardTeamList.Card1Team2] != ThisTableMoney)
            {
                DeleteCoin();
                ThisTableMoney = Cash_Control.TableCash[(byte)CardTeamList.Card1Team2];
                CoinAdd();
            }
        }
        else if (CardTeam_Control == CardTeamList.Card2Team1 && Cash_Control.CashMoveEnd[(byte)ThisTable] == 1)
        {
            if (Cash_Control.TableCash[(byte)CardTeamList.Card2Team1] != ThisTableMoney)
            {
                DeleteCoin();
                ThisTableMoney = Cash_Control.TableCash[(byte)CardTeamList.Card2Team1];
                CoinAdd();
                Cash_Control.CashMoveEnd[(byte)ThisTable] = 0;
            }
        }
        else if (CardTeam_Control == CardTeamList.Card2Team2)
        {
            if (Cash_Control.TableCash[(byte)CardTeamList.Card2Team2] != ThisTableMoney)
            {
                DeleteCoin();
                ThisTableMoney = Cash_Control.TableCash[(byte)CardTeamList.Card2Team2];
                CoinAdd();
            }
        }
        else if (CardTeam_Control == CardTeamList.Card3Team1 && Cash_Control.CashMoveEnd[(byte)ThisTable] == 1)
        {
            if (Cash_Control.TableCash[(byte)CardTeamList.Card3Team1] != ThisTableMoney)
            {
                DeleteCoin();
                ThisTableMoney = Cash_Control.TableCash[(byte)CardTeamList.Card3Team1];
                CoinAdd();
                Cash_Control.CashMoveEnd[(byte)ThisTable] = 0;
            }
        }
        else if (CardTeam_Control == CardTeamList.Card3Team2)
        {
            if (Cash_Control.TableCash[(byte)CardTeamList.Card3Team2] != ThisTableMoney)
            {
                DeleteCoin();
                ThisTableMoney = Cash_Control.TableCash[(byte)CardTeamList.Card3Team2];
                CoinAdd();
            }
        }
        else if (CardTeam_Control == CardTeamList.Card4Team1 && Cash_Control.CashMoveEnd[(byte)ThisTable] == 1)
        {
            if (Cash_Control.TableCash[(byte)CardTeamList.Card4Team1] != ThisTableMoney)
            {
                DeleteCoin();
                ThisTableMoney = Cash_Control.TableCash[(byte)CardTeamList.Card4Team1];
                CoinAdd();
                Cash_Control.CashMoveEnd[(byte)ThisTable] = 0;
            }
        }
        else if (CardTeam_Control == CardTeamList.Card4Team2)
        {
            if (Cash_Control.TableCash[(byte)CardTeamList.Card4Team2] != ThisTableMoney)
            {
                DeleteCoin();
                ThisTableMoney = Cash_Control.TableCash[(byte)CardTeamList.Card4Team2];
                CoinAdd();
            }
        }
        else if (CardTeam_Control == CardTeamList.Card5Team1 && Cash_Control.CashMoveEnd[(byte)ThisTable] == 1)
        {
            if (Cash_Control.TableCash[(byte)CardTeamList.Card5Team1] != ThisTableMoney)
            {
                DeleteCoin();
                ThisTableMoney = Cash_Control.TableCash[(byte)CardTeamList.Card5Team1];
                CoinAdd();
                Cash_Control.CashMoveEnd[(byte)ThisTable] = 0;
            }
        }
        else if (CardTeam_Control == CardTeamList.Card5Team2)
        {
            if (Cash_Control.TableCash[(byte)CardTeamList.Card5Team2] != ThisTableMoney)
            {
                DeleteCoin();
                ThisTableMoney = Cash_Control.TableCash[(byte)CardTeamList.Card5Team2];
                CoinAdd();
            }
        }
    }

    void CoinAdd()
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

        if (TotalCoin > 5)
            TotalCoin = 5;

        for (int i = 0; i < TotalCoin; i++)
        {
            CoinLIst(i, Coin100K, Coin10K, Coin5K, Coin1K);
        }
    
    }

    void CoinLIst(int usNumber, byte usCoin100K, byte usCoin10K, byte usCoin5K, byte usCoin1K)
    {
        GameObject Data = Instantiate(InstantiateCash);
        Data.transform.parent = InstantiateSeat.transform;
        Data.transform.localScale = new Vector3(1, 1, 1);
        Data.transform.localPosition = new Vector3(0, usNumber * 3, 1);
        BarMoney Data_Control = Data.GetComponent<BarMoney>();

        Data_Control.Money_Sprite.depth = usNumber + 15;

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

    void DeleteCoin()
    {
        Transform[] Objs = InstantiateSeat.GetComponentsInChildren<Transform>();
        int Len = Objs.Length;
        for (int i = 0; i < Len; i++)
        {
            if (Objs[i].name != "BarCoin")
            {
                Destroy(Objs[i].gameObject);
            }
        }
    }
}
