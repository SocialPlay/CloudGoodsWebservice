using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using CloudGoods.SDK.Models;
using CloudGoods.Enums;
using CloudGoods.SDK;


namespace CloudGoods.SDK.Store.UI
{
    [RequireComponent(typeof(Text))]
    public class UnityUILabelCurrency : MonoBehaviour
    {

        public CurrencyType type = CurrencyType.Standard;
        Text mLabel;

        void Start()
        {
            mLabel = GetComponent<Text>();
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
            mLabel.text = currencyInfo.Amount.ToString();
        }
    }
}
