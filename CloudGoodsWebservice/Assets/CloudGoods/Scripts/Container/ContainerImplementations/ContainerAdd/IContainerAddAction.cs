using UnityEngine;
using System.Collections;

namespace CloudGoods.Container
{

    public interface IContainerAddAction
    {
        void AddItem(CloudGoods.Models.ItemData addItem, int amount, bool isSave);
    }
}
