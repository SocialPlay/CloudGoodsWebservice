using UnityEngine;
using System.Collections;
using CloudGoods.Models;

namespace CloudGoods.Container
{

    public interface IContainerAddAction
    {
        void AddItem(OwnedItemInformation addItem, int amount, bool isSave);
    }
}
