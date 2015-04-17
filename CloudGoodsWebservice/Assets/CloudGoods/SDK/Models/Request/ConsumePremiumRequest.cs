using UnityEngine;
using System.Collections;

namespace CloudGoods.SDK.Models
{
    public class ConsumePremiumRequest : IRequestClass
    {
        public int Amount;

        public string ToHashable()
        {
            return Amount.ToString();
        }
        public ConsumePremiumRequest(int amount)
        {
            Amount = amount;
        }
    }
}
