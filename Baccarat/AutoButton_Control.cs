using UnityEngine;
using System.Collections;

public class AutoButton_Control : MonoBehaviour {
    public static ArrayList TableGroupID = new ArrayList();
    public static bool TableDataGet = false;
    public static byte ClickGroupID = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (TableDataGet)
        {
            foreach (var item in BaccaratManager.m_MachineBuyInConfig.m_dicTableGroupSet)
            {
                TableGroupID.Add(item.Value.m_byGroupID);
            }
            TableDataGet = false;
        }
	}

   
}
