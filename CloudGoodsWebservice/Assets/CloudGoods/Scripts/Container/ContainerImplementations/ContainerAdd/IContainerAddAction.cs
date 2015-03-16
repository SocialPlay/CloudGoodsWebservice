using UnityEngine;
using System.Collections;

public interface IContainerAddAction {
    void AddItem(CallHandler.Models.ItemData addItem, int amount, bool isSave);
}
