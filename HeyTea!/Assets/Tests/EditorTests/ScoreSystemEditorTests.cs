using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using static IKichenwareObejct;
using UnityEngine.SocialPlatforms.Impl;

/**
 * @author Bingyu Guo
 * @date 2024-07-05
 * 
 * @brief Unit tests for the ScoreSystem class and its interaction with OrderListManager.
 * 
 * @details This class contains unit tests to verify the behavior of the ScoreSystem 
 *          when orders are delivered correctly or incorrectly.
 */
public class ScoreSystemEditorTest
{
    private ScoreSystem scoreSystem;
    private OrderListManager orderListManager;
    private CupObject cupObject;
    private OrderListSO orderListSO;

    [SetUp]
    public void Setup() {
        orderListManager = new GameObject().AddComponent<OrderListManager>();

        orderListSO = ScriptableObject.CreateInstance<OrderListSO>();
        orderListSO.orderSOList = new List<OrderSO>();

        var cupPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Prefabs/HeyTeaObjects/Cup.prefab");
        Transform cupInstance = Transform.Instantiate(cupPrefab);
        cupObject = cupInstance.GetComponent<CupObject>();

        orderListManager.SetOrderListSO(orderListSO);

        // Initialize ScoreSystem
        scoreSystem = new GameObject().AddComponent<ScoreSystem>();
        scoreSystem.AddScore(0);
        orderListManager.OnOrderCompleted += scoreSystem.HandleOrderCompleted;
    }


    /**
     * @brief Creates a correct order for waiting order list.
     * 
     * @details This method generates a correct OrderSO instance based on the specified type.
     * 
     * @param type The type of order to create.
     * @return The created OrderSO instance.
     */
    private OrderSO CreateCorrectOrderSO(int type)
    {
        HeyTeaObjectSO heyTeaObjectSO;
        OrderSO orderSO = ScriptableObject.CreateInstance<OrderSO>();
        List<HeyTeaObjectSO> heyTeaObjectSOList = new List<HeyTeaObjectSO>();
        if (type == 1)
        {
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidMilktea.asset");
            heyTeaObjectSOList.Add(heyTeaObjectSO);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset");
            heyTeaObjectSOList.Add(heyTeaObjectSO);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/Sugar.asset");
            heyTeaObjectSOList.Add(heyTeaObjectSO);
        }
        else if (type == 2)
        {
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidTea.asset");
            heyTeaObjectSOList.Add(heyTeaObjectSO);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset");
            heyTeaObjectSOList.Add(heyTeaObjectSO);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/GrapeSlice.asset");
            heyTeaObjectSOList.Add(heyTeaObjectSO);
        }
        else
        {
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidTea.asset");
            heyTeaObjectSOList.Add(heyTeaObjectSO);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/Ice.asset");
            heyTeaObjectSOList.Add(heyTeaObjectSO);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/StrawberrySlice.asset");
            heyTeaObjectSOList.Add(heyTeaObjectSO);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/OrangeSlice.asset");
            heyTeaObjectSOList.Add(heyTeaObjectSO);
        }

        orderSO.heyTeaObjectSOLists = heyTeaObjectSOList;
        return orderSO;
    }

    /**
     * @brief Creates a correct CupObject for delivery.
     * 
     * @details This method generates a correct CupObject instance based on the specified type.
     * 
     * @param type The type of CupObject to create.
     * @return The created CupObject instance.
     */
    private CupObject CreateCorrectOrder(int type)
    {
        HeyTeaObjectSO heyTeaObjectSO;
        if (type == 1)
        {
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidMilktea.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/Sugar.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
        }
        else if (type == 2)
        {
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidTea.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/GrapeSlice.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
        }
        else
        {
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidTea.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/Ice.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/StrawberrySlice.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/OrangeSlice.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
        }

        return cupObject;
    }

    /**
     * @brief Creates a wrong CupObject for delivery.
     * 
     * @details This method generates a wrong CupObject instance based on the specified type.
     * 
     * @param type The type of CupObject to create.
     * @return The created CupObject instance.
     */
    private CupObject CreateWrongOrder(int type)
    {
        HeyTeaObjectSO heyTeaObjectSO;

        if (type == 1)
        {
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidMilktea.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
        }
        else if (type == 2)
        {
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidTea.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/GrapeSlice.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
        }
        else
        {
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidTea.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/Ice.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/GrapeSlice.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
        }

        return cupObject;
    }


    /**
     * @brief [TC17001] Test case for delivering a correct order.
     * 
     * @details This test verifies that delivering a correct order adds 10 points to the score.
     * 
     * @param type The type of order to create.
     */
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void DiliverCorrectOrder_AddScore(int type) {
        // Generate waiting order list
        OrderSO correctOrder1 = CreateCorrectOrderSO(1);
        OrderSO correctOrder2 = CreateCorrectOrderSO(2);
        OrderSO correctOrder3 = CreateCorrectOrderSO(3);
        orderListSO.orderSOList.Add(correctOrder1);
        orderListSO.orderSOList.Add(correctOrder2);
        orderListSO.orderSOList.Add(correctOrder3);
        orderListManager.SetWaitingOrderSOList(new List<OrderSO>());
        orderListManager.GetWaitingOrderSOList().Add(correctOrder1);
        orderListManager.SetOrdersGenerated();
        orderListManager.GetWaitingOrderSOList().Add(correctOrder2);
        orderListManager.SetOrdersGenerated();
        orderListManager.GetWaitingOrderSOList().Add(correctOrder3);
        orderListManager.SetOrdersGenerated();

        // Generate a wrong order to be delivered
        cupObject = CreateCorrectOrder(type);

        int initialScore = scoreSystem.GetCurrentScore();

        // Act
        orderListManager.DeliverOrder(cupObject);

        // Assert
        Assert.AreEqual(initialScore + 10, scoreSystem.GetCurrentScore());
    }

    /**
     * @brief [TC17002] Test case for delivering a wrong order.
     * 
     * @details This test verifies that delivering a wrong order does not change the score.
     * 
     * @param type The type of order to create.
     */
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void DiliverWrongOrder_NoChange(int type) {
        // Generate waiting order list
        OrderSO correctOrder1 = CreateCorrectOrderSO(1);
        OrderSO correctOrder2 = CreateCorrectOrderSO(2);
        OrderSO correctOrder3 = CreateCorrectOrderSO(3);
        orderListSO.orderSOList.Add(correctOrder1);
        orderListSO.orderSOList.Add(correctOrder2);
        orderListSO.orderSOList.Add(correctOrder3);
        orderListManager.SetWaitingOrderSOList(new List<OrderSO>());
        orderListManager.GetWaitingOrderSOList().Add(correctOrder1);
        orderListManager.SetOrdersGenerated();
        orderListManager.GetWaitingOrderSOList().Add(correctOrder2);
        orderListManager.SetOrdersGenerated();
        orderListManager.GetWaitingOrderSOList().Add(correctOrder3);
        orderListManager.SetOrdersGenerated();

        // Generate a wrong order to be delivered
        cupObject = CreateWrongOrder(type);

        int initialScore = scoreSystem.GetCurrentScore();

        // Act
        orderListManager.DeliverOrder(cupObject);

        // Assert
        Assert.AreEqual(initialScore, scoreSystem.GetCurrentScore());
    }
}
