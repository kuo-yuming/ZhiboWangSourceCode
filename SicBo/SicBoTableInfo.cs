using UnityEngine;
using System.Collections;

public class SicBoTableInfo : MonoBehaviour
{
    public bool ChangeInfo = false;
    public UILabel TableID;
    public UILabel MemberCnt;
    public UILabel BetMax;
    public UILabel HighPoint;
    public UILabel LowPoint;
    public UILabel AnyTriple;
    public UILabel One;
    public UILabel Two;
    public UILabel Three;
    public UILabel Four;
    public UILabel Five;
    public UILabel Six;
    public UILabel NoAnyTriple;
    public UILabel NoAnyQuadruple;

    // Use this for initialization
    void Start()
    {
        ChangeInfo = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (ChangeInfo)
        {
            ChangeInfo = false;
            TableID.text = SicBoManager.MachineInfo.m_uiTID + "";
            MemberCnt.text = SicBoManager.m_MachineDatas[SicBoManager.MachineInfo.m_uiTID].m_usMemberCnt + "";
            BetMax.text = SicBoManager.m_MachineBuyInConfig.m_dicTableGroupSet[SicBoManager.m_MachineTableArea[SicBoManager.MachineInfo.m_uiTID]].m_uiMinBetMoney + " ~ " + SicBoManager.m_MachineBuyInConfig.m_dicTableGroupSet[SicBoManager.m_MachineTableArea[SicBoManager.MachineInfo.m_uiTID]].m_uiMaxBetMoney;
            HighPoint.text = SicBoManager.MachineInfo.m_byBigNum + "";
            LowPoint.text = SicBoManager.MachineInfo.m_bySmallNum + "";
            AnyTriple.text = SicBoManager.MachineInfo.m_byThreeSameNum + "";
            One.text = SicBoManager.MachineInfo.m_byOne + "";
            Two.text = SicBoManager.MachineInfo.m_byTwo + "";
            Three.text = SicBoManager.MachineInfo.m_byThree + "";
            Four.text = SicBoManager.MachineInfo.m_byFour + "";
            Five.text = SicBoManager.MachineInfo.m_byFive + "";
            Six.text = SicBoManager.MachineInfo.m_bySix + "";
            NoAnyTriple.text = SicBoManager.MachineInfo.m_usNotSameNum + "";
            NoAnyQuadruple.text = SicBoManager.MachineInfo.m_usNoQuadrupleRound + "";
        }

    }
}
