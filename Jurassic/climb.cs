using UnityEngine;
using System.Collections;

public class climb : MonoBehaviour {
	public Reel reel;
	public UISprite sprite;
	public GameObject symbol;
	float currentHeight = 120f;
	float currentSpeed;
	const float spinSpeed = -5f;
	bool target;

	void Start() {
		target = true;
		sprite.spriteName = reel.RandomSymbol ();
		Reel.StartSpin += OnSpin;
	}

	void OnDiestory()
	{
		Reel.StartSpin -= OnSpin;
	}
		
	void Update () {
		
		switch (reel.state) {
		case Reel.State.Idle:
			break;
		case Reel.State.Spinning:
			currentSpeed = Mathf.Lerp (currentSpeed, spinSpeed, Time.deltaTime);
			transform.Translate (0f, currentSpeed * Time.deltaTime, 0f, Space.Self);
			if (this.transform.localPosition.y < -reel.rowHeight) {
				transform.localPosition = new Vector3 (0f, reel.rowHeight, 0f);
				sprite.spriteName = reel.RandomSymbol ();
			}
			break;
		case Reel.State.Stopping:
			currentSpeed = Mathf.Lerp (currentSpeed, 0.5f, Time.deltaTime);
			transform.Translate (0f, currentSpeed * Time.deltaTime, 0f, Space.Self);
			if (target) {
				if (this.transform.localPosition.y < -reel.rowHeight) {
					transform.localPosition = new Vector3 (0f, reel.rowHeight, 0f);
					sprite.spriteName = reel.RandomSymbol ();
					target = false;
				}
			} else if (this.transform.localPosition.y <= 2f) {
					transform.localPosition = new Vector3 (0f, 0f, 0f);
					symbol.transform.localPosition = new Vector3 (0f, 138f, 0f);
					reel.state = Reel.State.Idle;
				}
			
			break;
		default:
			break;
		}
	}

	public void OnSpin()
	{
		target = true;
		currentSpeed = -3f;
		reel.state = Reel.State.Spinning;
	}
}
