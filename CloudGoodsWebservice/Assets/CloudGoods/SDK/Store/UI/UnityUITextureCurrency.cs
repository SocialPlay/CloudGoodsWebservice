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
                CurrencyManager.GetStandardCurrencyDetails(0, SetCurrencyLabel);
            }
            else if (type == CurrencyType.Premium)
            {
                CurrencyManager.GetPremiumCurrencyDetails(SetCurrencyLabel);
            }
        }

        void SetCurrencyLabel(string name, Texture2D icon)
        {
            mTexture.texture = icon;
        }
    }
}
