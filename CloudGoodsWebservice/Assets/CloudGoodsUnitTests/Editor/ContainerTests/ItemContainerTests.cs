using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using CloudGoods.SDK.Models;
using CloudGoods.SDK.Container;

[TestFixture]
public class ItemContainerTests : MonoBehaviour {

    List<ItemContainer> ItemContainers;
    ContainerTestUtilities containerTestUtilities;

    [SetUp]
    public void Init()
    {
        ItemContainers = new List<ItemContainer>();
        containerTestUtilities = new ContainerTestUtilities();
    }

    [Test]
    public void GetContainerAddState_ValidAddItem_ReturnsAdd()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(true, false, ItemContainers);
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(10, 11, 10, 10, 10, 0, "Test Item", "123456", false);

        Assert.AreEqual(ContainerMoveState.ActionState.Add, container.GetContainerAddState(itemData).ContainerActionState);
    }

    [Test]
    public void GetContainerAddState_RestrictedItem_ReturnsNo()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(true, false, ItemContainers);
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(10, 10, 10, 10, 10, 0, "Test Item", "123456", false);

        Assert.AreEqual(ContainerMoveState.ActionState.No, container.GetContainerAddState(itemData).ContainerActionState);
    }

    [Test]
    public void GetContainerAddState_NoRestriction_ReturnsAdd()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(false, false, ItemContainers);
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(10, 10, 10, 10, 10, 0, "Test Item", "123456", false);

        Assert.AreEqual(ContainerMoveState.ActionState.Add, container.GetContainerAddState(itemData).ContainerActionState);
    }

    [Test]
    public void GetContainerRemoveState_ValidRemoveItem_ReturnsRemove()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(false, true, ItemContainers);
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(10, 11, 10, 10, 10, 0, "Test Item", "123456", false);

        Assert.AreEqual(ContainerMoveState.ActionState.Remove, container.GetContainerRemoveState(itemData).ContainerActionState);
    }

    [Test]
    public void GetContainerRemoveState_RestrictedItem_ReturnsNo()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(false, true, ItemContainers);
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(10, 10, 10, 10, 10, 0, "Test Item", "123456", false);

        Assert.AreEqual(ContainerMoveState.ActionState.No, container.GetContainerRemoveState(itemData).ContainerActionState);
    }

    [Test]
    public void GetContainerRemoveState_NoRestriction_ReturnsRemove()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(false, false, ItemContainers);
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(10, 10, 10, 10, 10, 0, "Test Item", "123456", false);

        Assert.AreEqual(ContainerMoveState.ActionState.Remove, container.GetContainerRemoveState(itemData).ContainerActionState);
    }

    [Test]
    public void Contains_ItemDataExists_Returns10()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(false, false, ItemContainers);
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(10, 10, 10, 10, 10, 0, "Test Item", "123456", false);

        container.Add(itemData);

        Assert.AreEqual(10, container.Contains(itemData));
    }

    [Test]
    public void Contains_ItemDataDoesntExist_Returns0()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(false, false, ItemContainers);
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(10, 10, 10, 10, 10, 0, "Test Item", "123456", false);

        Assert.AreEqual(0, container.Contains(itemData));
    }

    [TearDown]
    public void CleanUp()
    {
        ItemContainer[] objects = GameObject.FindObjectsOfType<ItemContainer>();

        foreach (ItemContainer container in objects)
        {
            DestroyImmediate(container.gameObject);
        }
    }


}
