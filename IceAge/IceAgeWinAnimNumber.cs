using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IceAgeWinAnimNumber : MonoBehaviour
{
    public int ChangeCntMin = 3;
    public int ChangeCntMax = 6;
    public float ChangeTime = 0.2f;
    public float AnimDelay = 0.0f;
    public int Final = 0;
    bool StartPlay = false;
    public bool Over = false;
    UISprite m_MainSprite;
    List<int> ChangeNumber = new List<int>();
    public IceAgeAnaimationPlayer m_AnimPlayer;
    int Index = 0;
    float Timer = 0.0f;
    // Use this for initialization
    void Start()
    {
        m_MainSprite = this.GetComponent<UISprite>();
        m_AnimPlayer = this.GetComponentInChildren<IceAgeAnaimationPlayer>();
        m_AnimPlayer.StartDelayTime = AnimDelay;
        int ChangeCnt = Random.Range(ChangeCntMin, ChangeCntMax);
        for (int i = 0; i < ChangeCnt; i++)
        {
            int Target = Random.Range(0, 10);

            if (i > 0)
            {
                if (ChangeNumber[(i - 1)] == Target)
                {
                    while (ChangeNumber[(i - 1)] != Target)
                    {
                        Target = Random.Range(0, 10);
                    }
                }
            }
            else
                Target = Random.Range(1, 10);
            ChangeNumber.Add(Target);
        }
        ChangeNumber.Add(Final);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_AnimPlayer != null && m_MainSprite != null)
        {
            if (!StartPlay)
            {
                m_AnimPlayer.StartPlay = true;
                StartPlay = true;
            }
            if (m_AnimPlayer.OneceOver && !Over)
            {
                if (Timer >= ChangeTime)
                {
                    m_MainSprite.enabled = true;
                    m_AnimPlayer.Show = false;
                    m_MainSprite.spriteName = "0" + ChangeNumber[Index];
                    Index++;
                    Timer = 0.0f;
                    if (Index >= ChangeNumber.Count)
                    {
                        Over = true;
                    }
                }
                else
                    Timer += Time.deltaTime;
            }
        }
    }
}