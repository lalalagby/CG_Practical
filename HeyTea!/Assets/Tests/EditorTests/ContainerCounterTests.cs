using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

/**
 * @brief Unit tests for ContainerCounter interactions.
 * 
 * This file contains unit tests for the ContainerCounter class,
 * testing interactions between the Player and the ContainerCounter.
 * 
 * @author Xinyue Cheng
 * 
 * @note 
 * - ContainerCounter is responsible for handling the interactions between the Player and the container.
 * - Player can either put or grab objects from the ContainerCounter.
 */

public class ContainerCounterTests
{
    private ContainerCounter containerCounter;
    private Player player;
    private HeyTeaObject heyTeaObject;
    private HeyTeaObjectSO heyTeaObjectSO;

    /**
     * @brief Instantiates a prefab from a given path.
     * 
     * @param prefabPath The path to the prefab.
     * @return The instantiated GameObject.
     */
    private GameObject InstantiatePrefab(string prefabPath)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        return GameObject.Instantiate(prefab);
    }

    /**
     * @brief Creates a HeyTeaObjectSO from a given path.
     * 
     * @param path The path to the HeyTeaObjectSO asset.
     * @return The created HeyTeaObjectSO.
     */
    private HeyTeaObjectSO CreateHeyTeaObjectSO(string path)
    {
        return AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>(path);
    }

    /**
     * @brief Sets up the test environment.
     * 
     * This method is called before each test to set up the necessary objects and state.
     */
    [SetUp]
    public void Setup()
    {
        containerCounter = new GameObject().AddComponent<ContainerCounter>();
        player = new GameObject().AddComponent<Player>();
        heyTeaObjectSO = ScriptableObject.CreateInstance<HeyTeaObjectSO>();
        heyTeaObject = new GameObject().AddComponent<HeyTeaObject>();
        heyTeaObject.SetHeyTeaObjectSO(heyTeaObjectSO);
    }

    /**
     * @brief [TC0801] Tests that the player cannot put or grab objects from the container counter when holding an object.
     * 
     * This test ensures that if the player already has an object, they cannot put it on or grab another object from the container counter.
     */
    [Test]
    public void PlayerHasObject_CannotPutOrGrabObjectFromContainerCounter()
    {
        // Arrange
        player.SetHeyTeaObject(heyTeaObject);

        // Act
        containerCounter.Interact(player);

        // Assert
        LogAssert.Expect(LogType.Log, "Can't put an object on a ContainerCounter and can't grab the object from the ContainerCounter.");
        Assert.IsTrue(player.HasHeyTeaObject());
        Assert.AreEqual(heyTeaObject, player.GetHeyTeaObject());
        Assert.IsFalse(containerCounter.HasHeyTeaObject());
    }

    /**
     * @brief [TC0802] Tests that the player can grab the correct object from the container counter when not holding any object.
     * 
     * This test ensures that if the player has no object, they can grab the correct object from the container counter.
     * 
     * @param containerCounterPath The path to the container counter prefab.
     * @param expectedSOPath The expected HeyTeaObjectSO asset path.
     */
    [TestCase("Assets/Prefabs/Counters/AddOnCounter/GrapeCounter.prefab", "Assets/ScriptableObjectSO/HeyTeaObjectSO/Grape.asset")]
    [TestCase("Assets/Prefabs/Counters/AddOnCounter/OrangeCounter.prefab", "Assets/ScriptableObjectSO/HeyTeaObjectSO/Orange.asset")]
    [TestCase("Assets/Prefabs/Counters/AddOnCounter/IceCounter.prefab", "Assets/ScriptableObjectSO/HeyTeaObjectSO/Ice.asset")]
    [TestCase("Assets/Prefabs/Counters/AddOnCounter/SugarCounter.prefab", "Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedSugar.asset")]
    [TestCase("Assets/Prefabs/Counters/AddOnCounter/StrawberryCounter.prefab", "Assets/ScriptableObjectSO/HeyTeaObjectSO/Strawberry.asset")]
    [TestCase("Assets/Prefabs/Counters/AddOnCounter/PearlCounter.prefab", "Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset")]
    [TestCase("Assets/Prefabs/Counters/AddOnCounter/RedBeanCounter.prefab", "Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset")]
    public void PlayerHasNoObject_CanGrabObjectFromContainerCounter(string containerCounterPath, string expectedSOPath)
    {
        // Arrange
        player.ClearHeyTeaObject();
        heyTeaObjectSO = CreateHeyTeaObjectSO(expectedSOPath);
        heyTeaObject.SetHeyTeaObjectSO(heyTeaObjectSO);
        containerCounter = InstantiatePrefab(containerCounterPath).GetComponent<ContainerCounter>();

        // Act
        containerCounter.Interact(player);

        // Assert
        Assert.IsTrue(player.HasHeyTeaObject());
        Assert.AreEqual(heyTeaObjectSO, player.GetHeyTeaObject().GetHeyTeaObjectSO());
    }
}
