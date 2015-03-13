using UnityEngine;
using System.Collections;
using CloudGoodsClasses;

[System.Serializable]
public abstract class ItemDataSelector 
{
    public abstract bool isItemSelected(ItemData item, IEnumerable tagList,bool isInverted = false);
}
