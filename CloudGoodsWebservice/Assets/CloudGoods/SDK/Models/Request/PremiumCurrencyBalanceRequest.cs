using UnityEngine;
using System.Collections;

namespace CloudGoods.SDK.Models
{

    public class PremiumCurrencyBalanceRequest : IRequestClass
    {
        public string ToHashable()
        {
            return "PremiumCurrencyBalanceRequest";
        }
    }
}
