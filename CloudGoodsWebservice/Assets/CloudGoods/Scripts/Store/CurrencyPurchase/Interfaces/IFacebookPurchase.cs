using UnityEngine;
using System.Collections;
using System;


namespace CloudGoods.CurrencyPurchase
{
    public interface IFacebookPurchase
    {

        void Init();

        void Purchase(PremiumBundle bundleItem, int amount, Action<string> callback);

    }
}
