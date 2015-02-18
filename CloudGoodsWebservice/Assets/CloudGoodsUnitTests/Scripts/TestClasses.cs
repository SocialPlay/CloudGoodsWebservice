using UnityEngine;
using System.Collections;

public class TestError
{

    public int errorCode;
    public string errorMessage;
}

public class TestLoginResponse
{
    public TestLoginResponse(int newSessionId, string newUserId, string newUserName, string newEmail, string newSignature)
    {
        SessionId = newSessionId;
        UserId = newUserId;
        UserName = newUserName;
        Email = newEmail;
        Signature = newSignature;
    }

    public int SessionId;
    public string UserId;
    public string UserName;
    public string Email;
    public string Signature;
}