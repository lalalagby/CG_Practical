using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

/**
 * @class DeliveryCounterTests
 * @brief Unit tests for the DeliveryCounter class.
 * 
 * This class contains unit tests for the DeliveryCounter class, ensuring that it correctly handles interactions
 * with the Player, specifically when the Player is holding a CupObject or other HeyTeaObjects.
 * 
 * @date 30.06.2024
 */

public class DeliveryCounterTests
{
    private DeliveryCounter deliveryCounter;
    private Player player;
    private HeyTeaObject heyTeaObject;
    private CupObject cup;

    /**
     * @brief Creates a CupObject instance from a prefab.
     * 
     * @details This method loads a CupObject prefab from the specified path and instantiates it.
     * 
     * @return A CupObject instance.
     */
    public CupObject CreateCupObject()
    {
        var cupPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Prefabs/HeyTeaObjects/Cup.prefab");
        Transform cupInstance = Transform.Instantiate(cupPrefab);

        return cupInstance.GetComponent<CupObject>();
    }

    /**
     * @brief Creates a HeyTeaObjectSO instance from a specified path.
     * 
     * @details This method loads a HeyTeaObjectSO asset from the specified path.
     * 
     * @param path The path to the HeyTeaObjectSO asset.
     * @return The loaded HeyTeaObjectSO instance.
     */
    public HeyTeaObjectSO CreateHeyTeaObjectSO(string path)
    {
        return AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>(path);
    }

    /**
     * @brief Instantiates a prefab from the given path.
     * 
     * @details This method loads a prefab from the specified path and instantiates it.
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
     * @brief Sets up the test environment before each test.
     * 
     * @details This method initializes the DeliveryCounter, Player, HeyTeaObject, and CupObject instances for testing.
     */
    [SetUp]
    public void Setup()
    {
        // Initialize DeliveryCounter
        deliveryCounter = InstantiatePrefab("Assets/Prefabs/Counters/DeliveryCounter.prefab").GetComponent<DeliveryCounter>();

        // Initialize Player
        player = new GameObject().AddComponent<Player>();

        // Initialize HeyTeaObject and CupObject
        heyTeaObject = new GameObject().AddComponent<HeyTeaObject>();
        cup = CreateCupObject();
    }

    /**
     * @brief [TC1501] Tests that the CupObject is destroyed when delivered.
     * 
     * @details This test checks that when the Player interacts with the DeliveryCounter while holding a CupObject,
     * the CupObject is correctly destroyed.
     */
    [Test]
    public void PlayerHasCup_DestroysCup()
    {
        // Set Player has HeyTeaObject.
        player.ClearHeyTeaObject();
        player.SetHeyTeaObject(cup);
        cup.SetHeyTeaObjectParents(player);

        // Act
        deliveryCounter.Interact(player);

        // Assert
        Assert.IsFalse(player.HasHeyTeaObject());
    }

    /**
     * @brief [TC1502] Tests that other HeyTeaObjects cannot be delivered.
     * 
     * @details This test checks that when the Player interacts with the DeliveryCounter while holding other HeyTeaObjects,
     * nothing happens and the Player still holds the object.
     * 
     * @param expectedSOPath The path to the HeyTeaObjectSO asset.
     */
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Grape.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Strawberry.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Orange.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedSugar.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Ice.asset")]
    public void PlayerHasOther_DoNothing(string expectedSOPath)
    {
        // Set Player has HeyTeaObject.
        HeyTeaObjectSO heyTeaObjectSO = CreateHeyTeaObjectSO(expectedSOPath);
        heyTeaObject.SetHeyTeaObjectSO(heyTeaObjectSO);
        player.ClearHeyTeaObject();
        player.SetHeyTeaObject(heyTeaObject);

        // Act
        deliveryCounter.Interact(player);

        // Assert
        Assert.IsTrue(player.HasHeyTeaObject());
        Assert.AreEqual(heyTeaObject, player.GetHeyTeaObject());
    }
}
