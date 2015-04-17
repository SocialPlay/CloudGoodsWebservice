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

        public static void GetStandardCurrencyBalance(StandardCurrencyBalanceRequest request, Action<SimpleItemInfo> callback)
        {
            CallHandler.Instance.GetStandardCurrencyBalance(request, callback);
        }

        public static void ConsumePremiumCurrency(ConsumePremiumRequest request, Action<ConsumePremiumResponce> callback)
        {
            CallHandler.Instance.ConsumePremiumCurrency(request, callback);
        }
        public static void GetStoreItems(StoreItemsRequest request, Action<List<StoreItem>> callback)
        {
            CallHandler.Instance.GetStoreItems(request, callback);
        }

        public static void PurchaseItem(PurchaseItemRequest request, Action<SimpleItemInfo> callback)
        {
            CallHandler.Instance.PurchaseItem(request, callback);
        }

        public static void GetItemBundles(ItemBundlesRequest request, Action<ItemBundlesResponse> callback)
        {
            CallHandler.Instance.GetItemBundles(request, callback);
        }

        public static void PurchaseItemBundle(ItemBundlePurchaseRequest request, Action<ItemBundlePurchaseResponse> callback)
        {
            CallHandler.Instance.PurchaseItemBundle(request, callback);
        }

        public static void GetPremiumBundles(PremiumBundlesRequest request, Action<List<PremiumCurrencyBundle>> callback)
        {
            CallHandler.Instance.PremiumBundles(request, callback);
        }

    }
}
