using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using CloudGoods;
using CloudGoods.Models;
using CloudGoods.Enums;
using System.Linq;
using System;

namespace WebCallTests
{

    public class WebCallsTest : MonoBehaviour
    {

        public enum SystemTabs
        {
            BaseItems,
            ItemVouchers,
            Store,
            ItemBundles
        }

        public enum ItemAction
        {
            move,
            UpdateStackByFive,
            UpdateStackToOne,
            RemoveStack,
            None
        }
        string title = "";
        private static SystemTabs activeTab = SystemTabs.BaseItems;
        public int voucherModifyamount = 0;


        public static ItemAction CurrentAction;



        void OnEnable()
        {
            CallHandler.CloudGoodsInitilized += Instance_CloudGoodsInitilized;
        }

        void Start()
        {
            CallHandler.Initialize();
            title = "Basic item calls";
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
            DisplayHelper.NewDisplayLine(debugString);
        }

        private void UpdateItemById()
        {
            List<UpdateItemByIdRequest.UpdateOrderByID> infos = new List<UpdateItemByIdRequest.UpdateOrderByID>(){
                new UpdateItemByIdRequest.UpdateOrderByID(){location = 0, itemId = 111542, amount = 5},
                new UpdateItemByIdRequest.UpdateOrderByID(){location = 6, itemId = 111542, amount = 2}
            };

            CallHandler.UpdateItemsByIds(infos, ItemManagerCalls.DisplayUpdatedItems);
        }


        private void PerformCurrentAction(ItemData item)
        {
            switch (CurrentAction)
            {
                case ItemAction.move:
                    ItemManagerCalls.MoveItem(item);
                    break;
                case ItemAction.UpdateStackByFive:
                    ItemManagerCalls.AddFiveToStack(item);
                    break;
                case ItemAction.UpdateStackToOne:
                    ItemManagerCalls.MakeStackSizeOne(item);
                    break;
                case ItemAction.RemoveStack:
                    ItemManagerCalls.RemoveStack(item);
                    break;
                default:
                    break;
            }
        }





