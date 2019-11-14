using UnityEngine;
using System.Collections;

public class BaccaratLobby_Control : MonoBehaviour {
    public GameObject[] LobbyObject;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (VersionDef.CN_LogInPack)
        {
            LobbyObject[0].SetActive(false);
            LobbyObject[1].SetActive(true);
        }
        else
        {
            LobbyObject[0].SetActive(true);
            LobbyObject[1].SetActive(false);
        }
	}
}
