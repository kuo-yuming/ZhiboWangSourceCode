using UnityEngine;
using System.Collections;
using GameEnum;
public class FCBar : MonoBehaviour {
    public GameObject R_BarObject;
    public GameObject KiRaObject;
   
    float SizeFloat = 0;
    float KiRaX = 0;
    float Speed = 0.1f;
    float EndSpeed = 0.3f;
    float size = 0.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
   //     SizeFloat = 1 + ((FourCard_Control.AllFCBetMoney[1] * 1.0f - FourCard_Control.AllFCBetMoney[0]) / FourCard_Control.AllFCBetMoney[1]);
        if ((FourCard_Control.AllFCBetMoney[0] + FourCard_Control.AllFCBetMoney[1]) != 0)
        {
            size = ((float)FourCard_Control.AllFCBetMoney[1] - (float)FourCard_Control.AllFCBetMoney[0]) / ((float)FourCard_Control.AllFCBetMoney[1] + (float)FourCard_Control.AllFCBetMoney[0]);
        }
        else
        {
            size = 0;
        }
        SizeFloat = 1 + size;

        if (MainGame_Control.StopModeState == ENUM_STOPMODE_STATE.FourCardShow)
        {
            if (FourCardHistory_Control.NowSizeFloat < SizeFloat)
            {
                FourCardHistory_Control.NowSizeFloat += Time.deltaTime * Speed;
            }
            else if (FourCardHistory_Control.NowSizeFloat > SizeFloat)
            {
                FourCardHistory_Control.NowSizeFloat -= Time.deltaTime * Speed;
            }
            else if (FourCardHistory_Control.NowSizeFloat == SizeFloat)
            {
                FourCardHistory_Control.NowSizeFloat = SizeFloat;
            }
        }
        else
        {
            if (FourCardHistory_Control.NowSizeFloat < SizeFloat)
            {
                FourCardHistory_Control.NowSizeFloat += Time.deltaTime * EndSpeed;
            }
            else if (FourCardHistory_Control.NowSizeFloat > SizeFloat)
            {
                FourCardHistory_Control.NowSizeFloat -= Time.deltaTime * EndSpeed;
            }
            else if (FourCardHistory_Control.NowSizeFloat == SizeFloat)
            {
                FourCardHistory_Control.NowSizeFloat = SizeFloat;
            }
        }

        KiRaX = (416 * FourCardHistory_Control.NowSizeFloat) - 416;
        R_BarObject.transform.localScale = new Vector3(1 * FourCardHistory_Control.NowSizeFloat, 1, 1);
        KiRaObject.transform.localPosition = new Vector3(KiRaX, KiRaObject.transform.localPosition.y, KiRaObject.transform.localPosition.z);
	}
}
