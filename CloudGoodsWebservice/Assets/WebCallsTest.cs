using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using CloudGoods;
using CloudGoods.Models;
using CloudGoods.Emuns;
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

        string title = "";
        private static SystemTabs activeTab = SystemTabs.BaseItems;

        void OnEnable()
        {
            CallHandler.CloudGoodsInitilized += Instance_CloudGoodsInitilized;
            CallHandler.IsError += CallHandler_IsError;
        }

        void CallHandler_IsError(WebserviceError obj)
        {
            string debug = string.Format("Error Code: {0} , Message: {1}", obj.ErrorCode, obj.Message);
            DisplayHelper.NewDisplayLine(debug, Color.red);
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
                    ItemManagerCalls.Draw();
                    break;
                case SystemTabs.ItemVouchers:
                    ItemVouchersCalls.Draw();
                    break;
                case SystemTabs.Store:

                    break;
                case SystemTabs.ItemBundles:
                    ItemBundlesCalls.DrawGUI();
                    break;
            }
            ItemManagerCalls.DrawItemDetails();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

    }


    internal static class DisplayHelper
    {
        static string DebugDisplay = "";
        static string Last = "";

        static Color defaultColor = Color.white;
        static Color currentColor = defaultColor;

        public static void DrawRight()
        {
            GUILayout.BeginArea(new Rect(Screen.width / 2, 0, Screen.width, Screen.height));
          
            GUI.color = currentColor;
            GUILayout.TextArea(Last);
            GUI.color = Color.gray;
            GUILayout.TextField(DebugDisplay);
            GUILayout.EndArea();
        }

        public static void NewDisplayLine(string line)
        {
            NewDisplayLine(line, defaultColor);
        }

        public static void NewDisplayLine(string line, Color newLineColor)
        {
            currentColor = newLineColor;
            DebugDisplay = Last + "\n" + DebugDisplay;
            Last = line;
        }
    }


    internal static class ItemManagerCalls
    {

        public enum ItemAction
        {
            move,
            UpdateStackByFive,
            UpdateStackToOne,
            RemoveStack,
            None
        }

        public static ItemAction CurrentAction;
        public static List<OwnedItemInformation> UsersItems = new List<OwnedItemInformation>();
        public static int destinationLocation = 0;

        public static void Draw()
        {
            if (GUILayout.Button("Load users Items"))
            {
                ItemManagerCalls.LoadUserItems();
            }
            if (GUILayout.Button("Give User Item"))
            {
                UpdateItemById();
            }
        }

        public static void DrawItemDetails()
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
            foreach (OwnedItemInformation item in ItemManagerCalls.UsersItems)
            {
                if (GUILayout.Button(string.Format("{0}\nSLID:{3}\n  Amount:{1}\n  Location:{2}", item.Information.Name, item.Amount, item.Location, item.StackLocationId)))
                {
                    PerformCurrentAction(item);
                    return;
                }
            }
        }

        static void PerformCurrentAction(OwnedItemInformation item)
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

        static void UpdateItemById()
        {
            List<UpdateItemByIdRequest.UpdateOrderByID> infos = new List<UpdateItemByIdRequest.UpdateOrderByID>(){
                new UpdateItemByIdRequest.UpdateOrderByID(){location = 0, itemId = 111542, amount = 50000},
                new UpdateItemByIdRequest.UpdateOrderByID(){location = 6, itemId = 111542, amount = 2}
            };

            CallHandler.UpdateItemsByIds(infos, ItemManagerCalls.DisplayUpdatedItems);
        }

        static void LoadUserItems()
        {
            CallHandler.GetUserItems(0, delegate(List<InstancedItemInformation> items)
            {
                DisplayItems(items);
            });
        }

        static void MoveItem(OwnedItemInformation item)
        {
            CallHandler.MoveItem(item, destinationLocation, item.Amount, DisplayUpdatedItems);
            UsersItems.Remove(item);
        }

        static void RemoveStack(OwnedItemInformation item)
        {
            CallHandler.UpdateItemByStackIds(item.StackLocationId, -item.Amount, item.Location, DisplayUpdatedItems);
            UsersItems.Remove(item);
        }

        static void MakeStackSizeOne(OwnedItemInformation item)
        {
            CallHandler.UpdateItemByStackIds(item.StackLocationId, -(item.Amount - 1), item.Location, DisplayUpdatedItems);
            item.Amount = 1;
        }

        public static void AddFiveToStack(OwnedItemInformation item)
        {
            CallHandler.UpdateItemByStackIds(item.StackLocationId, 5, item.Location, DisplayUpdatedItems);

            item.Amount += 5;

        }

        static void DisplayItems(List<InstancedItemInformation> items)
        {
            string debugString = "Recived Items";
            UsersItems.Clear();
            foreach (InstancedItemInformation item in items)
            {
                if (item.Amount == 0) { items.Remove(item); continue; }
                debugString += "\nName: " + item.Information.Name;
                debugString += "\n    Amount:" + item.Amount;
                debugString += "\n    Id: " + item.Information.Id;
                debugString += "\n    Location: " + item.Location.ToString();
                debugString += "\n    Detail:" + item.Information.Detail;
                debugString += "\n    SLID:" + item.StackLocationId + "\n";
                UsersItems.Add(new OwnedItemInformation()
                {
                    Location = item.Location,
                    Amount = item.Amount,
                    Information = item.Information,
                    IsLocked = false,
                    OwnerContainer = null,
                    StackLocationId = item.StackLocationId
                });
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
                InstancedItemInformation data = UsersItems.Find(x => x.StackLocationId == item.StackId);
                if (data != null)
                {
                    data.Amount = item.Amount;
                    data.Location = item.Location;
                }
                else if (item.Amount != 0)
                {
                    UsersItems.Add(new OwnedItemInformation()
                    {
                        Information = new ItemInformation()
                        {
                            Name = "---- Needs refresh -----"
                        },
                        Location = item.Location,
                        StackLocationId = item.StackId,
                        Amount = item.Amount,
                        IsLocked = false,
                        OwnerContainer = null
                    });

                }
            }

        }

        public static string ToReadable(this ItemManagerCalls.ItemAction action)
        {
            switch (action)
            {
                case ItemManagerCalls.ItemAction.move:
                    return "Move to";
                case ItemManagerCalls.ItemAction.UpdateStackByFive:
                    return "Add 5 to amount";
                case ItemManagerCalls.ItemAction.UpdateStackToOne:
                    return "Chnage amount to One";
                case ItemManagerCalls.ItemAction.RemoveStack:
                    return "Remove Stack";
                default:
                    return "Unknow Action";
            }
        }
        public static ItemManagerCalls.ItemAction Next(this ItemManagerCalls.ItemAction action)
        {
            action += 1;
            ItemManagerCalls.destinationLocation = 0;
            if (action == ItemManagerCalls.ItemAction.None)
            {
                return 0;
            }
            return action;
        }
    }

    internal class ItemVouchersCalls
    {
        static List<VoucherItemInformation> CurrentVouchers = new List<VoucherItemInformation>();
        static int voucherModifyamount = 0;

        public static void Draw()
        {
            if (GUILayout.Button("Create Item Vouchers"))
            {
                CreateItemVoucher();
            }
            GUILayout.Label(string.Format("Item Vouchers ({0})", CurrentVouchers.Count));
            GUILayout.BeginHorizontal();
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
            int count = CurrentVouchers.Count;
            int shown = 5;
            for (int i = count < shown ? 0 : count - shown; i < count; i++)
            {
                GUILayout.BeginHorizontal();
                VoucherItemInformation voucher = CurrentVouchers[i];
                if (GUILayout.Button(string.Format("({0}) {1} : {2}", voucher.VoucherId, voucher.Information.Id, voucher.Information.Name)))
                {
                    RedeemItemVoucher(voucher);

                    return;
                }
                if (GUILayout.Button("Get", GUILayout.MaxWidth(100)))
                {
                    GetItemVoucher(voucher.VoucherId);
                }
                GUILayout.EndHorizontal();
            }
        }


        static void CreateItemVoucher()
        {
            CallHandler.CreateItemVouchers(1, 700, DisaplayItemVouchers);

        }

        static void RedeemItemVoucher(VoucherItemInformation voucher)
        {

            List<RedeemItemVouchersRequest.ItemVoucherSelection> selectedVouchers = new List<RedeemItemVouchersRequest.ItemVoucherSelection>() { new RedeemItemVouchersRequest.ItemVoucherSelection() { Amount = voucher.Amount, ItemId = voucher.Information.Id, Location = 0, VoucherId = voucher.VoucherId } };

            CallHandler.RedeemItemVouchers(selectedVouchers, delegate(UpdatedStacksResponse response)
            {
                ItemManagerCalls.DisplayUpdatedItems(response);
                CurrentVouchers.Remove(voucher);
            });


        }

        static void GetItemVoucher(int Id)
        {
            CallHandler.GetItemVoucher(Id, DisaplayItemVouchers);
        }


        static void DisaplayItemVouchers(ItemVouchersResponse response)
        {
            string debugString = "Vouchers Items";
            foreach (VoucherItemInformation voucher in response.Vouchers)
            {
                var existing = CurrentVouchers.FirstOrDefault(v => v.VoucherId == voucher.VoucherId);
                if (existing != null)
                {
                    existing.Amount = voucher.Amount;
                }
                else
                {
                    CurrentVouchers.Add(voucher);
                }
                debugString += string.Format("\n{0}\nId: {1}", voucher.Information.Name, voucher.VoucherId);
            }
            DisplayHelper.NewDisplayLine(debugString);
        }
    }

    internal class ItemBundlesCalls
    {
        static List<ItemBundleInfo> bundles = new List<ItemBundleInfo>();
        static bool IsStandard = true;

        public static void DrawGUI()
        {
            if (GUILayout.Button("Get Item Bundles"))
            {
                GetItemBundles();
            }
            GUILayout.BeginHorizontal();
            GUILayout.Label(string.Format("Current Item Bundles ({0})", bundles.Count));
            if (GUILayout.Button(IsStandard ? "Standard" : "Premium"))
            {
                IsStandard = !IsStandard;
            }
            GUILayout.EndHorizontal();
            foreach (ItemBundleInfo info in bundles)
            {
                if (GUILayout.Button(string.Format("Buy: {0}\nItems {1}\nCost:\nStandard:{2},Premium :{3}", info.Name, info.Items.Count, info.StandardPrice, info.PremiumPrice)))
                {
                    ItemBundlesCalls.PurchaseItemBundle(info.Id);
                }
            }
        }

        static void GetItemBundles()
        {
            CallHandler.GetItemBundles("", "", delegate(ItemBundlesResponse response)
            {
                ItemBundleResponseHandler(response);
            });
        }

        static void ItemBundleResponseHandler(ItemBundlesResponse response)
        {
            string debugstring = "Got item bundles";
            foreach (ItemBundleInfo bundle in response.bundles)
            {
                string items = "";
                bundle.Items.ForEach(i => items = string.Format("{0}\n     {2} {1}", items, i.Information.Name, i.Amount));
                debugstring = string.Format("{0}\n{1}\n   Item count:{2}{3}", debugstring, bundle.Name, bundle.Items.Count, items);
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

        static void PurchaseItemBundle(int bundleId)
        {
            CallHandler.PurchaseItemBundle(bundleId, IsStandard ? 1 : 2, 0, PurchaseItemBundleHandler);
        }

        static void PurchaseItemBundleHandler(ItemBundlePurchaseResponse response)
        {
            string debug = string.Format("Purchased {0} items", response.purchasedItems.Count);
            foreach (SimpleItemInfo item in response.purchasedItems)
            {
                debug = string.Format("{0}\n  {1}:{2}", debug, item.StackId, item.Amount);
            }

            foreach (var item in ItemManagerCalls.UsersItems)
            {
                if (item.StackLocationId == response.StandardCurrency.StackId)
                    item.Amount = response.StandardCurrency.Amount;
            }

            DisplayHelper.NewDisplayLine(debug);
        }
    }



}
