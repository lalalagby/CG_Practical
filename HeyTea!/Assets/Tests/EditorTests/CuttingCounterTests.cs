using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
/**
 * @class CuttingCounterTests
 * @brief Unit tests for the CuttingCounter class, which handles the interaction between the player and the cutting counter.
 * @author Xinyue Cheng
 * @details
 * The CuttingCounterTests class includes various test cases to validate the interactions and functionalities of the CuttingCounter class,
 * such as handling HeyTeaObject instances and their states during interactions.
 */
public class CuttingCounterTests
{
    private CuttingCounter cuttingCounter;
    private Player player;
    private HeyTeaObject heyTeaObject;
    private HeyTeaObjectSO heyTeaObjectSO;
    private HeyTeaObjectSO outputHeyTeaObjectSO;
    private CuttingRecipeSO cuttingRecipeSO;
    private CupObject cup;

    /**
     * @brief Instantiates a prefab from a given path.
     * @param prefabPath The path to the prefab asset.
     * @return The instantiated GameObject.
     */
    private GameObject InstantiatePrefab(string prefabPath)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        return GameObject.Instantiate(prefab);
    }

    /**
     * @brief Loads a HeyTeaObjectSO from a given path.
     * @param path The path to the HeyTeaObjectSO asset.
     * @return The loaded HeyTeaObjectSO.
     */
    private HeyTeaObjectSO CreateHeyTeaObjectSO(string path)
    {
        return AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>(path);
    }

    /**
     * @brief Creates a CupObject instance.
     * @return The created CupObject.
     */
    private CupObject CreateCupObject()
    {
        var cupPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Prefabs/HeyTeaObjects/Cup.prefab");
        Transform cupInstance = Transform.Instantiate(cupPrefab);

        return cupInstance.GetComponent<CupObject>();
    }

    /**
     * @brief Sets up the test environment by initializing player, cutting counter, and necessary objects.
     */
    [SetUp]
    public void Setup()
    {
        player = new GameObject().AddComponent<Player>();
        cuttingCounter = new GameObject().AddComponent<CuttingCounter>();
        heyTeaObject = new GameObject().AddComponent<HeyTeaObject>();
        heyTeaObjectSO = ScriptableObject.CreateInstance<HeyTeaObjectSO>();
        outputHeyTeaObjectSO = ScriptableObject.CreateInstance<HeyTeaObjectSO>();
        cup = CreateCupObject();
    }

    /**
     * @brief [TC1401] Test case to validate interaction when both player and cutting counter have nothing.
     * @param cuttingCounterPath The path to the cutting counter prefab.
     */
    [TestCase("Assets/Prefabs/Counters/FunctionCounter/CuttingCounter.prefab")]
    public void PlayerHasNothingCuttingCounterHasNothing(string cuttingCounterPath)
    {
        cuttingCounter = InstantiatePrefab(cuttingCounterPath).GetComponent<CuttingCounter>();
        player.ClearHeyTeaObject();
        cuttingCounter.ClearHeyTeaObject();
        cuttingCounter.Interact(player);

        Assert.IsFalse(cuttingCounter.HasHeyTeaObject());
        Assert.IsFalse(player.HasHeyTeaObject());
    }

    /**
     * @brief [TC1402] Test case to validate interaction when player takes an object from the cutting counter.
     * @param expectedSOPath The path to the expected HeyTeaObjectSO asset.
     * @param cuttingCounterPath The path to the cutting counter prefab.
     */
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Grape.asset", "Assets/Prefabs/Counters/FunctionCounter/CuttingCounter.prefab")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Orange.asset", "Assets/Prefabs/Counters/FunctionCounter/CuttingCounter.prefab")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Strawberry.asset", "Assets/Prefabs/Counters/FunctionCounter/CuttingCounter.prefab")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/GrapeSlice.asset", "Assets/Prefabs/Counters/FunctionCounter/CuttingCounter.prefab")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/OrangeSlice.asset", "Assets/Prefabs/Counters/FunctionCounter/CuttingCounter.prefab")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/StrawberrySlice.asset", "Assets/Prefabs/Counters/FunctionCounter/CuttingCounter.prefab")]
    public void PlayerTakesObjectFromCuttingCounter(string expectedSOPath, string cuttingCounterPath)
    {
        // Arrange
        heyTeaObjectSO = CreateHeyTeaObjectSO(expectedSOPath);
        heyTeaObject.SetHeyTeaObjectSO(heyTeaObjectSO);
        cuttingCounter = InstantiatePrefab(cuttingCounterPath).GetComponent<CuttingCounter>();
        player.ClearHeyTeaObject();
        cuttingCounter.SetHeyTeaObject(heyTeaObject);
        // Act
        cuttingCounter.Interact(player);

        // Assert
        Assert.IsTrue(cuttingCounter.HasHeyTeaObject());
        Assert.IsTrue(player.HasHeyTeaObject());
        Assert.AreEqual(heyTeaObject, player.GetHeyTeaObject());
    }

    /**
     * @brief [TC1403] Test case to validate interaction when player cannot take an object from the cutting counter.
     * @param expectedSOPath The path to the expected HeyTeaObjectSO asset.
     * @param cuttingCounterPath The path to the cutting counter prefab.
     */
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Grape.asset", "Assets/Prefabs/Counters/FunctionCounter/CuttingCounter.prefab")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Orange.asset", "Assets/Prefabs/Counters/FunctionCounter/CuttingCounter.prefab")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Strawberry.asset", "Assets/Prefabs/Counters/FunctionCounter/CuttingCounter.prefab")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/GrapeSlice.asset", "Assets/Prefabs/Counters/FunctionCounter/CuttingCounter.prefab")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/OrangeSlice.asset", "Assets/Prefabs/Counters/FunctionCounter/CuttingCounter.prefab")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/StrawberrySlice.asset", "Assets/Prefabs/Counters/FunctionCounter/CuttingCounter.prefab")]
    public void PlayerCannotTakesObjectFromCuttingCounter(string expectedSOPath, string cuttingCounterPath)
    {
        // Arrange
        heyTeaObjectSO = CreateHeyTeaObjectSO(expectedSOPath);
        heyTeaObject.SetHeyTeaObjectSO(heyTeaObjectSO);
        cuttingCounter = InstantiatePrefab(cuttingCounterPath).GetComponent<CuttingCounter>();
        player.SetHeyTeaObject(heyTeaObject);
        cuttingCounter.SetHeyTeaObject(heyTeaObject);
        // Act
        cuttingCounter.Interact(player);

        // Assert
        Assert.IsTrue(cuttingCounter.HasHeyTeaObject());
        Assert.IsTrue(player.HasHeyTeaObject());
    }

    /**
     * @brief [TC1404] Test case to validate interaction when player puts an object on the cutting counter.
     * @param expectedSOPath The path to the expected HeyTeaObjectSO asset.
     * @param cuttingCounterPath The path to the cutting counter prefab.
     */
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Grape.asset", "Assets/Prefabs/Counters/FunctionCounter/CuttingCounter.prefab")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Orange.asset", "Assets/Prefabs/Counters/FunctionCounter/CuttingCounter.prefab")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Strawberry.asset", "Assets/Prefabs/Counters/FunctionCounter/CuttingCounter.prefab")]
    public void PlayerHasObjectPutOnCuttingCounter(string expectedSOPath, string cuttingCounterPath)
    {
        heyTeaObjectSO = CreateHeyTeaObjectSO(expectedSOPath);
        heyTeaObject.SetHeyTeaObjectSO(heyTeaObjectSO);
        cuttingCounter = InstantiatePrefab(cuttingCounterPath).GetComponent<CuttingCounter>();
        cuttingCounter.ClearHeyTeaObject();
        player.SetHeyTeaObject(heyTeaObject);
        cuttingCounter.Interact(player);
        Assert.IsTrue(cuttingCounter.HasHeyTeaObject());
        Assert.AreEqual(heyTeaObject, cuttingCounter.GetHeyTeaObject());
    }

    /**
     * @brief [TC1405] Test case to validate interaction when player has an object that cannot be put on the cutting counter.
     * @param expectedSOPath The path to the expected HeyTeaObjectSO asset.
     * @param cuttingCounterPath The path to the cutting counter prefab.
     */
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedSugar.asset", "Assets/Prefabs/Counters/FunctionCounter/CuttingCounter.prefab")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset", "Assets/Prefabs/Counters/FunctionCounter/CuttingCounter.prefab")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset", "Assets/Prefabs/Counters/FunctionCounter/CuttingCounter.prefab")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Ice.asset", "Assets/Prefabs/Counters/FunctionCounter/CuttingCounter.prefab")]
    public void PlayerHasObjectCannotPutOnCuttingCounter(string expectedSOPath, string cuttingCounterPath)
    {
        heyTeaObjectSO = CreateHeyTeaObjectSO(expectedSOPath);
        heyTeaObject.SetHeyTeaObjectSO(heyTeaObjectSO);
        cuttingCounter = InstantiatePrefab(cuttingCounterPath).GetComponent<CuttingCounter>();
        cuttingCounter.ClearHeyTeaObject();
        player.SetHeyTeaObject(heyTeaObject);
        cuttingCounter.Interact(player);
        Assert.IsFalse(cuttingCounter.HasHeyTeaObject());
        Assert.IsTrue(player.HasHeyTeaObject());
    }

    /**
     * @brief [TC1406] Test case to validate interaction when player cannot take an object with a cup from the cutting counter.
     * @param expectedSOPath The path to the expected HeyTeaObjectSO asset.
     * @param cuttingCounterPath The path to the cutting counter prefab.
     */
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Grape.asset", "Assets/Prefabs/Counters/FunctionCounter/CuttingCounter.prefab")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Orange.asset", "Assets/Prefabs/Counters/FunctionCounter/CuttingCounter.prefab")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Strawberry.asset", "Assets/Prefabs/Counters/FunctionCounter/CuttingCounter.prefab")]
    public void PlayerCannotTakesObjectWithCupFromCuttingCounter(string expectedSOPath, string cuttingCounterPath)
    {
        // Arrange
        heyTeaObjectSO = CreateHeyTeaObjectSO(expectedSOPath);
        heyTeaObject.SetHeyTeaObjectSO(heyTeaObjectSO);
        cuttingCounter = InstantiatePrefab(cuttingCounterPath).GetComponent<CuttingCounter>();
        player.SetHeyTeaObject(cup);
        cuttingCounter.SetHeyTeaObject(heyTeaObject);
        // Act
        cuttingCounter.Interact(player);

        // Assert
        Assert.IsTrue(cuttingCounter.HasHeyTeaObject());
        Assert.IsTrue(player.HasHeyTeaObject());
        Assert.AreEqual(cup, player.GetHeyTeaObject());
        Assert.AreEqual(heyTeaObject, cuttingCounter.GetHeyTeaObject());
    }

    //[TC1407]
    //[TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/GrapeSlice.asset", "Assets/Prefabs/Counters/FunctionCounter/CuttingCounter.prefab")]
    //[TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/OrangeSlice.asset", "Assets/Prefabs/Counters/FunctionCounter/CuttingCounter.prefab")]
    //[TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/StrawberrySlice.asset", "Assets/Prefabs/Counters/FunctionCounter/CuttingCounter.prefab")]
    //public void PlayerTakesObjectWithCupFromCuttingCounter(string expectedSOPath, string cuttingCounterPath)
    //{
    //    // Arrange
    //    heyTeaObjectSO = CreateHeyTeaObjectSO(expectedSOPath);
    //    heyTeaObject.SetHeyTeaObjectSO(heyTeaObjectSO);
    //    cuttingCounter = InstantiatePrefab(cuttingCounterPath).GetComponent<CuttingCounter>();
    //    player.SetHeyTeaObject(cup);
    //    cuttingCounter.SetHeyTeaObject(heyTeaObject);
    //    // Act
    //    cuttingCounter.Interact(player);

    //    //var heyTeaObjectSOList = cup.GetOutputHeyTeaObejctSOList();
    //    //Assert.AreEqual(heyTeaObjectSOList.FirstOrDefault(a => a == heyTeaObjectSO), heyTeaObjectSO);

    //    // Assert
    //    //Assert.IsFalse(cuttingCounter.HasHeyTeaObject());
    //    //Assert.IsTrue(player.HasHeyTeaObject());
    //    //Assert.IsTrue(cup.GetMilkTeaMaterialQuota().Any(q => q.heyTeaObejctStructArray.Any(s => s.heyTeaObjectSO == heyTeaObjectSO)));
    //}
}


