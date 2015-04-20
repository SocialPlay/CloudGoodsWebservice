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
                CurrencyManager.GetStandardCurrencyDetails(0, SetCurrencyLabel);
            }
            else if (type == CurrencyType.Premium)
            {
                CurrencyManager.GetPremiumCurrencyDetails(SetCurrencyLabel);
            }
        }

        void SetCurrencyLabel(string name, Texture2D icon)
        {
                   mLabel.text = string.Format("{0}{1}{2}", prefix, name, suffix);
        }
    }
}
