using UnityEngine;
using System.Collections;
using System;
using CloudGoods.SDK.Models;
using CloudGoods.Services.WebCommunication;
using System.Collections.Generic;

namespace CloudGoods.Services
{
    public class ItemStoreServices
    {
        public static void GetCurrencyInfo(Action<CurrencyInfoResponse> callback)
        {
            CallHandler.Instance.GetCurrencyInfo(callback);
        }

        public static void GetPremiumCurrencyBalance(Action<CurrencyBalanceResponse> callback)
        {
            CallHandler.Instance.GetPremiumCurrencyBalance(callback);
        }

        public static void GetStandardCurrencyBalance(int accessLocation, Action<SimpleItemInfo> callback)
        {
            CallHandler.Instance.GetStandardCurrencyBalance(accessLocation, callback);
        }

        public static void ConsumePremiumCurrency(int amount, Action<ConsumePremiumResponce> callback)
        {
            CallHandler.Instance.ConsumePremiumCurrency(amount, callback);
        }
        public static void GetStoreItems(Action<List<StoreItem>> callback, string andTags = null, string orTags = null)
        {
            CallHandler.Instance.GetStoreItems(andTags, orTags, callback);
        }

        public static void PurchaseItem(int itemId, int amount, int paymentOption, int saveLocation, Action<SimpleItemInfo> callback, int amountToConsume = -1)
        {
            CallHandler.Instance.PurchaseItem(itemId, amount, paymentOption, saveLocation, callback, amountToConsume);
        }

        public static void GetItemBundles(string andTags, string orTags, Action<ItemBundlesResponse> callback)
        {
            CallHandler.Instance.GetItemBundles(andTags, orTags, callback);
        }

        public static void PurchaseItemBundle(int bundleId, int paymentType, int location, Action<ItemBundlePurchaseResponse> callback)
        {
            CallHandler.Instance.PurchaseItemBundle(bundleId, paymentType, location, callback);
        }

        public static void GetPremiumBundles(int platformId, Action<List<PremiumCurrencyBundle>> callback)
        {
            CallHandler.Instance.GetPremiumBundles(platformId, callback);
        }

    }
}
