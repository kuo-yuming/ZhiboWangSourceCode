using UnityEngine;
using System.Collections;

public class AutoMode_Unlimited : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        AutoMode_Control.AutoModeNumber = 1000;
    }
}
