using UnityEngine;
using System.Collections;

namespace BankEnum
{
    //頁面
    public enum ENUM_BANK_PAGE
    {
        MainPage = 0,//主頁面
        BusinessPage = 1,//交易頁面
        KeyRevisePage = 2,//密碼更變頁面
        PointChangePage = 3,//點數換金幣頁面
        GiftPage = 4,//儲值頁面
        KeyForgetPage = 5,//忘記密碼頁面
    }

    //訊息
    public enum ENUM_BANK_MESSAGE_STATUS
    {
        //點數
        NoPoint = 1001,//沒有點數
        CheckPoint = 1002,//確定是否要換點數
        ChangeEndPoint = 1003,//點數兌換結束
        KeyForgetSuccess = 1004,//修改密碼成功
        KeyForgetVerifyFial = 1005,//修改密碼失敗
        DataClickError = 1006,//資料輸入錯誤
        LevelNoClear = 1007,//等級不足
        PhoneNoClear = 1008,//手機尚未認證
        PassNoClear = 1009,//尚未設定密碼
        KeyError = 1010,//密碼錯誤
        BusinessCancel = 1011,//交易取消
        NoPlayer = 1012,//交易對象不存在
        InfoError = 1013,//交易對象資料不符合
        PlayerNowBusiness = 1014,//交易對象正在交易中
        LessThanZero = 1015,//金額小於零
        NoHoldCash = 1016,//持有金不足
        Thousand = 1017,//以千元為單位
        NoCash = 1018,//餘額不足
        TodayOutError = 1019,// 超出當日匯出上限
        TodayInError = 1020,// 超出當日匯入上限
        OutCashError = 1021,// 超出單次匯入上限
        OneAbove = 1022,
        BankOutMax = 1023,
    }
}
