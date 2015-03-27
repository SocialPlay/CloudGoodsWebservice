using UnityEngine;
using System.Collections;
using CloudGoods;

public class CallHandlerInit : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        CallHandler.Initialize();
	}
	
}
