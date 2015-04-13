using System;
using UnityEngine;
using System.Collections.Generic;
using CloudGoods.SDK.Models;

namespace CloudGoods.SDK.Utilities
{
	public interface IGridLoader
	{
        event Action<PremiumCurrencyBundle, GameObject> ItemAdded;
        void LoadGrid(List<PremiumCurrencyBundle> data);
	}
}
