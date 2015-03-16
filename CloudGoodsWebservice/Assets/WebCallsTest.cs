using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using CloudGoods;
using CloudGoods.Models;
using CloudGoods.Emuns;

namespace WebCallTests
{
    public class WebCallsTest : MonoBehaviour
    {
        string DebugDisplay = "";
        string Last = "";

        public enum ItemAction
        {
            move,
            UpdateStackByFive,
            UpdateStackToOne,
            RemoveStack,
            None
        }

        public ItemAction CurrentAction;
        public static int destinationLocation = 0;

        private List<ItemData> UsersItems = new List<ItemData>();
        private List<ItemVouchersResponse.ItemVoucher> CurrentVouchers = new List<ItemVouchersResponse.ItemVoucher>();

        void OnEnable()
        {
            CallHandler.CloudGoodsInitilized += Instance_CloudGoodsInitilized;
        }

        void Start()
        {
            CallHandler.Initialize();
        }

        void Instance_CloudGoodsInitilized()
        {
            CallHandler.Login(CloudGoodsPlatform.SocialPlay, "0", "lionel.sy@gmail.com", "123456", OnReceivedUser);

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

            CallHandler.UpdateItemsByIds(infos, DisplayUpdatedItems);


        }

        private void CreateItemVoucher()
        {
            CallHandler.CreateItemVouchers(1, 700, DisaplayItemVouchers);

        }



        private void RedeemItemVoucher(ItemVouchersResponse.ItemVoucher voucher)
        {

            List<RedeemItemVouchersRequest.ItemVoucherSelection> selectedVouchers = new List<RedeemItemVouchersRequest.ItemVoucherSelection>() { new RedeemItemVouchersRequest.ItemVoucherSelection() { Amount = voucher.Item.Amount, ItemId = voucher.Item.Id, Location = 0, VoucherId = voucher.Id } };

            CallHandler.RedeemItemVouchers(selectedVouchers, DisplayUpdatedItems);

        }

        void LoadUserItems()
        {
            CallHandler.GetUserItems(0, delegate(List<ItemData> items)
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

            }
            NewDisplayLine(debugString);
            foreach (var item in response.UpdatedStackIds)
            {
                ItemData data = UsersItems.Find(x => x.StackLocationId == item.StackId);
                if (data != null)
                {
                    data.Amount = item.Amount;
                    data.Location = item.Location;
                }
                else
                {
                    UsersItems.Add(new ItemData()
                      {
                          Name = "---- Needs refresh -----",
                          StackLocationId = item.StackId,
                          Location = item.Location,
                          Amount = item.Amount
                      });
                }
            }

        }

        public void DisaplayItemVouchers(ItemVouchersResponse response)
        {
            string debugString = "Vouchers Items";
            foreach (var voucher in response.Vouchers)
            {
                CurrentVouchers.Add(voucher);
                debugString += "\n(" + voucher.Id + ")" + voucher.Item.Name;
            }
            NewDisplayLine(debugString);
        }

        private void PerformCurrentAction(ItemData item)
        {
            switch (CurrentAction)
            {
                case ItemAction.move:
                    CallHandler.MoveItem(item, destinationLocation, item.Amount, DisplayUpdatedItems);
                    UsersItems.Remove(item);
                    break;
                case ItemAction.UpdateStackByFive:
                    CallHandler.UpdateItemByStackIds(item.StackLocationId, 5, item.Location, DisplayUpdatedItems);
                    if (item.Amount < 5)
                        UsersItems.Remove(item);
                    else
                    {
                        item.Amount -= 5;
                    }
                    break;
                case ItemAction.UpdateStackToOne:
                    CallHandler.UpdateItemByStackIds(item.StackLocationId, -(item.Amount - 1), item.Location, DisplayUpdatedItems);
                    item.Amount = 1;
                    break;
                case ItemAction.RemoveStack:
                    CallHandler.UpdateItemByStackIds(item.StackLocationId, -item.Amount, item.Location, DisplayUpdatedItems);
                    UsersItems.Remove(item);
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
            if (CallHandler.User != null)
            {
                GUILayout.Label("Welcome " + CallHandler.User.UserName + ".");
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
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(CurrentAction.ToReadable()))
            {
                CurrentAction = CurrentAction.Next();
            }

            if (CurrentAction == ItemAction.move)
            {
                if (GUILayout.Button((destinationLocation == 0 ? "(Vault)" : destinationLocation.ToString()), GUILayout.MaxWidth(140)))
                {
                    destinationLocation++;
                    if (destinationLocation > 10)
                    {
                        destinationLocation = 0;
                    }
                }
            }

            GUILayout.EndHorizontal();
            foreach (ItemData item in UsersItems)
            {

                if (GUILayout.Button(string.Format("{0}\nSLID:{3}\n  Amount:{1}\n  Location:{2}", item.Name, item.Amount, item.Location, item.StackLocationId)))
                {
                    PerformCurrentAction(item);
                    return;
                }


            }

            GUILayout.FlexibleSpace();
         
            GUILayout.Label(string.Format("Item Vouchers ({0})", CurrentVouchers.Count));
            for (int i = CurrentVouchers.Count < 3 ? 0 : CurrentVouchers.Count - 3; i < CurrentVouchers.Count; i++)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(string.Format("({0}) {1} : {2}", CurrentVouchers[i].Id, CurrentVouchers[i].Item.Id, CurrentVouchers[i].Item.Amount)))
                {
                    RedeemItemVoucher(CurrentVouchers[i]);
                    CurrentVouchers.Remove(CurrentVouchers[i]);
                    return;
                }
                if (GUILayout.Button("Get", GUILayout.MaxWidth(100)))
                {
                    CallHandler.GetItemVoucher(CurrentVouchers[i].Id, DisaplayItemVouchers);
                }
                GUILayout.EndHorizontal();
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
                case WebCallsTest.ItemAction.move:
                    return "Move to";
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
            WebCallsTest.destinationLocation = 0;
            if (action == WebCallsTest.ItemAction.None)
            {
                return 0;
            }
            return action;
        }

    }

}
