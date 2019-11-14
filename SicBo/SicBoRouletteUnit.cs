using UnityEngine;
using System.Collections;

public class SicBoRouletteUnit : MonoBehaviour
{
    public SicBoRouletteTurnControl RouletteTurnControl;    //輪盤控制
    public byte RouletteID = 0;     //輪盤ID
    public bool TurnLeft = false;   //是否往左轉
    public bool NowRotating = false;//正在轉動    

    // Update is called once per frame
    void FixedUpdate()
    {
        if (NowRotating)
        {
            if (TurnLeft)
                transform.Rotate(0.0f, 0.0f, RouletteTurnControl.NowSpeed);
            else
                transform.Rotate(0.0f, 0.0f, -RouletteTurnControl.NowSpeed);
        }
    }

    public void StopSelf(byte DiceID)
    {
        NowRotating = false;    //停止轉動
        if (RouletteID < 3)
        {
            transform.eulerAngles = new Vector3(0.0f, 0.0f, DiceID * 20);   //設定停止的位置
            RouletteTurnControl.RouletteStops[RouletteID] = true;   //設定輪盤停止狀態
            RouletteTurnControl.CheckRouletteStops = true;  //確認輪盤停止狀態
        }
        else
            transform.eulerAngles = new Vector3(0.0f, 0.0f, (DiceID - 1) * 60); //設定停止的位置        
    }
}
