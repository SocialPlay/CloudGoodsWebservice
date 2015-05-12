using UnityEngine;
using System.Collections;

public class AndroidConnect : MonoBehaviour {

    AndroidJavaClass jc;

    string androidKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAi81cAx8/D6EI2Q8ldAMxz+S9RWcHYhP6STFi6XwqLBb0pjyHVcBdCyCpUVvG+Ss6ngRlVLXrQCBP/knI2CRsT3MqFiafPFuyABPWIMoOytH6sN8jmZE/Hek7UOu3p4yJDEgza52A+ZGls8nDPbEHoz9Gk9QlOTukN5eOEVe2scrldQOPIh42JdwjKFLh3klqYkKLzQQyExM0VC0vKjqHxpJi65qLgkpqJvhJpERRcw6gAGnNiXw6dW2kZKQYI48sVSi/jtwKr+YqSQaSg0Y3LpJVCsS86qdStaZAOiMp5biYYLRkcyd9wzrDz6CR5VJbjSZ3cXEOJ8uLtzyx0ia2twIDAQAB";
	void Start () {

        jc = new AndroidJavaClass("com.example.unityandroidpremiumpurchase.AndroidPurchaser");

        using (AndroidJavaClass cls = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj_Activity = cls.GetStatic<AndroidJavaObject>("currentActivity"))
            {

                Debug.Log("Calling androidpurchas init");
                jc.CallStatic("InitAndroidPurchaser", obj_Activity, androidKey);

            }
        }
	}

    public void PurchaseAndroidBundle(string bundleName)
    {
        using (AndroidJavaClass cls = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj_Activity = cls.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                jc.CallStatic("PurchasePremiumCurrencyBundle", obj_Activity, "socialplay_item.2");

            }
        }
    }

    public void RecieveFromJava(string msg)
    {
        Debug.Log("received from java: " + msg);
    }

    public void ErrorFromAndroid(string msg)
    {
        Debug.LogError("Error Message From Java:" + msg);
    }
}
