using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BankEnum;
using GameCore;
using GameCore.Manager.Common;
public class Business_Control : MonoBehaviour {
    public static byte BusinessFee = 0;//手續費
    public static uint BusinessMaxExportMoney = 0;//最大匯出金額
    public static uint BusinessKeepMoney = 0;//匯出餘額限制

    public static bool PassEnactment = false;//有無密碼設定
    public static bool PassEnter = false;//密碼是否輸入
    public static bool BusinessStart = false;

    public static bool FirstNewKey = false;//第一次密碼輸入
    public static bool FirstNewAgainKey = false;//第一次密碼再次輸入
    public static bool BusinessDataInit = false;//交易初始化
    public static bool BusinessWaitTime = false;//等待玩家交易
    public GameObject WaitTimeObject;
    public UILabel WaitTimeLabel;
    int MaxWaitTime = 30;
    float RanWaitTime = 0;
    int TotalWaitTime = 0;
    //密碼輸入
    public GameObject[] BusinessObject;
    public UIInput FirstNewKeyInput;
    public UILabel FirstNewKeyLabel;
    public static string FirstNewKeyString = "";
    public UIInput FirstNewAgainKeyInput;
    public UILabel FirstNewAgainKeyLabel;
    public static string FirstNewAgainKeyString = "";
    public UIInput PassKeyInput;
    public UILabel PassKeyLabel;
    public static string PassKeyString = "";
    public UILabel[] KeyLabel = new UILabel[4];

    //玩家列表
    public UIInput SelectInput;
    public GameObject PlayerDataObject;
    public UITable PlayerDataTable;
    public UIScrollBar BusinessListScrollBar;
    public UISprite[] ListButtonSrpite = new UISprite[2];
    public static Dictionary<uint, GameObject> PlayerListDictionary = new Dictionary<uint, GameObject>();
    public static Dictionary<uint, GameObject> FriendListDictionary = new Dictionary<uint, GameObject>();
    public static bool GetPlayerListData = false;
    public static bool PlayerListBool = false;
    public static bool FriendListBool = true;
    public static bool ListCheckBool = false;
    public BoxCollider SeachBarBox;
    public UIScrollView BarScrollView;
    public GameObject TableObject;

    //BAR
    float DelayTimer = 0.0f;
    bool DelayTimerBool = false;
    bool BarBoolStart = false;
    string SaveBarString = "";
    string BarString = "";
	// Use this for initialization
    void Start()
    {
        WaitTimeObject.SetActive(false);
        PassEnactment = false;//有無密碼設定
        PassEnter = false;//密碼是否輸入
        BusinessStart = false;
        FirstNewKey = false;//第一次密碼輸入
        FirstNewAgainKey = false;//第一次密碼再次輸入
        BusinessDataInit = false;//交易初始化
        BusinessWaitTime = false;//等待玩家交易
        FirstNewKeyString = "";
        FirstNewAgainKeyString = "";
        PassKeyString = "";
        GetPlayerListData = false;
        PlayerListBool = false;
        FriendListBool = true;
        ListCheckBool = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (LobbyTitle_Control.m_TitleStatus == LobbyTitle_Control.TitleType.Bank)
        {
            TableObject.SetActive(true);
        }
        else
        {
            TableObject.SetActive(false);
        }

        //玩家搜索BAR
        NameSelect();
        WaitTime();
        BARSelect();

        //密碼輸入
        if (!PassEnactment)
        {
          //  FirstNewKeyLabel.text = FirstNewKeyInput.value;
            FirstNewKeyString = FirstNewKeyInput.value;
           // FirstNewAgainKeyLabel.text = FirstNewAgainKeyInput.value;
            FirstNewAgainKeyString = FirstNewAgainKeyInput.value;
            if (FirstNewKeyInput.value != "")
            {
                KeyLabel[0].enabled = false;
            }
            else
            {
                KeyLabel[0].enabled = true;
            }

            if (FirstNewAgainKeyInput.value != "")
            {
                KeyLabel[1].enabled = false;
            }
            else
            {
                KeyLabel[1].enabled = true;
            }
        }
        else
        {
          //  PassKeyLabel.text = PassKeyInput.value;
            PassKeyString = PassKeyInput.value;
            if (PassKeyInput.value != "")
            {
                KeyLabel[2].enabled = false;
            }
            else
            {
                KeyLabel[2].enabled = true;
            }
        }

        //玩家列表
        if (ListCheckBool)
        {
            if (FriendListBool)
            {
                FriendList();
            }
            else if (PlayerListBool)
            {
                PlayerList();   
            }
           DelayTimerBool = true;
           ListCheckBool = false;
        }

        if (FriendListBool)
        {
            ListButtonSrpite[0].spriteName = "switch_friendList_1";
            ListButtonSrpite[1].spriteName = "switch_playerList_0";
        }
        else if (PlayerListBool)
        {
            ListButtonSrpite[0].spriteName = "switch_friendList_0";
            ListButtonSrpite[1].spriteName = "switch_playerList_1";
        }

        //密碼頁面是否開啟
        if (!PassEnter)
        {
            if (!PassEnactment)
            {
                BusinessObject[0].SetActive(true);
                BusinessObject[1].SetActive(false);
            }
            else
            {
                BusinessObject[1].SetActive(true);
                BusinessObject[0].SetActive(false);
            }
            SeachBarBox.enabled = false;
        }
        else
        {
            SeachBarBox.enabled = true;
            BusinessObject[0].SetActive(false);
            BusinessObject[1].SetActive(false);
        }

        if (BusinessStart)
        {
            BusinessObject[2].SetActive(true);
        }
        else
        {
            BusinessObject[2].SetActive(false);
        }

        //交易資料Init
        if (BusinessDataInit)
        {
            DataInit();
        }
	}

