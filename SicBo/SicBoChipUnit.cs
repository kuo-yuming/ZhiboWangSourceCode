using UnityEngine;
using System.Collections;

public class SicBoChipUnit : MonoBehaviour
{
    public byte MyID;
    public uint MyQuota;
    public UISprite MyBackground;
    public BoxCollider MyCollider;

    void ClickMySelf()
    {   //如果不是點目前的籌碼 切換目前選擇籌碼
        if (MyID != SicBoGameMain.Inst.NowChipID)
            SicBoGameMain.Inst.ChipControl.SetNowChoose(MyID);
    }

    public void SetQuota(uint Quota)
    {   //進入遊戲時 設定籌碼面額
        MyBackground.spriteName = "chip" + Quota;
        MyQuota = Quota;
    }

    public void SetSwitch(bool NowSwitch)
    {   //選擇籌碼 切換籌碼顯示
        if (NowSwitch)
            MyBackground.color = Color.white;
        else
            MyBackground.color = Color.gray;
    }

    public void ChipCanChoose(bool CanChoose)
    {   //關閉籌碼Collider
        MyCollider.enabled = CanChoose;
    }
}