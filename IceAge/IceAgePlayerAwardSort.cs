﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore.Machine;

public class IceAgePlayerAwardSort : MonoBehaviour
{
    public static Dictionary<int, CPACK_PMachineAwardRecord> M_AwardShow = new Dictionary<int, CPACK_PMachineAwardRecord>();
    public static Dictionary<int, CPACK_PMachineAwardRecord> O_AwardShow = new Dictionary<int, CPACK_PMachineAwardRecord>();
    public M_SortingStatus MSorting = M_SortingStatus.Idle;
    public O_SortingStatus OSorting = O_SortingStatus.Idle;

    public enum M_SortingStatus
    {
        Idle = 0,
        TimeFirst = 1,
        ReTimeFirst = 2,
        AwardFirst = 3,
        ReAwardFirst = 4,
        MachineIDFirst = 5,
        ReMachineIDFirst = 6,
        MoneyFirst = 7,
        ReMoneyFirst = 8,



    };
    public enum O_SortingStatus
    {
        Idle = 0,
        TimeFirst = 1,
        ReTimeFirst = 2,
        AwardFirst = 3,
        ReAwardFirst = 4,
        MachineIDFirst = 5,
        ReMachineIDFirst = 6,
        MoneyFirst = 7,
        ReMoneyFirst = 8,
        NameFirst = 9,
        ReNameFirst = 10,



    };
    // Use this for initialization
    void Start()
    {
        M_AwardShow.Clear();
        O_AwardShow.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (IceAgeManager.M_AwardPacket.m_bEnd)
        {
            MSorting = M_SortingStatus.Idle;
            M_TimeFirst();

            IceAgeManager.M_AwardPacket.m_bEnd = false;
        }
        if (IceAgeManager.O_AwardPacket.m_bEnd)
        {
            OSorting = O_SortingStatus.Idle;
            O_TimeFirst();
            //OSorting = O_SortingStatus.TimeFirst;
            IceAgeManager.O_AwardPacket.m_bEnd = false;
        }

    }
    //個人大獎排序
    public void M_TimeFirst()
    {
        M_AwardShow.Clear();
        List<CPACK_PMachineAwardRecord> Loca_Data = new List<CPACK_PMachineAwardRecord>(IceAgeManager.M_AwardRecord);

        CPACK_PMachineAwardRecord Temp = new CPACK_PMachineAwardRecord();

        for (int i = 0; i <= (Loca_Data.Count) - 1; i++)
        {

            for (int j = i + 1; j < Loca_Data.Count; j++)
            {

                if (Loca_Data[j].m_ui64Time > Loca_Data[i].m_ui64Time)
                {
                    Temp = Loca_Data[i];
                    Loca_Data[i] = Loca_Data[j];
                    Loca_Data[j] = Temp;

                }

            }

        }

        if (MSorting == M_SortingStatus.TimeFirst)
        {
            Loca_Data.Reverse();
            for (int i = 0; i < Loca_Data.Count; i++)
            {
                M_AwardShow.Add(i, Loca_Data[i]);

            }
            MSorting = M_SortingStatus.ReTimeFirst;
        }
        else
        {
            for (int i = 0; i < Loca_Data.Count; i++)
            {
                M_AwardShow.Add(i, Loca_Data[i]);

            }
            MSorting = M_SortingStatus.TimeFirst;
        }

    }

