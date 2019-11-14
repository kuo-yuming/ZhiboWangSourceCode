using UnityEngine;
using System.Collections;
using GameCore.Manager.Baccarat;
using GameEnum;
public class Money_Control : MonoBehaviour {
    public UILabel MyMoney_Label;
    public UILabel MyBetMoney_Label;

    public static ulong MyMoney = 0;
    public static int MyBetMoney = 0;

    public static ulong SaveMyMoney = 0;
    public static bool CoinInitBool = false;
	// Use this for initialization
	void Start () {
        CoinInitBool = false;
        MyMoney = (ulong)GameConnet.m_TMachineBuyInGameData.m_uiGameMoney;
        MyBetMoney = 0;
	}
	
	// Update is called once per frame
	void Update () {
        MyMoney_Label.text = (MyMoney - (ulong)MyBetMoney).ToString();
        MyBetMoney_Label.text = MyBetMoney.ToString();

        if (CoinInitBool)
        {
            MyBetMoney = 0;
            MyMoney = SaveMyMoney;
            CoinInitBool = false;
        }
	}
}
