﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using CloudGoods.SDK.Models;
using CloudGoods.Enums;
using CloudGoods.SDK;
using CloudGoods.CurrencyPurchase;


namespace CloudGoods.SDK.Store.UI
{
    [RequireComponent(typeof(Text))]
    public class UnityUILabelCurrency : MonoBehaviour
    {

        public CurrencyType type = CurrencyType.Standard;
        Text mLabel;

        void OnEnable()
        {
            UnityUIItemPurchase.OnPurchasedItem += UnityUIItemPurchase_OnPurchasedItem;
            PremiumCurrencyBundleStore.OnPremiumCurrencyPurchased += PremiumCurrencyBundleStore_OnPremiumCurrencyPurchased;
        }

        void OnDisable()
        {
            UnityUIItemPurchase.OnPurchasedItem -= UnityUIItemPurchase_OnPurchasedItem;
        }

        void UnityUIItemPurchase_OnPurchasedItem(SimpleItemInfo obj)
        {
            UpdateLabels();
        }

        void PremiumCurrencyBundleStore_OnPremiumCurrencyPurchased(PurchasePremiumCurrencyBundleResponse obj)
        {
            UpdateLabels();
        }


        void Start()
        {
            UpdateLabels();
        }


        void UpdateLabels()
        {
            mLabel = GetComponent<Text>();
            if (type == CurrencyType.Standard)
            {
                CurrencyManager.GetStandardCurrencyBalance(0, SetCurrencyLabel);
            }
            else if (type == CurrencyType.Premium)
            {
                CurrencyManager.GetPremiumCurrencyBalance(SetCurrencyLabel);
            }
        }

        void SetCurrencyLabel(int amount)
        {
            mLabel.text = amount.ToString();
        }
    }
}