    public void M_AwardFirst()
    {
        M_AwardShow.Clear();
        List<CPACK_PMachineAwardRecord> Loca_Data = new List<CPACK_PMachineAwardRecord>(IceAgeManager.M_AwardRecord);
        List<CPACK_PMachineAwardRecord> Loca_Data2 = new List<CPACK_PMachineAwardRecord>();
        int Index = 0;
        int Long_all = Loca_Data.Count;

        Loca_Data.Sort(delegate (CPACK_PMachineAwardRecord x, CPACK_PMachineAwardRecord y) {
            if (x.m_byAllWinAwardID == null && y.m_byAllWinAwardID == null)
                return 0;
            else if (x.m_byAllWinAwardID == null)
                return -1;
            else if (y.m_byAllWinAwardID == null)
                return 1;
            else
                return x.m_byAllWinAwardID.CompareTo(y.m_byAllWinAwardID);
        });
        Loca_Data.Reverse();
        foreach (CPACK_PMachineAwardRecord item in Loca_Data)
        {
            if (item.m_byAllWinAwardID == 37)
                Loca_Data2.Add(item);
        }
        foreach (CPACK_PMachineAwardRecord item in Loca_Data)
        {
            if (item.m_byAllWinAwardID == 37)
                continue;
            else
                Loca_Data2.Add(item);
        }
        M_AwardShow.Clear();

        if (MSorting == M_SortingStatus.AwardFirst)
        {
            Loca_Data2.Reverse();
            for (int i = 0; i < Loca_Data2.Count; i++)
            {
                M_AwardShow.Add(i, Loca_Data2[i]);

            }
            MSorting = M_SortingStatus.ReAwardFirst;
        }
        else
        {
            for (int i = 0; i < Loca_Data2.Count; i++)
            {
                M_AwardShow.Add(i, Loca_Data2[i]);

            }
            MSorting = M_SortingStatus.AwardFirst;
        }

    }

    public void M_MachineIDFirst()
    {
        M_AwardShow.Clear();
        List<CPACK_PMachineAwardRecord> Loca_Data = new List<CPACK_PMachineAwardRecord>(IceAgeManager.M_AwardRecord);
        CPACK_PMachineAwardRecord Temp = new CPACK_PMachineAwardRecord();

        for (int i = 0; i <= (Loca_Data.Count) - 1; i++)
        {

            for (int j = i + 1; j < Loca_Data.Count; j++)
            {
                if (Loca_Data[j].m_uiMID > Loca_Data[i].m_uiMID)
                {
                    Temp = Loca_Data[i];
                    Loca_Data[i] = Loca_Data[j];
                    Loca_Data[j] = Temp;

                }

            }

        }
        if (MSorting == M_SortingStatus.MachineIDFirst)
        {
            Loca_Data.Reverse();
            for (int i = 0; i < Loca_Data.Count; i++)
            {
                M_AwardShow.Add(i, Loca_Data[i]);

            }
            MSorting = M_SortingStatus.ReMachineIDFirst;
        }
        else
        {
            for (int i = 0; i < Loca_Data.Count; i++)
            {
                M_AwardShow.Add(i, Loca_Data[i]);

            }
            MSorting = M_SortingStatus.MachineIDFirst;
        }
    }

    public void M_MoneyFirst()
    {
        M_AwardShow.Clear();
        List<CPACK_PMachineAwardRecord> Loca_Data = new List<CPACK_PMachineAwardRecord>(IceAgeManager.M_AwardRecord);
        CPACK_PMachineAwardRecord Temp = new CPACK_PMachineAwardRecord();

        for (int i = 0; i <= (Loca_Data.Count) - 1; i++)
        {

            for (int j = i + 1; j < Loca_Data.Count; j++)
            {
                if (Loca_Data[j].m_uiMoney > Loca_Data[i].m_uiMoney)
                {
                    Temp = Loca_Data[i];
                    Loca_Data[i] = Loca_Data[j];
                    Loca_Data[j] = Temp;

                }

            }

        }
        if (MSorting == M_SortingStatus.MoneyFirst)
        {
            Loca_Data.Reverse();
            for (int i = 0; i < Loca_Data.Count; i++)
            {
                M_AwardShow.Add(i, Loca_Data[i]);

            }
            MSorting = M_SortingStatus.ReMoneyFirst;
        }
        else
        {
            for (int i = 0; i < Loca_Data.Count; i++)
            {
                M_AwardShow.Add(i, Loca_Data[i]);

            }
            MSorting = M_SortingStatus.MoneyFirst;
        }
    }

