using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using static IKichenwareObejct;

/**
 * @brief Unit tests for the CupObject class.
 * 
 * @details This class contains unit tests for various functionalities of the CupObject class,
 * ensuring that ingredients are correctly added to the Cup and validating the states of these objects.
 * 
 * @date 05.07.2024
 * @author Xinyue Cheng
 */
public class CupObjectTests
{
    private CupObject cup;
    private Player player;
    private HeyTeaObject ingredient;
    private HeyTeaObjectSO ingredientSO;
    private MilkTeaMaterialQuota quota;
    private HeyTeaObjectSO midStateHeyTeaObjectSO;

    /**
     * @brief Instantiates a prefab from a given path.
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
     * @param path The path to the HeyTeaObjectSO asset.
     * @return The created HeyTeaObjectSO.
     */
    private HeyTeaObjectSO CreateHeyTeaObjectSO(string path)
    {
        return AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>(path);
    }

    /**
     * @brief Creates a CupObject from a prefab.
     * @return The created CupObject.
     */
    public CupObject CreateCupObject()
    {
        var cupPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Prefabs/HeyTeaObjects/Cup.prefab");
        Transform cupInstance = Transform.Instantiate(cupPrefab);

        return cupInstance.GetComponent<CupObject>();
    }

    /**
     * @brief Sets up the test environment before each test.
     */
    [SetUp]
    public void Setup()
    {
        player = new GameObject().AddComponent<Player>();
        cup = CreateCupObject();
        ingredient = new GameObject().AddComponent<HeyTeaObject>();
        ingredientSO = ScriptableObject.CreateInstance<HeyTeaObjectSO>();
    }

    /**
     * @brief [TC1301] Tests adding various ingredients to the cup successfully.
     * @param expectedSOPath The path to the expected HeyTeaObjectSO asset.
     */
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

    /**
     * @brief [TC1302] Tests that adding an ingredient fails when there is a type mismatch.
     * @param expectedSOPath The path to the HeyTeaObjectSO asset with a mismatched type.
     */
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

    /**
     * @brief [TC1303] Tests that adding an ingredient fails when the cup is full.
     * @param expectedSOPath The path to the HeyTeaObjectSO asset to be added.
     */
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
        // Arrange
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
