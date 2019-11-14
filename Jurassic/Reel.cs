using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Reel : MonoBehaviour
{
	/// <summary>
	/// The height of the row.
	/// </summary>
	float clickOffset = 0f;
	const float numberOfRowsPerClick = 2f;
	public List<UISprite> symbol = new List<UISprite>(); 
	public List<climb> climbs = new List<climb> ();
	public UISprite animObj;
	public float rowHeight;
	public int target = 0;
	public AudioSource reelClickSfx;
	public static Action StartSpin;
	public UISpriteAnimation symbolAnim;
	public UISprite AnimSprite;
	public AudioClip StopSource;
	public bool LoadSource;
	List<string> SymNum = new List<string> {"SymNum1_","SymNum2_","SymNum3_",
											"SymNum4_","SymNum5_","SymNum6_",
											"SymNum7_","SymNum8_","SymNum9_","SymNum10_"} ;
	

	public enum State
	{
		Idle,
		Spinning,
		Stopping
	}

	[HideInInspector]
	public State state = State.Idle;

	void Start ()
	{
		if(LoadSource)
			StopSource = Sound_Control.Instance.Jurassic_Sound.SlotStop;
		if (reelClickSfx)
			reelClickSfx.pitch = UnityEngine.Random.Range (0.8f, 1.2f);
		clickOffset = UnityEngine.Random.Range (0f, rowHeight * numberOfRowsPerClick);
		SlotManager.startSpin += StartRandomSpin;
		StopSymbolAnimation ();
	}

	void OnDiestory()
	{
		SlotManager.startSpin -= StartRandomSpin;
	}

	public string RandomSymbol ()
	{
		
		return state == State.Stopping ? SymNum[target-1] + "1" : SymNum[UnityEngine.Random.Range (0, SymNum.Count)] + "1";
	}

	public void PlaySymbolAnimation()
	{
		symbol.ForEach (x => x.enabled = false);
		animObj.enabled = true;
		AnimSprite.spriteName = SymNum [target - 1] + "01";
		symbolAnim.namePrefix = SymNum[target - 1];
		symbolAnim.ResetToBeginning ();
		symbolAnim.Play ();
	}

	public void PlayDiamondAnimation()
	{
		if (target != 9)
			return;
		symbol.ForEach (x => x.enabled = false);
		animObj.enabled = true;
		AnimSprite.spriteName = SymNum [target - 1] + "01";
		symbolAnim.namePrefix = SymNum[target - 1];
		symbolAnim.ResetToBeginning ();
		symbolAnim.Play ();
	}

	public void StopSymbolAnimation()
	{
		symbol.ForEach (x => x.enabled = true);
		animObj.enabled = false;
		symbolAnim.Pause ();
	}
		
	public void StartRandomSpin ()
	{
		if (StartSpin != null)
			StartSpin ();
		state = State.Spinning;
	}
		
	public void StopSpin ()
	{
		if (StopSource != null && SlotManager.lastState != SlotManager.State.excited)
			AudioSource.PlayClipAtPoint (StopSource,this.transform.position);
		if(state == State.Spinning)
			state = State.Stopping;
	}
}
