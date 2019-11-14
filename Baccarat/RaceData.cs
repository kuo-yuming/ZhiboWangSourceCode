using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore.Machine;

public class RaceData : MonoBehaviour {
    public ushort ID;
    public ENUM_RACE_STATE RACE_EVENT_TYPE = ENUM_RACE_STATE.Racing;
    public ENUM_RACE_EVENT_TYPE m_RACE_EVENT_TYPE = ENUM_RACE_EVENT_TYPE.Once;
    public UILabel Date_Label;
    public UILabel ApplyTime_Label;
    public UILabel StartTime_Label;
    public UILabel Round_Label;
    public UILabel Cost_Label;
    public UILabel People_Label;
    public UIButton RaceDataButton;
    public UIButton RaceInfoButton;
    public UISprite ButtonSprite;
    public ENUM_RACE_FEE_TYPE ENUM_RACE_FEE_TYPE = ENUM_RACE_FEE_TYPE.Money;
    public string RaceName = "";
    public string RaceTime = "";
    public uint uiFeeVal = 0;
    public bool Signed_Bool = false;
    public GameObject m_Button;
    public GameObject m_ExplainButton;
    public uint MaxPlayerCnt = 0;
    public UILabel Explanation_Label;
    // Sign = 0, // 可接受報名
    // WaitStart = 1, // 等待開始
    // Racing = 2, // 賽局進行中	
    // End = 3, // 賽局結束  	(預設)

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        RaceButton Data_cs = RaceDataButton.GetComponent<RaceButton>();
        Data_cs.ButtonSigned_Bool = Signed_Bool;
        Data_cs.ButtonType = RACE_EVENT_TYPE;
        Data_cs.ButtonID = ID;
        Data_cs.FeeType = ENUM_RACE_FEE_TYPE;
        Data_cs.FeeVal = uiFeeVal;
        Data_cs.RaceButtonName = RaceName;
        Data_cs.RaceButtonTime = RaceTime;
        RaceButton Data_cs2 = RaceInfoButton.GetComponent<RaceButton>();
        Data_cs2.ButtonSigned_Bool = Signed_Bool;
        Data_cs2.ButtonType = RACE_EVENT_TYPE;
        Data_cs2.ButtonID = ID;
        Data_cs2.FeeType = ENUM_RACE_FEE_TYPE;
        Data_cs2.FeeVal = uiFeeVal;
        Data_cs2.RaceButtonName = RaceName;
        Data_cs2.RaceButtonTime = RaceTime;

        if (RACE_EVENT_TYPE == ENUM_RACE_STATE.Sign && !Signed_Bool)
        {
            ButtonSprite.spriteName = "btn_bglgamelistA_0";
            if (Competition.SinedOK_Bool)
                m_Button.SetActive(false);
            else
                m_Button.SetActive(true);
            m_ExplainButton.SetActive(true);
            m_Button.transform.localPosition = new Vector3(595, -80, 0);
        }
        else if (RACE_EVENT_TYPE == ENUM_RACE_STATE.Sign && Signed_Bool)
        {
            ButtonSprite.spriteName = "btn_bglgamelistD_0";
            m_Button.SetActive(true);
            m_ExplainButton.SetActive(true);
            m_Button.transform.localPosition = new Vector3(595, -80, 0);
        }
        else if (RACE_EVENT_TYPE == ENUM_RACE_STATE.WaitStart || RACE_EVENT_TYPE == ENUM_RACE_STATE.Racing)
        {
            if (VersionDef.InternationalLanguageSystem)
            {
                ApplyTime_Label.text = Font_Control.Instance.m_dicMsgStr[2008092];
                StartTime_Label.text = Font_Control.Instance.m_dicMsgStr[2008093];
            }
            else
            {
                ApplyTime_Label.text = "已截止";
                StartTime_Label.text = "比賽進行中";
            }
            ButtonSprite.spriteName = "btn_bglgamelistC_0";
            if (!Signed_Bool)
            {
                m_Button.SetActive(false);
            }
            m_ExplainButton.SetActive(false);
            m_Button.transform.localPosition = new Vector3(595, -126, 0);
        }
        else if (RACE_EVENT_TYPE == ENUM_RACE_STATE.End)
        {
            ButtonSprite.spriteName = "btn_bglgamelistE_0";
            if (!Signed_Bool)
            {
                m_Button.SetActive(false);
            }
            m_ExplainButton.SetActive(false);
            m_Button.transform.localPosition = new Vector3(595, -126, 0);
        }
    }
}
