using UnityEngine;
using System.Collections;
using CallHandler.Models;

[System.Serializable]
public abstract class ItemDataSelector 
{
    public abstract bool isItemSelected(ItemData item, IEnumerable tagList,bool isInverted = false);
}
