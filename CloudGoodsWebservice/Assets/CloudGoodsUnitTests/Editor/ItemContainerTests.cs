using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System;
using System.Collections.Generic;

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
        ItemData itemData = containerTestUtilities.CreateItemData(10, 11, 10, 10, 10, 0, "Test Item", "123456", false);

        Assert.AreEqual(ContainerMoveState.ActionState.Add, container.GetContainerAddState(itemData).actionState);
    }

    [Test]
    public void GetContainerAddState_RestrictedItem_ReturnsNo()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(true, false, ItemContainers);
        ItemData itemData = containerTestUtilities.CreateItemData(10, 10, 10, 10, 10, 0, "Test Item", "123456", false);

        Assert.AreEqual(ContainerMoveState.ActionState.No, container.GetContainerAddState(itemData).actionState);
    }

    [Test]
    public void GetContainerAddState_NoRestriction_ReturnsAdd()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(false, false, ItemContainers);
        ItemData itemData = containerTestUtilities.CreateItemData(10, 10, 10, 10, 10, 0, "Test Item", "123456", false);

        Assert.AreEqual(ContainerMoveState.ActionState.Add, container.GetContainerAddState(itemData).actionState);
    }

    [Test]
    public void GetContainerRemoveState_ValidRemoveItem_ReturnsRemove()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(false, true, ItemContainers);
        ItemData itemData = containerTestUtilities.CreateItemData(10, 11, 10, 10, 10, 0, "Test Item", "123456", false);

        Assert.AreEqual(ContainerMoveState.ActionState.Remove, container.GetContainerRemoveState(itemData).actionState);
    }

    [Test]
    public void GetContainerRemoveState_RestrictedItem_ReturnsNo()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(false, true, ItemContainers);
        ItemData itemData = containerTestUtilities.CreateItemData(10, 10, 10, 10, 10, 0, "Test Item", "123456", false);

        Assert.AreEqual(ContainerMoveState.ActionState.No, container.GetContainerRemoveState(itemData).actionState);
    }

    [Test]
    public void GetContainerRemoveState_NoRestriction_ReturnsRemove()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(false, false, ItemContainers);
        ItemData itemData = containerTestUtilities.CreateItemData(10, 10, 10, 10, 10, 0, "Test Item", "123456", false);

        Assert.AreEqual(ContainerMoveState.ActionState.Remove, container.GetContainerRemoveState(itemData).actionState);
    }

    [Test]
    public void Contains_ItemDataExists_Returns10()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(false, false, ItemContainers);
        ItemData itemData = containerTestUtilities.CreateItemData(10, 10, 10, 10, 10, 0, "Test Item", "123456", false);

        container.Add(itemData);

        Assert.AreEqual(10, container.Contains(itemData));
    }

    [Test]
    public void Contains_ItemDataDoesntExist_Returns0()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(false, false, ItemContainers);
        ItemData itemData = containerTestUtilities.CreateItemData(10, 10, 10, 10, 10, 0, "Test Item", "123456", false);

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