    //玩家名稱搜索
    void NameSelect()
    {
        if (SelectInput.value != "")
        {
            KeyLabel[3].enabled = false;
            if (BarString.Length != SelectInput.value.Length && !DelayTimerBool)
            {
                DelayTimerBool = true;
            }
            BarString = SelectInput.value;
            foreach (var item in FriendListDictionary.Values)
            {
                PlayerData Data_cs = item.GetComponent<PlayerData>();
                if (Data_cs.PlayerName.text.IndexOf(SelectInput.value) < 0)
                {
                    item.SetActive(false);
                }
                else
                {
                    item.SetActive(true);
                }
                Data_cs.ColorCheck = true;
                Data_cs.SelectCheck = true;
              
            }
            foreach (var item in PlayerListDictionary.Values)
            {
                PlayerData Data_cs = item.GetComponent<PlayerData>();
                if (Data_cs.PlayerName.text.IndexOf(SelectInput.value) < 0)
                {
                    item.SetActive(false);
                }
                else
                {
                    item.SetActive(true);
                }
                Data_cs.ColorCheck = true;
                Data_cs.SelectCheck = true;
               
            }
            if (BarString != SaveBarString)
            {
                SaveBarString = BarString;
                BarBoolStart = true;
            }
        }
        else
        {
            KeyLabel[3].enabled = true;
            if (BarString.Length != SelectInput.value.Length && !DelayTimerBool)
            {
                DelayTimerBool = true;
            }
            foreach (var item in FriendListDictionary.Values)
            {
                PlayerData Data_cs = item.GetComponent<PlayerData>();
                item.SetActive(true);
                Data_cs.ColorCheck = true;
                Data_cs.SelectCheck = false;
            }
            foreach (var item in PlayerListDictionary.Values)
            {
                PlayerData Data_cs = item.GetComponent<PlayerData>();
                item.SetActive(true);
                Data_cs.ColorCheck = true;
                Data_cs.SelectCheck = false;
            }
            BarBoolStart = false;
            BarString = "";
            SaveBarString = "";
        }
       
        //   PlayerDataTable.repositionNow = true;
    }
    //BAR處理
    void BARSelect()
    {
        //Bar初始化 
        if (DelayTimerBool)
        {
            if (DelayTimer < 0.5f)
            {
                DelayTimer += Time.deltaTime;
                BusinessListScrollBar.value = 0;
                PlayerDataTable.repositionNow = true;
                BarScrollView.UpdatePosition();
                BarScrollView.UpdateScrollbars();
            }
            else
            {
                DelayTimer = 0;
                DelayTimerBool = false;
            }
        }

        if (BarBoolStart)
        {
            BusinessListScrollBar.value = 0;
            BarBoolStart = false;
        }
    }
    //玩家列表資料
    void PlayerList()
    {
        foreach (GameObject item in FriendListDictionary.Values)
        {
            Destroy(item);
        }
        FriendListDictionary.Clear();
        BusinessListScrollBar.barSize = 1;
        if (PlayerListCheck.GetAllPlayerList.Count != 0)
        {
            List<ulong> CkeckList = new List<ulong>();
            foreach (var item in PlayerListCheck.GetAllPlayerList)
            {
                if (SNS_Manager.Public_GroupsMemberData.ContainsKey(item.Key))
                {
                    if (item.Key != MainConnet.m_PlayerData.m_uiDBID)
                    {
                        CkeckList.Add(item.Key);
                    }
                }
            }
            int Number = 0;
            foreach (uint item in CkeckList)
            {
                GameObject Data = Instantiate(PlayerDataObject);
                Data.transform.parent = PlayerDataTable.transform;
                Data.transform.localScale = new Vector3(1, 1, 1);
                PlayerData Data_cs1 = Data.GetComponent<PlayerData>();
                BankPlayerData Data_cs = Data_cs1.Button.GetComponent<BankPlayerData>();
                Data_cs1.ColorCheck = true;
                Data.name = (1000 + Number).ToString();
                Data_cs.PlayerName = PlayerListCheck.GetAllPlayerList[item].m_strName;
                Data_cs.PlayerDBID = PlayerListCheck.GetAllPlayerList[item].m_uiDBID;
                Data_cs1.Number = (byte)(Number % 2);
                Data_cs1.PlayerName.text = PlayerListCheck.GetAllPlayerList[item].m_strName;
                Data_cs1.PlayerLevel.text = PlayerListCheck.GetAllPlayerList[item].m_usLv.ToString();
                Data_cs1.PlayerCash.text = PlayerListCheck.GetAllPlayerList[item].m_ui64OwnMoney.ToString();
                if (VersionDef.InternationalLanguageSystem)
                {
                    Data_cs1.ActiveHoshi.text = Font_Control.Instance.m_dicMsgStr[2504028] + Net.ActivityString(PlayerListCheck.GetAllPlayerList[item].m_byActivity);
                }
                else
                {
                    Data_cs1.ActiveHoshi.text = "活躍度：" + Net.ActivityString(PlayerListCheck.GetAllPlayerList[item].m_byActivity);
                }
                PlayerListDictionary.Add(PlayerListCheck.GetAllPlayerList[item].m_uiDBID, Data);
                Number++;
            }
            BusinessListScrollBar.value = 1; 
            //PlayerDataTable.repositionNow = true;
        }
    }
    //好友列表資料
    void FriendList()
    {
        foreach (GameObject item in PlayerListDictionary.Values)
        {
            Destroy(item);
        }
        PlayerListDictionary.Clear();
        BusinessListScrollBar.barSize = 1;
        if (PlayerListCheck.GetAllPlayerList.Count != 0)
        {
            List<ulong> CkeckList = new List<ulong>();
            foreach (var item in PlayerListCheck.GetAllPlayerList)
            {
                //if (SNS_Manager.Public_GroupsMemberData.ContainsKey(item.Key))
                //{
                //    if (item.Key != MainConnet.m_PlayerData.m_uiDBID)
                //    {
                //        CkeckList.Add(item.Key);
                //    }
                //}

                    if (SNS_Manager.m_dicFriends.ContainsKey(item.Key) && item.Key != MainConnet.m_PlayerData.m_uiDBID)
                    {
                        CkeckList.Add(item.Key);
                    }
            }
            int Number = 0;
            foreach (uint item in CkeckList)
            {
                GameObject Data = Instantiate(PlayerDataObject);
                Data.transform.parent = PlayerDataTable.transform;
                Data.transform.localScale = new Vector3(1, 1, 1);
                PlayerData Data_cs1 = Data.GetComponent<PlayerData>();
                BankPlayerData Data_cs = Data_cs1.Button.GetComponent<BankPlayerData>();
                Data_cs1.ColorCheck = true;
                Data.name = (1000 + Number).ToString();
                Data_cs.PlayerDBID = PlayerListCheck.GetAllPlayerList[item].m_uiDBID;
                Data_cs.PlayerName = PlayerListCheck.GetAllPlayerList[item].m_strName;
                Data_cs1.Number = (byte)(Number % 2);
                Data_cs1.PlayerName.text = PlayerListCheck.GetAllPlayerList[item].m_strName;
                Data_cs1.PlayerLevel.text = PlayerListCheck.GetAllPlayerList[item].m_usLv.ToString();
                Data_cs1.PlayerCash.text = PlayerListCheck.GetAllPlayerList[item].m_ui64OwnMoney.ToString();
                if (VersionDef.InternationalLanguageSystem)
                {
                    Data_cs1.ActiveHoshi.text = Font_Control.Instance.m_dicMsgStr[2504028] + Net.ActivityString(PlayerListCheck.GetAllPlayerList[item].m_byActivity);
                }
                else
                {
                    Data_cs1.ActiveHoshi.text = "活躍度：" + Net.ActivityString(PlayerListCheck.GetAllPlayerList[item].m_byActivity);
                }
                FriendListDictionary.Add(PlayerListCheck.GetAllPlayerList[item].m_uiDBID, Data);
                Number++;
            }
            BusinessListScrollBar.value = 1;
           // PlayerDataTable.repositionNow = true;
        }
    }
    void DataInit()
    {
        BusinessStart = false;
        FirstNewKey = false;
        FirstNewAgainKey = false;
        PassEnter = false;
        PlayerListBool = false;
        FriendListBool = true;
        FirstNewKeyLabel.text = "";
        FirstNewKeyInput.value = "";
        FirstNewKeyString = "";
        FirstNewAgainKeyLabel.text = "";
        FirstNewAgainKeyInput.value = "";
        FirstNewAgainKeyString = "";
        PassKeyInput.value = "";
        PassKeyLabel.text = "";
        PassKeyString = "";
        SelectInput.value = "";
        CashBusiness.BusinessPlayerName = "";
        CashBusiness.PlayerDBID = 0;
        foreach (GameObject item in FriendListDictionary.Values)
        {
            Destroy(item);
        }
        foreach (GameObject item in PlayerListDictionary.Values)
        {
            Destroy(item);
        }
        PlayerListDictionary.Clear();
        FriendListDictionary.Clear();

        BusinessDataInit = false;
    }
    //等待時間
    void WaitTime()
    {
        if (BusinessWaitTime)
        {
            WaitTimeObject.SetActive(true);
            WaitTimeLabel.text = TotalWaitTime.ToString();
        }
        else if (!BusinessWaitTime)
        {
            BusinessWaitTime = false;
            WaitTimeObject.SetActive(false);
            RanWaitTime = 0;
        }
    }
}
