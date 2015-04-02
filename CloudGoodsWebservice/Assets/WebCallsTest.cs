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
            ItemBundles,
            UserCurrency,
            UserData
        }



        string title = "";
        private static SystemTabs activeTab = SystemTabs.BaseItems;

        Vector2 option = Vector2.zero;

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
                GUILayout.BeginHorizontal();
                GUILayout.Label("Welcome " + CallHandler.User.UserName + ".");
                if (GUILayout.Button("Logout"))
                {
                    LoginCalls.logout();
                }
                GUILayout.EndHorizontal();
            }
            else
            {
                LoginCalls.Draw();
                GUILayout.EndArea();
                return;
            }


            option = GUILayout.BeginScrollView(option, GUILayout.MaxHeight(50));
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Basic Items"))
            {
                activeTab = SystemTabs.BaseItems;
                title = "Basic Item Calls";
                SelctionInit();
            }
            if (GUILayout.Button("Item voucher"))
            {
                activeTab = SystemTabs.ItemVouchers;
                title = "Item Voucher Calls";
                SelctionInit();
            }
            if (GUILayout.Button("Store"))
            {
                activeTab = SystemTabs.Store;
                title = "Store Calls";
                SelctionInit();
            }
            if (GUILayout.Button("Item Bundle"))
            {
                activeTab = SystemTabs.ItemBundles;
                title = "Item Bundle Calls";
                SelctionInit();
            }

            if (GUILayout.Button("User Currency"))
            {
                activeTab = SystemTabs.UserCurrency;
                title = "Users currency";
                SelctionInit();
            }

            if (GUILayout.Button("User Data"))
            {
                activeTab = SystemTabs.UserData;
                title = "Users Data";
                SelctionInit();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();

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
                    StoreCalls.Draw();
                    break;
                case SystemTabs.ItemBundles:
                    ItemBundlesCalls.DrawGUI();
                    break;
                case SystemTabs.UserCurrency:
                    UsersCurrency.Draw();
                    break;
                case SystemTabs.UserData:
                    UserDataCalls.Draw();
                    break;
            }
            ItemManagerCalls.DrawItemDetails();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }


        void SelctionInit()
        {
            UsersCurrency.Init();
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

        public static void AddSimpleItemInfo(SimpleItemInfo info)
        {
            foreach (OwnedItemInformation item in UsersItems)
            {
                if (info.StackId == item.StackLocationId)
                {
                    item.Amount = info.Amount;
                    item.Location = info.Location;
                }
                else
                {
                    OwnedItemInformation newItem = new OwnedItemInformation()
                           {
                               Amount = info.Amount,
                               Location = info.Location,
                               StackLocationId = info.StackId,
                               Information = new ItemInformation()
                               {
                                   Name = "--- Needs Refresh ---"
                               }
                           };
                    UsersItems.Add(newItem);
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

    internal static class ItemVouchersCalls
    {
        static List<VoucherItemInformation> CurrentVouchers = new List<VoucherItemInformation>();
        static int voucherModifyamount = 0;
        static Vector2 scroll = Vector2.zero;

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
            scroll = GUILayout.BeginScrollView(scroll);
            foreach (VoucherItemInformation voucher in CurrentVouchers)
            {
                GUILayout.BeginHorizontal();
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
            GUILayout.EndScrollView();
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

    internal static class ItemBundlesCalls
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

    internal static class StoreCalls
    {
        static List<StoreItem> storeItems = new List<StoreItem>();
        static Vector2 scroll = Vector2.zero;

        public static void Draw()
        {
            UsersCurrency.SimpleDisplay();
            if (GUILayout.Button("Get Store Items"))
            {
                GetStoreItems();
            }

            GUILayout.FlexibleSpace();

            scroll = GUILayout.BeginScrollView(scroll);
            foreach (StoreItem item in storeItems)
            {
                GUILayout.BeginHorizontal(GUI.skin.box);
                GUILayout.Label(string.Format("{1}\n  ID:{0}", item.ItemId, item.Name));
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical(GUILayout.Width(300));
                if (item.CreditValue != -1)
                {
                    if (GUILayout.Button(string.Format("{1}: {0}", item.CreditValue, UsersCurrency.PremiumName())))
                    {
                        CallHandler.PurchaseItem(item.ItemId, 1, (int)PurchaseItemRequest.PaymentType.Premium, 0, StoreItemPurchaseResponse);
                    }
                }
                if (item.CoinValue != -1)
                {
                    if (GUILayout.Button(string.Format("{1}: {0}", item.CoinValue, UsersCurrency.StandardName())))
                    {
                        CallHandler.PurchaseItem(item.ItemId, 1, (int)PurchaseItemRequest.PaymentType.Standard, 0, StoreItemPurchaseResponse);
                    }
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }

        private static void StoreItemPurchaseResponse(SimpleItemInfo info)
        {
            ItemManagerCalls.AddSimpleItemInfo(info);

            string debugString = "Purchased item success";
            debugString = string.Format("{0}\nItem\n   Amount:{1}\n   Location:{2}\n   stack Id:{3}", debugString, info.Amount, info.Location, info.StackId);
        }

        private static void StoreResponse(List<StoreItem> items)
        {
            storeItems = items;
        }

        private static void GetStoreItems()
        {
            CallHandler.GetStoreItems(StoreResponse);
        }


    }

    internal static class UsersCurrency
    {



        static CurrencyInfoResponse info;

        static int premiumAmount;
        static int standardAmount;
        static int consumeAmount = 1;

        public static void Init()
        {
            if (info == null)
            {
                CallHandler.GetCurrencyInfo(delegate(CurrencyInfoResponse newInfo)
                {
                    info = newInfo;
                    GetUserAmounts();
                });
            }
            else
            {
                GetUserAmounts();
            }
        }

        private static void GetUserAmounts()
        {
            CallHandler.GetPremiumCurrencyBalance(delegate(CurrencyBalanceResponse response)
            {
                premiumAmount = response.Amount;
            });

            CallHandler.GetStandardCurrencyBalance(0, delegate(SimpleItemInfo response)
            {
                standardAmount = response.Amount;
            });
        }

        public static void Draw()
        {
            if (info == null) return;
            GUILayout.Label(string.Format("Premium:\n  Name: {0}\n  Amount: {1}\nStandard\n  Name: {2}\n  Amount: {3}", info.PremiumCurrencyName, premiumAmount, info.StandardCurrencyName, standardAmount));
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
            {
                consumeAmount++;
            }
            GUILayout.FlexibleSpace();
            GUILayout.Label(consumeAmount.ToString());
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("-"))
            {
                consumeAmount--;
                if (consumeAmount < 1) consumeAmount = 1;
            }
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Consume Premium Currency"))
            {
                ConsumePremium();
            }
        }

        static void ConsumePremium()
        {
            CallHandler.ConsumePremiumCurrency(consumeAmount, delegate(ConsumePremiumResponce responce)
            {
                if (responce.isSuccess)
                {
                    premiumAmount = responce.currentBalance;
                }
            });
        }
        public static void SimpleDisplay()
        {
            if (info == null)
            {
                GUILayout.Label("Waiting");
            }
            else
            {
                GUILayout.BeginHorizontal(GUI.skin.box);
                GUILayout.Label(string.Format("{0}: {1}", info.PremiumCurrencyName, premiumAmount));
                GUILayout.Label(string.Format("{0}: {1}", info.StandardCurrencyName, standardAmount));
                GUILayout.EndHorizontal();
            }
        }

        public static string StandardName()
        {
            return info != null ? info.StandardCurrencyName : "Standard";
        }

        public static string PremiumName()
        {
            return info != null ? info.PremiumCurrencyName : "Premium";
        }

    }

    internal static class LoginCalls
    {

        static bool isSent = false;
        static bool isPlatform = false;

        static string userEmail = "";
        static string password = "";
        static int platform = 1;
        static string platformUserName = "";
        static string platformUserId = "";
        static string platString = "1";


        public static void logout()
        {
            CallHandler.User = null;
        }

        public static void Draw()
        {
            GUILayout.Label("LOGIN");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Use SP"))
            {
                isPlatform = false;
                isSent = false;
                userEmail = PlayerPrefs.GetString("SPLogin_UserEmail", "");
                password = PlayerPrefs.GetString("SPLogin_Password", "");
            }
            if (GUILayout.Button("Use Platform Id"))
            {
                isPlatform = true;
                isSent = false;
                platformUserName = PlayerPrefs.GetString("SPLogin_PlatformUserName", "");
                platformUserId = PlayerPrefs.GetString("SPLogin_PlatformUserID", "");
                platform = PlayerPrefs.GetInt("SPLogin_PlatformId", 1);
            }
            GUILayout.EndHorizontal();
            if (!isPlatform)
            {

                GUILayout.Label("User Email");
                userEmail = GUILayout.TextField(userEmail);

                GUILayout.Label("Password");
                password = GUILayout.PasswordField(password, '*');
            }
            if (isPlatform)
            {
                GUILayout.Label("User Name");
                platformUserName = GUILayout.TextField(platformUserName);
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                GUILayout.Label("Platform ID");
                GUILayout.BeginHorizontal();
                platString = GUILayout.TextField(platString, GUILayout.MaxWidth(100));
                if (!String.IsNullOrEmpty(platString))
                {
                    if (!int.TryParse(platString, out  platform))
                    {
                        platform = -1;
                    }
                }
                if (Enum.IsDefined(typeof(CloudGoodsPlatform), platform))
                {
                    if (GUILayout.Button(((CloudGoodsPlatform)platform).ToString()))
                    {
                        platform++;
                        if (!Enum.IsDefined(typeof(CloudGoodsPlatform), platform))
                        {
                            platform = 1;
                        }

                        platString = platform.ToString();
                    }
                }
                else
                {
                    GUILayout.Label("Unknow");
                }
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                GUILayout.BeginVertical();
                GUILayout.Label("User ID");
                platformUserId = GUILayout.TextField(platformUserId);
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
            if (!isSent)
            {
                if (isReadyToLogin())
                {
                    if (GUILayout.Button("LOGIN"))
                    {
                        if (isPlatform)
                        {
                            PlayerPrefs.SetString("SPLogin_PlatformUserName", platformUserName);
                            PlayerPrefs.SetString("SPLogin_PlatformUserID", platformUserId);
                            PlayerPrefs.SetInt("SPLogin_PlatformId", platform);
                            CallHandler.LoginByPlatform((CloudGoodsPlatform)platform, platformUserId, userEmail, OnReceivedUser);
                        }
                        else
                        {
                            PlayerPrefs.SetString("SPLogin_UserEmail", userEmail);
                            PlayerPrefs.SetString("SPLogin_Password", password);
                            CallHandler.Login(userEmail, password, OnReceivedUser);
                        }
                    }
                }
                else
                {
                    GUILayout.Label("Missing elements");
                }
            }
            else
            {
                GUILayout.Label("Waiting");
            }
        }

        static bool isReadyToLogin()
        {
            if (isPlatform)
            {
                if (string.IsNullOrEmpty(platformUserName) || !Enum.IsDefined(typeof(CloudGoodsPlatform), platform) || string.IsNullOrEmpty(platformUserId)) return false;
            }
            else
            {
                if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(password)) return false;
            }

            return true;
        }

        static void OnReceivedUser(CloudGoodsUser user)
        {
            string debugString = "login Info\nName: " + user.UserName;
            debugString += "\nId: " + user.UserID;
            debugString += "\nEmail: " + user.UserEmail;
            debugString += "\nIs New: " + user.IsNewUserToWorld;
            debugString += "\nSession:" + user.SessionID.ToString();
            DisplayHelper.NewDisplayLine(debugString);

            UsersCurrency.Init();
        }
    }

    internal static class UserDataCalls
    {
        static List<OwnedUserDataValues> worldsData = new List<OwnedUserDataValues>();
        static List<UserDataValue> userDatas = new List<UserDataValue>();
        static string lookupKey = "";
        static string lookupValue = "";

        public static void Draw()
        {
            GUILayout.Space(5);
            GUILayout.Label("Look up key");
            lookupKey = GUILayout.TextField(lookupKey);
            GUILayout.Label("Set value");
            lookupValue = GUILayout.TextField(lookupValue);
            GUILayout.Space(5);

            if (GUILayout.Button("Get User Key"))
            {
                if (!String.IsNullOrEmpty(lookupKey))
                    CallHandler.UserDataGet(lookupKey, OnReciveData);
            }
            if (GUILayout.Button("Update User Key"))
            {
                if (!String.IsNullOrEmpty(lookupKey) && !string.IsNullOrEmpty(lookupValue))
                    CallHandler.UserDataUpdate(lookupKey, lookupValue, OnReciveData);
            }
            if (GUILayout.Button("Get All Users Values"))
            {
                CallHandler.UserDataAll(OnReciveUserDatas);

            }
            if (GUILayout.Button("Get all data by Key"))
            {
                if (!String.IsNullOrEmpty(lookupKey))
                    CallHandler.UserDataByKey(lookupKey, OnRecivedDataByKey);
            }
            GUILayout.FlexibleSpace();
            foreach (UserDataValue udv in userDatas)
            {
                GUILayout.Label(string.Format("key: {0}, Value: {1}", udv.Key, udv.Value),GUI.skin.box); 
            }

        }

        static void OnReciveData(UserDataValue userData)
        {
            if (userData.IsExisting)
            {
                DisplayHelper.NewDisplayLine(string.Format("Recived User Data\n  Key: {0}\n  Value: {1}", userData.Key, userData.Value));
            }
            else
            {
                DisplayHelper.NewDisplayLine("User does not have that key", Color.yellow);
            }
            AddToExisting(userData);
        }

        static void OnReciveUserDatas(List<UserDataValue> values)
        {
            string debugString = "Recived User Datas";
            foreach (UserDataValue value in values)
            {
                AddToExisting(value);
                debugString = string.Format("{0}\n  Key: {1}, Value: {2}", debugString, value.Key, value.Value);
            }
            DisplayHelper.NewDisplayLine(debugString);
        }

        static void AddToExisting(UserDataValue userData)
        {
            if (!userData.IsExisting) return;

            UserDataValue existing = userDatas.FirstOrDefault(u => u.Key == userData.Key);
            if (existing != null)
            {
                existing.Value = userData.Value;
                existing.LastUpdated = userData.LastUpdated;
            }
            else
            {
                userDatas.Add(userData);
            }
        }

        static void OnRecivedDataByKey(List<OwnedUserDataValues> response)
        {
            worldsData = response;
            if (response.Count() == 0) return;
            string debugString = "Recived User Data of " + response[0].UserData.Key;
            foreach (OwnedUserDataValues ownedData in response)
            {
                debugString = string.Format("{0}\n {1}\n   {2}", debugString, ownedData.UserId, ownedData.UserData.Value);
            }
            DisplayHelper.NewDisplayLine(debugString);
        }
    }
}
