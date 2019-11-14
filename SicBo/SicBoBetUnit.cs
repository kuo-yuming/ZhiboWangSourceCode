using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore.Manager.SicBo;
using GameCore;
using System;

public class SicBoBetUnit : MonoBehaviour
{
    public BoxCollider MyCollider;  //Collider
    public UISprite MyChipSprite;   //籌碼圖片
    public UILabel MyChipLabel;     //投注金額Label
    public uint MyChipUint;         //投注金額Uint
    public UISprite MyChipBack;     //金額背景
    public ENUM_SicBo_AWARD_AREA AwardType = ENUM_SicBo_AWARD_AREA.OneDice;
    public byte Offset; //押注區塊補充值   例如"單一豹子"區的333,則填為3.  但任一豹子/四枚及通殺填0即可.
    public TweenPosition ChipTweenPos;  //籌碼TweenPosition
    public Vector3 BankerVector3;   //莊家收回籌碼的位置
    public Vector3 PlayerVector3;   //玩家得到籌碼的位置

    public void ClickBet()
    {   //要求押注        
        if ((int)SicBoGameMain.Inst.ButtonControl.NowMoney - (int)SicBoGameMain.Inst.NowQuota >= 0)
        {   //如果金錢足夠
            CPACK_SicBo_ReqBet TableReqBet = new CPACK_SicBo_ReqBet();
            TableReqBet.m_byAreaID = (byte)AwardType;
            TableReqBet.m_byOffset = (byte)Offset;
            TableReqBet.m_iAddBet = (int)SicBoGameMain.Inst.NowQuota;
            GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.SicBo, (uint)ENUM_SICBO_PACKID_GC.C2G_Game_ReqBet, GameConnet.m_oGameClient.DoSerialize<CPACK_SicBo_ReqBet>(TableReqBet));            
        }
        else //顯示警告 //金錢不足
        {
            Message_Control.OpenMessage = true;
            Message_Control.MessageStatus = Message_Control.MessageStatu.Buyin_MoneyNotEnough;
        }
    }

    public void CancelBet()
    {   //取消押注
        if (MyChipBack.enabled)
        {   //如果金額背景有開啟 代表有押注此區
            CPACK_SicBo_ReqBet TableReqBet = new CPACK_SicBo_ReqBet();
            TableReqBet.m_byAreaID = (byte)AwardType;
            TableReqBet.m_byOffset = (byte)Offset;
            TableReqBet.m_iAddBet = -(int)MyChipUint;
            GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.SicBo, (uint)ENUM_SICBO_PACKID_GC.C2G_Game_ReqBet, GameConnet.m_oGameClient.DoSerialize<CPACK_SicBo_ReqBet>(TableReqBet));            
        }
    }

    public void AutoBet(int BetMoney)
    {   //自動押注
        if ((int)SicBoGameMain.Inst.ButtonControl.NowMoney - (int)SicBoGameMain.Inst.NowQuota >= 0)
        {   //如果金錢足夠
            CPACK_SicBo_ReqBet TableReqBet = new CPACK_SicBo_ReqBet();
            TableReqBet.m_byAreaID = (byte)AwardType;
            TableReqBet.m_byOffset = (byte)Offset;
            TableReqBet.m_iAddBet = BetMoney;
            GameConnet.m_oGameClient.Send(ENUM_GAME_FRAME.SicBo, (uint)ENUM_SICBO_PACKID_GC.C2G_Game_ReqBet, GameConnet.m_oGameClient.DoSerialize<CPACK_SicBo_ReqBet>(TableReqBet));            
        }
        else //顯示警告 //金錢不足
        {
            Message_Control.OpenMessage = true;
            Message_Control.MessageStatus = Message_Control.MessageStatu.Buyin_MoneyNotEnough;
        }
    }

    public void InitMySelf()
    {
        MyCollider.enabled = false;    //Collider
        MyChipSprite.enabled = false;  //籌碼圖片
        MyChipLabel.enabled = false;   //投注金額
        MyChipBack.enabled = false;    //金額背景
    }

    public void OpenMySelf()
    {
        MyCollider.enabled = true;    //Collider
    }

    public void CloseMySelf()
    {
        MyCollider.enabled = false;    //Collider
    }

    public void ShowMyBet(uint BetMoney)
    {
        if (BetMoney <= 0)
        {   //隱藏
            MyChipLabel.enabled = false;//投注金額
            MyChipBack.enabled = false; //金額背景
        }
        else
        {   // > 0 顯示
            MyChipLabel.enabled = true;   //投注金額
            MyChipBack.enabled = true;    //金額背景
            MyChipLabel.text = BetMoney.ToString(); //設定金額Label
            MyChipUint = BetMoney;  //設定金額Uint
        }
    }

    public void HideMyBet()
    {
        MyChipLabel.enabled = false;   //投注金額
        MyChipBack.enabled = false;    //金額背景
    }

    public void ShowAllBet(uint AllBetMoney)
    {   //根據總投注面額決定籌碼圖片 面額：100、500、1,000、5,000、10,000、100,000、1,000,000
        MyChipSprite.enabled = true;    //開啟籌碼圖片        
        if (AllBetMoney >= 1000000)
            MyChipSprite.spriteName = "chip1000000";//如果總投注 > 1,000,000
        else if (AllBetMoney >= 100000)
            MyChipSprite.spriteName = "chip100000"; //如果總投注 > 100,000
        else if (AllBetMoney >= 10000)
            MyChipSprite.spriteName = "chip10000";  //如果總投注 > 10,000
        else if (AllBetMoney >= 5000)
            MyChipSprite.spriteName = "chip5000";  //如果總投注 > 5,000
        else if (AllBetMoney >= 1000)
            MyChipSprite.spriteName = "chip1000";  //如果總投注 > 1,000
        else if (AllBetMoney >= 500)
            MyChipSprite.spriteName = "chip500";   //如果總投注 > 500
        else if (AllBetMoney >= 100)
            MyChipSprite.spriteName = "chip100";   //如果總投注 > 100
        else
            MyChipSprite.enabled = false;   //如果未滿100 隱藏圖片
        //圖片適應尺寸
        if (AllBetMoney >= 1000000)
        {
            MyChipSprite.width = 56;
            MyChipSprite.height = 40;
        }
        else
        {
            MyChipSprite.width = 40;
            MyChipSprite.height = 40;
        }
    }

    public void PayoutOver()
    {   //派彩完畢 籌碼初始化
        MyChipSprite.enabled = false;   //關閉圖片
        MyChipSprite.transform.localPosition = ChipTweenPos.from;   //恢復位置
    }
}
