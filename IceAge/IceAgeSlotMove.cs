using UnityEngine;
using System.Collections;

public class IceAgeSlotMove : MonoBehaviour
{
    public UISprite[] m_ChildSprites = new UISprite[4]; //Slot陣列    
    public byte SlotID;
    public bool StopMove = false;

    public byte SelfMainSlot = 0;    //主要Slot的位置
    public byte SelfStopPos = 0;    //停止時 主要Slot的位置
    public int[] SelfSlotPos;   //停止時的圖順
    public bool SelfMoving = false;
    public bool SelfStopMove = false;

    // Use this for initialization
    void Start()
    {
        //初始給予隨機圖
        foreach (UISprite spris in m_ChildSprites)
        {
            int Image = Random.Range(1, 10);
            spris.spriteName = ("Sym" + Image);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SelfMoving) SelfSlotMoving();
        if (SelfStopMove) SelfMoveStop();
    }

    public void MoveReady()
    {
        foreach (var item in m_ChildSprites)
            item.GetComponent<TweenPosition>().PlayForward();
    }
    public void MoveStart()
    {   //開始轉動時 每個Slot都將自己的SelfMoving=true 關閉時 五個Slot都停止後 才開始後續動作
        if (SelfStopMove/* && IceAgeManager.GetAward*/)
        {
            IceAgeSoundControl.Inst.PlaySound(IceAgeSoundControl.SoundName.SlotEnd, false);   //播放音效
            SelfStopMove = false;   //設定自身狀態
            IceAgeSlotControl.Inst.NumOfStopSlot += 1;  //已停止的Slot個數 +1
            if (IceAgeSlotControl.Inst.NumOfStopSlot == 5)
                IceAgeGameMain.Inst.GetBetResult(); //最後一個slot停止後 執行結果確認
        }
        else
        {
            SelfMoving = true;   //打開自控轉動
            //IceAgeSlotControl.Inst.NumOfStopSlot -= 1;  //已停止的Slot個數 -1
        }

    }

    void SelfSlotMoving()
    {   //移動時 m_ChildSprites[0] 為MainSlot //MainSlot = m_ChildSprites[0] 在Slot中的順位 //請參照最下方 Slot位置規劃表
        Vector3 MovePosition = IceAgeSlotControl.Inst.speed * Time.deltaTime * Vector3.down;   //計算移動量
        for (int ID = 0; ID < m_ChildSprites.Length; ID++)
        {   //如果 m_ChildSprites[ID] 到最下方
            if (m_ChildSprites[ID].transform.localPosition.y <= -IceAgeSlotControl.Inst.MoveRange)
            {
                if (ID == 0)
                    SelfMainSlot = 0;   //更換順序後 MainSlot的位置
                else
                    SelfMainSlot = (byte)(m_ChildSprites.Length - ID);
                // m_ChildSprites[ID] 變換圖示
                int Image = Random.Range(1, 10);
                m_ChildSprites[ID].spriteName = ("Sym" + Image);
                // m_ChildSprites[ID] 移動到 m_ChildSprites[ID+1] 上方 MoveRange / 2 的位置
                if (ID == 3)
                    m_ChildSprites[ID].transform.localPosition = new Vector3(0, m_ChildSprites[0].transform.localPosition.y + IceAgeSlotControl.Inst.MoveRange / 2, 0);
                else
                    m_ChildSprites[ID].transform.localPosition = new Vector3(0, m_ChildSprites[ID + 1].transform.localPosition.y + IceAgeSlotControl.Inst.MoveRange / 2, 0);
            }
            else ////如果 m_ChildSprites[ID] 未到最下方
            {
                if (ID == 0)    //MainSlot 正常移動
                    m_ChildSprites[ID].transform.localPosition += MovePosition;
                else  //其他則根據 MainSlot 的位置 去計算 新的位置  //詳情見最下面 Slot狀態規劃表
                {
                    if (SelfMainSlot < m_ChildSprites.Length - ID)
                        m_ChildSprites[ID].transform.localPosition = new Vector3(0, m_ChildSprites[0].transform.localPosition.y - (ID * (IceAgeSlotControl.Inst.MoveRange / 2)), 0);
                    else
                        m_ChildSprites[ID].transform.localPosition = new Vector3(0, m_ChildSprites[0].transform.localPosition.y - ((ID - m_ChildSprites.Length) * (IceAgeSlotControl.Inst.MoveRange / 2)), 0);
                }
            }
        }
    }

