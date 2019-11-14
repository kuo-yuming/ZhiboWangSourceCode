using UnityEngine;
using System.Collections;
using MoneyTable;

public class BJMoney_Control : MonoBehaviour {

    public TweenPosition Money_TweenPosition;
    public UISprite Money_Sprite;
    public TableList ThisTable;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Cash_Control.CashMoveStart)
        {
            Money_TweenPosition.PlayForward();
        }
	}

    public void MoveEnd()
    {
        Money_Sprite.enabled = false;
        if (ThisTable == TableList.MyTable)
        {
            Cash_Control.CashMoveEnd[(byte)TableList.MyTable] = 1;
        }
        else if (ThisTable == TableList.PlayerTable1)
        {
            Cash_Control.CashMoveEnd[(byte)TableList.PlayerTable1] = 1;
        }
        else if (ThisTable == TableList.PlayerTable2)
        {
            Cash_Control.CashMoveEnd[(byte)TableList.PlayerTable2] = 1;
        }
        else if (ThisTable == TableList.PlayerTable3)
        {
            Cash_Control.CashMoveEnd[(byte)TableList.PlayerTable3] = 1;
        }
        else if (ThisTable == TableList.PlayerTable4)
        {
            Cash_Control.CashMoveEnd[(byte)TableList.PlayerTable4] = 1;
        }
    }
}
