using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using LitJson;
using CallHandler.Models;

public class PersistentItemContainer : MonoBehaviour
{
    public Action<List<ItemData>, ItemContainer> LoadedItemsForContainerEvent;

    public ItemContainer Container;
    public ItemOwnerTypes OwnerType;
    public int Location;

    public void LoadItems()
    {
        CallHandler.GetUserItems(Location, RecivedItems);
    }

    void Start()
    {
        if (Container == null)
        {
            Container = this.GetComponent<ItemContainer>();
        }
    }

    protected string GetOwnerID()
    {
        switch (OwnerType)
        {
            //case ItemOwnerTypes.Instance:
            //    return ItemSystemGameData.InstanceID.ToString();
            case ItemOwnerTypes.Session:
                return CallHandler.User.SessionID.ToString();
            case ItemOwnerTypes.User:
                return CallHandler.User.UserID.ToString();
        }
        return "";

    }

    #region Loading Items
    protected void RecivedItems(List<ItemData> receivedItems)
    {
        foreach (ItemData item in receivedItems)
        {
            Container.Add(item, -1, false);
        }

        if (LoadedItemsForContainerEvent != null)
        {
            LoadedItemsForContainerEvent(receivedItems, Container);
        }
    }
    #endregion

    #region Saving Items

    void OnEnable()
    {
        if (Container != null)
        {
            Container.AddedItem += AddedItem;
            Container.ModifiedItem += ModifiedItem;
            Container.RemovedItem += RemovedItem;
        }
    }

    void OnDisable()
    {
        if (Container != null)
        {
            Container.ModifiedItem -= ModifiedItem;
            Container.AddedItem -= AddedItem;
            Container.RemovedItem -= RemovedItem;
        }
    }

    void ModifiedItem(ItemData data, bool isSave)
    {
        if (isSave == true)
        {
            Debug.Log("Mod Item");
            CallHandler.MoveItem(data, Location, data.Amount, x =>
            {
                data.StackLocationId = x.UpdatedStackIds[0].StackId;
                data.IsLocked = false;
            });
        }
    }

    void AddedItem(ItemData data, bool isSave)
    {
        if (isSave == true)
        {
            Debug.Log("Add Item");
            data.IsLocked = true;

            CallHandler.MoveItem(data, Location, data.Amount, x =>
            {
                data.StackLocationId = x.UpdatedStackIds[0].StackId;
                data.IsLocked = false;
            });
        }
    }

    void RemovedItem(ItemData data, int amount, bool isMoving)
    {
        if (!isMoving)
        {
            //CloudGoods.DeductStackAmount(data.stackID, -amount, delegate(string x)
            //{
            //    data.isLocked = false;
            //});
        }
    }

    #endregion
}
