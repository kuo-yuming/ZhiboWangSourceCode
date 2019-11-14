using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore.Manager.SicBo;
using GameCore;
using System;

public class SicBoWinAreaUnit : MonoBehaviour
{
    public ENUM_SicBo_AWARD_AREA AwardType = ENUM_SicBo_AWARD_AREA.OneDice;
    public byte Offset; //押注區塊補充值   例如"單一豹子"區的333,則填為3.  但任一豹子/四枚及通殺填0即可.
    public UISprite SelfSprite; //圖片
    public bool IamFlashing = false;//是否閃爍
}