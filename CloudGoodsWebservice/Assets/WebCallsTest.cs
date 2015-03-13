using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using CloudgoodsClasses;

namespace WebCallTests
{
    public class WebCallsTest : MonoBehaviour
    {
        string DebugDisplay = "";
        string Last = "";

        public enum ItemAction
        {
            moveToVault,
            UpdateStackByFive,
            UpdateStackToOne,
            RemoveStack,
            None
        }

        public ItemAction CurrentAction;

        private List<ItemData> UsersItems = new List<ItemData>();
        private List<CreateItemVouchersResponse.ItemVoucher> CurrentVouchers = new List<CreateItemVouchersResponse.ItemVoucher>();

        void OnEnable()
        {
            CloudGoods.CloudGoodsInitilized += Instance_CloudGoodsInitilized;
        }

        void Start()
        {
            CloudGoods.Initialize();
        }

        void Instance_CloudGoodsInitilized()
        {
            CloudGoods.Login(CloudGoodsPlatform.SocialPlay, "0", "lionel.sy@gmail.com", "123456", OnReceivedUser);

        }

        void OnReceivedUser(CloudGoodsUser user)
        {
            string debugString = "login Info\nName: " + user.UserName;
            debugString += "\nId: " + user.UserID;
            debugString += "\nEmail: " + user.UserEmail;
            debugString += "\nIs New: " + user.IsNewUserToWorld;
            debugString += "\nSession:" + user.SessionID.ToString();
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

            CloudGoods.UpdateItemsByIds(infos, DisplayUpdatedItems);


        }


        private void CreateItemVoucher()
        {
            CloudGoods.CreateItemVouchers(1, 700, delegate(CreateItemVouchersResponse response)
            {
                string debugString = "Created Vouchers Items";
                foreach (var voucher in response.Vouchers)
                {
                    CurrentVouchers.Add(voucher);
                    debugString += "\n(" + voucher.Id + ")" + voucher.Item.Name;
                }
                NewDisplayLine(debugString);
            });
        }

        private void ConsumeItemVoucher(CreateItemVouchersResponse.ItemVoucher voucher)
        {

            List<RedeemItemVouchersRequest.ItemVoucherSelection> selectedVouchers = new List<RedeemItemVouchersRequest.ItemVoucherSelection>() { new RedeemItemVouchersRequest.ItemVoucherSelection() { Amount = voucher.Item.Amount, ItemId = voucher.Item.Id, Location = 0, VoucherId = voucher.Id } };

            CloudGoods.RedeemItemVouchers(selectedVouchers, delegate(RedeemItemVouchersResponse response)
            {
                string debugString = "Consume Item Voucher";
                foreach (var result in response.results)
                {
                    debugString += "\n" + result.StackLocationId + ":" + result.ItemId + "{" + result.Amount + "}";
                }

                NewDisplayLine(debugString);
            });

        }

        void LoadUserItems()
        {
            CloudGoods.GetUserItems(0, delegate(List<ItemData> items)
            {
                string debugString = "Recived Items";
                UsersItems.Clear();
                foreach (ItemData item in items)
                {
                    debugString += "\nName: " + item.Name;
                    debugString += "\n    Amount:" + item.Amount;
                    debugString += "\n    Id: " + item.Id;
                    debugString += "\n    Location: " + item.Location.ToString();
                    debugString += "\n    Detail:" + item.Detail;
                    debugString += "\n    SLID:" + item.StackLocationId + "\n";
                    UsersItems.Add(item);
                }
                NewDisplayLine(debugString);
            });
        }


        private void DisplayUpdatedItems(UpdatedStacksResponse response)
        {
            string debugString = "Update Items";
            foreach (var item in response.UpdatedStackIds)
            {
                debugString += "\n" + item.StackId;
                debugString += "\n  Amount: " + item.Amount;
                debugString += "\n  Location: " + item.Location;

                ItemData data = UsersItems.Find(x => x.StackLocationId == item.StackId);
                if (data != null)
                {
                    data.Amount = item.Amount;
                    data.Location = item.Location;
                }
            }
            NewDisplayLine(debugString);
        }

        private void PerformCurrentAction(ItemData item)
        {
            switch (CurrentAction)
            {
                case ItemAction.moveToVault:
                    CloudGoods.MoveItem(item, 0, item.Amount, DisplayUpdatedItems);
                    break;
                case ItemAction.UpdateStackByFive:
              
                    break;
                case ItemAction.UpdateStackToOne:
                    break;
                case ItemAction.RemoveStack:
                    break;
                default:
                    break;
            }
        }

        void OnGUI()
        {
            drawLeft();
            GUILayout.BeginArea(new Rect(Screen.width / 2, 0, Screen.width, Screen.height));
            Color orig = GUI.color;
            GUI.color = Color.green;
            GUILayout.TextArea(Last);
            GUI.color = orig;
            GUILayout.TextField(DebugDisplay);
            GUILayout.EndArea();
        }

        void drawLeft()
        {
            GUILayout.BeginArea(new Rect(25, 25, Screen.width / 2 - 50, Screen.height - 50));
            if (CloudGoods.User != null)
            {
                GUILayout.Label("Welcome " + CloudGoods.User.UserName + ".");
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
            if (GUILayout.Button(CurrentAction.ToReadable()))
            {
                CurrentAction = CurrentAction.Next();
            }
            foreach (ItemData item in UsersItems)
            {
                if (GUILayout.Button(string.Format("{0}:{1} - {2}", item.Name, item.Amount, item.Location)))
                {
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.Label("Item Vouchers");
            for (int i = CurrentVouchers.Count < 3 ? 0 : CurrentVouchers.Count - 3; i < CurrentVouchers.Count; i++)
            {
                if (GUILayout.Button(string.Format("({0}) {1} : {2}", CurrentVouchers[i].Id, CurrentVouchers[i].Item.Id, CurrentVouchers[i].Item.Amount)))
                {
                    ConsumeItemVoucher(CurrentVouchers[i]);
                    CurrentVouchers.Remove(CurrentVouchers[i]);
                    return;
                }
            }
            GUILayout.EndArea();
        }

        void NewDisplayLine(string line)
        {
            DebugDisplay = Last + "\n" + DebugDisplay;
            Last = line;
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
            action += 1;
            if (action == WebCallsTest.ItemAction.None)
            {
                return 0;
            }
            return action + 1;
        }

    }

}
