using UnityEngine;
using System.Collections;

public class IceAge_JP_Control : MonoBehaviour
{
    public static IceAge_JP_Control Inst;
    public float RandgeNumber = 10;
    public bool NowChange = false;
    public long TargetNumber = 0;
    public long NowNumber = 0;
    public Transform JPAnim;
    private float ResetTime = 3.0f;
    private float ResetTimer = 10.0f; //一開始進入遊戲 就要更新JP

    void Awake()
    {
        Inst = this;
    }

    // Use this for initialization
    void Start()
    {
        JPAnim.GetComponent<UISprite>().enabled = false;
        JPAnim.GetComponent<UISpriteAnimation>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        ShowNumber();   //顯示金錢圖片
        if (NowChange)
        {   //更新金錢值
            long Randge = (long)((TargetNumber - NowNumber) / RandgeNumber);
            NowNumber += Randge;
            if ((TargetNumber - NowNumber) <= RandgeNumber && (TargetNumber - NowNumber) >= 0)
            {
                NowNumber = TargetNumber;
                if (TargetNumber == 0) ResetTimer = 0.01f;
                NowChange = false;
            }
            else if ((NowNumber - TargetNumber) <= RandgeNumber && (NowNumber - TargetNumber) >= 0)
            {
                NowNumber = TargetNumber;
                if (TargetNumber == 0) ResetTimer = 0.01f;
                NowChange = false;
            }
        }
        if (ResetTimer != 0.0f)
        {
            ResetTimer += Time.deltaTime;
            if (ResetTimer > ResetTime && (IceAgeManager.GetNewJP || NowNumber == 0))
            {   //Reset時間到 且 (得到新JP 或 目前金額=0) > 更新JP
                UpdateJP();
                IceAgeManager.GetNewJP = false;
                ResetTimer = 0.0f;
            }
        }
    }

    public void UpdateJP()
    {   //NowJP = JP * 現在注數 / 滿注 (算完小數點捨去)
        TargetNumber = IceAgeManager.JPCnt * IceAgeButtonControl.Inst.NumberOfBets / 10;
        NowChange = true;
    }

    void ShowNumber()
    {
        for (int i = 0; i < transform.childCount; i++) transform.GetChild(i).GetComponent<UISprite>().enabled = false;  //隱藏全部數字
        string NumberString = NowNumber + "";   //數字轉字串
        byte NumberLength = (byte)NumberString.Length;  //取得字串長度
        for (int i = 0; i < NumberLength; i++)  //根據字串長度顯示數字 並給值
        {
            transform.GetChild(i).GetComponent<UISprite>().spriteName = "Number_JP_" + NumberString.Substring(NumberLength - (i + 1), 1);
            transform.GetChild(i).GetComponent<UISprite>().enabled = true;
        }
    }
}