using UnityEngine;
using System.Collections;

public class JackPotAnim : MonoBehaviour {
	public GameObject jackpot;
	UISprite sprite;
	UISpriteAnimation spriteAnim;
	public bool ss;
	static JackPotAnim _instance;
	public static JackPotAnim instance{
		get{
			return _instance;
		}
	}



	// Use this for initialization
	void Start () {
		_instance = gameObject.GetComponent<JackPotAnim> ();
		sprite = jackpot.GetComponent<UISprite> ();
		spriteAnim = jackpot.GetComponent<UISpriteAnimation> ();
		sprite.enabled = false;
		spriteAnim.Pause ();

	}
	
	// Update is called once per frame
	void Update () {
		if(sprite.spriteName == "JP22")
			sprite.enabled = false;
		if (ss) {
			ss = false;
			play ();
		}
	}

	public void play()
	{
		sprite.enabled = true;
		sprite.spriteName = "JP1";
		StartCoroutine (SpritePlay());
//		spriteAnim.ResetToBeginning ();
//		spriteAnim.Play ();

	}
	IEnumerator SpritePlay(){
		for (int i = 1; i <= 22; i++) {
			sprite.spriteName = "JP" + i.ToString ();
			yield return new WaitForSeconds (0.06f);
		}

	}
}
