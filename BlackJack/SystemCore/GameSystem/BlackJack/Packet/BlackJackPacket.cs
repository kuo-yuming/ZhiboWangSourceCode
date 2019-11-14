using System;
using System.Collections.Generic;
using ProtoBuf;
using GameCore;

namespace GameCore.Manager.BlackJack
{

    //----------------------------------------------------------------------------------------------------------
    // 21點系統 相關的封包定義
    //----------------------------------------------------------------------------------------------------------
    // GS<->UC間的21點系統的封包ID的列舉. (值須小於_DEF_GAME_FRAME_AID_BASE)
    // 使用的GameFrame為 ENUM_GAME_FRAME.Baccarat (同樣GameFrame的系統,請使用同一組列舉)
    // 非Common系列的列舉,須從10000開始

    public enum ENUM_BlackJack_PACKID_GC
    {
        StartPackID = ENUM_COMMON_PACKID_GC.MaxPackID,
        // 系統初始化
        G2C_NotifyGameConfig = 10001,    // GS->UC 通知21點特有的遊戲相關設定.    CPACK_BlackJack_GameConfig

        // 機台資訊相關
        //        C2G_Machine_GetMachineInfo = 10021,    // UC->GS 要求取得某機台的成員名單及機台資訊.  uint 機台ID
        //        G2C_Machine_NotifyMachineInfo = 10022,    // GS->UC 通知某幾台的機台資訊.   CPACK_Baccarat_MachineInfo   
        
        // 遊戲
        G2C_Game_NotifyTableInfo = 10031,   // GS->UC 通知此機台的遊戲資訊    CPACK_BlackJack_NotifyTableInfo
        G2C_Game_NotifyState = 10032,    // GS->UC 通知更新某機台的狀態階段.   CPACK_BlackJack_UpdateTbleState   
        G2C_Game_NotifyShuffle = 10033,    // GS->UC 通知洗牌   CPACK_BlackJack_NotifyShuffle   
        G2C_Game_SeatDataAll = 10034,       //GS -> UC 通知一組 現在的座位資訊  CAllSeatData
        G2C_Game_SeatDataOne = 10035,       //GS -> UC 通知一位 現在的座位資訊  CSeatPlayerData
        G2C_Game_PlayerOut = 10036,       //GS -> UC 通知玩家有人離開  CSeatPlayerData

        C2G_Game_ReqBet = 10041,    // UC->GS 要求 在某區域押注     CPACK_BlackJack_ReqBet
        G2C_Game_NotifyBet = 10042,    // GS->UC 通知 更新某區域的押金變動     CPACK_BlackJack_NotifyBet    (會廣播給桌內所有成員. 若有錯,則只會回覆給要求者)

        G2C_Game_NotifyDeal = 10051,    // GS->UC 通知發一般牌        CPACK_BlackJack_NotifyDealData

        G2C_Game_NowTargetPlayer = 10060,    // GS->UC 通知 現在可動作之玩家     CNowCanDoTarget
        C2G_Game_HIT = 10061,    // UC->GS 要求 要牌     Null
        C2G_Game_STAND = 10062,    // UC->GS 要求 停牌     Null
        C2G_Game_SPLIT = 10063,    // UC->GS 要求 分牌     Null
        C2G_Game_DOUBLE = 10064,    // UC->GS 要求 加倍下注     Null
        C2G_Game_SURRENDER = 10065,    // UC->GS 要求 投降     Null
        C2G_Game_BLACKJACK = 10066,    // UC->GS 要求 報到     Null
        C2G_Game_INSURE = 10067,    // UC->GS 要求 保險     Null

        G2C_Game_playerDo = 10068,      //GS->UC 回復玩家所做動作之結果       CPACK_BlackJack_RePlayerDo

        G2C_Game_Abstain = 10069,    // GS->UC 通知有玩家中途離開 CSeatPlayerData



