using UnityEngine;
using System.Collections;
using CloudGoodsClasses;

[RequireComponent(typeof(ItemContainer))]
public class BasicAddContainer : MonoBehaviour, IContainerAddAction
{
    public ItemContainer ItemContainer;

    void Awake()
    {
        ItemContainer = GetComponent<ItemContainer>();
    }

    public void AddItem(ItemData addItem, int amount, bool isSave)
    {
        if (amount == -1 || amount > addItem.Amount)
        {
            amount = addItem.Amount;
            addItem.OwnerContainer = ItemContainer;
            if (!AddToExistingStack(addItem, addItem.Amount, isSave))
            {
                ItemContainer.containerItems.Add(addItem);
                ItemContainer.AddItemEvent(addItem, isSave);
            }
        }
        else
        {
            addItem.OwnerContainer = ItemContainer;
            if (!AddToExistingStack(addItem, amount, isSave))
            {
                addItem.Amount = amount;
                ItemContainer.containerItems.Add(addItem);
                ItemContainer.AddItemEvent(addItem, isSave);
            }
        }
    }

    private bool AddToExistingStack(ItemData data, int amount, bool isSave)
    {
        foreach (ItemData item in ItemContainer.containerItems)
        {
            Debug.Log("Checking Id: " + item.Id + "  with ID: " + data.Id);

            if (item.Id.Equals(data.Id))
            {
                Debug.Log("add to existing stack");

                ItemContainer.ModifiedItemEvent(data, isSave);


                item.Amount = item.Amount + amount;
                data.Amount -= amount;

                return true;
            }
        }
        return false;
    }


}
