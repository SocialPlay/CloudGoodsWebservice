using UnityEngine;
using System.Collections;

public interface CallObjectCreator  {

    WWW CreateLoginCallObject(string appID, string userName, string userEmail, string password);
}
