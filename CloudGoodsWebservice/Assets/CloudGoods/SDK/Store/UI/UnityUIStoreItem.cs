using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using CloudGoods.SDK.Models;
using CloudGoods.Enums;
using CloudGoods.SDK.Utilities;

namespace CloudGoods.SDK.Store.UI
{
    public class UnityUIStoreItem : MonoBehaviour
    {

        public StoreItem storeItem { get; private set; }

        public GameObject SalePanel;
        public Text SaleName;

        UnityUIStoreLoader storeLoader;

        //public UISprite itemImageSprite;


        //protected virtual void OnEnable()
        //{
        //    if (itemID != 0) SP.OnStoreListLoaded += OnStoreListLoaded;
        //}

        //protected virtual void OnDisable()
        //{
        //    if (itemID != 0) SP.OnStoreListLoaded -= OnStoreListLoaded;
        //}

        //protected virtual void OnStoreListLoaded(List<StoreItem> storeList)
        //{
        //    SetItemData(SP.GetStoreItem(itemID));
        //}

        void OnReceivedItemTexture(Texture2D texture)
        {
            if (gameObject == null) return;

            RawImage uiTexture = gameObject.GetComponentInChildren<RawImage>();
            uiTexture.texture = texture;
        }

        public virtual void Init(StoreItem item, UnityUIStoreLoader unityStoreLoader)
        {
            //if (nameLabel != null) nameLabel.text = item.itemName;
            //if(descriptionLabel != null) descriptionLabel.text = item. <-- There is no description on StoreItems. This is a must have.
            storeItem = item;
            storeLoader = unityStoreLoader;
            ItemTextureCache.GetItemTexture(storeItem.ItemInformation.ImageName, OnReceivedItemTexture);

            SetDisplayForSale();
        }

        void SetDisplayForSale()
        {
            if (storeItem.Sale.Count > 0)
            {
                SalePanel.SetActive(true);
                SaleName.text = storeItem.Sale[0].SaleName;
            }
        }

        public void OnStoreItemClicked()
        {
            storeLoader.DisplayItemPurchasePanel(gameObject);
        }
    }
}
