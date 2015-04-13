using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CloudGoods.Services;
using CloudGoods.Services.WebCommunication;

namespace CloudGoods.SDK.Store.UI
{
    public class UnityUIPurchaseStatus : MonoBehaviour
    {

        public GameObject purchasePopup;

        // Use this for initialization
        void Awake()
        {
            UnityUIItemPurchase.OnPurchasedItem += UnityUIItemPurchase_OnPurchasedItem;
            CallHandler.IsError += CallHandler_IsError;
            //UnityUIBundlePurchasing.OnPurchaseSuccessful += UnityUIItemPurchase_OnPurchasedItem;
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

        public void ClosePopup()
        {
            purchasePopup.SetActive(false);
        }
    }
}
