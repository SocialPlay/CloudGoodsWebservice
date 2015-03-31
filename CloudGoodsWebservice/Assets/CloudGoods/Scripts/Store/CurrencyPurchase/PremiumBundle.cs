﻿using UnityEngine;
using System.Collections;
using System;

namespace CloudGoods.CurrencyPurchase
{
    public abstract class PremiumBundle : MonoBehaviour
    {

        public abstract string Amount { get; set; }

        public abstract Texture CurrencyIcon { get; set; }

        public abstract string PremiumCurrencyName { get; set; }

        public abstract string PremiumBundleName { get; set; }

        public abstract string Cost { get; set; }

        public abstract string Description { get; set; }

        public Action<PremiumBundle> OnPurchaseRequest;


        public string ProductID { get; set; }
        public string BundleID { get; set; }


        void Start()
        {
            if (string.IsNullOrEmpty(CallHandler.PremiumCurrencyName))
            {
                CallHandler.OnPremiumCurrencyName += OnPremiumCurrencyName;
            }
            else
            {
                PremiumCurrencyName = CallHandler.PremiumCurrencyName;
            }
        }

        void OnPremiumCurrencyName(string premiumName)
        {
            PremiumCurrencyName = premiumName;
        }


        public void MakePurchaseRequest()
        {
            if (OnPurchaseRequest != null) OnPurchaseRequest(this);
        }

        public abstract void SetIcon(Texture2D texture);

        public abstract void SetBundleName(string name);
    }
}
