﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using CloudGoods.Models;
using CloudGoods.Enums;

namespace CloudGoods.Store.UI
{
    [RequireComponent(typeof(Text))]
    public class UnityUILabelCurrency : MonoBehaviour
    {

        public CurrencyType type = CurrencyType.Standard;
        Text mLabel;

        void Awake()
        {
            mLabel = GetComponent<Text>();
            if (type == CurrencyType.Standard) CallHandler.OnStandardCurrency += OnFreeCurrency;
            else if (type == CurrencyType.Premium) CallHandler.OnPremiumCurrency += OnPaidCurrency;
        }

        void OnFreeCurrency(int currency)
        {
            mLabel.text = currency.ToString();
        }

        void OnPaidCurrency(int currency)
        {
            mLabel.text = currency.ToString();
        }
    }
}
