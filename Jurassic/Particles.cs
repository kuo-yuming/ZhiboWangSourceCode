using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Particles : MonoBehaviour {
	public List<GameObject> Stone = new List<GameObject> ();
	public GameObject Fire;
	static Particles _instance;

	public static Particles instance{
		get{ return _instance;}
	}
	// Use this for initialization
	void Start () {
		_instance = gameObject.GetComponent<Particles>();
	}

	public void PlayStone()
	{
		foreach (var obj in Stone) {
			obj.SetActive (true);
		}
	}
	public void PlayFire()
	{
		Fire.SetActive (true);
	}
	public void StopParticle()
	{
		foreach (var obj in Stone) {
			obj.SetActive (false);
		}
		Fire.SetActive (false);
	}

}
