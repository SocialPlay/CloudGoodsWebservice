using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CloudGoods.SDK.Models
{
    public class PremiumCurrencyBundle
    {
        public int ID;
        public string Name;
        public string Image;
        public string Description;
        public double Cost;
        public int CreditAmount;
        public string Currency;
        public List<KeyValuePair<string, string>> Data;
    }
}
