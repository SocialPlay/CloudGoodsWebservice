using UnityEngine;
using System.Collections;
using CloudGoods.SDK.Models;


namespace CloudGoods.SDK.Item
{
    [System.Serializable]
    public abstract class ItemDataSelector
    {
        public abstract bool isItemSelected(ItemInformation item, IEnumerable tagList, bool isInverted = false);
    }
}
