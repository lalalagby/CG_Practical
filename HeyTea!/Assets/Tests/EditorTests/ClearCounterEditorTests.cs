using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ClearCounterEditorTests
{
    private ClearCounter counter;
    private Player mockPlayer;
    private HeyTeaObject mockHeyTeaObject;

    [SetUp]
    public void Setup()
    {
        counter = new GameObject().AddComponent<ClearCounter>();
        mockPlayer = new GameObject().AddComponent<Player>();
        mockHeyTeaObject = new GameObject().AddComponent<HeyTeaObject>();
    }

    [Test]
    public void Interact_WhenCounterEmptyAndPlayerHasObject_PutObjectOnCounter()
    {
        // Arrange
        counter.SetHeyTeaObject(null);
        mockPlayer.SetHeyTeaObject(mockHeyTeaObject);
        Assert.IsFalse(counter.HasHeyTeaObject());
        Assert.IsTrue(mockPlayer.HasHeyTeaObject());

        // Act
        counter.Interact(mockPlayer);

        // Assert
        Assert.IsTrue(counter.HasHeyTeaObject());
        Assert.IsFalse(mockPlayer.HasHeyTeaObject());
    }

    [Test]
    public void Interact_WhenCounterHasObjectAndPlayerEmpty_TakeObjectToPlayer()
    {
        // Arrange
        //mockPlayer.SetHeyTeaObject(null);
        counter.SetHeyTeaObject(mockHeyTeaObject);
        Assert.IsTrue(counter.HasHeyTeaObject());
        Assert.IsFalse(mockPlayer.HasHeyTeaObject());

        // Act
        counter.Interact(mockPlayer);

        // Assert
        Assert.IsFalse(counter.HasHeyTeaObject());
        Assert.IsTrue(mockPlayer.HasHeyTeaObject());
    }
}