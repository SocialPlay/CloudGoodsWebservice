using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using CloudgoodsClasses;

namespace WebCallTests
{
    public class WebCallsTest : MonoBehaviour
    {

        string debugDisplay = "";
        string last = "";
        WebAPICallObjectCreator objectCreator;

        public enum ItemAction
        {
            moveToVault,
            UpdateStackByFive,
            UpdateStackToOne,
            RemoveStack
        }

        public ItemAction currentAction;

        private List<ItemData> usersItems = new List<ItemData>();
        private List<CreateItemVouchersResponse.ItemVoucher> currentVouchers = new List<CreateItemVouchersResponse.ItemVoucher>();

        void OnEnable()
        {
            CloudGoods.CloudGoodsInitilized += Instance_CloudGoodsInitilized;
        }

        void Start()
        {
            CloudGoods.Initialize();
            objectCreator = new WebAPICallObjectCreator();
        }

        void Instance_CloudGoodsInitilized()
        {
            CloudGoods.Login(CloudGoodsPlatform.SocialPlay, "0", "lionel.sy@gmail.com", "123456", OnReceivedUser);

        }

        void OnReceivedUser(CloudGoodsUser user)
        {

            string debugString = "login Info\nName: " + user.userName;
            debugString += "\nId: " + user.userID;
            debugString += "\nEmail: " + user.userEmail;
            debugString += "\nIs New: " + user.isNewUserToWorld;
            debugString += "\nSession:" + user.sessionID.ToString();
            NewDisplayLine(debugString);

        }


        private void UpdateItemById()
        {
            List<UpdateItemByIdRequest.UpdateOrderByID> infos = new List<UpdateItemByIdRequest.UpdateOrderByID>(){
            new UpdateItemByIdRequest.UpdateOrderByID(){
             location = 0, itemId = 111542, amount = 5
            }, new UpdateItemByIdRequest.UpdateOrderByID(){
             location = 6, itemId = 111542, amount = 2
            }
        };

            CloudGoods.UpdateItemsByIds(infos
                , delegate(UpdatedStacksResponse response)
                {
                    string debugString = "Update Items";
                    foreach (var item in response.updatedStackIds)
                    {
                        debugString += "\n" + item;
                    }
                    NewDisplayLine(debugString);
                });

        }

        private void CreateItemVoucher()
        {
            CloudGoods.CreateItemVouchers(1, 700, delegate(CreateItemVouchersResponse response)
            {
                string debugString = "Created Vouchers Items";
                foreach (var voucher in response.vouchers)
                {
                    currentVouchers.Add(voucher);
                    debugString += "\n(" + voucher.Id + ")" + voucher.item.name;
                }
                NewDisplayLine(debugString);
            });
        }

        private void ConsumeItemVoucher(CreateItemVouchersResponse.ItemVoucher voucher)
        {

            List<RedeemItemVouchersRequest.ItemVoucherSelection> selectedVouchers = new List<RedeemItemVouchersRequest.ItemVoucherSelection>() { new RedeemItemVouchersRequest.ItemVoucherSelection() { amount = voucher.item.amount, itemId = voucher.item.Id, location = 0, voucherId = voucher.Id } };

            CloudGoods.RedeemItemVouchers(selectedVouchers, delegate(RedeemItemVouchersResponse response)
            {
                string debugString = "Consume Item Voucher";
                foreach (var result in response.results)
                {
                    debugString += "\n" + result.stackLocationId + ":" + result.itemId + "{" + result.amount + "}";
                }

                NewDisplayLine(debugString);
            });

        }

        void LoadUserItems()
        {
            CloudGoods.GetUserItems(0, delegate(List<ItemData> items)
            {
                string debugString = "Recived Items";
                usersItems.Clear();
                foreach (ItemData item in items)
                {
                    debugString += "\nName: " + item.name;
                    debugString += "\n    Amount:" + item.amount;
                    debugString += "\n    Id: " + item.Id;
                    debugString += "\n    Location: " + item.location.ToString();
                    debugString += "\n    Detail:" + item.detail;
                    debugString += "\n    SLID:" + item.stackLocationId + "\n";
                    usersItems.Add(item);
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
            GUILayout.TextArea(last);
            GUI.color = orig;
            GUILayout.TextField(debugDisplay);
            GUILayout.EndArea();
        }

        void drawLeft()
        {
            GUILayout.BeginArea(new Rect(25, 25, Screen.width / 2 - 50, Screen.height - 50));
            if (CloudGoods.User != null)
            {
                GUILayout.Label("Welcome " + CloudGoods.User.userName + ".");
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
                UpdateItemById();
            }
            if (GUILayout.Button("Create Item Voucher"))
            {
                CreateItemVoucher();
            }
            GUILayout.FlexibleSpace();
            GUILayout.Label("Users Items");
            if (GUILayout.Button(currentAction.ToReadable()))
            {
                currentAction = currentAction.Next();
            }
            foreach (ItemData item in usersItems)
            {
                if (GUILayout.Button(string.Format("{0}:{1} - {2}", item.name, item.amount, item.location)))
                {
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.Label("Item Vouchers");
            for (int i = currentVouchers.Count < 3 ? 0 : currentVouchers.Count - 3; i < currentVouchers.Count; i++)
            {
                if (GUILayout.Button(string.Format("({0}) {1} : {2}", currentVouchers[i].Id, currentVouchers[i].item.Id, currentVouchers[i].item.amount)))
                {
                    ConsumeItemVoucher(currentVouchers[i]);
                    currentVouchers.Remove(currentVouchers[i]);
                    return;
                }
            }
            GUILayout.EndArea();
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

    public static class Utils
    {
        public static string ToReadable(this WebCallsTest.ItemAction action)
        {
            switch (action)
            {
                case WebCallsTest.ItemAction.moveToVault:
                    return "Move to vault(0)";
                case WebCallsTest.ItemAction.UpdateStackByFive:
                    return "Add 5 to amount";
                case WebCallsTest.ItemAction.UpdateStackToOne:
                    return "Chnage amount to One";
                case WebCallsTest.ItemAction.RemoveStack:
                    return "Remove Stack";
                default:
                    return "Unknow Action";
            }
        }
        public static WebCallsTest.ItemAction Next(this WebCallsTest.ItemAction action)
        {
            if ((int)action == 3)
            {
                return WebCallsTest.ItemAction.moveToVault;
            }
            return action + 1;
        }

    }

}
