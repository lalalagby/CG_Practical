using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/*
File Name : BaseCounterEditorTests.cs
Author    : Bingyu Guo
Created   : 14.05.2024
*/
public class BaseCounterEditorTests
{
    private GameObject baseCounterObject;
    private BaseCounter baseCounter;
    private HeyTeaObject heyTeaObject;

    [SetUp]
    public void Setup()
    {
        baseCounterObject = new GameObject();
        baseCounter = baseCounterObject.AddComponent<BaseCounter>();
        heyTeaObject = new GameObject().AddComponent<HeyTeaObject>();
    }

    [Test]  // TC0501
    public void GetHeyTeaObjectFollowTransform_ReturnsCounterTopPoint()
    {
        // Arrange
        Transform testTransform = new GameObject().transform;
        baseCounterObject.GetComponent<BaseCounter>().GetType().GetField("counterTopPoint", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(baseCounter, testTransform);

        // Act
        Transform result = baseCounter.GetHeyTeaObjectFollowTransform();

        // Assert
        Assert.AreEqual(testTransform, result);
    }

    [Test]  // TC0502
    public void ClearHeyTeaObject_ClearsHeyTeaObject()
    {
        // Arrange
        baseCounter.SetHeyTeaObject(heyTeaObject);

        // Act
        baseCounter.ClearHeyTeaObject();

        // Assert
        Assert.IsNull(baseCounter.GetHeyTeaObject());
    }

    [Test]  // TC0503
    public void HasHeyTeaObject_ReturnsTrue()
    {
        Assert.IsFalse(baseCounter.HasHeyTeaObject());

        baseCounter.SetHeyTeaObject(heyTeaObject);

        Assert.IsTrue(baseCounter.HasHeyTeaObject());
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(baseCounterObject);
    }
}
