using UnityEngine;
using System.Collections;
using GameCore.Manager.Common;
using System.Collections.Generic;

public static class Function_cs
{

    public static CTransactionSet GetTransactionSetWithLv(ushort usLv)
    {
        CTransactionSet oDefaultUnit = new CTransactionSet();
        oDefaultUnit.m_usFunctionLv = 0;

        if (Bank_Manager.m_CPACK_TransactionSysConfig.m_dicTransactionSet.Count == 0)
            return oDefaultUnit;

        List<ushort> listLv = new List<ushort>(Bank_Manager.m_CPACK_TransactionSysConfig.m_dicTransactionSet.Keys);
        listLv.Sort();  // 由小到大排序

        // 起碼要有一筆資料
        if (listLv.Count < 1)
            return oDefaultUnit;
        // 指定等級若比最小的開放等級小,就當做不開放交易功能
        if (usLv < listLv[0])
            return oDefaultUnit;

        ushort usSelectLv = 0;
        // 檢查需要找哪個等級區間
        foreach (ushort usFunctionLv in listLv)
        {
            if (usLv < usFunctionLv)
                break;

            usSelectLv = usFunctionLv;
        }

        CTransactionSet oTSetUnit = null;
        if (!Bank_Manager.m_CPACK_TransactionSysConfig.m_dicTransactionSet.TryGetValue(usSelectLv, out oTSetUnit))
            return oDefaultUnit;
        if (oTSetUnit == null)
            return oDefaultUnit;

        return oTSetUnit;
    }
}
