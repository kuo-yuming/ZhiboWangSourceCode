using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IceAgeSoundControl : MonoBehaviour
{
    public static IceAgeSoundControl Inst;
    public AudioSource IceAgeSound;
    public Dictionary<SoundName, AudioClip> SoundID;
    public enum SoundName
    {
        ChangeScene = 0, 
        DoubleUpBGM = 1, 
        DoubleUpFail = 2,
        DoubleUpSuccess = 3,
        FiveCombo = 4,
        FourCombo = 5,
        GetAwards = 6,
        JPCombo = 7,
        Knock = 8,
        NotGetAwards = 9,
        Shot = 10,
        ShowBonus = 11,
        SlotEnd = 12,
        SlotMoving = 13,
        DoubleBite = 14,
    }

    void Awake()
    {
        Inst = this;
    }

    // Use this for initialization
    void Start()
    {   //初始化音效元件與音效列表
        IceAgeSound = transform.GetComponent<AudioSource>();
        SoundID = new Dictionary<SoundName, AudioClip>();
        SoundID.Add(SoundName.ChangeScene, Sound_Control.Instance.IceAge_Sound.ChangeScene);
        SoundID.Add(SoundName.DoubleUpBGM, Sound_Control.Instance.IceAge_Sound.DoubleUpBGM);
        SoundID.Add(SoundName.DoubleUpFail, Sound_Control.Instance.IceAge_Sound.DoubleUpFail);
        SoundID.Add(SoundName.DoubleUpSuccess, Sound_Control.Instance.IceAge_Sound.DoubleUpSuccess);
        SoundID.Add(SoundName.FiveCombo, Sound_Control.Instance.IceAge_Sound.FiveCombo);
        SoundID.Add(SoundName.FourCombo, Sound_Control.Instance.IceAge_Sound.FourCombo);
        SoundID.Add(SoundName.GetAwards, Sound_Control.Instance.IceAge_Sound.GetAwards);
        SoundID.Add(SoundName.JPCombo, Sound_Control.Instance.IceAge_Sound.JPCombo);
        SoundID.Add(SoundName.Knock, Sound_Control.Instance.IceAge_Sound.Knock);
        SoundID.Add(SoundName.NotGetAwards, Sound_Control.Instance.IceAge_Sound.NotGetAwards);
        SoundID.Add(SoundName.Shot, Sound_Control.Instance.IceAge_Sound.Shot);
        SoundID.Add(SoundName.ShowBonus, Sound_Control.Instance.IceAge_Sound.ShowBonus);
        SoundID.Add(SoundName.SlotEnd, Sound_Control.Instance.IceAge_Sound.SlotEnd);
        SoundID.Add(SoundName.SlotMoving, Sound_Control.Instance.IceAge_Sound.SlotMoving);
        SoundID.Add(SoundName.DoubleBite, Sound_Control.Instance.IceAge_Sound.DoubleBite);
    }

    public void PlaySound(SoundName Name, bool IsLoop)
    {
        IceAgeSound.clip = SoundID[Name];
        IceAgeSound.loop = IsLoop;
        IceAgeSound.Play();
    }
}