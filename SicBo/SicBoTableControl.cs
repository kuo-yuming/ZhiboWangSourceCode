using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore.Machine;
using GameCore;

public class SicBoTableControl : MonoBehaviour
{    
    public bool CheckMachineClass = false;
    public bool ChangeMachineClass = false;
    public ushort SortType = 0;
    public GameObject SortBtn_TableID;
    public GameObject SortBtn_MemberID;
    public uint NowPage = 0;
    public uint MaxPage = 0;

    // Use this for initialization
    void Start()
    {
        NowPage = 0;
        SortType = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (SicBoManager.CheckMachineClass && SicBoManager.m_MachineDatas.Count != 0 && SicBoManager.m_MachineBuyInConfig != null)
        {
            SicBoManager.m_MachineClass.Clear();
            foreach (var item in SicBoManager.m_MachineBuyInConfig.m_dicTableGroupSet)
            {
                List<uint> Data = new List<uint>();
                for (uint i = item.Value.m_uiStartTableID; i <= item.Value.m_uiEndTableID; i++)
                {
                    if (SicBoManager.m_MachineDatas.ContainsKey(i))
                        Data.Add(i);
                }
                Data.Sort();
                SicBoManager.m_MachineClass.Add(item.Key, Data);
            }
            SicBoManager.CheckMachineClass = false;
            ChangeMachineClass = true;
        }

        if (ChangeMachineClass)
        {
            ChangeMachineClass = false;
            if (!SicBoManager.m_MachineBuyInConfig.m_dicTableGroupSet.ContainsKey(SicBoManager.NowGroup))
                return; //如果收到的資料內找不到分區 return
            SicBoManager.MachineList.Clear();
            SicBoManager.MachineList = new List<uint>(SicBoManager.m_MachineClass[SicBoManager.NowGroup]);
            SortMachine();
            NowPage = 0;
            if (SicBoManager.MachineList.Count % 2 != 0)
                MaxPage = (uint)(SicBoManager.MachineList.Count / 2);
            else
            {
                if (SicBoManager.MachineList.Count / 2 != 0)
                    MaxPage = (uint)(SicBoManager.MachineList.Count / 2 - 1);
                else
                    MaxPage = 0;
            }
        }
    }

    void SortMachine()
    {
        if (SortType == 0)
        {
            SicBoManager.MachineList.Sort();
            SortBtn_TableID.SetActive(false);
            SortBtn_MemberID.SetActive(true);
        }
        else if (SortType == 1)
        {
            SicBoManager.MachineList.Sort(delegate (uint x, uint y)
            {
                int Data = SicBoManager.m_MachineDatas[x].m_usMemberCnt.CompareTo(SicBoManager.m_MachineDatas[y].m_usMemberCnt);
                if (Data != 0)
                    return Data;
                else
                {
                    int NextData = x.CompareTo(y);
                    if (NextData == 0)
                        return NextData;
                    else
                    {
                        return NextData * -1;
                    }
                }
            });
            SicBoManager.MachineList.Reverse();
            SortBtn_TableID.SetActive(true);
            SortBtn_MemberID.SetActive(false);
        }
    }

    void NextPage()
    {
        if (NowPage == MaxPage)
            NowPage = 0;
        else
            NowPage++;
    }
    void BackPage()
    {
        if (NowPage == 0)
            NowPage = MaxPage;
        else
            NowPage--;
    }

    void MemberSort()
    {
        SortType = 1;
        ChangeMachineClass = true;

    }

    void TableIDSort()
    {
        SortType = 0;
        ChangeMachineClass = true;
    }

}
