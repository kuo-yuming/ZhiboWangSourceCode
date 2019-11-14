using UnityEngine;
using System.Collections;

public class FreeGame_Control : MonoBehaviour
{
    public static FreeGame_Control Inst;
    public Transform MainGameTitle;
    public Transform FreeGameTitle;
    public Transform NumberTitleTen;
    public Transform NumberTitleOne;

    private float ResetTitleTime = 1.0f;
    private float ResetTitleTimer = 0.0f;

    void Awake()
    {
        Inst = this;
    }

    // Use this for initialization
    void Start()
    {
        FreeGameTitle.GetComponent<UISprite>().enabled = false;
        NumberTitleTen.GetComponent<UISprite>().enabled = false;
        NumberTitleOne.GetComponent<UISprite>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IceAgeGameMain.Inst.GameStatus == IceAgeGameMain.Game_Status.FreeGame)
        {
            if (ResetTitleTimer != 0)
            {
                ResetTitleTimer += Time.deltaTime;
                if (ResetTitleTimer > ResetTitleTime)
                {   //重新播放
                    FreeGameTitle.GetComponent<UISprite>().spriteName = "FreeGame_00";
                    FreeGameTitle.GetComponent<UISpriteAnimation>().ResetToBeginning();
                    FreeGameTitle.GetComponent<UISpriteAnimation>().Play();
                    ResetTitleTimer = 0.0f;
                }
            }

            if (FreeGameTitle.GetComponent<UISprite>().spriteName == "FreeGame_15" && ResetTitleTimer == 0)
            {   //每播一次 休息一秒
                ResetTitleTimer = 0.01f;
            }
        }
    }

    public void ChangeGameTitle(bool IsMain)
    {
        MainGameTitle.GetComponent<UISprite>().enabled = IsMain;
        FreeGameTitle.GetComponent<UISprite>().enabled = !IsMain;
        NumberTitleTen.GetComponent<UISprite>().enabled = !IsMain;
        NumberTitleOne.GetComponent<UISprite>().enabled = !IsMain;
        ChangeGameNumber(IceAgeManager.m_BetResult.m_byFreeRoundCnt);
    }

    public void ChangeGameNumber(byte Count)
    {
        NumberTitleTen.GetComponent<UISprite>().spriteName = "FreeNumber_" + (Count / 10);
        NumberTitleOne.GetComponent<UISprite>().spriteName = "FreeNumber_" + (Count % 10);
    }
}