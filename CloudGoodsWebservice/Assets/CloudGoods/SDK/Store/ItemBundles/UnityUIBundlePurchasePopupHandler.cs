using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CloudGoods.SDK.Models;

namespace CloudGoods.ItemBundles
{
    public class UnityUIBundlePurchasePopupHandler : MonoBehaviour
    {

        //public PremiumCurrencyBundleStore bundleStore;
        public GameObject PurchaseWindow;

        public Text purchaseMessage;

        bool platformPurchaserSet = false;

        void OnEnable()
        {
            UnityUIBundlePurchasing.OnPurchaseSuccessful += RecievedPurchaseResponse;
        }

        void OnDisable()
        {
            UnityUIBundlePurchasing.OnPurchaseSuccessful -= RecievedPurchaseResponse;
        }

        void RecievedPurchaseResponse(ItemBundlePurchaseResponse obj)
        {
            Debug.Log("Purchase popup event called");

            PurchaseWindow.SetActive(true);

            if (obj.StatusCode == 1)
                purchaseMessage.text = "Purchase Successful";
            else
                purchaseMessage.text = "Error Code: " +  obj.StatusCode;
        }

        public void CloseWindow()
        {
            PurchaseWindow.SetActive(false);
        }
    }
}
