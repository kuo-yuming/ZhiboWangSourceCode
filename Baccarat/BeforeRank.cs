using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using GameCore;
using GameCore.Machine;

public class BeforeRank : MonoBehaviour {
    public static Dictionary<ushort, CRaceRanking> BeforeRankData = new Dictionary<ushort, CRaceRanking>();
    private Dictionary<ushort, GameObject> ListObject = new Dictionary<ushort, GameObject>();
    public UIScrollBar BeforeRankBar;
    public UITable BeforeRankTable;
    public GameObject BeforeRank_Object;
    public static bool BeforeRankDataCheck = false;
    public UILabel[] NameLabel;
    
    float DelayTime = 0;
    bool DelayTimeBool = false;
    // Use this for initialization
    void Start () {
        BeforeRankData.Clear();
        BeforeRankDataCheck = false;
       
    }
	
	// Update is called once per frame
	void Update () {
        if (VersionDef.InternationalLanguageSystem)
        {
            NameLabel[0].text = Font_Control.Instance.m_dicMsgStr[2008027];
            NameLabel[1].text = Font_Control.Instance.m_dicMsgStr[2008028];
            NameLabel[2].text = Font_Control.Instance.m_dicMsgStr[2008029];
            NameLabel[3].text = Font_Control.Instance.m_dicMsgStr[2008030];
        }
        else
        {
            NameLabel[0].text = "名次";
            NameLabel[1].text = "暱稱";
            NameLabel[2].text = "得分";
            NameLabel[3].text = "獎勵";
        }

        if (BeforeRankData.Count < 8)
        {
            BeforeRankBar.barSize = 1;
            BeforeRankBar.value = 0;
        }

        if (BeforeRankDataCheck)
        {
            BeforeRankData_Void();
            BeforeRankDataCheck = false;
        }

        if (DelayTimeBool)
        {
            if (DelayTime < 0.4f)
            {
                DelayTime += Time.deltaTime;
                BeforeRankTable.repositionNow = true;
                BeforeRankBar.value = 0;
            }
            else
            {
                DelayTime = 0;
                DelayTimeBool = false;
            }
        }
    }

    void BeforeRankData_Void()
    {
        if (ListObject.Count != 0)
        {
            foreach (GameObject item in ListObject.Values)
            {
                Destroy(item);
            }
            ListObject.Clear();
        }

        ushort Number = 0;
        foreach (var item in BeforeRankData)
        {
            GameObject Data = Instantiate(BeforeRank_Object);
            Data.transform.parent = BeforeRankTable.transform;
            Data.transform.localScale = new Vector3(1, 1, 1);
            if (item.Value.m_uiRank < 10)
            {
                Data.name = "0" + item.Value.m_uiRank.ToString();
            }
            else
            {
                Data.name = item.Value.m_uiRank.ToString();
            }
            BRObject m_BRObject = Data.GetComponent<BRObject>();
            m_BRObject.Rank.text = item.Value.m_uiRank.ToString();
            m_BRObject.Name.text = item.Value.m_strNickName.ToString();
            m_BRObject.WinPoint.text = item.Value.m_i64Score.ToString();
            m_BRObject.Money.text = item.Value.m_uiAward_Money.ToString();
            m_BRObject.Diamond.text = item.Value.m_uiAward_Diamond.ToString();
            ListObject.Add(Number, Data);
            Number++;
        }
        DelayTimeBool = true;
    }
}
