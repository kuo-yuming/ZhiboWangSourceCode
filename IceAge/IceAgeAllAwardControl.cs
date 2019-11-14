using UnityEngine;
using System.Collections;

public class IceAgeAllAwardControl : MonoBehaviour
{
    public static int O_Page = 1;
    public IceAgePlayerAwardSort SortData_cs;
    public UILabel O_MaxPage;
    public UILabel O_NowPage;
    int MaxPage = 1;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if ((IceAgeManager.O_AwardRecord.Count % 12) != 0)
        {
            MaxPage = (IceAgeManager.O_AwardRecord.Count / 12) + 1;
        }
        else
        {
            MaxPage = (IceAgeManager.O_AwardRecord.Count / 12);

        }
        if (MaxPage < 1)
        {
            MaxPage = 1;
        }
        if (O_Page > MaxPage)
        {
            O_Page = 1;
        }
        O_MaxPage.text = MaxPage + "";
        O_NowPage.text = O_Page + "";

    }
    void O_TimeClick()
    {
        SortData_cs.O_TimeFirst();
        O_Page = 1;
    }
    void O_MachineIDClick()
    {
        SortData_cs.O_MachineIDFirst();
        O_Page = 1;
    }
    void O_NameClick()
    {
        SortData_cs.O_NameFirst();
        O_Page = 1;
    }
    void O_AwardClick()
    {
        SortData_cs.O_AwardFirst();
        O_Page = 1;
    }
    void O_MoneyClick()
    {
        SortData_cs.O_MoneyFirst();
        O_Page = 1;
    }
    void O_NextClick()
    {


        O_Page++;
        if (O_Page > MaxPage)
        {
            O_Page = 1;
        }
    }
    void O_BackClick()
    {

        O_Page--;
        if (O_Page < 1)
        {
            O_Page = MaxPage;
        }
    }
}
