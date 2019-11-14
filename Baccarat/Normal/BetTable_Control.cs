using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore.Manager.Baccarat;
using GameEnum;
using GameCore;

public class BetTable_Control : MonoBehaviour {
    public static uint[] MyBetMoneySeat = new uint[5];
    public static ulong[] TableAllBetMoneySeat = new ulong[5];
    public ulong[] MoneyUpData = new ulong[5];
    public static ulong[] SaveMoneyUpData = new ulong[5];

    public GameObject[] CoinGameobject;
    public UITable[] BetTable;

    //Lock
    private Object BankerLock = new Object();
    private Object PlayerLock = new Object();
    private Object DrawLock = new Object();
    private Object BankerPairLock = new Object();
    private Object PlayerPairLock = new Object();
   
    //硬幣數量計算
    private byte[] Total_Gold1M = new byte[5];
    private byte[] Total_Gold100k = new byte[5];
    private byte[] Total_Gold10k = new byte[5];
    private byte[] Total_Gold5k = new byte[5];
    private byte[] Total_Gold1k = new byte[5];
    private byte[] Total_Gold500 = new byte[5];
    private byte[] Total_Gold100 = new byte[5];
    private byte[] Table_Total_Gold = new byte[5];

    bool TableInitBool = true;
    public static bool SaveCheck = false;
    float DelayTimer = 0;
	// Use this for initialization
    void Start()
    {
        TableInitBool = true;
        for (int i = 0; i < 5; i++)
        {
            MyBetMoneySeat[i] = 0;
            TableAllBetMoneySeat[i] = 0;
            MoneyUpData[i] = 0;
            Total_Gold100k[i] = 0;
            Total_Gold10k[i] = 0;
            Total_Gold5k[i] = 0;
            Total_Gold1k[i] = 0;
            Total_Gold500[i] = 0;
            Total_Gold100[i] = 0;
            SaveMoneyUpData[i] = 0;
        }
    }

    void Update()
    {
        BankerCoinGenerateVoid();
        PlayerCoinGenerateVoid();
        DrawCoinGenerateVoid();
        BankerPairCoinGenerateVoid();
        PlayerPairCoinGenerateVoid();

        if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.EndShow)
        {
            if (TableInitBool)
            {
                MoneyDataInit();
                TableInitBool = false;
            }
        }
        else
        {
            TableInitBool = true;
        }

