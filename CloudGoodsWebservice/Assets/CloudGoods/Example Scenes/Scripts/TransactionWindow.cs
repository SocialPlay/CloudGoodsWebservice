using UnityEngine;
using System.Collections;
using CloudGoods.CurrencyPurchase;
using CloudGoods.SDK.Models;
using UnityEngine.UI;

public class TransactionWindow : MonoBehaviour {

    public PremiumCurrencyBundleStore store;
    public GameObject transactionWindowObj;
    public Text transactionText;

    bool SetEvent = false;

    void Update()
    {
        if (store.platformPurchasor != null && SetEvent == false)
        {
            store.platformPurchasor.RecievedPurchaseResponse += OnTransactionSuccess;
            store.platformPurchasor.OnPurchaseErrorEvent += OnTransactionFail;
            SetEvent = true;
        }
    }

    void OnTransactionSuccess(PurchasePremiumCurrencyBundleResponse response)
    {
        transactionWindowObj.SetActive(true);
        transactionText.text = response.Message;
    }

    void OnTransactionFail(PurchasePremiumCurrencyBundleResponse response)
    {
        transactionWindowObj.SetActive(true);
        transactionText.text = response.Message;
    }

	public void CloseWindow()
    {
        transactionWindowObj.SetActive(false);
    }
}
