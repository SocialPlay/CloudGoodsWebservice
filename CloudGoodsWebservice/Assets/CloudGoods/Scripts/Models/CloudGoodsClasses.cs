using System;
using System.Collections.Generic;
using CloudGoods.Models;
using CloudGoods.Container;

namespace CloudGoods.Models
{
    public interface IRequestClass
    {
        string ToHashable();
    }

    #region User

    public class SecurePayload
    {
        public string Token;
        public string Data;
    }

    public class GiveOwnerItemWebserviceRequest
    {
        //public List<WebModels.ItemsInfo> listOfItems;
        //public WebModels.OwnerTypes OwnerType;
        public string OwnerID;
        public string AppID;
    }

    public class CloudGoodsUser
    {
        public string UserID = "";
        public bool IsNewUserToWorld = false;
        public string UserName = "";
        public string UserEmail = "";
        public string SessionID;
    }

    public class UserLoginInfo
    {
        public int Code;
        public string Message;
        public CloudGoodsUser UserInfo;

        public UserLoginInfo(int caseCode, string msg, CloudGoodsUser newUserInfo)
        {
            Code = caseCode;
            Message = msg;
            UserInfo = newUserInfo;
        }

        public override string ToString()
        {
            return "Code :" + Code + "\nMessage :" + Message;
        }
    }

    public class LoginUserInfo
    {
        public Guid ID;
        public string Name;
        public string Email;

        public LoginUserInfo(Guid userID, string userName, string userEmail)
        {
            ID = userID;
            Name = userName;
            Email = userEmail;
        }
    }

    public class WebserviceError
    {
        public int ErrorCode;
        public string Message;

        public WebserviceError(int newErrorCode, string newErrorMessage)
        {
            ErrorCode = newErrorCode;
            Message = newErrorMessage;
        }
    }


    public class OtherOwner
    {
        public string Id = "Default";
        public string Type = "User";

        public OtherOwner()
        {
            Id = "Default";
            Type = "User";
        }

        public OtherOwner(string Id, string type)
        {
            this.Id = Id;
            this.Type = type;
        }
        private string ToHashable()
        {
            return Id + Type;
        }

        public static string ToHashable(OtherOwner value)
        {
            return value == null ? "" : value.ToHashable();
        }
    }


    #endregion

    #region Item voucher

    public class CreateItemVouchersRequest : IRequestClass
    {
        public int MinimumEnergy { get; set; }
        public int TotalEnergy { get; set; }
        public string AndTags = "";
        public string OrTags = "";

        public string ToHashable()
        {
            return MinimumEnergy + TotalEnergy + AndTags + OrTags;
        }
    }

    public class ItemVouchersResponse
    {
        public List<ItemVoucher> Vouchers;

        public class ItemVoucher
        {
            public ItemData Item;
            public int Id;
        }
    }



    public class RedeemItemVouchersRequest : IRequestClass
    {
        public List<ItemVoucherSelection> SelectedVouchers;
        public OtherOwner OtherOwner { get; set; }

        public string ToHashable()
        {
            string HashValue = OtherOwner.ToHashable(OtherOwner);
            SelectedVouchers.ForEach(v => HashValue += v.ItemId + v.Amount + v.VoucherId);
            return HashValue;
        }

        public class ItemVoucherSelection
        {
            public int VoucherId;
            public int ItemId;
            public int Amount;
            public int Location;
        }
    }

    #endregion

    #region Move Items
    public class MoveItemsRequest : IRequestClass
    {
        public List<MoveOrder> MoveOrders = new List<MoveOrder>();
        public OtherOwner OtherOwner = null;

        public string ToHashable()
        {
            string results = "";
            foreach (MoveOrder order in MoveOrders)
            {
                results += order.ToHashable();
            }
            results += OtherOwner.ToHashable(OtherOwner);
            return results;
        }

        public class MoveOrder
        {
            public string StackId;
            public int Amount;
            public int Location;

            public string ToHashable()
            {
                return StackId + Amount + Location;
            }
        }
    }

    public class UpdateItemByIdRequest : IRequestClass
    {
        public List<UpdateOrderByID> Orders = new List<UpdateOrderByID>();
        public OtherOwner OtherOwner;

        public string ToHashable()
        {
            string resluts = "";
            Orders.ForEach(x => resluts += x.ToHashable());
            resluts += OtherOwner.ToHashable(OtherOwner);
            return resluts;
        }

