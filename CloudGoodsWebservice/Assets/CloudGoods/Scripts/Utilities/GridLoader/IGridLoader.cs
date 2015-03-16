using System;
using UnityEngine;
using System.Collections.Generic;
using CallHandler.Models;

namespace SocialPlay.Generic
{
	public interface IGridLoader
	{
        event Action<PaidCurrencyBundleItem, GameObject> ItemAdded;
        void LoadGrid(List<PaidCurrencyBundleItem> data);
	}
}
