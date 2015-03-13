﻿using System;
using System.Collections.Generic;



public class SecurePayload
{
    public string token;
    public string data;
}

public class GiveOwnerItemWebserviceRequest
{
    //public List<WebModels.ItemsInfo> listOfItems;
    //public WebModels.OwnerTypes OwnerType;
    public string ownerID;
    public string appID;
}

public class CloudGoodsUser
{
    public string UserID = "";
    public bool IsNewUserToWorld = false;
    public string UserName = "";
    public string UserEmail = "";
    public string SessionID;
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
    public int restrictionType;
    public int restrictionAmount;

    public virtual int GetRestrictionForType(int type)
    {
        if (restrictionType == type || restrictionType == -1)
        {
            return restrictionAmount;
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
    public int recipeID;
    public string name;
    public int energy;
    public string description;
    public string imgURL;

    public List<ItemDetail> RecipeDetails;
    public List<IngredientDetail> IngredientDetails;
}

public class IngredientDetail
{
    public string name;
    public int ingredientID;
    public int amount;
    public int energy;
    public string imgURL;
}

public class ItemDetail
{
    public string name;
    public float value;
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
    public bool isExisting;
    public string userValue;
    public DateTime lastUpdated;
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
    public User user;
    public string value;

    public class User
    {
        public string userName;
        public int platformID;
        public string platformUserID;
        public string userID;
    }

    public MultipleUserDataValue(string userName, int platformID, string platformUserID, string userID, string newValue)
    {
        user = new User();
        user.userName = userName;
        user.platformID = platformID;
        user.platformUserID = platformUserID;
        user.userID = userID;
        value = newValue;
    }
}
#endregion





namespace CloudgoodsClasses
{
    public abstract class RequestClass
    {
        public abstract string ToHashable();
    }


    public class UpdatedStacksResponse
    {
        public List<string> UpdatedStackIds;
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

    #region Item voucher

    public class CreateItemVouchersRequest : RequestClass
    {
        public int MinimumEnergy { get; set; }
        public int TotalEnergy { get; set; }
        public string AndTags = "";
        public string OrTags = "";

        public override string ToHashable()
        {
            return MinimumEnergy + TotalEnergy + AndTags + OrTags;
        }
    }

    public class CreateItemVouchersResponse
    {
        public List<ItemVoucher> Vouchers;

        public class ItemVoucher
        {
            public ItemData Item;
            public int Id;
        }
    }



    public class RedeemItemVouchersRequest : RequestClass
    {
        public List<ItemVoucherSelection> SelectedVouchers;
        public OtherOwner OtherOwner { get; set; }

        public override string ToHashable()
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

    public class RedeemItemVouchersResponse
    {
        public List<ConsumeItemVoucherResult> results { get; set; }

        public class ConsumeItemVoucherResult
        {
            public int ItemId { get; set; }
            public string StackLocationId { get; set; }
            public int Amount { get; set; }
        }

    }

    #endregion

    #region Move Items
    public class MoveItemsRequest : RequestClass
    {
        public List<MoveOrder> moveOrders = new List<MoveOrder>();
        public OtherOwner otherOwner = null;

        public override string ToHashable()
        {
            string results = "";
            foreach (MoveOrder order in moveOrders)
            {
                results += order.ToHashable();
            }
            results += OtherOwner.ToHashable(otherOwner);
            return results;
        }

        public class MoveOrder
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

    public class UpdateItemByIdRequest : RequestClass
    {
        public List<UpdateOrderByID> orders = new List<UpdateOrderByID>();
        public OtherOwner otherOwner;

        public override string ToHashable()
        {
            string resluts = "";
            orders.ForEach(x => resluts += x.ToHashable());
            resluts += OtherOwner.ToHashable(otherOwner);
            return resluts;
        }

        public class UpdateOrderByID : RequestClass
        {
            public int itemId;
            public int amount;
            public int location;

            public override string ToHashable()
            {
                return itemId.ToString() + amount.ToString() + location.ToString();
            }
        }
    }

    public class UpdateItemsByStackIdRequest : RequestClass
    {

        public List<UpdateOrderByStackId> orders = new List<UpdateOrderByStackId>();
        public OtherOwner otherOwner;

        public override string ToHashable()
        {
            string resluts = "";
            orders.ForEach(x => resluts += x.ToHashable());
            resluts += OtherOwner.ToHashable(otherOwner);
            return resluts;
        }

        public class UpdateOrderByStackId : RequestClass
        {
            public string stackId;
            public int amount;
            public int location;

            public override string ToHashable()
            {
                return stackId + amount + location;
            }
        }
    }

    #endregion
}