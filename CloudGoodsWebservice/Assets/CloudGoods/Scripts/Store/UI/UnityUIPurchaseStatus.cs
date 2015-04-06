using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CloudGoods.Models;

namespace CloudGoods.Store.UI
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

        void CallHandler_IsError(Models.WebserviceError obj)
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