        if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.WaitStop && SaveCheck)
        {
            lock (BaccaratManager.BetLock)
            {
                for (int i = 0; i < 5; i++)
                {
                    TableAllBetMoneySeat[i] = SaveMoneyUpData[i];
                }
                SaveCheck = false;
            }
        }
        else if (MainGame_Control.StopModeState != ENUM_STOPMODE_STATE.WaitStop)
        {
            DelayTimer = 0;
         //   SaveCheck = false;
        }
    }

    void BankerCoinGenerateVoid()
    {
        if (TableAllBetMoneySeat[0] != MoneyUpData[0])
        {
            lock (BaccaratManager.BetLock)
            {
                MoneyUpData[0] = TableAllBetMoneySeat[0];
                Transform[] Objs = BetTable[0].GetComponentsInChildren<Transform>();
                int Len = Objs.Length;
                for (int i = 0; i < Len; i++)
                {
                    if (Objs[i].name == "BankerCoin")
                    {
                        Destroy(Objs[i].gameObject);
                    }
                }
                BankerCoinVoid();
                SaveMoneyUpData[0] = 0;
            }
        }
    }
    void PlayerCoinGenerateVoid()
    {
        if (TableAllBetMoneySeat[1] != MoneyUpData[1])
        {
            lock (BaccaratManager.BetLock)
            {
                MoneyUpData[1] = TableAllBetMoneySeat[1];
                Transform[] Objs = BetTable[1].GetComponentsInChildren<Transform>();
                int Len = Objs.Length;
                for (int i = 0; i < Len; i++)
                {
                    if (Objs[i].name == "PlayerCoin")
                    {
                        Destroy(Objs[i].gameObject);
                    }
                }               
                PlayerCoinVoid();
                SaveMoneyUpData[1] = 0;
            }
        }
    }
    void DrawCoinGenerateVoid()
    {
        if (TableAllBetMoneySeat[2] != MoneyUpData[2])
        {
            lock (BaccaratManager.BetLock)
            {
                MoneyUpData[2] = TableAllBetMoneySeat[2];
                Transform[] Objs = BetTable[2].GetComponentsInChildren<Transform>();
                int Len = Objs.Length;
                for (int i = 0; i < Len; i++)
                {
                    if (Objs[i].name == "DrawCoin")
                    {
                        Destroy(Objs[i].gameObject);
                    }
                }              
                DrawCoinVoid();
                SaveMoneyUpData[2] = 0;
            }
        }
    }
    void BankerPairCoinGenerateVoid()
    {
        if (TableAllBetMoneySeat[3] != MoneyUpData[3])
        {
            lock (BaccaratManager.BetLock)
            {
                MoneyUpData[3] = TableAllBetMoneySeat[3];
                Transform[] Objs = BetTable[3].GetComponentsInChildren<Transform>();
                int Len = Objs.Length;
                for (int i = 0; i < Len; i++)
                {
                    if (Objs[i].name == "BankerPairCoin")
                    {
                        Destroy(Objs[i].gameObject);
                    }
                }               
                BankerPairCoinVoid();
                SaveMoneyUpData[3] = 0;
            }
        }
    }
    void PlayerPairCoinGenerateVoid()
    {
        if (TableAllBetMoneySeat[4] != MoneyUpData[4])
        {
            lock (BaccaratManager.BetLock)
            {
                MoneyUpData[4] = TableAllBetMoneySeat[4];
                Transform[] Objs = BetTable[4].GetComponentsInChildren<Transform>();
                int Len = Objs.Length;
                for (int i = 0; i < Len; i++)
                {
                    if (Objs[i].name == "PlayerPairCoin")
                    {
                        Destroy(Objs[i].gameObject);
                    }
                }            
                PlayerPairCoinVoid();
                SaveMoneyUpData[4] = 0;
            }
        }
    }

    void BankerCoinVoid()
    {
        Total_Gold1M[0] = (byte)((MoneyUpData[0] / 1000000) % 10);
        Total_Gold100k[0] = (byte)((MoneyUpData[0] / 100000) % 10);
        Total_Gold10k[0] = (byte)((MoneyUpData[0] / 10000) % 10);
        Total_Gold5k[0] = (byte)((MoneyUpData[0] / 5000) % 2);
        Total_Gold1k[0] = (byte)((MoneyUpData[0] / 1000) % 5);
        Total_Gold500[0] = (byte)((MoneyUpData[0] / 500) % 2);
        Total_Gold100[0] = (byte)((MoneyUpData[0] / 100) % 5);

         Table_Total_Gold[0] = (byte)(Total_Gold1M[0] + Total_Gold100k[0] + Total_Gold10k[0] + Total_Gold5k[0] + Total_Gold1k[0] + Total_Gold500[0] + Total_Gold100[0]);
       // Table_Total_Gold[0] = (byte)(Total_Gold100k[0] + Total_Gold10k[0] + Total_Gold5k[0] + Total_Gold1k[0] + Total_Gold500[0] + Total_Gold100[0]);

        for (byte i = 0; i < Table_Total_Gold[0]; i++)
        {
            byte XPosition = (byte)((i / 10) % 3);
            byte YPosition = (byte)(i % 10);
            byte YAddPosition = (byte)(i / 30);

            if (i < Total_Gold1M[0])
            {
                GameObject Data = Instantiate(CoinGameobject[6]);
                Data.transform.parent = BetTable[0].transform;
                Data.name = "BankerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -72), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - Total_Gold1M[0]) < Total_Gold100k[0])
            {
                GameObject Data = Instantiate(CoinGameobject[5]);
                Data.transform.parent = BetTable[0].transform;
                Data.name = "BankerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -72),1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (Total_Gold1M[0] + Total_Gold100k[0])) < Total_Gold10k[0])
            {
                GameObject Data = Instantiate(CoinGameobject[4]);
                Data.transform.parent = BetTable[0].transform;
                Data.name = "BankerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -72), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (Total_Gold1M[0] + Total_Gold100k[0] + Total_Gold10k[0])) < Total_Gold5k[0])
            {
                GameObject Data = Instantiate(CoinGameobject[3]);
                Data.transform.parent = BetTable[0].transform;
                Data.name = "BankerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -72), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (Total_Gold1M[0] + Total_Gold100k[0] + Total_Gold10k[0] + Total_Gold5k[0])) < Total_Gold1k[0])
            {
                GameObject Data = Instantiate(CoinGameobject[2]);
                Data.transform.parent = BetTable[0].transform;
                Data.name = "BankerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -72), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (Total_Gold1M[0] + Total_Gold100k[0] + Total_Gold10k[0] + Total_Gold5k[0] + Total_Gold1k[0])) < Total_Gold500[0])
            {
                GameObject Data = Instantiate(CoinGameobject[1]);
                Data.transform.parent = BetTable[0].transform;
                Data.name = "BankerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -72), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (Total_Gold1M[0] + Total_Gold100k[0] + Total_Gold10k[0] + Total_Gold5k[0] + Total_Gold1k[0] + Total_Gold500[0])) < Total_Gold100[0])
            {
                GameObject Data = Instantiate(CoinGameobject[0]);
                Data.transform.parent = BetTable[0].transform;
                Data.name = "BankerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -72), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
        }
           
    }
    void PlayerCoinVoid()
    {
        Total_Gold1M[1] = (byte)((MoneyUpData[1] / 1000000) % 10);
        Total_Gold100k[1] = (byte)((MoneyUpData[1] / 100000) % 10);
        Total_Gold10k[1] = (byte)((MoneyUpData[1] / 10000) % 10);
        Total_Gold5k[1] = (byte)((MoneyUpData[1] / 5000) % 2);
        Total_Gold1k[1] = (byte)((MoneyUpData[1] / 1000) % 5);
        Total_Gold500[1] = (byte)((MoneyUpData[1] / 500) % 2);
        Total_Gold100[1] = (byte)((MoneyUpData[1] / 100) % 5);

       // Table_Total_Gold = (byte)(Total_Gold1M + Total_Gold100k + Total_Gold10k + Total_Gold5k + Total_Gold1k + Total_Gold500 + Total_Gold100);
        Table_Total_Gold[1] = (byte)(Total_Gold1M[1] + Total_Gold100k[1] + Total_Gold10k[1] + Total_Gold5k[1] + Total_Gold1k[1] + Total_Gold500[1] + Total_Gold100[1]);

        for (byte i = 0; i < Table_Total_Gold[1]; i++)
        {
            byte XPosition = (byte)((i / 10) % 3);
            byte YPosition = (byte)(i % 10);
            byte YAddPosition = (byte)(i / 30);
            if (i < Total_Gold1M[1])
            {
                GameObject Data = Instantiate(CoinGameobject[6]);
                Data.transform.parent = BetTable[1].transform;
                Data.name = "PlayerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -72), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - Total_Gold1M[1]) < Total_Gold100k[1])
            {
                GameObject Data = Instantiate(CoinGameobject[5]);
                Data.transform.parent = BetTable[1].transform;
                Data.name = "PlayerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -72), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (Total_Gold1M[1] + Total_Gold100k[1])) < Total_Gold10k[1])
            {
                GameObject Data = Instantiate(CoinGameobject[4]);
                Data.transform.parent = BetTable[1].transform;
                Data.name = "PlayerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -72), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (Total_Gold1M[1] + Total_Gold100k[1] + Total_Gold10k[1])) < Total_Gold5k[1])
            {
                GameObject Data = Instantiate(CoinGameobject[3]);
                Data.transform.parent = BetTable[1].transform;
                Data.name = "PlayerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -72), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (Total_Gold1M[1] + Total_Gold100k[1] + Total_Gold10k[1] + Total_Gold5k[1])) < Total_Gold1k[1])
            {
                GameObject Data = Instantiate(CoinGameobject[2]);
                Data.transform.parent = BetTable[1].transform;
                Data.name = "PlayerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -72), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (Total_Gold1M[1] + Total_Gold100k[1] + Total_Gold10k[1] + Total_Gold5k[1] + Total_Gold1k[1])) < Total_Gold500[1])
            {
                GameObject Data = Instantiate(CoinGameobject[1]);
                Data.transform.parent = BetTable[1].transform;
                Data.name = "PlayerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -72), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (Total_Gold1M[1] + Total_Gold100k[1] + Total_Gold10k[1] + Total_Gold5k[1] + Total_Gold1k[1] + Total_Gold500[1])) < Total_Gold100[1])
            {
                GameObject Data = Instantiate(CoinGameobject[0]);
                Data.transform.parent = BetTable[1].transform;
                Data.name = "PlayerCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -72), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
        }
           
    }
    void DrawCoinVoid()
    {
        Total_Gold1M[2] = (byte)((MoneyUpData[2] / 1000000) % 10);
        Total_Gold100k[2] = (byte)((MoneyUpData[2] / 100000) % 10);
        Total_Gold10k[2] = (byte)((MoneyUpData[2] / 10000) % 10);
        Total_Gold5k[2] = (byte)((MoneyUpData[2] / 5000) % 2);
        Total_Gold1k[2] = (byte)((MoneyUpData[2] / 1000) % 5);
        Total_Gold500[2] = (byte)((MoneyUpData[2] / 500) % 2);
        Total_Gold100[2] = (byte)((MoneyUpData[2] / 100) % 5);

        // Table_Total_Gold = (byte)(Total_Gold1M + Total_Gold100k + Total_Gold10k + Total_Gold5k + Total_Gold1k + Total_Gold500 + Total_Gold100);
        Table_Total_Gold[2] = (byte)(Total_Gold1M[2] + Total_Gold100k[2] + Total_Gold10k[2] + Total_Gold5k[2] + Total_Gold1k[2] + Total_Gold500[2] + Total_Gold100[2]);

        for (byte i = 0; i < Table_Total_Gold[2]; i++)
        {
            byte XPosition = (byte)((i / 10) % 4);
            byte YPosition = (byte)(i % 10);
            byte YAddPosition = (byte)(i / 40);
            if (i < Total_Gold1M[2])
            {
                GameObject Data = Instantiate(CoinGameobject[6]);
                Data.transform.parent = BetTable[2].transform;
                Data.name = "DrawCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -66), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - Total_Gold1M[2]) < Total_Gold100k[2])
            {
                GameObject Data = Instantiate(CoinGameobject[5]);
                Data.transform.parent = BetTable[2].transform;
                Data.name = "DrawCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -66), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (Total_Gold1M[2] + Total_Gold100k[2])) < Total_Gold10k[2])
            {
                GameObject Data = Instantiate(CoinGameobject[4]);
                Data.transform.parent = BetTable[2].transform;
                Data.name = "DrawCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -66), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (Total_Gold1M[2] + Total_Gold100k[2] + Total_Gold10k[2])) < Total_Gold5k[2])
            {
                GameObject Data = Instantiate(CoinGameobject[3]);
                Data.transform.parent = BetTable[2].transform;
                Data.name = "DrawCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -66), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (Total_Gold1M[2] + Total_Gold100k[2] + Total_Gold10k[2] + Total_Gold5k[2])) < Total_Gold1k[2])
            {
                GameObject Data = Instantiate(CoinGameobject[2]);
                Data.transform.parent = BetTable[2].transform;
                Data.name = "DrawCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -66), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (Total_Gold1M[2] + Total_Gold100k[2] + Total_Gold10k[2] + Total_Gold5k[2] + Total_Gold1k[2])) < Total_Gold500[2])
            {
                GameObject Data = Instantiate(CoinGameobject[1]);
                Data.transform.parent = BetTable[2].transform;
                Data.name = "DrawCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -66), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (Total_Gold1M[2] + Total_Gold100k[2] + Total_Gold10k[2] + Total_Gold5k[2] + Total_Gold1k[2] + Total_Gold500[2])) < Total_Gold100[2])
            {
                GameObject Data = Instantiate(CoinGameobject[0]);
                Data.transform.parent = BetTable[2].transform;
                Data.name = "DrawCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -66), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
        }

    }
    void BankerPairCoinVoid()
    {
        Total_Gold1M[3] = (byte)((MoneyUpData[3] / 1000000) % 10);
        Total_Gold100k[3] = (byte)((MoneyUpData[3] / 100000) % 10);
        Total_Gold10k[3] = (byte)((MoneyUpData[3] / 10000) % 10);
        Total_Gold5k[3] = (byte)((MoneyUpData[3] / 5000) % 2);
        Total_Gold1k[3] = (byte)((MoneyUpData[3] / 1000) % 5);
        Total_Gold500[3] = (byte)((MoneyUpData[3] / 500) % 2);
        Total_Gold100[3] = (byte)((MoneyUpData[3] / 100) % 5);

        // Table_Total_Gold = (byte)(Total_Gold1M + Total_Gold100k + Total_Gold10k + Total_Gold5k + Total_Gold1k + Total_Gold500 + Total_Gold100);
        Table_Total_Gold[3] = (byte)(Total_Gold1M[3] + Total_Gold100k[3] + Total_Gold10k[3] + Total_Gold5k[3] + Total_Gold1k[3] + Total_Gold500[3] + Total_Gold100[3]);

        for (byte i = 0; i < Table_Total_Gold[3]; i++)
        {
            byte XPosition = (byte)((i / 10) % 2);
            byte YPosition = (byte)(i % 10);
            byte YAddPosition = (byte)(i / 20);

            if (i < Total_Gold1M[3])
            {
                GameObject Data = Instantiate(CoinGameobject[6]);
                Data.transform.parent = BetTable[3].transform;
                Data.name = "BankerPairCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -66), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - Total_Gold1M[3]) < Total_Gold100k[3])
            {
                GameObject Data = Instantiate(CoinGameobject[5]);
                Data.transform.parent = BetTable[3].transform;
                Data.name = "BankerPairCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -66), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (Total_Gold1M[3] + Total_Gold100k[3])) < Total_Gold10k[3])
            {
                GameObject Data = Instantiate(CoinGameobject[4]);
                Data.transform.parent = BetTable[3].transform;
                Data.name = "BankerPairCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -66), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (Total_Gold1M[3] + Total_Gold100k[3] + Total_Gold10k[3])) < Total_Gold5k[3])
            {
                GameObject Data = Instantiate(CoinGameobject[3]);
                Data.transform.parent = BetTable[3].transform;
                Data.name = "BankerPairCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -66), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (Total_Gold1M[3] + Total_Gold100k[3] + Total_Gold10k[3] + Total_Gold5k[3])) < Total_Gold1k[3])
            {
                GameObject Data = Instantiate(CoinGameobject[2]);
                Data.transform.parent = BetTable[3].transform;
                Data.name = "BankerPairCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -66), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (Total_Gold1M[3] + Total_Gold100k[3] + Total_Gold10k[3] + Total_Gold5k[3] + Total_Gold1k[3])) < Total_Gold500[3])
            {
                GameObject Data = Instantiate(CoinGameobject[1]);
                Data.transform.parent = BetTable[3].transform;
                Data.name = "BankerPairCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -66), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (Total_Gold1M[3] + Total_Gold100k[3] + Total_Gold10k[3] + Total_Gold5k[3] + Total_Gold1k[3] + Total_Gold500[3])) < Total_Gold100[3])
            {
                GameObject Data = Instantiate(CoinGameobject[0]);
                Data.transform.parent = BetTable[3].transform;
                Data.name = "BankerPairCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -66), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
        }

    }
    void PlayerPairCoinVoid()
    {
        Total_Gold1M[4] = (byte)((MoneyUpData[4] / 1000000) % 10);
        Total_Gold100k[4] = (byte)((MoneyUpData[4] / 100000) % 10);
        Total_Gold10k[4] = (byte)((MoneyUpData[4] / 10000) % 10);
        Total_Gold5k[4] = (byte)((MoneyUpData[4] / 5000) % 2);
        Total_Gold1k[4] = (byte)((MoneyUpData[4] / 1000) % 5);
        Total_Gold500[4] = (byte)((MoneyUpData[4] / 500) % 2);
        Total_Gold100[4] = (byte)((MoneyUpData[4] / 100) % 5);

        // Table_Total_Gold = (byte)(Total_Gold1M + Total_Gold100k + Total_Gold10k + Total_Gold5k + Total_Gold1k + Total_Gold500 + Total_Gold100);
        Table_Total_Gold[4] = (byte)(Total_Gold1M[4] + Total_Gold100k[4] + Total_Gold10k[4] + Total_Gold5k[4] + Total_Gold1k[4] + Total_Gold500[4] + Total_Gold100[4]);

        for (byte i = 0; i < Table_Total_Gold[4]; i++)
        {
            byte XPosition = (byte)((i / 10) % 2);
            byte YPosition = (byte)(i % 10);
            byte YAddPosition = (byte)(i / 20);
            if (i < Total_Gold1M[4])
            {
                GameObject Data = Instantiate(CoinGameobject[6]);
                Data.transform.parent = BetTable[4].transform;
                Data.name = "PlayerPairCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -66), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - Total_Gold1M[4]) < Total_Gold100k[4])
            {
                GameObject Data = Instantiate(CoinGameobject[5]);
                Data.transform.parent = BetTable[4].transform;
                Data.name = "PlayerPairCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -66), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (Total_Gold1M[4] + Total_Gold100k[4])) < Total_Gold10k[4])
            {
                GameObject Data = Instantiate(CoinGameobject[4]);
                Data.transform.parent = BetTable[4].transform;
                Data.name = "PlayerPairCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -66), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (Total_Gold1M[4] + Total_Gold100k[4] + Total_Gold10k[4])) < Total_Gold5k[4])
            {
                GameObject Data = Instantiate(CoinGameobject[3]);
                Data.transform.parent = BetTable[4].transform;
                Data.name = "PlayerPairCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -66), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (Total_Gold1M[4] + Total_Gold100k[4] + Total_Gold10k[4] + Total_Gold5k[4])) < Total_Gold1k[4])
            {
                GameObject Data = Instantiate(CoinGameobject[2]);
                Data.transform.parent = BetTable[4].transform;
                Data.name = "PlayerPairCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -66), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (Total_Gold1M[4] + Total_Gold100k[4] + Total_Gold10k[4] + Total_Gold5k[4] + Total_Gold1k[4])) < Total_Gold500[4])
            {
                GameObject Data = Instantiate(CoinGameobject[1]);
                Data.transform.parent = BetTable[4].transform;
                Data.name = "PlayerPairCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -66), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
            else if ((i - (Total_Gold1M[4] + Total_Gold100k[4] + Total_Gold10k[4] + Total_Gold5k[4] + Total_Gold1k[4] + Total_Gold500[4])) < Total_Gold100[4])
            {
                GameObject Data = Instantiate(CoinGameobject[0]);
                Data.transform.parent = BetTable[4].transform;
                Data.name = "PlayerPairCoin";
                Data.transform.localPosition = new Vector3(XPosition * 81, (YPosition * 4) + (YAddPosition * -66), 1);
                Data.transform.localScale = new Vector3(1, 1, 1);
            }
        }

    }

    void MoneyDataInit()
    {
        for (int i = 0; i < 5; i++)
        {
            MyBetMoneySeat[i] = 0;
            TableAllBetMoneySeat[i] = 0;
            MoneyUpData[i] = 0;
            Total_Gold100k[i] = 0;
            Total_Gold10k[i] = 0;
            Total_Gold5k[i] = 0;
            Total_Gold1k[i] = 0;
            Total_Gold500[i] = 0;
            Total_Gold100[i] = 0;
        }
    }
	
}