    //----------------------------------------------------------------------------------------------------------------------------------
    //本廳大獎排序
    public void O_TimeFirst()
    {
        O_AwardShow.Clear();
        List<CPACK_PMachineAwardRecord> Loca_Data = new List<CPACK_PMachineAwardRecord>(IceAgeManager.O_AwardRecord);


        CPACK_PMachineAwardRecord Temp = new CPACK_PMachineAwardRecord();

        for (int i = 0; i <= (Loca_Data.Count) - 1; i++)
        {

            for (int j = i + 1; j < Loca_Data.Count; j++)
            {

                if (Loca_Data[j].m_ui64Time > Loca_Data[i].m_ui64Time)
                {
                    Temp = Loca_Data[i];
                    Loca_Data[i] = Loca_Data[j];
                    Loca_Data[j] = Temp;

                }

            }

        }
        if (OSorting == O_SortingStatus.TimeFirst)
        {
            Loca_Data.Reverse();
            for (int i = 0; i < Loca_Data.Count; i++)
            {
                O_AwardShow.Add(i, Loca_Data[i]);

            }
            OSorting = O_SortingStatus.ReTimeFirst;
        }
        else
        {
            for (int i = 0; i < Loca_Data.Count; i++)
            {
                O_AwardShow.Add(i, Loca_Data[i]);

            }
            OSorting = O_SortingStatus.TimeFirst;
        }
    }

    public void O_NameFirst()
    {
        O_AwardShow.Clear();
        List<CPACK_PMachineAwardRecord> Loca_Data = new List<CPACK_PMachineAwardRecord>(IceAgeManager.O_AwardRecord);
        List<CPACK_PMachineAwardRecord> Final_Data = new List<CPACK_PMachineAwardRecord>();
        //	string Temp = new string;
        Dictionary<string, uint> NameDictionary = new Dictionary<string, uint>();
        Dictionary<uint, List<int>> DataIndex = new Dictionary<uint, List<int>>();
        for (int i = 0; i < Loca_Data.Count; i++)
        {
            if (!NameDictionary.ContainsKey(Loca_Data[i].m_strPlayerNickName))
            {
                NameDictionary.Add(Loca_Data[i].m_strPlayerNickName, Loca_Data[i].m_uiPlayerDBID);
                List<int> Data = new List<int>();
                Data.Add(i);
                DataIndex.Add(Loca_Data[i].m_uiPlayerDBID, Data);
            }
            else
            {
                if (DataIndex.ContainsKey(Loca_Data[i].m_uiPlayerDBID))
                {
                    DataIndex[Loca_Data[i].m_uiPlayerDBID].Add(i);
                }

            }
        }

        /*
                List<string> NameList = new List<string> ();
                foreach (string DataInfo in NameDictionary.Keys) {
                    NameList.Add(DataInfo);
                        }
                for (int i = 0; i <=(NameList.Count)-1; i++) {
                    for (int j = i+1; j < NameList.Count; j++) {

                        if ((string.Compare (NameList[j], NameList[i])) > 0) {
                            string Temp = NameList [i];
                            NameList [i] = NameList [j];
                            NameList [j] = Temp;

                        }

                    }
                }
        */
        List<string> NameList = new List<string>(NameDictionary.Keys);
        if (OSorting == O_SortingStatus.NameFirst)
        {
            NameList.Sort();
        }
        else
        {
            NameList.Sort();
            //NameList.Reverse ();
        }

        for (int i = 0; i < NameList.Count; i++)
        {
            for (int j = 0; j < DataIndex[NameDictionary[NameList[i]]].Count; j++)
            {
                Final_Data.Add(Loca_Data[DataIndex[NameDictionary[NameList[i]]][j]]);
            }
        }













        if (OSorting == O_SortingStatus.NameFirst)
        {

            for (int i = 0; i < Final_Data.Count; i++)
            {
                O_AwardShow.Add(i, Final_Data[i]);

            }
            OSorting = O_SortingStatus.ReNameFirst;
        }
        else
        {
            Final_Data.Reverse();
            for (int i = 0; i < Final_Data.Count; i++)
            {
                O_AwardShow.Add(i, Final_Data[i]);

            }
            OSorting = O_SortingStatus.NameFirst;
        }
    }

