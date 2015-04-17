using UnityEngine;
using System.Collections;
namespace CloudGoods.SDK.Models
{

    public class StandardCurrencyBalanceRequest : IRequestClass
    {
        public int AccessLocation;

        public string ToHashable()
        {
            return AccessLocation.ToString();
        }

        public StandardCurrencyBalanceRequest(int accessLocation)
        {
            AccessLocation = accessLocation;
        }
    }
}
