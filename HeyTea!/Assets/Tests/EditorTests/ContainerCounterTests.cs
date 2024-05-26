using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class ContainerCounterTests
{
    private ContainerCounter containerCounter;
    private Player player;
    private HeyTeaObject heyTeaObject;
    private HeyTeaObjectSO heyTeaObjectSO;
    private GameObject InstantiatePrefab(string prefabPath)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        return GameObject.Instantiate(prefab);
    }
    private HeyTeaObjectSO CreateHeyTeaObjectSO(string path)
    {
        return AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>(path);
    }

    [SetUp]
    public void Setup()
    {

        containerCounter = new GameObject().AddComponent<ContainerCounter>();
        player = new GameObject().AddComponent<Player>();
        heyTeaObjectSO = ScriptableObject.CreateInstance<HeyTeaObjectSO>();
        heyTeaObject = new GameObject().AddComponent<HeyTeaObject>();
        heyTeaObject.SetHeyTeaObjectSO(heyTeaObjectSO);

    }

    //player has an object, cannot put or grab object from containercounter
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


    //player has no object, can grab object from containercounter
    [TestCase("Assets/Prefabs/Counters/AddOnCounter/GrapeCounter.prefab", "Assets/ScriptableObjectSO/HeyTeaObjectSO/Grape.asset")]
    [TestCase("Assets/Prefabs/Counters/AddOnCounter/OrangeCounter.prefab", "Assets/ScriptableObjectSO/HeyTeaObjectSO/Orange.asset")]
    [TestCase("Assets/Prefabs/Counters/AddOnCounter/IceCounter.prefab", "Assets/ScriptableObjectSO/HeyTeaObjectSO/Ice.asset")]
    [TestCase("Assets/Prefabs/Counters/AddOnCounter/SugarCounter.prefab", "Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedSugar.asset")]
    [TestCase("Assets/Prefabs/Counters/AddOnCounter/StrawberryCounter.prefab", "Assets/ScriptableObjectSO/HeyTeaObjectSO/Strawberry.asset")]
    [TestCase("Assets/Prefabs/Counters/AddOnCounter/PearlCounter.prefab", "Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset")]
    [TestCase("Assets/Prefabs/Counters/AddOnCounter/RedBeanCounter.prefab", "Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset")]
    public void PlayerHasNoObject_CanGrabObjectFromContainerCounter(string containerCounterPath,string expectedSOPath)
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
