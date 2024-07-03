using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using static IKichenwareObejct;


public class OrderListManagerEditorTests{
    public static OrderListManager Instance { get; private set; }
    private OrderListManager orderListManager;
    private CupObject cupObject;
    private OrderListSO orderListSO;

    [SetUp]
    public void Setup() {
    // 创建OrderListManager对象
        orderListManager = new GameObject().AddComponent<OrderListManager>();

        // 创建OrderListSO
        orderListSO = ScriptableObject.CreateInstance<OrderListSO>();
        orderListSO.orderSOList = new List<OrderSO>();

        // 创建CupObject对象
        var cupPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Prefabs/HeyTeaObjects/Cup.prefab");
        Transform cupInstance = Transform.Instantiate(cupPrefab);
        cupObject = cupInstance.GetComponent<CupObject>();

        // 设置OrderListManager的OrderListSO
        orderListManager.SetOrderListSO(orderListSO);
    }

    // 生成一个真正的订单
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
    
    
    // 提交错误的订单：订单列表没有订单被移除。
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

        // Generate an order to be delivered
        cupObject = CreateWrongOrder(type);

        // Get the number of orders in the current waiting order list.
        int waitingOrderNum = orderListManager.GetWaitingOrderSOList().Count;

        // Get the number of all orders that have been generated so far.
        int initialOrderCount = orderListManager.GetOrdersGenerated();

        // Act: Deliver the order
        orderListManager.DeliverOrder(cupObject);


        // The number of orders in the waiting order list does not change.
        Assert.AreEqual(waitingOrderNum, orderListManager.GetWaitingOrderSOList().Count);

        // Check if there is a new order generated after 3 seconds
        Task.Delay(3000).ContinueWith(_ => {
            // No new orders were generated.
            Assert.AreEqual(orderListManager.GetOrdersGenerated(), initialOrderCount);
        });
    }


    // 提交正确的订单：该订单从订单列表中删除。
    [Test]
    public void DeliverCorrectOrder_RemoveOrderAndGenerateNewOrder() {
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

        // Generate an order to be delivered
        cupObject = CreateCorrectOrder(2);

        // Get the number of orders in the current waiting order list.
        int waitingOrderNum = orderListManager.GetWaitingOrderSOList().Count;

        // Get the number of all orders that have been generated so far.
        int initialOrderCount = orderListManager.GetOrdersGenerated();

        // Act: Deliver the order
        orderListManager.DeliverOrder(cupObject);

        // The number of orders in the waiting order list is reduced by 1.
        Assert.AreEqual(waitingOrderNum - 1, orderListManager.GetWaitingOrderSOList().Count);

        // Wait for 3 seconds to generate a new order (Question? )
        Task.Delay(3000).ContinueWith(_ => {
            // Add 1 to the number of orders generated
            Assert.AreEqual(orderListManager.GetOrdersGenerated(), initialOrderCount + 1);

            // The waiting order list still has three orders
            Assert.AreEqual(waitingOrderNum, orderListManager.GetWaitingOrderSOList().Count);
        });
    }

}