        G2C_Game_NotifyAward = 10071,    // GS->UC 通知 結算結果       CPACK_BlackJack_NotifyAward
        G2C_Game_AllPlayerWin = 10072,    // GS->UC 通知 所有玩家的贏金       CPACK_BlackJack_PlayerRank



    }
    //----------------------------------------------------------------------------------------------------------
    // 一副牌組卡片ID
    public enum ENUM_BACCARAT_CARD_ID
    {
        NULL = 0,       // 無
        Club_One = 01,   // 梅花 A
        Club_Two = 02,   // 梅花 2
        Club_Three = 03,   // 梅花 3
        Club_Four = 04,
        Club_Five = 05,
        Club_Six = 06,
        Club_Seven = 07,
        Club_Eight = 08,
        Club_Nine = 09,
        Club_Ten = 10,
        Club_Eleven = 11,
        Club_Twelve = 12,
        Club_Thirteen = 13,// 梅花 13
        Diamond_One = 14,   // 方塊 A
        Diamond_Two = 15,   // 方塊 2
        Diamond_Three = 16,
        Diamond_Four = 17,
        Diamond_Five = 18,
        Diamond_Six = 19,
        Diamond_Seven = 20,
        Diamond_Eight = 21,
        Diamond_Nine = 22,
        Diamond_Ten = 23,
        Diamond_Eleven = 24,
        Diamond_Twelve = 25,
        Diamond_Thirteen = 26,// 方塊 13
        Heart_One = 27,   // 紅心 A
        Heart_Two = 28,   // 紅心 2
        Heart_Three = 29,
        Heart_Four = 30,
        Heart_Five = 31,
        Heart_Six = 32,
        Heart_Seven = 33,
        Heart_Eight = 34,
        Heart_Nine = 35,
        Heart_Ten = 36,
        Heart_Eleven = 37,
        Heart_Twelve = 38,
        Heart_Thirteen = 39,// 紅心 13
        Spade_One = 40,   // 黑桃 A
        Spade_Two = 41,   // 黑桃 2
        Spade_Three = 42,
        Spade_Four = 43,
        Spade_Five = 44,
        Spade_Six = 45,
        Spade_Seven = 46,
        Spade_Eight = 47,
        Spade_Nine = 48,
        Spade_Ten = 49,
        Spade_Eleven = 50,
        Spade_Twelve = 51,
        Spade_Thirteen = 52,// 黑桃 13
    };
    //----------------------------------------------------------------------------------------------------------
    // 押注區域 
    public enum ENUM_BLACKJACK_AWARD_AREA
    {
        Player = 1, // 玩家押注金額
        Insurance = 2, // 保險

        //----------------------------------------------------- 
        MAX,
    }
    //----------------------------------------------------------------------------------------------------------
    // 獎項列舉
    public enum ENUM_BLACKJACK_CardStatus
    {
        Idle = 0,  //初始(未計算)
        Normal = 1,//一般狀態(已計算)
        CardBust = 2,//爆牌
        BlackJack = 3,  //報到
        DoubleDown = 4, //加倍下注

    }
    //----------------------------------------------------------------------------------------------------------
    // 獎項列舉
    public enum ENUM_BLACKJACK_PLAYERDO
    {
        Idle = 0,  //初始(未計算)
        INSURE = 1,//保險
        HIT = 2,//要牌
        STAND = 3,  //停牌
        SPLIT = 4, //分牌
        DOUBLE = 5,//加倍下注
        SURRENDER = 6,  //投降
        BLACKJACK = 7, //報到

    }
    //----------------------------------------------------------------------------------------------------------
    // 獎項列舉
    public enum ENUM_BLACKJACK_AWARD
    {
        Idle = 0,
        WinDraw = 1,  //和贏
        WinBanker = 2,//莊贏
        WinPlayer = 3,//閒贏
        BlackJack = 4,//閒家選擇報到
    }
    //----------------------------------------------------------------------------------------------------------
    // 21點 檯桌的狀態階段 列舉
    public enum ENUM_BLACKJACK_TABLE_STATE
    {
        Idle = 0,         // 閒置 (沒人時的狀態)
        NewRound = 1,         // 新局開始
        ShuffleNewRound = 2,    // 一般洗牌兼新局開始
        WaitBet = 3,         // 等待押注 (通常是Buyin時會收到此狀態)
        GameStart = 4,         // 遊戲開始(開始發牌)

        CheckInsurance = 5,         // 檢查是否需要開放買保險
        InsuranceTime = 6,         // 購買保險時間
        InsuranceOver = 7,          // 保險時間結束(檢查結果)莊: 21點(直接結算) 莊 無21點(繼續遊戲)

        CheckPlayer = 8,      //檢查玩家(再來是誰下注)
        PlayerTime = 9,         //玩家動作時間
        PlayerOver = 10,        //玩家動作結束

        GameSettlement = 11,          //遊戲結算(檢查玩家狀態)
        GameOver = 12,          //遊戲再開

    }

    //----------------------------------------------------------------------------------------------------------
    // GS <-> Client
    #region GS_Client

    // 21點 區塊押注限制
    [ProtoContract]
    public class CBlackJack_BetLimit
    {
        [ProtoMember(1, IsRequired = true)]
        public uint m_uiMinBet = 0;    // 最小押注金
        [ProtoMember(2, IsRequired = true)]
        public uint m_uiMaxBet = 0;    // 最大押注金
    }




