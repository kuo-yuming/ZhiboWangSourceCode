using UnityEngine;
using System.Collections;

public class SicBoPanelAlphaFollow : MonoBehaviour
{
    public UIPanel SelfPanel;   //自己
    public UIPanel TargetPanel; //目標Panel

    // Update is called once per frame
    void Update()
    {
        SelfPanel.alpha = TargetPanel.alpha;
    }
}