using UnityEngine;
using System.Collections;
using GameCore.Machine;

public class RaceInfoObject : MonoBehaviour {
    public GameObject NormalObject;
    public GameObject OnlyOneOjbect;

    public ENUM_RACE_EVENT_TYPE ObjectType = ENUM_RACE_EVENT_TYPE.Once;

    public UILabel NormalEntryPeople_Label;
    public UILabel NormalWinterOne_Label;
    public UILabel NormalWinterTwo_Label;
    public UILabel NormalWinterThree_Label;

    public UILabel OnlyOneEntryPeople_Label;
    public UILabel OnlyOneWinterOne_Label;
    public UILabel OnlyOneWinterTwo_Label;
    public UILabel OnlyOneWinterThree_Label;
    public UILabel OnlyOneEntryMoney_Label;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (ObjectType == ENUM_RACE_EVENT_TYPE.Once)
        {
            NormalObject.SetActive(false);
            OnlyOneOjbect.SetActive(true);
        }
        else
        {
            NormalObject.SetActive(true);
            OnlyOneOjbect.SetActive(false);
        }
	}
}