    // 21點 特有的遊戲相關設定
    [ProtoContract]
    public class CPACK_BlackJack_GameConfig
    {
        [ProtoMember(1, IsRequired = true)]
        public byte m_byPlayCardGroup = 8;  // 莊閒牌組數
        [ProtoMember(2, IsRequired = true)]
        public Dictionary<byte, CBlackJack_BetLimit> m_dicBetLimit = new Dictionary<byte, CBlackJack_BetLimit>();   // 各區塊賠率表 <區塊ID, 對應區塊的押注限制>
    }
    //----------------------------------------------------------------------------------------------------------
    //獎項結果
    [ProtoContract]
    public class CAllAward
    {
        [ProtoMember(1, IsRequired = true)]
        public COneCardBetAward m_BankerCards = new COneCardBetAward();
        [ProtoMember(2, IsRequired = true)]
        public Dictionary<byte, CPlayerBetAward> m_dicPlayerCardAward = new Dictionary<byte, CPlayerBetAward>();
    }

    //單一座位資料
    [ProtoContract]
    public class CSeatPlayerData
    {
        [ProtoMember(1, IsRequired = true)]
        public uint m_uiPlayerDBID = 0;
        [ProtoMember(2, IsRequired = true)]
        public byte m_bySeatID = 0;
        [ProtoMember(3, IsRequired = true)]
        public string m_strNickName = "";
        [ProtoMember(4, IsRequired = true)]
        public ulong m_uiPlayerBet = 0;

    }

    //全部的資料
    [ProtoContract]
    public class CAllSeatData
    {
        [ProtoMember(1, IsRequired = true)]
        public List<CSeatPlayerData> m_liSeatDatas = new List<CSeatPlayerData>();
    }


    // 詳細獎項結果
    [ProtoContract]
    public class CPlayerBetAward
    {
        [ProtoMember(1, IsRequired = true)]
        public byte m_bySeatID = 0;  // 座位ID
        [ProtoMember(2, IsRequired = true)]
        public uint m_uiDBID = 0;  // DBID
        [ProtoMember(3, IsRequired = true)]
        public COneCardBetAward m_byMainCardList = new COneCardBetAward();  // 第一副牌 牌組
        [ProtoMember(4, IsRequired = true)]
        public COneCardBetAward m_byOtherCardList = new COneCardBetAward();  // 第二副牌 牌組

    }


    // 詳細獎項結果
    [ProtoContract]
    public class COneCardBetAward
    {
        [ProtoMember(1, IsRequired = true)]
        public List<byte> m_byMainCardList = new List<byte>();  // 牌組
        [ProtoMember(2, IsRequired = true)]
        public byte m_bySam = 0;  // 點數和
        [ProtoMember(3, IsRequired = true)]
        public ENUM_BLACKJACK_CardStatus m_ENCardStatus = ENUM_BLACKJACK_CardStatus.Idle;  // 卡組狀態
        [ProtoMember(4, IsRequired = true)]
        public ENUM_BLACKJACK_AWARD m_ENAwardStatus = ENUM_BLACKJACK_AWARD.Idle;  // 獎項狀態

    }

    // 機台的戰績資訊
    [ProtoContract]
    public class CPACK_BlackJack_NotifyTableInfo
    {
        [ProtoMember(1, IsRequired = true)]
        public ushort m_usRemainNormalCards = 0;    // 一般牌組的剩餘張數

    }
    //----------------------------------------------------------------------------------------------------------
    // 21點 更新檯桌狀態階段
    [ProtoContract]
    public class CPACK_BlackJack_UpdateTbleState
    {
        [ProtoMember(1, IsRequired = true)]
        public ENUM_BLACKJACK_TABLE_STATE m_enumState = ENUM_BLACKJACK_TABLE_STATE.Idle; // 檯桌的狀態階段
        [ProtoMember(2, IsRequired = true)]
        public uint m_uiWaitMSec = 0;   // 此狀態的剩餘等待毫秒數. 單位: ms
    }
    //----------------------------------------------------------------------------------------------------------
    // 21點 通知洗牌
    [ProtoContract]
    public class CPACK_BlackJack_NotifyShuffle
    {
        [ProtoMember(1, IsRequired = true)]
        public bool m_bNormal = false;  // 洗一般牌組


    }
    //----------------------------------------------------------------------------------------------------------
    // 21點 要求 加押/減押
    [ProtoContract]
    public class CPACK_BlackJack_ReqBet
    {
        [ProtoMember(1, IsRequired = true)]
        public int m_iAddBet = 0;       // 在指定區域 加押/減押 多少遊戲幣.   正值為加押, 負值為減押=取消全部押注.

    }
    //--------------------------------------------------------------------------
    // 21點 通知更新某區域的押金變動
    [ProtoContract]
    public class CPACK_BlackJack_NotifyBet
    {
        [ProtoMember(1, IsRequired = true)]
        public ENUM_COMMON_ERROR_CODE m_enumResult = ENUM_COMMON_ERROR_CODE.Fail;   // 處理結果
        [ProtoMember(2, IsRequired = true)]
        public uint m_uiBetPlayerDBID = 0;  // 押注者DBID
        [ProtoMember(3, IsRequired = true)]
        public byte m_bySeatID = 0;         // 押注座位
        [ProtoMember(4, IsRequired = true)]
        public int m_uiPlayerBetMoney = 0; // 押注者在此區塊的押注總額

    }
    //--------------------------------------------------------------------------
    // 21點 通知更新某玩家的事件成功狀態
    [ProtoContract]
    public class CPACK_BlackJack_RePlayerDo
    {

