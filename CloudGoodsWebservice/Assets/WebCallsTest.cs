using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class WebCallsTest : MonoBehaviour
{

    string debugDisplay;
    string last = "";
    WebAPICallObjectCreator objectCreator;

    void OnEnable()
    {
        CloudGoods.Instance.CloudGoodsInitilized += Instance_CloudGoodsInitilized;
    }

    void Start()
    {
        CloudGoods.Instance.Initialize();
        objectCreator = new WebAPICallObjectCreator();
    }

    void Instance_CloudGoodsInitilized()
    {
        CloudGoods.Instance.Login(CloudGoodsPlatform.SocialPlay, "0", "lionel.sy@gmail.com", "123456", OnReceivedUser);

    }

    void OnReceivedUser(CloudGoodsUser user)
    {

        string debugString = "login Info\nName: " + user.userName;
        debugString += "\nId: " + user.UserID;
        debugString += "\nEmail: " + user.userEmail;
        debugString += "\nIs New: " + user.isNewUserToWorld;
        debugString += "\nSession:" + user.sessionID.ToString();
        NewDisplayLine(debugString);

    }


    private void GiveUserItem()
    {
        RequestValues requestV = new RequestValues()
        {
            items = new ItemInfo[2] { 
                new ItemInfo() { Id = 111542, location = 0, amount = 2 }, 
                new ItemInfo() { Id = 111542, location = 1, amount = 3 } 
            }
        };

        StartCoroutine(DebugResults(objectCreator.GenerateWWWPost("GiveOwnerItems", requestV)));
    }



    void LoadUserItems()
    {
        CloudGoods.Instance.GetUserItems(delegate(List<ItemData> items)
        {
            string debugString = "Recived Items";
            foreach (ItemData item in items)
            {
                debugString += "\nName: " + item.Name;
                debugString += "\n    Amount:" + item.Amount;
                debugString += "\n    Id: " + item.Id;
                debugString += "\n    Location: " + item.Location.ToString();
                debugString += "\n    Detail:" + item.Detail + "\n";
            }

            NewDisplayLine(debugString);
        });
    }


    void OnGUI()
    {
        drawLeft();
        GUILayout.BeginArea(new Rect(Screen.width / 2, 0, Screen.width, Screen.height));
        Color orig = GUI.color;
        GUI.color = Color.green;
        GUILayout.Label(last);
        GUI.color = orig;
        GUILayout.Label(debugDisplay);
        GUILayout.EndArea();
    }

    void drawLeft()
    {
        GUILayout.BeginArea(new Rect(25, 25, Screen.width / 2 - 50, Screen.height - 50));
        if (CloudGoods.Instance.User != null)
        {
            GUILayout.Label("Welcome " + CloudGoods.Instance.User.userName + ".");
        }
        else
        {
            GUILayout.Label("Waiting for login");
            GUILayout.EndArea();
            return;
        }
        if (GUILayout.Button("Load users Items"))
        {
            LoadUserItems();
        }

        if (GUILayout.Button("Give User Item"))
        {
            GiveUserItem();
        }

        GUILayout.EndArea();
    }


    public class ItemInfo
    {
        public int Id { get; set; }
        public int amount { get; set; }
        public int location { get; set; }
    }

    public class RequestValues
    {
        public ItemInfo[] items { get; set; }
        public string ownerType = "User";
        public int otherOwner = -1;
    }


    IEnumerator DebugResults(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            NewDisplayLine(www.text);
            Debug.LogWarning(www.text);
        }
        else
        {
            NewDisplayLine(www.error);
        }
    }

    void NewDisplayLine(string line)
    {
        debugDisplay = last + "\n" + debugDisplay;
        last = line;
    }

}
