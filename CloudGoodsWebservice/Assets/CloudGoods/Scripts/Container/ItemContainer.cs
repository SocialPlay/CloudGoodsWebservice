using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using CloudGoods.Models;
using CloudGoods.Container.Restrcitions;

namespace CloudGoods.Container
{

    public class ItemContainer : MonoBehaviour
    {
        public int ItemQuantityLimit = 1;

        public bool IsItemQuantityLimited = false;

        public PersistentItemContainer ItemLoader;

        public List<ItemData> containerItems = new List<ItemData>();

        public List<IContainerRestriction> ContainerAddRestrictions = new List<IContainerRestriction>();
        public List<IContainerRestriction> ContainerRemoveRestrictions = new List<IContainerRestriction>();

        public IContainerAddAction ContainerAddAction;

        public event Action<ItemData, bool> AddedItem;
        public event Action<ItemData, bool> ModifiedItem;
        public event Action<ItemData, int, bool> RemovedItem;
        public event Action ClearItems;

        private ItemContainerRestrictor restriction = null;

        void Awake()
        {
            if (ItemLoader == null) ItemLoader = GetComponentInChildren<PersistentItemContainer>();

            if (GetComponent(typeof(IContainerAddAction)) == null)
                ContainerAddAction = gameObject.AddComponent<BasicAddContainer>();
            else
                ContainerAddAction = (IContainerAddAction)GetComponent(typeof(IContainerAddAction));

            //CloudGoods.OnRegisteredUserToSession += OnRegisteredSession;
        }

        void OnDestroy()
        {
            //CloudGoods.OnRegisteredUserToSession -= OnRegisteredSession;
        }

        void OnRegisteredSession(string user)
        {
            RefreshContainer();
        }

        public void ModifiedItemEvent(ItemData item, bool isSave)
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

        public void AddItemEvent(ItemData item, bool isSave)
        {
            if (AddedItem != null)
            {
                AddedItem(item, isSave);
            }
        }

        public void RemoveItemEvent(ItemData item, int amount, bool isMoving)
        {
            if (RemovedItem != null)
            {
                RemovedItem(item, amount, isMoving);
            }
        }

        public ContainerMoveState GetContainerAddState(ItemData itemData)
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

        public ContainerMoveState GetContainerRemoveState(ItemData itemData)
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

        protected ContainerMoveState MyContainerAddState(ItemData modified)
        {
            int addAbleAmount = modified.Amount;

            if (IsItemQuantityLimited == true)
            {
                foreach (ItemData item in containerItems)
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

        public void Add(ItemData itemData, int amount = -1, bool isSave = true)
        {
            ContainerAddAction.AddItem(itemData, amount, isSave);
        }


        public void Remove(ItemData itemData, bool isMoving, int amount = -1)
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

        protected void RemoveItem(ItemData modified, bool isMoving, int amount = -1)
        {
            foreach (ItemData item in containerItems)
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

        public int Contains(ItemData modified)
        {
            foreach (ItemData item in containerItems)
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

        public void UpdateContainerWithItems(List<GiveGeneratedItemResult> givenUserItems)
        {
            bool IsItemInContainer = false;

            foreach (GiveGeneratedItemResult givenUserItem in givenUserItems)
            {
                IsItemInContainer = false;

                ItemData givenItemData;

                if (!string.IsNullOrEmpty(givenUserItem.StackLocationId))
                {
                    givenItemData = containerItems.FirstOrDefault(x => x.StackLocationId == givenUserItem.StackLocationId);

                    if (givenItemData != null)
                    {
                        IsItemInContainer = true;
                        givenItemData.Amount = givenUserItem.Amount;
                    }
                }
                else
                {
                    givenItemData = containerItems.FirstOrDefault(x => x.Id == givenUserItem.ItemId);

                    if (givenItemData != null)
                    {
                        IsItemInContainer = true;
                        givenItemData.Amount += givenUserItem.Amount;
                    }
                }
            }

            if (IsItemInContainer == false)
            {
                RefreshContainer();
            }
        }
    }
}

