using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ItemContainer))]
public class BasicAddContainer : MonoBehaviour, IContainerAddAction
{
    public ItemContainer itemContainer;

    void Awake()
    {
        itemContainer = GetComponent<ItemContainer>();
    }

    public void AddItem(ItemData addItem, int amount, bool isSave)
    {
        if (amount == -1)
        {
            amount = addItem.Amount;
            addItem.OwnerContainer = itemContainer;
            if (!AddToExistingStack(addItem, addItem.Amount, isSave))
            {
                itemContainer.containerItems.Add(addItem);
                itemContainer.AddItemEvent(addItem, isSave);
            }
        }
        else
        {
            addItem.OwnerContainer = itemContainer;
            if (!AddToExistingStack(addItem, amount, isSave))
            {
                addItem.Amount = amount;
                itemContainer.containerItems.Add(addItem);
                itemContainer.AddItemEvent(addItem, isSave);
            }
        }
    }

    private bool AddToExistingStack(ItemData data, int amount, bool isSave)
    {
        foreach (ItemData item in itemContainer.containerItems)
        {
            Debug.Log("Checking Id: " + item.Id + "  with ID: " + data.Id);

            if (item.Id.Equals(data.Id))
            {
                Debug.Log("add to existing stack");

                itemContainer.ModifiedItemEvent(data, isSave);


                item.Amount = item.Amount + amount;
                data.Amount -= amount;

                return true;
            }
        }
        return false;
    }


}
