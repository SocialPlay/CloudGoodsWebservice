using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System;
using System.Collections.Generic;


[TestFixture]
[Category("Container Tests")]
public class ContainerTests : MonoBehaviour {

    ItemContainer ItemContainerOne;
    ItemContainer ItemContainerTwo;
    ItemContainer ItemContainerThree;

    ItemData ItemDataOne;
    ItemData ItemDataTwo;

    [SetUp]
    public void Init()
    {
        ItemContainerOne = new ItemContainer();
        ItemContainerTwo = new ItemContainer();
        ItemContainerThree = new ItemContainer();

        ItemDataOne = CreateItemData(1, 10, 100, 20, 1, 0, "Item One", "123456");

        ItemDataTwo = CreateItemData(2, 20, 200, 30, 2, 0, "Item Two", "654321");
    }

    private ItemData CreateItemData(int amount, int classId, int collectionId, int energy, int id, int location, string name, string stackLocationId)
    {
        ItemData tmpData = new ItemData()
        {
            Amount = amount,
            ClassId = classId,
            CollectionId = collectionId,
            Detail = "Some Details Here",
            Energy = energy,
            Id = id,
            Location = location,
            Name = name,
            StackLocationId = stackLocationId
        };

        for (int i = 0; i < 3; i++)
        {
            tmpData.behaviours.Add(new ItemData.Behaviours() { name = i.ToString(), Id = i });
        }

        for (int i = 0; i < 3; i++)
        {
            tmpData.tags.Add(new ItemData.Tag() { name = i.ToString(), Id = i });
        }

        return tmpData;
    }

    [Test]
    public void AddItemDataToContainerUpdatesContainerItemList()
    {
        ItemContainerOne = new ItemContainer();

        ItemContainerManager.MoveItem(ItemDataOne, null, ItemContainerOne);

        Assert.AreEqual(1, ItemContainerOne.containerItems.Count);
        Assert.AreEqual("Item One", ItemContainerOne.containerItems[0].Name);
    }

    [Test]
    public void RemoveItemDataFromContainerRemovesDisplay()
    {
        ItemContainerOne = new ItemContainer();

        ItemContainerManager.MoveItem(ItemDataOne, null, ItemContainerOne);

        Assert.AreEqual(1, ItemContainerOne.containerItems.Count);
        Assert.AreEqual("Item One", ItemContainerOne.containerItems[0].Name);

        ItemContainerManager.RemoveItem(ItemContainerOne, ItemDataOne);

        Assert.AreEqual(0, ItemContainerOne.containerItems.Count);
    }

    [Test]
    public void MoveItemDataFromContainerOneToContainerTwoUpdatesDisplay()
    {
        ItemContainerOne = new ItemContainer();
        ItemContainerTwo = new ItemContainer();

        ItemContainerManager.MoveItem(ItemDataOne, null, ItemContainerOne);

        Assert.AreEqual(1, ItemContainerOne.containerItems.Count);
        Assert.AreEqual("Item One", ItemContainerOne.containerItems[0].Name);

        ItemContainerManager.MoveItem(ItemDataOne, ItemContainerOne, ItemContainerTwo);

        Assert.AreEqual(0, ItemContainerOne.containerItems.Count);
        Assert.AreEqual(1, ItemContainerTwo.containerItems.Count);
        Assert.AreEqual("Item One", ItemContainerTwo.containerItems[0].Name);

    }

    [Test]
    public void MoveItemDataFromContainerMergeWithExistsingItem()
    {
        ItemContainerOne = new ItemContainer();

        ItemContainerManager.MoveItem(ItemDataOne, null, ItemContainerOne);

        Assert.AreEqual(1, ItemContainerOne.containerItems.Count);
        Assert.AreEqual("Item One", ItemContainerOne.containerItems[0].Name);
        Assert.AreEqual(1, ItemContainerOne.containerItems[0].Amount);

    }

    [Test]
    public void MoveToItemContainerTwoThenThreeUpdatesDisplay()
    {
        ItemContainerOne = new ItemContainer();
        ItemContainerTwo = new ItemContainer();
        ItemContainerThree = new ItemContainer();

        ItemContainerManager.MoveItem(ItemDataOne, null, ItemContainerOne);

        Assert.AreEqual(1, ItemContainerOne.containerItems.Count);
        Assert.AreEqual("Item One", ItemContainerOne.containerItems[0].Name);

        ItemContainerManager.MoveItem(ItemDataOne, ItemContainerOne, ItemContainerTwo);

        Assert.AreEqual(0, ItemContainerOne.containerItems.Count);
        Assert.AreEqual(1, ItemContainerTwo.containerItems.Count);
        Assert.AreEqual("Item One", ItemContainerTwo.containerItems[0].Name);

        ItemContainerManager.MoveItem(ItemDataOne, ItemContainerTwo, ItemContainerThree);

        Assert.AreEqual(0, ItemContainerTwo.containerItems.Count);
        Assert.AreEqual(1, ItemContainerThree.containerItems.Count);
        Assert.AreEqual("Item One", ItemContainerThree.containerItems[0].Name);
    }

    [Test]
    public void AddTwoItemsToSingleContainerUpdatesContainerItemList()
    {
        ItemContainerOne = new ItemContainer();

        ItemContainerManager.MoveItem(ItemDataOne, null, ItemContainerOne);
        ItemContainerManager.MoveItem(ItemDataTwo, null, ItemContainerOne);

        Assert.AreEqual(2, ItemContainerOne.containerItems.Count);
        Assert.AreEqual("Item One", ItemContainerOne.containerItems[0].Name);
        Assert.AreEqual("Item Two", ItemContainerOne.containerItems[1].Name);
    }
}
