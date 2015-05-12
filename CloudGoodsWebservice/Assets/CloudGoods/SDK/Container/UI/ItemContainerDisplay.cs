using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using CloudGoods.SDK.Models;
using CloudGoods.Services;
using CloudGoods.SDK.Item.UI;
using CloudGoods.SDK.Item;

namespace CloudGoods.SDK.Container
{

    public class ItemContainerDisplay : MonoBehaviour
    {
        public ItemContainer MyContainer;
        // public GameObject displayObject;
        public Transform ChildTarget;

        private List<ItemDataDisplay> currentDisplayObjects = new List<ItemDataDisplay>();

        /// <summary>
        /// Called After the user clicks an item
        /// </summary>
        public event Action<ItemDataComponent> ItemSingleClicked;

        /// <summary>
        /// Called after the user double clicks an item
        /// </summary>
        public event Action<ItemDataComponent> ItemDoubleClicked;

        /// <summary>
        /// Called after the user right clicks an item
        /// </summary>
        public event Action<ItemDataComponent> ItemRightClicked;

        /// <summary>
        /// Called after a user presses a key that is linked to the Container(only for slotted containers)
        /// </summary>
        public event Action<ItemDataComponent> ItemKeyBindingClicked;


        void OnEnable()
        {
            MyContainer.AddedItem += AddedItem;
            MyContainer.ModifiedItem += ModifiedItem;
            MyContainer.RemovedItem += RemovedItem;
            MyContainer.ClearItems += ClearItems;
        }

        void OnDisable()
        {
            MyContainer.AddedItem -= AddedItem;
            MyContainer.ModifiedItem -= ModifiedItem;
            MyContainer.RemovedItem -= RemovedItem;
            MyContainer.ClearItems -= ClearItems;
        }

        public virtual void AddedItem(OwnedItemInformation itemData, bool isSaving)
        {
            GameObject newItem = GameObject.Instantiate(CloudGoodsSettings.DefaultUIItem) as GameObject;
            ItemDataDisplay newDisplay = newItem.GetComponent<ItemDataDisplay>();
            newDisplay.itemObject = newItem.GetComponent<ItemDataComponent>();
            newItem.GetComponent<ItemDataComponent>().itemData = itemData;
            newItem.name = itemData.Information.Name;
            newItem.transform.SetParent(ChildTarget);
            newItem.transform.localPosition = Vector3.zero;
            newItem.transform.localScale = Vector3.one;
            currentDisplayObjects.Add(newDisplay);
        }


        public virtual void ModifiedItem(OwnedItemInformation itemData, bool isSaving)
        {

            foreach (ItemDataDisplay display in currentDisplayObjects)
            {
                if (display.itemObject.itemData.IsSameItemAs(itemData))
                {
                    display.SetAmountText(display.itemObject.itemData.Amount.ToString());
                    return;
                }
            }
        }


        public virtual void RemovedItem(OwnedItemInformation itemData, int amount, bool arg3)
        {
            ItemDataDisplay selected = FindDisplayMatch(itemData);
            if (selected != null)
            {
                if (itemData.Amount <= 0)
                {
                    currentDisplayObjects.Remove(selected);
                    Destroy(selected.gameObject);
                }
            }
        }

        public virtual void ClearItems()
        {
            foreach (ItemDataDisplay item in currentDisplayObjects)
            {
                Destroy(item.gameObject);
            }
            currentDisplayObjects.Clear();
        }


        void Start()
        {
            if (CloudGoodsSettings.DefaultUIItem == null)
            {
                Debug.LogError("Default UI Item is not set int the settings file");
                this.gameObject.SetActive(false);
            }
        }

        public ItemDataDisplay FindDisplayMatch(OwnedItemInformation item)
        {
            foreach (ItemDataDisplay itemDisplay in currentDisplayObjects)
            {
                if (itemDisplay.itemObject.itemData == item)
                {
                    return itemDisplay;
                }
            }
            return null;
        }


        public void PerformSingleClick(ItemDataComponent item)
        {
            if (ItemSingleClicked != null)
            {
                ItemSingleClicked(item);
            }
        }

        public void PerformDoubleClick(ItemDataComponent item)
        {
            if (ItemDoubleClicked != null)
            {
                ItemDoubleClicked(item);
            }
        }

        public void PerformRightClick(ItemDataComponent item)
        {
            if (ItemRightClicked != null)
            {
                ItemRightClicked(item);
            }
        }

        public void PerformKeybindingClick(ItemDataComponent item)
        {
            if (ItemKeyBindingClicked != null)
            {
                ItemKeyBindingClicked(item);
            }
        }
    }
}
