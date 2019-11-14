using UnityEngine;
using System.Collections;

public class Machine_Control : MonoBehaviour {
    public UISprite[] Machine_Sprite;
	// Use this for initialization
	void Start () {
        Machine_Sprite[0].spriteName = (GameConnet.m_TMachineBuyInGameData.m_uiTID % 10).ToString();
        Machine_Sprite[1].spriteName = ((GameConnet.m_TMachineBuyInGameData.m_uiTID / 10) % 10).ToString();
        Machine_Sprite[2].spriteName = ((GameConnet.m_TMachineBuyInGameData.m_uiTID / 100) % 10).ToString();
	}
	
	// Update is called once per frame
	void Update () {
       
	}
}
