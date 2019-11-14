using UnityEngine;
using System.Collections;
using BankEnum;

public class KeyRevise_Control : MonoBehaviour {
    public UIInput OldKey_Input;
    public UIInput NewKey_Input;
    public UIInput AgainNewKey_Input;
    public UILabel OldKey_Label;
    public UILabel NewKey_Label;
    public UILabel AgainNewKey_Label;

    public static string OldKey = "";
    public static string NewKey = "";
    public static string Againkey = "";
    public static bool OldKeyClick = false;
    public static bool NewKeyClick = false;
    public static bool AgainNewClick = false;
	// Use this for initialization
    void Start()
    {
        OldKey = "";
        NewKey = "";
        Againkey = "";
        OldKeyClick = false;
        NewKeyClick = false;
        AgainNewClick = false;
    }
	
	// Update is called once per frame
	void Update () {
        //OldKey_Label.text = OldKey_Input.value;
        //NewKey_Label.text = NewKey_Input.value;
        //AgainNewKey_Label.text = AgainNewKey_Input.value;
        OldKey = OldKey_Input.value;
        NewKey = NewKey_Input.value;
        Againkey = AgainNewKey_Input.value;

        if (Bank_Control.BankPage != (byte)ENUM_BANK_PAGE.KeyRevisePage)
        {
            OldKey_Input.value = "";
            NewKey_Input.value = "";
            AgainNewKey_Input.value = "";
            OldKey_Label.text = "";
            NewKey_Label.text = "";
            AgainNewKey_Label.text = "";
            OldKey = "";
            NewKey = "";
            Againkey = "";
            OldKeyClick = false;
            NewKeyClick = false;
            AgainNewClick = false;
        }
     }
}
