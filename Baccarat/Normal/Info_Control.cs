using UnityEngine;
using System.Collections;

public class Info_Control : MonoBehaviour {
    public static bool InfoButtonClickBool = false;
    public static byte InfoStaut = 0;
    public TweenPosition InfoTweenPosition;
	// Use this for initialization
	void Start () {
        InfoButtonClickBool = false;
        InfoStaut = 0;
	}
	
	// Update is called once per frame
	void Update () {

        if (InfoButtonClickBool && InfoStaut == 0)
        {
            InfoTweenPosition.PlayForward();
        }
        else if (!InfoButtonClickBool && InfoStaut == 1)
        {
            InfoTweenPosition.PlayReverse();
        }
	}

    public void InfoChangeVoid()
    {
        if (InfoButtonClickBool)
        {
            InfoStaut = 1;
        }
        else if (!InfoButtonClickBool)
        {
            InfoStaut = 0;
        }
        MainGame_Control.AutoAndInfoClickBool = false;
    }
}
