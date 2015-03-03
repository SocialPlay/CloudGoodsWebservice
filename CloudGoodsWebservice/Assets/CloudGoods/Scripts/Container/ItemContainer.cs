using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ItemContainer : MonoBehaviour
{
    public int ItemQuantityLimit = 1;

    public bool IsItemQuantityLimited = false;

    public PersistentItemContainer itemLoader;

    public List<ItemData> containerItems = new List<ItemData>();

    public List<IContainerRestriction> containerAddRestrictions = new List<IContainerRestriction>();
    public List<IContainerRestriction> containerRemoveRestrictions = new List<IContainerRestriction>();

    public IContainerAddAction containerAddAction;

    public event Action<ItemData, bool> AddedItem;
    public event Action<ItemData, bool> ModifiedItem;
    public event Action<ItemData, int, bool> RemovedItem;
    public event Action ClearItems;

    private ItemContainerRestrictor restriction = null;

    void Awake()
    {
        if (itemLoader == null) itemLoader = GetComponentInChildren<PersistentItemContainer>();

        if (GetComponent(typeof(IContainerAddAction)) == null)
            containerAddAction = gameObject.AddComponent<BasicAddContainer>();
        else
            containerAddAction = (IContainerAddAction)GetComponent(typeof(IContainerAddAction));

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
        if (containerAddRestrictions.Count > 0)
        {
            foreach (IContainerRestriction newRestriction in containerAddRestrictions)
            {
                if (newRestriction.IsRestricted(ContainerAction.add, itemData))
                    return new ContainerMoveState(ContainerMoveState.ActionState.No);
            }
        }

        return MyContainerAddState(itemData);
    }

    public bool GetContainerRemoveState(ItemData itemData)
    {
        if (containerRemoveRestrictions.Count > 0)
        {
            foreach (IContainerRestriction newRestriction in containerRemoveRestrictions)
            {
                if (newRestriction.IsRestricted(ContainerAction.remove, itemData))
                    return false;
            }
        }

        return true;
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
        containerAddAction.AddItem(itemData, amount, isSave);
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

                modified.Amount-= amount;

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
        if (itemLoader != null)
        {
            itemLoader.LoadItems();
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

