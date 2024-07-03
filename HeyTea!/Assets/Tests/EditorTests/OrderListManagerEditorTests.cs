using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using static IKichenwareObejct;

/**
 * @author Bingyu Guo
 * 
 * @date 2024-06-30
 * 
 * @brief Unit tests for the OrderListManager class.
 * 
 * @details This class contains unit tests for the OrderListManager class, 
 *          testing the behavior of order delivery and order list management.
 */
public class OrderListManagerEditorTests{
    private OrderListManager orderListManager;
    private CupObject cupObject;
    private OrderListSO orderListSO;

    /**
     * @brief Sets up the test environment.
     * 
     * @details This method initializes the OrderListManager, OrderListSO, and CupObject instances for testing.
     */
    [SetUp]
    public void Setup() {
        orderListManager = new GameObject().AddComponent<OrderListManager>();

        orderListSO = ScriptableObject.CreateInstance<OrderListSO>();
        orderListSO.orderSOList = new List<OrderSO>();

        var cupPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Prefabs/HeyTeaObjects/Cup.prefab");
        Transform cupInstance = Transform.Instantiate(cupPrefab);
        cupObject = cupInstance.GetComponent<CupObject>();

        orderListManager.SetOrderListSO(orderListSO);
    }

    /**
     * @brief Creates a correct order for waiting order list.
     * 
     * @details This method generates a correct OrderSO instance based on the specified type.
     * 
     * @param type The type of order to create.
     * @return The created OrderSO instance.
     */
    private OrderSO CreateCorrectOrderSO(int type) {
        HeyTeaObjectSO heyTeaObjectSO;
        OrderSO orderSO = ScriptableObject.CreateInstance<OrderSO>();
        List<HeyTeaObjectSO> heyTeaObjectSOList = new List<HeyTeaObjectSO>();
        if (type == 1) {
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidMilktea.asset");
            heyTeaObjectSOList.Add(heyTeaObjectSO);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset");
            heyTeaObjectSOList.Add(heyTeaObjectSO);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/Sugar.asset");
            heyTeaObjectSOList.Add(heyTeaObjectSO);
        } else if(type == 2){
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidTea.asset");
            heyTeaObjectSOList.Add(heyTeaObjectSO);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset");
            heyTeaObjectSOList.Add(heyTeaObjectSO);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/GrapeSlice.asset");
            heyTeaObjectSOList.Add(heyTeaObjectSO);
        } else {
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
    private CupObject CreateCorrectOrder(int type) {
        HeyTeaObjectSO heyTeaObjectSO;
        if (type == 1) {
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidMilktea.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/Sugar.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
        } else if (type == 2) {
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidTea.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/GrapeSlice.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
        } else {
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
    private CupObject CreateWrongOrder(int type) {
        HeyTeaObjectSO heyTeaObjectSO;

        if (type == 1) {
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidMilktea.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
        } else if (type == 2) {
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidTea.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
            heyTeaObjectSO = AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>("Assets/ScriptableObjectSO/HeyTeaObjectSO/GrapeSlice.asset");
            cupObject.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType);
        } else {
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
     * @brief [TC1601] Test case for delivering a wrong order.
     * 
     * @details This test verifies that delivering a wrong order does not change the order list.
     * 
     * @param type The type of order to create.
     */
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void DeliverWrongOrder_OrderListUnchanged(int type) {
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

        // Get the number of orders in the current waiting order list.
        int waitingOrderNum = orderListManager.GetWaitingOrderSOList().Count;

        // Get the number of all orders that have been generated so far.
        int initialOrderCount = orderListManager.GetOrdersGenerated();

        // Act: Deliver the order
        orderListManager.DeliverOrder(cupObject);


        // The number of orders in the waiting order list does not change.
        Assert.AreEqual(waitingOrderNum, orderListManager.GetWaitingOrderSOList().Count);

        // Check if there is a new order generated after 2 seconds
        Task.Delay(2000).ContinueWith(_ => {
            // No new orders were generated.
            Assert.AreEqual(orderListManager.GetOrdersGenerated(), initialOrderCount);
        });
    }


    /**
     * @brief [TC1602] Test case for delivering a correct order.
     * 
     * @details This test verifies that delivering a correct order 
     *          removes the order from the order list and generates a new order after 2 seconds.
     * 
     * @param type The type of order to create.
     */
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void DeliverCorrectOrder_RemoveOrderAndGenerateNewOrder(int type) {
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

        // Generate a correct order to be delivered
        cupObject = CreateCorrectOrder(type);

        // Get the number of orders in the current waiting order list.
        int waitingOrderNum = orderListManager.GetWaitingOrderSOList().Count;

        // Get the number of all orders that have been generated so far.
        int initialOrderCount = orderListManager.GetOrdersGenerated();

        // Act: Deliver the order
        orderListManager.DeliverOrder(cupObject);

        // The number of orders in the waiting order list is reduced by 1.
        Assert.AreEqual(waitingOrderNum - 1, orderListManager.GetWaitingOrderSOList().Count);

        // Wait for 2 seconds to generate a new order
        Task.Delay(2000).ContinueWith(_ => {
            // Add 1 to the number of orders generated
            Assert.AreEqual(orderListManager.GetOrdersGenerated(), initialOrderCount + 1);

            // The waiting order list still has three orders
            Assert.AreEqual(waitingOrderNum, orderListManager.GetWaitingOrderSOList().Count);
        });
    }

}
