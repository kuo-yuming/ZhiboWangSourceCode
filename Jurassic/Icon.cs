using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Icon : MonoBehaviour {

	public UISprite iconUI;
	public UISpriteAnimation iconAnim;
	public List<string> boxType = new List<string>();
	public bool isCombo = false;



	public void SetBox()
	{
		if (SlotManager.slotState != SlotManager.State.freeGame) {
			iconUI.spriteName = boxType [0] + "1";
			iconAnim.namePrefix = boxType [0];
			iconAnim.loop = true;
		} else {
			iconUI.spriteName = boxType [1] + "18";
			iconAnim.namePrefix = boxType [1];
			iconAnim.loop = false;
		}
		iconAnim.ResetToBeginning ();
	}

	public void LockPlay()
	{
		iconAnim.ResetToBeginning ();
		iconAnim.Play ();
		//iconAnim.ResetToBeginning ();
	}

	void OnEnable()
	{
		if (isCombo) {
			iconUI.spriteName = boxType [1] + "18";
			iconAnim.namePrefix = boxType [1];
			iconAnim.loop = true;
		} else {
			iconUI.spriteName = boxType [0] + "1";
			iconAnim.namePrefix = boxType [0];
			iconAnim.loop = false;
		}
	}
}
