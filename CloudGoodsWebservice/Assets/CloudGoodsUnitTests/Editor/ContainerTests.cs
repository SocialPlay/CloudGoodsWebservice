using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System;
using System.Collections.Generic;


[TestFixture]
[Category("Container Tests")]
public class ContainerTests : MonoBehaviour {

    ItemContainer itemContainerOne = new ItemContainer();
    ItemContainer ItemContainerTwo = new ItemContainer();

    [SetUp]
    public void Init()
    {

    }

    [Test]
    public void AddItemDataToContainerShowsDisplay()
    {

    }

    [Test]
    public void RemoveItemDataFromContainerRemovesDisplay()
    {

    }

    [Test]
    public void MoveItemDataFromContainerOneToContainerTwoUpdatesDisplay()
    {

    }

    [Test]
    public void MoveItemDataFromContainerMergeWithExistsingItem()
    {

    }

    [Test]
    public void SwapItemDataWithContainersUpdatesDisplay()
    {

    }

    [Test]
    public void MoveToItemContainerTwoThenThreeUpdatesDisplay()
    {

    }
}
