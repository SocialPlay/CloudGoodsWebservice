using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using CloudGoods.SDK.Models;
using CloudGoods.SDK.Container;

[TestFixture]
public class ContainerAddTests : MonoBehaviour
{

    List<ItemContainer> itemContainers;
    ContainerTestUtilities containerTestUtilities;

    [SetUp]
    public void Init()
    {
        containerTestUtilities = new ContainerTestUtilities();
        itemContainers = new List<ItemContainer>();
    }

    [Test]
    public void BasicAddItem_AddFullAmountEmptyContainer_AddItemToContainer()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(false, false, itemContainers);
        BasicAddContainer basicAdd = container.GetComponent<BasicAddContainer>();
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(10, 10, 10, 100, 10, 0, "Test Item", "123456", false);

        basicAdd.AddItem(itemData, itemData.Amount, false);

        Assert.AreEqual(1, container.containerItems.Count);
        Assert.AreEqual(10, container.containerItems[0].Amount);
        Assert.AreEqual("Test Item", container.containerItems[0].Information.Name);
    }

    [Test]
    public void BasicAddItem_AddPartialAmountEmptyContainer_AddItemToContainer()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(false, false, itemContainers);
        BasicAddContainer basicAdd = container.GetComponent<BasicAddContainer>();
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(10, 10, 10, 100, 10, 0, "Test Item", "123456", false);

        basicAdd.AddItem(itemData, 5, false);

        Assert.AreEqual(1, container.containerItems.Count);
        Assert.AreEqual(5, container.containerItems[0].Amount);
        Assert.AreEqual("Test Item", container.containerItems[0].Information.Name);
    }

    [Test]
    public void BasicAddItem_AddFullAmountToContainerWithItem_AddItemToContainer()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(false, false, itemContainers);
        BasicAddContainer basicAdd = container.GetComponent<BasicAddContainer>();
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(10, 10, 10, 100, 10, 0, "Test Item", "123456", false);

        basicAdd.AddItem(itemData, itemData.Amount, false);

        Assert.AreEqual(1, container.containerItems.Count);

        OwnedItemInformation itemDataTwo = containerTestUtilities.CreateItemData(20, 1, 1, 100, 1, 0, "Test Item Two", "654321", false);

        basicAdd.AddItem(itemDataTwo, itemDataTwo.Amount, false);

        Assert.AreEqual(2, container.containerItems.Count);
        Assert.AreEqual(20, container.containerItems[1].Amount);
        Assert.AreEqual("Test Item Two", container.containerItems[1].Information.Name);
    }

    [Test]
    public void BasicAddItem_AddPartialAmountToContainerWithItem_AddItemToContainer()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(false, false, itemContainers);
        BasicAddContainer basicAdd = container.GetComponent<BasicAddContainer>();
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(10, 10, 10, 100, 10, 0, "Test Item", "123456", false);

        basicAdd.AddItem(itemData, itemData.Amount, false);

        Assert.AreEqual(1, container.containerItems.Count);

        OwnedItemInformation itemDataTwo = containerTestUtilities.CreateItemData(20, 1, 1, 100, 1, 0, "Test Item Two", "654321", false);

        basicAdd.AddItem(itemDataTwo, 5, false);

        Assert.AreEqual(2, container.containerItems.Count);
        Assert.AreEqual(5, container.containerItems[1].Amount);
        Assert.AreEqual("Test Item Two", container.containerItems[1].Information.Name);
    }

    [Test]
    public void BasicAddItem_AddFullAmountToContainerWithSameItem_UpdateItemAmount()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(false, false, itemContainers);
        BasicAddContainer basicAdd = container.GetComponent<BasicAddContainer>();
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(10, 10, 10, 100, 10, 0, "Test Item", "123456", false);

        basicAdd.AddItem(itemData, itemData.Amount, false);

        Assert.AreEqual(1, container.containerItems.Count);

        OwnedItemInformation itemDataTwo = containerTestUtilities.CreateItemData(10, 10, 10, 100, 10, 0, "Test Item", "123456", false);

        basicAdd.AddItem(itemDataTwo, itemDataTwo.Amount, false);

        Assert.AreEqual(1, container.containerItems.Count);
        Assert.AreEqual(20, container.containerItems[0].Amount);
        Assert.AreEqual("Test Item", container.containerItems[0].Information.Name);
    }

    [Test]
    public void BasicAddItem_AddPartialAmountToContainerWithSameItem_UpdateItemAmount()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(false, false, itemContainers);
        BasicAddContainer basicAdd = container.GetComponent<BasicAddContainer>();
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(10, 10, 10, 100, 10, 0, "Test Item", "123456", false);

        basicAdd.AddItem(itemData, itemData.Amount, false);

        Assert.AreEqual(1, container.containerItems.Count);

        OwnedItemInformation itemDataTwo = containerTestUtilities.CreateItemData(10, 10, 10, 100, 10, 0, "Test Item", "123456", false);

        basicAdd.AddItem(itemDataTwo, 5, false);

        Assert.AreEqual(1, container.containerItems.Count);
        Assert.AreEqual(15, container.containerItems[0].Amount);
        Assert.AreEqual("Test Item", container.containerItems[0].Information.Name);
    }

    [Test]
    public void BasicAddItem_AddMoreThanAmountToContainer_AddMaxItemAmount()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(false, false, itemContainers);
        BasicAddContainer basicAdd = container.GetComponent<BasicAddContainer>();
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(10, 10, 10, 100, 10, 0, "Test Item", "123456", false);

        basicAdd.AddItem(itemData, 100, false);

        Assert.AreEqual(1, container.containerItems.Count);
        Assert.AreEqual(10, container.containerItems[0].Amount);
        Assert.AreEqual("Test Item", container.containerItems[0].Information.Name);
    }

    [TearDown]
    public void CleanUp()
    {
        foreach (ItemContainer itemContainer in itemContainers)
        {
            DestroyImmediate(itemContainer.gameObject);
        }
    }

}
