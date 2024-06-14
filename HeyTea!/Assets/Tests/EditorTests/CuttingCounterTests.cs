using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class CuttingCounterTests
{
    private CuttingCounter cuttingCounter;
    private Player player;
    private HeyTeaObject heyTeaObject;
    private HeyTeaObjectSO heyTeaObjectSO;
    private HeyTeaObjectSO outputHeyTeaObjectSO;
    private CuttingRecipeSO cuttingRecipeSO;
    private CupObject cup;

    private GameObject InstantiatePrefab(string prefabPath)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        return GameObject.Instantiate(prefab);
    }
    private HeyTeaObjectSO CreateHeyTeaObjectSO(string path)
    {
        return AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>(path);
    }
    private CupObject CreateCupObject()
    {
        var cupPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Prefabs/HeyTeaObjects/Cup.prefab");
        Transform cupInstance = Transform.Instantiate(cupPrefab);

        return cupInstance.GetComponent<CupObject>();
    }

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