        void OnGUI()
        {
            drawLeft();
            DisplayHelper.DrawRight();
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

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Basic Items"))
            {
                activeTab = SystemTabs.BaseItems;
                title = "Basic Item Calls";
            }
            if (GUILayout.Button("Item voucher"))
            {
                activeTab = SystemTabs.ItemVouchers;
                title = "Item Voucher Calls";
            }
            if (GUILayout.Button("Store"))
            {
                activeTab = SystemTabs.Store;
                title = "Store Calls";
            }
            if (GUILayout.Button("Item Bundle"))
            {
                activeTab = SystemTabs.ItemBundles;
                title = "Item Bundle Calls";
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label(title);
            switch (activeTab)
            {
                case SystemTabs.BaseItems:
                    DisplayBaseItemCalls();
                    break;
                case SystemTabs.ItemVouchers:
                    DisplayItemVouchersCalls();
                    break;
                case SystemTabs.Store:
                    DisplayStoreCalls();
                    break;
                case SystemTabs.ItemBundles:
                    DisplayItemBundlesCalls();
                    break;
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        private void DisplayItemBundlesCalls()
        {
            if (GUILayout.Button("Get Item Bundles"))
            {
                ItemBundlesCalls.GetItemBundles();
            }
            DrawItemBundlesDetails();
            GUILayout.FlexibleSpace();
            DrawItemDetails();
        }



        private void DisplayStoreCalls()
        {

        }

        private void DisplayItemVouchersCalls()
        {
            if (GUILayout.Button("Create Item Voucher"))
            {
                ItemVouchersCalls.CreateItemVoucher();
            }
            GUILayout.FlexibleSpace();
            DrawItemVouchersDetails();
            GUILayout.FlexibleSpace();
            DrawItemDetails();

        }



        private void DisplayBaseItemCalls()
        {
            if (GUILayout.Button("Load users Items"))
            {
                ItemManagerCalls.LoadUserItems();
            }
            if (GUILayout.Button("Give User Item"))
            {
                UpdateItemById();
            }
            DrawItemDetails();
        }

        private void DrawItemDetails()
        {
            GUILayout.FlexibleSpace();

            GUILayout.Label("Users Items");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(CurrentAction.ToReadable()))
            {
                CurrentAction = CurrentAction.Next();
            }
            if (CurrentAction == ItemAction.move)
            {
                if (GUILayout.Button((ItemManagerCalls.destinationLocation == 0 ? "(Vault)" : ItemManagerCalls.destinationLocation.ToString()), GUILayout.MaxWidth(140)))
                {
                    ItemManagerCalls.destinationLocation++;
                    if (ItemManagerCalls.destinationLocation > 10)
                    {
                        ItemManagerCalls.destinationLocation = 0;
                    }
                }
            }

            GUILayout.EndHorizontal();
            foreach (ItemData item in ItemManagerCalls.UsersItems)
            {
                if (GUILayout.Button(string.Format("{0}\nSLID:{3}\n  Amount:{1}\n  Location:{2}", item.Name, item.Amount, item.Location, item.StackLocationId)))
                {
                    PerformCurrentAction(item);
                    return;
                }
            }
        }

        private void DrawItemVouchersDetails()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(string.Format("Item Vouchers ({0})", ItemVouchersCalls.CurrentVouchers.Count));
            if (GUILayout.Button("+"))
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    voucherModifyamount += 10;
                }
                else
                    voucherModifyamount++;
            }
            GUILayout.Label("Consume: " + (voucherModifyamount == 0 ? "all" : voucherModifyamount.ToString()));
            if (GUILayout.Button("-"))
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    voucherModifyamount -= 10;
                }
                else
                    voucherModifyamount--;
                if (voucherModifyamount < 0) voucherModifyamount = 0;
            }
            GUILayout.EndHorizontal();
            int count = ItemVouchersCalls.CurrentVouchers.Count;
            for (int i = count < 3 ? 0 : count - 3; i < count; i++)
            {
                GUILayout.BeginHorizontal();
                ItemVouchersResponse.ItemVoucher voucher = ItemVouchersCalls.CurrentVouchers[i];
                if (GUILayout.Button(string.Format("({0}) {1} : {2}", voucher.Id, voucher.Item.Id, voucher.Item.Amount)))
                {
                    ItemVouchersCalls.RedeemItemVoucher(voucher);

                    return;
                }
                if (GUILayout.Button("Get", GUILayout.MaxWidth(100)))
                {
                    ItemVouchersCalls.GetItemVoucher(voucher.Id);
                }
                GUILayout.EndHorizontal();
            }
        }

        private void DrawItemBundlesDetails()
        {
            GUILayout.Label(string.Format("Current Item Bundles ({0})",ItemBundlesCalls.bundles.Count));
            foreach (ItemBundlesResponse.ItemBundleInfo info in ItemBundlesCalls.bundles)
            {
                if (GUILayout.Button(string.Format("Buy: {0}\nItems {1}", info.Name,info.items.Count)))
                {
                    ItemBundlesCalls.PurchaseItemBundle(info.Id);
                }
            }
        }
    }


    internal static class DisplayHelper
    {
        static string DebugDisplay = "";
        static string Last = "";

        public static void DrawRight()
        {
            GUILayout.BeginArea(new Rect(Screen.width / 2, 0, Screen.width, Screen.height));
            Color orig = GUI.color;
            GUI.color = Color.green;
            GUILayout.TextArea(Last);
            GUI.color = orig;
            GUILayout.TextField(DebugDisplay);
            GUILayout.EndArea();
        }


        public static void NewDisplayLine(string line)
        {
            DebugDisplay = Last + "\n" + DebugDisplay;
            Last = line;
        }
    }


    internal class ItemManagerCalls
    {
        public static List<ItemData> UsersItems = new List<ItemData>();
        public static int destinationLocation = 0;

        public static void LoadUserItems()
        {
            CallHandler.GetUserItems(0, delegate(List<ItemData> items)
            {
                DisplayItems(items);
            });
        }

        public static void MoveItem(ItemData item)
        {
            CallHandler.MoveItem(item, destinationLocation, item.Amount, DisplayUpdatedItems);
            UsersItems.Remove(item);
        }

        public static void RemoveStack(ItemData item)
        {
            CallHandler.UpdateItemByStackIds(item.StackLocationId, -item.Amount, item.Location, DisplayUpdatedItems);
            UsersItems.Remove(item);
        }

        public static void MakeStackSizeOne(ItemData item)
        {
            CallHandler.UpdateItemByStackIds(item.StackLocationId, -(item.Amount - 1), item.Location, DisplayUpdatedItems);
            item.Amount = 1;
        }

        public static void AddFiveToStack(ItemData item)
        {
            CallHandler.UpdateItemByStackIds(item.StackLocationId, 5, item.Location, DisplayUpdatedItems);
            if (item.Amount < 5)
                UsersItems.Remove(item);
            else
            {
                item.Amount -= 5;
            }
        }

        public static void DisplayItems(List<ItemData> items)
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
            DisplayHelper.NewDisplayLine(debugString);
        }


        public static void DisplayUpdatedItems(UpdatedStacksResponse response)
        {
            string debugString = "Update Items";
            foreach (var item in response.UpdatedStackIds)
            {
                debugString += "\n" + item.StackId;
                debugString += "\n  Amount: " + item.Amount;
                debugString += "\n  Location: " + item.Location;

            }
            DisplayHelper.NewDisplayLine(debugString);
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
    }

    internal class ItemVouchersCalls
    {
        public static List<ItemVouchersResponse.ItemVoucher> CurrentVouchers = new List<ItemVouchersResponse.ItemVoucher>();

        public static void CreateItemVoucher()
        {
            CallHandler.CreateItemVouchers(1, 700, DisaplayItemVouchers);

        }

        public static void RedeemItemVoucher(ItemVouchersResponse.ItemVoucher voucher)
        {

            List<RedeemItemVouchersRequest.ItemVoucherSelection> selectedVouchers = new List<RedeemItemVouchersRequest.ItemVoucherSelection>() { new RedeemItemVouchersRequest.ItemVoucherSelection() { Amount = voucher.Item.Amount, ItemId = voucher.Item.Id, Location = 0, VoucherId = voucher.Id } };

            CallHandler.RedeemItemVouchers(selectedVouchers, delegate(UpdatedStacksResponse response)
            {
                ItemManagerCalls.DisplayUpdatedItems(response);
                CurrentVouchers.Remove(voucher);
            });


        }

        public static void GetItemVoucher(int Id)
        {
            CallHandler.GetItemVoucher(Id, DisaplayItemVouchers);
        }


        static void DisaplayItemVouchers(ItemVouchersResponse response)
        {
            string debugString = "Vouchers Items";
            foreach (var voucher in response.Vouchers)
            {
                var existing = CurrentVouchers.FirstOrDefault(v => v.Id == voucher.Id);
                if (existing != null)
                {
                    existing.Item.Amount = voucher.Item.Amount;
                }
                else
                {
                    CurrentVouchers.Add(voucher);
                }
                debugString += string.Format("{0}\nId: {1}", voucher.Item.Name, voucher.Id);
            }
            DisplayHelper.NewDisplayLine(debugString);
        }
    }

    internal class ItemBundlesCalls
    {
        public static List<ItemBundlesResponse.ItemBundleInfo> bundles = new List<ItemBundlesResponse.ItemBundleInfo>();
        public static void GetItemBundles()
        {
            CallHandler.GetItemBundles("", "", delegate(ItemBundlesResponse response)
            {
                ItemBundleResponseHandler(response);
            });
        }

        private static void ItemBundleResponseHandler(ItemBundlesResponse response)
        {
            string debugstring = "Got item bundles";
            foreach (var bundle in response.bundles)
            {
                string items = "";
                bundle.items.ForEach(i => items = string.Format("{0}\n     {2} {1}", items, i.Name, i.Amount));
                debugstring = string.Format("{0}\n{1}\n   Item count:{2}{3}", debugstring, bundle.Name, bundle.items.Count, items);
                var existingBundle = bundles.FirstOrDefault(b => b.Id == bundle.Id);
                if (existingBundle != null)
                {
                    existingBundle = bundle;
                }
                else
                {
                    bundles.Add(bundle);
                }
            }
            DisplayHelper.NewDisplayLine(debugstring);
        }

        internal static void PurchaseItemBundle(int bundleId)
        {
           
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
            ItemManagerCalls.destinationLocation = 0;
            if (action == WebCallsTest.ItemAction.None)
            {
                return 0;
            }
            return action;
        }

    }

}
