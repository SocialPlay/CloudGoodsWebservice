using UnityEngine;
using System.Collections;

public interface IContainerAddAction {
    void AddItem(CloudGoodsClasses.ItemData addItem, int amount, bool isSave);
}
