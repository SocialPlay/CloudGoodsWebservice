using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using CloudGoods.SDK.Models;
using CloudGoods.Enums;
using CloudGoods.SDK;

namespace CloudGoods.SDK.Store.UI
{
    [RequireComponent(typeof(RawImage))]
    public class UnityUITextureCurrency : MonoBehaviour
    {

        public CurrencyType type = CurrencyType.Standard;
        RawImage mTexture;

        void Start()
        {
            mTexture = GetComponent<RawImage>();
            if (type == CurrencyType.Standard)
            {
                CurrencyManager.GetStandardCurrency(0, SetCurrencyLabel);
            }
            else if (type == CurrencyType.Premium)
            {
                CurrencyManager.GetPremiumCurrency(0, SetCurrencyLabel);
            }
        }

        void SetCurrencyLabel(CurrencyManager.CurrencyInfo currencyInfo)
        {
            mTexture.texture = currencyInfo.Icon;
        }
    }
}
