using System;
using UnityEngine;
using System.Collections.Generic;
using CloudGoods.Models;

namespace SocialPlay.Utilities
{
	public interface IGridLoader
	{
        event Action<PaidCurrencyBundleItem, GameObject> ItemAdded;
        void LoadGrid(List<PaidCurrencyBundleItem> data);
	}
}
