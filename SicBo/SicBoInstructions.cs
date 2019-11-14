using UnityEngine;
using System.Collections;

public class SicBoInstructions : MonoBehaviour
{
    private byte NowPage;       //目前頁面
    private byte MinPage = 1;   //第一頁
    private byte MaxPage = 5;   //最後一頁
    public BoxCollider[] PageButton;   //按鈕
    //public UILabel PageLabel;   //頁面編號顯示
    public GameObject[] Content;//每頁的內容

    public void OpenInstructions()
    {
        NowPage = MinPage;  //初始目前頁數
        ShowContent(NowPage);
        //啟用按鈕
        PageButton[0].enabled = true;   
        PageButton[1].enabled = true;
        PageButton[2].enabled = true;
    }

    public void CloseInstructions()
    {   //關閉按鈕
        PageButton[0].enabled = false;
        PageButton[1].enabled = false;
        PageButton[2].enabled = false;
    }

    void NextPage()
    {
        NowPage = (byte)((NowPage == MaxPage) ? MinPage : NowPage + 1);
        ShowContent(NowPage);
    }

    void LastPage()
    {
        NowPage = (byte)((NowPage == MinPage) ? MaxPage : NowPage - 1);
        ShowContent(NowPage);
    }

    void ShowContent(byte PageNumber)
    {   //開啟目前頁面 關閉其他頁面
        for (int i = 0; i < Content.Length; i++)
            Content[i].SetActive(i == PageNumber - 1);
        //PageLabel.text = PageNumber.ToString("00") + "/" + MaxPage.ToString("00");  //顯示頁面編號
    }
}
