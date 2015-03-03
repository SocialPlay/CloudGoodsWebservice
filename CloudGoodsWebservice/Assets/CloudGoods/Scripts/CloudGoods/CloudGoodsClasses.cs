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
    public string UserID = "";
    public bool isNewUserToWorld = false;
    public string userName = "";
    public string userEmail = "";
    public string sessionID;

    public CloudGoodsUser(string newUserId, string newUserName, string newUserEmail, string newSessionID, bool newIsNewUserToWorld)
    {
        UserID = newUserId;
        userName = newUserName;
        userEmail = newUserEmail;
        sessionID = newSessionID;
        isNewUserToWorld = newIsNewUserToWorld;
    }
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
    public int Id;
    public int CollectionId;
    public string StackLocationId;
    public int ClassId;
    public string Name;
    public int Amount;
    public int Location;
    public string Detail;
    public int Energy;
    public List<Behaviours> behaviours = new List<Behaviours>();
    public List<Tag> tags = new List<Tag>();

    public ItemContainer OwnerContainer;

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
public class ContainerAddState
{
    public enum ActionState { Add, Swap, No }
    public ActionState actionState;
    public int possibleAddAmount;
    public ItemData possibleSwapItem;


    public ContainerAddState(ActionState newState = ActionState.No, int possibleAddAmount = 0, ItemData possibleSwapItem = null)
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

