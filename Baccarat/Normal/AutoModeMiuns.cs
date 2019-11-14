using UnityEngine;
using System.Collections;

public class AutoModeMiuns : MonoBehaviour
{
    private float AutoSpeed = 10;
    private float AutoSpeedMax = 20;
    float Timer = 0;
    float DelayTimer = 0.5f;
    bool LongClick = false;
    // Use this for initialization
    void Start()
    {
        Timer = 0;
        LongClick = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (LongClick)
        {
            if (Timer >= DelayTimer)
            {
                AutoSpeed += AutoSpeed * Time.deltaTime;
                if (AutoSpeed > AutoSpeedMax)
                {
                    AutoSpeed = AutoSpeedMax;
                }
                AutoMode_Control.AutoModeNumber -= (short)AutoSpeed;
                if (AutoMode_Control.AutoModeNumber < 0)
                {
                    AutoMode_Control.AutoModeNumber = 1000;
                }
            }
            else
            {
                Timer += Time.deltaTime;
            }
        }
    }

    void OnClick()
    {
        AutoMode_Control.AutoModeNumber--;
        if (AutoMode_Control.AutoModeNumber < 0)
        {
            AutoMode_Control.AutoModeNumber = 1000;
        }
    }

    void OnPress(bool bpress)
    {
        if (bpress)
        {
            LongClick = true;
        } 
        else
        {
            AutoSpeed = 10;
            Timer = 0;
            LongClick = false;
        }
    }
}
