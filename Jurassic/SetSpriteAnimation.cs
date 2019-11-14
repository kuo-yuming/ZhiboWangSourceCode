using UnityEngine;
using System.Collections;

public class SetSpriteAnimation : MonoBehaviour {
	public AtlasLoad atlas;
	public UISpriteAnimation anim;
	// Use this for initialization
	void Start () {
		atlas = gameObject.GetComponent<AtlasLoad> ();
		anim = gameObject.GetComponent<UISpriteAnimation> ();
	}
		

	void Update() {
		if(atlas.Seat){
			anim.enabled = true;
			Destroy (this);
		}
	}
}
