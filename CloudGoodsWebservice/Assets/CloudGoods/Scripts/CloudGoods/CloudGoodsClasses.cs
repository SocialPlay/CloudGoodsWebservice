using System;
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
    public string userID = "";
    public bool isNewUserToWorld = false;
    public string userName = "";
    public string userEmail = "";
    public string sessionID;
}

public class LoginUserInfo
{
    public Guid ID;
    public string name;
    public string email;

    public LoginUserInfo(Guid userID, string userName, string userEmail)
    {
        ID = userID;
        name = userName;
        email = userEmail;
    }
}

public class UserLoginInfo
{
    public int code;
    public string message;
    public CloudGoodsUser userInfo;

    public UserLoginInfo(int caseCode, string msg, CloudGoodsUser newUserInfo)
    {
        code = caseCode;
        message = msg;
        userInfo = newUserInfo;
    }

    public override string ToString()
    {
        return "Code :" + code + "\nMessage :" + message;
    }
}

public class WebserviceError
{
    public int errorCode;
    public string Message;

    public WebserviceError(int newErrorCode, string newErrorMessage)
    {
        errorCode = newErrorCode;
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
    public string stackLocationId;
}

public class StoreItem
{
    public int ID = 0;
    public string itemName = "";
    public List<StoreItemDetail> itemDetail = new List<StoreItemDetail>();
    public DateTime addedDate;
    public string behaviours;
    public List<string> tags;
    public int itemID = 0;
    public int premiumCurrencyValue = 0;
    public int standardCurrencyValue = 0;
    public string imageURL = "";
}

public class StoreItemDetail
{
    public string propertyName;
    public int propertyValue;
    public bool invertEnergy;
}

public class GeneratedItems
{
    public List<ItemData> generatedItems;
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
    public string stackLocationId;
    public int Id;
    public int collectionId;
    public int classId;
    public string name;
    public int amount;
    public int location;
    public string detail;
    public int energy;
    public int quality;
    public string description;
    public string imageName;
    public string assetBundleURL;
    public List<Behaviours> behaviours = new List<Behaviours>();
    public List<Tag> tags = new List<Tag>();

    public ItemContainer OwnerContainer;
    public bool IsLocked = false;

    public class Tag
    {
        public string name;
        public int Id;
    }

    public class Behaviours
    {
        public string name;
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
    public ActionState actionState;
    public int possibleAddAmount;
    public ItemData possibleSwapItem;


    public ContainerMoveState(ActionState newState = ActionState.No, int possibleAddAmount = 0, ItemData possibleSwapItem = null)
    {
        this.actionState = newState;
        this.possibleAddAmount = possibleAddAmount;
        this.possibleSwapItem = possibleSwapItem;
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
            int restrictedAmount = restriction.GetRestrictionForType(data.classId);
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
        public List<string> updatedStackIds;
    }

    public class OtherOwner
    {
        public string Id = "Default";
        public string type = "User";

        public OtherOwner()
        {
            Id = "Default";
            type = "User";
        }

        public OtherOwner(string Id, string type)
        {
            this.Id = Id;
            this.type = type;
        }
        private string ToHashable()
        {
            return Id + type;
        }

        public static string ToHashable(OtherOwner value)
        {
            return value == null ? "" : value.ToHashable();
        }
    }

    #region Item voucher

    public class CreateItemVouchersRequest : RequestClass
    {
        public int minimumEnergy { get; set; }
        public int totalEnergy { get; set; }
        public string andTags = "";
        public string orTags = "";

        public override string ToHashable()
        {
            return minimumEnergy + totalEnergy + andTags + orTags;
        }
    }

    public class CreateItemVouchersResponse
    {
        public List<ItemVoucher> vouchers;

        public class ItemVoucher
        {
            public ItemData item;
            public int Id;
        }
    }



    public class RedeemItemVouchersRequest : RequestClass
    {
        public List<ItemVoucherSelection> selectedVouchers;
        public OtherOwner otherOwner { get; set; }

        public override string ToHashable()
        {
            string hashValue = OtherOwner.ToHashable(otherOwner);
            selectedVouchers.ForEach(v => hashValue += v.itemId + v.amount + v.voucherId);
            return hashValue;
        }

        public class ItemVoucherSelection
        {
            public int voucherId;
            public int itemId;
            public int amount;
            public int location;
        }
    }

    public class RedeemItemVouchersResponse
    {
        public List<ConsumeItemVoucherResult> results { get; set; }

        public class ConsumeItemVoucherResult
        {
            public int itemId { get; set; }
            public string stackLocationId { get; set; }
            public int amount { get; set; }
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