using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using static IKichenwareObejct;

public class CupObjectTests
{
    private CupObject cup;
    private Player player;
    private HeyTeaObject ingredient;
    private HeyTeaObjectSO ingredientSO;
    private MilkTeaMaterialQuota quota;
    private HeyTeaObjectSO midStateHeyTeaObjectSO;
    private GameObject InstantiatePrefab(string prefabPath)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        return GameObject.Instantiate(prefab);
    }
    private HeyTeaObjectSO CreateHeyTeaObjectSO(string path)
    {
        return AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>(path);
    }
    public CupObject CreateCupObject()
    {
        var cupPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Prefabs/HeyTeaObjects/Cup.prefab");
        Transform cupInstance = Transform.Instantiate(cupPrefab);

        return cupInstance.GetComponent<CupObject>();
    }

    [SetUp]
    public void Setup()
    {
        player = new GameObject().AddComponent<Player>();
        cup = CreateCupObject();
        ingredient = new GameObject().AddComponent<HeyTeaObject>();
        ingredientSO = ScriptableObject.CreateInstance<HeyTeaObjectSO>();
    }

    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidMilk.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidTea.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/OrangeSlice.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/GrapeSlice.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/StrawberrySlice.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Ice.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset")]
    public void TryAddIngredient_AddsIngredientSuccessfully(string expectedSOPath)
    {
        // Arrange
        ingredientSO = CreateHeyTeaObjectSO(expectedSOPath);
        ingredient.SetHeyTeaObjectSO(ingredientSO);

        // Act
        bool result = cup.TryAddIngredient(ingredientSO, (IKichenwareObejct.MilkTeaMaterialType)ingredientSO.materialType);

        // Assert
        Assert.IsTrue(result);
        var heyTeaObjectSOList = cup.GetOutputHeyTeaObejctSOList();
        Assert.AreEqual(heyTeaObjectSOList.FirstOrDefault(a => a == ingredientSO), ingredientSO);
    }


    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedSugar.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Grape.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Orange.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Strawberry.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset")]
    public void TryAddIngredient_FailsWhenTypeMismatch(string expectedSOPath)
    {
        // Arrange
        ingredientSO = CreateHeyTeaObjectSO(expectedSOPath);
        ingredient.SetHeyTeaObjectSO(ingredientSO);
        // Act
        bool result = cup.TryAddIngredient(ingredientSO, (IKichenwareObejct.MilkTeaMaterialType)ingredientSO.materialType);

        // Assert
        Assert.IsFalse(result);
    }

    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidMilk.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidTea.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/OrangeSlice.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/GrapeSlice.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/StrawberrySlice.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Ice.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset")]
    public void TryAddIngredient_FailsWhenCannotAddMore(string expectedSOPath)
    {
        ingredientSO = CreateHeyTeaObjectSO(expectedSOPath);
        ingredient.SetHeyTeaObjectSO(ingredientSO);
        cup.milkTeaMaterialQuotaList = new List<IKichenwareObejct.MilkTeaMaterialQuota>
        {
            new IKichenwareObejct.MilkTeaMaterialQuota
            {
                milkTeaMaterialType = (IKichenwareObejct.MilkTeaMaterialType)ingredientSO.materialType,
                heyTeaObejctStructArray = new IKichenwareObejct.HeyTeaObejctStruct[]
                {
                    new IKichenwareObejct.HeyTeaObejctStruct
                    {
                        heyTeaObjectSO = ingredientSO,
                        maxNum = 5,
                        currentNum = 5
                    }
                }
            }
        };

        // Act
        bool result = cup.TryAddIngredient(ingredientSO, (IKichenwareObejct.MilkTeaMaterialType)ingredientSO.materialType);

        // Assert
        Assert.IsFalse(result);
    }
}
