﻿using UnityEngine;
using System.Collections;
using CloudGoods.Models;


namespace CloudGoods.Container
{

    [RequireComponent(typeof(ItemContainer))]
    public class BasicAddContainer : MonoBehaviour, IContainerAddAction
    {
        public ItemContainer ItemContainer;

        void Awake()
        {
            ItemContainer = GetComponent<ItemContainer>();
        }

        public void AddItem(OwnedItemInformation addItem, int amount, bool isSave)
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

        private bool AddToExistingStack(OwnedItemInformation data, int amount, bool isSave)
        {
            foreach (OwnedItemInformation item in ItemContainer.containerItems)
            {
                Debug.Log("Checking Id: " + item.Information.Id + "  with ID: " + data.Information.Id);

                if (item.Information.Id.Equals(data.Information.Id))
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
}
