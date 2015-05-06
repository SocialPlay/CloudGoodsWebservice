using UnityEngine;
using System.Collections;

public class AndroidTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AndroidJNIHelper.debug = true;
        using (AndroidJavaClass jc = new AndroidJavaClass("com.example.unitycurrencypurchase"))
        {
            jc.CallStatic("SendMessageToUnity", "Test Message");
        }
	}

    void ReceivedFromJava(string message)
    {
        Debug.Log("Message from java: " + message);
    }
}