        [ProtoMember(1, IsRequired = true)]
        public ENUM_COMMON_ERROR_CODE m_enumResult = ENUM_COMMON_ERROR_CODE.Fail;   // 處理結果
        [ProtoMember(2, IsRequired = true)]
        public ENUM_BLACKJACK_PLAYERDO m_enumPlayerDoResult = ENUM_BLACKJACK_PLAYERDO.Idle;   // 處理的事件
        [ProtoMember(3, IsRequired = true)]
        public uint m_uiBetPlayerDBID = 0;  // 押注者DBID
        [ProtoMember(4, IsRequired = true)]
        public byte m_bySeatID = 0;         // 押注座位
        [ProtoMember(5, IsRequired = true)]
        public byte m_byMainCard = 0; // 動作者做動作後更新的主要牌堆
        [ProtoMember(6, IsRequired = true)]
        public byte m_byOtherCard = 0; // 動作者如要求分牌 才會有此值 分排排堆的第二張牌
    }
    //--------------------------------------------------------------------------

    // 21點 通知發一般牌
    [ProtoContract]
    public class CPACK_BlackJack_NotifyDealData
    {
        [ProtoMember(1, IsRequired = true)]
        public byte m_byBankerFirstCard = 0; // 莊家的明牌    (數值參閱 ENUM_BACCARAT_CARD_ID)
        [ProtoMember(2, IsRequired = true)]
        public Dictionary<byte, CPACK_BlackJack_PlayerCardData>  m_dicPlayerCard = null; // new <座位ID,byte[2]>;//閒家頭兩張牌  0/第一張  1/第二張 (數值參閱 ENUM_BACCARAT_CARD_ID)
        
    }
    //--------------------------------------------------------------------------

    [ProtoContract]
    public class CPACK_BlackJack_PlayerCardData
    {
        [ProtoMember(1, IsRequired = true)]
        public byte m_bySeatID = 0; // 座位ID
        [ProtoMember(2, IsRequired = true)]
        public uint m_uiDBID = 0; // DBID
        [ProtoMember(3, IsRequired = true)]
        public byte[] m_byarCards = null; // 玩家卡片

    }
    //--------------------------------------------------------------------------
    //單一座位資料
    [ProtoContract]
    public class CNowCanDoTarget
    {
        [ProtoMember(1, IsRequired = true)]
        public uint m_uiPlayerDBID = 0;
        [ProtoMember(2, IsRequired = true)]
        public byte m_bySeatID = 0;
        [ProtoMember(3, IsRequired = true)]
        public byte m_byCardKind = 0;

    }





    // 21點 通知結算開獎結果
    [ProtoContract]
    public class CPACK_BlackJack_NotifyAward
    {
        [ProtoMember(1, IsRequired = true)]
        public CAllAward m_oBetAward = new CAllAward();
        [ProtoMember(2, IsRequired = true)]
        public UInt64 m_ui64Score = 0;      // 得分 
        [ProtoMember(3, IsRequired = true)]
        public UInt64 m_ui64GameMoney = 0;      // 遊戲幣的更新值
    }

    // 21點 通知結算開獎結果
    [ProtoContract]
    public class CPACK_BlackJack_PlayerRank
    {
        [ProtoMember(1, IsRequired = true)]
        public Dictionary<uint, CPACK_BlackJack_PlayerBetWin> m_dicPlayerWin = null; // 玩家DBID 與 玩家得贏金
    }

    // 21點 通知結算開獎結果
    [ProtoContract]
    public class CPACK_BlackJack_PlayerBetWin
    {
        [ProtoMember(1, IsRequired = true)]
        public UInt64 m_ui64AllBetMoney = 0;      // 總押金 
        [ProtoMember(2, IsRequired = true)]
        public UInt64 m_ui64GameMoney = 0;      // 總贏金
        [ProtoMember(3, IsRequired = true)]
        public string m_stPlayerName = "";      // 玩家名稱
    }



    #endregion
    //----------------------------------------------------------------------------------------------------------
    // GS <-> DC



    //----------------------------------------------------------------------------------------------------------
}
