using UnityEngine;
using System.Collections;

public class WinControl : MonoBehaviour {
	public GameObject Win;
	public GameObject Jackpot;
	public UISpriteAnimation JPanim;
	public UISpriteAnimation Winanim;
	// Use this for initialization


	public void JackPot()
	{
		Jackpot.SetActive (true);
		JPanim.ResetToBeginning ();
		JPanim.Play ();
	}
}
