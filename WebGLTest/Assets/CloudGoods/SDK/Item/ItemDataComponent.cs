using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CloudGoods.SDK.Models;


namespace CloudGoods.SDK.Item
{
    public class ItemDataComponent : MonoBehaviour
    {
        public Action<OwnedItemInformation> onPickup;
        public bool addOnPickup = true;
        public bool pickupOnClick = true;
        public bool destroyOnPickup = true;
        [HideInInspector]
        public bool isValid = true;

        public OwnedItemInformation itemData
        {
            get
            {
                if (mData == null)
                {
                    mData = new OwnedItemInformation();
                    //mData.uiReference = this;
                }
                return mData;
            }
            set
            {
                mData = value;
                //mData.uiReference = this;
                SetData(mData);
            }
        }

        protected OwnedItemInformation mData;

        /// <summary>
        /// if pickupOnClick is true the item can be picked up on Click event.
        /// </summary>

        void OnClick()
        {
            Debug.Log("ON CLICK");

            if (pickupOnClick) Pickup(addOnPickup);
        }

        public virtual void SetData(OwnedItemInformation itemData) { }

        /// <summary>
        /// Convenient method to use it to pickup items.
        /// </summary>

        public void Pickup(bool addToContainer)
        {
            if (onPickup != null) onPickup(itemData);

            //if (addToContainer) GetItemsContainerInserter.instance.GetGameItem(new List<ItemData>(new ItemData[1] { itemData }));

            if (destroyOnPickup) GameObject.Destroy(gameObject);
        }
    }
}