    public void SetSelfSprite()
    {
        // 停止位置為 所有物件下移一位 所以 StopPos = MainSlot + 1;
        SelfStopPos = (SelfMainSlot + 1 < m_ChildSprites.Length) ? (byte)(SelfMainSlot + 1) : (byte)0;
        // 使用 StopPos 計算陣列 //計算結果請參照最下方 Slot位置規劃表
        SelfSlotPos = new int[4] { 0, 0, 0, 0 };
        for (byte i = 0; i < m_ChildSprites.Length; i++)
            SelfSlotPos[i] = (i - SelfStopPos < 0) ? m_ChildSprites.Length + (i - SelfStopPos) : i - SelfStopPos;
        //print(string.Format("SlotID = {0}    SelfSlotPos=[{1},  {2},  {3},  {4}]", SlotID, SelfSlotPos[0], SelfSlotPos[1], SelfSlotPos[2], SelfSlotPos[3]));
        //給圖
        for (int i = 0; i < IceAgeManager.m_BetResult.m_byarGridSymbol.Length; i++)
        {   //IceAgeSM[0~4].m_ChildSprites[SlotPos[1~3]].spriteName = ("Sym" + IceAgeManager.m_BetResult.m_byarGridSymbol[0~14]);
            if (i < 5 && i == SlotID) m_ChildSprites[SelfSlotPos[1]].spriteName = ("Sym" + IceAgeManager.m_BetResult.m_byarGridSymbol[i]);
            else if (i > 4 && i < 10 && i % 5 == SlotID) m_ChildSprites[SelfSlotPos[2]].spriteName = ("Sym" + IceAgeManager.m_BetResult.m_byarGridSymbol[i]);
            else if (i > 9 && i % 10 == SlotID) m_ChildSprites[SelfSlotPos[3]].spriteName = ("Sym" + IceAgeManager.m_BetResult.m_byarGridSymbol[i]);
        }
    }

    public void SelfMoveStop()
    {   //目前圖順最上方(0)圖停止
        if (SelfStopPos == 0)
        {
            m_ChildSprites[0].transform.localPosition = new Vector3(0, IceAgeSlotControl.Inst.MoveRange, 0);
            m_ChildSprites[0].transform.GetComponent<TweenPosition>().from = m_ChildSprites[0].transform.localPosition;
            m_ChildSprites[0].transform.GetComponent<TweenPosition>().to = m_ChildSprites[0].transform.localPosition - new Vector3(0, 20, 0);
        }
        else
        {
            m_ChildSprites[m_ChildSprites.Length - SelfStopPos].transform.localPosition = new Vector3(0, IceAgeSlotControl.Inst.MoveRange, 0);
            m_ChildSprites[m_ChildSprites.Length - SelfStopPos].transform.GetComponent<TweenPosition>().from = m_ChildSprites[m_ChildSprites.Length - SelfStopPos].transform.localPosition;
            m_ChildSprites[m_ChildSprites.Length - SelfStopPos].transform.GetComponent<TweenPosition>().to = m_ChildSprites[m_ChildSprites.Length - SelfStopPos].transform.localPosition - new Vector3(0, 20, 0);
        }


        for (int i = 1; i < m_ChildSprites.Length; i++)
        {   //目前圖順的下三張(1,2,3)圖停止     
            byte SpriteStop = (i - SelfStopPos < 0) ? (byte)(m_ChildSprites.Length + (i - SelfStopPos)) : (byte)(i - SelfStopPos);
            m_ChildSprites[SpriteStop].transform.localPosition = new Vector3(0, IceAgeSlotControl.Inst.MoveRange - i * (IceAgeSlotControl.Inst.MoveRange / 2), 0);
            m_ChildSprites[SpriteStop].transform.GetComponent<TweenPosition>().from = m_ChildSprites[SpriteStop].transform.localPosition;
            m_ChildSprites[SpriteStop].transform.GetComponent<TweenPosition>().to = m_ChildSprites[SpriteStop].transform.localPosition - new Vector3(0, 20, 0);
            //設定靜態圖陣列   //設定完成後 (Array)StaticPic 會變成與 (GameObject)SlotPicControl 相同的結構
            IceAgeLineAnimControl.Inst.StaticPic[(i - 1) * 5 + SlotID] = m_ChildSprites[SpriteStop].transform.GetComponent<UISprite>();
        }

        foreach (var item in m_ChildSprites)
            item.GetComponent<TweenPosition>().PlayReverse();
        SelfMainSlot = SelfStopPos; //更新SelfMainSlot位置
    }

    #region Slot位置規劃表
    //  0   1   2   3
    //---------------------------------
    //  0   3   2   1
    //  1   0   3   2
    //  2   1   0   3
    //  3   2   1   0
    #endregion

    #region Slot狀態規劃表                                    
    //Sprites.Length - ID                         3                   2                   1
    //圖片位置              MainSlot      m_ChildSprites[1]   m_ChildSprites[2]   m_ChildSprites[3]
    //                        0                   1                   2                   3
    //                        1                   1                   2                  -1
    //                        2                   1                  -2                  -1
    //                        3                  -3                  -2                  -1
    #endregion
}