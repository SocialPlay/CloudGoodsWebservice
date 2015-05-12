using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using LitJson;
using CloudGoods.Services;
using CloudGoods.SDK.Models;
using CloudGoods.Enums;

namespace CloudGoods.SDK.Container
{

    public class PersistentItemContainer : MonoBehaviour
    {
        public Action<List<InstancedItemInformation>, ItemContainer> LoadedItemsForContainerEvent;

        public ItemContainer Container;
        public ItemOwnerTypes OwnerType;
        public int Location;

        public void LoadItems()
        {
            ItemManipulationServices.UserItems(new UserItemsRequest(Location), RecivedItems);
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
                case ItemOwnerTypes.Session:
                    return AccountServices.ActiveUser.SessionId.ToString();
                case ItemOwnerTypes.User:
                    return AccountServices.ActiveUser.UserID.ToString();
            }
            return "";

        }

        #region Loading Items
        protected void RecivedItems(List<InstancedItemInformation> receivedItems)
        {
            foreach (InstancedItemInformation item in receivedItems)
            {
                Container.Add(new OwnedItemInformation()
                {
                    Location = item.Location,
                    Amount = item.Amount,
                    Information = item.Information,
                    IsLocked = false,
                    OwnerContainer = Container,
                    StackLocationId = item.StackLocationId

                }, -1,false);
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

        void ModifiedItem(OwnedItemInformation data, bool isSave)
        {
            if (isSave == true)
            {
                ItemManipulationServices.MoveItem(new MoveItemsRequest(new MoveItemsRequest.MoveOrder( data.StackLocationId,data.Amount, Location)), x =>
                {
                    data.StackLocationId = x.UpdatedStackIds[0].StackId;
                    data.IsLocked = false;
                });
            }
        }

        void AddedItem(OwnedItemInformation data, bool isSave)
        {
            if (isSave == true)
            {
                data.IsLocked = true;
        
                ItemManipulationServices.MoveItem(new MoveItemsRequest(new MoveItemsRequest.MoveOrder(data.StackLocationId, data.Amount, Location)), x =>
                {
                    Debug.Log(x.UpdatedStackIds[0].StackId);
                    data.StackLocationId = x.UpdatedStackIds[0].StackId;
                    data.IsLocked = false;
                });
            }
        }

        void RemovedItem(InstancedItemInformation data, int amount, bool isMoving)
        {
            if (!isMoving)
            {
                //CloudGoods.DeductStackAmount(data.stackID, -Amount, delegate(string x)
                //{
                //    data.isLocked = false;
                //});
            }
        }

        #endregion
    }
}