    public void O_AwardFirst()
    {
        O_AwardShow.Clear();
        List<CPACK_PMachineAwardRecord> Loca_Data = new List<CPACK_PMachineAwardRecord>(IceAgeManager.O_AwardRecord);
        List<CPACK_PMachineAwardRecord> Loca_Data2 = new List<CPACK_PMachineAwardRecord>();
        int Index = 0;
        int Long_all = Loca_Data.Count;
        Loca_Data.Sort(delegate (CPACK_PMachineAwardRecord x, CPACK_PMachineAwardRecord y) {
            if (x.m_byAllWinAwardID == null && y.m_byAllWinAwardID == null)
                return 0;
            else if (x.m_byAllWinAwardID == null)
                return -1;
            else if (y.m_byAllWinAwardID == null)
                return 1;
            else
                return x.m_byAllWinAwardID.CompareTo(y.m_byAllWinAwardID);
        });
        Loca_Data.Reverse();
        foreach (CPACK_PMachineAwardRecord item in Loca_Data)
        {
            if (item.m_byAllWinAwardID == 37)
                Loca_Data2.Add(item);
        }
        foreach (CPACK_PMachineAwardRecord item in Loca_Data)
        {
            if (item.m_byAllWinAwardID == 37)
                continue;
            else
                Loca_Data2.Add(item);
        }
        
        O_AwardShow.Clear();
        if (OSorting == O_SortingStatus.AwardFirst)
        {
            Loca_Data2.Reverse();
            for (int i = 0; i < Loca_Data2.Count; i++)
            {
                O_AwardShow.Add(i, Loca_Data2[i]);

            }
            OSorting = O_SortingStatus.ReAwardFirst;
        }
        else
        {
            for (int i = 0; i < Loca_Data2.Count; i++)
            {
                O_AwardShow.Add(i, Loca_Data2[i]);

            }
            OSorting = O_SortingStatus.AwardFirst;
        }
    }

    public void O_MachineIDFirst()
    {
        O_AwardShow.Clear();
        List<CPACK_PMachineAwardRecord> Loca_Data = new List<CPACK_PMachineAwardRecord>(IceAgeManager.O_AwardRecord);
        CPACK_PMachineAwardRecord Temp = new CPACK_PMachineAwardRecord();

        for (int i = 0; i <= (Loca_Data.Count) - 1; i++)
        {

            for (int j = i + 1; j < Loca_Data.Count; j++)
            {
                if (Loca_Data[j].m_uiMID > Loca_Data[i].m_uiMID)
                {
                    Temp = Loca_Data[i];
                    Loca_Data[i] = Loca_Data[j];
                    Loca_Data[j] = Temp;

                }

            }

        }
        if (OSorting == O_SortingStatus.MachineIDFirst)
        {
            Loca_Data.Reverse();
            for (int i = 0; i < Loca_Data.Count; i++)
            {
                O_AwardShow.Add(i, Loca_Data[i]);

            }
            OSorting = O_SortingStatus.ReMachineIDFirst;
        }
        else
        {
            for (int i = 0; i < Loca_Data.Count; i++)
            {
                O_AwardShow.Add(i, Loca_Data[i]);

            }
            OSorting = O_SortingStatus.MachineIDFirst;
        }
    }

    public void O_MoneyFirst()
    {
        O_AwardShow.Clear();
        List<CPACK_PMachineAwardRecord> Loca_Data = IceAgeManager.O_AwardRecord;
        CPACK_PMachineAwardRecord Temp = new CPACK_PMachineAwardRecord();

        for (int i = 0; i <= (Loca_Data.Count) - 1; i++)
        {

            for (int j = i + 1; j < Loca_Data.Count; j++)
            {
                if (Loca_Data[j].m_uiMoney > Loca_Data[i].m_uiMoney)
                {
                    Temp = Loca_Data[i];
                    Loca_Data[i] = Loca_Data[j];
                    Loca_Data[j] = Temp;

                }

            }

        }
        if (OSorting == O_SortingStatus.MoneyFirst)
        {
            Loca_Data.Reverse();
            for (int i = 0; i < Loca_Data.Count; i++)
            {
                O_AwardShow.Add(i, Loca_Data[i]);

            }
            OSorting = O_SortingStatus.ReMoneyFirst;
        }
        else
        {
            for (int i = 0; i < Loca_Data.Count; i++)
            {
                O_AwardShow.Add(i, Loca_Data[i]);

            }
            OSorting = O_SortingStatus.MoneyFirst;
        }
    }

    //------------------------------------------------------------------------------------
}
