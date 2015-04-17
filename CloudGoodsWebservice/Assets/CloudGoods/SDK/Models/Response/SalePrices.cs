using UnityEngine;
using System.Collections;
using System;

namespace CloudGoods.SDK.Models
{
    public class SalePrices
    {
        public int PremiumCurrencySaleValue;
        public int StandardCurrencySaleValue;
        public string SaleName;

        public DateTime SaleStartDate;
        public DateTime SaleEndDate;
    }
}