        public class UpdateOrderByID : IRequestClass
        {
            public int itemId;
            public int amount;
            public int location;

            public string ToHashable()
            {
                return itemId.ToString() + amount.ToString() + location.ToString();
            }
        }
    }

    public class UpdateItemsByStackIdRequest : IRequestClass
    {
        public List<UpdateOrderByStackId> Orders = new List<UpdateOrderByStackId>();
        public OtherOwner DestinationOwner;

        public string ToHashable()
        {
            string resluts = "";
            Orders.ForEach(x => resluts += x.ToHashable());
            resluts += OtherOwner.ToHashable(DestinationOwner);
            return resluts;
        }

        public class UpdateOrderByStackId : IRequestClass
        {
            public string stackId;
            public int amount;
            public int location;

            public string ToHashable()
            {
                return stackId + amount + location;
            }
        }
    }

    public class SimpleItemInfo
    {
        public string StackId;
        public int Amount;
        public int Location;
    }

    public class UpdatedStacksResponse
    {
        public List<SimpleItemInfo> UpdatedStackIds;
    }

    #endregion

    #region Bundles

    public class PaidCurrencyBundleItem
    {
        public int Amount = 0;
        public string Cost = "";
        public int ID = 0;
        public string CurrencyName = "";
        public string CurrencyIcon = "";
        public string Description = "";
        public string BundleName = "";

        public Dictionary<string, string> CreditPlatformIDs = new Dictionary<string, string>();
    }


    public class ItemBundle
    {
        public int ID;
        public int CreditPrice;
        public int CoinPrice;

        //State 1 = Credit and Coin Purchaseable
        //State 2 = Credit purchase only
        //State 3 = Coin Purchase only
        //State 4 = Free
        //public CloudGoodsBundle State;

        public string Name;
        public string Description;
        public string Image;

        public List<BundleItem> bundleItems = new List<BundleItem>();
    }

    public class BundleItem
    {
        public int Quantity;
        public int Quality;

        public string Name;
        public string Image;
        public string Description;

        public List<BundleItemDetails> bundleItemDetails = new List<BundleItemDetails>();
    }

    public class BundleItemDetails
    {
        public float Value;
        public string BundleDetailName;
    }

    public class BundlePurchaseRequest
    {
        public int BundleID;
        public string UserID;
        public string ReceiptToken;
        public int PaymentPlatform;
    }

    public class WorldCurrencyInfo
    {
        public string FreeCurrencyName;
        public string FreeCurrencyImage;
        public string PaidCurrencyName;
        public string PaidCurrencyImage;
    }

    public class PurchasePremiumCurrencyBundleResponse
    {
        public int StatusCode;
        public int Balance;
        public string Message;
    }

    #endregion

    #region Items

    public class NewItemStack
    {
        public string StackLocationId;
    }

    public class StoreItem
    {
        public int ID = 0;
        public string ItemName = "";
        public List<StoreItemDetail> ItemDetail = new List<StoreItemDetail>();
        public DateTime AddedDate;
        public string Behaviours;
        public List<string> Tags;
        public int ItemID = 0;
        public int PremiumCurrencyValue = 0;
        public int StandardCurrencyValue = 0;
        public string ImageURL = "";
    }

    public class StoreItemDetail
    {
        public string PropertyName;
        public int PropertyValue;
        public bool InvertEnergy;
    }

    public class GeneratedItems
    {
        public List<ItemData> Items;
        public int GenerationID;
    }

    public class SelectedGenerationItem
    {
        public int ItemId;
        public int Amount;
    }

    public class GiveGeneratedItemResult
    {
        public int ItemId;
        public string StackLocationId;
        public int Amount;
    }

    [System.Serializable]
    public class BehaviourDefinition
    {
        public string Name;
        public int ID;
        public string Description;
        public int Energy;
    }


    [System.Serializable]
    public class ItemData
    {
        public string StackLocationId;
        public int Id;
        public int CollectionId;
        public int ClassId;
        public string Name;
        public int Amount;
        public int Location;
        public string Detail;
        public int Energy;
        public int Quality;
        public string Description;
        public string ImageName;
        public string AssetBundleURL;
        public List<Behaviour> Behaviours = new List<Behaviour>();
        public List<Tag> Tags = new List<Tag>();

        public ItemContainer OwnerContainer;
        public bool IsLocked = false;

        public class Tag
        {
            public string Name;
            public int Id;
        }

        public class Behaviour
        {
            public string Name;
            public int Id;
        }

