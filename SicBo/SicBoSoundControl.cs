using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SicBoSoundControl : MonoBehaviour
{
    public AudioSource Sound;
    public Dictionary<SoundName, AudioClip> SoundID;
    private byte ClipID = 0;
    private List<SoundName> ClipList;
    private bool IsPlaying = false;

    public enum SoundName
    {
        //語音 - 1				
        SicBo_VoiceOne = 0,   //dv01
        //語音 - 2				
        SicBo_VoiceTwo = 1,   //dv02
        //語音 - 3				
        SicBo_VoiceThree = 2, //dv03
        //語音 - 4				
        SicBo_VoiceFour = 3,  //dv04
        //語音 - 5				
        SicBo_VoiceFive = 4,  //dv05
        //語音 - 6				
        SicBo_VoiceSix = 5,   //dv06
        //語音 - 4 點				
        SicBo_Voice4Point = 6,    //dv07
        //語音 - 5 點				
        SicBo_Voice5Point = 7,    //dv08
        //語音 - 6 點				
        SicBo_Voice6Point = 8,    //dv09
        //語音 - 7 點				
        SicBo_Voice7Point = 9,    //dv10
        //語音 - 8 點				
        SicBo_Voice8Point = 10,    //dv11
        //語音 - 9 點				
        SicBo_Voice9Point = 11,    //dv12
        //語音 - 10 點				
        SicBo_Voice10Point = 12,   //dv13
        //語音 - 11 點				
        SicBo_Voice11Point = 13,   //dv14
        //語音 - 12 點				
        SicBo_Voice12Point = 14,   //dv15
        //語音 - 13 點				
        SicBo_Voice13Point = 15,   //dv16
        //語音 - 14 點			
        SicBo_Voice14Point = 16,   //dv17
        //語音 - 15 點				
        SicBo_Voice15Point = 17,   //dv18
        //語音 - 16 點				
        SicBo_Voice16Point = 18,   //dv19
        //語音 - 17 點
        SicBo_Voice17Point = 19,   //dv20
        //語音 - 大
        SicBo_VoiceBig = 20,   //dv21
        //語音 - 小
        SicBo_VoiceSmall = 21, //dv22
        //語音 - 圍骰
        SicBo_VoiceTripleDice = 22,//dv23
    }
    // Use this for initialization
    void Start()
    {
        SoundID = new Dictionary<SoundName, AudioClip>();
        SoundID.Add(SoundName.SicBo_VoiceOne, Sound_Control.Instance.SicBo_Sound.SicBo_VoiceOne);
        SoundID.Add(SoundName.SicBo_VoiceTwo, Sound_Control.Instance.SicBo_Sound.SicBo_VoiceTwo);
        SoundID.Add(SoundName.SicBo_VoiceThree, Sound_Control.Instance.SicBo_Sound.SicBo_VoiceThree);
        SoundID.Add(SoundName.SicBo_VoiceFour, Sound_Control.Instance.SicBo_Sound.SicBo_VoiceFour);
        SoundID.Add(SoundName.SicBo_VoiceFive, Sound_Control.Instance.SicBo_Sound.SicBo_VoiceFive);
        SoundID.Add(SoundName.SicBo_VoiceSix, Sound_Control.Instance.SicBo_Sound.SicBo_VoiceSix);
        SoundID.Add(SoundName.SicBo_Voice4Point, Sound_Control.Instance.SicBo_Sound.SicBo_Voice4Point);
        SoundID.Add(SoundName.SicBo_Voice5Point, Sound_Control.Instance.SicBo_Sound.SicBo_Voice5Point);
        SoundID.Add(SoundName.SicBo_Voice6Point, Sound_Control.Instance.SicBo_Sound.SicBo_Voice6Point);
        SoundID.Add(SoundName.SicBo_Voice7Point, Sound_Control.Instance.SicBo_Sound.SicBo_Voice7Point);
        SoundID.Add(SoundName.SicBo_Voice8Point, Sound_Control.Instance.SicBo_Sound.SicBo_Voice8Point);
        SoundID.Add(SoundName.SicBo_Voice9Point, Sound_Control.Instance.SicBo_Sound.SicBo_Voice9Point);
        SoundID.Add(SoundName.SicBo_Voice10Point, Sound_Control.Instance.SicBo_Sound.SicBo_Voice10Point);
        SoundID.Add(SoundName.SicBo_Voice11Point, Sound_Control.Instance.SicBo_Sound.SicBo_Voice11Point);
        SoundID.Add(SoundName.SicBo_Voice12Point, Sound_Control.Instance.SicBo_Sound.SicBo_Voice12Point);
        SoundID.Add(SoundName.SicBo_Voice13Point, Sound_Control.Instance.SicBo_Sound.SicBo_Voice13Point);
        SoundID.Add(SoundName.SicBo_Voice14Point, Sound_Control.Instance.SicBo_Sound.SicBo_Voice14Point);
        SoundID.Add(SoundName.SicBo_Voice15Point, Sound_Control.Instance.SicBo_Sound.SicBo_Voice15Point);
        SoundID.Add(SoundName.SicBo_Voice16Point, Sound_Control.Instance.SicBo_Sound.SicBo_Voice16Point);
        SoundID.Add(SoundName.SicBo_Voice17Point, Sound_Control.Instance.SicBo_Sound.SicBo_Voice17Point);
        SoundID.Add(SoundName.SicBo_VoiceBig, Sound_Control.Instance.SicBo_Sound.SicBo_VoiceBig);
        SoundID.Add(SoundName.SicBo_VoiceSmall, Sound_Control.Instance.SicBo_Sound.SicBo_VoiceSmall);
        SoundID.Add(SoundName.SicBo_VoiceTripleDice, Sound_Control.Instance.SicBo_Sound.SicBo_VoiceTripleDice);
    }

    void Update()
    {
        if (IsPlaying)
        {
            if (!Sound.isPlaying)
            {
                if (ClipID >= ClipList.Count)
                {
                    IsPlaying = false;//結束播放
                    SicBoGameMain.Inst.WaitTimer = 1.0f;    //1秒後 回到桌檯
                }
                else
                {
                    PlaySound(ClipList[ClipID]);
                    ClipID++;
                }
            }
        }
    }

    public void PlayResultSound(byte[] Dice, byte Dice4)
    {
        ClipID = 0; //初始化ID
        ClipList = new List<SoundName>();   //初始化List
        if (Dice[0] == Dice[0] && Dice[1] == Dice[2] && Dice[0] == Dice4)   //四豹
        {   //四豹不播
            SicBoGameMain.Inst.WaitTimer = 3.0f;    //3秒後 回到桌檯
            return;
        }
        else if (Dice[0] == Dice[1] && Dice[0] == Dice[2] && Dice[0] != Dice4 && Dice4 != 0)    //破骰
        {   //1 1 1 圍骰 1 1 2 4點 小 9段 音效
            ClipList.Add((SoundName)(Dice[0] - 1)); //骰子1
            ClipList.Add((SoundName)(Dice[1] - 1)); //骰子2
            ClipList.Add((SoundName)(Dice[2] - 1)); //骰子3
            ClipList.Add(SoundName.SicBo_VoiceTripleDice);  //圍骰
            ClipList.Add((SoundName)(Dice[0] - 1)); //骰子1
            ClipList.Add((SoundName)(Dice[1] - 1)); //骰子2
            ClipList.Add((SoundName)(Dice4 - 1));   //骰子4
            int TmpTotal = Dice[0] + Dice[1] + Dice4;
            ClipList.Add((SoundName)(TmpTotal + 2));   //總和
            if (TmpTotal >= 4 && TmpTotal <= 10)
                ClipList.Add(SoundName.SicBo_VoiceSmall);   //小
            else if (TmpTotal >= 11 && TmpTotal <= 17)
                ClipList.Add(SoundName.SicBo_VoiceBig);     //大
        }
        else if (Dice[0] == Dice[1] && Dice[0] == Dice[2] && Dice4 == 0)    //三豹
        {   //1 1 1 圍骰 4段音效
            ClipList.Add((SoundName)(Dice[0] - 1)); //骰子1
            ClipList.Add((SoundName)(Dice[1] - 1)); //骰子2
            ClipList.Add((SoundName)(Dice[2] - 1)); //骰子3
            ClipList.Add(SoundName.SicBo_VoiceTripleDice);  //圍骰
        }
        else  //其他
        {   //1 2 3 6點 小 5段 音效
            ClipList.Add((SoundName)(Dice[0] - 1)); //骰子1
            ClipList.Add((SoundName)(Dice[1] - 1)); //骰子2
            ClipList.Add((SoundName)(Dice[2] - 1)); //骰子3
            int TmpTotal = Dice[0] + Dice[1] + Dice[2];
            ClipList.Add((SoundName)(TmpTotal + 2));   //總和
            if (TmpTotal >= 4 && TmpTotal <= 10)
                ClipList.Add(SoundName.SicBo_VoiceSmall);   //小
            else if (TmpTotal >= 11 && TmpTotal <= 17)
                ClipList.Add(SoundName.SicBo_VoiceBig);     //大
        }
        //開始播放
        PlaySound(ClipList[ClipID]);
        ClipID++;
        IsPlaying = true;
    }

    public void PlaySound(SoundName Name)
    {
        Sound.clip = SoundID[Name];
        Sound.Play();
    }
}
