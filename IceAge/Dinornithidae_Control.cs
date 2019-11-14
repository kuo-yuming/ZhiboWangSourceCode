using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Dinornithidae_Control : MonoBehaviour
{
    private Transform[] Dinornithidae;
    private List<byte> NotPlayed = new List<byte>(); //還沒播放
    private float WaitTime = 30.0f;
    private float Timer = 0.01f;
    private byte NowPlay = 255;
    private bool IsHalfbody;

    // Use this for initialization
    void Start()
    {
        Dinornithidae = new Transform[transform.childCount];
        for (byte i = 0; i < transform.childCount; i++)
        {
            NotPlayed.Add(i);
            Dinornithidae[i] = transform.GetChild(i);
            Dinornithidae[i].transform.GetComponent<UISprite>().enabled = false;
            Dinornithidae[i].transform.GetComponent<UISpriteAnimation>().enabled = false;
        }
        //設定種子 確保亂數重複率降低
        System.Random rndVal = new System.Random(Guid.NewGuid().GetHashCode());
        WaitTime = (float)rndVal.Next(30, 61); //播放時間 30 ~ 60 秒
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer > WaitTime && NowPlay == 255) //時間大於 且 目前沒有播放
        {
            if (NotPlayed.Count == 0) for (byte i = 0; i < transform.childCount; i++) NotPlayed.Add(i);    //如果全部播放過了 重新給值
            //設定種子 確保亂數重複率降低
            System.Random rndVal = new System.Random(Guid.NewGuid().GetHashCode());
            byte Which = (byte)rndVal.Next(0, NotPlayed.Count); //亂數決定播放
            
            if (Dinornithidae[NotPlayed[Which]].transform.GetComponent<UISprite>().spriteName.Substring(14,1) == "H")
            {
                Dinornithidae[NotPlayed[Which]].transform.GetComponent<UISprite>().spriteName = "Dinornithidae_Halfbody_";
                Dinornithidae[NotPlayed[Which]].transform.GetComponent<UISprite>().enabled = true;
                Dinornithidae[NotPlayed[Which]].transform.GetComponent<UISpriteAnimation>().ResetToBeginning();
                Dinornithidae[NotPlayed[Which]].transform.GetComponent<UISpriteAnimation>().enabled = true;
                IsHalfbody = true;
            }
            else
            {
                Dinornithidae[NotPlayed[Which]].transform.GetComponent<UISprite>().spriteName = "Dinornithidae_Wholebody_";
                Dinornithidae[NotPlayed[Which]].transform.GetComponent<UISprite>().enabled = true;
                Dinornithidae[NotPlayed[Which]].transform.GetComponent<UISpriteAnimation>().ResetToBeginning();
                Dinornithidae[NotPlayed[Which]].transform.GetComponent<UISpriteAnimation>().enabled = true;
                IsHalfbody = false;
            }
            NowPlay = NotPlayed[Which]; //設定目前播放
            NotPlayed.Remove(Which);    //從List中移除
            Timer = 0.0f;   //計時器暫停
        }
        else if (NowPlay != 255) //目前有播放  且 播放來源為最後一張
        {
            if (IsHalfbody && Dinornithidae[NowPlay].transform.GetComponent<UISprite>().spriteName == "Dinornithidae_Halfbody_15")
            {   //關閉圖
                Dinornithidae[NowPlay].transform.GetComponent<UISprite>().enabled = false;
                Dinornithidae[NowPlay].transform.GetComponent<UISpriteAnimation>().enabled = false;
                NowPlay = 255;  //重置播放
                Timer = 0.01f;  //重啟計時器
            }
            else if (!IsHalfbody && Dinornithidae[NowPlay].transform.GetComponent<UISprite>().spriteName == "Dinornithidae_Wholebody_11")
            {
                Dinornithidae[NowPlay].transform.GetComponent<UISprite>().enabled = false;
                Dinornithidae[NowPlay].transform.GetComponent<UISpriteAnimation>().enabled = false;
                NowPlay = 255;  //重置播放
                Timer = 0.01f;  //重啟計時器
            }
        }
    }
}