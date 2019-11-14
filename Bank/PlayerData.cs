using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {
    public UILabel PlayerName;
    public UILabel PlayerLevel;
    public UILabel PlayerCash;
    public GameObject Button;
    public UILabel ActiveHoshi;
    public byte Number;
    public UISprite BarSprite;
    public bool ColorCheck = false;
    public bool SelectCheck = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (ColorCheck)
        {
            if (!SelectCheck)
            {
                if (Number == 0)
                {
                    BarSprite.color = new Color32(255, 255, 255, 0);
                }
                else
                {
                    BarSprite.color = new Color32(255, 255, 255, 255);
                }
            }
            else
            {
                BarSprite.color = new Color32(255, 255, 255, 0);
            }
            ColorCheck = false;
        }
	}
}
