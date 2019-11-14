using UnityEngine;
using System.Collections;
using BankEnum;
public class KeyForget_Control : MonoBehaviour {
    public UIInput MemberAccountNumber_Input;
    public UIInput MemberKey_Input;
    public UIInput PhoneNumber_Input;
    public static string MemberAccountNumber = "";
    public static string MemberKey = "";
    public static string PhoneNumber = "";
    public UILabel MemberAccountNumber_Label;
    public UILabel MemberKey_Label;
    public UILabel PhoneNumber_Label;
	// Use this for initialization
	void Start () {
        MemberAccountNumber = "";
        MemberAccountNumber_Input.value = "";
        MemberKey = "";
        MemberKey_Input.value = "";
        PhoneNumber = "";
        PhoneNumber_Input.value = "";
        MemberAccountNumber_Label.text = "";
        MemberKey_Label.text = "";
        PhoneNumber_Label.text = "";
	}
	
	// Update is called once per frame
	void Update () {
        MemberAccountNumber = MemberAccountNumber_Input.value;
        MemberKey = MemberKey_Input.value;
        PhoneNumber = PhoneNumber_Input.value;
        //MemberAccountNumber = MemberAccountNumber_Label.text;
        //MemberKey = MemberKey_Label.text;
        //PhoneNumber = PhoneNumber_Label.text;


        if (Bank_Control.BankPage != (byte)ENUM_BANK_PAGE.KeyForgetPage)
        {
            MemberAccountNumber = "";
            MemberAccountNumber_Input.value = "";
            MemberKey = "";
            MemberKey_Input.value = "";
            PhoneNumber = "";
            PhoneNumber_Input.value = ""; 
            MemberAccountNumber_Label.text = "";
            MemberKey_Label.text = "";
            PhoneNumber_Label.text = "";
        }
	}
}
