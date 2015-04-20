using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamScript : MonoBehaviour {


	// Use this for initialization
	void Start () {
        if (SteamManager.Initialized)
        {
            string name = SteamFriends.GetPersonaName();
            
            Debug.Log(name);
        }
	}
	
}
