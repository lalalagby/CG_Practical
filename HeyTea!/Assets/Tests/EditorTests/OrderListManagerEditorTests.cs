using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using static IKichenwareObejct;


public class OrderListManagerEditorTests{
    public static OrderListManager Instance { get; private set; }
    private OrderListManager orderListManager;
    private CupObject cupObject;
    private RecipeListSO recipeListSO;

    [SetUp]
    public void Setup() {
    // 创建OrderListManager对象
        orderListManager = new GameObject().AddComponent<OrderListManager>();

        // 创建RecipeListSO
        recipeListSO = ScriptableObject.CreateInstance<RecipeListSO>();
        recipeListSO.recipeSOList = new List<RecipeSO>();

        // 创建CupObject对象
        var cupPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Prefabs/HeyTeaObjects/Cup.prefab");
        Transform cupInstance = Transform.Instantiate(cupPrefab);
        cupObject = cupInstance.GetComponent<CupObject>();

        // 设置OrderListManager的RecipeListSO
        orderListManager.SetRecipeListSO(recipeListSO);
    }

    // 生成一个真正的订单
    private RecipeSO CreateCorrectRecipeSO(int type) {
        HeyTeaObjectSO heyTeaObjectSO;
        RecipeSO recipeSO = ScriptableObject.CreateInstance<RecipeSO>();
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
        
        recipeSO.heyTeaObjectSOLists = heyTeaObjectSOList;
        return recipeSO;
    }

    private CupObject CreateCorrectRecipe(int type) {
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
    private CupObject CreateWrongRecipe(int type) {
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
    public void DeliverIncorrectRecipe_OrderListUnchanged(int type) {
        // 生成Waiting订单
        RecipeSO correctRecipe1 = CreateCorrectRecipeSO(1);
        RecipeSO correctRecipe2 = CreateCorrectRecipeSO(2);
        RecipeSO correctRecipe3 = CreateCorrectRecipeSO(3);
        recipeListSO.recipeSOList.Add(correctRecipe1);
        recipeListSO.recipeSOList.Add(correctRecipe2);
        recipeListSO.recipeSOList.Add(correctRecipe3);
        orderListManager.SetWaitingRecipeSOList(new List<RecipeSO>());
        orderListManager.GetWaitingRecipeSOList().Add(correctRecipe1);
        orderListManager.SetOrdersGenerated();
        orderListManager.GetWaitingRecipeSOList().Add(correctRecipe2);
        orderListManager.SetOrdersGenerated();
        orderListManager.GetWaitingRecipeSOList().Add(correctRecipe3);
        orderListManager.SetOrdersGenerated();

        // 生成提交的订单
        cupObject = CreateWrongRecipe(type);

        // 获取当前的waiting订单数
        int waitingRecipeNum = orderListManager.GetWaitingRecipeNum();
        // 初始生成订单数
        int ordersGeneratedNum = orderListManager.GetOrdersGenerated();

        // 交付订单
        orderListManager.DeliverOrder(cupObject);

        // Assert
        Assert.AreEqual(waitingRecipeNum, orderListManager.GetWaitingRecipeNum());
        // 生成订单数不变
        Assert.AreEqual(orderListManager.GetOrdersGenerated(), ordersGeneratedNum);
    }


    // 提交正确的订单，并且当前totalNumOrders小于最大值-1：该订单从订单列表中删除，并在3秒后生成新的订单。
    [Test]
    public void DeliverCorrectRecipe_RemoveOrderAndGenerateNewOrder() {
        // 生成Waiting订单
        RecipeSO correctRecipe1 = CreateCorrectRecipeSO(1);
        RecipeSO correctRecipe2 = CreateCorrectRecipeSO(2);
        RecipeSO correctRecipe3 = CreateCorrectRecipeSO(3);
        recipeListSO.recipeSOList.Add(correctRecipe1);
        recipeListSO.recipeSOList.Add(correctRecipe2);
        recipeListSO.recipeSOList.Add(correctRecipe3);
        orderListManager.SetWaitingRecipeSOList(new List<RecipeSO>());
        orderListManager.GetWaitingRecipeSOList().Add(correctRecipe1);
        orderListManager.SetOrdersGenerated();
        orderListManager.GetWaitingRecipeSOList().Add(correctRecipe2);
        orderListManager.SetOrdersGenerated();
        orderListManager.GetWaitingRecipeSOList().Add(correctRecipe3);
        orderListManager.SetOrdersGenerated();

        // 生成提交的订单
        cupObject = CreateCorrectRecipe(2);
        // 获取当前的waiting订单数
        int waitingRecipeNum = orderListManager.GetWaitingRecipeNum();
        // 初始生成订单数
        int ordersGeneratedNum = orderListManager.GetOrdersGenerated();

        // 交付订单
        orderListManager.DeliverOrder(cupObject);

        // 此时订单被删除，并且当前生成的订单数小于最大值-1
        Assert.Less(orderListManager.GetWaitingRecipeNum(), 4);
        // 生成订单+1
        Assert.AreEqual(orderListManager.GetOrdersGenerated(), ordersGeneratedNum + 1);
        // waiting依旧有三个订单
        Assert.AreEqual(waitingRecipeNum, orderListManager.GetWaitingRecipeNum());
    }


    // 提交正确的订单：删除订单列表的该订单，如果totalNumOrders等于于8，则不再产生新订单。
    public void DeliverCorrectRecipe_RemoveOrderAndCannotGenerateNewOrder() {
        // 生成Waiting订单
        RecipeSO correctRecipe1 = CreateCorrectRecipeSO(1);
        RecipeSO correctRecipe2 = CreateCorrectRecipeSO(2);
        RecipeSO correctRecipe3 = CreateCorrectRecipeSO(3);
        recipeListSO.recipeSOList.Add(correctRecipe1);
        recipeListSO.recipeSOList.Add(correctRecipe2);
        recipeListSO.recipeSOList.Add(correctRecipe3);
        orderListManager.SetWaitingRecipeSOList(new List<RecipeSO>());
        orderListManager.GetWaitingRecipeSOList().Add(correctRecipe1);
        orderListManager.SetOrdersGenerated();
        orderListManager.GetWaitingRecipeSOList().Add(correctRecipe2);
        orderListManager.SetOrdersGenerated();
        orderListManager.GetWaitingRecipeSOList().Add(correctRecipe3);
        orderListManager.SetOrdersGenerated();

        // 生成提交的订单
        cupObject = CreateCorrectRecipe(1);

        // 获取当前的waiting订单数
        int waitingRecipeNum = orderListManager.GetWaitingRecipeNum();
        // 初始生成订单数
        int ordersGeneratedNum = orderListManager.GetOrdersGenerated();

        // 交付订单
        orderListManager.DeliverOrder(cupObject);

        // 此时订单被删除，并且当前已经生成了最大值-1个订单数
        Assert.IsFalse(orderListManager.GetWaitingRecipeSOList().Contains(correctRecipe1));
        Assert.Less(orderListManager.GetWaitingRecipeNum(), 4);

        // 此时waiting的订单数减少了1.
        Assert.AreEqual(waitingRecipeNum-1, orderListManager.GetWaitingRecipeNum());
        // 生成订单不变
        Assert.AreEqual(orderListManager.GetOrdersGenerated(), ordersGeneratedNum);
    }


}
