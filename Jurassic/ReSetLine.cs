using UnityEngine;
using System.Collections;

public class ReSetLine : MonoBehaviour {
	TweenAlpha line;

	void OnEnable() {
		if (line == null)
			line = this.gameObject.GetComponent<TweenAlpha> ();
		line.ResetToBeginning ();
	}
}
