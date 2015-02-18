using UnityEngine;
using System.Collections;
using NUnit.Framework;
using LitJson;

public class TestLitJsonErrorTest : MonoBehaviour {

    ResponseCreator responseCreator = new LitJsonResponseCreator();

    void Awake()
    {
        responseCreator.ResponseHasError += responseCreator_ResponseHasError;
    }

    void responseCreator_ResponseHasError(WebserviceError obj)
    {
        Debug.Log("Error has occured, Error Code: " + obj.ErrorCode + "  Error Message: " + obj.ErrorMessage);
    }

	// Use this for initialization
	void Start () {

        RunTestCallbackNoError();

	}

    
    void RunTestCallbackNoError()
    {
        TestLoginResponse testLoginResponse = new TestLoginResponse(01, "01", "Test User", "test@email.com", "ABCD1234");

        string jsonString = JsonMapper.ToJson(testLoginResponse);

        Debug.Log("jsonString for non error: " + jsonString);

        UserResponse userResponse = responseCreator.CreateLoginResponse(jsonString);
        
        
    }
}