        public bool IsSameItemAs(ItemData other)
        {
            if (this == null || other == null)
            {
                return false;
            }
            if (Id == other.Id)
                return true;
            else return false;
        }
    }

    #endregion

    #region Containers

    [System.Serializable]
    public class ContainerMoveState
    {
        public enum ActionState { Add, Swap, No, Remove }
        public ActionState ContainerActionState;
        public int PossibleAddAmount;
        public ItemData PossibleSwapItem;


        public ContainerMoveState(ActionState newState = ActionState.No, int possibleAddAmount = 0, ItemData possibleSwapItem = null)
        {
            this.ContainerActionState = newState;
            this.PossibleAddAmount = possibleAddAmount;
            this.PossibleSwapItem = possibleSwapItem;
        }
    }

    [System.Serializable]
    public class ItemContainerStackRestrictor
    {
        public static ItemStackRestrictionHandler Restrictor;
    }

    [System.Serializable]
    public class ItemContainerStackRestrictions
    {
        public int RestrictionType;
        public int RestrictionAmount;

        public virtual int GetRestrictionForType(int type)
        {
            if (RestrictionType == type || RestrictionType == -1)
            {
                return RestrictionAmount;
            }
            return -1;
        }

    }

    [System.Serializable]
    public abstract class ItemStackRestrictionHandler
    {
        protected List<ItemContainerStackRestrictions> restrictions = new List<ItemContainerStackRestrictions>();


        public virtual int GetRestrictedAmount(ItemData data, ItemContainer target)
        {
            restrictions = GetRestrictionsFor(target);
            foreach (ItemContainerStackRestrictions restriction in restrictions)
            {
                int restrictedAmount = restriction.GetRestrictionForType(data.ClassId);
                if (restrictedAmount != -1)
                {
                    return restrictedAmount;
                }
            }
            return -1;
        }

        protected virtual List<ItemContainerStackRestrictions> GetRestrictionsFor(ItemContainer target)
        {
            return restrictions;
        }

    }

    #endregion

    #region Recipes

    public class RecipeInfo
    {
        public int Id;
        public string Name;
        public int Energy;
        public string Description;
        public string ImgUrl;

        public List<ItemDetail> RecipeDetails;
        public List<IngredientDetail> IngredientDetails;
    }

    public class IngredientDetail
    {
        public string Name;
        public int Id;
        public int Amount;
        public int Energy;
        public string ImgUrl;
    }

    public class ItemDetail
    {
        public string Name;
        public float Value;
    }

    public class ConsumeResponse
    {
        public int Result;
        public int Balance;
        public string Message;
    }

    #endregion

    #region Persistant User Data
    public class SaveUserDataRequest
    {
        public string Key;
        public string Value;
        public string UserID;
        public string AppID;
    }

    class DeleteUserDataRequest
    {
        public string Key;
        public string UserID;
        public string AppID;
    }

    public class PersistentDataResponse
    {
        public bool IsExisting;
        public string UserValue;
        public DateTime LastUpdated;
    }

    public class SaveAppDataRequest
    {
        public string Key;
        public string Value;
        public string AppID;
    }

    class DeleteAppDataRequest
    {
        public string Key;
        public string AppID;
    }

    public class MultipleUserDataValue
    {
        public UserData User;
        public string Value;

        public class UserData
        {
            public string UserName;
            public int PlatformID;
            public string PlatformUserID;
            public string UserID;
        }

        public MultipleUserDataValue(string userName, int platformID, string platformUserID, string userID, string newValue)
        {
            User = new UserData();
            User.UserName = userName;
            User.PlatformID = platformID;
            User.PlatformUserID = platformUserID;
            User.UserID = userID;
            Value = newValue;
        }
    }
    #endregion

    #region Store

    public class CurrencyInfoResponse
    {
        public string PremiumCurrencyName;
        public string StandardCurrencyName;
        public string PremiumCurrencyImage;
        public string StandardCurrencyImage;
    }

    public class CurrencyBalanceResponse
    {
        public int Amount;
    }

    #endregion

    #region Item Bundles
    public class ItemBundlesResponse
    {
        public List<ItemBundleInfo> bundles = new List<ItemBundleInfo>();

        public class ItemBundleInfo
        {
            public int Id;
            public string Name;
            public string Description;
            public string Image;
            public int CreditPrice;
            public int CoinPrice;
            public List<ItemData> items = new List<ItemData>();
            public int State;
        }
    }
    #endregion
}
