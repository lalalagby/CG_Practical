using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/**
 * @author Bingyu Guo
 * 
 * @brief  Unit tests for the BaseCounter class.
 * 
 * @details This class contains unit tests for various functionalities of the BaseCounter class,
 *          ensuring that the basic operations such as getting and setting HeyTeaObject, 
 *          and clearing HeyTeaObject are working correctly.
 * 
 * @note
 *      milkTeaMaterialType contains six types: teaBase, basicAdd, ingredients, fruit, none, un treat.
 *      HeyTeaObject includes the objects contained in teaBase, basicAdd, ingredients, fruit, none, un treat.
 *          - teaBase: tea, milk, milktea
 *          - basicAdd: sugar, ice
 *          - ingredients: red bean cooked, pearl cooked
 *          - fruit: orange slice, grape slice, strawberry slice
 *          - none: pot, cup
 *          - un treat: bagged pearl, bagged red bean, bagged sugar, grape, orange, strawberry
 */
public class BaseCounterEditorTests
{
    private GameObject baseCounterObject;
    private BaseCounter baseCounter;
    private HeyTeaObject heyTeaObject;

    /**
     * @brief Sets up the test environment before each test.
     * 
     * @details This method initializes the BaseCounter and HeyTeaObject instances for testing.
     */
    [SetUp]
    public void Setup()
    {
        baseCounterObject = new GameObject();
        baseCounter = baseCounterObject.AddComponent<BaseCounter>();
        heyTeaObject = new GameObject().AddComponent<HeyTeaObject>();
    }

   /**
    * @brief [TC0501] Tests if GetHeyTeaObjectFollowTransform returns the correct transform.
    * 
    * @details This test case checks if the GetHeyTeaObjectFollowTransform method returns the transform
    *          set as the counterTopPoint.
    */
    [Test]
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

   /**
    * @brief [TC0502] Tests if ClearHeyTeaObject correctly clears the HeyTeaObject.
    * 
    * @details This test case checks if the ClearHeyTeaObject method sets the heyTeaObject to null.
    */
    [Test] 
    public void ClearHeyTeaObject_ClearsHeyTeaObject()
    {
        // Arrange
        baseCounter.SetHeyTeaObject(heyTeaObject);

        // Act
        baseCounter.ClearHeyTeaObject();

        // Assert
        Assert.IsNull(baseCounter.GetHeyTeaObject());
    }

    /**
     * @brief [TC0503] Tests if HasHeyTeaObject returns the correct boolean value.
     * 
     * @details This test case checks if the HasHeyTeaObject method returns true when there is a HeyTeaObject,
     *          and false when there isn't.
     */
    [Test]
    public void HasHeyTeaObject_ReturnsTrue()
    {
        Assert.IsFalse(baseCounter.HasHeyTeaObject());

        baseCounter.SetHeyTeaObject(heyTeaObject);

        Assert.IsTrue(baseCounter.HasHeyTeaObject());
    }
    /**
     * @brief Tears down the test environment after each test.
     * 
     * @details This method destroys the BaseCounter and HeyTeaObject instances to clean up after each test.
     */
    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(baseCounterObject);
    }
}
