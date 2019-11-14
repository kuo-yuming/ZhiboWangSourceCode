using UnityEngine;
using System.Collections;

public class Explain : MonoBehaviour {
    public static bool ExplainBoxOpen_Bool = false;
    public GameObject ExplainBox_Object;
    public static byte Page_Number = 1;
    public UISprite Page_Sprite;
	// Use this for initialization
	void Start () {
        ExplainBoxOpen_Bool = false;
        Page_Number = 1;
    }
	
	// Update is called once per frame
	void Update () {
        if (ExplainBoxOpen_Bool)
        {
            ExplainBox_Object.SetActive(true);
        }
        else
        {
            ExplainBox_Object.SetActive(false);
            Page_Number = 1;
        }

        Page_Sprite.spriteName = "bg_aboutrbgl_0" + Page_Number;
	}
}
