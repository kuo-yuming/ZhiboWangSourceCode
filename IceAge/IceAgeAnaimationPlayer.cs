using UnityEngine;
using System.Collections;
using System.Threading;

public class IceAgeAnaimationPlayer : MonoBehaviour
{
    public UISprite AnimSprite;
    //public Sprite[] Images;
    public bool Loop = true;
    public float Speed;
    public float StartDelayTime = 0.0f;
    bool DelayOver = false;
    public bool PlayOnAwake = false;
    public bool Show = true;
    public bool StartPlay = false;
    public bool PlayReStart = false;
    bool ReStartFirst = true;
    float Timer = 0.0f;
    public int Index = 0;
    public bool OneceOver = false;
    public bool ReOneceOver = false;
    float BtnTimer = 0.0f;

    // Use this for initialization
    void Start()
    {
        AnimSprite = this.gameObject.GetComponent<UISprite>();
        if (PlayOnAwake) StartPlay = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (StartPlay)
        {
            if (DelayOver)
            {
                if (BtnTimer != 0.0f)
                {
                    BtnTimer += Time.fixedDeltaTime;
                    if (BtnTimer >= 0.2f)
                    {
                        //第一次播完 開放 得分按鈕
                        IceAgeButtonControl.Inst.SetGetScoreButton();
                        IceAgeGameMain.Inst.ReStartTimer = 0.0f;
                        BtnTimer = 0.0f;
                    }
                }

                if (Timer > (Speed))
                {
                    AnimSprite.spriteName = AnimSprite.atlas.spriteList[Index].name;
                    Index++;
                    Timer = 0.0f;
                }
                if (Index >= AnimSprite.atlas.spriteList.Count && !Loop)
                {
                    OneceOver = true;
                    DelayOver = false;
                    StartPlay = false;
                    Index = 0;
                    Timer = 0.0f;
                    return;
                }
                else if (Index >= AnimSprite.atlas.spriteList.Count && Loop)
                {   //第一次播完 開放 得分按鈕
                    //IceAgeButtonControl.Inst.SetGetScoreButton();
                    //IceAgeGameMain.Inst.ReStartTimer = 0.01f;
                }
            }
            else
            {
                if (Timer >= StartDelayTime)
                {
                    Timer = 0.0f;
                    BtnTimer = 0.01f;
                    DelayOver = true;
                    Show = true;
                }
            }
            Timer += Time.fixedDeltaTime;
        }
        if (PlayReStart)
        {
            if (ReStartFirst)
            {
                Loop = false;
                ReStartFirst = false;
                Index = AnimSprite.atlas.spriteList.Count - 1;
            }
            if (Timer > (Speed))
            {
                AnimSprite.spriteName = AnimSprite.atlas.spriteList[Index].name;
                Index--;

                Timer = 0.0f;
            }
            if (Index < 0)
            {
                ReStartFirst = true;
                PlayReStart = false;
                Index = 0;
                ReOneceOver = true;
                Timer = 0.0f;
                return;
            }
            Timer += Time.fixedDeltaTime;
        }
        if (Loop)
        {
            if (Index >= AnimSprite.atlas.spriteList.Count)
            {
                Index = 0;
            }
        }
        if (StartDelayTime != 0.0f && StartPlay)
        {
            if (DelayOver)
                AnimSprite.enabled = Show;
            else
                AnimSprite.enabled = false;
        }
        else
        {
            AnimSprite.enabled = Show;
        }


    }
    public void Play()
    {
        StartPlay = true;
        Index = 0;
    }
    public void PlayRe()
    {
        PlayReStart = true;
    }
    public void Speace()
    {
        StartPlay = false;
    }
    public void KeepPlay()
    {
        StartPlay = true;
    }
    public void Reseat()
    {
        Index = 0;
        if (AnimSprite.atlas != null)
            AnimSprite.spriteName = AnimSprite.atlas.spriteList[Index].name;
    }

}