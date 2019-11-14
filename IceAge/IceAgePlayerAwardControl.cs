using UnityEngine;
using System.Collections;

public class IceAgePlayerAwardControl : MonoBehaviour
{

    public static int M_Page = 1;
    public IceAgePlayerAwardSort SortData_cs;
    public UILabel M_MaxPage;
    public UILabel M_NowPage;
    int MaxPage = 1;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ((IceAgeManager.M_AwardRecord.Count % 12) != 0)
        {
            MaxPage = (IceAgeManager.M_AwardRecord.Count / 12 + 1);
        }
        else
        {
            MaxPage = (IceAgeManager.M_AwardRecord.Count / 12);

        }
        if (MaxPage < 1)
        {
            MaxPage = 1;
        }
        if (M_Page > MaxPage)
        {
            M_Page = 1;
        }
        M_MaxPage.text = MaxPage + "";
        M_NowPage.text = M_Page + "";

    }
    void M_TimeClick()
    {
        SortData_cs.M_TimeFirst();
        M_Page = 1;
    }
    void M_MachineIDClick()
    {
        SortData_cs.M_MachineIDFirst();
        M_Page = 1;
    }
    void M_AwardClick()
    {
        SortData_cs.M_AwardFirst();
        M_Page = 1;
    }
    void M_MoneyClick()
    {
        SortData_cs.M_MoneyFirst();
        M_Page = 1;
    }
    void M_NextClick()
    {



        M_Page++;
        if (M_Page > MaxPage)
        {
            M_Page = 1;
        }
    }
    void M_BackClick()
    {

        M_Page--;
        if (M_Page < 1)
        {
            M_Page = MaxPage;
        }
    }
}
