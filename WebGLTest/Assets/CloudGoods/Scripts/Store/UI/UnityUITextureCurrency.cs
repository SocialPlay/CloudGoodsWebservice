using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using CloudGoods.Models;
using CloudGoods.Enums;

namespace CloudGoods.Store.UI
{
    [RequireComponent(typeof(RawImage))]
    public class UnityUITextureCurrency : MonoBehaviour
    {

        public CurrencyType type = CurrencyType.Standard;
        RawImage mTexture;

        void Awake()
        {
            mTexture = GetComponent<RawImage>();
            if (type == CurrencyType.Standard)
            {
                CallHandler.OnStandardCurrencyTexture += OnFreeCurrency;
                mTexture.texture = CallHandler.standardCurrencyTexture;
            }
            else if (type == CurrencyType.Premium)
            {
                CallHandler.OnPremiumCurrencyTexture += OnPaidCurrency;
                mTexture.texture = CallHandler.premiumCurrencyTexture;
            }
        }

        void OnFreeCurrency(Texture2D currencyTexture)
        {
            mTexture.texture = currencyTexture;
        }

        void OnPaidCurrency(Texture2D currencyTexture)
        {
            mTexture.texture = currencyTexture;
        }
    }
}
