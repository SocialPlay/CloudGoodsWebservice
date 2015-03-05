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

    [SetUp]
    public void Init()
    {

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

    private ItemContainer SetUpContainer()
    {
        GameObject containerObj = new GameObject();
        containerObj.name = "Item Container";
        ItemContainer container = containerObj.AddComponent<ItemContainer>();
        container.containerAddAction = containerObj.AddComponent<BasicAddContainer>();
        BasicAddContainer containerAdd = (BasicAddContainer)container.containerAddAction;
        containerAdd.itemContainer = container;

        return container;
    }

    [Test]
    public void BasicAddItemDataToContainerUpdatesContainerItemList()
    {
        ItemData testItemOne = CreateItemData(1, 10, 10, 100, 10, 0, "Item One", "123456");

        ItemContainerOne = SetUpContainer();

        ItemContainerManager.AddItem(testItemOne, ItemContainerOne);

        Assert.AreEqual(1, ItemContainerOne.containerItems.Count);
        Assert.AreEqual("Item One", ItemContainerOne.containerItems[0].Name);
    }

    [Test]
    public void RemoveItemDataFromContainerRemovesDisplay()
    {
        ItemData testItemOne = CreateItemData(1, 10, 10, 100, 10, 0, "Item One", "123456");

        ItemContainerOne = SetUpContainer();

        ItemContainerManager.AddItem(testItemOne, ItemContainerOne);

        Assert.AreEqual(1, ItemContainerOne.containerItems.Count);
        Assert.AreEqual("Item One", ItemContainerOne.containerItems[0].Name);

        ItemContainerManager.RemoveItem(testItemOne, ItemContainerOne);

        Assert.AreEqual(0, ItemContainerOne.containerItems.Count);
    }

    [Test]
    public void MoveItemDataFromContainerOneToContainerTwoUpdatesDisplay()
    {
        ItemContainerOne = SetUpContainer();
        ItemContainerTwo = SetUpContainer();

        ItemData testItemOne = CreateItemData(1, 10, 10, 100, 10, 0, "Item One", "123456");

        ItemContainerManager.AddItem(testItemOne, ItemContainerOne);

        Assert.AreEqual(1, ItemContainerOne.containerItems.Count);
        Assert.AreEqual("Item One", ItemContainerOne.containerItems[0].Name);

        ItemContainerManager.MoveItem(testItemOne, ItemContainerTwo);

        Assert.AreEqual(0, ItemContainerOne.containerItems.Count);
        Assert.AreEqual(1, ItemContainerTwo.containerItems.Count);
        Assert.AreEqual("Item One", ItemContainerTwo.containerItems[0].Name);

    }

    [Test]
    public void MoveItemDataFromContainerMergeWithExistsingItem()
    {
        ItemContainerOne = SetUpContainer();

        ItemData testItemOne = CreateItemData(1, 10, 10, 100, 10, 0, "Item One", "123456");
        ItemData testItemTwo = CreateItemData(1, 10, 10, 100, 10, 0, "Item One", "123456");

        ItemContainerManager.AddItem(testItemOne, ItemContainerOne);

        Assert.AreEqual(1, ItemContainerOne.containerItems.Count);
        Assert.AreEqual("Item One", ItemContainerOne.containerItems[0].Name);
        Assert.AreEqual(1, ItemContainerOne.containerItems[0].Amount);

        ItemContainerManager.MoveItem(testItemTwo, ItemContainerOne);

        Assert.AreEqual(1, ItemContainerOne.containerItems.Count);
        Assert.AreEqual("Item One", ItemContainerOne.containerItems[0].Name);
        Assert.AreEqual(2, ItemContainerOne.containerItems[0].Amount);

    }

    [Test]
    public void MoveToItemContainerTwoThenThreeUpdatesDisplay()
    {
        ItemContainerOne = SetUpContainer();
        ItemContainerTwo = SetUpContainer();
        ItemContainerThree = SetUpContainer();
        ItemData testItemOne = CreateItemData(1, 10, 10, 100, 10, 0, "Item One", "123456");


        ItemContainerManager.MoveItem(testItemOne, ItemContainerOne);

        Assert.AreEqual(1, ItemContainerOne.containerItems.Count);
        Assert.AreEqual("Item One", ItemContainerOne.containerItems[0].Name);

        ItemContainerManager.MoveItem(testItemOne, ItemContainerTwo);

        Assert.AreEqual(0, ItemContainerOne.containerItems.Count);
        Assert.AreEqual(1, ItemContainerTwo.containerItems.Count);
        Assert.AreEqual("Item One", ItemContainerTwo.containerItems[0].Name);

        ItemContainerManager.MoveItem(testItemOne, ItemContainerThree);

        Assert.AreEqual(0, ItemContainerTwo.containerItems.Count);
        Assert.AreEqual(1, ItemContainerThree.containerItems.Count);
        Assert.AreEqual("Item One", ItemContainerThree.containerItems[0].Name);
    }

    [Test]
    public void AddTwoItemsToSingleContainerUpdatesContainerItemList()
    {
        ItemContainerOne = SetUpContainer();

        ItemData testItemOne = CreateItemData(1, 10, 10, 100, 10, 0, "Item One", "123456");
        ItemData testItemTwo = CreateItemData(1, 15, 15, 100, 15, 0, "Item Two", "654321");

        ItemContainerManager.MoveItem(testItemOne, ItemContainerOne);
        ItemContainerManager.MoveItem(testItemTwo, ItemContainerOne);

        Assert.AreEqual(2, ItemContainerOne.containerItems.Count);
        Assert.AreEqual("Item One", ItemContainerOne.containerItems[0].Name);
        Assert.AreEqual("Item Two", ItemContainerOne.containerItems[1].Name);
    }
}
