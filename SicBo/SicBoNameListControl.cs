using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SicBoNameListControl : MonoBehaviour
{
    public bool ChangeNameList = false;
    public uint ClickTarget = 0;
    public GameObject M_Target;
    public UIGrid m_Grid;
    public UISprite m_BackGround;
    List<GameObject> m_ObjList = new List<GameObject>();
    public GameObject NameListPrefeb;
    public Transform[] m_NameListPos = new Transform[2];

    // Use this for initialization
    void Start()
    {
        ChangeNameList = false;
        ClickTarget = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (M_Target.activeSelf)
        {
            if (Input.GetMouseButtonUp(0))
            {
                M_Target.SetActive(false);
            }
        }
        if (ChangeNameList)
        {
            ChangeNameList = false;
            int Index = m_ObjList.Count;
            for (int i = 0; i < Index; i++)
                Destroy(m_ObjList[i]);
            m_ObjList.Clear();

            if (SicBoManager.MachineMemberList.m_listMember.Count == 0)
            {
                M_Target.SetActive(false);
                return;
            }
            else if (SicBoManager.MachineMemberList.m_listMember.Count <= 10)
            {
                m_Grid.maxPerLine = 1;
                m_BackGround.width = 170;
                m_BackGround.height = SicBoManager.MachineMemberList.m_listMember.Count * 28;
                m_BackGround.height += 40;
            }
            else if (SicBoManager.MachineMemberList.m_listMember.Count > 10)
            {
                m_Grid.maxPerLine = 2;
                m_BackGround.width = 300;
                int Number = SicBoManager.MachineMemberList.m_listMember.Count / 2;
                if (SicBoManager.MachineMemberList.m_listMember.Count % 2 != 0)
                    Number++;
                m_BackGround.height = Number * 28;
                m_BackGround.height += 40;
            }

            if ((SicBoManager.MachineList[(int)(SicBoLobby.Inst.TableControl.NowPage * 2 + ClickTarget)]) != SicBoManager.MachineMemberList.m_uiTID)
                return;
            M_Target.transform.position = m_NameListPos[ClickTarget].position;
            M_Target.SetActive(true);
            foreach (var item in SicBoManager.MachineMemberList.m_listMember)
            {
                GameObject Data = Instantiate(NameListPrefeb);
                Data.transform.parent = M_Target.transform;
                Data.transform.localScale = Vector3.one;
                UILabel M_Info = Data.GetComponent<UILabel>();
                M_Info.text = item.m_strNickName;
                M_Info.depth = 12;
                m_ObjList.Add(Data);
            }
            m_Grid.repositionNow = true;
        }
    }    
}