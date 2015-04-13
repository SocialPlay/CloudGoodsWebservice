using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CloudGoods.Enums;
using CloudGoods.SDK;

namespace CloudGoods.SDK.Store.UI
{
    [RequireComponent(typeof(Text))]
    public class UnityUICurrencyNameLabel : MonoBehaviour
    {
        public string prefix;
        public string suffix;
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
                   mLabel.text = string.Format("{0}{1}{2}", prefix, currencyInfo.Name, suffix);
        }
    }
}
