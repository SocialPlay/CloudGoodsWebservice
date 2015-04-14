using UnityEngine;
using System.Collections;
using CloudGoods.Services.WebCommunication;

public class CallHandlerInit : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        CallHandler.Initialize();
	}
	
}
