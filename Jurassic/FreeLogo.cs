using UnityEngine;
using System.Collections;

public class FreeLogo : MonoBehaviour {
	UISprite logo;
	UISpriteAnimation logoani;
	public float pauseTime = 2.0f;

	void Awake () {
		logo = gameObject.GetComponent<UISprite> ();
		logoani = gameObject.GetComponent<UISpriteAnimation> ();
	}

	void Update () {
		if (logo.spriteName == "FreeLogo_30" && logoani.isPlaying) {
			StartCoroutine (Delay ());
		}
	}

	void OnDisable()
	{
		logoani.ResetToBeginning ();
	}

	IEnumerator Delay()
	{
		logoani.Pause ();
		yield return new WaitForSeconds (pauseTime);
		logoani.ResetToBeginning ();
	}
}
