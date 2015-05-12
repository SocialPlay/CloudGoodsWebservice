using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CloudGoods.Services;
using CloudGoods.Services.WebCommunication;
using CloudGoods.SDK.Models;
using CloudGoods.ItemBundles;

namespace CloudGoods.SDK.Store.UI
{
    public class UnityUIPurchaseStatus : MonoBehaviour
    {

        public GameObject purchasePopup;

        // Use this for initialization
        void Awake()
        {
            UnityUIItemPurchase.OnPurchasedItem += UnityUIItemPurchase_OnPurchasedItem;
            UnityUIBundlePurchasing.OnPurchaseSuccessful += UnityUIBundlePurchaseSuccessful;
            CallHandler.IsError += CallHandler_IsError;
        }

        void CallHandler_IsError(SDK.Models.WebserviceError obj)
        {
            if(obj.ErrorCode == 500)
            {
                purchasePopup.SetActive(true);
                purchasePopup.GetComponentInChildren<Text>().text = obj.Message;
            }
        }

        void UnityUIItemPurchase_OnPurchasedItem(SimpleItemInfo obj)
        {
            purchasePopup.SetActive(true);
            purchasePopup.GetComponentInChildren<Text>().text = "Purchase Successful";
        }

        void UnityUIBundlePurchaseSuccessful(ItemBundlePurchaseResponse response)
        {
            purchasePopup.SetActive(true);
            purchasePopup.GetComponentInChildren<Text>().text = "Purchase Successful";
        }

        public void ClosePopup()
        {
            purchasePopup.SetActive(false);
        }
    }
}
