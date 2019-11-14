using UnityEngine;
using System.Collections;

public class MyBetCashTip_Control : MonoBehaviour {
    public UISprite[] TipSprite = new UISprite[5];
    public UILabel[] TipLabel = new UILabel[5];

    /////////////修改   之後移動到BETCASH
    public static uint[] MyBetCashData = new uint[5];//0:莊,1:閒,2:和,3:莊對,4:閒對
    public static bool TipCashInitBool = false;
    ///////////////////

	// Use this for initialization
	void Start () {
        TipCashInitBool = false;
        for (int i = 0; i < 5; i++)
        {
            MyBetCashData[i] = 0;
        }
	}
	
	// Update is called once per frame
	void Update () {
        TipShow();
        TipLabel[0].text = MyBetCashData[0].ToString();
        TipLabel[1].text = MyBetCashData[1].ToString();
        TipLabel[2].text = MyBetCashData[2].ToString();
        TipLabel[3].text = MyBetCashData[3].ToString();
        TipLabel[4].text = MyBetCashData[4].ToString();

        if (TipCashInitBool)
        {
            for (int i = 0; i < 5; i++)
            {
                MyBetCashData[i] = 0;
            }
            TipCashInitBool = false;
        }
	}

    void TipShow()
    {
        /////莊Tips
        if (MyBetCashData[0] == 0)
        {
            TipSprite[0].enabled = false;
            TipLabel[0].enabled = false;
        }
        else
        {
            TipSprite[0].enabled = true;
            TipLabel[0].enabled = true;
        }

        /////閒Tips
        if (MyBetCashData[1] == 0)
        {
            TipSprite[1].enabled = false;
            TipLabel[1].enabled = false;
        }
        else
        {
            TipSprite[1].enabled = true;
            TipLabel[1].enabled = true;
        }

        /////和Tips
        if (MyBetCashData[2] == 0)
        {
            TipSprite[2].enabled = false;
            TipLabel[2].enabled = false;
        }
        else
        {
            TipSprite[2].enabled = true;
            TipLabel[2].enabled = true;
        }

        /////莊對Tips
        if (MyBetCashData[3] == 0)
        {
            TipSprite[3].enabled = false;
            TipLabel[3].enabled = false;
        }
        else
        {
            TipSprite[3].enabled = true;
            TipLabel[3].enabled = true;
        }

        /////閒對Tips
        if (MyBetCashData[4] == 0)
        {
            TipSprite[4].enabled = false;
            TipLabel[4].enabled = false;
        }
        else
        {
            TipSprite[4].enabled = true;
            TipLabel[4].enabled = true;
        }
    }
}
