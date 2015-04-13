using UnityEngine;
using System.Collections;
using CloudGoods.SDK.Models;

namespace CloudGoods.SDK.Container
{

    public interface IContainerAddAction
    {
        void AddItem(OwnedItemInformation addItem, int amount, bool isSave);
    }
}
