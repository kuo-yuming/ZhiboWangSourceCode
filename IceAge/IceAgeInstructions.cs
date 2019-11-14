using UnityEngine;
using System.Collections;

public class IceAgeInstructions : MonoBehaviour
{
    public static IceAgeInstructions Inst;
    public Transform BackMask;
    public UISprite Background;
    public UISprite Content;
    public GameObject Arrow_Right;
    public GameObject Arrow_Left;
    public GameObject CloseButton;
    private byte NowPage;
    private byte MinPage = 1;
    private byte MaxPage = 7;
    void Awake()
    {
        Inst = this;
    }
    // Use this for initialization
    void Start()
    {   //初始化
        NowPage = MinPage;  //初始目前頁數
        if (!GameConnet.GameInfoOpen) OpenInstructions(true);    
        else OpenInstructions(false);   //關閉 物件顯示
    }

    public void OpenInstructions(bool IsOpen)
    {
        BackMask.GetComponent<UITexture>().enabled = IsOpen;
        BackMask.GetComponent<BoxCollider>().enabled = IsOpen;
        Background.enabled = IsOpen;
        Content.enabled = IsOpen;
        Content.spriteName = "Instructions_01";
        Arrow_Right.SetActive(IsOpen);
        Arrow_Left.SetActive(IsOpen);
        CloseButton.SetActive(IsOpen);
    }   

    void NextPage()
    {
        NowPage = (byte)((NowPage == MaxPage) ? MinPage : NowPage + 1);
        Content.spriteName = "Instructions_0" + NowPage;
    }

    void LastPage()
    {
        NowPage = (byte)((NowPage == MinPage) ? MaxPage : NowPage - 1);
        Content.spriteName = "Instructions_0" + NowPage;
    }

    void Close()
    {
        OpenInstructions(false);
    }
}