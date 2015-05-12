using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using CloudGoods.SDK.Models;
using CloudGoods.SDK.Container.Restrcitions;

namespace CloudGoods.SDK.Container
{

    public class ItemContainer : MonoBehaviour
    {
        public int ItemQuantityLimit = 1;

        public bool IsItemQuantityLimited = false;

        public PersistentItemContainer ItemLoader;

        public List<OwnedItemInformation> containerItems = new List<OwnedItemInformation>();

        public List<IContainerRestriction> ContainerAddRestrictions = new List<IContainerRestriction>();
        public List<IContainerRestriction> ContainerRemoveRestrictions = new List<IContainerRestriction>();

        public IContainerAddAction ContainerAddAction;

        public event Action<OwnedItemInformation, bool> AddedItem;
        public event Action<OwnedItemInformation, bool> ModifiedItem;
        public event Action<OwnedItemInformation, int, bool> RemovedItem;
        public event Action ClearItems;

        private ItemContainerRestrictor restriction = null;

        void Awake()
        {
            if (ItemLoader == null) ItemLoader = GetComponentInChildren<PersistentItemContainer>();

            if (GetComponent(typeof(IContainerAddAction)) == null)
                ContainerAddAction = gameObject.AddComponent<BasicAddContainer>();
            else
                ContainerAddAction = (IContainerAddAction)GetComponent(typeof(IContainerAddAction));
        }

        void OnRegisteredSession(string user)
        {
            RefreshContainer();
        }

        public void ModifiedItemEvent(OwnedItemInformation item, bool isSave)
        {
            if (ModifiedItem != null)
            {
                ModifiedItem(item, isSave);
            }
        }

        public void ClearItemEvent()
        {
            if (ClearItems != null)
            {
                ClearItems();
            }
        }

        public void AddItemEvent(OwnedItemInformation item, bool isSave)
        {
            if (AddedItem != null)
            {
                AddedItem(item, isSave);
            }
        }

        public void RemoveItemEvent(OwnedItemInformation item, int amount, bool isMoving)
        {
            if (RemovedItem != null)
            {
                RemovedItem(item, amount, isMoving);
            }
        }

        public ContainerMoveState GetContainerAddState(OwnedItemInformation itemData)
        {
            if (ContainerAddRestrictions.Count > 0)
            {
                foreach (IContainerRestriction newRestriction in ContainerAddRestrictions)
                {
                    if (newRestriction.IsRestricted(ContainerAction.add, itemData))
                        return new ContainerMoveState(ContainerMoveState.ActionState.No);
                }
            }

            return MyContainerAddState(itemData);
        }

        public ContainerMoveState GetContainerRemoveState(OwnedItemInformation itemData)
        {
            if (ContainerRemoveRestrictions.Count > 0)
            {
                foreach (IContainerRestriction newRestriction in ContainerRemoveRestrictions)
                {
                    if (newRestriction.IsRestricted(ContainerAction.remove, itemData))
                        return new ContainerMoveState(ContainerMoveState.ActionState.No);
                }
            }

            return new ContainerMoveState(ContainerMoveState.ActionState.Remove);
        }

        protected ContainerMoveState MyContainerAddState(OwnedItemInformation modified)
        {
            int addAbleAmount = modified.Amount;

            if (IsItemQuantityLimited == true)
            {
                foreach (OwnedItemInformation item in containerItems)
                {
                    if (item.IsSameItemAs(modified))
                    {
                        return new ContainerMoveState(ContainerMoveState.ActionState.No, 0);
                    }
                }

                if (addAbleAmount >= ItemQuantityLimit)
                    addAbleAmount = ItemQuantityLimit;
            }

            return new ContainerMoveState(ContainerMoveState.ActionState.Add, addAbleAmount);
        }

        public void Add(OwnedItemInformation itemData, int amount = -1, bool isSave = true)
        {
            ContainerAddAction.AddItem(itemData, amount, isSave);
        }


        public void Remove(OwnedItemInformation itemData, bool isMoving, int amount = -1)
        {
            if (ItemContainerStackRestrictor.Restrictor != null)
            {
                if (restriction.IsRestricted(ContainerAction.remove))
                {
                    return;
                }
            }

            RemoveItem(itemData, isMoving, amount);
        }

        protected void RemoveItem(OwnedItemInformation modified, bool isMoving, int amount = -1)
        {
            foreach (OwnedItemInformation item in containerItems)
            {
                if (item.IsSameItemAs(modified))
                {
                    if (amount == -1 || item.Amount <= amount)
                        containerItems.Remove(item);

                    modified.Amount -= amount;

                    RemoveItemEvent(item, amount, isMoving);
                    return;
                }
            }
            return;
        }

        public int Contains(OwnedItemInformation modified)
        {
            foreach (OwnedItemInformation item in containerItems)
            {
                if (item.IsSameItemAs(modified))
                {
                    return item.Amount;
                }
            }
            return 0;
        }

        public void Clear()
        {
            containerItems.Clear();
            ClearItemEvent();
        }

        public void RefreshContainer()
        {
            if (ItemLoader != null)
            {
                ItemLoader.LoadItems();
                Clear();
            }
        }
    }
}

